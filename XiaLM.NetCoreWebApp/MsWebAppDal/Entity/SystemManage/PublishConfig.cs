using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.SystemManage
{
    public class PublishConfig : BaseEntity
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
        /// 交换机类型
        /// </summary>
        public string ExchangeType { get; set; }
        /// <summary>
        /// 是否为默认,0:是，1：否
        /// </summary>
        public int IsDefault { get; set; }
        /// <summary>
        /// 当前交换机对应的任务
        /// </summary>
        public string Jobs { get; set; }
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
            return "PublishConfig";
        }
    }
}
