using Base.Common.LogHelp;
using Base.Common.Utils;
using MsWebApp.Mods;
using MsWebApp.Mods.Config;
using MsWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MsWebApp.Comm
{
    /// <summary>
    /// 进程管理工具
    /// </summary>
    public class ProcessHandler
    {
        private CancellationTokenSource cts;
        private List<string> processNames;
        private static ProcessHandler instance;
        private readonly static object objLock = new object();
        public static ProcessHandler GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new ProcessHandler();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 开启进程
        /// </summary>
        /// <param name="processPaths">exe执行文件的路径</param>
        public void Start(List<ProcessConfig> processes)
        {
            cts = new CancellationTokenSource();
            if (processes != null && processes.Count > 0)
            {
                foreach (var temp in processes)
                {
                    StartProcessMonitor(temp);
                    processNames.Add(temp.Name);
                }
            }
        }

        public void Kill(List<string> pNames)
        {
            if (pNames != null && pNames.Count > 0)
            {
                foreach (var item in pNames)
                {
                    ProcessUtil.Kill(item);
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// 启动进程监控
        /// </summary>
        /// <param name="monitorArgs"></param>
        private void StartProcessMonitor(ProcessConfig monitorArgs)
        {
            bool b = false;
            var token = cts.Token;
            Task.Factory.StartNew(() =>
            {

                bool isCancel = false;
                while (!(isCancel = token.IsCancellationRequested))
                {
                    if (!b)
                    {
                        Thread.Sleep(1000);
                        b = true;
                        if (isCancel) break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        if (isCancel) break;
                    }
                    if (isCancel) break;
                    var state = ProcessUtil.GetPorcessState(monitorArgs.Name);
                    if (!state)
                    {
                        Log.Info($"启动{monitorArgs.Describe}...");
                        if (OsUtil.IsConventionWindows)
                        {
                            ProcessUtil.Start(DirectoryUtil.GetAppParentDir() + monitorArgs.WindowsPath);
                        }
                        else
                        {
                            ProcessUtil.Start(DirectoryUtil.GetAppParentDir() + monitorArgs.WindowsPath);
                        }
                        Log.Info($"启动{monitorArgs.Describe}完成。");
                    }
                }
            });
        }

        public void Cancle()
        {
            cts?.Cancel();
            Thread.Sleep(1500);
            Kill(processNames);
        }
    }
}
