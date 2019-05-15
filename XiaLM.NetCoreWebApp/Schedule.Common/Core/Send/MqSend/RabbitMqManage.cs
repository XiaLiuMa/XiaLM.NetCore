using System;
using System.Collections.Generic;
using Base.Common.LogHelp;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Schedule.Common.Util;
using Schedule.model;

// namespaces...
namespace Schedule.Common.Core.Send.MqSend
{
    // public classes...
    /// <summary>
    /// RABBITMQ管理工厂类
    /// </summary>
    public class RabbitMqManage
    {
        // private fields...
        /// <summary>
        /// 这里暂时只考虑一个RABBITMQ的情况
        /// </summary>
        private static ConnectionPool pool;

        // public properties...
        public static RabbitMqConfig MqConfig { get; private set; }

        /// <summary>
        /// 从队列中接收到消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                Log.Info($"{DateTime.Now.ToString(CalculateUtil.FormatDt)}:已从mq获取消息，正在进行处理");

                IsolatorData data = SerializeUtil.DeserializeObject(e.Body) as IsolatorData;

                IsolatorUtil.PostData_ActionBlock(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        private static void ExchangeDeclare(ConnectionFactory factory)
        {
            try
            {
                if (MqConfig.Publishs == null || MqConfig.Publishs.Count == 0)
                {
                    return;
                }

                using (var channel = pool.GetOrCreateChannel())
                {
                    var model = channel.Model;
                    MqConfig.Publishs.ForEach(r =>
                        {
                            if (!string.IsNullOrWhiteSpace(r.ExchangeName))
                            {
                                model.ExchangeDeclare(r.ExchangeName, r.ExchangeType, true);
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"MQ初始化申明路由时失败--{ex.ToString()}");
            }
        }
        /// <summary>
        /// 申明队列
        /// </summary>
        /// <param name="factory"></param>
        private static void QueueDeclare(ConnectionFactory factory)
        {
            try
            {
                if (MqConfig.Consumers == null || MqConfig.Consumers.Count == 0)
                {
                    return;
                }
                using (var channel = pool.GetOrCreateChannel())
                {
                    var model = channel.Model;
                    MqConfig.Consumers.ForEach(r =>
                        {
                            IDictionary<string, object> queueParams = new Dictionary<string, object>();
                            if (r.IsPriority)
                            {
                                queueParams.Add("x-max-priority", r.PrioritySize);
                            }
                            if (r.IsTtl)
                            {
                                queueParams.Add("x-message-ttl", r.TtlTime);
                            }

                            model.QueueDeclare(r.QueueName, true, false, false, queueParams);


                            if (!string.IsNullOrWhiteSpace(r.ExchangeName))
                            {
                                model.QueueBind(r.QueueName, r.ExchangeName, r.ExchangeKey);
                            }
                        });
                }
            }
            catch (Exception ex)
            {
                Log.Error($"MQ初始化申明队列时失败--{ex.ToString()}");
            }
        }

        /// <summary>
        /// 消费者注册。这里以事件的方式来接收消费者信息
        /// </summary>
        public static void ConsumeRegist()
        {
            try
            {
                if (MqConfig.Consumers == null || MqConfig.Consumers.Count == 0)
                {
                    return;
                }
                MqConfig.Consumers.ForEach(r =>
                    {
                        if (r.IsListen)
                        {
                            using (var channel = pool.GetOrCreateChannel())
                            {
                                var Model = channel.Model;
                                Model.BasicQos(0, 1, false);
                                var consumer = new EventingBasicConsumer(channel.Model);
                                channel.Model.BasicConsume(r.QueueName, true, consumer);

                                consumer.Received += Consumer_Received;
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                Log.Error($"MQ绑定消费者失败--{ex.ToString()}");
            }
        }
        /// <summary>
        /// 初始化RABBIT
        /// </summary>
        public static void Init()
        {
            MqConfig = CacheRecord.ConfigCache.mqconfig;
            var uri = new Uri($"qmqp://{MqConfig.Ip}:{MqConfig.Port}");

            var factory = new ConnectionFactory();
            factory.Endpoint = new AmqpTcpEndpoint(uri);
            factory.VirtualHost = MqConfig.Vhost;
            factory.UserName = MqConfig.Use;
            factory.Password = MqConfig.Pwd;
            factory.AutomaticRecoveryEnabled = true;
            factory.RequestedHeartbeat = 60;

            pool = new ConnectionPool(factory, MqConfig.ConnPool);
            pool.ChannelPool = MqConfig.ChannelPool;

            ExchangeDeclare(factory);
            QueueDeclare(factory);
        }

        private readonly static object lckObj = new object();
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="exchange">路由</param>
        /// <param name="routingKey">路由键</param>
        /// <param name="body">内容</param>
        /// <param name="priority">优先级(0-9)</param>
        /// <param name="persistence">是否持久化</param>
        /// <param name="expiration">过期时间</param>
        public static void Publish(string exchange, string routingKey, byte[] body, int priority, bool persistence, int expiration = 0, string jobName = "")
        {
            lock (lckObj)
            {
                try
                {
                    using (var channel = pool.GetOrCreateChannel())
                    {
                        var properties = channel.Model.CreateBasicProperties();


                        if (priority >= 0)
                        {
                            properties.Priority = (byte)priority;
                        }


                        if (persistence)
                        {
                            properties.DeliveryMode = 2;
                        }


                        if (expiration > 1)
                        {
                            properties.Expiration = (expiration * 1000).ToString();
                        }

                        properties.MessageId = jobName;

                        channel.Model.BasicPublish(exchange, routingKey, properties, body);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void Stop()
        {
            pool.Dispose();
        }
    }
}
