using System;

// namespaces...
namespace Schedule.Common.Extention
{
    // public classes...
    public static class StringExt
    {
        // public methods...
        /// <summary>
        /// string扩展方法
        /// </summary>
        /// <param name="tgsj"></param>
        /// <returns></returns>
        public static string FormatFssj(this string tgsj)
        {
            var tgsjFormated = tgsj.Substring(0, 4) + "-" + tgsj.Substring(4, 2) + "-" + tgsj.Substring(6, 2) + " " + tgsj.Substring(8, 2) + ":" + tgsj.Substring(10, 2) + ":" + tgsj.Substring(12, 2);
            return tgsjFormated;
        }
        /// <summary>
        /// string 扩展方法，通关时间转换
        /// </summary>
        /// <param name="tgsj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTime FormatFssj(this string tgsj, object args)
        {
            var tgsjFormated = tgsj.Substring(0, 4) + "-" + tgsj.Substring(4, 2) + "-" + tgsj.Substring(6, 2) + " " + tgsj.Substring(8, 2) + ":" + tgsj.Substring(10, 2) + ":" + tgsj.Substring(12, 2);
            return Convert.ToDateTime(tgsjFormated);
        }
        /// <summary>
        /// string 扩展方法,解析Wybs包含的时间
        /// </summary>
        /// <param name="wybs"></param>
        /// <returns></returns>
        public static DateTime ParseWybs(this string wybs)
        {
            var sj =  wybs.Substring( 8, 4) + "-" +  wybs.Substring( 12, 2) + "-" +  wybs.Substring( 14, 2) + " " +  wybs.Substring( 16, 2) + ":" +  wybs.Substring( 18, 2) + ":" +  wybs.Substring( 20, 2);
            return Convert.ToDateTime(sj);
        }
    }
}
