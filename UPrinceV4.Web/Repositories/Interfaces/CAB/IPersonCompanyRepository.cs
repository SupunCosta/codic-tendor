using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Controllers.CAB;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Repositories.Interfaces.CAB;

public interface IPersonCompanyRepository
{
    Task<IEnumerable<CabPersonCompany>> GetPersonCompanyList(
        PersonCompanyRepositoryParameter personCompanyRepositoryParameter);

    Task<CabPerson> GetPersonCompanyById(PersonCompanyRepositoryParameter personCompanyRepositoryParameter);
    Task<string> AddPersonCompany(PersonCompanyRepositoryParameter personCompanyRepositoryParameter);
    Task<bool> DeletePersonCompany(PersonCompanyRepositoryParameter personCompanyRepositoryParameter);
}

public class PersonCompanyRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string PersonCompanyId { get; set; }
    public CabPersonCompany PersonCompany { get; set; }
    public CabHistoryLogRepositoryParameter CabHistoryLogRepositoryParameter { get; set; }
    public ICabHistoryLogRepository CabHistoryLogRepository { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public ILogger<CentralAddressBookController> Logger { get; set; }
}