using System;
using System.Collections.Generic;
using UPrinceV4.Web.Catalog;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class TenantRepository : ITenantRepository
{
    public string CreateTenant(ApplicationDbContext context, Tenant tenant)
    {
        throw new NotImplementedException();
    }

    public bool DeleteTenant(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public Currency GetTenantById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Tenant> GetTenants(ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }

    public string UpdateTenant(ApplicationDbContext context, Tenant tenant)
    {
        throw new NotImplementedException();
    }
}