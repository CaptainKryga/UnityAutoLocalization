using System;
using System.Net;
using System.Web;
using UnityEditor.Localization;
using UnityEngine;

namespace UnityLocalization
{
	public class Translate
	{
		public static void TranslateTable(StringTableCollection table, bool[] langs, int defLang)
		{
			// int maxKeys = table.StringTables[0].Values.Count; 
			// Debug.Log(maxKeys);
			
			//SharedData => links
			//StringTables => languages
			for (int x = 0; x < table.SharedData.Entries.Count; x++)
			{
				string key = table.SharedData.Entries[x].Key;
				long keyId = table.SharedData.Entries[x].Id;
				// Debug.Log(key);
				
				for (int x2 = 0; x2 < table.StringTables.Count; x2++)
				{
					if ((!langs[x2] && x2 != defLang) || x2 == defLang || 
						table.StringTables[defLang][key] == null ||
						table.StringTables[defLang][key].LocalizedValue == "") 
						continue;
					
					string lang = table.StringTables[x2].LocaleIdentifier.Code;
					// string text = table.StringTables[x2][key]?.LocalizedValue;

					// Debug.Log(lang + ": " + (text ?? "-"));
					// Debug.Log(table.StringTables[x2].Count);
					
					string result = TranslateWord(table.StringTables[defLang].LocaleIdentifier.Code, 
						lang, table.StringTables[defLang][key].LocalizedValue);
					
					
					if (table.StringTables[x2][key] != null)
					{
						table.StringTables[x2][key].Value = result;
					}
					else
					{
						table.StringTables[x2].AddEntry(keyId, result);
					}
				}
			}
		}

		//88 символов на ссылку-запрос
		//168 символов на слова
		private static string TranslateWord(string fromLang, string toLang, string word)
		{
			var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLang}&tl={toLang}&dt=t&q={HttpUtility.UrlEncode(word)}";
			var webClient = new WebClient
			{
				Encoding = System.Text.Encoding.UTF8
			};
			var result = webClient.DownloadString(url);
			try
			{
				result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
				return result;
			}
			catch
			{
				return "Error";
			}

			return null;
		}
	}
}