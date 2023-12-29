using System;
using UnityEditor;
using UnityEngine;

namespace Assets.UnityLocalization
{
	public class AutoTranslate : EditorWindow
	{
		private string myString = "Hello world!";
		private bool groundEnabled;
		private bool meybool = true;
		private float myfloat = 1.25f;
		
		[MenuItem("Window/UnityLocalization")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(AutoTranslate));
		}

		private void OnGUI()
		{
			//лейбл
			GUILayout.Label("Auto Translate", EditorStyles.boldLabel);
			myString = EditorGUILayout.TextField("TF", myString);

			//группа параметров подчиняющиеся булевой переменной
			groundEnabled = EditorGUILayout.BeginToggleGroup("OP", groundEnabled);
			meybool = EditorGUILayout.Toggle("T", meybool);
			myfloat = EditorGUILayout.Slider("S", myfloat, -3, 3);
			EditorGUILayout.EndToggleGroup();
		}
	}
}
