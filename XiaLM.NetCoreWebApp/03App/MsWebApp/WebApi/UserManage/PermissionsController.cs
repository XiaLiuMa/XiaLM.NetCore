using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebAppDal.Dal.UserManage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.WebApi.UserManage
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class PermissionsController : Controller
    {
        private const string ModelName = "权限管理";
        private PermissionsDal dal = new PermissionsDal();

        ///// <summary>
        ///// 查询权限数据
        ///// </summary>
        ///// <param name="query"></param>
        ///// <param name="page"></param>
        ///// <param name="rows"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Permissions/Query")]
        //[LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端权限数据")]
        //public ActionResult Query(string query, string page, string rows)
        //{
        //    int count = 0;
        //    List<User> list = new List<User>();
        //    list = dal.Select($" Uname like '{uname}'");
        //    if (list == null) { list = new List<User>(); }
        //    count = list.Count;
        //    list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();
        //    Dictionary<string, object> dic = new Dictionary<string, object>();
        //    dic.Add("total", count);
        //    dic.Add("rows", list);
        //    return Json(dic);
        //}

        ///// <summary>
        ///// 新增权限数据
        ///// </summary>
        ///// <param name="uname"></param>
        ///// <param name="pwd"></param>
        ///// <param name="permissions"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Permissions/Add")]
        //[LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端权限数据")]
        //public ActionResult Add(string uname, string pwd, string permissions)
        //{
        //    Permissions user = new Permissions()
        //    {
        //        Uname = uname,
        //        Pword = pwd,
        //        Permissions = permissions
        //    };
        //    return Content(dal.Add(user).ToString());
        //}

        ///// <summary>
        ///// 修改权限数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="uname"></param>
        ///// <param name="pwd"></param>
        ///// <param name="permissions"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Permissions/Update")]
        //[LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端权限数据")]
        //public ActionResult Update(string id, string uname, string pwd, string permissions)
        //{
        //    User user = new User()
        //    {
        //        ID = int.Parse(id),
        //        Uname = uname,
        //        Pword = pwd,
        //        Permissions = permissions
        //    };
        //    return Content(dal.Update(user).ToString());
        //}

        ///// <summary>
        ///// 删除权限数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Permissions/Delect")]
        //[LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端权限数据")]
        //public ActionResult Delect(string id)
        //{
        //    string[] str = id.Split(',');
        //    foreach (string item in str)
        //    {
        //        dal.Delete(item);
        //    }
        //    return Content("");

        //}
    }
}
