namespace TeaTime.Data
{
    using System.Data;
    using Contracts.Data;
    using MySql.Data.MySqlClient;

    public sealed class ConnectionFactory : IConnectionFactory
    {
        private readonly string _readConnectionString;

        public ConnectionFactory(string readConnectionString)
        {
            _readConnectionString = readConnectionString;
        }

        public IDbConnection GetConnection()
        {
            return new MySqlConnection(_readConnectionString);
        }
    }
}