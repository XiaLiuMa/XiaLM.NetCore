using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Common.DataSync.Mod
{
    /// <summary>
    /// 出入境人员表
    /// </summary>
    [Serializable]
    public partial class T_CRJRY
    {
        #region Model
        private string _rylbdm;
        private string _xm;
        private string _gjdqdm;
        private string _xbdm;
        private string _csrq;
        private string _zjlbdm;
        private string _zjhm;
        private string _qzzldm;
        private string _crrq;
        private string _crsj;
        private string _crrqsj;
        private string _crkadm;
        private string _jtfsdm;
        private string _jtgjbs;
        private string _qwgjdqdm;
        private string _fzd;
        private string _crjsydm;
        private string _czy;
        private string _tdh;
        private string _nbth;
        private string _lxth;
        private string _ccxmbz;
        private string _ynzsm;
        private string _d2zh;
        private string _d2zldm;
        private string _d2csrq;
        private string _d2xm;
        private string _ri;
        private string _xmpy;
        private string _jtxm;
        private string _bmdm;
        private string _wybs;
        private string _mzdm;
        private string _zdydm;
        private string _zztdbj;
        private string _htblbj;
        private string _crbz;
        private string _bgczy;
        private string _bgczsj;
        private string _qzh;
        private string _pjnr;
        private string _tlq;

        /// <summary>
        /// 人员类别代码
        /// </summary>
        public string PJNR
        {
            set { _pjnr = value; }
            get { return _pjnr; }
        }

        /// <summary>
        /// 人员类别代码
        /// </summary>
        public string RYLBDM
        {
            set { _rylbdm = value; }
            get { return _rylbdm; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XM
        {
            set { _xm = value; }
            get { return _xm; }
        }
        /// <summary>
        /// 国家或地区代码
        /// </summary>
        public string GJDQDM
        {
            set { _gjdqdm = value; }
            get { return _gjdqdm; }
        }
        /// <summary>
        /// 性别代码
        /// </summary>
        public string XBDM
        {
            set { _xbdm = value; }
            get { return _xbdm; }
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string CSRQ
        {
            set { _csrq = value; }
            get { return _csrq; }
        }
        /// <summary>
        /// 证件类别代码
        /// </summary>
        public string ZJLBDM
        {
            set { _zjlbdm = value; }
            get { return _zjlbdm; }
        }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string ZJHM
        {
            set { _zjhm = value; }
            get { return _zjhm; }
        }
        /// <summary>
        /// 签证种类代码
        /// </summary>
        public string QZZLDM
        {
            set { _qzzldm = value; }
            get { return _qzzldm; }
        }
        /// <summary>
        /// 出入日期
        /// </summary>
        public string CRRQ
        {
            set { _crrq = value; }
            get { return _crrq; }
        }
        /// <summary>
        /// 出入时间
        /// </summary>
        public string CRSJ
        {
            set { _crsj = value; }
            get { return _crsj; }
        }
        /// <summary>
        /// 出入日期时间
        /// </summary>
        public string CRRQSJ
        {
            set { _crrqsj = value; }
            get { return _crrqsj; }
        }
        /// <summary>
        /// 出入口岸代码
        /// </summary>
        public string CRKADM
        {
            set { _crkadm = value; }
            get { return _crkadm; }
        }
        /// <summary>
        /// 交通方式代码
        /// </summary>
        public string JTFSDM
        {
            set { _jtfsdm = value; }
            get { return _jtfsdm; }
        }
        /// <summary>
        /// 交通工具标识(航次)
        /// </summary>
        public string JTGJBS
        {
            set { _jtgjbs = value; }
            get { return _jtgjbs; }
        }
        /// <summary>
        /// 前往国家地区代码
        /// </summary>
        public string QWGJDQDM
        {
            set { _qwgjdqdm = value; }
            get { return _qwgjdqdm; }
        }
        /// <summary>
        /// 发证地（国内：行政区划；国外：国家地区
        /// </summary>
        public string FZD
        {
            set { _fzd = value; }
            get { return _fzd; }
        }
        /// <summary>
        /// 出境理由代码
        /// </summary>
        public string CRJSYDM
        {
            set { _crjsydm = value; }
            get { return _crjsydm; }
        }
        /// <summary>
        /// 检查员号
        /// </summary>
        public string CZY
        {
            set { _czy = value; }
            get { return _czy; }
        }
        /// <summary>
        /// 通道号
        /// </summary>
        public string TDH
        {
            set { _tdh = value; }
            get { return _tdh; }
        }
        /// <summary>
        /// 内部团号（1位团体类型+5位组团单位+10位日期时间+4位通道号+1位标识位）
        /// </summary>
        public string NBTH
        {
            set { _nbth = value; }
            get { return _nbth; }
        }
        /// <summary>
        /// 旅行团号
        /// </summary>
        public string LXTH
        {
            set { _lxth = value; }
            get { return _lxth; }
        }
        /// <summary>
        /// 超长姓名备注
        /// </summary>
        public string CCXMBZ
        {
            set { _ccxmbz = value; }
            get { return _ccxmbz; }
        }
        /// <summary>
        /// 疑难字说明
        /// </summary>
        public string YNZSM
        {
            set { _ynzsm = value; }
            get { return _ynzsm; }
        }
        /// <summary>
        /// 第二证号
        /// </summary>
        public string D2ZH
        {
            set { _d2zh = value; }
            get { return _d2zh; }
        }
        /// <summary>
        /// 第二证类
        /// </summary>
        public string D2ZLDM
        {
            set { _d2zldm = value; }
            get { return _d2zldm; }
        }
        /// <summary>
        /// 第二出生日期
        /// </summary>
        public string D2CSRQ
        {
            set { _d2csrq = value; }
            get { return _d2csrq; }
        }
        /// <summary>
        /// 第二姓名
        /// </summary>
        public string D2XM
        {
            set { _d2xm = value; }
            get { return _d2xm; }
        }
        /// <summary>
        /// 日
        /// </summary>
        public string RI
        {
            set { _ri = value; }
            get { return _ri; }
        }
        /// <summary>
        /// 姓名拼音
        /// </summary>
        public string XMPY
        {
            set { _xmpy = value; }
            get { return _xmpy; }
        }
        /// <summary>
        /// 简体姓名
        /// </summary>
        public string JTXM
        {
            set { _jtxm = value; }
            get { return _jtxm; }
        }
        /// <summary>
        /// 部门代码（当前部门）
        /// </summary> 
        public string BMDM
        {
            set { _bmdm = value; }
            get { return _bmdm; }
        }
        /// <summary>
        /// 唯一标识（口岸代码 +通道号+出入日期+出入时间+3位流水号）
        /// </summary>        
        public string WYBS
        {
            set { _wybs = value; }
            get { return _wybs; }
        }
        /// <summary>
        /// 民族代码
        /// </summary>
        public string MZDM
        {
            set { _mzdm = value; }
            get { return _mzdm; }
        }
        /// <summary>
        /// 自定义代码
        /// </summary>
        public string ZDYDM
        {
            set { _zdydm = value; }
            get { return _zdydm; }
        }
        /// <summary>
        /// 自助通道标记（1：自助通道验放，0：非自助通道验放）
        /// </summary>
        public string ZZTDBJ
        {
            set { _zztdbj = value; }
            get { return _zztdbj; }
        }
        /// <summary>
        /// 后台补录标记（0非后台补录；1后台补录）
        /// </summary>
        public string HTBLBJ
        {
            set { _htblbj = value; }
            get { return _htblbj; }
        }
        /// <summary>
        /// 出入标志
        /// </summary>
        public string CRBZ
        {
            set { _crbz = value; }
            get { return _crbz; }
        }
        /// <summary>
        /// 变更操作员(记录疑难字处理人)
        /// </summary>
        public string BGCZY
        {
            set { _bgczy = value; }
            get { return _bgczy; }
        }
        /// <summary>
        /// 变更操作时间（记录疑难字处理时间）
        /// </summary>
        public string BGCZSJ
        {
            set { _bgczsj = value; }
            get { return _bgczsj; }
        }
        /// <summary>
        /// 签证号
        /// </summary>
        public string QZH
        {
            set { _qzh = value; }
            get { return _qzh; }
        }
        /// <summary>
        /// 停留期
        /// </summary>
        public string TLQ
        {
            set { _tlq = value; }
            get { return _tlq; }
        }
        #endregion Model

    }
}
