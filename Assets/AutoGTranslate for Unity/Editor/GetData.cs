using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace UnityLocalization.Editor
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
				string pathToFile = file.FullName;
				pathToFile = "Assets" + pathToFile.Remove(0, Application.dataPath.Length);
				StringTableCollection temp = (StringTableCollection) (AssetDatabase.LoadAssetAtPath(pathToFile,typeof(StringTableCollection)));
				if (temp)
				{
					list.Add(temp);
				}
			}
		}
	}
}