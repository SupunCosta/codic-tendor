using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Repositories.Interfaces.PBS;

public interface IPbsRiskRepository
{
    Task<string> CreatePbsRisk(PbsRiskParameters pbsParameters);
    Task DeletePbsRisk(PbsRiskParameters pbsParameters);
    Task<IEnumerable<RiskReadDapperDto>> GetPbsRiskByPbsProductId(PbsRiskParameters pbsParameters);
    Task<IEnumerable<RiskReadDapperDto>> GetAllPbsRiskByPbsProductId(PbsRiskParameters pbsParameters);
}

public class PbsRiskParameters
{
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public PbsRiskCreateDto PbsRiskCreateDto { get; set; }
    public List<string> IdList { get; set; }
    public string PbsProductId { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
}