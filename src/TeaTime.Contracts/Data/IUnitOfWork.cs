namespace TeaTime.Contracts.Data
{
    using System;
    using System.Data;

    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction GetTransaction(bool begin = true);

        IDbConnection GetReadOnlyConnection();
        IDbConnection GetWriteConnection();

        bool Commit();
    }
}