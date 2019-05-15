using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Base.Common.Utils
{
    public class FileUtil
    {
        /// <summary>
        /// 清空文本
        /// </summary>
        /// <param name="txtPath">文本路径</param>
        public static void ClearTxt(string txtPath)
        {
            FileStream stream = File.Open(txtPath, FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();
        }

        /// <summary>
        /// 追加或覆盖文本
        /// </summary>
        /// <param name="txtPath">文本路径</param>
        /// <param name="str">要追加的文本</param>
        /// <param name="saOrAp">false为覆盖，true为追加</param>
        public static void SaveFile(string txtPath, string str, bool saOrAp)
        {
            StreamWriter sw = new StreamWriter(txtPath, saOrAp);//saOrAp表示覆盖或者是追加  
            sw.WriteLine(str);
            sw.Close();
        }


    }
}
