namespace Atanet.Services.UoW
{
    using System;

    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public const string HostName = "atanet-db";

        public string ConstructConnectionStringFromEnvironment()
        {
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT");
            var user = Environment.GetEnvironmentVariable("MYSQL_USER");
            var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            var databaseName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            return $"Server=${HostName};Port=${port.ToString()};Database=${databaseName};Uid=${user};Pwd=${password};";
        }
    }
}
