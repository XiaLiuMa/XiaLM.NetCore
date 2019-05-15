using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebAppDal.Entity.SystemManage
{
    /// <summary>
    /// 数据库配置模型
    /// </summary>
    public class DataBase : BaseEntity
    {
        public string ServiceName { get; set; }
        public string ServerIp { get; set; }
        public string ServerPort { get; set; }
        public string Default { get; set; }
        public Int64 ID { get; set; }
        public string DGuid { get; set; }

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
            return "DataBaseConfig";
        }
    }
}
