using System.Net;
using System.Web;
using UnityEditor.Localization;

namespace UnityLocalization
{
	public static class Translate
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
				
				for (int x2 = 0; x2 < table.StringTables.Count; x2++)
				{
					if ((!langs[x2] && x2 != defLang) || x2 == defLang || 
						table.StringTables[defLang][key] == null ||
						table.StringTables[defLang][key].LocalizedValue == "") 
						continue;
					
					string lang = table.StringTables[x2].LocaleIdentifier.Code;
					
					string result = Utils.ParseJson(TranslateWord(table.StringTables[defLang].LocaleIdentifier.Code, 
						lang, table.StringTables[defLang][key].LocalizedValue));
					
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
				return result;
			}
			catch
			{
				return "Error";
			}
		}
	}
}