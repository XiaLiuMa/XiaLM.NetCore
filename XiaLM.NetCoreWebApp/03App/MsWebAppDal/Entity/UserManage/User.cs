﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.UserManage
{
    public class User : BaseEntity
    {
        /// <summary>
        /// 编号（数据库自动编号）
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Uname { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pword { get; set; }
        /// <summary>
        /// 权限(用逗号隔开)
        /// </summary>
        public string Permissions { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public override string[] GetKeyValues()
        {
            return new string[] { "ID" };
        }

        /// <summary>
        /// 数据库自增字段
        /// </summary>
        /// <returns></returns>
        public override string[] GetDataBaseAuto()
        {
            return new string[] { "ID" };
        }

        /// <summary>
        /// 表名
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "User";
        }
    }
}
