using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.SystemManage
{
    public class QuartzByCron : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 ID { get; set; }
        /// <summary>
        /// 用于生成配置文件需要的guid
        /// </summary>
        public string QGuid { get; set; }
        /// <summary>
        /// 调度器任务名称
        /// </summary>
        public string SchedulerName { get; set; }
        /// <summary>
        /// 运行状态。0：启用，1：启用
        /// </summary>
        public int SchedulerState { get; set; }
        /// <summary>
        /// 发生频率
        /// </summary>
        public string Frequency { get; set; }
        /// <summary>
        /// 优先级，0：高，1：中，2：低，3：不设置
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 任务消息是否需要持久化.0:持久化，1：非持久化
        /// </summary>
        public int Durable { get; set; }
        /// <summary>
        /// 当前任务消息的过期时间。为0表示不设置过期时间。
        /// </summary>
        public int TtlTime { get; set; }
        /// <summary>
        /// 关联DataBaseConfig的ID,这里可赋值为‘1’或‘1,2,4’
        /// </summary>
        public string DataBaseId { get; set; }
        /// <summary>
        /// Quartz表达式
        /// </summary>
        public string cronExpression { get; set; }
        /// <summary>
        /// 运行计划。主要是显示对Quartz表达式的文字说明
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 初始化修改界面用
        /// </summary>
        public string AlertInit { get; set; }
        /// <summary>
        /// 调度服务名,这里输入的是Job表的id
        /// </summary>
        public string ServiceName { get; set; }
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
            return "QuartzByCron";
        }
    }
}
