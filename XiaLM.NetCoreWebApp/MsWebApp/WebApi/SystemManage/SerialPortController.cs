using Microsoft.AspNetCore.Mvc;
using MsWebApp.Comm;
using MsWebAppDal.Dal.SystemManage;
using MsWebAppDal.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using Base.Common.Utils;
using Base.Common.LogHelp;

namespace MsWebApp.WebApi.SystemManage
{
    /// <summary>
    /// 串口配置
    /// </summary>
    public class SerialPortController : Controller
    {
        private const string ModelName = "串口配置";
        private SerialPortDal dal = new SerialPortDal();

        /// <summary>
        /// 获取本地串口驱动列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SerialPort/GetDrives")]
        public ActionResult GetDrives()
        {
            List<string> list = SerialPort.GetPortNames().ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("lst", list);
            return Json(dic);
        }

        /// <summary>
        /// 查询串口配置
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SerialPort/Query")]
        [LogFilter(czmk = ModelName, czlx = OperationType.search, czsm = "查询Ms端串口配置数据")]
        public ActionResult Query(string query, string page, string rows)
        {
            List<SerialPortConfig> list = GetSerialPorts();
            list = list.Where(p => p.PortName.Contains(query)).OrderBy(p => p.PortName).ToList();
            int count = list.Count;
            list = list.Take(int.Parse(page) * int.Parse(rows)).Skip(int.Parse(rows) * (int.Parse(page) - 1)).ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total", count);
            dic.Add("rows", list);
            return Json(dic);
        }

        /// <summary>
        /// 新增串口配置
        /// </summary>
        /// <param name="PortName"></param>
        /// <param name="baudRate"></param>
        /// <param name="isReceive"></param>
        /// <param name="isSend"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SerialPort/Add")]
        [LogFilter(czmk = ModelName, czlx = OperationType.add, czsm = "添加Ms端串口配置数据")]
        public ActionResult Add(string portName, string baudRate, string isReceive, string isSend)
        {
            List<SerialPortConfig> list = GetSerialPorts();
            SerialPortConfig serialPort = new SerialPortConfig()
            {
                ID = list.OrderBy(p => p.ID).LastOrDefault().ID + 1,    //ID自增长
                PortName = portName,
                BaudRate = baudRate,
                IsReceive = isReceive == "true" ? 0 : 1,
                IsSend = isSend == "true" ? 0 : 1
            };
            list.Add(serialPort);
            return Content(SetSerialPorts(list).ToString());
        }

        /// <summary>
        /// 修改串口配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="isReceive"></param>
        /// <param name="isSend"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SerialPort/Alert")]
        [LogFilter(czmk = ModelName, czlx = OperationType.edit, czsm = "修改Ms端串口配置数据")]
        public ActionResult Alert(string id, string portName, string baudRate, string isReceive, string isSend)
        {
            SerialPortConfig serialPort = new SerialPortConfig()
            {
                ID = int.Parse(id),
                PortName = portName,
                BaudRate = baudRate,
                IsReceive = isReceive == "true" ? 0 : 1,
                IsSend = isSend == "true" ? 0 : 1
            };
            List<SerialPortConfig> list = GetSerialPorts();
            var obj = list.Where(p => p.ID == int.Parse(id)).FirstOrDefault();
            int index = ListUtil.GetIndex(list, obj);   //获取索引
            list = ListUtil.Alert(list, serialPort, index); //修改列表
            return Content(SetSerialPorts(list).ToString());
        }

        /// <summary>
        /// 删除串口配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SerialPort/Delete")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "删除Ms端串口配置数据")]
        public ActionResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids.Trim())) return Content("请输入ids");
            List<SerialPortConfig> list = GetSerialPorts();
            string[] idArray = ids.Split(',');
            if (idArray != null && idArray.Length > 0)
            {
                foreach (string id in idArray)
                {
                    list.Remove(list.Where(p => p.ID == int.Parse(id)).FirstOrDefault());
                }
                return Content(SetSerialPorts(list).ToString());
            }
            else
            {
                return Content("");
            }
        }


        /// <summary>
        /// 清除串口配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SerialPort/Clear")]
        [LogFilter(czmk = ModelName, czlx = OperationType.delete, czsm = "清除Ms端串口配置数据")]
        public ActionResult Clear()
        {
            return Content(SetSerialPorts(null).ToString());
        }


        #region 串口配置文件操作
        /// <summary>
        /// 获取串口配置
        /// </summary>
        /// <returns></returns>
        private List<SerialPortConfig> GetSerialPorts()
        {

            string fPath = $"{DirectoryUtil.GetAppParentDir()}/Ms.IsolatorService/config/SerialPort.xml";  //配置文件路径
            List<SerialPortConfig> lst = XmlUtils.Load<List<SerialPortConfig>>(fPath);

            return lst;
        }


        /// <summary>
        /// 保存串口配置
        /// </summary>
        /// <returns></returns>
        private bool SetSerialPorts(List<SerialPortConfig> lst)
        {
            string fPath = $"{DirectoryUtil.GetAppParentDir()}/Ms.IsolatorService/config/SerialPort.xml";  //配置文件路径
            List<SerialPortConfig> oldlst = XmlUtils.Load<List<SerialPortConfig>>(fPath);   //原始列表
            try
            {
                XmlUtils.Save(lst, fPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                XmlUtils.Save(oldlst, fPath);
                return false;
            }
            return true;
        }
        #endregion
    }
}
