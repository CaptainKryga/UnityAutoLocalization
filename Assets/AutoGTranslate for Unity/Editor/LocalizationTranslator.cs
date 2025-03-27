using System;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace AutoGTranslate_for_Unity.Editor
{
	public static class LocalizationTranslator
	{
        public static void TranslateTable(
            StringTableCollection table,
            bool[] targetLanguages,
            int sourceLanguageIndex,
            bool overrideExisting)
        {
            if (table == null || table.SharedData == null || table.StringTables == null)
            {
                Debug.LogError("Invalid table data");
                return;
            }

            Undo.RecordObject(table, "Translate Localization Table");

            StringTable sourceLanguage = table.StringTables[sourceLanguageIndex];

            foreach (SharedTableData.SharedTableEntry entry in table.SharedData.Entries)
            {
                string sourceText = sourceLanguage[entry.Key]?.LocalizedValue;

                if (string.IsNullOrEmpty(sourceText))
                    continue;

                for (int i = 0; i < table.StringTables.Count; i++)
                {
                    if (!ShouldTranslate(targetLanguages, i, sourceLanguageIndex))
                        continue;

                    StringTable targetTable = table.StringTables[i];
                    string existingText = targetTable[entry.Key]?.LocalizedValue;

                    if (!overrideExisting && !string.IsNullOrEmpty(existingText))
                        continue;

                    string translatedText = GetTranslation(
                        sourceLanguage.LocaleIdentifier.Code,
                        targetTable.LocaleIdentifier.Code,
                        sourceText
                    );

                    if (!string.IsNullOrEmpty(translatedText))
                    {
                        if (targetTable[entry.Key] != null)
                        {
                            targetTable[entry.Key].Value = translatedText;
                        }
                        else
                        {
                            targetTable.AddEntry(entry.Id, translatedText);
                        }
                    }

                    //delay from google
                    System.Threading.Thread.Sleep(200);
                }
            }

            // save table from hard
            foreach (var stringTable in table.StringTables)
            {
                EditorUtility.SetDirty(stringTable);
            }

            EditorUtility.SetDirty(table.SharedData);
            EditorUtility.SetDirty(table);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Localization changes saved successfully");
        }

        private static bool ShouldTranslate(bool[] targetLanguages, int currentIndex, int sourceLanguageIndex)
        {
            return currentIndex != sourceLanguageIndex && 
                   currentIndex < targetLanguages.Length && 
                   targetLanguages[currentIndex];
        }

        private static string GetTranslation(string fromLang, string toLang, string text)
        {
            string[] lines = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            List<string> translatedLines = new List<string>();
            
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) 
                {
                    translatedLines.Add("");
                    continue;
                }

                try
                {
                    var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLang}&tl={toLang}&dt=t&q={WebUtility.UrlEncode(line)}";
            
                    using (var webClient = new WebClient { Encoding = System.Text.Encoding.UTF8 })
                    {
                        var response = webClient.DownloadString(url);
                        translatedLines.Add(ParseTranslationResponse(response));
                    }
                }
                catch
                {
                    translatedLines.Add(line); 
                }
            }

            return string.Join("\n", translatedLines);
        }

        private static string ParseTranslationResponse(string jsonResponse)
        {
            try
            {
                // Simple parsing - can be replaced with proper JSON parser if needed
                string[] parts = jsonResponse.Split('\"');
                return parts.Length > 1 ? parts[1] : string.Empty;
            }
            catch
            {
                Debug.LogError("Failed to parse translation response");
                return string.Empty;
            }
        }
	}
}