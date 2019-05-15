using System.IO;
using System.Text;

// namespaces...
namespace Schedule.Common.Util
{
    // public classes...
    /// <summary>
    /// 提供对文件的常用操作
    /// </summary>
    public partial class FileUtil
    {
        // public methods...
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dirPath"></param>
        public static void ClearDirectory(string dirPath)
        {
            var filePaths = Directory.GetFiles(dirPath);
            foreach (string file in filePaths)
            {
                File.Delete(file);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// GetFileNameNoPath 获取不包括路径的文件名
        /// </summary>
        public static string GetFileNameNoPath(string filePath)
        {
            return Path.GetFileName(filePath);
        }
        /// <summary>
        /// 获取目标文件的大小
        /// </summary>
        public static int GetFileSize(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var size = (int)fs.Length;
            fs.Close();

            return size;
        }
        /// <summary>
        /// ReadFileReturnBytes 从文件中读取二进制数据
        /// </summary>
        public static byte[] ReadFileReturnBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var br = new BinaryReader(fs);

            var buff = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return buff;
        }
        /// <summary>
        /// 使用UTF-8格式读取文本文件的内容。如果文件不存在，则返回空
        /// </summary>
        public static string ReadText(string file_path)
        {
            if (!File.Exists(file_path))
            {
                return null;
            }





            return File.ReadAllText(file_path, Encoding.UTF8);
        }
        /// <summary>
        /// WriteBuffToFile 将二进制数据写入文件中
        /// </summary>
        public static void WriteBuffToFile(byte[] buff, string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var bw = new BinaryWriter(fs);

            bw.Write(buff);
            bw.Flush();

            bw.Close();
            fs.Close();
        }
        /// <summary>
        /// WriteBuffToFile 将二进制数据写入文件中
        /// </summary>
        public static void WriteBuffToFile(byte[] buff, int offset, int len, string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var bw = new BinaryWriter(fs);

            bw.Write(buff, offset, len);
            bw.Flush();

            bw.Close();
            fs.Close();
        }
        /// <summary>
        /// 将字符串写入指定文件.如果指定文件或目录不存在，则创建
        /// </summary>
        public static void WriteText(string filePath, string text)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }







            File.WriteAllText(filePath, text);
        }
    }
}
