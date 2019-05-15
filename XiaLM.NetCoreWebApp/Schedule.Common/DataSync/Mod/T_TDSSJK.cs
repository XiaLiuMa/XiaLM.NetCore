using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Common.DataSync.Mod
{
    /// <summary>
    /// 通道实时监控信息表
    /// </summary>
    [Serializable]
    public partial class T_TDSSJK
    {
        #region Model
        private string _tdh;
        private string _gzzzt;
        private string _yflklx;
        private string _zjhm;
        private string _wybs;
        private string _tdxspdm;
        private string _tdxspxx;
        private string _czydm;
        private string _czyxm;
        private string _tdlx;
        private string _dqyxmk;
        private string _czyzcsj;
        private string _tdktsj;
        private string _tdgbsj;
        private string _zcbm;
        private string _ip;
        private string _kadm;
        private int? _galkyfs;
        private int? _fgalkyfs;
        private int? _ygyfs;
        private int? _jtgjyfs;
        private int? _bkbjs;
        private int? _yctjs;
        private string _rq;
        private string _bmmc;
        private string _pjnr;
        private string _lkxm;
        private int? _yfs;
        /// <summary>
        /// 通道号
        /// </summary>       
        public string TDH
        {
            set { _tdh = value; }
            get { return _tdh; }
        }
        /// <summary>
        /// 通道当前状态
        /// </summary>
        public string GZZZT
        {
            set { _gzzzt = value; }
            get { return _gzzzt; }
        }
        /// <summary>
        /// 通道查验当前旅客的类型标志(第一位为旅客年龄代码：值为0时，表示小于3岁，为1表示3岁至70岁，2大于70岁，9-未知年龄；第二位标示是否怀疑临重控；第三位标示是否怀疑失效证；第四位标示是否怀疑在逃对象；第五位标示是否怀疑常控。类型标志中第二至第五位值为1代表是，其它值表示为否)
        /// </summary>
        public string YFLKLX
        {
            set { _yflklx = value; }
            get { return _yflklx; }
        }
        /// <summary>
        /// 本通道查验最后的人员的证件号码
        /// </summary>
        public string ZJHM
        {
            set { _zjhm = value; }
            get { return _zjhm; }
        }
        /// <summary>
        /// 唯一标识（口岸代码 +通道号+出入日期+出入时间+3位流水号）。是出入境记录的唯一标识对应
        /// </summary>
        public string WYBS
        {
            set { _wybs = value; }
            get { return _wybs; }
        }
        /// <summary>
        /// 通道显示屏代码
        /// </summary>
        public string TDXSPDM
        {
            set { _tdxspdm = value; }
            get { return _tdxspdm; }
        }
        /// <summary>
        /// 通道的LED屏显示内容
        /// </summary>
        public string TDXSPXX
        {
            set { _tdxspxx = value; }
            get { return _tdxspxx; }
        }
        /// <summary>
        /// 操作员代码
        /// </summary>
        public string CZYDM
        {
            set { _czydm = value; }
            get { return _czydm; }
        }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string CZYXM
        {
            set { _czyxm = value; }
            get { return _czyxm; }
        }
        /// <summary>
        /// 通道类型
        /// </summary>
        public string TDLX
        {
            set { _tdlx = value; }
            get { return _tdlx; }
        }
        /// <summary>
        /// 当前运行模块
        /// </summary>
        public string DQYXMK
        {
            set { _dqyxmk = value; }
            get { return _dqyxmk; }
        }
        /// <summary>
        /// 检查员注册时间
        /// </summary>
        public string CZYZCSJ
        {
            set { _czyzcsj = value; }
            get { return _czyzcsj; }
        }
        /// <summary>
        /// 通道开通时间
        /// </summary>
        public string TDKTSJ
        {
            set { _tdktsj = value; }
            get { return _tdktsj; }
        }
        /// <summary>
        /// 通道关闭时间
        /// </summary>
        public string TDGBSJ
        {
            set { _tdgbsj = value; }
            get { return _tdgbsj; }
        }
        /// <summary>
        /// 注册部门
        /// </summary>
        public string ZCBM
        {
            set { _zcbm = value; }
            get { return _zcbm; }
        }
        /// <summary>
        /// 工作站IP
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 口岸代码
        /// </summary>
        public string KADM
        {
            set { _kadm = value; }
            get { return _kadm; }
        }
        /// <summary>
        /// 港澳旅客验放数
        /// </summary>
        public int? GALKYFS
        {
            set { _galkyfs = value; }
            get { return _galkyfs; }
        }
        /// <summary>
        /// 非港澳旅客验放数
        /// </summary>
        public int? FGALKYFS
        {
            set { _fgalkyfs = value; }
            get { return _fgalkyfs; }
        }
        /// <summary>
        /// 员工验放数
        /// </summary>
        public int? YGYFS
        {
            set { _ygyfs = value; }
            get { return _ygyfs; }
        }
        /// <summary>
        /// 交通工具验放数
        /// </summary>
        public int? JTGJYFS
        {
            set { _jtgjyfs = value; }
            get { return _jtgjyfs; }
        }
        /// <summary>
        /// 边控报警数（不含常控数）
        /// </summary>
        public int? BKBJS
        {
            set { _bkbjs = value; }
            get { return _bkbjs; }
        }
        /// <summary>
        /// 异常提交数
        /// </summary>
        public int? YCTJS
        {
            set { _yctjs = value; }
            get { return _yctjs; }
        }
        /// <summary>
        /// 日期
        /// </summary>
        public string RQ
        {
            set { _rq = value; }
            get { return _rq; }
        }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string BMMC
        {
            set { _bmmc = value; }
            get { return _bmmc; }
        }
        /// <summary>
        /// 当前旅客评价内容
        /// </summary>
        public string PJNR
        {
            set { _pjnr = value; }
            get { return _pjnr; }
        }
        /// <summary>
        /// 当前或最后通关的旅客姓名
        /// </summary>
        public string LKXM
        {
            set { _lkxm = value; }
            get { return _lkxm; }
        }
        /// <summary>
        /// 验放数
        /// </summary>
        public int? YFS
        {
            set { _yfs = value; }
            get { return _yfs; }
        }
        #endregion Model
    }
}
