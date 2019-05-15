using Base.Common.LogHelp;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Schedule.Common.SqlHelp.Impl
{
    /// <summary>
    /// 提供SQL SERVER的数据库操作接口实现
    /// </summary>
    public class SqlServerSqlOperate : ISqlOperate
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
                using (var sqlDb = new SqlConnection(myConnectionStr))
                {
                    using (var cmd = new SqlCommand(cmdText, sqlDb))
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
                using (var sqlDb = new SqlConnection(myConnectionStr))
                {
                    using (var da = new SqlDataAdapter(cmdText, sqlDb))
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
