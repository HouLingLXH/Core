
namespace ConfigManagerEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;

    public class ConfigWindow : EditorWindow
    {
        [MenuItem("Window/Config Manager")]
        public static ConfigWindow GetWindow()
        {
            return GetWindow<ConfigWindow>("Config Manager");
        }

        void Awake()
        {
            Debug.Log("========Awake========");
            LoadCache();
            
        }

        public void OnGUI()
        {
            //Base Setting
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            cache.sorceFolder = EditorGUILayout.TextField("Sorce Folder", cache.sorceFolder);
            if (GUILayout.Button("改为" + ConfigMenu.GetSelectedPath()))
            {
                if (EditorUtility.DisplayDialog(
                    "修改 Sorce Folder",
                    "确定修改?",
                    "确定",
                    "取消"
                    ))
                {
                    ConfigMenu.SetSourcePath();
                }
                
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            cache.configOutputFolder = EditorGUILayout.TextField("Config Output", cache.configOutputFolder);
            if (GUILayout.Button("改为" + ConfigMenu.GetSelectedPath()))
            {
                if (EditorUtility.DisplayDialog(
                   "修改 Config Output",
                   "确定修改?",
                   "确定",
                   "取消"
                   ))
                {
                    ConfigMenu.SetConfigOutput();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            cache.assetOutputFolder = EditorGUILayout.TextField("AssetOutput", cache.assetOutputFolder);
            if (GUILayout.Button("改为" + ConfigMenu.GetSelectedPath()))
            {
                if (EditorUtility.DisplayDialog(
                   "修改 AssetOutput",
                   "确定修改?",
                   "确定",
                   "取消"
                   ))
                {
                    ConfigMenu.SetAssetOutput();
                }
            }
            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                SaveCache();
            }

            

            //Operation
            EditorGUILayout.Space();
            GUILayout.Label("Operation", EditorStyles.boldLabel);

            if(GUILayout.Button("Clear Output"))
            {
                if (EditorUtility.DisplayDialog("Clear Output",
                    "Are you want to clear ?" + cache.configOutputFolder + "and" + cache.assetOutputFolder + "/" + assetName,
                    "Yes", "No"))
                {
                    ClearOutput();
                }
            }

            if (GUILayout.Button("Output"))
            {
                Output();
            }

        }

        #region cache
        private const string cacheKey = "ConfigManagerCache"; //缓存数据存储的key
        private const string assetName = "SerializableSet.asset"; //序列化数据名
        private static bool justRecompiled;

        static ConfigWindow()
        {
            justRecompiled = true;
            Debug.Log("ConfigWindow");
        }
        /// <summary>
        /// 缓存数据
        /// </summary>
        public Cache cache;
        //加载cache 信息
        void LoadCache()
        {
            if (PlayerPrefs.HasKey(cacheKey))
            {
                cache = JsonUtility.FromJson<Cache>(PlayerPrefs.GetString(cacheKey));
                Debug.Log(cache.assetOutputFolder);
            }
            else
            {
                cache = new Cache();
            }
        }

        //保存cache 信息
        void SaveCache()
        {
            string json = JsonUtility.ToJson(cache);
            PlayerPrefs.SetString(cacheKey, json);
        }

        #endregion

        #region Output

        void Output()
        {
            //创建输出目录
            if (!Directory.Exists(cache.configOutputFolder))
            {
                Directory.CreateDirectory(cache.configOutputFolder);
            }
            
            //从文件读出的源信息
            List<ConfigSource> configSources = GetSources();

            //生成 配置相对应的 类
            ConfigGenerator.Generate(configSources, cache.configOutputFolder);

            //生产SerializableSet
            SerializableSetGenerator.Generate(configSources, cache.configOutputFolder);

            DeserializerGenerator.Generate(configSources, cache.configOutputFolder);

            AssetDatabase.Refresh();

            if (EditorApplication.isCompiling)
            {
                waitingForSerialize = true;
                Debug.Log("输出完成，正在等待Unity编译后序列化数据...");
            }
            else
            {
                Serialize();
            }
        }

        void ClearOutput()
        {
            //clear config
            if (Directory.Exists(cache.configOutputFolder))
            {
                Directory.Delete(cache.configOutputFolder, true);
                File.Delete(cache.configOutputFolder + ".meta");
            }

            //clear asset
            string assetPath = cache.assetOutputFolder + "/" + assetName;
            if (File.Exists(assetPath))
            {
                File.Delete(assetPath);
                File.Delete(assetPath + ".meta");
            }

            AssetDatabase.Refresh();
        }

        List<ConfigSource> GetSources()
        {
            DirectoryInfo directory = new DirectoryInfo(cache.sorceFolder);
            //要与文件名匹配的搜索字符串。 此参数可以包含有效的文本路径和通配符的组合（* 和 ?） 字符（请参阅备注），但不支持正则表达式。 默认模式为“*”，该模式返回所有文件。
            FileInfo[] files = directory.GetFiles("*.*", SearchOption.AllDirectories);

            string separator; //分隔符
            string lineBreak; //换行符

            List<ConfigSource> configSources = new List<ConfigSource>();
            int filesLength = files.Length;
            for (int i = 0; i < filesLength; i++)
            {
                FileInfo file = files[i];

                //判断文件后缀   CSV是以逗号间隔的文本文件
                if (file.Extension != ".txt" && file.Extension != ".csv" && file.Extension != ".txt")
                    continue;

                ConfigSource source = new ConfigSource();

                string content;
                byte[] bytes;

                ConfigTools.ReadFile(file.FullName, out bytes);
                ConfigTools.DetectTextEncoding(bytes, out content);//转换编码格式

                //报告指定字符在此实例中的第一个匹配项的索引。搜索从指定字符位置开始，并检查指定数量的字符位置。
                if (content.IndexOf("\r\n") != -1)
                {
                    separator = "\t";
                }
                else
                {
                    separator = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
                }

                if (content.IndexOf("\r\n") != -1)
                {
                    lineBreak = "\r\n";
                }
                else
                {
                    lineBreak = "\n";
                }

                source.content = content;
                //Debug.Log("-content---" + content);
                source.sourceName = file.Name.Replace(file.Extension, "");//文件名
                source.configName = source.sourceName + "Config";//类名
                source.matrix = ConfigTools.Content2Matrix(content, separator, lineBreak, out source.row, out source.column);

                configSources.Add(source);
            }
            return configSources;
        }

        /// <summary>
        /// 是否正在等待序列化（新生成脚本需要编译）
        /// </summary>
        private bool waitingForSerialize = false;

        //序列化
        private void Serialize()
        {
            if (!Directory.Exists(cache.assetOutputFolder))
            {
                Directory.CreateDirectory(cache.assetOutputFolder);
            }

            List<ConfigSource> sources = GetSources();

            UnityEngine.Object set = (UnityEngine.Object)Serializer.Serialize(sources) ;
            string o = cache.assetOutputFolder + "/" + assetName;
            AssetDatabase.CreateAsset(set, o);
            Debug.Log("序列化完成");
        }

        void Update()
        {
            if (justRecompiled && waitingForSerialize)
            {
                waitingForSerialize = false;
                Serialize();
            }
            justRecompiled = false;
        }

        #endregion

    }


    [System.Serializable]
    public class Cache
    {
        public string sorceFolder = "Assets/Scripts/Core/ConfigTool/Example/Data";
        public string configOutputFolder = "Assets/Scripts/Core/ConfigTool/Example/Configs"; // 
        public string assetOutputFolder = "Assets/Scripts/Core/ConfigTool/Example";
    }
}
