using System;
using System.Collections.Concurrent;
using System.Text;
using Base.Common.LogHelp;
using Schedule.Common.Util;
using Schedule.model;

// namespaces...
namespace Schedule.Common.Core.Send.MqSend
{
    // public classes...
    public class RabbitMqSend : IDataSend
    {
        // private fields...
        private static ConcurrentDictionary<string, JobConfigEx> JobsConfig = Core.CacheRecord.ConfigCache.JobsConfig;
        private static Core.Compress.GZip compress = new Compress.GZip();

        // public constructors...
        public RabbitMqSend()
        {
        }

        // private properties...
        private static string DefaultExchangeKey { get; set; }
        private static string DefaultExchangeName { get; set; }

        // public methods...
        public void Send(byte[] data, string job)
        {
            JobConfigEx jobconfig = null;

            if (JobsConfig.ContainsKey(job))
            {
                jobconfig = JobsConfig[job];
            }
            else
            {
                RabbitMqManage.MqConfig.Publishs.ForEach(r =>
                    {
                        if (r.Default && string.IsNullOrWhiteSpace(DefaultExchangeKey))
                        {
                            DefaultExchangeName = r.ExchangeName;
                            DefaultExchangeKey = r.ExchangeKey;
                        }
                        if (r.Jobs != null)
                        {
                            r.Jobs.ForEach(j =>
                                {
                                    if (j.Name.Equals(job, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        jobconfig = new JobConfigEx()
                                        {
                                            ExchangeName = r.ExchangeName,
                                            ExchangeKey = r.ExchangeKey,
                                            Durable = j.Durable,
                                            Name = job,
                                            Priority = j.Priority,
                                            TtlTime = j.TtlTime
                                        };
                                    }
                                });
                        }
                    });
            }


            if (jobconfig == null)
            {
                RabbitMqManage.Publish(DefaultExchangeName, DefaultExchangeKey, data, -1, true);
            }
            else
            {
                IsolatorData bd = SerializeUtil.DeserializeObject(data) as IsolatorData;
                Log.Debug(DateTime.Now + ":发送数据" + job + "," + Encoding.UTF8.GetString(bd.Value));

                RabbitMqManage.Publish(jobconfig.ExchangeName, jobconfig.ExchangeKey, data, jobconfig.Priority, jobconfig.Durable, jobconfig.TtlTime, jobconfig.Name);
            }
        }

        public class JobConfigEx
        {
            // public properties...
            /// <summary>
            /// 任务消息是否需要持久化
            /// </summary>
            public bool Durable { get; set; }
            public string ExchangeKey { get; set; }
            public string ExchangeName { get; set; }
            public string Name { get; set; }
            /// <summary>
            /// 任务的优先级。为-1表示不设置优先级
            /// </summary>
            public int Priority { get; set; }
            /// <summary>
            /// 当前任务消息的过期时间。为0表示不设置过期时间。
            /// </summary>
            public int TtlTime { get; set; }
        }
    }
}
