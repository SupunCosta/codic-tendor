

using Microsoft.Data.SqlClient;

namespace UPrinceV4.Shared;

public interface ITenantProvider
{
    Tenant GetTenant();
    TenantDto GetTenantDto();
    SqlConnection projectSqlConnection();
    
    SqlConnection cuSqlConnection();
    
    SqlConnection orgSqlConnection();
    SqlConnection Connection();
    

}