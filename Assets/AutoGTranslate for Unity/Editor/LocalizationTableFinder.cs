using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace AutoGTranslate_for_Unity.Editor
{
	public static class LocalizationTableFinder
	{
		public static StringTableCollection[] GetAllStringTableCollections()
		{
			List<StringTableCollection> collections = new List<StringTableCollection>();
			ScanProjectForCollections(new DirectoryInfo(Application.dataPath), collections);
			return collections.ToArray();
		}
        
		private static void ScanProjectForCollections(DirectoryInfo directory, List<StringTableCollection> collections)
		{
			foreach (DirectoryInfo subDirectory in directory.GetDirectories())
			{
				ScanProjectForCollections(subDirectory, collections);
			}
        
			foreach (FileInfo file in directory.GetFiles("*.asset"))
			{
				string relativePath = "Assets" + file.FullName.Substring(Application.dataPath.Length);
				StringTableCollection collection = AssetDatabase.LoadAssetAtPath<StringTableCollection>(relativePath);
                
				if (collection != null)
				{
					collections.Add(collection);
				}
			}
		}
	}
}