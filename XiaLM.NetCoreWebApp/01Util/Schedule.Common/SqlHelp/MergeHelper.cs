using Base.Common.LogHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Common.SqlHelp
{
    /// <summary>
    /// Merge操作类(批量执行（不存在插入，存在更新)操作）
    /// </summary>
    public class MergeHelper
    {
        static int MaxAddCount = 1000;

        /// <summary>
        /// 批量执行（不存在插入，存在更新)操作
        /// </summary>
        /// <typeparam name="T">执行实体类</typeparam>
        /// <param name="lst">执行实体类集合</param>
        /// <param name="tmpTableName">临时表名称</param>
        /// <param name="tableName">表名称</param>
        /// <param name="keys">表主键</param>
        /// <param name="sqlOperate">sql操作类</param>
        public static void AddMergeParallelLst<T>(List<T> lst, string tmpTableName, string tableName, string[] keys, ISqlOperate sqlOperate) where T : new()
        {
            try
            {
                List<List<T>> LstUsers = new List<List<T>>();

                #region sql语句中insert最大允许1000条数据，所以如果插入的数据大于1000条时需要分割
                if (lst != null && lst.Count >= MaxAddCount)
                {
                    for (int i = 0; i < lst.Count; i += MaxAddCount)
                    {
                        List<T> lstt = new List<T>();
                        lstt = lst.Take(i + MaxAddCount).Skip(i).ToList();
                        LstUsers.Add(lstt);
                    }
                }
                else
                {
                    LstUsers.Add(lst);
                }
                #endregion

                #region 获取实体类属性及更新属性信息
                List<string> lstProperties = new List<string>();
                List<string> lstUpdate = new List<string>();
                T tTmp = lst[0];
                PropertyInfo[] propertiesTmp = tTmp.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                for (int j = 0; j < propertiesTmp.Length; j++)
                {
                    PropertyInfo item = propertiesTmp[j];
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        lstProperties.Add(item.Name);
                        if (!keys.Contains(item.Name))
                        {
                            lstUpdate.Add(item.Name);
                        }
                    }
                    else
                    {

                    }
                }
                #endregion

                #region 避免临时表有数据，导致主键重复错误，先删除临时表数据
                string strDelete = "delete from " + tmpTableName;
                int re = sqlOperate.RunSQL(strDelete);
                #endregion

                #region 并行循环拼接插入语句，将数据插入到临时表
                Parallel.For(0, LstUsers.Count, i =>
                {
                    List<string> SQLStringList = new List<string>();
                    StringBuilder strBuilder = new StringBuilder("INSERT INTO " + tmpTableName + " Values");

                    #region 动态获取实体类属性值
                    for (int m = 0; m < LstUsers[i].Count; m++)
                    {
                        T t = LstUsers[i][m];
                        strBuilder.Append("(");
                        PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                        for (int j = 0; j < properties.Length; j++)
                        {
                            PropertyInfo item = properties[j];
                            string value = item.GetValue(t, null).ToString();
                            if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                            {
                                if (j != 0)
                                {
                                    strBuilder.Append(",");
                                }
                                strBuilder.Append("'");
                                strBuilder.Append(value.Replace("\'", "").Replace("\"", "").Replace(":", "").Replace(".", ""));
                                strBuilder.Append("'");
                            }
                            else
                            {

                            }
                        }
                        if (m == LstUsers[i].Count - 1)
                        {
                            strBuilder.Append(")");
                        }
                        else
                        {
                            strBuilder.Append("),");
                        }
                    };
                    #endregion

                    string cmdTextAddTemp = strBuilder.ToString();
                    int reAddTemp = sqlOperate.RunSQL(cmdTextAddTemp);
                });
                #endregion

                #region 动态拼接merge语句
                StringBuilder strBuilderMerge = new StringBuilder("MERGE ");
                strBuilderMerge.Append(tableName);
                strBuilderMerge.Append(" as target1 USING ");
                strBuilderMerge.Append(tmpTableName);
                strBuilderMerge.Append(" as source1 on ");
                for (int k = 0; k < keys.Length; k++)
                {
                    string key = keys[k];
                    if (k != 0)
                    {
                        strBuilderMerge.Append(" and ");
                    }
                    strBuilderMerge.Append("target1.");
                    strBuilderMerge.Append(key);
                    strBuilderMerge.Append("=source1.");
                    strBuilderMerge.Append(key);
                }
                strBuilderMerge.Append(" WHEN MATCHED THEN update set ");

                for (int l = 0; l < lstUpdate.Count; l++)
                {
                    if (l != 0)
                    {
                        strBuilderMerge.Append(",");
                    }
                    string name = lstUpdate[l];
                    strBuilderMerge.Append("target1.");
                    strBuilderMerge.Append(name);
                    strBuilderMerge.Append("=source1.");
                    strBuilderMerge.Append(name);
                }

                strBuilderMerge.Append(" WHEN NOT MATCHED THEN insert values(");

                for (int j = 0; j < lstProperties.Count; j++)
                {
                    if (j != 0)
                    {
                        strBuilderMerge.Append(",");
                    }
                    strBuilderMerge.Append("source1.");
                    strBuilderMerge.Append(lstProperties[j]);
                }

                strBuilderMerge.Append(");delete from ");
                strBuilderMerge.Append(tmpTableName);
                strBuilderMerge.Append(";");

                string strCmdTextMerge = strBuilderMerge.ToString();
                int reMerge = sqlOperate.RunSQL(strCmdTextMerge);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private readonly static object lockObj = new object();

        /// <summary>
        /// 批量执行（不存在插入，存在更新)操作
        /// </summary>
        /// <typeparam name="T">执行实体类</typeparam>
        /// <param name="lst">执行实体类集合</param>
        /// <param name="tmpTableName">临时表名称</param>
        /// <param name="tableName">表名称</param>
        /// <param name="keys">表主键</param>
        /// <param name="sqlOperate">sql操作类</param>
        public static void AddMergeParallelLst(List<Dictionary<string, object>> lst, string tmpTableName, string tableName, string[] keys, ISqlOperate sqlOperate)
        {
            string strCmdTextMerge = string.Empty;
            try
            {
                int re = -1;
                List<List<Dictionary<string, object>>> LstUsers = new List<List<Dictionary<string, object>>>();

                #region sql语句中insert最大允许1000条数据，所以如果插入的数据大于1000条时需要分割
                if (lst != null && lst.Count >= MaxAddCount)
                {
                    for (int i = 0; i < lst.Count; i += MaxAddCount)
                    {
                        List<Dictionary<string, object>> lstt = new List<Dictionary<string, object>>();
                        lstt = lst.Take(i + MaxAddCount).Skip(i).ToList();
                        LstUsers.Add(lstt);
                    }
                }
                else
                {
                    LstUsers.Add(lst);
                }
                #endregion

                #region 获取实体类属性及更新属性信息
                List<string> lstProperties = new List<string>();
                List<string> lstUpdate = new List<string>();
                Dictionary<string, object> dicPro = LstUsers[0][0];
                foreach (KeyValuePair<string, object> pro in dicPro)
                {
                    lstProperties.Add(pro.Key);
                    if (!keys.Contains(pro.Key))
                    {
                        lstUpdate.Add(pro.Key);
                    }
                }
                #endregion

                #region 判断tmp临时表是否存在，不存在则创建会话临时表

                string sqlCreatTable = $@"if object_id(N'{tmpTableName}',N'U') is null select * into {tmpTableName} from {tableName} where 1<>1 ";

                re = sqlOperate.RunSQL(sqlCreatTable);
                #endregion

                #region 避免临时表有数据，导致主键重复错误，先删除临时表数据
                string strDelete = "delete from " + tmpTableName;
                re = sqlOperate.RunSQL(strDelete);
                #endregion

                #region 并行循环拼接插入语句，将数据插入到临时表
                Parallel.For(0, LstUsers.Count, i =>
                {
                    List<string> SQLStringList = new List<string>();
                    StringBuilder strBuilder = new StringBuilder("INSERT INTO ");
                    strBuilder.Append(tmpTableName);
                    strBuilder.Append(" (");
                    for (int l = 0; l < lstProperties.Count; l++)
                    {
                        strBuilder.Append(lstProperties[l]);
                        if (l != lstProperties.Count - 1)
                        {
                            strBuilder.Append(",");
                        }
                    }
                    strBuilder.Append(") ");
                    strBuilder.Append(" Values");

                    #region 动态获取实体类属性值
                    for (int m = 0; m < LstUsers[i].Count; m++)
                    {
                        Dictionary<string, object> t = LstUsers[i][m];
                        strBuilder.Append("(");
                        int j = 0;
                        foreach (KeyValuePair<string, object> p in t)
                        {
                            string value = p.Value == null ? "0" : p.Value.ToString();
                            if (j != 0)
                            {
                                strBuilder.Append(",");
                            }
                            strBuilder.Append("'");
                            strBuilder.Append(value.Replace("\'", "").Replace("\"", "").Replace(":", "").Replace(".", ""));
                            strBuilder.Append("'");
                            j++;
                        }
                        if (m == LstUsers[i].Count - 1)
                        {
                            strBuilder.Append(")");
                        }
                        else
                        {
                            strBuilder.Append("),");
                        }
                    };
                    #endregion

                    string cmdTextAddTemp = strBuilder.ToString();
                    int reAddTemp = sqlOperate.RunSQL(cmdTextAddTemp);
                });
                #endregion

                #region 动态拼接merge语句
                StringBuilder strBuilderMerge = new StringBuilder("MERGE ");
                strBuilderMerge.Append(tableName);
                strBuilderMerge.Append(" as target1 USING ");
                strBuilderMerge.Append(tmpTableName);
                strBuilderMerge.Append(" as source1 on ");
                for (int k = 0; k < keys.Length; k++)
                {
                    string key = keys[k];
                    if (k != 0)
                    {
                        strBuilderMerge.Append(" and ");
                    }
                    strBuilderMerge.Append("target1.");
                    strBuilderMerge.Append(key);
                    strBuilderMerge.Append("=source1.");
                    strBuilderMerge.Append(key);
                }
                strBuilderMerge.Append(" WHEN MATCHED THEN update set ");

                for (int l = 0; l < lstUpdate.Count; l++)
                {
                    if (l != 0)
                    {
                        strBuilderMerge.Append(",");
                    }
                    string name = lstUpdate[l];
                    strBuilderMerge.Append("target1.");
                    strBuilderMerge.Append(name);
                    strBuilderMerge.Append("=source1.");
                    strBuilderMerge.Append(name);
                }

                string insertItem = ""; //要插入的字段
                for (int i = 0; i < lstProperties.Count; i++)
                {
                    if (i == lstProperties.Count - 1)
                    {
                        insertItem += $"{lstProperties[i]}";
                    }
                    else
                    {
                        insertItem += $"{lstProperties[i]},";
                    }
                }

                strBuilderMerge.Append($" WHEN NOT MATCHED THEN insert({insertItem}) values(");

                for (int j = 0; j < lstProperties.Count; j++)
                {
                    if (j != 0)
                    {
                        strBuilderMerge.Append(",");
                    }
                    strBuilderMerge.Append("source1.");
                    strBuilderMerge.Append(lstProperties[j]);
                }

                strBuilderMerge.Append(");delete from ");
                strBuilderMerge.Append(tmpTableName);
                strBuilderMerge.Append(";");

                strCmdTextMerge = strBuilderMerge.ToString();
                int reMerge = sqlOperate.RunSQL(strCmdTextMerge);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                Log.Info(strCmdTextMerge);
            }
        }
    }
}
