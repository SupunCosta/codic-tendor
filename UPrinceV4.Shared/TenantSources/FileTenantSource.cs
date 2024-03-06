using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace UPrinceV4.Shared.TenantSources;

public class FileTenantSource : ITenantSource
{
    public FileTenantSource(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public IEnumerable<Tenant> ListTenants()
    {
        var query = "select * from [dbo].[UPrinceCustomerTenantsInfo]";
        using (var dbConnection = new SqlConnection(Configuration.GetConnectionString("OrgDb")))
        {
            return dbConnection.Query<Tenant>(query);;
        }
    }


    public IEnumerable<TenantDto> ListTenantsDto()
    {
        var query = "select * from [dbo].[UPrinceCustomerTenantsInfo]";
               using (var dbConnection = new SqlConnection(Configuration.GetConnectionString("OrgDb")))
        {
            return dbConnection.Query<TenantDto>(query);;
        }
    }

    public Tenant Tenant(string host)
    {
        var query = "select * from [dbo].[UPrinceCustomerTenantsInfo] where LOWER(Host) = @Host";
        using (var dbConnection = new SqlConnection(Configuration.GetConnectionString("OrgDb")))
        {
            return dbConnection.QueryFirstOrDefaultAsync<Tenant>(query, new {Host = host}).Result;
        }
    }
}