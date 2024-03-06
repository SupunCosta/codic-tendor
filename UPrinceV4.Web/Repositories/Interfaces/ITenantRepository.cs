using System.Collections.Generic;
using UPrinceV4.Web.Catalog;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

internal interface ITenantRepository
{
    IEnumerable<Tenant> GetTenants(ApplicationDbContext context);
    Currency GetTenantById(ApplicationDbContext context, string id);
    string CreateTenant(ApplicationDbContext context, Tenant tenant);
    string UpdateTenant(ApplicationDbContext context, Tenant tenant);
    bool DeleteTenant(ApplicationDbContext context, string id);
}