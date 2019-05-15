using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Common.DataSync.Mod
{
    /// <summary>
    /// 通道实时信息与当前旅客通道信息推送
    /// </summary>
    [Serializable]
    public class TdssxxtjBroadCastMsg
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Wybs = "0";

        /// <summary>
        /// 通道号
        /// </summary>
        public string Tdh = "0";

        /// <summary>
        /// 开通状态
        /// </summary>
        public string Ktzt = "0";

        /// <summary>
        /// 验放状态
        /// </summary>
        public string Yfzt = "0";

        /// <summary>
        /// 在台时间
        /// </summary>
        public string Ztsj = "0";

        /// <summary>
        /// 操作员代码
        /// </summary>
        public string Czydm = "0";

        /// <summary>
        /// 检查员
        /// </summary>
        public string Jcy = "0";

        /// <summary>
        /// 所属部门
        /// </summary>
        public string Ssbm = "0";

        /// <summary>
        /// 上台时间
        /// </summary>
        public string Stsj = "0";

        /// <summary>
        /// 验放人数
        /// </summary>
        public string Yfrs = "0";

        /// <summary>
        /// 报警次数
        /// </summary>
        public string Bjcs = "0";

        /// <summary>
        /// 不满意次数
        /// </summary>
        public string Bmycs = "0";

        /// <summary>
        /// 旅客姓名
        /// </summary>
        public string Lkxm = "0";

        /// <summary>
        /// 性别
        /// </summary>
        public string Xb = "0";

        /// <summary>
        /// 国籍
        /// </summary>
        public string Gj = "0";


        /// <summary>
        /// 出生日期
        /// </summary>
        public string Csrq = "0";

        /// <summary>
        /// 证件种类
        /// </summary>
        public string Zjzl = "0";

        /// <summary>
        /// 证件号码
        /// </summary>
        public string Zjhm = "0";

        /// <summary>
        /// 出入境类型
        /// </summary>
        public string Crjlx = "0";

    }
}
