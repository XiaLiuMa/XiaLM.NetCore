using System.Diagnostics;

// namespaces...
namespace Schedule.Common.Util
{
    // public classes...
    public static class ProcessUtil
    {
        // public methods...
        /// <summary>
        /// 获取当前正在运行的例程，防止程序运行两次
        /// </summary>
        public static Process RunningInstance(string exeFileName)
        {
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            foreach (var process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (current.MainModule != null)
                    {
                        if (exeFileName == current.MainModule.FileName)
                        {
                            return process;
                        }
                    }
                }
            }
            return null;
        }
    }
}
