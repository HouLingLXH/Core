
namespace ConfigManagerEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ConfigSource
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string content;

        /// <summary>
        /// 源文件文件名
        /// </summary>
        public string sourceName;

        /// <summary>
        /// 配置文件名
        /// </summary>
        public string configName;

        /// <summary>
        /// 解析出来的矩阵
        /// </summary>
        public string[,] matrix;

        /// <summary>
        /// 行
        /// </summary>
        public int row;

        /// <summary>
        /// 列
        /// </summary>
        public int column;
       
    }
}

