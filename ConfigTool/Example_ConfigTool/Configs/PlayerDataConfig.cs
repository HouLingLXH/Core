using System.Collections.Generic;

            [System.Serializable]
            public class PlayerDataConfig
            {
                /// <summary>
                /// ID
                /// </summary>
                public int id;

                /// <summary>
                /// 名称
                /// </summary>
                public string name;

            
                private static Dictionary<int, PlayerDataConfig> dictionary = new Dictionary<int, PlayerDataConfig>();

                /// <summary>
                /// 通过id获取PlayerDataConfig的实例
                /// </summary>
                /// <param name="id">索引</param>
                /// <returns>PlayerDataConfig的实例</returns>
                public static PlayerDataConfig Get(int id)
                {
                    return dictionary[id];
                }
    
                /// <summary>
                /// 获取字典
                /// </summary>
                /// <returns>字典</returns>
                public static Dictionary<int, PlayerDataConfig> GetDictionary()
                {
                    return dictionary;
                }

                /// <summary>
                /// 获取所有键
                /// </summary>
                /// <returns>所有键</returns>
                public static int[] GetKeys()
                {
                    int count = dictionary.Keys.Count;
                    int[] keys = new int[count];
                    dictionary.Keys.CopyTo(keys,0);
                    return keys;
                }

                /// <summary>
                /// 获取所有实例
                /// </summary>
                /// <returns>所有实例</returns>
                public static PlayerDataConfig[] GetValues()
                {
                    int count = dictionary.Values.Count;
                    PlayerDataConfig[] values = new PlayerDataConfig[count];
                    dictionary.Values.CopyTo(values, 0);
                    return values;
                }
            }
            