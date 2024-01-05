using System.Collections.Generic;

namespace UnityLocalization.Editor
{
	public static class Utils
	{
		public static string ParseJson(string source)
		{
			string[] array = source.Split('\"', '\"');
			List<string> temp = new List<string>();
			
			for (int x = 1; x < array.Length; x++)
			{
				if (array[x - 1] == "[[[" || array[x - 1] == "]]]],[")
					temp.Add(array[x]);
			}

			array = temp.ToArray();
			source = "";
			for (int x = 0; x < array.Length; x++)
				source += array[x];

			return source;
		}
	}
}