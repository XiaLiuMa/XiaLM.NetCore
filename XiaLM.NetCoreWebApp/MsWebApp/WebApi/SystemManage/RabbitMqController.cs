using Base.Common.LogHelp;
using Base.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebApp.Mods.Comm;
using MsWebAppDal.Dal.SystemManage;
using MsWebAppDal.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MsWebApp.WebApi.SystemManage
{
    /// <summary>
    /// RabbitMq配置
    /// </summary>
    public class RabbitMqController : Controller
    {
        private const string ModelName = "RabbitMq配置";
        private RabbitMqDal dal = new RabbitMqDal();

        #region 废弃
        ////消费者列表
        //public JsonResult getConsumers(string serviceName)
        //{
        //    ConsumerConfigBLL ccbll = new ConsumerConfigBLL();
        //    List<ConsumerConfig> list = ccbll.SelectAll();
        //    if (list == null)
        //    {
        //        list = new List<ConsumerConfig>();
        //    }
        //    var lst = (from r in list
        //               where r.SelectServiceName != null
        //               select r).Where(s => s.SelectServiceName.Equals(serviceName)).Select(r =>
        //                      new { r.ID, r.ExchangeName }
        //                     );
        //    return Json(lst, JsonRequestBehavior.AllowGet);
        //}
        ////发布者列表
        //public JsonResult getPublishs()
        //{
        //    BLLPublish bpl = new BLLPublish();
        //    List<PublishConfig> list = bpl.SelectAll();
        //    if (list == null)
        //    {
        //        list = new List<PublishConfig>();
        //    }
        //    var lst = (from r in list
        //               select new
        //               {
        //                   r.ID,
        //                   r.ExchangeName
        //               });
        //    return Json(lst, JsonRequestBehavior.AllowGet);
        //}
        ////得到初始化信息
        //public JsonResult getInit()
        //{
        //    List<RMQConnConfig> list = bll.SelectAll();
        //    if (list == null)
        //    {
        //        list = new List<RMQConnConfig>();
        //    }
        //    RMQConnConfig rm = list.Count == 0 ? null : list[0];
        //    if (rm == null)
        //    {
        //        rm = new RMQConnConfig();
        //    }
        //    return Json(rm, JsonRequestBehavior.AllowGet);
        //}
        ////已添加的Consumer的信息
        //public ActionResult InitAddConsumers()
        //{
        //    var lst = bll.SelectAll();
        //    if (lst == null)
        //    {
        //        return Content("");
        //    }
        //    else
        //    {
        //        if (lst.Count <= 0) { return Content(""); }
        //        else
        //        {
        //            return Content(lst[0].Consumers);
        //        }
        //    }
        //} 
        #endregion

        /// <summary>
        /// 获取可能用到RabbitMQ配置的服务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RabbitMq/GetServiceNames")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "获取需要配置RabbitMq的服务名称列表")]
        public ActionResult GetServiceNames()
        {
            List<string> list = new List<string>();
            foreach (var item in ConfigHandler.GetInstance().processConfigs)
            {
                list.Add(item.Describe);
            }
            return Json(list);
        }

        /// <summary>
        /// 根据服务名称获取RabbitMQ配置数据
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RabbitMq/Get")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "获取RabbitMq配置数据")]
        public ActionResult Get(string serviceName)
        {
            string fPath = $"{DirectoryUtil.GetAppParentDir()}/{serviceName}/config/RabbitMQ.xml";  //配置文件路径
            RabbitMqConfig mqConfig = XmlUtils.Load<RabbitMqConfig>(fPath);

            return Json(mqConfig);
        }

        /// <summary>
        /// 根据服务名称设置RabbitMQ配置数据
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="rmqConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RabbitMq/Set")]
        [LogFilter(czmk = ModelName, czlx = OperationType.other, czsm = "设置RabbitMq配置数据")]
        public ActionResult Set(string serviceName, RabbitMqConfig rmqConfig)
        {
            #region 输入检查
            if (string.IsNullOrEmpty(serviceName)) return Content("未配置服务名称");
            if (rmqConfig == null) return Content("未配置任何MQ数据");
            if (string.IsNullOrEmpty(rmqConfig.Ip)) return Content("未配置Ip");
            if (string.IsNullOrEmpty(rmqConfig.Port)) return Content("未配置Port");
            if (string.IsNullOrEmpty(rmqConfig.Vhost)) return Content("未配置虚拟机");
            if (string.IsNullOrEmpty(rmqConfig.Use)) return Content("未配置用户");
            if (string.IsNullOrEmpty(rmqConfig.Pwd)) return Content("未配置密码");
            if (string.IsNullOrEmpty(rmqConfig.Publishs)) return Content("未配置任何一个发布者");
            if (string.IsNullOrEmpty(rmqConfig.Consumers)) return Content("未配置任何一个消费者");
            #endregion

            string fPath = $"{DirectoryUtil.GetAppParentDir()}/{serviceName}/config/RabbitMQ.xml";  //配置文件路径
            List<QuartzByCron> oldlst = XmlUtils.Load<List<QuartzByCron>>(fPath);   //原始列表
            try
            {
                XmlUtils.Save(rmqConfig, fPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                XmlUtils.Save(oldlst, fPath);
                return Content("保存RabbitMQ失败");
            }
            return Content("");
        }

    }
}
