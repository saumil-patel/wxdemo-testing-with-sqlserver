using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Wx.Demo.Api.Data
{
    public class DatabaseConnectionProvider : IDatabaseConnectionProvider
    {
        private readonly ILogger<IDatabaseConnectionProvider> _logger;

        private readonly string _connectionString;
        
        public DatabaseConnectionProvider(IConfiguration configuration, ILogger<IDatabaseConnectionProvider> logger)
        {
            _logger = logger;
            _connectionString = GetConnectionString(configuration);
        }

        public async Task<IDbConnection> GetOpenConnection()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        private string GetConnectionString(IConfiguration configuration)
        {
            var dataSource = configuration.GetValue<string>("Database:Server");
            if (string.IsNullOrEmpty(dataSource))
            {
                throw new InvalidOperationException(
                    "No value has been provided for the Database:Server configuration setting.");
            }

            var initialCatalog = configuration.GetValue<string>("Database:Name");
            if (string.IsNullOrEmpty(initialCatalog))
            {
                throw new InvalidOperationException(
                    "No value has been provided for the Database:Name configuration setting.");
            }
            
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                ApplicationName = "wx-demo-api",
                ConnectTimeout = 60,
                ConnectRetryCount = 5,
                ConnectRetryInterval = 2,
                MaxPoolSize = 300,
                Pooling = true
            };

            var userId = configuration.GetValue<string>("Database:UserId");
            var password = configuration.GetValue<string>("Database:Password");
            
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(password))
            {
                builder.IntegratedSecurity = true;
                _logger.LogInformation("Using Integrated security for connection to db");
            }
            else
            {
                builder.UserID = userId;
                builder.Password = password;
                _logger.LogInformation("Using User/Pass security for connection to db");
            }
            
            return builder.ConnectionString;
        }
    }
}
