using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.UserManage
{
    public class Permissions : BaseEntity
    {
        /// <summary>
        /// 编号（数据库自动编号）
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// guid
        /// </summary>
        public string Pguid { get; set; }
        /// <summary>
        /// 权限名
        /// </summary>
        public string Name { get; set; }
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
            return "Permissions";
        }
    }
}
