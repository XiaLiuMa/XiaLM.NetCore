using Base.Common.SqlHelp;
using Base.Common.SqlHelp.Impl;
using Base.Common.Utils;
using MsWebAppDal.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MsWebAppDal.Dal
{
    public abstract class AbstractSqliteDal<T> : ISQLDAL<T> where T : BaseEntity, new()
    {
        /// <summary>
        /// 数据库中字符串类型数据（SQL语句中需添加''）
        /// </summary>
        private string[] StringPara = new string[] { "String", "DateTime" };

        #region 新增
        public virtual bool Add(T t)
        {
            bool re = false;
            string[] autos = t.GetDataBaseAuto();
            List<string> lstAdd = new List<string>();
            #region 获取实体类需Add的属性
            StringBuilder strBuilder = new StringBuilder("INSERT INTO ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append("(");
            PropertyInfo[] propertiesTmp = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            for (int j = 0; j < propertiesTmp.Length; j++)
            {
                PropertyInfo item = propertiesTmp[j];
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (!autos.Contains(item.Name))
                    {
                        strBuilder.Append(item.Name);
                        if (j != propertiesTmp.Length - 1)
                        {
                            strBuilder.Append(",");
                        }
                    }
                }
            }

            strBuilder.Append(")");

            strBuilder.Append(" Values(");

            for (int j = 0; j < propertiesTmp.Length; j++)
            {
                PropertyInfo item = propertiesTmp[j];
                string value = item.GetValue(t, null) == null ? "" : item.GetValue(t, null).ToString();
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (!autos.Contains(item.Name))
                    {
                        if (StringPara.Contains(item.PropertyType.Name))
                        {
                            strBuilder.Append("'");
                            strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                            strBuilder.Append("'");
                        }
                        else
                        {
                            strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                        }
                        if (j != propertiesTmp.Length - 1)
                        {
                            strBuilder.Append(",");
                        }
                    }
                }
            }
            strBuilder.Append(")");
            string strSql = strBuilder.ToString();
            SqlOperate.RunSQL(strSql);
            #endregion
            re = true;
            return re;
        }
        #endregion

        #region 更新
        public virtual bool Update(T t)
        {
            bool re = false;
            string[] autos = t.GetDataBaseAuto();
            string[] keys = t.GetKeyValues();
            PropertyInfo[] propertiesTmp = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            List<string> lstUpdate = new List<string>();
            for (int j = 0; j < propertiesTmp.Length; j++)
            {
                PropertyInfo item = propertiesTmp[j];
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (!autos.Contains(item.Name) && !keys.Contains(item.Name))
                    {
                        lstUpdate.Add(item.Name);
                    }
                }
            }

            StringBuilder strBuilder = new StringBuilder("update ");
            strBuilder.Append(t.GetTableName() + " set ");
            int upCount = 0;
            for (int j = 0; j < propertiesTmp.Length; j++)
            {
                PropertyInfo item = propertiesTmp[j];
                string value = item.GetValue(t, null) == null ? "" : item.GetValue(t, null).ToString();
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (lstUpdate.Contains(item.Name))
                    {
                        strBuilder.Append(item.Name);
                        strBuilder.Append("=");
                        if (StringPara.Contains(item.PropertyType.Name))
                        {
                            strBuilder.Append("'");
                            strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                            strBuilder.Append("'");
                        }
                        else
                        {
                            strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                        }
                        upCount++;
                        if (upCount != lstUpdate.Count)
                        {
                            strBuilder.Append(",");
                        }
                    }
                }
            }
            strBuilder.Append(" where ");
            int keyCount = 0;
            for (int j = 0; j < propertiesTmp.Length; j++)
            {
                PropertyInfo item = propertiesTmp[j];
                string value = item.GetValue(t, null) == null ? "" : item.GetValue(t, null).ToString();
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (keys.Contains(item.Name))
                    {
                        strBuilder.Append(item.Name);
                        strBuilder.Append("=");
                        if (StringPara.Contains(item.PropertyType.Name))
                        {
                            strBuilder.Append("'");
                            strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                            strBuilder.Append("'");
                        }
                        else
                        {
                            strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                        }
                        keyCount++;
                        if (keyCount != keys.Length)
                        {
                            strBuilder.Append(" and");
                        }
                    }
                }
            }

            string strSql = strBuilder.ToString();
            SqlOperate.RunSQL(strSql);
            re = true;
            return re;
        }
        /// <summary>
        /// 根据给定集合更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public virtual bool Update(List<T> list)
        {
            bool re = true;
            foreach (var item in list)
            {
                if (!Update(item))
                {
                    re = false;
                    break;
                }
            }
            return re;
        }
        #endregion

        #region 根据实体类删除
        public virtual bool Delete(T t)
        {
            bool re = false;
            StringBuilder strBuilder = new StringBuilder("delete from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" where ");
            PropertyInfo[] propertiesTmp = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            for (int j = 0; j < propertiesTmp.Length; j++)
            {
                PropertyInfo item = propertiesTmp[j];
                string value = item.GetValue(t, null) == null ? "" : item.GetValue(t, null).ToString();
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    strBuilder.Append(item.Name);
                    strBuilder.Append("=");
                    if (StringPara.Contains(item.PropertyType.Name))
                    {
                        strBuilder.Append("'");
                        strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                        strBuilder.Append("'");
                    }
                    else
                    {
                        strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                    }
                    if (j != propertiesTmp.Length - 1)
                    {
                        strBuilder.Append(" and ");
                    }
                }
            }
            string strSql = strBuilder.ToString();
            SqlOperate.RunSQL(strSql);
            re = true;
            return re;
        }
        #endregion

        #region 根据主键删除
        public virtual bool Delete(params string[] keyPara)
        {
            T t = new T();
            bool re = false;
            StringBuilder strBuilder = new StringBuilder("delete from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" where ");
            string[] keys = t.GetKeyValues();
            PropertyInfo[] propertiesTmp = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            for (int i = 0; i < keys.Length; i++)
            {
                strBuilder.Append(keys[i]);
                strBuilder.Append("=");
                bool isString = false;
                #region 判断属性是否为字符串类型
                for (int j = 0; j < propertiesTmp.Length; j++)
                {
                    PropertyInfo item = propertiesTmp[j];
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        if (item.Name == keys[i])
                        {
                            if (StringPara.Contains(item.PropertyType.Name))
                            {
                                isString = true;
                            }
                        }
                    }
                }
                #endregion
                if (isString)
                {
                    strBuilder.Append("'");
                    strBuilder.Append(keyPara[i].Replace("\'", "").Replace("\"", ""));
                    strBuilder.Append("'");
                }
                else
                {
                    strBuilder.Append(keyPara[i].Replace("\'", "").Replace("\"", ""));
                }
                if (i != keys.Length - 1)
                {
                    strBuilder.Append(" and ");
                }
            }

            string strSql = strBuilder.ToString();
            SqlOperate.RunSQL(strSql);
            re = true;
            return re;
        }
        #endregion

        #region 删除所有
        public virtual bool DeleteAll()
        {
            T t = new T();
            bool re = false;
            StringBuilder strBuilder = new StringBuilder("delete from ");
            strBuilder.Append(t.GetTableName());
            string strSql = strBuilder.ToString();
            SqlOperate.RunSQL(strSql);
            re = true;
            return re;
        }
        #endregion

        #region 根据主键查询
        public virtual T Query(params string[] keyPara)
        {
            T t = new T();
            T re = null;
            StringBuilder strBuilder = new StringBuilder("select * from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" where ");
            string[] keys = t.GetKeyValues();
            PropertyInfo[] propertiesTmp = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            for (int i = 0; i < keys.Length; i++)
            {
                strBuilder.Append(keys[i]);
                strBuilder.Append("=");
                bool isString = false;
                #region 判断属性是否为字符串类型
                for (int j = 0; j < propertiesTmp.Length; j++)
                {
                    PropertyInfo item = propertiesTmp[j];
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        if (item.Name == keys[i])
                        {
                            if (StringPara.Contains(item.PropertyType.Name))
                            {
                                isString = true;
                            }
                        }
                    }
                }
                #endregion
                if (isString)
                {
                    strBuilder.Append("'");
                    strBuilder.Append(keyPara[i].Replace("\'", "").Replace("\"", ""));
                    strBuilder.Append("'");
                }
                else
                {
                    strBuilder.Append(keyPara[i].Replace("\'", "").Replace("\"", ""));
                }
                if (i != keys.Length - 1)
                {
                    strBuilder.Append(" and ");
                }
            }

            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                List<T> lstt = datasetutil.DataSetToClassList(ds);
                if (lstt != null && lstt.Count > 0)
                {
                    re = lstt[0];
                }
            }
            return re;
        }
        #endregion

        #region 根据实体类判断是否存在
        public bool IsExist(T t)
        {
            bool re = false;
            StringBuilder strBuilder = new StringBuilder("select * from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" where ");
            PropertyInfo[] propertiesTmp = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            for (int j = 0; j < propertiesTmp.Length; j++)
            {
                PropertyInfo item = propertiesTmp[j];
                string value = item.GetValue(t, null) == null ? "" : item.GetValue(t, null).ToString();
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    strBuilder.Append(item.Name);
                    strBuilder.Append("=");
                    if (StringPara.Contains(item.PropertyType.Name))
                    {
                        strBuilder.Append("'");
                        strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                        strBuilder.Append("'");
                    }
                    else
                    {
                        strBuilder.Append(value.Replace("\'", "").Replace("\"", ""));
                    }
                    if (j != propertiesTmp.Length - 1)
                    {
                        strBuilder.Append(" and ");
                    }
                }
            }
            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            if (ds.Tables.Count > 0)
            {
                re = true;
            }
            return re;
        }
        #endregion

        #region 根据主键判断是否存在
        public virtual bool IsExist(params string[] key)
        {
            if (Query(key) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region 判断坐标中是否存在信息
        public virtual List<T> SelectZbxx()
        {
            T t = new T();
            List<T> lst = new List<T>();
            StringBuilder strBuilder = new StringBuilder("select ID,ZBID,CZYDM_DJZB_X,CZYDM_DJZB_Y,CZYXM_DJZB_X,CZYXM_DJZB_Y,CZYBM_DJZB_X,CZYBM_DJZB_Y,LKZJHM_DJZB_X,LKZJHM_DJZB_Y,LKXM_DJZB_X,LKXM_DJZB_Y,LKPJXX_DJZB_X,LKPJXX_DJZB_Y,LKGJ_DJZB_X,LKGJ_DJZB_Y,JTGJ_DJZB_X,JTGJ_DJZB_Y,ZBMC from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" ORDER BY ID DESC");
            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                lst = datasetutil.DataSetToClassList(ds);
            }
            return lst;
        }
        #endregion


        #region 查询所有
        public virtual List<T> SelectAll()
        {
            T t = new T();
            List<T> lst = new List<T>();
            StringBuilder strBuilder = new StringBuilder("select * from ");
            strBuilder.Append(t.GetTableName());
            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                lst = datasetutil.DataSetToClassList(ds);
            }
            return lst;
        }
        #endregion

        #region 根据条件查询
        public virtual List<T> Select(string condation)
        {
            T t = new T();
            List<T> lst = new List<T>();
            StringBuilder strBuilder = new StringBuilder("select * from ");
            strBuilder.Append(t.GetTableName());
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilder.Append(" where ");
                strBuilder.Append(condation);
            }
            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                lst = datasetutil.DataSetToClassList(ds);
            }
            return lst;
        }
        #endregion

        #region 分页查询
        public virtual bool SelectPage(string condation, int page, int pagesize, out List<T> lst, out int count)
        {
            bool re = false;
            lst = new List<T>();
            count = 0;
            T t = new T();
            StringBuilder strBuilderCount = new StringBuilder("SELECT count(*) as cn from ");
            strBuilderCount.Append(t.GetTableName());
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilderCount.Append(" where ");
                strBuilderCount.Append(condation);
            }

            string strSqlCount = strBuilderCount.ToString();
            DataSet dsCount = new DataSet();
            SqlOperate.RunSQL(strSqlCount, ref dsCount);
            if (dsCount.Tables.Count > 0)
            {
                count = Convert.ToInt32(dsCount.Tables[0].Rows[0]["cn"].ToString());
            }

            StringBuilder strBuilder = new StringBuilder("SELECT TOP ");
            strBuilder.Append(pagesize.ToString());
            strBuilder.Append(" * from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" where ");
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilder.Append(condation);
                strBuilder.Append(" and ");
            }
            strBuilder.Append("(Id>=(SELECT MAX(Id) FROM (SELECT TOP ");
            strBuilder.Append(((page - 1) * pagesize + 1).ToString());
            strBuilder.Append(" Id from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" ");
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilder.Append(" where ");
                strBuilder.Append(condation);
            }
            strBuilder.Append(" ORDER BY Id) AS T))ORDER BY Id");

            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                lst = datasetutil.DataSetToClassList(ds);
            }
            re = true;
            return re;
        }
        #endregion

        #region 通过添加时间分页查询
        public virtual bool SelectPageByTime(string condation, int page, int pagesize, out List<T> lst, out int count)
        {
            bool re = false;
            lst = new List<T>();
            count = 0;
            T t = new T();
            StringBuilder strBuilderCount = new StringBuilder("SELECT count(*) as cn from ");
            strBuilderCount.Append(t.GetTableName());
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilderCount.Append(" where ");
                strBuilderCount.Append(condation);
            }

            string strSqlCount = strBuilderCount.ToString();
            DataSet dsCount = new DataSet();
            SqlOperate.RunSQL(strSqlCount, ref dsCount);
            if (dsCount.Tables.Count > 0)
            {
                count = Convert.ToInt32(dsCount.Tables[0].Rows[0]["cn"].ToString());
            }

            StringBuilder strBuilder = new StringBuilder("SELECT TOP ");
            strBuilder.Append(pagesize.ToString());
            strBuilder.Append(" * from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" where ");
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilder.Append(condation);
                strBuilder.Append(" and ");
            }
            strBuilder.Append(" Id <=(SELECT MAX(Id) FROM (SELECT TOP ");
            strBuilder.Append((count - (page - 1) * pagesize).ToString());
            strBuilder.Append(" Id from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" ");
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilder.Append(" where ");
                strBuilder.Append(condation);
            }
            strBuilder.Append(" ORDER BY Id asc))ORDER BY Id desc");

            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                lst = datasetutil.DataSetToClassList(ds);
            }
            re = true;
            return re;
        }
        #endregion

        #region ISqlOperate
        private static ISqlOperate sqlOperate;
        public ISqlOperate SqlOperate
        {
            get
            {
                if (sqlOperate == null)
                {
                    sqlOperate = new SqliteSqlOperate()
                    {
                        DbConStr = $@"Data Source={AppContext.BaseDirectory}db/MsIsolator.db"
                    };
                }
                return sqlOperate;
            }
        }
        #endregion

        #region 批量执行删除语句
        /// <summary>
        /// 批量执行删除语句
        /// </summary>
        /// <param name="LstCondation">删除条件集合</param>
        /// <returns>删除是否成功(true:成功；false:失败；)</returns>
        public bool Delete(List<string> LstCondation)
        {
            bool re = false;
            if (LstCondation != null && LstCondation.Count > 0)
            {
                T t = new T();
                for (int i = 0; i < LstCondation.Count; i++)
                {
                    StringBuilder strBuilder = new StringBuilder();
                    string con = LstCondation[i];
                    strBuilder.Append("delete from ");
                    strBuilder.Append(t.GetTableName());
                    if (!string.IsNullOrEmpty(con))
                    {
                        strBuilder.Append(" where ");
                        strBuilder.Append(con);
                    }

                    string strSql = strBuilder.ToString();
                    SqlOperate.RunSQL(strSql);
                }
            }
            re = true;
            return re;
        }
        #endregion

        /// <summary>
        /// 自定义查询语句，只能查询某一条数据。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> ExQuery(string sql)
        {
            T t = new T();
            List<T> lst = new List<T>();
            string strSql = sql;
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                lst = datasetutil.DataSetToClassList(ds);
            }
            return lst;
        }

        public bool SelectPageEx(string condation, string order, int page, int pagesize, out List<T> lst, out int count)
        {
            bool re = false;
            lst = new List<T>();
            count = 0;
            T t = new T();
            StringBuilder strBuilderCount = new StringBuilder("SELECT count(*) as cn from ");
            strBuilderCount.Append(t.GetTableName());
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilderCount.Append(" where ");
                strBuilderCount.Append(condation);
            }

            string strSqlCount = strBuilderCount.ToString();
            DataSet dsCount = new DataSet();
            SqlOperate.RunSQL(strSqlCount, ref dsCount);
            if (dsCount.Tables.Count > 0)
            {
                count = Convert.ToInt32(dsCount.Tables[0].Rows[0]["cn"].ToString());
            }

            StringBuilder strBuilder = new StringBuilder("SELECT TOP ");
            strBuilder.Append(pagesize.ToString());
            strBuilder.Append(" * from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" where ");
            strBuilder.Append("(Id>=(SELECT MAX(Id) FROM (SELECT TOP ");
            strBuilder.Append(((page - 1) * pagesize + 1).ToString());
            strBuilder.Append(" Id from ");
            strBuilder.Append(t.GetTableName());
            strBuilder.Append(" ");
            if (!string.IsNullOrEmpty(condation))
            {
                strBuilder.Append(" where ");
                strBuilder.Append(condation);
            }
            if (string.IsNullOrEmpty(order))
            {
                strBuilder.Append(" ORDER BY Id) AS T))ORDER BY Id");
            }
            else
            {
                strBuilder.Append(" ORDER BY Id) AS T))" + order + ",Id");
            }

            string strSql = strBuilder.ToString();
            DataSet ds = new DataSet();
            SqlOperate.RunSQL(strSql, ref ds);
            DataSetUtil<T> datasetutil = new DataSetUtil<T>();
            if (ds.Tables.Count > 0)
            {
                lst = datasetutil.DataSetToClassList(ds);
            }
            re = true;
            return re;
        }
    }
}
