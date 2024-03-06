using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.DD;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IDropDownRepository
{
    Task<DropDownList> DropDownFilter(DDParameter DDParameter);
    Task<CoporateProductCatalog> FilterCPC(DDParameter DDParameter);
    Task<Project> FilterProject(DDParameter DDParameter);
    Task<ProjectBreakDownStructure> FilterPBS(DDParameter DDParameter);

    Task<ProjectMolecule> FilterProjectMolecule(DDParameter DDParameter);
    //Task<BillOfResource> FilterBillOfResource(DDParameter DDParameter);

    Task<IEnumerable<RoleDto>> GetProjectRoleByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddProjectRole(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteProjectRole(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetStockResourceByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddStockResource(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteStockResource(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetStockTypeByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddStockType(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteStockType(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetStockStatusByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddStockStatus(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteStockStatus(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetWFStatusByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddWFStatus(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteWFStatus(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetWFTypeByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddWFType(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteWFType(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetWHTypeByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddWHType(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteWHType(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetWHStatusByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddWHStatus(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteWHStatus(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetWHTaxonomyLevelByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddWHTaxonomyLevel(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteWHTaxonomyLevel(DDParameter DDParameter);
    Task<List<DatabasesException>> Migration(string env, string sql, object param);
    Task<IEnumerable<VPOrganisationShortcutPaneDto>> GetVPOrganisationShortcutPaneByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddVPOrganisationShortcutPane(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteVPOrganisationShortcutPane(DDParameter DDParameter);

    Task<IEnumerable<OrganizationTaxonomyLevelDropdown>>
        GetOrganizationTaxonomyLevelByCode(DDParameter DDParameter);

    Task<IEnumerable<DatabasesException>> AddOrganizationTaxonomyLevel(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteOrganizationTaxonomyLevel(DDParameter DDParameter);
    Task<IEnumerable<StockDropdownAddDto>> GetPOShortcutPaneByCode(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> AddPOShortcutPane(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeletePOShortcutPane(DDParameter DDParameter);

    Task<IEnumerable<OrganizationTaxonomyLevelDropdown>>
        GetCompetenciesTaxonomyLevelByCode(DDParameter DDParameter);

    Task<IEnumerable<DatabasesException>> AddCompetenciesTaxonomyLevel(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteCompetenciesTaxonomyLevel(DDParameter DDParameter);

    Task<IEnumerable<OrganizationTaxonomyLevelDropdown>> GetCertificationTaxonomyLevelByCode(
        DDParameter DDParameter);

    Task<IEnumerable<DatabasesException>> AddCertificationTaxonomyLevel(DDParameter DDParameter);
    Task<IEnumerable<DatabasesException>> DeleteCertificationTaxonomyLevel(DDParameter DDParameter);
    Task<IEnumerable<GetPOTypeDto>> GetPoTypeByCode(DDParameter DDParameter);
    Task<IEnumerable<GetPOStatusDto>> GetPoStatusByCode(DDParameter DDParameter);
    Task<string> AddPoType(DDParameter DDParameter);
    Task<string> AddPoStatus(DDParameter DDParameter);
}

public class DDParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Code { get; set; }
    public RoleDto DdDto { get; set; }
    public StockDropdownAddDto Dto { get; set; }
    public DropDownList DropDown { get; set; }
    public CpcBasicUnitOfMeasure CpcBasicUnitOfMeasure { get; set; }
    public OrganizationTaxonomyLevelDropdown LevelCreateDto { get; set; }
    public CreatePOType CreatePOType { get; set; }
    public CreatePOStatus CreatePOStatus { get; set; }
}