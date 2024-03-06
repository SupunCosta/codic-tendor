using System.Collections.Generic;

namespace UPrinceV4.Shared;

public interface ITenantSource
{
    IEnumerable<Tenant> ListTenants();
    IEnumerable<TenantDto> ListTenantsDto();
    
    Tenant Tenant(string host);
}