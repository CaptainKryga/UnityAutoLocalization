using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace AutoGTranslate_for_Unity.Editor
{
	public class LocalizationTranslatorWindow : EditorWindow
	{
        private StringTableCollection[] _tables;
        private bool[] _languageSelection = new bool[256];
        private int _selectedTableIndex;
        private int _defaultLanguageIndex;
        private bool _shouldOverride;
        
        private List<string> _tableNames = new List<string>();
        private List<string> _languageCodes = new List<string>();

        [MenuItem("Window/AutoGTranslate")]
        public static void ShowWindow()
        {
            GetWindow<LocalizationTranslatorWindow>("AutoGTranslate");
        }

        private void OnGUI()
        {
            DrawTitle();
            DrawRefreshButton();
            
            if (_tables == null || _tables.Length == 0)
                return;

            DrawTableSelection();
            DrawLanguageSelection();
            DrawOverrideToggle();
            DrawLanguageToggles();
            DrawTranslateButton();
        }

        private void DrawTitle()
        {
            GUILayout.Label("AutoGTranslate", EditorStyles.boldLabel);
        }

        private void DrawRefreshButton()
        {
            if (GUILayout.Button("Refresh Localization Tables"))
            {
                RefreshTables();
            }
        }

        private void RefreshTables()
        {
            _tables = LocalizationTableFinder.GetAllStringTableCollections();
            
            _tableNames.Clear();
            foreach (StringTableCollection table in _tables)
            {
                _tableNames.Add(table.TableCollectionName);
            }

            UpdateLanguageList();
        }

        private void UpdateLanguageList()
        {
            _languageCodes.Clear();
            if (_tables.Length == 0) return;
            
            foreach (StringTable table in _tables[_selectedTableIndex].StringTables)
            {
                _languageCodes.Add(table.LocaleIdentifier.Code.ToUpper());
            }
        }

        private void DrawTableSelection()
        {
            _selectedTableIndex = EditorGUILayout.Popup(
                "String Table Collection", 
                _selectedTableIndex, 
                _tableNames.ToArray()
            );
            
            // Update languages when table changes
            if (GUI.changed)
            {
                UpdateLanguageList();
            }
        }

        private void DrawLanguageSelection()
        {
            _defaultLanguageIndex = EditorGUILayout.Popup(
                "Default Language", 
                _defaultLanguageIndex, 
                _languageCodes.ToArray()
            );
        }

        private void DrawOverrideToggle()
        {
            _shouldOverride = EditorGUILayout.Toggle("Override existing translations?", _shouldOverride);
        }

        private void DrawLanguageToggles()
        {
            if (_tables.Length <= _selectedTableIndex) return;
            
            StringTableCollection currentTable = _tables[_selectedTableIndex];
            
            for (int i = 0; i < currentTable.StringTables.Count; i++)
            {
                string code = currentTable.StringTables[i].LocaleIdentifier.Code.ToUpper();
                
                if (i == _defaultLanguageIndex)
                {
                    GUILayout.Label($"{code} (Source)", EditorStyles.boldLabel);
                }
                else
                {
                    _languageSelection[i] = EditorGUILayout.Toggle(
                        $"Translate to {code}", 
                        _languageSelection[i]
                    );
                }
            }
        }

        private void DrawTranslateButton()
        {
            if (GUILayout.Button("Translate", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog(
                    "Confirm Translation", 
                    "This will translate all selected languages. Continue?", 
                    "Yes", "No"))
                {
                    LocalizationTranslator.TranslateTable(
                        _tables[_selectedTableIndex], 
                        _languageSelection, 
                        _defaultLanguageIndex, 
                        _shouldOverride
                    );
                    Debug.Log("Translation completed");
                }
            }
        }
	}
}
