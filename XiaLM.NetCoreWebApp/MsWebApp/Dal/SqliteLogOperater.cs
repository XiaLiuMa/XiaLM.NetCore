using Base.Common.LogHelp;
using Base.Common.Utils;
using MsWebApp.Comm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace MsWebApp.Dal
{
    public class SqliteLogOperater
    {
        private static object lckObj = new object();

        private static string strSqlite = $@"Data Source={DirectoryUtil.GetAppParentDir() + ConfigHandler.GetInstance().baseConfig.LogDb}";

        public static int RunSQL(string cmdText)
        {
            int re = -1;
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new SQLiteConnection(strSqlite))
                    {
                        using (var cmd = new SQLiteCommand(cmdText, oleDb))
                        {
                            oleDb.Open();
                            re = cmd.ExecuteNonQuery();
                            return re;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    return -1;
                }
            }
        }

        /// <summary>
        /// 查询单表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> SelectTable<T>(string sql)
        {
            DataSet ds = new DataSet();
            RunSQL(sql, ref ds);
            if (ds.Tables == null || ds.Tables.Count >= 0) return null;
            return JsonUtil.DataTableToObjs<T>(ds.Tables[0]);
        }

        public static void RunSQL(string cmdText, ref DataSet ds)
        {
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new SQLiteConnection(strSqlite))
                    {
                        using (var da = new SQLiteDataAdapter(cmdText, oleDb))
                        {
                            oleDb.Open();
                            da.Fill(ds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                }
            }
        }
    }

}
