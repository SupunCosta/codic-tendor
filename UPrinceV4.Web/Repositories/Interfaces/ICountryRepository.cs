using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetCountryList(CountryRepositoryParameter countryRepositoryParameter);
}

public class CountryRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string Lang { get; set; }

    public ITenantProvider TenantProvider { get; set; }
}