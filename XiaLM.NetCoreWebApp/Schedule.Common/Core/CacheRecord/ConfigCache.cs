using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Schedule.model;
using Schedule.model.common;

namespace Schedule.Common.Core.CacheRecord
{
    public class ConfigCache
    {
        private static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        private static readonly string mqConfigPath =  @"Config\rabbitmq.xml";

        // public properties...
        public static ConcurrentDictionary<string, Schedule.Common.Core.Send.MqSend.RabbitMqSend.JobConfigEx> JobsConfig
        {
            get
            {
                var lst = new ConcurrentDictionary<string, Schedule.Common.Core.Send.MqSend.RabbitMqSend.JobConfigEx>();

                var jobs = mqconfig.Publishs.SelectMany(p => p.Jobs, (pub, job) => new
                { pub, job });

                foreach (var jobsItem in jobs)
                {
                    lst.TryAdd(jobsItem.job.Name, new Schedule.Common.Core.Send.MqSend.RabbitMqSend.JobConfigEx()
                    {
                        ExchangeName = jobsItem.pub.ExchangeName,
                        ExchangeKey = jobsItem.pub.ExchangeKey,
                        Name = jobsItem.job.Name,
                        Priority = jobsItem.job.Priority,
                        Durable = jobsItem.job.Durable,
                        TtlTime = jobsItem.job.TtlTime
                    });
                }
                return lst ;
            }
        }
        public static RabbitMqConfig mqconfig
        {
            get
            {
                //var mqconfig = (RabbitMqConfig)_ConfigCache.Get("ConfigCache");
                var mqconfig = (RabbitMqConfig)cache.Get("ConfigCache");
                if (mqconfig == null)
                {
                    mqconfig = SerializationHelper.Load<RabbitMqConfig>(AppDomain.CurrentDomain.BaseDirectory + mqConfigPath);

                    //var policy = new CacheItemPolicy();
                    //policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string>() { AppDomain.CurrentDomain.BaseDirectory + mqConfigPath }));                                   
                    //_ConfigCache.Add("ConfigCache", mqconfig, policy);

                    cache.Set("ConfigCache", mqconfig, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromHours(1)
                    });
                }
                return mqconfig;
            }
        }
    }
}
