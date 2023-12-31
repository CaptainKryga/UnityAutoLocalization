using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace UnityLocalization
{
	public class Window : EditorWindow
	{
		private StringTableCollection[] _stc;

		private GUIContent _content;

		private bool[] _boolLang = new bool[256];
		
		int _selectedStc, _selectedLang;
		private bool _showBtn;
		private string _defaultLang;
		
		[MenuItem("Window/UnityLocalization")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(Window));
		}
		
		//dropdown tables
		//dropdown default language
		//булки/список языков которые подлежат обновлению
		//булка перезаписи
		//список доступных таблиц в проекте
		
		private void OnGUI()
		{
			//1 лейбл
			GUILayout.Label("Auto Translate", EditorStyles.boldLabel);
			_stc = GetData.GetAllStringTableCollection();

			//2 update get all string table collections
			if (GUILayout.Button("Update List Localization Table's"))
			{
				Debug.Log("Update");
				_stc = GetData.GetAllStringTableCollection();
			}
			
			//3 dropdown list string table collection
			List<string> options = new List<string>();
			for (int x = 0; x < _stc.Length; x++)
			{
				options.Add(_stc[x].TableCollectionName);
			}
			_selectedStc = EditorGUILayout.Popup("String Table Collection", _selectedStc, options.ToArray());
			
			//4 dropdown default language setup table
			List<string> options2 = new List<string>();
			for (int x = 0; x < _stc[_selectedStc].StringTables.Count; x++)
			{
				string code = _stc[_selectedStc].StringTables[x].LocaleIdentifier.Code.ToUpper();
				options2.Add(code);
			}
			_selectedLang = EditorGUILayout.Popup("Default Language", _selectedLang, options2.ToArray());

			//5 setup selected list bool lang
			// _boolLang = new bool[_stc[_selectedStc].StringTables.Count];
			for (int x = 0; x < _stc[_selectedStc].StringTables.Count; x++)
			{
				string code = _stc[_selectedStc].StringTables[x].LocaleIdentifier.Code.ToUpper();
				if (x == _selectedLang)
				{
					GUILayout.Label(code, EditorStyles.boldLabel);
				}
				else
				{
					_boolLang[x] = EditorGUILayout.Toggle(code, _boolLang[x]);
				}
			}
			
			//6 translate
			if (GUILayout.Button("Translate"))
			{
				Debug.Log("Translate");
			}
		}
	}
}
