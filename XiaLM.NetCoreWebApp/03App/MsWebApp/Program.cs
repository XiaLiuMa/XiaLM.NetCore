﻿using Base.Common.LogHelp;
using MsWebApp.Comm;
using MsWebApp.Dal;
using System;

namespace MsWebApp
{
    class Program
    {
        private static WebServer webServer;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "MsWebApp");
            bool Running = !mutex.WaitOne(0, false);
            if (!Running)
            {
                ConfigHandler.GetInstance().Init();
                Global.Init();

                #region 启动webapi服务
                webServer = new WebServer();
                webServer.Start();
                #endregion

                while (true) Console.ReadKey();
            }
        }

        /// <summary>
        /// 未捕获的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ToString());
            Environment.Exit(0);
        }

        /// <summary>
        /// 控制台进程关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Global.UnInit();    //释放系统
            webServer?.Dispose();
        }
    }
}
