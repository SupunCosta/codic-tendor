using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Util;

public static class ConnectionString
{
    public static string MapConnectionString(string contractingUnit, string project, ITenantProvider TenantProvider)
    {
        string connectionString = null;
        if (project == null && contractingUnit == null)
        {
            connectionString = TenantProvider.GetTenant().ConnectionString;
        }
        else if (project != null)
        {
            // var projectQuery =
            //     @"select ProjectConnectionString from ProjectDefinition where SequenceCode = @sequenceCode";
            //var projectQuery = "MapConnectionStringProjectQuery";
            using var dbConnection = new SqlConnection(TenantProvider.GetTenant().ConnectionString);
            dbConnection.Open();
            
             connectionString = dbConnection.QueryFirstOrDefaultAsync<string>("MapConnectionStringProjectQuery", param:new { sequenceCode = project },commandType:CommandType.StoredProcedure).Result;
             if (dbConnection.State != ConnectionState.Closed) dbConnection.CloseAsync();
        }
        else
        {
            var catelogConnection = TenantProvider.GetTenant().CatelogConnectionString;
            // var query =
            //     @"select  ConnectionString from [dbo].[UPrinceCustomerContractingUnit] where ContractingUnitId = @sequenceCode";
            //var query = "MapConnectionStringCuQuery";
            using var dbConnection = new SqlConnection(catelogConnection);
            
            connectionString = dbConnection.Query<string>("MapConnectionStringCuQuery", param:new { sequenceCode = contractingUnit },commandType:CommandType.StoredProcedure).FirstOrDefault();

            
        }

        return connectionString;
    }
}