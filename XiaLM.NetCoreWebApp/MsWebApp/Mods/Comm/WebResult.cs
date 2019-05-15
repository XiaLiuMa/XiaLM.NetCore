using MsWebApp.Comm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.Mods.Comm
{
    /// <summary>
    /// web请求返回值
    /// </summary>
    public class WebResult
    {
        public Rstate Rstate{ get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public object Rvalue { get; set; } = null;
    }

    /// <summary>
    /// web请求状态
    /// </summary>
    public enum Rstate
    {
        /// <summary>
        /// 未登录
        /// </summary>
        NoLogin = 1001,
        /// <summary>
        /// 请求成功
        /// </summary>
        Success = 1002,
        /// <summary>
        /// 部分失败(用于处理多个数据)
        /// </summary>
        PartFail = 1003,
        /// <summary>
        /// 请求失败
        /// </summary>
        Fail = 1004,
    }
}
