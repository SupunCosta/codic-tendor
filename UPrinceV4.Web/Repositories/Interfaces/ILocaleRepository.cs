using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ILocaleRepository
{
    Task<IEnumerable<LocaleName>> GetLocale(ApplicationDbContext context);
    Task<LocaleName> GetLocaleById(ApplicationDbContext context, string id);
    Task<string> CreateLocale(ApplicationDbContext context, ITenantProvider tenantProvider, IFormCollection qrDto);

    Task<string> UpdateLocale(ApplicationDbContext context, ITenantProvider tenantProvider,
        LocaleNameUpdateDto qrDto);

    Task<bool> DeleteLocale(ApplicationDbContext context, string id);
}