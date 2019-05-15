using System;
using System.Data;
using System.Threading;
using Microsoft.Data.Sqlite;

namespace Schedule.Common.SqlHelp.Impl
{
    /// <summary>
    /// 提供Sqlite的数据库操作接口实现
    /// </summary>
    public class SqliteSqlOperate : ISqlOperate
    {
        #region //变量定义

        /// <summary>
        /// 数据库连接源
        /// </summary>
        private string myConnectionStr;
        SpinLock slock = new SpinLock(false);
        bool lockTaken = false;
        #endregion

        /// <summary>
        /// 获取或设置数据库连接
        /// </summary>
        public string DbConStr
        {
            get { return this.myConnectionStr; }
            set { myConnectionStr = value; }
        }

        public int RunSQL(string cmdText)
        {
            int re = 0;
            try
            {
                lockTaken = false;
                slock.TryEnter(2000, ref lockTaken);
                if (lockTaken)
                {
                    using (SqliteConnection oleDb = new SqliteConnection(myConnectionStr))
                    {
                        using (SqliteCommand cmd = new SqliteCommand(cmdText, oleDb))
                        {
                            oleDb.Open();
                            re = cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            oleDb.Close();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (lockTaken)
                {
                    slock.Exit(false);
                }
            }
            return re;
        }

        public void RunSQL(string cmdText, ref DataSet ds)
        {
            try
            {
                lockTaken = false;
                slock.TryEnter(2000, ref lockTaken);
                if (lockTaken)
                {
                    using (SqliteConnection oleDb = new SqliteConnection(myConnectionStr))
                    {
                        oleDb.Open();
                        using (SqliteCommand cmd = new SqliteCommand(cmdText, oleDb))
                        {
                            ds = new DataSet();
                            using (SqliteDataReader dr = cmd.ExecuteReader())
                            {
                                do
                                {
                                    DataTable schemaTable = dr.GetSchemaTable();
                                    DataTable dataTable = new DataTable();
                                    if (schemaTable != null)
                                    {
                                        // A query returning records was executed 
                                        for (int i = 0; i < schemaTable.Rows.Count; i++)
                                        {
                                            DataRow dataRow = schemaTable.Rows[i];
                                            string columnName = (string)dataRow["ColumnName"];
                                            DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                                            dataTable.Columns.Add(column);
                                        }
                                        ds.Tables.Add(dataTable);
                                        while (dr.Read())
                                        {
                                            DataRow dataRow = dataTable.NewRow();
                                            for (int i = 0; i < dr.FieldCount; i++)
                                            {
                                                dataRow[i] = dr.GetValue(i);
                                            }
                                            dataTable.Rows.Add(dataRow);
                                        }
                                    }
                                    else
                                    {
                                        DataColumn column = new DataColumn("RowsAffected");
                                        dataTable.Columns.Add(column);
                                        ds.Tables.Add(dataTable);
                                        DataRow dataRow = dataTable.NewRow();
                                        dataRow[0] = dr.RecordsAffected;
                                        dataTable.Rows.Add(dataRow);
                                    }
                                }
                                while (dr.NextResult());

                                dr.Dispose();
                                oleDb.Close();
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (lockTaken)
                {
                    slock.Exit(false);
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
