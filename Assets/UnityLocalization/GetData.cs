using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace UnityLocalization
{
	public static class GetData
	{
		public static StringTableCollection[] GetAllStringTableCollection()
		{
			List<StringTableCollection> list = new List<StringTableCollection>();
			Recursive(new DirectoryInfo(Application.dataPath), list);
			
			return list.ToArray();
		}
		
		private static void Recursive(DirectoryInfo dir, List<StringTableCollection> list)
		{
			DirectoryInfo[] directories = dir.GetDirectories();
			foreach (var directory in directories)
			{
				Recursive(directory, list);
			}
        
			FileInfo[] files = dir.GetFiles("*.asset");
			foreach (FileInfo file in files) 
			{
				string pathToFile = file.FullName.Replace(Application.dataPath, "Assets");

				StringTableCollection temp = AssetDatabase.LoadAssetAtPath<StringTableCollection>(pathToFile);
				if (temp)
					list.Add(temp);
			}
		}
	}
}