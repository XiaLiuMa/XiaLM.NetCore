using Base.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebApp.Dal;
using MsWebAppDal.Entity.LogManage;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MsWebApp.WebApi.LogManage
{
    /// <summary>
    /// 常规日志管理
    /// </summary>
    public class ConventionLogController : Controller
    {
        private const string ModelName = "常规日志管理";

        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ConventionLog/GetServices")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "获取服务列表")]
        public ActionResult GetServices()
        {
            List<string> names = ConfigHandler.GetInstance().processConfigs.Select(p => p.LogTable).ToList();
            names.Add("MsWebApp");  //加入webapp程序日志的管理
            return Json(names);
        }

        /// <summary>
        /// 获取日志级别列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ConventionLog/GetLevels")]
        public ActionResult GetLevels()
        {
            List<string> levels = new List<string>()
            {
                "全部",
                "Debug",
                "Info",
                "Warn",
                "Error",
                "Fatal"
            };
            return Json(levels);
        }

        /// <summary>
        /// 查询常规日志
        /// </summary>
        /// <param name="sService"></param>
        /// <param name="sLevel"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConventionLog/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "分页查询Ms端常规日志")]
        public ActionResult Query(string sService, string sLevel, string sTime, string eTime, string page, string rows)
        {
            string sql = $"select * from {sService} where Level like '{sLevel}' Timestamp between '{sTime}' and '{eTime}' order by ID limit {rows} offset {rows}*{page} ";  //分页查询
            DataSet ds = new DataSet();
            SqliteLogOperater.RunSQL(sql, ref ds);
            DataSetUtil<ConventionLog> util = new DataSetUtil<ConventionLog>();
            List<ConventionLog> lst = util.DataSetToClassList(ds);
            if (lst == null) lst = new List<ConventionLog>();
            Dictionary<string, object> jsonMap = new Dictionary<string, object>();
            jsonMap.Add("total", lst.Count);
            jsonMap.Add("rows", lst);
            return Json(jsonMap);
        }

        /// <summary>
        /// 删除常规日志
        /// </summary>
        /// <param name="sService"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConventionLog/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "批量删除操作日志")]
        public ActionResult Delete(string sService, string ids)
        {
            if (string.IsNullOrEmpty(ids.Trim())) return Content("请输入ids");
            string[] idArray = ids.Split(',');
            string idsstr = "";
            foreach (var item in idArray)
            {
                idsstr += item + ",";
            }
            idsstr = idsstr.Substring(0, idsstr.Length - 1);    //去除最后一个逗号
            string sql = $"delete from {sService} where ID in ({idsstr})";
            bool flag = SqliteLogOperater.RunSQL(sql) == -1 ? false : true;
            return Json(flag);
        }

        /// <summary>
        /// 清空常规日志表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ConventionLog/Clear")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "清空所有操作日志")]
        public ActionResult Clear(string sService)
        {
            string sql = $"TRUNCATE TABLE {sService}";  //清空表
            bool flag = SqliteLogOperater.RunSQL(sql) == -1 ? false : true;
            return Json(flag);
        }

        /// <summary>
        /// 导出当前页面日志数据到文件
        /// </summary>
        /// <param name="sService"></param>
        /// <param name="sLevel"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConventionLog/ExportPage")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "导出操作日志到文件")]
        public ActionResult ExportPage(string sService, string sLevel, string sTime, string eTime, string page, string rows)
        {
            string dir = "ConventionLog/";    //导出到该文件夹
            string sql = $"select * from {sService} where Level like '{sLevel}' Timestamp between '{sTime}' and '{eTime}' order by ID limit {rows} offset {rows}*{page} ";  //分页查询
            DataSet ds = new DataSet();
            SqliteLogOperater.RunSQL(sql, ref ds);
            DataSetUtil<ConventionLog> util = new DataSetUtil<ConventionLog>();
            List<ConventionLog> lst = util.DataSetToClassList(ds);
            List<string> lines = new List<string>();
            foreach (var item in lst)
            {
                lines.Add($"{item.Timestamp}  {item.Level}  {item.Message}  {item.Action}  {item.Amount}  {item.StackTrace}");
            }
            string strFilePath = $"{dir}/{sService}_{sLevel}_{sTime}-{eTime}.log";
            System.IO.File.WriteAllLines(strFilePath, lines);
            return Json(strFilePath);
        }

        /// <summary>
        /// 导出所有符合查询条件的数据到文件
        /// </summary>
        /// <param name="sService"></param>
        /// <param name="sLevel"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConventionLog/ExportAll")]
        [LogFilter(czmk = ModelName, czlx = OperationType.other, czsm = "导出所有符合条件的常规日志")]
        public ActionResult ExportAll(string sService, string sLevel, string sTime, string eTime)
        {
            string dir = "ConventionLog/";    //导出到该文件夹
            string sql = $"select * from {sService} where Level like '{sLevel}' Timestamp between '{sTime}' and '{eTime}'";//查询
            DataSet ds = new DataSet();
            SqliteLogOperater.RunSQL(sql, ref ds);
            DataSetUtil<ConventionLog> util = new DataSetUtil<ConventionLog>();
            List<ConventionLog> lst = util.DataSetToClassList(ds);
            List<string> lines = new List<string>();
            foreach (var item in lst)
            {
                lines.Add($"{item.Timestamp}  {item.Level}  {item.Message}  {item.Action}  {item.Amount}  {item.StackTrace}");
            }
            string strFilePath = $"{dir}/{sService}_{sLevel}_{sTime}-{eTime}.log";
            System.IO.File.WriteAllLines(strFilePath, lines);
            return Json(strFilePath);
        }

    }
}
