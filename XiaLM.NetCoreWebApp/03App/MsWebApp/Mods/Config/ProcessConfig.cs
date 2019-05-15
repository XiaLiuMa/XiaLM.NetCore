using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.Mods.Config
{
    public class ProcessConfig
    {
        /// <summary>
        /// 进程名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Windows系统下的运行路径
        /// </summary>
        public string WindowsPath { get; set; }
        /// <summary>
        /// linux系统下的运行路径
        /// </summary>
        public string LinuxPath { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 日志表名称
        /// </summary>
        public string LogTable { get; set; }
    }
}
