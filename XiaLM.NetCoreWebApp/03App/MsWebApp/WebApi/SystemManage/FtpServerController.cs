using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebApp.Mods.Comm;
using MsWebAppDal.Dal.SystemManage;
using MsWebAppDal.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MsWebApp.WebApi.SystemManage
{
    /// <summary>
    /// Ftp服务器配置
    /// </summary>
    public class FtpServerController : Controller
    {
        private const string ModelName = "Ftp服务器管理";
        private FtpServerDal dal = new FtpServerDal();

        /// <summary>
        /// 查询Ftp服务器配置数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("FtpServer/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端Ftp服务器配置数据")]
        public ActionResult Query(string query, string page, string rows)
        {
            int count = 0;
            List<FtpServerConfig> list = new List<FtpServerConfig>();
            var str = "";
            if (query == "是")
            {
                str = "or IsDefault like '0'";
            }
            if (query == "否")
            {
                str = "or IsDefault like '1'";
            }
            list = dal.Select($" ExchangeName like '%{query}%' or ExchangeKey like '%{query}%' or ExchangeType like '%{query}%' {str} ");
            if (list == null) { list = new List<FtpServerConfig>(); }
            count = list.Count;
            list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", count);
            dic.Add("rows", list);
            return Json(dic);
        }

        /// <summary>
        /// 新增Ftp服务器配置数据
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FtpServer/Add")]
        [LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端Ftp服务器配置数据")]
        public ActionResult Add(string ip, int port, string uname, string pwd)
        {
            #region 输入检查
            if (string.IsNullOrEmpty(ip.Trim())) return Content("请输入ip");
            if (string.IsNullOrEmpty(pwd.Trim())) return Content("请输入pwd");
            if (string.IsNullOrEmpty(uname.Trim())) return Content("请输入uname");
            #endregion

            FtpServerConfig ftpServer = new FtpServerConfig()
            {
                FGuid = Guid.NewGuid().ToString("D"),
                Ip = ip,
                Port = port,
                Uname = uname,
                Pwd = pwd
            };
            if (!dal.Add(ftpServer))
            {
                return Content("添加失败");
            }
            return Content("");
        }

        /// <summary>
        /// 修改Ftp服务器配置数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fguid"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FtpServer/Alert")]
        [LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端Ftp服务器配置数据")]
        public ActionResult Alert(int id, string fguid, string ip, int port, string uname, string pwd)
        {
            #region 输入检查
            if (string.IsNullOrEmpty(fguid.Trim())) return Content("请输入fguid");
            if (string.IsNullOrEmpty(ip.Trim())) return Content("请输入ip");
            if (string.IsNullOrEmpty(pwd.Trim())) return Content("请输入pwd");
            if (string.IsNullOrEmpty(uname.Trim())) return Content("请输入uname");
            #endregion
            FtpServerConfig ftpServer = new FtpServerConfig()
            {
                ID = id,
                FGuid = fguid,
                Ip = ip,
                Port = port,
                Uname = uname,
                Pwd = pwd
            };
            if (!dal.Update(ftpServer))
            {
                return Content("修改失败");
            }
            return Content("");
        }

        /// <summary>
        /// 删除Ftp服务器配置数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FtpServer/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端Ftp服务器配置数据")]
        public ActionResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids.Trim())) return Content("请输入ids");
            string[] idArray = ids.Split(',');

            if (idArray != null && idArray.Length > 0)
            {
                return Content(dal.Delete(idArray).ToString());
            }
            return Content("");
        }

        /// <summary>
        /// 清除Ftp服务器配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FtpServer/Clear")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "清除Ms端Ftp服务器配置数据")]
        public WebResult Clear()
        {
            return new WebResult()
            {
                Rstate = dal.DeleteAll() ? Rstate.Success : Rstate.Fail
            };
        }
    }
}
