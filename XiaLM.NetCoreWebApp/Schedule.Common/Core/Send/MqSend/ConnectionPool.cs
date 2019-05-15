using System;
using Base.Common.LogHelp;
using RabbitMQ.Client;

// namespaces...
namespace Schedule.Common.Core.Send.MqSend
{
    // public classes...
    public class ConnectionPool : IDisposable
    {
        // private fields...
        private ConnectionFactory _connFactory;
        private readonly Concurrent.ConcurrentList<ConnectionWrapper> _connPool = new Concurrent.ConcurrentList<ConnectionWrapper>();
        private int _poolCount = 4;
        private ushort heartbeat;
        private volatile int index = 0;
        private readonly static object lockObj = new object();

        // public constructors...
        public ConnectionPool(ConnectionFactory factory, int pool = 4)
        {
            this._connFactory = factory;
            this._poolCount = pool;
            this._connFactory.AutomaticRecoveryEnabled = true;
            this._connFactory.RequestedHeartbeat = 60;
        }

        // public properties...
        /// <summary>
        /// 某个连接通道池中的最大数量
        /// </summary>
        public int ChannelPool { get; set; }
        public int PoolCount
        {
            get
            {
                return _poolCount;
            }
        }

        // private methods...
        private void InnerRemove(ConnectionWrapper item)
        {
            try
            {
                this._connPool.Remove(item);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        // public methods...
        public void Dispose()
        {
            if (_connPool != null)
            {
                for (var i = 0; i < this._connPool.Count; i++)
                {
                    try
                    {
                        this._connPool[i].Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.ToString());
                    }
                }
                this._connPool.Clear();
            }
        }
        public ChannelWrapper GetOrCreateChannel()
        {
            ConnectionWrapper connw = null;
            lock (lockObj)
            {
                if (this._connPool.Count <= index)
                {
                    if (this._connPool.Count < this._poolCount)
                    {
                        var conn = this._connFactory.CreateConnection();
                        connw = new ConnectionWrapper(conn, ChannelPool);
                        this._connPool.Add(connw);
                    }
                }
                connw = this._connPool[index];
                index = (index + 1) % this._poolCount;
            }
            return connw.GetOrCreateChannel();
        }

        public ChannelWrapper CreateChannel()
        {
            ConnectionWrapper connw = null;



            var conn = this._connFactory.CreateConnection();
            connw = new ConnectionWrapper(conn, ChannelPool);
            this._connPool.Add(connw);

            return connw.GetOrCreateChannel();
        }
    }
}
