using System;
using Base.Common.LogHelp;
using RabbitMQ.Client;

// namespaces...
namespace Schedule.Common.Core.Send.MqSend
{
    // public classes...
    public class ConnectionWrapper : IDisposable
    {
        // private fields...
        private ChannelPool _channelPool;
        private IConnection _conn;
        private ConnectionFactory _connFactory;
        private bool close = false;

        // public constructors...
        public ConnectionWrapper(IConnection conn)
            : this(conn, 20)
        {
        }
        public ConnectionWrapper(IConnection conn, int channelPool)
        {
            this._conn = conn;
            this._conn.ConnectionShutdown += _conn_ConnectionShutdown;
            this._channelPool = new ChannelPool(conn, channelPool);
        }

        // private methods...
        private void _conn_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            
            Log.Info(sender.ToString() + "  " + e.ReplyText + "  连接被断开");
        }

        // public methods...
        public void Dispose()
        {
            try
            {
                close = true;
                if (_channelPool != null)
                {
                    _channelPool.Dispose();
                    _channelPool = null;
                }

                if (_conn != null)
                {
                    _conn.Close();
                    ((IConnection)_conn).Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        public ChannelWrapper GetOrCreateChannel()
        {
            var channel = this._channelPool.GetOrCreateChannel();
            return channel;
        }
    }
}
