
// namespaces...
namespace Schedule.Common.Core.Compress
{
    // public interfaces...
    /// <summary>
    /// 压缩，解压接口
    /// </summary>
    public interface ICompress
    {
        // methods...
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        byte[] Compress(byte[] data);
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Decompress(byte[] data);
    }
}
