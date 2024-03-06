using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.BOR;

public interface IBorRepository
{
    Task<string> CreateBor(BorParameter borParameter);
    Task<IEnumerable<BorShortcutPaneDto>> GetShortcutPaneData(BorParameter borParameter);
    Task<IEnumerable<PbsProductDto>> GetProduct(BorParameter borParameter);
    Task<IEnumerable<BorListDto>> GetBorList(BorParameter borParameter);
    Task<IEnumerable<BorListDto>> FilterBorPo(BorParameter borParameter);
    Task<IEnumerable<BorResourceListDto>> GetBorResourceList(BorParameter borParameter);

    Task<BorGetByIdDto> GetBorById(BorParameter borParameter);
    Task<BorDropdownData> GetBorDropdownData(BorParameter borParameter);
    Task<string> UpdateBorStatus(BorParameter borParameter);
    Task<IEnumerable<BorListDto>> GetBorListByProduct(BorParameter borParameter);

    Task<BorResourceListGetByIdsDto> GetBorResourcesbyIds(BorParameter borParameter);
    Task<IEnumerable<BorListDto>> FilterReturnBorPo(BorParameter borParameter);
}

public class BorParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public BorDto BorDto { get; set; }
    public string Title { get; set; }
    public BorFilter BorFilter { get; set; }
    public string Id { get; set; }

    public List<string> idList { get; set; }

    public BorResource BorResourceCreateDto { get; set; }
    public IBorResourceRepository IBorResourceRepository { get; set; }
    public BorResourceFilter BorResourceFilter { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public ICoporateProductCatalogRepository ICoporateProductCatalogRepository { get; set; }
    public CpcParameters CpcParameters { get; set; }
    public BorStatusUpdateDto borStatusUpdateDto { get; set; }
}

public class BorParameterResoruce
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }

    public BorResourceUpdate borResourceUpdate { get; set; }
}

public class BorParameterResoruceDelete
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public List<string> idList { get; set; }
}