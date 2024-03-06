using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories.Interfaces.CAB;

public interface IUniqueContactDetailsRepository
{
    public Task<IEnumerable<CabUniqueData>> GetCabUsedUniqueContactDataList(
        UniqueContactDetailsRepositoryParameter uniqueContactDetailsRepositoryParameter);

    public Task<bool> IsUsedUniqueContact(
        UniqueContactDetailsRepositoryParameter uniqueContactDetailsRepositoryParameter);
}

public class UniqueContactDetailsRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public CabUniqueContactDetailsFilterModel CabUniqueContactDetailsFilterModel { get; set; }
    public ITenantProvider TenantProvider { get; set; }
}