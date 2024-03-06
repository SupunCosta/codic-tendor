using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IAppConfigurationRepository
{
    public Task<string> Configure(ApplicationDbContext context, ITenantProvider tenantProvider,
        IFormCollection csvFile);
}