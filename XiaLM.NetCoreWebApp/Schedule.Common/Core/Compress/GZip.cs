using System.IO;
using ICSharpCode.SharpZipLib.GZip;

// namespaces...
namespace Schedule.Common.Core.Compress
{
    // public classes...
    /// <summary>
    /// 提供GZip压缩，解压(GZIP格式压缩可以被JAVA解析)
    /// </summary>
    public class GZip : ICompress
    {
        // const fields...
        /// <summary>
        /// 块大小
        /// </summary>
        private const int BUFFER_SIZE = 16 * 1024;

        // public methods...
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public byte[] Compress(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var zos = new GZipOutputStream(ms))
                {
                    zos.Write(data, 0, data.Length);
                    zos.Flush();
                    zos.Finish();
                }
                return ms.ToArray();
            }
        }
        /// <summary>
        /// GZip解压
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decompress(byte[] data)
        {
            var buffer = new byte[BUFFER_SIZE];
            using (var mos = new MemoryStream())
            {
                using (var mis = new MemoryStream(data))
                {
                    using (var zis = new GZipInputStream(mis))
                    {
                        do
                        {
                            var i = zis.Read(buffer, 0, buffer.Length);
                            if (i <= 0)
                            {
                                break;
                            }
                            mos.Write(buffer, 0, i);
                        }
                        while (true);
                    }
                }
                return mos.ToArray();
            }
        }
    }
}
