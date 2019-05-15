using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MsWebAppDal.Dal.LogManage;
using MsWebAppDal.Entity.LogManage;
using System;
using System.ComponentModel;
using System.Linq;

namespace MsWebApp.Comm
{
    /// <summary>
    /// 操作日志拦截器
    /// </summary>
    public class LogFilter : Attribute, IActionFilter
    {
        /// <summary>
        /// 操作模块
        /// </summary>
        public string czmk { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType czlx { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string czsm { get; set; }

        /// <summary>
        /// Action执行前
        /// </summary>
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

        /// <summary>
        /// Action执行后
        /// </summary>
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            OperationLogDal dal = new OperationLogDal();
            OperationLog model = new OperationLog();

            model.czmk = czmk;
            switch (czlx)
            {
                case OperationType.add: model.czlx = "add"; break;
                case OperationType.edit: model.czlx = "edit"; break;
                case OperationType.delete: model.czlx = "delete"; break;
                case OperationType.search: model.czlx = "search"; break;
                default: model.czlx = "other"; break;
            }
            model.czsj = DateTime.Now;
            model.czr = "admin";
            model.czip = GetClientUserIp(filterContext.HttpContext);
            model.czsm = czsm;
            dal.Add(model);
        }

        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetClientUserIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }

    public enum OperationType
    {
        [Description("添加")]
        add,
        [Description("修改")]
        edit,
        [Description("删除")]
        delete,
        [Description("查询")]
        search,
        [Description("其他")]
        other
    }
}
