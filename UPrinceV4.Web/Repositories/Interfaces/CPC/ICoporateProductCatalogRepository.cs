using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.CPC;

public interface ICoporateProductCatalogRepository
{
    Task<CorporateProductCatalogDto> CreateCoporateProductCatalog(CpcParameters cpcParameters,
        IHttpContextAccessor ContextAccessor);

    Task<CpcDropdown> GetCpcDropdown(CpcParameters cpcParameters);
    Task<string> UploadImage(CpcParameters cpcParameters);
    Task DeleteVendor(CpcParameters cpcParameters);
    Task DeleteImage(CpcParameters cpcParameters);
    Task DeleteNickName(CpcParameters cpcParameters);
    Task<CorporateProductCatalogDto> GetCorporateProductCatalogById(CpcParameters cpcParameters);
    Task<IEnumerable<CoperateProductCatalogFilterNewDto>> GetCorporateProductCatalog(CpcParameters cpcParameters);
    Task<IEnumerable<CpcResourceType>> GetShortcutPaneData(CpcParameters cpcParameters);
    Task DeleteCpc(CpcParameters cpcParameters);
    Task<IEnumerable<CpcExporter>> GetCpcToExport(CpcParameters cpcParameters);
    Task<IEnumerable<CpcResourceType>> getCpcResouceType(CpcParameters cpcParameters);
    Task<CpcForBorDto> GetCpcForBorById(CpcParameters cpcParameters);
    Task<CpcDetails> GetCpcDetailsById(CpcParameters cpcParameters);
    Task<IEnumerable<CpcForProductDto>> ReadVehiclesForQr(CpcParameters cpcParameters);
    Task<IEnumerable<CoperateProductCatalogFilterDto>> MissingCPC(CpcParameters cpcParameters);
    Task<IEnumerable<CoperateProductCatalogFilterNewDto>> CpcLobourFilterMyEnv(CpcParameters cpcParameters);
}

public class CpcParameters
{
    public ApplicationDbContext Context { get; set; }
    public CoperateProductCatalogCreateDto CpcDto { get; set; }
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public IFormCollection Image { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public IEnumerable<string> IdList { get; set; }
    public string Id { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public CpcFilter filter { get; set; }
    public bool isCopy { get; set; }
    public string Oid { get; set; }
    public string Title { get; set; }
    public CpcLobourFilterMyEnvDto CpcLobourFilterMyEnvDto { get; set; }
}