using System;
using System.Collections.Generic;
using System.Threading;
using Base.Common.LogHelp;
using RabbitMQ.Client;

// namespaces...
namespace Schedule.Common.Core.Send.MqSend
{
    // public classes...
    /// <summary>
    /// channel 池
    /// </summary>
    public class ChannelPool : IDisposable
    {
        // private fields...
        private IConnection _conn;
        private List<ChannelWrapper> _listChannel = new List<ChannelWrapper>();
        private System.Threading.ReaderWriterLockSlim GetChannellockObj = new System.Threading.ReaderWriterLockSlim();
        private ManualResetEvent _busyChannelHandle = new ManualResetEvent(false);
        private int _poolCount;

        // internal fields...
        internal string Guid;

        // public constructors...
        public ChannelPool(IConnection conn, int poolCount = 20)
        {
            if (conn == null)
            {
                throw new ArgumentNullException("参数conn不能为空，请检查参数");
            }
            this._conn = conn;
            this._poolCount = poolCount;

            var r = new System.Random();
            Guid = r.Next(100, 1000).ToString();
        }

        // private methods...
        private void CloseChannel(ChannelWrapper item)
        {
            try
            {
                if (!item.Model.IsClosed)
                {
                    item.Model.Close();
                }
                item.Model.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        private ChannelWrapper CreateChannel()
        {
            GetChannellockObj.EnterWriteLock();
            try
            {
                var channel = this._conn.CreateModel();
                var modelWrapper = new ChannelWrapper(channel, this);
                _listChannel.Add(modelWrapper);
                return modelWrapper;
            }
            finally
            {
                GetChannellockObj.ExitWriteLock();
            }
        }

        // internal methods...
        internal void InnerRemove(ChannelWrapper item)
        {
            GetChannellockObj.EnterWriteLock();
            try
            {
                CloseChannel(item);
                this._listChannel.Remove(item);
            }
            finally
            {
                GetChannellockObj.ExitWriteLock();
            }
        }

        // public methods...
        public void Dispose()
        {
            GetChannellockObj.EnterWriteLock();
            try
            {
                this._listChannel.ForEach(r =>
                    {
                        CloseChannel(r);
                    });
                this._listChannel.Clear();
            }
            finally
            {
                GetChannellockObj.ExitWriteLock();
            }
        }
        public void DisposeChannel(ChannelWrapper item)
        {
            item.IsBusy = false;
            this._busyChannelHandle.Set();
        }
        public ChannelWrapper GetOrCreateChannel()
        {
            GetChannellockObj.EnterUpgradeableReadLock();
            try
            {
                var s = true;
                var c = 0;
                while (s && c < 3)
                {
                    var channel = this._listChannel.Find(p => p.IsBusy == false);
                    if (channel != null)
                    {
                        channel.IsBusy = true;
                        s = false;
                        return channel;
                    }
                    else
                    {
                        if (this._listChannel.Count >= this._poolCount)
                        {
                            this._busyChannelHandle.WaitOne(TimeSpan.FromSeconds(3));
                            c++;
                        }
                        else
                        {
                            s = false;
                            return CreateChannel();
                        }
                    }
                }

                return null;
            }
            finally
            {
                GetChannellockObj.ExitUpgradeableReadLock();
            }
        }
    }
}
