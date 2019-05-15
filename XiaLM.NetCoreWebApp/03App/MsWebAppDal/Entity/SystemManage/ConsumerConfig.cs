using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.SystemManage
{
    public class ConsumerConfig : BaseEntity
    {
        public Int64 ID { get; set; }
        /// <summary>
        /// 交换机名
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// 交换机标识
        /// </summary>
        public string ExchangeKey { get; set; }
        /// <summary>
        /// 当前系统是否监听当前队列数据进行消费
        /// </summary>
        public int IsListen { get; set; }

        /// <summary>
        /// 同步时是否需要接收确认
        /// </summary>
        public int IsAck { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 是否设置队列过期时间
        /// </summary>
        public int IsTtl { get; set; }
        /// <summary>
        /// 过期时间，0为不设置
        /// </summary>
        public int TtlTime { get; set; }

        /// <summary>
        /// 是否设置优先级
        /// </summary>
        public int IsPriority { get; set; }

        /// <summary>
        /// 优先级大小,0:高，1：中，2：低。3;不设置
        /// </summary>
        public int PrioritySize { get; set; }
        /// <summary>
        /// 选择服务名。DataStorageService:数据入库，SetCharService：字符叠加，Isolator.Ga：隔离器数据同步公安端
        /// </summary>
        public string SelectServiceName { get; set; }

        public override string[] GetKeyValues()
        {
            return new string[] { "ID" };
        }

        public override string[] GetDataBaseAuto()
        {
            return new string[] { "ID" };
        }

        public override string GetTableName()
        {
            return "ConsumerConfig";
        }
    }
}
