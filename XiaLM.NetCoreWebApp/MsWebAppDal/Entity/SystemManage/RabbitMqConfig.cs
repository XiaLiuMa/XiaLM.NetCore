using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.SystemManage
{
    public class RabbitMqConfig : BaseEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string Vhost { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Use { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 连接池大小
        /// </summary>
        public int ConnPool { get; set; }

        /// <summary>
        /// 每个通道连接池的大小
        /// </summary>
        public int ChannelPool { get; set; }

        /// <summary>
        /// 当前所有生产者配置，PublishConfig的id集合
        /// </summary>
        public string Publishs { get; set; }

        /// <summary>
        /// 当前消费者配置。ConsumerConfig的id集合
        /// </summary>
        public string Consumers { get; set; }

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
            return "RMQConnConfig";
        }
    }
}
