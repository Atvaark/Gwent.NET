using System;
using System.Threading;

namespace Gwent.NET.Extensions
{
    public static class ReaderWriterLockSlimExtensions
    {
        private class ReadLockToken : IDisposable
        {
            private ReaderWriterLockSlim _readerWriterLock;

            public ReadLockToken(ReaderWriterLockSlim readerWriterLock)
            {
                if (readerWriterLock == null) throw new ArgumentNullException("readerWriterLock");
                _readerWriterLock = readerWriterLock;
                _readerWriterLock.EnterReadLock();
            }

            public void Dispose()
            {
                Dispose(true);
            }

            public void Dispose(bool disposing)
            {
                if (disposing && _readerWriterLock != null)
                {
                    _readerWriterLock.ExitReadLock();
                    _readerWriterLock = null;
                }
            }
        }

        private class WriteLockToken : IDisposable
        {
            private ReaderWriterLockSlim _readerWriterLock;

            public WriteLockToken(ReaderWriterLockSlim readerWriterLock)
            {
                if (readerWriterLock == null) throw new ArgumentNullException("readerWriterLock");
                _readerWriterLock = readerWriterLock;
                _readerWriterLock.EnterWriteLock();
            }

            public void Dispose()
            {
                Dispose(true);
            }

            public void Dispose(bool disposing)
            {
                if (disposing && _readerWriterLock != null)
                {
                    _readerWriterLock.ExitWriteLock();
                    _readerWriterLock = null;
                }
            }
        }
        
        public static IDisposable EnterRead(this ReaderWriterLockSlim readerWriterLock)
        {
            return new ReadLockToken(readerWriterLock);
        }

        public static IDisposable EnterWrite(this ReaderWriterLockSlim readerWriterLock)
        {
            return new WriteLockToken(readerWriterLock);
        }
    }
}
