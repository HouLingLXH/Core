namespace ConfigManagerEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 用于生成解析器
    /// </summary>
    public class DeserializerGenerator 
    {
        private const string templete =
 @"public class Deserializer
{
    public static void Deserialize(SerializableSet set)
    {
/*SetDictionaries*/
    }
}
";
        private const string templete2 =
@"       
        for (int i = 0, l = set./*SourceName*/s.Length; i < l; i++)
        {
            /*ConfigName*/.GetDictionary().Add(set./*SourceName*/s[i]./*IDField*/, set./*SourceName*/s[i]);
        }
";

        public static void Generate(List<ConfigSource> sources, string outputFolder)
        {
            string outputPath = outputFolder + "/Deserializer.cs";
            string content = templete;

            string setDictionatries = "";
            foreach (ConfigSource src in sources)
            {
                string idField = src.matrix[2, 0];
                string setScript = templete2;

                setScript = setScript.Replace("/*ConfigName*/", src.configName);
                setScript = setScript.Replace("/*SourceName*/", src.sourceName);
                setScript = setScript.Replace("/*IDField*/", idField);

                setDictionatries += setScript;
            }

            content = content.Replace("/*SetDictionaries*/", setDictionatries);
            ConfigTools.WriteFile(outputPath, content);
        }
    }
}

