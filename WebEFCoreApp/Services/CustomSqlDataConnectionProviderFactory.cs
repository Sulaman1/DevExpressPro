using System;
using System.Collections.Generic;
using System.Linq;
using DBContext.Data;
using DevExpress.Data.Entity;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Web;
using DevExpress.DataAccess.Wizard.Services;
using Microsoft.Extensions.Configuration;
using WebEFCoreApp.Data;

using Microsoft.EntityFrameworkCore;

namespace WebEFCoreApp.Services
{
    public class CustomSqlDataConnectionProviderFactory : IConnectionProviderFactory {
        //private readonly ApplicationDbContext reportDbContext;
        private readonly IConnectionProviderService connectionProviderService;

        public CustomSqlDataConnectionProviderFactory(/*ApplicationDbContext reportDbContext*/IConnectionProviderService connectionProviderService) {
            //this.reportDbContext = reportDbContext;
            this.connectionProviderService = connectionProviderService;
        }

        public IConnectionProviderService Create() {
            return connectionProviderService;
            //return new CustomSqlConnectionProviderService(reportDbContext.SqlDataConnections.ToList());
        }
    }

    public class CustomSqlConnectionProviderService : IConnectionProviderService {
        //readonly IEnumerable<DBContext.Data.DataConnection> sqlDataConnections;
        
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public CustomSqlConnectionProviderService(IConfiguration configuration, ApplicationDbContext context/*, IEnumerable<DBContext.Data.DataConnection> sqlDataConnections*//*IEnumerable<DBContext.Data.DataConnection> sqlDataConnections*/) {
            this.configuration = configuration;
            this.context = context;
            //this.sqlDataConnections = sqlDataConnections;
        }
        public SqlDataConnection LoadConnection(string connectionName) {
            connectionName = string.IsNullOrEmpty(connectionName) ? "Reporting" : connectionName;
            //var sqlDataConnectionData = sqlDataConnections.FirstOrDefault(x => x.Name == connectionName);
            //if(sqlDataConnectionData == null)
            //    throw new InvalidOperationException();

            //if(string.IsNullOrEmpty(sqlDataConnectionData.ConnectionString))
            //    throw new KeyNotFoundException($"Connection string '{connectionName}' not found.");

            var connectionParameters = new CustomStringConnectionParameters(context.Database.GetConnectionString());
            return new SqlDataConnection(connectionName, connectionParameters);

            //var sqlDataConnectionData = sqlDataConnections.FirstOrDefault(x => x.Name == connectionName);
            //if(sqlDataConnectionData == null)
            //    throw new InvalidOperationException();

            //if(string.IsNullOrEmpty(sqlDataConnectionData.ConnectionString))
            //    throw new KeyNotFoundException($"Connection string '{connectionName}' not found.");
            //var connectionParameters = new CustomStringConnectionParameters(sqlDataConnectionData.ConnectionString);
            //return new SqlDataConnection(connectionName, connectionParameters);
        }
    }
}
