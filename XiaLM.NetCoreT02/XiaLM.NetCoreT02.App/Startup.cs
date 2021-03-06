﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using XiaLM.NetCoreT02.Db;
using XiaLM.NetCoreT02.Db.IRepositories;
using XiaLM.NetCoreT02.Db.Repositories;
using XiaLM.NetCoreT02.Service;
using XiaLM.NetCoreT02.Service.IService;
using XiaLM.NetCoreT02.Service.Service;

namespace XiaLM.NetCoreT02.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            BaseMapper.Initialize();    //初始化映射关系
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");//获取数据库连接字符串
            //services.AddDbContext<BaseDBContext>(options => options.UseSqlServer(sqlConnectionString));
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");//获取数据库连接字符串
            services.AddDbContext<BaseDBContext>(options => options.UseSqlServer(sqlConnectionString));

            //依赖注入
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddMvc();

            services.AddSession();  //Session服务
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//开发环境异常处理
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");//生产环境异常处理
            }

            app.UseStaticFiles();//缺少会导致wwwroot下的资源无法访问
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            });//使用静态文件
            app.UseSession();//Session
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
            });//使用Mvc，设置默认路由为系统登录

            SeedData.Initialize(app.ApplicationServices); //初始化数据
        }
    }
}
