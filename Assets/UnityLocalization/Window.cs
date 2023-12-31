using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace UnityLocalization
{
	public class Window : EditorWindow
	{
		//array tables
		private StringTableCollection[] _tables;

		//lang selected bools
		private bool[] _boolLang = new bool[256];
		
		//selected table and default lang
		int _selectedTable, _selectedLang;

		
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
			_tables = GetData.GetAllStringTableCollection();

			//2 update get all string table collections
			if (GUILayout.Button("Update List Localization Table's"))
			{
				Debug.Log("Update");
				_tables = GetData.GetAllStringTableCollection();
			}
			
			//3 dropdown list string table collection
			List<string> options = new List<string>();
			for (int x = 0; x < _tables.Length; x++)
			{
				options.Add(_tables[x].TableCollectionName);
			}
			_selectedTable = EditorGUILayout.Popup("String Table Collection", _selectedTable, options.ToArray());
			
			//4 dropdown default language setup table
			List<string> options2 = new List<string>();
			for (int x = 0; x < _tables[_selectedTable].StringTables.Count; x++)
			{
				string code = _tables[_selectedTable].StringTables[x].LocaleIdentifier.Code.ToUpper();
				options2.Add(code);
			}
			_selectedLang = EditorGUILayout.Popup("Default Language", _selectedLang, options2.ToArray());

			//5 setup selected list bool lang
			// _boolLang = new bool[_stc[_selectedStc].StringTables.Count];
			for (int x = 0; x < _tables[_selectedTable].StringTables.Count; x++)
			{
				string code = _tables[_selectedTable].StringTables[x].LocaleIdentifier.Code.ToUpper();
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
				
				Translate.TranslateTable(_tables[_selectedTable], _boolLang, _selectedLang);
			}
		}
	}
}
