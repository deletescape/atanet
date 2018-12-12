namespace Atanet.Services.UoW
{
    using System;
    using System.Linq;
    using System.Net;

    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public string ConstructConnectionStringFromEnvironment()
        {
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT");
            var user = Environment.GetEnvironmentVariable("MYSQL_USER");
            var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            var databaseName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            var host = Environment.GetEnvironmentVariable("DATABASE_HOST");
            var connectionString = $"server={host};port={port};database={databaseName};userid=root;password={password};";
            return connectionString;
        }
    }
}
