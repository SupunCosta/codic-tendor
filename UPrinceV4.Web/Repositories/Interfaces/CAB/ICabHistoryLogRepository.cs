using System.Collections.Generic;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;

namespace UPrinceV4.Web.Repositories.Interfaces.CAB;

public interface ICabHistoryLogRepository
{
    IEnumerable<CabHistoryLog> GetCabHistoryLog(CabHistoryLogRepositoryParameter cabHistoryLogRepositoryParameter);
    string AddCabHistoryLog(CabHistoryLogRepositoryParameter cabHistoryLogRepositoryParameter);
}

public class CabHistoryLogRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public ApplicationUser ChangedUser { get; set; }
    public CabDataDto CabDataDto { get; set; }
    public PersonDto Person { get; set; }
    public CompanyDto Company { get; set; }
    public PersonCompanyDto PersonCompany { get; set; }
    public string Action { get; set; }
    public ITenantProvider TenantProvider { get; set; }
}