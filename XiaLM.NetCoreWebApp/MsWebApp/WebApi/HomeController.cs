using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.WebApi
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("Home/Index")]
        public ActionResult Index()
        {
            return new RedirectResult($@"/Home/index.html"); //页面跳转
        }
    }
}
