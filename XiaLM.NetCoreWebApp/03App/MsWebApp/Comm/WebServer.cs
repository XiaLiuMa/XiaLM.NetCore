using Base.Common.LogHelp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MsWebApp.Hubs;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace MsWebApp.Comm
{
    public class WebServer : IDisposable
    {
        private IWebHost webHost;

        public void Start()
        {
            Log.Info("启动WebServer...");
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (PortInUse(ConfigHandler.GetInstance().baseConfig.HostPort))    //端口被占用
                    {
                        int tempPort = 0;
                        Random rand = new Random();
                        do
                        {
                            tempPort = rand.Next(5000, 10000);
                        } while (PortInUse(tempPort));
                        ConfigHandler.GetInstance().baseConfig.HostPort = tempPort;
                        ConfigHandler.GetInstance().SaveBaseConfig(ConfigHandler.GetInstance().baseConfig);
                        Thread.Sleep(1000); //等待基础数据刷新
                    }
                    string webapiUrl = $"http://{ConfigHandler.GetInstance().baseConfig.HostIp}:{ConfigHandler.GetInstance().baseConfig.HostPort}";

                    webHost = new WebHostBuilder()
                        .UseKestrel()
                        .UseUrls(webapiUrl)
                        //.UseWebRoot($"{AppContext.BaseDirectory}/wwwroot")
                        .UseContentRoot(AppContext.BaseDirectory)
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .Build();
                    webHost.Run();
                }
                catch (Exception ex)
                {
                    Log.Info("关闭WebServer失败。");
                    Log.Error(ex.ToString());
                }
            });
        }

        /// <summary>
        /// 检测端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private bool PortInUse(int port)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            return ipEndPoints.Any(c => c.Port == port);
        }

        public void Dispose()
        {
            Log.Info("关闭WebServer...");
            webHost?.StopAsync();
            webHost?.Dispose();
        }
    }

    public class Startup
    {
        public static GlobalHubServer<LinkHubs> GlobalHub { get; private set; }

        /// <summary>
        /// 此方法由运行时调用。使用此方法将服务添加到容器中。
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //添加cors 服务
            //services.AddCors(options =>options.AddPolicy("CorsSample", p => p.WithOrigins("http://192.168.110.56:5539").AllowAnyMethod().AllowAnyHeader()));

            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddTransient<GlobalHubServer<LinkHubs>>();
            services.AddSignalR();
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法配置HTTP请求管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            #region 调用文件服务器，使用静态html文件
            app.UseStaticFiles();//缺少会导致wwwroot下的资源无法访问
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider($@"{AppContext.BaseDirectory}wwwroot"),
                RequestPath = "/wwwroot"
            });
            #endregion
            app.UseMiddleware<MvcHandlerMiddleware>();  //注册路由拦截中间件
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });//使用Mvc，设置默认路由为系统登录
            //app.UseCors("CorsSample");  //配置Cors

            GlobalHub = serviceProvider.GetService<GlobalHubServer<LinkHubs>>();
        }
    }

    /// <summary>
    /// MVC路由拦截中间件
    /// </summary>
    public class MvcHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public MvcHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            string url = httpContext.Request.Path;

            #region 只有前端请求静态资源路径出错时才会进这里(可关闭)
            if (url.Contains("wwwroot/comm/"))
            {
                Log.Error($"前端请求静态资源路径错误--{url}");
                return _next(httpContext);  //放行本地公用静态资源
            } 
            #endregion

            if (url.Equals("/User/CkLogin"))
            {
                return _next(httpContext);  //放行登陆请求
            }

            //if (!UserCache.IsLogin)   //缓存中没有用户
            //{
            //    var app = new ApplicationBuilder(serviceProvider);
            //    httpContext.Request.Path = "/User/ToLogin";
            //    return _next(httpContext);
            //}
            //else
            //{
            //    if (url.Equals("/"))    //默认路由
            //    {
            //        var app = new ApplicationBuilder(serviceProvider);
            //        httpContext.Request.Path = "/Home/Index";
            //    }
            //    return _next(httpContext);
            //}

            if (url.Equals("/"))    //默认路由
            {
                var app = new ApplicationBuilder(serviceProvider);
                httpContext.Request.Path = "/Home/Index";
            }
            return _next(httpContext);
        }
    }
}
