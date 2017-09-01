
namespace ConfigManagerEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SerializableSetGenerator 
    {
        private const string templete =
            @"[System.Serializable]
            public class SerializableSet : UnityEngine.ScriptableObject
            {
            /*DeclareConfigs*/
            }
            ";
        private const string templete2 =
            @"    public /*ConfigName*/[] /*SourceName*/s;
            ";

        public static void Generate(List<ConfigSource> sources, string outputFolder)
        {
            string outputPath = outputFolder + "/SerializableSet.cs";
            string content = templete;

            string declareConfigs = "";
            foreach (ConfigSource src in sources)
            {
                string declare = templete2;
                declare = declare.Replace("/*ConfigName*/", src.configName);
                declare = declare.Replace("/*SourceName*/", src.sourceName);
                declareConfigs += declare;
            }

            content = content.Replace("/*DeclareConfigs*/", declareConfigs);

            ConfigTools.WriteFile(outputPath, content);
        }
    }
}

