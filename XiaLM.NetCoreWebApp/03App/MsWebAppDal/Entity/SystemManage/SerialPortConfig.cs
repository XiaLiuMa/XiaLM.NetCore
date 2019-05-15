using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.SystemManage
{
    public class SerialPortConfig : BaseEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 串口名称
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public string BaudRate { get; set; }
        /// <summary>
        /// 是否接收 0：接收，1：不接收
        /// </summary>
        public int IsReceive { get; set; }
        /// <summary>
        /// 是否发送 0：发送，1：不发送
        /// </summary>
        public int IsSend { get; set; }
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
            return "SerialPortConfig";
        }
    }
}
