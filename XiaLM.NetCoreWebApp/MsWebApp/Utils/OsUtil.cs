using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.Utils
{
    /// <summary>
    /// 系统查看助手
    /// </summary>
    public class OsUtil
    {
        // 获取操作系统ID
        public static System.PlatformID GetPlatformID()
        {
            //获取系统信息
            System.OperatingSystem osInfo = System.Environment.OSVersion;

            //获取操作系统ID
            System.PlatformID platformID = osInfo.Platform;

            return platformID;
        }

        // 获取主版本号
        public static int GetVersionMajor()
        {
            //获取系统信息
            System.OperatingSystem osInfo = System.Environment.OSVersion;

            //获取主版本号
            int versionMajor = osInfo.Version.Major;

            return versionMajor;
        }

        // 获取副版本号
        public static int GetVersionMinor()
        {
            //获取系统信息
            System.OperatingSystem osInfo = System.Environment.OSVersion;

            //获取副版本号
            int versionMinor = osInfo.Version.Minor;

            return versionMinor;
        }

        //C#判断操作系统是否为Windows98
        public static bool IsWindows98
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32Windows) && (Environment.OSVersion.Version.Minor == 10) && (Environment.OSVersion.Version.Revision.ToString() != "2222A");
            }
        }
        //C#判断操作系统是否为Windows98第二版
        public static bool IsWindows98Second
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32Windows) && (Environment.OSVersion.Version.Minor == 10) && (Environment.OSVersion.Version.Revision.ToString() == "2222A");
            }
        }

        /// <summary>
        /// 是否是常规Windows(Windows2000以上的系统)
        /// </summary>
        public static bool IsConventionWindows
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT);
            }
        }

        //C#判断操作系统是否为Windows2000
        public static bool IsWindows2000
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor == 0);
            }
        }
        //C#判断操作系统是否为WindowsXP
        public static bool IsWindowsXP
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor == 1);
            }
        }
        //C#判断操作系统是否为Windows2003
        public static bool IsWindows2003
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor == 2);
            }
        }
        //C#判断操作系统是否为WindowsVista
        public static bool IsWindowsVista
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor == 0);
            }
        }
        //C#判断操作系统是否为Windows7
        public static bool IsWindows7
        {
            get
            {
                return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 6) && (Environment.OSVersion.Version.Minor == 1);
            }
        }
        //C#判断操作系统是否为Unix
        public static bool IsUnix
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Unix;
            }
        }
        //利用C#判断当前操作系统是否为Win8系统
        public static bool IsWindows8()
        {
            Version currentVersion = Environment.OSVersion.Version;
            Version compareToVersion = new Version("6.2");
            if (currentVersion.CompareTo(compareToVersion) >= 0)
                return true;
            else
                return false;
        }
    }
}
