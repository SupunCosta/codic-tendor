using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IRiskTypeRepository
{
    Task<IEnumerable<RiskType>> GetRiskTypeList(RiskTypeRepositoryParameter riskTypeRepositoryParameter);
    Task<RiskType> GetRiskTypeById(RiskTypeRepositoryParameter riskTypeRepositoryParameter);
    Task<string> AddRiskType(RiskTypeRepositoryParameter riskTypeRepositoryParameter);
    bool DeleteRiskType(RiskTypeRepositoryParameter riskTypeRepositoryParameter);
}

public class RiskTypeRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public List<string> IdList { get; set; }
    public RiskType RiskType { get; set; }
    public string Lang { get; set; }
}