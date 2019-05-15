using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.LogManage
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperationLog : BaseEntity
    {
        /// <summary>
        /// 编号（数据库自动编号）
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 操作模块
        /// </summary>
        public string czmk { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string czlx { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime czsj { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string czr { get; set; }

        /// <summary>
        /// 操作ip
        /// </summary>
        public string czip { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string czsm { get; set; }

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
            return "OperationLog";
        }
    }
}
