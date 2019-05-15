using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MsWebApp.Utils
{
    /// <summary>
    /// 进程帮助类
    /// </summary>
    public class ProcessUtil
    {
        private static string _workingDirectory;//程序工作目录 
        /// <summary>
        /// 程序工作目录
        /// </summary>
        public static string WorkingDirectory
        {
            get { return _workingDirectory; }
            set { _workingDirectory = value; }
        }
        public static void Start(string executablePath, bool isHidden = false, bool _isAdmin = true)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = _workingDirectory;
            startInfo.FileName = executablePath;
            if (isHidden)
            {
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            //设置启动动作,确保以管理员身份运行
            if (_isAdmin)
            {
                startInfo.Verb = "runas";
            }
            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 杀死进程
        /// </summary>
        /// <param name="processName"></param>
        public static void Kill(string processName)
        {
            try
            {
                var list = System.Diagnostics.Process.GetProcesses().Where(p => p.ProcessName.Equals(processName)).ToList();
                list.ForEach(r => r.Kill());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 当前程序是否运行
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool GetPorcessState(string name)
        {
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
            System.Diagnostics.Process myProcess = myProcesses.Where(r => r.ProcessName.Equals(name)).FirstOrDefault();
            if (myProcess == null)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandTexts"></param>
        /// <returns></returns>
        public static void ExeCommand(string fileName, params string[] commandTexts)
        {
            Process p = new Process();
            p.StartInfo.FileName = fileName;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileName);
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strOutput = null;
            try
            {
                p.Start();
                foreach (string item in commandTexts)
                {
                    p.StandardInput.WriteLine(item);
                }
                p.StandardInput.WriteLine("quit");
                strOutput = p.StandardOutput.ReadToEnd();
                p.WaitForExit(200);
                p.Close();
                p.Kill();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
