namespace Atanet.Services.UoW
{
    using System;
    using System.Linq;
    using System.Net;

    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public const string DatabaseHost = "db";

        public string ConstructConnectionStringFromEnvironment()
        {
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT");
            var user = Environment.GetEnvironmentVariable("MYSQL_USER");
            var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            var databaseName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            var connectionString = $"server={DatabaseHost};port={port};database={databaseName};userid=root;password={password};";
            return connectionString;
        }
    }
}
