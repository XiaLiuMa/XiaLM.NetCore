using System.Collections;
using System.Collections.Generic;
using System.Threading;

// namespaces...
namespace Schedule.Common.Core.Concurrent
{
    // public classes...
    public class ConcurrentEnumerator<T> : IEnumerator<T>
    {
        // private fields...
        private readonly IEnumerator<T> _inner;
        private readonly ReaderWriterLockSlim _lock;

        // public constructors...
        public ConcurrentEnumerator(IEnumerable<T> inner, ReaderWriterLockSlim @lock)
        {
            this._lock = @lock;
            this._lock.EnterReadLock();
            this._inner = inner.GetEnumerator();
        }

        // private properties...
        object IEnumerator.Current
        {
            get
            {
                return _inner.Current;
            }
        }

        // public properties...
        public T Current
        {
            get
            {
                return _inner.Current;
            }
        }

        // public methods...
        public void Dispose()
        {
            this._lock.ExitReadLock();
        }
        public bool MoveNext()
        {
            return _inner.MoveNext();
        }
        public void Reset()
        {
            _inner.Reset();
        }
    }
}
