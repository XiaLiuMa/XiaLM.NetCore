using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.LogManage
{
    /// <summary>
    /// 常规日志
    /// </summary>
    public class ConventionLog : BaseEntity
    {
        /// <summary>
        /// 编号（数据库自动编号）
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 堆栈
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public override string[] GetKeyValues()
        {
            return new string[] { "Id" };
        }

        /// <summary>
        /// 数据库自增字段
        /// </summary>
        /// <returns></returns>
        public override string[] GetDataBaseAuto()
        {
            return new string[] { "Id" };
        }

        /// <summary>
        /// 表名
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "ConventionLog";
        }
    }
}
