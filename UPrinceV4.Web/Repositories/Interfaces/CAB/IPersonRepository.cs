using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Contract;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories.Interfaces.CAB;

public interface IPersonRepository
{
    Task<IEnumerable<CabDataDto>> GetPersonList(PersonRepositoryParameter personRepositoryParameter);
    Task<IEnumerable<CabDataDto>> GetPersonListByName(PersonRepositoryParameter personRepositoryParameter);
    Task<CabDataDto> GetPersonById(PersonRepositoryParameter personRepositoryParameter);
    Task<string> AddPerson(PersonRepositoryParameter personRepositoryParameter);
    Task<bool> DeletePerson(PersonRepositoryParameter personRepositoryParameter);
    Task<string> UploadImage(PersonRepositoryParameter personRepositoryParameter);
    Task<IEnumerable<ProjectPersonFilterDto>> Filter(PersonRepositoryParameter personRepositoryParameter);
    Task<IEnumerable<ProjectPersonFilterDto>> ProjectPersonFilter(PersonRepositoryParameter personRepositoryParameter);
    Task<bool> IsExistCabEntry(PersonRepositoryParameter personRepositoryParameter);

    Task<IEnumerable<CabPersonNameFilterDto>> PersonFilterByName(
        PersonRepositoryParameter personRepositoryParameter);

    Task<IEnumerable<CabPersonNameFilterDto>> ForemanFilterByName(
        PersonRepositoryParameter personRepositoryParameter);

    Task<string> GetMobileNumberId(PersonRepositoryParameter personRepositoryParameter);
    Task<string> GetEmailByPersonId(PersonRepositoryParameter personRepositoryParameter);
    Task<string> CreateCabCompetencies(PersonRepositoryParameter personRepositoryParameter);
    Task<IEnumerable<GetCabCompetencies>> GetCabCompetencies(PersonRepositoryParameter personRepositoryParameter);
    Task<string> DeleteCabCompetencies(PersonRepositoryParameter personRepositoryParameter);
    Task<string> CreateCabCertification(PersonRepositoryParameter personRepositoryParameter);
    Task<IEnumerable<GetCabCertification>> GetCabCertification(PersonRepositoryParameter personRepositoryParameter);
    Task<IEnumerable<GetCabCertification>> GetCabCertificationCiaw(PersonRepositoryParameter personRepositoryParameter);

    Task<string> DeleteCabCertification(PersonRepositoryParameter personRepositoryParameter);

    Task<IEnumerable<ContractTaxonomyDto>> GetContractorTaxonomyList(
        PersonRepositoryParameter personRepositoryParameter);

    Task<string> CreateContractorTaxonomy(PersonRepositoryParameter personRepositoryParameter);
}

public class PersonRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string PersonId { get; set; }
    public string Name { get; set; }
    public CabPerson Person { get; set; }
    public CabHistoryLogRepositoryParameter CabHistoryLogRepositoryParameter { get; set; }
    public ICabHistoryLogRepository CabHistoryLogRepository { get; set; }
    public IFormCollection Image { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public CabPersonFilterModel CabPersonFilter { get; set; }

    public OrganizationCabPersonFilterModel OrganizationCabPersonFilter { get; set; }
    public ICompanyRepository CompanyRepository { get; set; }
    public List<string> IdListForDelete { get; set; }
    public string ProjectSqCode { get; set; }
    public CabCompetencies CabCompetenciesDto { get; set; }
    public CabCertification CabCertificationDto { get; set; }
    public string Lang { get; set; }
    public ContractTaxonomyDto ContractTaxonomyDto { get; set; }
    public string NationalityId { get; set; }
    public GetCabCertificationDto GetCabCertificationDto { get; set; }
}