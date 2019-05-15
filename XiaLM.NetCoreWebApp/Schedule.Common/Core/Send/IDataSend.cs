
// namespaces...
namespace Schedule.Common.Core.Send
{
    // public interfaces...
    /// <summary>
    /// 数据发送接口
    /// </summary>
    public interface IDataSend
    {
        // methods...
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="job">任务名</param>
        /// <returns></returns>
        void Send(byte[] data, string job);
    }
}
