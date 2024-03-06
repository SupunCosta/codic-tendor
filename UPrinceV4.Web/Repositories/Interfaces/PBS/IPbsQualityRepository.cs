using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Repositories.Interfaces.PBS;

public interface IPbsQualityRepository
{
    Task<string> CreatePbsQuality(PbsQualityParameters pbsParameters);
    Task DeletePbsQuality(PbsQualityParameters pbsParameters);
    Task<IEnumerable<QualityDapperDto>> GetPbsQualityByPbsProductId(PbsQualityParameters pbsQualityParameters);
    Task<IEnumerable<QualityDapperDto>> GetAllPbsQualityByPbsProductId(PbsQualityParameters pbsQualityParameters);
}

public class PbsQualityParameters
{
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public PbsQualityCreateDto PbsQualityCreateDto { get; set; }
    public List<string> IdList { get; set; }
    public string PbsProductId { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
}