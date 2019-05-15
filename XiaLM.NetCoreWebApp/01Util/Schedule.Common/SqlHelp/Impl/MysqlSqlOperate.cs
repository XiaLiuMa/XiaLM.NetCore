using Base.Common.LogHelp;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Schedule.Common.SqlHelp.Impl
{
    /// <summary>
    /// 提供Mysql的数据库操作接口实现
    /// </summary>
    public class MysqlSqlOperate : ISqlOperate
    {
        /// <summary>
        /// 数据库连接源
        /// </summary>
        private string myConnectionStr;
        /// <summary>
        /// 获取或设置数据库连接
        /// </summary>
        public string DbConStr
        {
            get
            {
                return this.myConnectionStr;
            }
            set
            {
                myConnectionStr = value;
            }
        }

        public void Dispose()
        {
        }
        public int RunSQL(string cmdText)
        {
            var re = 0;
            try
            {
                using (var sqlDb = new MySqlConnection(myConnectionStr))
                {
                    using (var cmd = new MySqlCommand(cmdText, sqlDb))
                    {
                        sqlDb.Open();
                        re = cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        sqlDb.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error(cmdText);
                Log.Error(ex.ToString());
            }
            return re;
        }
        public void RunSQL(string cmdText, ref DataSet ds)
        {
            try
            {
                using (var sqlDb = new MySqlConnection(myConnectionStr))
                {
                    using (var da = new MySqlDataAdapter(cmdText, sqlDb))
                    {
                        sqlDb.Open();
                        da.Fill(ds);
                        da.Dispose();
                        sqlDb.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(cmdText);
                Log.Error(ex.ToString());
            }
        }
    }
}
