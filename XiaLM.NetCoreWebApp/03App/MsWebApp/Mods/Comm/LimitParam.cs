﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.Mods.Comm
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class LimitParam
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 查第几页
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 页显示数据量
        /// </summary>
        public int PageSize { get; set; }
    }
}
