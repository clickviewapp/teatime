namespace TeaTime.Data
{
    using System;
    using System.Data;
    using Contracts.Data;

    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IConnectionFactory _factory;

        private IDbTransaction _transaction;
        private IDbConnection _cachedConnection;
        
        public UnitOfWork(IConnectionFactory factory)
        {
            _factory = factory;
        }

        private readonly object _lock = new object();

        public IDbTransaction GetTransaction(bool begin = true)
        {
            lock (_lock)
            {
                if (_transaction == null && begin)
                {
                    // does this need a using?

                    var conn = GetWriteConnection();

                    if(conn.State != ConnectionState.Open)
                        conn.Open();

                    _transaction = conn.BeginTransaction(IsolationLevel.RepeatableRead);
                }
            }

            return _transaction;
        }

        public IDbConnection GetReadOnlyConnection()
        {
            lock (_lock)
            {
                if (_cachedConnection != null)
                    return _cachedConnection;
            }

            return _factory.GetConnection();
        }

        public IDbConnection GetWriteConnection()
        {
            lock (_lock)
                return _cachedConnection ?? (_cachedConnection = _factory.GetConnection());
        }

        public bool Commit()
        {
            lock (_lock)
            {
                if (_transaction == null)
                {
                    return true;
                }

                try
                {
                    if (_transaction.Connection.State == ConnectionState.Open)
                    {
                        _transaction.Commit();
                    }
                }
                finally
                {
                    _transaction.Connection?.Dispose();

                    _transaction.Dispose();
                    _transaction = null;
                }
                return true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            lock (_lock)
            {
                if (_transaction == null)
                    return;

                try
                {
                    _transaction.Rollback();
                }
                catch
                {
                    // ignored
                }

                //TODO: should this be responsible for closing conn?
                _transaction.Connection?.Dispose();

                _transaction.Dispose();
                _transaction = null;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}