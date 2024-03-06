using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IRiskStatusRepository
{
    Task<IEnumerable<RiskStatus>> GetRiskStatusList(RiskStatusRepositoryParameter riskStatusRepositoryParameter);
    Task<RiskStatus> GetRiskStatusById(RiskStatusRepositoryParameter riskStatusRepositoryParameter);
    Task<string> AddRiskStatus(RiskStatusRepositoryParameter riskStatusRepositoryParameter);
    bool DeleteRiskStatus(RiskStatusRepositoryParameter riskStatusRepositoryParameter);
}

public class RiskStatusRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public List<string> IdList { get; set; }
    public RiskStatus RiskStatus { get; set; }
    public string Lang { get; set; }
}