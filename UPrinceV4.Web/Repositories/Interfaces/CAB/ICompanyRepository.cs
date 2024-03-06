using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Repositories.Interfaces.CAB;

public interface ICompanyRepository
{
    Task<IEnumerable<CompanyDto>> GetCompanyList(CompanyRepositoryParameter companyRepositoryParameter);

    Task<IEnumerable<UnassignedCompanyDto>> GetUnassignedCompanyList(
        CompanyRepositoryParameter companyRepositoryParameter);

    Task<CompanyDto> GetCompanyById(CompanyRepositoryParameter companyRepositoryParameter);
    Task<string> AddCompany(CompanyRepositoryParameter companyRepositoryParameter);
    Task<bool> DeleteCompany(CompanyRepositoryParameter companyRepositoryParameter);
    Task<IEnumerable<CompanyDto>> GetCompanyListByName(CompanyRepositoryParameter companyRepositoryParameter);

    Task<IEnumerable<GroupByCompanyDto>> GetCompanyListGroupByCompany(
        CompanyRepositoryParameter companyRepositoryParameter);
}

public class CompanyRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public CabCompany Company { get; set; }
    public CabHistoryLogRepositoryParameter CabHistoryLogRepositoryParameter { get; set; }
    public ICabHistoryLogRepository CabHistoryLogRepository { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public List<string> IdListForDelete { get; set; }
}