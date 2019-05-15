using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebAppDal.Dal.LogManage;
using MsWebAppDal.Entity.LogManage;
using System;
using System.Collections.Generic;

namespace MsWebApp.WebApi.LogManage
{
    /// <summary>
    /// 调度日志管理
    /// </summary>
    public class ScheduLogController : Controller
    {
        private const string ModelName = "操作日志管理";
        private ScheduLogDal dal = new ScheduLogDal();

        /// <summary>
        /// 查询任务调度日志
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="jobName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ScheduLog/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "分页查询调度日志")]
        public ActionResult Query(string sTime, string eTime, string jobName, string page, string rows, string sort, string order)
        {
            List<ScheduLog> lst = null;
            int count = 0;
            dal.SelectPageEx(GetCondation(sTime, eTime, jobName), GetOrder(sort, order), Convert.ToInt32(page), Convert.ToInt32(rows), out lst, out count);
            if (lst == null) lst = new List<ScheduLog>();
            Dictionary<string, object> jsonMap = new Dictionary<string, object>();
            jsonMap.Add("total", count);
            jsonMap.Add("rows", lst);
            return Json(jsonMap);
        }

        /// <summary>
        /// 删除任务调度日志
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ScheduLog/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "批量删除调度日志")]
        public ActionResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids.Trim())) return Content("请输入ids");
            string[] idArray = ids.Split(',');
            return Json(dal.Delete(idArray));
        }

        /// <summary>
        /// 清空所有任务调度日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ScheduLog/Clear")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "清空所有调度日志")]
        public ActionResult DeleteAll()
        {
            return Json(dal.DeleteAll());
        }

        /// <summary>
        /// 导出当前页面数据到文件
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="jobName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ScheduLog/ExportPage")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "到处调度日志到文件")]
        public ActionResult ExportPage(string sTime, string eTime, string jobName, string page, string rows)
        {
            string dir = "ScheduLog/";    //导出到该文件夹
            List<ScheduLog> lst = null;
            int count = 0;
            dal.SelectPage(GetCondation(sTime, eTime, jobName), Convert.ToInt32(page), Convert.ToInt32(rows), out lst, out count);
            if (lst == null) lst = new List<ScheduLog>();
            List<string> lines = new List<string>();
            foreach (var item in lst)
            {
                lines.Add($"{item.JobName} {item.StartTime}  {item.EndTime}  {item.ReMark}");
            }
            string strFilePath = $"{dir}/{jobName}_{sTime}-{eTime}.log";
            System.IO.File.WriteAllLines(strFilePath, lines);
            return Json(strFilePath);
        }

        /// <summary>
        /// 导出所有符合查询条件的数据到文件
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ScheduLog/ExportAll")]
        [LogFilter(czmk = ModelName, czlx = OperationType.other, czsm = "导出所有调度日志")]
        public ActionResult ExportAll(string sTime, string eTime, string jobName)
        {
            string dir = "ScheduLog/";    //导出到该文件夹
            List<ScheduLog> lst = dal.Select(GetCondation(sTime, eTime, jobName));
            if (lst == null) lst = new List<ScheduLog>();
            List<string> lines = new List<string>();
            foreach (var item in lst)
            {
                lines.Add($"{item.JobName} {item.StartTime}  {item.EndTime}  {item.ReMark}");
            }
            string strFilePath = $"{dir}/{jobName}_{sTime}-{eTime}.log";
            System.IO.File.WriteAllLines(strFilePath, lines);
            return Json(strFilePath);
        }

        #region 获取查询条件
        private string GetCondation(string startTime, string endTime, string jobName)
        {
            string con = @"format(StartTime,'yyyy-mm-dd HH:mm:ss')  between '{0}' and '{1}'";
            string conn = string.Format(con, startTime, endTime);
            if (!string.IsNullOrEmpty(jobName))
            {
                conn = conn + " and JobName like '%%" + jobName + "%%'";
            }

            return conn;
        }
        #endregion

        #region 获取排序条件
        private string GetOrder(string sort, string order)
        {
            string conn = "order by StartTime desc";

            if (!string.IsNullOrEmpty(sort))
            {
                conn = "order by " + sort + " " + order;
            }

            return conn;
        }
        #endregion
    }
}
