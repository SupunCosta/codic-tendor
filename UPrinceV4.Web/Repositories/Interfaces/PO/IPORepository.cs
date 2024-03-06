using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Repositories.Interfaces.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.PO;

public interface IPORepository
{
    Task<IEnumerable<POShortcutpaneDataDto>> GetShortcutpaneData(POParameter POParameter);
    Task<IEnumerable<POListDto>> GetPOList(POParameter POParameter);

    Task<IEnumerable<POBORResources>> POResourceFilter(POParameter POParameter);
    Task<IEnumerable<POBORResources>> CUPOResourceFilter(POParameter POParameter);

    Task<IEnumerable<POResourceStockDto>> POResourceStockUpdate(POParameter POParameter);
    Task<IEnumerable<POResourceStockDto>> CUPOResourceStockUpdate(POParameter POParameter);


    Task<IEnumerable<POProductIsPoDto>> POProductIsPoUpdate(POParameter POParameter);
    Task<IEnumerable<POProductIsPoDto>> CUPOProductIsPoUpdate(POParameter POParameter);

    Task<string> CreateHeader(POParameter POParameter, bool isCopy, bool toCu);

    Task<PODropdownData> GetPbsDropdown(POParameter POParameter, CancellationToken cancellationToken);

    Task<POHeaderDto> GetPOById(POParameter POParameter);
    Task<string> ProjectSend(POParameter POParameter);

    Task<string> CUSend(POParameter POParameter);


    Task<string> CUApprove(POParameter POParameter);

    Task<string> CULevelApprove(POParameter POParameter);


    Task<string> PMApprove(POParameter POParameter);

    Task<string> CUFeedback(POParameter POParameter);

    Task<string> PMAccept(POParameter POParameter);


    Task<string> CreateLocation(POParameter POParameter);
    Task<MapLocation> ReadLocation(POParameter POParameter);

    Task<IEnumerable<ProjectDefinitionMobDto>> POProjectList(POParameter POParameter);

    Task<IEnumerable<POResourcesExcelDto>> POBorResourceFilter(POParameter POParameter);
    Task<IEnumerable<POProductDto>> POPBSResourceFilter(POParameter POParameter);
    Task<string> CreateWorkFlow(POParameter POParameter, string connectionString, string action);
    Task<string> CUSendCreateWorkFlow(POParameter POParameter, string connectionString, string action);
    Task<string> CUSendUpdateVendor(POParameter POParameter, string connectionString);
    Task<IEnumerable<POResourceStockDto>> POBorUpdate(POParameter POParameter);
    Task<IEnumerable<POResourceStockDto>> CUPOBorUpdate(POParameter POParameter);
    Task<string> ConvertToPo(POParameter POParameter);
    Task<string> AddPoLabourTeam(POParameter POParameter);
    Task<IEnumerable<GetPOLabourTeam>> GetPoLabourTeam(POParameter POParameter);

    Task<string> AddPoToolPool(POParameter POParameter);
}

public class POParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }

    public ITenantProvider TenantProvider { get; set; }

    // public PODto PODto { get; set; }
    public string Title { get; set; }
    public POFilter Filter { get; set; }

    public POResourceFilterDto POResourceFilterDto { get; set; }

    public POResourceStockUpdate POResourceStockUpdate { get; set; }

    public POProductIsPoUpdate POProductIsPoUpdate { get; set; }

    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> idList { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public POCreateDto PoDto { get; set; }
    public POHistoryDto POHistoryDto { get; set; }
    public POResourcesAddDto POResources { get; set; }
    public List<POProduct> ExternalProduct { get; set; }

    public POApproveDto POApprove { get; set; }

    public MapLocation MapLocation { get; set; }

    public string LocationId { get; set; }
    public POBorResourceFilter POBorResourceFilter { get; set; }
    public POPbsResourceFilter POPbsResourceFilter { get; set; }
    public CpcParameters CpcParameters { get; set; }

    public ICoporateProductCatalogRepository ICoporateProductCatalogRepository { get; set; }
    public WFCreateDto WFDto { get; set; }
    public POLabourTeam POLabourTeam { get; set; }
    public POToolPool POToolPool { get; set; }
    public IConfiguration Configuration { get; set; }
}