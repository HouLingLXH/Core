
namespace ConfigManagerEditor
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;

    public class ConfigMenu : ScriptableObject
    {
        [MenuItem("Assets/Config Manager/Set to Sorce Path")]
        public static void SetSourcePath()
        {
            ConfigWindow.GetWindow().cache.sorceFolder = GetSelectedPath();
        }

        [MenuItem("Assets/Config Manager/Set to Config Path")]
        public static void SetConfigOutput()
        {
            ConfigWindow.GetWindow().cache.configOutputFolder = GetSelectedPath();
        }

        [MenuItem("Assets/Config Manager/Set to Asset Path")]
        public static void SetAssetOutput()
        {
            ConfigWindow.GetWindow().cache.assetOutputFolder = GetSelectedPath();
        }


        public static string GetSelectedPath()
        {
            string path = "Assets";

            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }

            return path;
        }
    }
}

