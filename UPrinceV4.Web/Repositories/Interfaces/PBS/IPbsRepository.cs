using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.VisualPlaane;

namespace UPrinceV4.Web.Repositories.Interfaces.PBS;

public interface IPbsRepository
{
    Task<List<ShortcutPaneData>> getShortcutPaneData(PbsParameters pbsParameters);
    Task<PbsProduct> CreatePbs(PbsParameters pbsParameters);
    Task<IEnumerable<Pbs>> GetPbs(PbsParameters pbsParameters);

    Task<IEnumerable<PbsPo>> GetPbsPO(PbsParameters pbsParameters);

    Task<IEnumerable<PbsProductDto>> GetPbsFilterHasBor(PbsParameters pbsParameters);
    Task<PbsDropdownData> GetPbsDropdown(PbsParameters pbsParameters);
    public Task<PbsTreeStructureDto> GetTreeStructureData(PbsParameters pbsParameters);
    Task<PbsGetByIdDto> GetPbsById(PbsParameters pbsParameters);
    Task DeletePbs(PbsParameters pbsParameters);
    Task<TaxonomyLevelReadDto> GetTaxonomyLevels(PbsParameters pbsParameters);
    Task<string> CreateNode(PbsParameters pbsParameters);
    Task<IEnumerable<PbsProductDto>> GetProductByTaxonomyLevel(PbsParameters pbsParameters);
    Task<string> ClonePbs(PbsParameters pbsParameters);
    Task<ProductResourceListGetByIdsDto> ProductResourcesByIdById(PbsParameters pbsParameters);

    Task<IEnumerable<PbsTreeStructurefroProjectPlanningDto>> GetUtilityTaxonomyForProjectPlanning(
        PbsParameters pbsParameters);

    Task<IEnumerable<PbsTreeStructurefroProjectPlanningDto>> GetLocationTaxonomyForProjectPlanning(
        PbsParameters pbsParameters);

    Task<string> CreatePbsNew(PbsParameters pbsParameters);
    Task<string> ExcelUpload(PbsParameters pbsParameters);
    Task<List<PbsTreeStructure>> GetMachineTaxonomyForProjectPlanning(PbsParameters pbsParameters);
    Task<string> CreatePbsScopeOfWork(PbsParameters pbsParameters);
    Task<IEnumerable<PbsForWeekPlanDto>> GetPbsLabour(PbsParameters pbsParameters);
    Task<IEnumerable<AllPbsForWeekPlanDto>> GetAllPbsLabour(PbsParameters pbsParameters);
    Task<GetAllPmolLabourDto> GetAllPmolLabour(PbsParameters pbsParameters);
    Task<int> AddPbsTreeIndex(string Id, string Connection);
    Task<PbsRelationsDto> GetPbsRelations(PbsParameters pbsParameters);
    Task<PbsRelationsDto> GetCpcRelations(PbsParameters pbsParameters);
    Task<PbsCbcResources> AddPbsCbcResource(PbsParameters pbsParameters);
    Task<List<string>> DeletePbsCbcResource(PbsParameters pbsParameters);
    Task<List<GetPbsCbcResourcesDto>> GetPbsCbcResourcesById(PbsParameters pbsParameters);
    Task<List<PmolDeliverableResults>> GetPmolDeliverablesByPbsId(PbsParameters pbsParameters);
    Task<PbsLotDataDto> GetPbsLotIdById(PbsParameters pbsParameters);

}

public class PbsParameters
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public PbsProductCreateDto PbsDto { get; set; }
    public PbsCloneDto PbsCloneDto { get; set; }

    public PbsFilter Filter { get; set; }
    public string TaxonomyId { get; set; }
    public ApplicationUser ChangedUser { get; set; }
    public string Id { get; set; }
    public List<string> IdList { get; set; }
    public PbsProductNodeCreateDto nodeDto { get; set; }
    public PbsFilterByTaxonomyLevel taxonomyLevelFilter { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public IPbsResourceRepository IPbsResourceRepository { get; set; }

    public List<UploadExcelDto> UploadExcelDto { get; set; }

    public PbsTreeStructureFilter PbsTreeStructureFilter { get; set; }
    public PbsScopeOfWork PbsSquareMeters { get; set; }
    public PbsForWeekPlanDto PbsForWeekPlanDto { get; set; }
    public CreatePmolDto CreatePmolDto { get; set; }
    public GetPmolLabourDto GetPmolLabourDto { get; set; }
    public CpcRelationsDto CpcRelationsDto { get; set; }
    
    public PbsCbcResources PbsCbcResourcesDto { get; set; }
    
    public MyCalCreatePmolDto MyCalCreatePmolDto { get; set; }
    public string Title { get; set; }

}