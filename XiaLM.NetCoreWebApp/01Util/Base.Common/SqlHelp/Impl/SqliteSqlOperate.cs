using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace Base.Common.SqlHelp.Impl
{
    /// <summary>
    /// 提供Sqlite的数据库操作接口实现
    /// </summary>
    public class SqliteSqlOperate : ISqlOperate
    {
        private string dbConStr;
        /// <summary>
        /// 获取或设置数据库连接
        /// </summary>
        public string DbConStr
        {
            get { return this.dbConStr; }
            set { dbConStr = value; }
        }

        private static object lckObj = new object();
        public int RunSQL(string cmdText)
        {
            int re = -1;
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new SQLiteConnection(dbConStr))
                    {
                        using (var cmd = new SQLiteCommand(cmdText, oleDb))
                        {
                            //Log.Debug(cmdText);
                            oleDb.Open();
                            re = cmd.ExecuteNonQuery();
                            return re;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Log.Error(ex.ToString());
                    return -1;
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void RunSQL(string cmdText, ref DataSet ds)
        {
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new SQLiteConnection(dbConStr))
                    {
                        using (var da = new SQLiteDataAdapter(cmdText, oleDb))
                        {
                            //Log.Debug(cmdText);
                            oleDb.Open();
                            da.Fill(ds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Log.Error(ex.ToString());
                }
            }
        }
    }
}
