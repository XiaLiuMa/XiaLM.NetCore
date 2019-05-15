using System;

// namespaces...
namespace Schedule.Common.Util
{
    // public classes...
    /// <summary>
    /// 提供日期，时间的常用操作
    /// hcb 2013.12.4
    /// </summary>
    public class TimeUtil
    {
        // public methods...
        /// <summary>
        /// 比较两个时间的相差是否在指定的天数内
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="minute">天数，不能大于30或31或28</param>
        /// <returns></returns>
        public static bool CompareByDays(DateTime beginTime, DateTime endTime, int day)
        {
            var ts = endTime - beginTime ;
            return ts.Days < day;
        }
        /// <summary>
        /// 比较两个时间的相差是否在指定的分钟内
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="minute">分钟，不能大于60</param>
        /// <returns></returns>
        public static bool CompareByMinutes(DateTime beginTime, DateTime endTime, int minute)
        {
            var ts = endTime - beginTime;
            float minutes = ts.Minutes;

            if (ts.Days > 0)
            {
                minutes += ts.Days * 60;
            }
            return minutes < minute;
        }
    }
}
