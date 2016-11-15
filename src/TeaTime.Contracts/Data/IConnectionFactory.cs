namespace TeaTime.Contracts.Data
{
    using System.Data;

    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
