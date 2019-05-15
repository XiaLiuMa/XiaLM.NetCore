using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using MsWebApp.Comm;
using MsWebApp.Mods.Comm;
using MsWebAppDal.Dal.UserManage;
using MsWebAppDal.Entity.UserManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MsWebApp.WebApi.UserManage
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : Controller
    {
        private const string ModelName = "用户管理";
        private UserDal dal = new UserDal();

        /// <summary>
        /// 进入登陆界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("User/ToLogin")]
        public ActionResult ToLogin()
        {
            //return RedirectToAction("OutLogin");  //Action重定向
            return new RedirectResult($@"/UserManage/User/login.html"); //页面跳转                                          
        }


        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/CkLogin")]
        public WebResult CkLogin(string uname, string pwd)
        {
            List<User> list = dal.Select($" Uname like '{uname}' and Pword like '{pwd}'");
            if (list != null && list.Count > 0)
            {
                UserCache.SetCache(list.FirstOrDefault()); //设置用户缓存
            }
            return new WebResult() { Rstate = UserCache.IsLogin ? Rstate.Success : Rstate.Fail };
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/OutLogin")]
        public WebResult OutLogin()
        {
            UserCache.RemoveCache();
            return new WebResult() { Rstate = UserCache.IsLogin ? Rstate.Fail : Rstate.Success };
        }



        /// <summary>
        /// 查询用户数据
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端用户数据")]
        public ActionResult Query(string uname, string page, string rows)
        {
            int count = 0;
            List<User> list = new List<User>();
            list = dal.Select($" Uname like '{uname}'");
            if (list == null) { list = new List<User>(); }
            count = list.Count;
            list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", count);
            dic.Add("rows", list);
            return Json(dic);
        }

        /// <summary>
        /// 新增用户数据
        /// </summary>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/Add")]
        [LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端用户数据")]
        public ActionResult Add(string uname, string pwd, string permissions)
        {
            User user = new User()
            {
                Uname = uname,
                Pword = pwd,
                Permissions = permissions
            };
            return Content(dal.Add(user).ToString());
        }

        /// <summary>
        /// 修改用户数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uname"></param>
        /// <param name="pwd"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/Update")]
        [LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端用户数据")]
        public ActionResult Update(string id, string uname, string pwd, string permissions)
        {
            User user = new User()
            {
                ID = int.Parse(id),
                Uname = uname,
                Pword = pwd,
                Permissions = permissions
            };
            return Content(dal.Update(user).ToString());
        }

        /// <summary>
        /// 删除用户数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/Delect")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端用户数据")]
        public ActionResult Delect(string id)
        {
            string[] str = id.Split(',');
            foreach (string item in str)
            {
                dal.Delete(item);
            }
            return Content("");
        }
    }
}
