namespace ConfigManagerEditor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class ConfigTools 
    {
        //将文件读取为string
        public static string ReadFile(string path, out byte[] bytes)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fileStream);
            bytes = br.ReadBytes((int)fileStream.Length);
            StreamReader renderer = new StreamReader(fileStream);
            string content = renderer.ReadToEnd();
            fileStream.Close();
            return content;
        }

        //编码转换
        public static Encoding DetectTextEncoding(byte[] b, out String text, int taster = 1000)
        {
            if (b.Length >= 4 && b[0] == 0x00 && b[1] == 0x00 && b[2] == 0xFE && b[3] == 0xFF) { text = Encoding.GetEncoding("utf-32BE").GetString(b, 4, b.Length - 4); return Encoding.GetEncoding("utf-32BE"); }  // UTF-32, big-endian 
            else if (b.Length >= 4 && b[0] == 0xFF && b[1] == 0xFE && b[2] == 0x00 && b[3] == 0x00) { text = Encoding.UTF32.GetString(b, 4, b.Length - 4); return Encoding.UTF32; }    // UTF-32, little-endian
            else if (b.Length >= 2 && b[0] == 0xFE && b[1] == 0xFF) { text = Encoding.BigEndianUnicode.GetString(b, 2, b.Length - 2); return Encoding.BigEndianUnicode; }     // UTF-16, big-endian
            else if (b.Length >= 2 && b[0] == 0xFF && b[1] == 0xFE) { text = Encoding.Unicode.GetString(b, 2, b.Length - 2); return Encoding.Unicode; }              // UTF-16, little-endian
            else if (b.Length >= 3 && b[0] == 0xEF && b[1] == 0xBB && b[2] == 0xBF) { text = Encoding.UTF8.GetString(b, 3, b.Length - 3); return Encoding.UTF8; } // UTF-8
            else if (b.Length >= 3 && b[0] == 0x2b && b[1] == 0x2f && b[2] == 0x76) { text = Encoding.UTF7.GetString(b, 3, b.Length - 3); return Encoding.UTF7; } // UTF-7

            if (taster == 0 || taster > b.Length) taster = b.Length;

            int i = 0;
            bool utf8 = false;
            while (i < taster - 4)
            {
                if (b[i] <= 0x7F) { i += 1; continue; }     // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required' (and therefore the text is more desirable to be treated as the default codepage of the computer). Hence, there's no "utf8 = true;" code unlike the next three checks.
                if (b[i] >= 0xC2 && b[i] <= 0xDF && b[i + 1] >= 0x80 && b[i + 1] < 0xC0) { i += 2; utf8 = true; continue; }
                if (b[i] >= 0xE0 && b[i] <= 0xF0 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0) { i += 3; utf8 = true; continue; }
                if (b[i] >= 0xF0 && b[i] <= 0xF4 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0 && b[i + 3] >= 0x80 && b[i + 3] < 0xC0) { i += 4; utf8 = true; continue; }
                utf8 = false; break;
            }

            if (utf8 == true)
            {
                text = Encoding.UTF8.GetString(b);
                return Encoding.UTF8;
            }


            double threshold = 0.1; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
            int count = 0;
            for (int n = 0; n < taster; n += 2) if (b[n] == 0) count++;
            if (((double)count) / taster > threshold) { text = Encoding.BigEndianUnicode.GetString(b); return Encoding.BigEndianUnicode; }
            count = 0;
            for (int n = 1; n < taster; n += 2) if (b[n] == 0) count++;
            if (((double)count) / taster > threshold) { text = Encoding.Unicode.GetString(b); return Encoding.Unicode; } // (little-endian)


            // Finally, a long shot - let's see if we can find "charset=xyz" or
            // "encoding=xyz" to identify the encoding:
            for (int n = 0; n < taster - 9; n++)
            {
                if (
                    ((b[n + 0] == 'c' || b[n + 0] == 'C') && (b[n + 1] == 'h' || b[n + 1] == 'H') && (b[n + 2] == 'a' || b[n + 2] == 'A') && (b[n + 3] == 'r' || b[n + 3] == 'R') && (b[n + 4] == 's' || b[n + 4] == 'S') && (b[n + 5] == 'e' || b[n + 5] == 'E') && (b[n + 6] == 't' || b[n + 6] == 'T') && (b[n + 7] == '=')) ||
                    ((b[n + 0] == 'e' || b[n + 0] == 'E') && (b[n + 1] == 'n' || b[n + 1] == 'N') && (b[n + 2] == 'c' || b[n + 2] == 'C') && (b[n + 3] == 'o' || b[n + 3] == 'O') && (b[n + 4] == 'd' || b[n + 4] == 'D') && (b[n + 5] == 'i' || b[n + 5] == 'I') && (b[n + 6] == 'n' || b[n + 6] == 'N') && (b[n + 7] == 'g' || b[n + 7] == 'G') && (b[n + 8] == '='))
                    )
                {
                    if (b[n + 0] == 'c' || b[n + 0] == 'C') n += 8; else n += 9;
                    if (b[n] == '"' || b[n] == '\'') n++;
                    int oldn = n;
                    while (n < taster && (b[n] == '_' || b[n] == '-' || (b[n] >= '0' && b[n] <= '9') || (b[n] >= 'a' && b[n] <= 'z') || (b[n] >= 'A' && b[n] <= 'Z')))
                    { n++; }
                    byte[] nb = new byte[n - oldn];
                    Array.Copy(b, oldn, nb, 0, n - oldn);
                    try
                    {
                        string internalEnc = Encoding.ASCII.GetString(nb);
                        text = Encoding.GetEncoding(internalEnc).GetString(b);
                        return Encoding.GetEncoding(internalEnc);
                    }
                    catch { break; }    // If C# doesn't recognize the name of the encoding, break.
                }
            }


            // If all else fails, the encoding is probably (though certainly not
            // definitely) the user's local codepage! One might present to the user a
            // list of alternative encodings as shown here: https://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
            // A full list can be found using Encoding.GetEncodings();
            text = Encoding.Default.GetString(b);
            return Encoding.Default;
        }

        //将配置转换为矩阵
        public static string[,] Content2Matrix(string config, string separator, string lineBreak, out int row, out int col)
        {
            config = config.Trim(); //清空末尾空白

            //分割
            string[] lines = Regex.Split(config, lineBreak);
            string[] firstLine = Regex.Split(lines[0], separator,RegexOptions.Singleline);

            row = lines.Length;
            col = firstLine.Length;
            //Debug.Log(row + "col " + col);
            string[,] matrix = new string[row, col];

            //为第一行赋值
            for (int i = 0; i < col; i++)
            {
                matrix[0, i] = firstLine[i];
            }

            for (int i = 1; i < row; i++)
            {
                string[] line = Regex.Split(lines[i], separator);
                for (int j = 0; j < col; j++)
                {
                    //Debug.Log(line[j]);
                    matrix[i, j] = line[j];
                }
            }
            return matrix;

        }

        //写入文件
        public static void WriteFile(string path, string content)
        {
            FileStream fileStream;
            if (!File.Exists(path))
            {
                fileStream = File.Create(path);
            }
            else
            {
                fileStream = File.Open(path, FileMode.Truncate);
            }
            StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            writer.Write(content);
            writer.Close();

        }

        //将源中的类型 string 转换为 C#类型
        public static string SourceType2CShaarpType(string sourceType)
        {
            string sourceBaseType;
            if (IsArrayType(sourceType, out sourceBaseType))
            {
                return sourceBaseType + "[]";
            }
            return SourceBaseType2CSharpBaseType(sourceType);
        }

        //将 基础类型字符串 转化为C# 基础数据类型
        private static string SourceBaseType2CSharpBaseType(string sourceBaseType)
        {
            string baseType;
            switch (sourceBaseType)
            {
                case "bool":
                    baseType = "bool";
                    break;
                case "byte":
                case "uint8":
                    baseType = "byte";
                    break;
                case "ushort":
                case "uint16":
                    baseType = "ushort";
                    break;
                case "uint":
                case "uint32":
                    baseType = "uint";
                    break;
                case "sbyte":
                case "int8":
                    baseType = "short";
                    break;
                case "int":
                case "in32":
                    baseType = "int";
                    break;
                case "long":
                case "int64":
                    baseType = "long";
                    break;
                case "float":
                    baseType = "float";
                    break;
                case "double":
                    baseType = "double";
                    break;
                case "string":
                    baseType = "string";
                    break;
                default:
                    baseType = sourceBaseType;
                    break;
            }

            return baseType;
        }

        //是否是数组类型
        private static bool IsArrayType(string sourceType, out string sourceBaseType)
        {
            int index = sourceType.LastIndexOf("[");
            if (index != -1)
            {
                sourceBaseType = SourceBaseType2CSharpBaseType(sourceType.Substring(0, index));

            }
            else
            {
                sourceBaseType = sourceType;
            }

            return index != -1;
        }

        #region 将SourceValue转换为Object
        /// <summary>
        /// 解析源值
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="sourceValue"></param>
        /// <returns></returns>
        public static object SourceValue2Object(string sourceType, string sourceValue)
        {
            string sourceBaseType;
            if (IsArrayType(sourceType, out sourceBaseType))
            {
                return SourceValue2ArrayObject(sourceType, sourceValue, sourceBaseType);
            }
            else
            {
                //返回独值
                return SourceValue2BaseObject(sourceType, sourceValue);
            }
        }

        private static object SourceValue2BaseObject(string sourceBaseType, string sourceValue)
        {
            string csharpType = SourceBaseType2CSharpBaseType(sourceBaseType);
            switch (csharpType)
            {
                case "bool":
                    return sourceValue != "0" && sourceValue != "false" && sourceValue != "False" && sourceValue != "FALSE";
                case "byte":
                    return byte.Parse(sourceValue);
                case "ushort":
                    return ushort.Parse(sourceValue);
                case "uint":
                    return uint.Parse(sourceValue);
                case "sbyte":
                    return sbyte.Parse(sourceValue);
                case "short":
                    return short.Parse(sourceValue);
                case "int":
                    return int.Parse(sourceValue);
                case "long":
                    return long.Parse(sourceValue);
                case "ulong":
                    return ulong.Parse(sourceValue);
                case "float":
                    return float.Parse(sourceValue);
                case "double":
                    return double.Parse(sourceValue);
                case "string":
                    //去除CSV的""
                    if (!string.IsNullOrEmpty(sourceValue) && sourceValue[0] == '"')
                    {
                        sourceValue = sourceValue.Remove(0, 1);
                        sourceValue = sourceValue.Remove(sourceValue.Length - 1, 1);
                    }
                    return sourceValue;
                default:
                    return sourceValue;
            }
        }
        #region 将SourceType转换为SystemType
        private static Type SourceBaseType2Type(string sourceBaseType)
        {
            string csharpBaseType = SourceBaseType2CSharpBaseType(sourceBaseType);
            switch (csharpBaseType)
            {
                case "bool":
                    return typeof(bool);
                case "byte":
                    return typeof(byte);
                case "ushort":
                    return typeof(ushort);
                case "uint":
                    return typeof(uint);
                case "sbyte":
                    return typeof(sbyte);
                case "short":
                    return typeof(short);
                case "int":
                    return typeof(int);
                case "long":
                    return typeof(long);
                case "ulong":
                    return typeof(ulong);
                case "float":
                    return typeof(float);
                case "double":
                    return typeof(double);
                case "string":
                    return typeof(string);
                default:
                    return null;
            }
        }
        #endregion

        /// <summary>
        /// 将配置的数组转换为C#数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cValue"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static Array SourceValue2ArrayObject(string sourceType, string sourceValue, string sourceBaseType)
        {
            //解析数组
            if (string.IsNullOrEmpty(sourceValue) || sourceValue == "0") return null;

            //去除CSV的""
            if (sourceValue[0] == '"')
            {
                if (sourceValue.Length <= 2)
                    return null;

                sourceValue = sourceValue.Remove(0, 1);
                sourceValue = sourceValue.Remove(sourceValue.Length - 1, 1);
            }

            string[] values = sourceValue.Split(',');
            Type type = SourceBaseType2Type(sourceBaseType);
            Array array = Array.CreateInstance(type, values.Length);
            for (int i = 0, l = array.Length; i < l; i++)
            {
                object value = SourceValue2BaseObject(sourceBaseType, values[i]);
                array.SetValue(value, i);
            }
            return array;
        }
        #endregion


    }
}


