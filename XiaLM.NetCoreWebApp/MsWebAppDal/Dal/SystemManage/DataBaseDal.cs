using MsWebAppDal.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MsWebAppDal.Dal.SystemManage
{
    public class DataBaseDal : AbstractSqliteDal<DataBase>
    {
        /// <summary>
        /// 判断其他字段是否有重复值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsExistOther(string str)
        {
            bool b = false;
            try
            {
                string strSql = string.Format("select * from DataBaseConfig where 1=1 and {0} ", str);
                DataSet ds = new DataSet();
                SqlOperate.RunSQL(strSql, ref ds);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            b = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return b;
        }
    }
}
