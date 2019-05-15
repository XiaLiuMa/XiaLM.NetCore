using Base.Common.LogHelp;
using Base.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebAppDal.Dal.SystemManage;
using MsWebAppDal.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsWebApp.WebApi.SystemManage
{
    public class SchedulerController : Controller
    {
        private const string ModelName = "任务调度配置";
        private SchedulerDal dal = new SchedulerDal();

        /// <summary>
        /// 分页得到数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult GetData(string id, string page, string rows)
        {
            List<QuartzByCron> list = new List<QuartzByCron>();
            int count = 10;
            string con = "1=1";
            if (id != "")
            {
                con = string.Format("Id={0}", id);
            }
            list = dal.SelectAll();
            if (list == null)
            {
                list = new List<QuartzByCron>();
            }
            count = list.Count;
            var ls = (from r in list
                      orderby r.SchedulerState ascending
                      select new
                      {
                          r.ID,
                          r.QGuid,
                          r.SchedulerName,
                          r.Priority,
                          r.Frequency,
                          r.description,
                          r.SchedulerState

                      }).ToList().Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList(); ;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", count);
            dic.Add("rows", ls);
            return Json(dic);
        }


        /// <summary>
        /// 查询调度配置
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Scheduler/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端调度配置数据")]
        public ActionResult Query(string query, string page, string rows)
        {
            List<QuartzByCron> list = GetQuartzByCrons();
            list = list.Where(p => p.SchedulerName.Contains(query)).OrderBy(p => p.SchedulerName).ToList();
            int count = list.Count;
            list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", count);
            dic.Add("rows", list);
            return Json(dic);
        }


        /// <summary>
        /// 添加调度配置
        /// </summary>
        /// <param name="SchedulerName"></param>
        /// <param name="SchedulerState"></param>
        /// <param name="Frequency"></param>
        /// <param name="Priority"></param>
        /// <param name="Durable"></param>
        /// <param name="TtlTime"></param>
        /// <param name="DataBaseId"></param>
        /// <param name="cronExpression"></param>
        /// <param name="description"></param>
        /// <param name="AlertInit"></param>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Scheduler/Add")]
        [LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端调度配置数据")]
        public ActionResult Add(string SchedulerName, string SchedulerState, string Frequency, string Priority, string Durable, string TtlTime, string DataBaseId, string cronExpression, string description, string AlertInit, string ServiceName)
        {
            #region 输入检查
            if (string.IsNullOrEmpty(SchedulerName.Trim())) return Content("请输入SchedulerName");
            if (string.IsNullOrEmpty(SchedulerState.Trim())) return Content("请输入SchedulerState");
            if (string.IsNullOrEmpty(Frequency.Trim())) return Content("请输入Frequency");
            if (string.IsNullOrEmpty(Priority.Trim())) return Content("请输入Priority");
            if (string.IsNullOrEmpty(Durable.Trim())) return Content("请输入Durable");
            if (string.IsNullOrEmpty(TtlTime.Trim())) return Content("请输入TtlTime");
            if (string.IsNullOrEmpty(DataBaseId.Trim())) return Content("请输入DataBaseId");
            if (string.IsNullOrEmpty(cronExpression.Trim())) return Content("请输入cronExpression");
            if (string.IsNullOrEmpty(AlertInit.Trim())) return Content("请输入AlertInit");
            if (string.IsNullOrEmpty(ServiceName.Trim())) return Content("请输入ServiceName");
            #endregion

            List<QuartzByCron> list = GetQuartzByCrons();
            QuartzByCron qbc = new QuartzByCron()
            {
                ID = list.OrderBy(p => p.ID).LastOrDefault().ID + 1,    //ID自增长
                SchedulerName = SchedulerName,
                QGuid = Guid.NewGuid().ToString("D"),
                SchedulerState = int.Parse(SchedulerState),
                Frequency = Frequency,
                Priority = int.Parse(Priority),
                DataBaseId = DataBaseId,
                cronExpression = cronExpression,
                description = description,
                AlertInit = AlertInit,
                ServiceName = ServiceName,
                Durable = int.Parse(Durable),
                TtlTime = int.Parse(TtlTime)
            };
            list.Add(qbc);
            return Content(SetQuartzByCrons(list).ToString());
        }

        /// <summary>
        /// 修改调度配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="QGuid"></param>
        /// <param name="SchedulerName"></param>
        /// <param name="SchedulerState"></param>
        /// <param name="Frequency"></param>
        /// <param name="Priority"></param>
        /// <param name="Durable"></param>
        /// <param name="TtlTime"></param>
        /// <param name="DataBaseId"></param>
        /// <param name="cronExpression"></param>
        /// <param name="description"></param>
        /// <param name="AlertInit"></param>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Scheduler/Alert")]
        [LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端调度配置数据")]
        public async Task<ActionResult> Alert(string id, string QGuid, string SchedulerName, string SchedulerState, string Frequency, string Priority, string Durable, string TtlTime, string DataBaseId, string cronExpression, string description, string AlertInit, string ServiceName)
        {
            #region 输入检查
            if (string.IsNullOrEmpty(id.Trim())) return Content("请输入id");
            if (string.IsNullOrEmpty(QGuid.Trim())) return Content("请输入QGuid");
            if (string.IsNullOrEmpty(SchedulerName.Trim())) return Content("请输入SchedulerName");
            if (string.IsNullOrEmpty(SchedulerState.Trim())) return Content("请输入SchedulerState");
            if (string.IsNullOrEmpty(Frequency.Trim())) return Content("请输入Frequency");
            if (string.IsNullOrEmpty(Priority.Trim())) return Content("请输入Priority");
            if (string.IsNullOrEmpty(Durable.Trim())) return Content("请输入Durable");
            if (string.IsNullOrEmpty(TtlTime.Trim())) return Content("请输入TtlTime");
            if (string.IsNullOrEmpty(DataBaseId.Trim())) return Content("请输入DataBaseId");
            if (string.IsNullOrEmpty(cronExpression.Trim())) return Content("请输入cronExpression");
            if (string.IsNullOrEmpty(AlertInit.Trim())) return Content("请输入AlertInit");
            if (string.IsNullOrEmpty(ServiceName.Trim())) return Content("请输入ServiceName");
            #endregion

            QuartzByCron qbc = new QuartzByCron()
            {
                ID = int.Parse(id),
                QGuid = QGuid,
                SchedulerName = SchedulerName,
                SchedulerState = int.Parse(SchedulerState),
                Frequency = Frequency,
                Priority = int.Parse(Priority),
                DataBaseId = DataBaseId,
                cronExpression = cronExpression,
                description = description,
                AlertInit = AlertInit,
                ServiceName = ServiceName,
                Durable = int.Parse(Durable),
                TtlTime = int.Parse(TtlTime)
            };
            List<QuartzByCron> list = GetQuartzByCrons();
            var obj = list.Where(p => p.ID == int.Parse(id)).FirstOrDefault();
            int index = ListUtil.GetIndex(list, obj);   //获取索引
            list = ListUtil.Alert(list, qbc, index); //修改列表
            return Content(SetQuartzByCrons(list).ToString());
        }

        /// <summary>
        /// 删除调度配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Scheduler/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端调度配置数据")]
        public ActionResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids.Trim())) return Content("请输入ids");
            List<QuartzByCron> list = GetQuartzByCrons();
            string[] idArray = ids.Split(',');
            if (idArray != null && idArray.Length > 0)
            {
                foreach (string id in idArray)
                {
                    list.Remove(list.Where(p => p.ID == int.Parse(id)).FirstOrDefault());
                }
                return Content(SetQuartzByCrons(list).ToString());
            }
            else
            {
                return Content("");
            }
        }


        /// <summary>
        /// 清除调度配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Scheduler/Clear")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "清除Ms端调度配置数据")]
        public ActionResult Clear()
        {
            return Content(SetQuartzByCrons(null).ToString());
        }

        /// <summary>
        /// 获取Job列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Scheduler/GetJobs")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "Scheduler获取Job列表")]
        public ActionResult GetJobs(string id)
        {
            HttpUtil http = new HttpUtil();
            string str = http.Get("http:127.0.0.1:50231/Scheduler/GetJobs");
            return Content(str);


            //List<Job> jobs = null;
            //QuartzByCron qc = null;
            //BLLJob bJ = new BLLJob();
            //List<QuartzByCron> list = dal.SelectAll();
            //if (id != null) if (id != "") { qc = dal.Query(id); }
            //if (list == null)
            //{
            //    list = new List<QuartzByCron>();
            //}
            //string str = "";
            //foreach (var item in list)
            //{
            //    if (qc != null)
            //    {
            //        if (item.ID == qc.ID) { continue; }
            //    }
            //    str += item.ServiceName + ",";
            //}
            //str = str.TrimEnd(',');
            //if (str == "")
            //{
            //    jobs = bJ.SelectAll();
            //}
            //else
            //{
            //    jobs = bJ.Select(string.Format("ID not in ({0})", str));
            //}
            //if (jobs == null)
            //{
            //    jobs = new List<Job>();
            //}
            //return Json(jobs, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 得到数据库地址队列
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult GetSqlServer()
        //{
        //    DataBaseConfigBLL dbcBll = new DataBaseConfigBLL();
        //    List<DataBaseConfig> dbs = dbcBll.SelectAll();
        //    if (dbs != null)
        //    {
        //        return Json(dbs, JsonRequestBehavior.AllowGet);
        //    }
        //    return Content("");
        //}


        #region 调度配置文件操作
        /// <summary>
        /// 获取调度配置
        /// </summary>
        /// <returns></returns>
        private List<QuartzByCron> GetQuartzByCrons()
        {

            string fPath = $"{DirectoryUtil.GetAppParentDir()}/Ms.DataEngineService/config/QuartzByCron.xml";  //配置文件路径
            List<QuartzByCron> lst = XmlUtils.Load<List<QuartzByCron>>(fPath);

            return lst;
        }


        /// <summary>
        /// 保存调度配置
        /// </summary>
        /// <returns></returns>
        private bool SetQuartzByCrons(List<QuartzByCron> lst)
        {
            string fPath = $"{DirectoryUtil.GetAppParentDir()}/Ms.DataEngineService/config/QuartzByCron.xml";  //配置文件路径
            List<QuartzByCron> oldlst = XmlUtils.Load<List<QuartzByCron>>(fPath);   //原始列表
            try
            {
                XmlUtils.Save(lst, fPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                XmlUtils.Save(oldlst, fPath);
                return false;
            }
            return true;
        }
        #endregion
    }
}
