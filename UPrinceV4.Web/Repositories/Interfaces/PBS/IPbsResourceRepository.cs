using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.PBS;

public interface IPbsResourceRepository
{
    Task<string> CreatePbsLabour(PbsResourceParameters pbsParameters);
    Task<string> CreatePbsMaterial(PbsResourceParameters pbsParameters);
    Task<string> CreatePbsTool(PbsResourceParameters pbsParameters);
    Task<string> CreatePbsConsumable(PbsResourceParameters pbsParameters);
    Task DeletePbsLabour(PbsResourceParameters pbsParameters);
    Task DeletePbsMaterial(PbsResourceParameters pbsParameters);
    Task DeletePbsTool(PbsResourceParameters pbsParameters);
    Task DeletePbsConsumable(PbsResourceParameters pbsParameters);
    Task<IEnumerable<CpcForProductDto>> GetMaterial(PbsResourceParameters pbsParameters);
    Task<IEnumerable<CpcForProductDto>> GetLabour(PbsResourceParameters pbsParameters);
    Task<IEnumerable<CpcForProductDto>> GetTool(PbsResourceParameters pbsParameters);
    Task<IEnumerable<CpcForProductDto>> GetConsumable(PbsResourceParameters pbsParameters);
    Task<IEnumerable<PbsResourceReadDto>> GetMaterialByProductId(PbsResourceParameters pbsResourceParameters);
    Task<IEnumerable<PbsResourceReadDto>> GetToolByProductId(PbsResourceParameters pbsResourceParameters);
    Task<IEnumerable<PbsResourceReadDto>> GetLabourByProductId(PbsResourceParameters pbsResourceParameters);
    Task<IEnumerable<PbsResourceReadDto>> GetConsumableByProductId(PbsResourceParameters pbsResourceParameters);
    Task<string> CreatePbsService(PbsResourceParameters pbsParameters);
    Task<string> PbsLabourAssign(PbsResourceParameters pbsParameters);
    Task<string> PbsLabourAssignReCalculate(PbsResourceParameters pbsParameters);
    Task<string> PbsAssignedLabourDelete(PbsResourceParameters pbsParameters);
    Task<IEnumerable<PbsServiceGetByIdDto>> ReadServiceByPbsProduct(PbsResourceParameters pbsResourceParameters);
    Task<List<DatabasesEx>> PbsResourcesEnvironment(PbsResourceParameters pbsResourceParameters);

    Task<string> CopyCpcFromCuToProject(PbsResourceParameters pbsParameters, string coperateProductCatalogId,
        string connectionString, string environment);

    Task<ReadAllPbsResourceReadDto> ReadPbsResourcesByPbsProduct(PbsResourceParameters pbsResourceParameters);

    Task<List<ConsolidateReadDto>> GetPbsResourceConsolidatedQuantity(PbsResourceParameters pbsParameters,string table);
    Task PbsParentDateAdjust(PbsResourceParameters pbsResourceParameters);

}

public class PbsResourceParameters
{
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public LabourForPbsCreateDto PbsLabourCreateDto { get; set; }
    public MaterialForPbsCreateDto PbsMaterialCreateDto { get; set; }
    public ConsumableForPbsCreateDto PbsConsumableCreateDto { get; set; }
    public ToolsForPbsCreateDto PbsToolCreateDto { get; set; }
    public List<string> IdList { get; set; }
    public string PbsProductId { get; set; }
    public string Name { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public CpcParameters cpcParameter { get; set; }
    public ICoporateProductCatalogRepository ICoporateProductCatalogRepository { get; set; }
    public IHttpContextAccessor ContextAccessor { get; set; }
    public PbsServiceCreateDto PbsServiceCreateDto { get; set; }
    public string UserId { get; set; }
    public PbsAssignedLabourDto PbsAssignedLabour { get; set; }
    public IBorRepository BorRepository { get; set; }
    public IBorResourceRepository _iBorResourceRepositoryRepository { get; set; }
    public IVPRepository IVPRepository { get; set; }
    public IConfiguration Configuration { get; set; }
    public IProjectDefinitionRepository ProjectDefinitionRepository { get; set; }
    public ApplicationDbContext uPrinceCustomerContext { get; set; }
    public PbsAssignedLabourDeleteDto PbsAssignedLabourDelete { get; set; }
}

public class PbsResourceReadDto
{
    public string ResourceId { get; set; }
    public string CpcTitle { get; set; }
    public string CpcKey { get; set; }
    public string CpcText { get; set; }
    public string ResourceFamilyTitle { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; } // CPCUnitOfMeasure  
    public double? ConsolidatedQuantity { get; set; }
    public string ResourceTypeId { get; set; }
    public string PbsId { get; set; }

}

public class ReadAllPbsResourceReadDto
{
    public List<PbsResourceReadDto> Materials { get; set; }
    public List<PbsResourceReadDto> Tools { get; set; }
    public List<PbsResourceReadDto> Labours { get; set; }
    public List<PbsResourceReadDto> Consumables { get; set; }
}

public class ConsolidateReadDto
{
    public string PbsId { get; set; }
    public double? TotalConsolidatedQuantity { get; set; }
    public List<PbsResourceReadDto> res { get; set; }
}