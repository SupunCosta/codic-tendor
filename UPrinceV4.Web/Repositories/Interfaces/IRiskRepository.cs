using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.CAB;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IRiskRepository
{
    Task<IEnumerable<RiskReadDto>> GetRiskList(RiskRepositoryParameter riskRepositoryParameter);
    Task<RiskReadDtoDapper> GetRiskById(RiskRepositoryParameter riskRepositoryParameter);
    Task<Risk> AddRisk(RiskRepositoryParameter riskRepositoryParameter);
    bool DeleteRisk(RiskRepositoryParameter riskRepositoryParameter);
    Task<RiskDropdown> GetRiskDropdownData(RiskRepositoryParameter riskRepositoryParameter);
    Task<IEnumerable<RiskReadDtoDapper>> Filter(RiskRepositoryParameter riskRepositoryParameter);
}

public class RiskRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public List<string> IdList { get; set; } // should pass sequence code list as id
    public Risk Risk { get; set; }
    public string RiskId { get; set; } // should pass sequence code as id
    public RiskStatusRepositoryParameter RiskStatusRepositoryParameter { get; set; }
    public RiskTypeRepositoryParameter RiskTypeRepositoryParameter { get; set; }
    public PersonRepositoryParameter PersonRepositoryParameter { get; set; }
    public IRiskStatusRepository IRiskStatusRepository { get; set; }
    public IRiskTypeRepository IRiskTypeRepository { get; set; }
    public IPersonRepository IPersonRepository { get; set; }
    public string Lang { get; set; }
    public RiskFilterModel RiskFilterModel { get; set; }
    public ApplicationUser ChangedUser { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
}

public class RiskDropdown
{
    public IEnumerable<RiskStatus> RiskStatus { get; set; }

    public IEnumerable<RiskType> RiskTypes { get; set; }
    //public IEnumerable<CabDataDto> personList { get; set; }
}