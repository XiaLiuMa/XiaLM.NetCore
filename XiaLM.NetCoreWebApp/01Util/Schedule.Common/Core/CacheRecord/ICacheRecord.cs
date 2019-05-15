using System.Collections.Generic;

// namespaces...
namespace Schedule.Common.Core.CacheRecord
{
    // public interfaces...
    /// <summary>
    /// 缓存记录比较接口.
    /// 主要用于缓存上一次比较的数据。并将此数据做为下一条记录的对比数据
    /// </summary>
    public interface ICacheRecord
    {
        // properties...
        /// <summary>
        /// 字段值比较列。可以指定当前缓存数据在比较时，只需要比较的某几个字段值。如果不指定，则比较所有值。
        /// </summary>
        string[] CompareColumn { get; set; }
        /// <summary>
        /// 缓存记录用于生成键值的字段列。
        /// </summary>
        string[] KeyColumn { get; set; }

        // methods...
        /// <summary>
        /// 清空缓存记录。建议每个任务在获取数据库记录时，定时清空此缓存的记录值，以保证第三方因某原因也无法及时接收到第一次的记录。
        /// </summary>
        void Clear();
        /// <summary>
        /// 获取未比较的记录列表。
        /// </summary>
        /// <returns></returns>
        List<Dictionary<string, object>> GetNotComple();
        /// <summary>
        /// 判断当前信息和缓存中的数据比较是否为新信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool IsNew(Dictionary<string, object> data);
        /// <summary>
        /// 复位。在所有数据比较完后，必须调用此函数以复位。否则得知下一次未比较的记录
        /// </summary>
        void Reset();
    }
}
