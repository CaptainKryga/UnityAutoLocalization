using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace CaptainKryga.Localization
{
    [ExecuteAlways]
    public class Translate : MonoBehaviour
    {
        [SerializeField] private List<StringTableCollection> _collection;
        // Start is called before the first frame update
        void Start()
        {
            //SharedData
            //for (_collection.Entries)
            //int id
            //
            //StringTables
            //
            // GetDirectory(new DirectoryInfo(Application.dataPath));
        }

        public bool test;
        public bool test2;
        public string path;
        private void Update()
        {
            if (test)
            {
                test = false;

                StringTableCollection temp = AssetDatabase.LoadAssetAtPath<StringTableCollection>(path);
                StringTableCollection temp2 = (StringTableCollection)AssetDatabase.LoadMainAssetAtPath(path);
                StringTableCollection temp3 = (StringTableCollection)AssetDatabase.LoadAssetAtPath(path, typeof(StringTableCollection));

            
                AssetDatabase.ImportAsset(path);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(StringTableCollection));

                StringTableCollection temp5 = AssetDatabase.LoadAssetAtPath(path, (typeof(StringTableCollection))) as StringTableCollection;

            
                Debug.Log(temp);
                Debug.Log(temp2);
                Debug.Log(temp3);
                Debug.Log(asset);
                Debug.Log(temp5);
            }
        
            if (test2)
            {
                test2 = false;
                Debug.Log(Application.dataPath);
                
                _collection.Clear();
                GetDirectory(new DirectoryInfo(Application.dataPath));

                foreach (var temp in _collection)
                {
                    Debug.Log(temp);
                }
            }
        }

        private void GetDirectory(DirectoryInfo dir)
        {
            DirectoryInfo[] directories = dir.GetDirectories();
            foreach (var directory in directories)
            {
                GetDirectory(directory);
            }
        
            FileInfo[] info = dir.GetFiles("*.asset");
            foreach (FileInfo f in info) 
            {
                // Debug.Log(f.FullName);

                string t = f.FullName.Replace(Application.dataPath, "Assets");

                StringTableCollection temp = AssetDatabase.LoadAssetAtPath<StringTableCollection>(t);
                
                if (temp)
                    _collection.Add(temp);
            }
        }
    }
}
