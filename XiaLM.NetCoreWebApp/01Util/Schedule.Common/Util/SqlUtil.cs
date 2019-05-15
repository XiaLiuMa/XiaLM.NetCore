using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Schedule.Common.Util;

namespace Schedule.Common.SqlHelp
{
    /// <summary>
    /// SQL常用操作
    /// </summary>
    public class SqlUtil
    {
        // public methods...
        /// <summary>
        /// 组装插入SQL语句
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string GetInsertSqlText(string table, Dictionary<string, object> data)
        {
            var sbHead = new StringBuilder();
            var sbValues = new StringBuilder();

            sbHead.Append(" insert into " + table + "(");
            sbValues.Append(" values( ");

            var index = 1;

            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null)
                {
                    if (ObjectUtil.IsString(item.Value.GetType()) || ObjectUtil.IsPrimritive(item.Value.GetType()))
                    {
                        if (index > 1)
                        {
                            sbHead.Append(",");
                            sbValues.Append(",");
                        }
                        sbHead.Append(item.Key);
                        sbValues.Append("'" + item.Value.ToString() + "'");
                    }
                    else
                    {
                        if (ObjectUtil.IsNumbericType(item.Value.GetType()))
                        {
                            if (index > 1)
                            {
                                sbHead.Append(",");
                                sbValues.Append(",");
                            }
                            sbHead.Append(item.Key);
                            sbValues.Append(item.Value.ToString());
                        }
                    }
                    index++;
                }
            }
            sbHead.Append(") ");
            sbValues.Append(") ");
            sbHead.Append(sbValues.ToString());
            return sbHead.ToString();
        }
        /// <summary>
        /// 组装更新的SQL语句
        /// </summary>
        /// <param name="table">表名 </param>
        /// <param name="keys">主键名列表</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string GetUpdateSqlText(string table, string[] keys, Dictionary<string, object> data)
        {
            var sb = new StringBuilder();
            sb.Append("update " + table + " set ");
            var index = 1;



            try
            {
                foreach (KeyValuePair<string, object> item in data)
                {
                    if (item.Value != null)
                    {
                        if (ObjectUtil.IsString(item.Value.GetType()))
                        {
                            if (index > 1)
                            {
                                sb.Append(",");
                            }
                            sb.Append(item.Key + "='" + item.Value.ToString() + "'");
                        }
                        else
                        {
                            if (ObjectUtil.IsNumbericType(item.Value.GetType()))
                            {
                                if (index > 1)
                                {
                                    sb.Append(",");
                                }
                                sb.Append(item.Key + "=" + item.Value.ToString());
                            }
                        }
                        index++;
                    }
                }



                sb.Append(" where ");
                index = 1;
                foreach (string key in keys)
                {
                    if (data.ContainsKey(key))
                    {
                        if (!string.IsNullOrEmpty(data[key] == null ? string.Empty : data[key].ToString()))
                        {
                            if (index > 1)
                            {
                                sb.Append(" and ");
                            }
                            if (ObjectUtil.IsString(data[key].GetType()) || ObjectUtil.IsPrimritive(data[key].GetType()))
                            {
                                sb.Append(key + "='" + data[key].ToString() + "'");
                            }
                            else
                            {
                                if (ObjectUtil.IsNumbericType(data[key].GetType()))
                                {
                                    sb.Append(key + "=" + data[key].ToString());
                                }
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return sb.ToString();
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> Select(string cmdText, ISqlOperate SqlOperate)
        {
            try
            {
                var lst = new List<Dictionary<string, object>>();

                var ds = new DataSet();
                SqlOperate.RunSQL(cmdText, ref ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        var data = new Dictionary<string, object>();
                        foreach (DataColumn dc in ds.Tables[0].Columns)
                        {
                            data.Add(dc.ColumnName.ToUpper(), dr[dc]);
                        }
                        lst.Add(data);
                    }
                }
                return lst;
            }
            catch
            {
                throw;
            }
        }

        static System.Threading.SpinLock spinLock = new System.Threading.SpinLock(false);


        /// <summary>
        /// 将Dictionary的数据更新或插入。暂只支持数据为字符或整形的数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="keys">主键名（小写）</param>
        /// <param name="data">数据值</param>
        /// <param name="SqlOperate">ADO.NET操作对象</param>
        public static void UpdateOrAdd(string table, string[] keys, Dictionary<string, object> data, ISqlOperate SqlOperate)
        {
            ////bool triLock = false;
            try
            {
                //spinLock.TryEnter(2000, ref triLock);
                //if (triLock)
                //{
                var updateCmd = GetUpdateSqlText(table, keys, data);
                if (SqlOperate.RunSQL(updateCmd) > 0)
                {
                    return;
                }
                else
                {
                    var insertCmd = GetInsertSqlText(table, data);
                    SqlOperate.RunSQL(insertCmd);
                }
                //}
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //if (triLock)
                //{
                //    spinLock.Exit();
                //}
            }
        }

        /// <summary>
        /// 对某个对象进行插入或更新操作
        /// </summary>
        /// <param name="table"></param>
        /// <param name="keys"></param>
        /// <param name="data"></param>
        /// <param name="SqlOperate"></param>
        public static void UpdateOrAdd<T>(string table, string[] keys, T obj, ISqlOperate SqlOperate)
        {
            var json = JsonConvert.SerializeObject(obj);
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            UpdateOrAdd(table, keys, data, SqlOperate);
        }
    }
}
