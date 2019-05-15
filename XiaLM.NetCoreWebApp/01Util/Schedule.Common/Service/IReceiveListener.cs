using Schedule.model;
using System.Threading.Tasks.Dataflow;

// namespaces...
namespace Schedule.Common.Service
{
    // public interfaces...
    /// <summary>
    /// 提供用于第三方继承接口。当隔离器接收到数据时执行此接口的子类
    /// </summary>
    public interface IReceiveListener
    {
        // methods...
        /// <summary>
        /// 数据接收通知方法执行入口
        /// </summary>
        /// <param name="data">接收到的隔离器数据</param>
        void ReceiveNotified(IsolatorData data);

        ActionBlock<IsolatorData> Data_ActionBlock { get; set; }
    }
}
