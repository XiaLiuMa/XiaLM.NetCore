using Base.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebApp.Dal;
using MsWebApp.Mods.Comm;
using MsWebAppDal.Dal.SystemManage;
using MsWebAppDal.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;

namespace MsWebApp.WebApi.SystemManage
{
    /// <summary>
    /// 发布者配置
    /// </summary>
    public class PublisherController : Controller
    {
        private const string ModelName = "发布者管理";
        private PublishDal dal = new PublishDal();

        /// <summary>
        /// 获取Job列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Publisher/GetJobs")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "Publisher获取Job列表")]
        public ActionResult GetJobs()
        {
            HttpUtil http = new HttpUtil();
            string str = http.Get("http:127.0.0.1:50231/Scheduler/GetJobs");
            return Content(str);
        }

        /// <summary>
        /// 查询发布者配置数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Publisher/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端发布者配置数据")]
        public ActionResult Query(string query, string page, string rows)
        {
            int count = 0;
            List<PublishConfig> list = new List<PublishConfig>();
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
            if (list == null) { list = new List<PublishConfig>(); }
            count = list.Count;
            list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", count);
            dic.Add("rows", list);
            return Json(dic);
        }

        /// <summary>
        /// 新增发布者配置数据
        /// </summary>
        /// <param name="publishConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Publisher/Add")]
        [LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端发布者配置数据")]
        public ActionResult Add(PublishConfig publishConfig)
        {
            if (publishConfig == null)
            {
                if (!dal.Add(publishConfig))
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
        /// 修改发布者配置数据
        /// </summary>
        /// <param name="publishConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Publisher/Alert")]
        [LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端发布者配置数据")]
        public ActionResult Alert(PublishConfig publishConfig)
        {
            if (publishConfig != null)
            {
                if (!dal.Update(publishConfig))
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
        /// 删除发布者配置数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Publisher/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端发布者配置数据")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return Content("删除失败");
            }
            if (!dal.Delete(id))
            {
                return Content("删除失败");
            }
            return Content("");
        }

        #region 备用
        ///// <summary>
        ///// 获取数据
        ///// </summary>
        ///// <param name="param">分页查询参数</param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("Publisher/GetData")]
        //public WebResult GetData(LimitParam param)
        //{
        //    string where = string.IsNullOrEmpty(param.Condition) ? "" : $"where ExchangeKey='{param.Condition}'";
        //    string sql = $"select* from PublishConfig {where} order by GuestId limit {param.PageSize} offset {param.PageNum * param.PageSize}";
        //    var publishs = SqliteOperater.SelectTable<PublishConfig>(sql);
        //    WebResult result = new WebResult()
        //    {
        //        Rvalue = publishs
        //    };
        //    return result;

        //    #region 废弃
        //    //List<PublishConfig> publishers = new List<PublishConfig>();
        //    //if (string.IsNullOrEmpty(param.Condition))
        //    //{
        //    //    var lst1 = publishers.Skip(param.PageNum * param.PageSize).Take(param.PageSize).ToList();
        //    //    return new WebResult() { Rvalue = lst1 };
        //    //}
        //    //else
        //    //{
        //    //    var lst2 = publishers.Where(p => p.ExchangeKey.Equals(param.Condition)).Skip(param.PageNum * param.PageSize).Take(param.PageSize).ToList();
        //    //    return new WebResult() { Rvalue = lst2 };
        //    //} 
        //    #endregion
        //}

        ///// <summary>
        ///// 新增数据
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Publisher/AddData")]
        //public WebResult AddData(List<PublishConfig> param)
        //{
        //    string sql = "";    //sql语句
        //    for (int i = 0; i < param.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            sql += $"INSERT INTO PublishConfig SELECT {param[i].ExchangeName} AS ExchangeName,{param[i].ExchangeKey} AS ExchangeKey,{param[i].IsDefault} AS IsDefault,{param[i].ExchangeType} AS ExchangeType";
        //        }
        //        else
        //        {
        //            sql += $"UNION SELECT {param[i].ExchangeName}, {param[i].ExchangeKey},{param[i].ExchangeType}, {param[i].IsDefault}";
        //        }
        //    }

        //    int flag = SqliteOperater.RunSQL(sql);
        //    WebResult result = new WebResult()
        //    {
        //        IsSuccess = flag != param.Count ? true : false,
        //        Rvalue = $"总条数{param.Count},插入成功{flag}条"
        //    };
        //    return result;
        //}

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Publisher/DeleteData")]
        //public WebResult DeleteData(List<string> param)
        //{
        //    string sql = $"delete from PublishConfig where Id=''";
        //    foreach (var item in param)
        //    {
        //        sql += $"or Id='{item}'";
        //    }
        //    int flag = SqliteOperater.RunSQL(sql);
        //    WebResult result = new WebResult()
        //    {
        //        IsSuccess = flag != param.Count ? true : false,
        //        Rvalue = $"总条数{param.Count},删除成功{flag}条"
        //    };
        //    return result;
        //}

        ///// <summary>
        ///// 修改数据
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Publisher/UpdateData")]
        //public WebResult UpdateData(List<PublishConfig> param)
        //{
        //    int flag = 0;
        //    foreach (var item in param)
        //    {
        //        string sql = $"update PublishConfig set where Id=''";
        //        flag += SqliteOperater.RunSQL(sql);
        //    }
        //    WebResult result = new WebResult()
        //    {
        //        IsSuccess = flag != param.Count ? true : false,
        //        Rvalue = $"总条数{param.Count},修改成功{flag}条"
        //    };
        //    return result;
        //} 
        #endregion
    }
}
