using Base.Common.LogHelp;
using Base.Common.Utils;
using Schedule.Common.SqlHelp.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Common.DataSync
{
    /// <summary>
    /// 文件同步
    /// </summary>
    public class FileSync
    {
        public static void SendOneTime(List<Dictionary<string, object>> lst, string jobname, byte cmd, string sqlitePath)
        {
            if (lst != null && lst.Count < 1) return;

            SqliteSqlOperate sqliteSql = new SqliteSqlOperate()
            {
                DbConStr = $@"Data Source={sqlitePath}"
            };
            DataSet ds = new DataSet();
            string sqlStr = $"Select * from DbCmdConfig where Cmd='{cmd}'";
            
            try
            {
                sqliteSql.RunSQL(sqlStr, ref ds);
                string table = (string)ds.Tables[0].Rows[0][ds.Tables[0].Columns[1]];
                string fpath = $"{AppContext.BaseDirectory}SyncFiles/{table},{jobname},{cmd}.txt";//保存路径

                if (File.Exists(fpath)) File.Delete(fpath);
                string str1 = JsonUtil.ObjectToStr(lst);
                File.WriteAllText(fpath, JsonUtil.ObjectToStr(lst));
                string str2 = File.ReadAllText(fpath);
                List<Dictionary<string, object>> tlist = JsonUtil.StrToObject<List<Dictionary<string, object>>>(str2);

                #region 上传到FTP服务器
                //        //foreach (var ftpItem in ConfigProvider.GetInstance().ftpConfigs)
                //        //{
                //        //    string dir = $@"{time.Substring(0, 4)}/{time.Substring(4, 2)}/{time.Substring(6, 2)}/{zjhm}/";
                //        //    var ftp = new FtpUtil($@"ftp://{ftpItem.Ip}:{ftpItem.Port}/", ftpItem.Uname, ftpItem.Pwd);
                //        //    foreach (var item in flies)
                //        //    {
                //        //        if (File.Exists(item))
                //        //        {
                //        //            if (ftp.Upload(item, dir, Path.GetFileName(item), FtpOption.create))
                //        //            {
                //        //                File.Delete(item);
                //        //            }
                //        //        }
                //        //    }
                //        //}
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            #region 废弃
            ////string vStr = "";   //存储到文件的字符串
            //foreach (var dict in lst)
            //{
            //    //string zjhm = "";  //证件号
            //    //string time = "";  //时间
            //    //string dic = AppContext.BaseDirectory + "SyncFiles";    //文件保存目录
            //    //Dictionary<string, object> keyv1 = new Dictionary<string, object>();    //信息字典
            //    //Dictionary<string, object> keyv2 = new Dictionary<string, object>();    //图片字典

            //    foreach (KeyValuePair<string, object> item in dict)
            //    {
            //        try
            //        {
            //            if (item.Key.StartsWith("TP_")) //图片
            //            {
            //                keyv2.Add(item.Key.Remove(0, 3), item.Value);
            //            }
            //            else if (item.Key.StartsWith("HM_")) //证件号码
            //            {
            //                zjhm = item.Value is System.DBNull ? "" : (string)item.Value;
            //                keyv1.Add(item.Key.Remove(0, 3), item.Value);
            //            }
            //            else if (item.Key.StartsWith("SJ_")) //时间
            //            {
            //                time = item.Value is System.DBNull ? "" : (string)item.Value;
            //                keyv1.Add(item.Key.Remove(0, 3), item.Value);
            //            }
            //            else
            //            {
            //                keyv1.Add(item.Key, item.Value);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.Error(ex.ToString());
            //        }
            //    }

            //    Task.Factory.StartNew(() =>
            //    {
            //        List<string> flies = new List<string>();    //要上传的文件路径

            //        #region 保存图片
            //        string xtxt = "";  //信息
            //        foreach (KeyValuePair<string, object> item in keyv1)
            //        {
            //            string xvalue = item.Value is System.DBNull ? "" : (string)item.Value;
            //            xtxt += $"{xvalue}\r\n";
            //        }
            //        string xpath = $@"{dic}/{zjhm}_RYZL_{time}.txt"; //人员资料
            //        if (File.Exists(xpath)) File.Delete(xpath);
            //        if (!string.IsNullOrEmpty(xtxt))
            //        {
            //            File.WriteAllText(xpath, xtxt.Trim());
            //            flies.Add(xpath);
            //        }

            //        foreach (KeyValuePair<string, object> item in keyv2)
            //        {
            //            byte[] tbytes = null;    //图片数据
            //            string tpath = $@"{dic}/{zjhm}_{item.Key}_{time}.jpg"; //图片临时保存地址
            //            if (!(item.Value is System.DBNull)) tbytes = (byte[])item.Value;
            //            if (File.Exists(tpath)) File.Delete(tpath);
            //            if (tbytes != null)
            //            {
            //                File.WriteAllBytes(tpath, tbytes);
            //                flies.Add(xpath);
            //            }
            //        }
            //        #endregion

            //        #region 上传到FTP服务器
            //        //foreach (var ftpItem in ConfigProvider.GetInstance().ftpConfigs)
            //        //{
            //        //    string dir = $@"{time.Substring(0, 4)}/{time.Substring(4, 2)}/{time.Substring(6, 2)}/{zjhm}/";
            //        //    var ftp = new FtpUtil($@"ftp://{ftpItem.Ip}:{ftpItem.Port}/", ftpItem.Uname, ftpItem.Pwd);
            //        //    foreach (var item in flies)
            //        //    {
            //        //        if (File.Exists(item))
            //        //        {
            //        //            if (ftp.Upload(item, dir, Path.GetFileName(item), FtpOption.create))
            //        //            {
            //        //                File.Delete(item);
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //        #endregion
            //    });
            //} 
            #endregion

        }
    }
}
