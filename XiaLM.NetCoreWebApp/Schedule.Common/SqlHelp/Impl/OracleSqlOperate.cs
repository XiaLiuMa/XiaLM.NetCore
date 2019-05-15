using Base.Common.LogHelp;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

// namespaces...
namespace Schedule.Common.SqlHelp.Impl
{
    // public classes...
    public class OracleSqlOperate : ISqlOperate
    {
        // private fields...
        /// <summary>
        /// 数据库连接源
        /// </summary>
        private string myConnectionStr;

        //SemaphoreSlim semaphore = new SemaphoreSlim(1, 2);

        private static object lckObj = new object();

        // public properties...
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

        // public methods...
        public void Dispose()
        {
        }

        int re = -1;
        public int RunSQL(string cmdText)
        {
            //semaphore.Wait(2000);
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new OracleConnection(myConnectionStr))
                    {
                        using (var cmd = new OracleCommand(cmdText, oleDb))
                        {
                            oleDb.Open();
                            re = cmd.ExecuteNonQuery();
                            return re;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return -1;
                }
                finally
                {

                }
            }
            //finally
            //{
            //    semaphore.Release();
            //}
        }
        public void RunSQL(string cmdText, ref DataSet ds)
        {
            //semaphore.Wait(2000);
            lock (lckObj)
            {
                try
                {
                    using (var oleDb = new OracleConnection(myConnectionStr))
                    {
                        using (var da = new OracleDataAdapter(cmdText, oleDb))
                        {
                            oleDb.Open();
                            da.Fill(ds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    Log.Error(cmdText);
                }
                finally
                {

                }

            }
            //finally
            //{
            //    semaphore.Release();
            //}
        }
    }
}
