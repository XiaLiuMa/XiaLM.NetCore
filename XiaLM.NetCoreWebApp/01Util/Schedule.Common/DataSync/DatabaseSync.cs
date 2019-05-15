using Base.Common.LogHelp;
using Base.Common.Utils;
using Schedule.Common.DataSync.Mod;
using Schedule.Common.SqlHelp;
using Schedule.Common.SqlHelp.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Common.DataSync
{
    /// <summary>
    /// 数据库同步
    /// </summary>
    public class DatabaseSync
    {
        ///// <summary>
        ///// 工作员上台缓存
        ///// </summary>
        //public static Queue QuTdssxx = new Queue();

        //public DatabaseSync()
        //{
        //    QuTdssxx = Queue.Synchronized(QuTdssxx);
        //}

        public static void SendOneTime(List<Dictionary<string, object>> lst, byte cmd, ISqlOperate sql, string sqlitePath)
        {
            switch (cmd)
            {
                case 0x38:  //国家地区代码数据同步
                    {
                        Parallel.ForEach(lst, dic =>
                        {
                            #region 国家地区代码入库处理
                            if (dic.ContainsKey("YWMC") && dic["YWMC"] != null)
                            {
                                string value = dic["YWMC"].ToString();
                                value = value.Replace("'", "''".ToString());
                                dic["YWMC"] = value;
                            }
                            #endregion
                            SqlUtil.UpdateOrAdd("QWSJ_D_GJDQDM", new string[] { "GJDQ3" }, dic, sql);
                        });
                    }
                    break;
                case 0x01:  //检查员上台
                    {
                        Parallel.ForEach(lst, dic =>
                        {
                            Jcystcl(dic, sql);
                        });
                    }
                    break;
                case 0x02:  //检查员下台
                    {
                        Parallel.ForEach(lst, dic =>
                        {
                            Jcyxtcl(dic, sql);
                        });
                    }
                    break;
                default:
                    {
                        if (lst != null && lst.Count < 1) return;
                        try
                        {
                            SqliteSqlOperate sqliteSql = new SqliteSqlOperate()
                            {
                                DbConStr = $@"Data Source={sqlitePath}"
                            };
                            DataSet ds = new DataSet();
                            string sqlStr = $"Select * from DbCmdConfig where Cmd='{cmd}'";
                            sqliteSql.RunSQL(sqlStr, ref ds);
                            string table = (string)ds.Tables[0].Rows[0][ds.Tables[0].Columns[1]];
                            string keyStr = (string)ds.Tables[0].Rows[0][ds.Tables[0].Columns[2]];
                            string[] keys = keyStr.Split(',');  //逗号分隔
                            MergeHelper.AddMergeParallelLst(lst, $"tmp_{table}", table, keys, sql);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex.ToString());
                        }
                    }
                    break;
            }
        }


        /// <summary>
        /// 检查员上台处理
        /// </summary>
        /// <returns></returns>
        private static void Jcystcl(Dictionary<string, object> receive, ISqlOperate sql)
        {
            //string vaKey = receive["VALKEY"].ToString();
            //if (QuTdssxx.Count > 100)
            //{
            //    QuTdssxx.Dequeue();
            //}

            //if (QuTdssxx.Count > 0 && QuTdssxx.Contains(vaKey))
            //{
            //    return;
            //}
            //QuTdssxx.Enqueue(vaKey);

            string wybs = receive["WYBS"].ToString();
            string yfzt = "0";
            if (!string.IsNullOrEmpty(wybs))
            {
                yfzt = "1";
            }
            //推送信息赋值
            TdssxxtjBroadCastMsg castMsg = new TdssxxtjBroadCastMsg();
            castMsg.Wybs = receive["WYBS"].ToString();
            castMsg.Tdh = receive["TDH"].ToString();
            castMsg.Ktzt = receive["GZZZT"].ToString();
            castMsg.Yfzt = yfzt;
            DateTime stsj = DateTime.ParseExact(receive["CZYZCSJ"].ToString(), "yyyyMMddHHmmss", null);
            DateTime ztsj = DateTime.Now;
            TimeSpan ts = stsj.Subtract(ztsj).Duration();
            castMsg.Ztsj = ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟";
            castMsg.Jcy = receive["CZYXM"].ToString();
            castMsg.Stsj = stsj.ToString("yyyy-MM-dd HH:mm:ss");
            Dictionary<string, object> receiveTj = StatisticsPjbjs(receive, sql);
            if (receiveTj == null)
            {
                castMsg.Gj = "0";
                castMsg.Zjzl = "0";
                castMsg.Yfrs = "0";
            }
            else
            {
                castMsg.Yfrs = receiveTj["YFS"].ToString();
                receive["YFS"] = receiveTj["YFS"].ToString();
            }
            castMsg.Lkxm = receive["XM"].ToString();
            castMsg.Xb = receive["XBDM"].ToString();
            castMsg.Csrq = receive["CSRQ"].ToString();
            castMsg.Zjhm = receive["ZJHM"].ToString();
            castMsg.Crjlx = receive["CRBZ"].ToString();
            castMsg.Ssbm = receive["BMMC"].ToString();
            castMsg.Zjzl = GetZJLBMC(receive["ZJLBDM"].ToString(), sql);
            castMsg.Gj = GetGJDQMC(receive["GJDQDM"].ToString(), sql);

            try
            {
                T_CRJRY crjry = JsonUtil.StrToObject<T_CRJRY>(JsonUtil.ObjectToStr(receive));

                if (!string.IsNullOrEmpty(crjry.WYBS))
                {
                    SqlUtil.UpdateOrAdd<T_CRJRY>("QWSJ_T_CRJRY", new string[] { "WYBS" }, crjry, sql);
                }

                T_TDSSJK tdssjk = JsonUtil.StrToObject<T_TDSSJK>(JsonUtil.ObjectToStr(receive));

                SqlUtil.UpdateOrAdd<T_TDSSJK>("QWSJ_T_TDSSJK", new string[] { "TDH" }, tdssjk, sql);

                receive.Add("ZTSJ", castMsg.Ztsj);
                receive.Add("YFRS", castMsg.Yfrs);
                receive.Add("JCY", castMsg.Jcy);
                receive.Add("GJ", castMsg.Gj);
                receive.Add("ZJZL", castMsg.Zjzl);
                receive.Add("XB", castMsg.Xb);
                receive.Add("KTZT", "2");
                T_TDSSTJ tdsstj = JsonUtil.StrToObject<T_TDSSTJ>(JsonUtil.ObjectToStr(receive));

                SqlUtil.UpdateOrAdd<T_TDSSTJ>("QWSJ_T_TDSSTJ", new string[] { "TDH" }, tdsstj, sql);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region 上台处理
        /// <summary>
        /// 统计评价报警数、业务报警数，查询国家地区代码
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, object> StatisticsPjbjs(Dictionary<string, object> receive, ISqlOperate sql)
        {
            string strSql = "select count(f.wybs) as yfs from QWSJ_T_CRJRY f left join QWSJ_T_TDSSJK a on f.TDH=a.TDH  where  " +
           "f.CRRQSJ>'" + receive["CZYZCSJ"].ToString() + "' and f.TDH='" + receive["TDH"].ToString() + "'";

            List<Dictionary<string, object>> list = SqlUtil.Select(strSql, sql);
            if (list.Count <= 0) return null;
            return (Dictionary<string, object>)list[0];
        }

        /// <summary>
        /// 根据证件类别代码获取证件类别名称
        /// </summary>
        /// <param name="tdh"></param>
        /// <returns></returns>
        public static string GetZJLBMC(string zjlbdm, ISqlOperate sql)
        {
            string strSql = @"SELECT ZJLBDM,ZJLBMC FROM QWSJ_D_ZJLBDM";
            DataSet ds = new DataSet();
            sql.RunSQL(strSql, ref ds);

            DataSetUtil<ZJZL> dSet = new DataSetUtil<ZJZL>();
            List<ZJZL> zJZLs = dSet.DataSetToClassList(ds);

            ZJZL zj = zJZLs.FirstOrDefault(a => a.ZJLBDM.Equals(zjlbdm));

            return zj == null ? "" : zj.ZJLBMC;
        }

        /// <summary>
        /// 根据国家地区代码获取国家地区名称
        /// </summary>
        /// <param name="tdh"></param>
        /// <returns></returns>
        public static string GetGJDQMC(string gjdqdm, ISqlOperate sql)
        {
            string strSql = @"SELECT GJDQ3,GJDQMC FROM QWSJ_D_GJDQDM";
            DataSet ds = new DataSet();
            sql.RunSQL(strSql, ref ds);

            DataSetUtil<GJDQDMMC> dSet = new DataSetUtil<GJDQDMMC>();
            List<GJDQDMMC> gJDQDMs = dSet.DataSetToClassList(ds);

            GJDQDMMC gj = gJDQDMs.FirstOrDefault(a => a.GJDQ3.Equals(gjdqdm));

            return gj == null ? "" : gj.GJDQMC;
        }
        #endregion

        /// <summary>
        /// 检查员下台处理
        /// </summary>
        /// <param name="receive"></param>
        private static void Jcyxtcl(Dictionary<string, object> receive, ISqlOperate sql)
        {
            //object vaKey = receive["VALKEY"];
            //if (QuTdssxx.Count > 100)
            //{
            //    QuTdssxx.Dequeue();
            //}

            //if (QuTdssxx.Count > 0 && QuTdssxx.Contains(vaKey))
            //{
            //    return;
            //}
            //QuTdssxx.Enqueue(vaKey.ToString());
            receive.Add("LKXM", "");
            T_TDSSTJ tdsstj = JsonUtil.StrToObject<T_TDSSTJ>(JsonUtil.ObjectToStr(receive));
            tdsstj.KTZT = "1";
            SqlUtil.UpdateOrAdd<T_TDSSTJ>("QWSJ_T_TDSSTJ", new string[] { "TDH" }, tdsstj, sql);

            T_TDSSJK tdssjk = JsonUtil.StrToObject<T_TDSSJK>(JsonUtil.ObjectToStr(receive));
            SqlUtil.UpdateOrAdd<T_TDSSJK>("QWSJ_T_TDSSJK", new string[] { "TDH" }, tdssjk, sql);

        }
    }
}
