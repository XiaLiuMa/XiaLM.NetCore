using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebAppDal.Dal.SystemManage;
using MsWebAppDal.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace MsWebApp.WebApi.SystemManage
{
    /// <summary>
    /// 消费者配置
    /// </summary>
    public class ConsumerController : Controller
    {
        private const string ModelName = "消费者管理";
        private ConsumerDal dal = new ConsumerDal();

        /// <summary>
        /// 查询消费者配置数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Consumer/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端消费者配置数据")]
        public ActionResult Query(string query, string page, string rows)
        {
            int count = 0;
            List<ConsumerConfig> list = new List<ConsumerConfig>();
            string str = "";
            if (query == "是")
            {
                str = " or IsListen like '0'  or IsAck like '0' ";
            }
            else if (query == "否") { str = " or IsListen like '1'  or IsAck like '1' "; }
            else if (string.Format("无过期时间").Contains(query))
            {
                str = " or TtlTime like '0'";
            }
            else
            {
                str = "or TtlTime like '" + query + "'";
                switch (query)
                {
                    case "高": { str = " or PrioritySize like '0'"; break; }
                    case "中": { str = " or PrioritySize like '1'"; break; }
                    case "低": { str = " or PrioritySize like '2'"; break; }
                }
                if (string.Format("不设置").Contains(query))
                {
                    str = " or PrioritySize like '3'";
                }
            }
            list = dal.Select(string.Format(" ExchangeName like '%{0}%' or ExchangeKey like '%{0}%' or QueueName like '%{0}%' {1}", query, str));
            if (list == null) { list = new List<ConsumerConfig>(); }
            count = list.Count;
            list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", count);
            dic.Add("rows", list);
            return Json(dic);
        }

        /// <summary>
        /// 添加消费者配置
        /// </summary>
        /// <param name="consumerConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Consumer/Add")]
        [LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端消费者配置")]
        public ActionResult Add(ConsumerConfig consumerConfig)
        {
            if (consumerConfig != null)
            {
                if (!dal.Add(consumerConfig))
                {
                    return Content("添加失败");
                }
            }
            else
            {
                return Content("添加失败");
            }
            return Content("");
        }

        /// <summary>
        /// 修改消费者配置
        /// </summary>
        /// <param name="consumerConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Consumer/Alert")]
        [LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端消费者配置")]
        public ActionResult Alert(ConsumerConfig consumerConfig)
        {
            if (consumerConfig != null)
            {
                if (!dal.Update(consumerConfig))
                {
                    return Content("修改失败");
                }
            }
            else
            {
                return Content("修改失败");
            }
            return Content("");
        }

        /// <summary>
        /// 删除消费者配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Consumer/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端消费者配置")]
        public ActionResult Delete(string id)
        {
            if (!dal.Delete(id))
            {
                return Content("删除失败");
            }
            return Content("");
        }
    }
}
