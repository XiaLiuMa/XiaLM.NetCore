using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.LogHelp
{
    public class Log
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Debug(string strMsg)
        {
            logger.Debug(strMsg);
        }

        public static void Info(string strMsg)
        {
            logger.Info(strMsg);
        }

        public static void Warn(string strMsg)
        {
            logger.Warn(strMsg);
        }

        public static void Error(string strMsg)
        {
            logger.Error(strMsg);
        }

        public static void Fatal(string strMsg)
        {
            logger.Fatal(strMsg);
        }

        public static void Trace(string strMsg)
        {
            logger.Trace(strMsg);
        }
    }
}
