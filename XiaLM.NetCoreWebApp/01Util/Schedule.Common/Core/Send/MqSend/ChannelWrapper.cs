using System;
using RabbitMQ.Client;

// namespaces...
namespace Schedule.Common.Core.Send.MqSend
{
    // public classes...
    /// <summary>
    /// 通道包装
    /// </summary>
    public class ChannelWrapper : IDisposable
    {
        // private fields...
        private ChannelPool pool;

        // public constructors...
        public ChannelWrapper(IModel model, ChannelPool pool)
        {
            this.Model = model;
            this.pool = pool;
            var r = new System.Random();

            Guid = pool.Guid + "_" + r.Next(100, 1000).ToString();
        }

        // public properties...
        public string Guid { get; private set; }
        public bool IsBusy { get; set; }
        public IModel Model { get; private set; }

        // public methods...
        public void Dispose()
        {
            if (this.pool != null)
            {
                this.pool.DisposeChannel(this);
            }
        }
    }
}
