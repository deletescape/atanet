namespace Atanet.Services.UoW
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;

    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        public string ConstructConnectionStringFromEnvironment()
        {
            var gateway = GetDefaultGateway();
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT");
            var user = Environment.GetEnvironmentVariable("MYSQL_USER");
            var password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            var databaseName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            var connectionString = $"server={gateway.ToString()};port={port};database={databaseName};userid=root;password={password};";
            Console.WriteLine(connectionString);
            return connectionString;
        }

        public static IPAddress GetDefaultGateway()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up)
                .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties()?.GatewayAddresses)
                .Select(g => g?.Address)
                .Where(a => a != null)
                .FirstOrDefault();
        }
    }
}
