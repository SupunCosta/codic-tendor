using System.Data;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace UPrinceV4.Shared.TenantProviders;

public class WebTenantProvider : ITenantProvider
{
    private readonly string _host;
    private readonly ITenantSource _tenantSource;
    private readonly string _project;
    private readonly string _cu;
    private readonly Tenant _tenant;
    public WebTenantProvider(ITenantSource tenantSource, IHttpContextAccessor accessor)
    {
        _tenantSource = tenantSource;
        _host = accessor.HttpContext.Request.Host.ToString();
        _tenant = _tenantSource.Tenant(_host);
        if (accessor.HttpContext.Request.Headers.TryGetValue("project", out var headerValue))
        {
            _project = headerValue.First();
        }

        if (accessor.HttpContext.Request.Headers.TryGetValue("cu", out var cu))
        {
            _cu = cu.First();
        }
    }

    public Tenant GetTenant()
    {
        return _tenant;
    }

    public TenantDto GetTenantDto()
    {
        var tenants = _tenantSource.ListTenantsDto();

        return tenants
            .FirstOrDefault(t => t.Host.ToLower() == _host.ToLower());
    }

    public SqlConnection projectSqlConnection()
    {
        using var dbConnection = new SqlConnection(GetTenant().ConnectionString);
        var connectionString = dbConnection.QueryFirstOrDefaultAsync<string>("MapConnectionStringProjectQuery", param:new { sequenceCode = _project },commandType:CommandType.StoredProcedure).Result;
        return new SqlConnection(connectionString);;
    }

    public SqlConnection cuSqlConnection()
    {
        using var dbConnection = new SqlConnection(GetTenant().CatelogConnectionString);
        var connectionString = dbConnection.Query<string>("MapConnectionStringCuQuery", param:new { sequenceCode = _cu },commandType:CommandType.StoredProcedure).FirstOrDefault();
        var sqlConnection = new SqlConnection(connectionString);
        return sqlConnection;
    }

    public SqlConnection orgSqlConnection()
    {
        var sqlConnection = new SqlConnection(GetTenant().ConnectionString);
        return sqlConnection;
    }
    
    
    public SqlConnection Connection()
    {
        var sqlConnection = new SqlConnection(MapConnectionString(_cu, _project));
        return sqlConnection;
    }
    
    public  string MapConnectionString(string contractingUnit, string project)
    {
        string connectionString = null;
        if (project == null && contractingUnit == null)
        {
            connectionString = GetTenant().ConnectionString;
        }
        else if (project != null)
        {
            using var dbConnection = new SqlConnection(GetTenant().ConnectionString);
            connectionString = dbConnection.QueryFirstOrDefaultAsync<string>("MapConnectionStringProjectQuery", param:new { sequenceCode = project },commandType:CommandType.StoredProcedure).Result;
        }
        else
        {
            var catelogConnection = GetTenant().CatelogConnectionString;
            using var dbConnection = new SqlConnection(catelogConnection);
            connectionString = dbConnection.Query<string>("MapConnectionStringCuQuery", param:new { sequenceCode = contractingUnit },commandType:CommandType.StoredProcedure).FirstOrDefault();
            
        }

        return connectionString;
    }
}