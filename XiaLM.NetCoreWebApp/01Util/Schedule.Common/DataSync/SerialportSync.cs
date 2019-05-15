using Base.Common.LogHelp;
using Base.Common.Utils;
using Schedule.Common.Core.Send;
using Schedule.Common.Core.Send.MqSend;
using Schedule.Common.Service;
using Schedule.Common.Util;
using Schedule.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Schedule.Common.DataSync
{
    /// <summary>
    /// 串口同步
    /// </summary>
    public class SerialportSync
    {
        /// <summary>
        /// 所有监听隔离器处理数据监听器
        /// </summary>
        //private static List<IReceiveListener> LstReceiveListener;

        private static IDataSend MqSend = new RabbitMqSend();

        private static void IsolatorDataLister()
        {
            var lst = ReflectionUtil.FindSubClasses(typeof(IReceiveListener));
            foreach (Type t in lst)
            {
                var handler = (IReceiveListener)Activator.CreateInstance(t);
                Data_BroadcastBlock.LinkTo(handler.Data_ActionBlock);
            }
        }

        public static void PostData_ActionBlock(IsolatorData item)
        {
            if (Data_BroadcastBlock != null)
            {
                Data_BroadcastBlock.Post(item);
            }
        }

        private static BroadcastBlock<IsolatorData> Data_BroadcastBlock = null;

        public static CancellationTokenSource _CancellationTokenSourceData = new CancellationTokenSource();

        public static void StartDataBlock()
        {
            Data_BroadcastBlock = new BroadcastBlock<IsolatorData>((item) => { return item; });
            IsolatorDataLister();
        }

        #region 一次性将list所有数据发送
        /// <summary>
        /// 一次性将list所有数据发送。而不是逐条发送。主要用于接收公安端的查询命令获取的数据需要一次性发送。而不是逐条发送。
        /// </summary>
        /// <param name="lst">数据列表</param>
        /// <param name="isolator">任务名</param>
        /// <param name="cmdIsolator">命令</param>
        public static void SendOneTime(List<Dictionary<string, object>> lst, string jobname, byte cmd, int MaxSendCount, bool isAck = false)
        {
            if (lst != null && lst.Count < 1)
            {
                return;
            }
            try
            {
                if (lst != null && lst.Count >= MaxSendCount)
                {
                    int j = MaxSendCount;
                    List<List<Dictionary<string, object>>> lstTTTT = new List<List<Dictionary<string, object>>>();
                    for (int i = 0; i < lst.Count; i += MaxSendCount)
                    {
                        List<Dictionary<string, object>> lstt = new List<Dictionary<string, object>>();
                        lstt = lst.Take(j).Skip(i).ToList();
                        j += MaxSendCount;
                        lstTTTT.Add(lstt);
                    }
                    for (int i = 0; i < lstTTTT.Count; i++)
                    {
                        var isd = new IsolatorData();
                        isd.Cmd = cmd;
                        isd.Value = Encoding.UTF8.GetBytes(JsonUtil.ObjectToStr(lstTTTT[i]));
                        OnSend(isd, jobname);

                    }
                }
                else if (lst != null && lst.Count < MaxSendCount && lst.Count >= 1)
                {
                    var isd = new IsolatorData();
                    isd.Cmd = cmd;
                    var va = JsonUtil.ObjectToStr(lst);
                    isd.Value = Encoding.UTF8.GetBytes(JsonUtil.ObjectToStr(lst));
                    OnSend(isd, jobname);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void OnSend(IsolatorData data, string jobname, bool isAck = false)
        {
            Thread.Sleep(50);
            data.SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            data.JobName = jobname;
            var bd = SerializeUtil.SerializeObject(data);

            IsolatorData da = (IsolatorData)SerializeUtil.DeserializeObject(bd);
            MqSend.Send(bd, jobname);
        }
        #endregion
    }
}
