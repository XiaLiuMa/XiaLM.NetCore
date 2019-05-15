using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.LogManage
{
    /// <summary>
    /// 调度日志
    /// </summary>
    public class ScheduLog : BaseEntity
    {
        /// <summary>
        /// 编号（数据库自动编号）
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// 调度任务名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ReMark { get; set; }


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
            return "RunLog";
        }
    }
}
