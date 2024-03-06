using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IOrganizationSettingsRepository
{
    Task<string> CreateOrganizationCompetencies(OSParameter OSParameter);
    Task<IEnumerable<GetOrganizationCompetencies>> GetOrganizationCompetenceList(OSParameter OSParameter);
    Task<GetOrganizationCompetencies> OrganizationCompetenceGetById(OSParameter OSParameter);
    Task<OrganizationCompetenciesDownData> GetOrganizationCompetenceDropdown(OSParameter OSParameter);
    Task<string> CreateCompetenceTaxonomy(OSParameter OSParameter);
    Task<IEnumerable<CompetenciesTaxonomyList>> GetCompetenceTaxonomyList(OSParameter OSParameter);
    Task<string> CreateOrganizationCertification(OSParameter OSParameter);
    Task<IEnumerable<GetOrganizationCertification>> GetOrganizationCertificationList(OSParameter OSParameter);
    Task<GetOrganizationCertification> OrganizationCertificationGetById(OSParameter OSParameter);
    Task<OrganizationCertificationDownData> GetOrganizationCertificationDropdown(OSParameter OSParameter);
    Task<string> CreateCertificationTaxonomy(OSParameter OSParameter);
    Task<IEnumerable<CertificationTaxonomyList>> GetCertificationTaxonomyList(OSParameter OSParameter);
    Task<IEnumerable<CompetenciesTaxonomyLevel>> GetCompetenceTaxonomyLevels(OSParameter OSParameter);

    Task<string> CreateOrganization(OSParameter OSParameter);
    Task<IEnumerable<GetOrganization>> GetOrganizationList(OSParameter OSParameter);
    Task<GetOrganization> OrganizationGetById(OSParameter OSParameter);
    Task<IEnumerable<OrganizationTaxonomyLevel>> GetOrganizationTaxonomyLevel(OSParameter OSParameter);
    Task<string> CreateOrganizationTaxonomy(OSParameter OSParameter);
    Task<IEnumerable<OrganizationTaxonomyList>> GetOrganizationTaxonomyList(OSParameter OSParameter);

    Task<IEnumerable<OrganizationTaxonomyListForProjectPlan>> GetOrganizationTaxonomyListForProjectPlan(
        OSParameter OSParameter);
    
    Task<IEnumerable<OrganizationTaxonomyList>> GetOrganizationTaxonomyListForMyCalender(
        OSParameter OSParameter);

    Task<string> CreateCorporateShedule(OSParameter OSParameter);
    Task<IEnumerable<CorporateShedule>> GetCorporateSheduleList(OSParameter OSParameter);
    Task<CorporateSheduleDto> CorporateSheduleGetById(OSParameter OSParameter);
    Task<string> DeleteOrganizationTaxonomyNode(OSParameter OSParameter);
    Task<OrganizationDownData> GetOrganizationDropdown(OSParameter OSParameter);
    Task<IEnumerable<ProjectPersonFilterDto>> Filter(OSParameter OSParameter);
    Task<IEnumerable<ProjectPersonFilterDto>> PersonFilterForBu(OSParameter OSParameter);
    Task<IEnumerable<ProjectPersonFilterDto>> PersonFilterForBuTeam(OSParameter OSParameter);
    Task<string> OrganizationTaxonomySetDefaultBu(OSParameter OSParameter);
}

public class OSParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public OrganizationCompetenciesCreate OrganizationCompetenciesCreate { get; set; }
    public CompetenciesTaxonomy CompetenciesTaxonomyDto { get; set; }
    public CompetenciesTaxonomyFilter TaxonomyFilter { get; set; }
    public OrganizationCertificationCreate OrganizationCertificationCreate { get; set; }
    public CertificationTaxonomy CertificationTaxonomyDto { get; set; }
    public OrganizationCreate OrganizationCreate { get; set; }
    public OrganizationTaxonomyDto OrganizationTaxonomyDto { get; set; }
    public CorporateSheduleDto CSDto { get; set; }
    public OrganizationTaxonomyFilter Filter { get; set; }
    public CorporateSheduleList CSList { get; set; }
    public OrganizationCabPersonFilterModel organizationCabPersonFilter { get; set; }
    public OrganizationCabPersonFilterDto OrganizationCabPersonFilterDto { get; set; }
    public OrganizationTaxonomyBu OrganizationTaxonomyBu { get; set; }
}