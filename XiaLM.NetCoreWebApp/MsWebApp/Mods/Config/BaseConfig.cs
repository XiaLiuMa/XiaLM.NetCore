using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.Mods.Config
{
    public class BaseConfig
    {
        /// <summary>
        /// web服务的主机ip
        /// </summary>
        public string HostIp { get; set; }
        /// <summary>
        ///  web服务的主机port
        /// </summary>
        public int HostPort { get; set; }
        /// <summary>
        ///  sqlite数据库路径
        /// </summary>
        public string SqliteDb { get; set; }
        /// <summary>
        ///  Log数据库路径
        /// </summary>
        public string LogDb { get; set; }
        /// <summary>
        ///  日志保存时间
        /// </summary>
        public int RunLogDays { get; set; }
    }
}
