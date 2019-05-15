using Base.Common.LogHelp;
using Base.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebApp.Dal;
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
    /// 数据库配置
    /// </summary>
    public class DataBaseController : Controller
    {
        private const string ModelName = "数据库配置";
        private DataBaseDal dal = new DataBaseDal();

        /// <summary>
        /// 查询数据库配置
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DataBase/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端数据库配置数据")]
        public ActionResult Query(string query, string page, string rows)
        {
            List<DataBase> list = GetDataBases();
            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(p => p.ServerIp.Contains(query)).OrderBy(p => p.ServerIp).ToList();
            };
            int count = list.Count;
            list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("total", count);
            //dic.Add("rows", list);
            //return Json(dic);
            return Json(list);
        }

        /// <summary>
        /// 新增数据库配置
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DataBase/Add")]
        [LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端数据库配置数据")]
        public ActionResult Add(string serverName, string serverIp, string serverPort, string isDefault)
        {
            try
            {
                List<DataBase> list = GetDataBases();
                DataBase dataBaseConfig = new DataBase()
                {
                    ServiceName = serverName,
                    ServerIp = serverIp,
                    ServerPort = serverPort,
                    Default = isDefault,
                    ID = list.OrderBy(p => p.ID).LastOrDefault().ID + 1,    //ID自增长
                    DGuid = Guid.NewGuid().ToString("D"),
                };
                list.Add(dataBaseConfig);
                SetDataBases(list);
                return Content("true");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return Content("false");
            }
        }

        /// <summary>
        /// 修改数据库配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dGuid"></param>
        /// <param name="serverIP"></param>
        /// <param name="serverPort"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DataBase/Alert")]
        [LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端数据库配置数据")]
        public ActionResult Alert(string id, string dGuid, string serverIP, string serverPort, string isDefault)
        {
            try
            {
                DataBase dataBaseConfig = new DataBase()
                {
                    ID = int.Parse(id),
                    DGuid = dGuid,
                    ServerIp = serverIP,
                    ServerPort = serverPort,
                    Default = isDefault
                };
                List<DataBase> list = GetDataBases();
                var obj = list.Where(p => p.ID == int.Parse(id)).FirstOrDefault();
                int index = ListUtil.GetIndex(list, obj);   //获取索引
                list = ListUtil.Alert(list, dataBaseConfig, index); //修改列表
                SetDataBases(list);
                return Content("true");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return Content("false");
            }
        }

        /// <summary>
        /// 删除数据库配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DataBase/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端数据库配置数据")]
        public ActionResult Delete(List<string> ids)
        {
            try
            {
                List<DataBase> list = GetDataBases();
                foreach (string id in ids)
                {
                    //if (!IsUsed(id))    //没有被使用的才能被删除
                    //{
                    //    list.Remove(list.Where(p => p.DGuid.Equals(id)).FirstOrDefault());
                    //}
                    list.Remove(list.Where(p => p.DGuid.Equals(id)).FirstOrDefault());
                }
                SetDataBases(list);
                return Content("true");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return Content("false");
            }
        }

        #region 数据库配置文件操作
        /// <summary>
        /// 该数据是否被使用，如果被使用返回true。防止数据结构混乱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsUsed(string id)
        {
            string fPath = $"{DirectoryUtil.GetAppParentDir()}/Ms.DataEngineService/config/QuartzByCron.xml";  //配置文件路径
            List<QuartzByCron> lst = XmlUtils.Load<List<QuartzByCron>>(fPath);
            foreach (var item in lst)
            {
                if (item.DataBaseId.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取数据库配置
        /// </summary>
        /// <returns></returns>
        private List<DataBase> GetDataBases()
        {
            string fPath = $"{DirectoryUtil.GetAppParentDir()}/Ms.DataEngineService/config/DataBase.xml";  //配置文件路径
            List<DataBase> lst = XmlUtils.Load<List<DataBase>>(fPath);
            return lst;
        }


        /// <summary>
        /// 保存数据库配置
        /// </summary>
        /// <returns></returns>
        private bool SetDataBases(List<DataBase> lst)
        {
            string fPath = $"{DirectoryUtil.GetAppParentDir()}/Ms.DataEngineService/config/DataBase.xml";  //配置文件路径
            List<DataBase> oldlst = XmlUtils.Load<List<DataBase>>(fPath);   //原始列表
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
