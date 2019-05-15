using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebAppDal.Dal.LogManage;
using MsWebAppDal.Entity.LogManage;
using System;
using System.Collections.Generic;

namespace MsWebApp.WebApi.LogManage
{
    /// <summary>
    /// 操作日志管理
    /// </summary>
    public class OperationLogController : Controller
    {
        private const string ModelName = "操作日志管理";
        private OperationLogDal dal = new OperationLogDal();

        /// <summary>
        /// 获取操作类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("OperationLog/GetOperationTypes")]
        public ActionResult GetOperationTypes()
        {
            List<string> types = new List<string>()
            {
                "全部",
                "Query",
                "Add",
                "Alert",
                "Delete"
            };
            return Json(types);
        }

        /// <summary>
        /// 分页查询操作日志
        /// </summary>
        /// <param name="cType"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperationLog/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "分页查询操作日志")]
        public ActionResult Query(string cType, string sTime, string eTime,  string page, string rows)
        {
            List<OperationLog> lst = null;
            int count = 0;
            dal.SelectPage(GetCondation(sTime, eTime, cType), Convert.ToInt32(page), Convert.ToInt32(rows), out lst, out count);
            if (lst == null) lst = new List<OperationLog>();
            Dictionary<string, object> jsonMap = new Dictionary<string, object>();
            jsonMap.Add("total", count);
            jsonMap.Add("rows", lst);
            return Json(jsonMap);
        }

        #region 批量删除
        [HttpPost]
        [Route("OperationLog/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "批量删除操作日志")]
        public ActionResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids.Trim())) return Content("请输入ids");
            string[] idArray = ids.Split(',');
            return Json(dal.Delete(idArray));
        }
        #endregion

        /// <summary>
        /// 清除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("OperationLog/Clear")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "清空所有操作日志")]
        public ActionResult Clear()
        {
            return Json(dal.DeleteAll());
        }

        /// <summary>
        /// 导出当前页面日志数据到文件
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="czlx"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperationLog/ExportPage")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "导出操作日志到文件")]
        public ActionResult ExportPage(string sTime, string eTime, string czlx, string page, string rows)
        {
            string dir = "OperationLog/";    //导出到该文件夹
            List<OperationLog> lst = null;
            int count = 0;
            dal.SelectPage(GetCondation(sTime, eTime, czlx), Convert.ToInt32(page), Convert.ToInt32(rows), out lst, out count);
            if (lst == null) lst = new List<OperationLog>();
            List<string> lines = new List<string>();
            foreach (var item in lst)
            {
                lines.Add($"{item.czr} {item.czmk}  {item.czlx}  {item.czsj}  {item.czsm}");
            }
            string strFilePath = $"{dir}/{czlx}_{sTime}-{eTime}.log";
            System.IO.File.WriteAllLines(strFilePath, lines);
            return Json(strFilePath);
        }

        /// <summary>
        /// 导出所有符合查询条件的数据到文件
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="czlx"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperationLog/ExportAll")]
        [LogFilter(czmk = ModelName, czlx = OperationType.other, czsm = "导出所有操作日志")]
        public ActionResult ExportAll(string sTime, string eTime, string czlx)
        {
            string dir = "OperationLog/";    //导出到该文件夹
            List<OperationLog> lst = dal.Select(GetCondation(sTime, eTime, czlx));
            if (lst == null) lst = new List<OperationLog>();
            List<string> lines = new List<string>();
            foreach (var item in lst)
            {
                lines.Add($"{item.czr} {item.czmk}  {item.czlx}  {item.czsj}  {item.czsm}");
            }
            string strFilePath = $"{dir}/{czlx}_{sTime}-{eTime}.log";
            System.IO.File.WriteAllLines(strFilePath, lines);
            return Json(strFilePath);
        }

        #region 获取查询条件
        private string GetCondation(string startTime, string endTime, string czlx)
        {
            string con = @"format(czsj,'yyyy-mm-dd HH:mm:ss')  between '{0}' and '{1}'";
            string conn = string.Format(con, startTime, endTime);
            if (czlx != "qb")
            {
                conn = conn + " and czlx='" + czlx + "'";
            }
            return conn;
        }
        #endregion

    }
}
