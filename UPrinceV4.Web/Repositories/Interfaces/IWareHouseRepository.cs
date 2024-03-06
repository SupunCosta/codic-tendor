using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.WH;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IWareHouseRepository
{
    Task<IEnumerable<WHShortCutPaneDto>> GetShortcutpaneData(WHParameter WHParameter);

    Task<IEnumerable<WHListDto>> GetWHList(WHParameter WHParameter);

    Task<string> CreateHeader(WHParameter WHParameter);

    Task<WHDropDownData> GetWHDropdown(WHParameter WHParameter);
    Task<WHHeaderDto> GetWHById(WHParameter WHParameter);
    Task<MapLocation> ReadLocation(WHParameter WHParameter);
    Task<string> CreateWHTaxonomy(WHParameter WHParameter);
    Task<IEnumerable<WHTaxonomyListDto>> GetWHTaxonomyList(WHParameter WHParameter);

    Task<List<WHRockCpcList>> GetWHRockCpcList(WHParameter WHParameter);
    Task<StockHeader> GetWHRockCpcById(WHParameter WHParameter);
    Task<string> SaveWHRockCpc(WHParameter WHParameter);
    Task<string> UploadImageForMobile(WHParameter WHParameter);
}

public class WHParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string LocationId { get; set; }
    public WHCreateDto WHDto { get; set; }
    public string UserId { get; set; }
    public WHFilter Filter { get; set; }
    public WHTaxonomyCreateDto WHTaxonomyDto { get; set; }
    public WHTaxonomyFilterDto WHTaxonomyFilter { get; set; }

    public WHRockCpcFilter WHRockCpcFilter { get; set; }
    public WHRockCpcDto WHRockCpcDto { get; set; }
    public IFormCollection formData { get; set; }
}