namespace ConfigManagerEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ConfigGenerator 
    {
        private static string templete =
           @"using System.Collections.Generic;

            [System.Serializable]
            public class /*ClassName*/
            {
            /*DeclareProperties*/
                private static Dictionary</*IDType*/, /*ClassName*/> dictionary = new Dictionary</*IDType*/, /*ClassName*/>();

                /// <summary>
                /// 通过/*IDField*/获取/*ClassName*/的实例
                /// </summary>
                /// <param name=""/*IDField*/"">索引</param>
                /// <returns>/*ClassName*/的实例</returns>
                public static /*ClassName*/ Get(/*IDType*/ /*IDField*/)
                {
                    return dictionary[/*IDField*/];
                }
    
                /// <summary>
                /// 获取字典
                /// </summary>
                /// <returns>字典</returns>
                public static Dictionary</*IDType*/, /*ClassName*/> GetDictionary()
                {
                    return dictionary;
                }

                /// <summary>
                /// 获取所有键
                /// </summary>
                /// <returns>所有键</returns>
                public static /*IDType*/[] GetKeys()
                {
                    int count = dictionary.Keys.Count;
                    /*IDType*/[] keys = new /*IDType*/[count];
                    dictionary.Keys.CopyTo(keys,0);
                    return keys;
                }

                /// <summary>
                /// 获取所有实例
                /// </summary>
                /// <returns>所有实例</returns>
                public static /*ClassName*/[] GetValues()
                {
                    int count = dictionary.Values.Count;
                    /*ClassName*/[] values = new /*ClassName*/[count];
                    dictionary.Values.CopyTo(values, 0);
                    return values;
                }
            }
            ";
                    private static string templete2 =
            @"    /// <summary>
                /// {0}
                /// </summary>
                public {1} {2};

            ";


        public static void Generate(List<ConfigSource> sources, string outputFolder)
        {
            foreach (ConfigSource src in sources)
            {
                string content = templete;
                string outputPath = outputFolder + "/" + src.configName + ".cs";

                //主键
                string idType = ConfigTools.SourceType2CShaarpType(src.matrix[1, 0]);
                //主键注释
                string idField = src.matrix[2, 0];

                string declareProperties = "";
                for (int x = 0; x < src.column; x++)
                {
                    string coment = src.matrix[0, x];
                    //键
                    string csType = ConfigTools.SourceType2CShaarpType(src.matrix[1, x]);
                    //键 注释
                    string field = src.matrix[2, x];
                    //指定格式替换
                    string decare = string.Format(templete2, coment, csType, field);
                    declareProperties += decare;
                }
                //替换，生成文本
                content = content.Replace("/*ClassName*/", src.configName);
                content = content.Replace("/*DeclareProperties*/", declareProperties);
                content = content.Replace("/*IDType*/", idType);
                content = content.Replace("/*IDField*/", idField);

                //写入
                ConfigTools.WriteFile(outputPath, content);
            }
        }
    }

}
