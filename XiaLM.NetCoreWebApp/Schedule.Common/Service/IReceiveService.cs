using Schedule.Common.SqlHelp;

// namespaces...
namespace Schedule.Common.Service
{
    // public interfaces...
    /// <summary>
    /// 数据接收服务接口
    /// </summary>
    public interface IReceiveService
    {
        // properties...
        /// <summary>
        /// 获取当前服务名称。不同的服务名称不能相同
        /// </summary>
        string ServiceName { get; }
        /// <summary>
        /// 获取或设置SQL操作接口
        /// </summary>
        ISqlOperate SqlOperate { get; set; }
    }
}
