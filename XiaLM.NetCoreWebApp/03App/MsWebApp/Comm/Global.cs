using MsWebApp.Dal;
using MsWebAppDal.Dal.LogManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MsWebApp.Comm
{
    public static class Global
    {
        public static void Init()
        {
            StartMonitorLog();  //启动日志监控
            //ProcessHandler.GetInstance().Start(ConfigHandler.GetInstance().processConfigs);
        }

        /// <summary>
        /// 启动日志监控(超时的日志删除)
        /// </summary>
        /// <param name="names"></param>
        private static void StartMonitorLog()
        {
            List<string> names = ConfigHandler.GetInstance().processConfigs.Select(p => p.LogTable).ToList();

            Task.Factory.StartNew(() =>
            {
                names.Add("MsWebApp");  //加入webapp程序日志的管理
                foreach (var item in names)
                {
                    string sql = $"delete from {item} where Timestamp<='{DateTime.Now.AddDays(Convert.ToInt32("-" + ConfigHandler.GetInstance().baseConfig.RunLogDays)).ToString("yyyy-mm-dd HH:mm:ss")}'";
                    SqliteLogOperater.RunSQL(sql);  //清理过期日志
                }
                Thread.Sleep(30 * 60 * 1000);//30分钟清理1次
            });
        }

        public static void UnInit()
        {
            ProcessHandler.GetInstance().Cancle();
        }
    }
}
