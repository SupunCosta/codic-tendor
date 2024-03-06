using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces.HR;

public interface IHRRepository
{
    Task<string> CreateHR(HRParameter HRParameter);
    Task<GetHRByIdDto> GetHRById(HRParameter HRParameter);
    Task<IEnumerable<GetHRListDto>> FilterHRList(HRParameter HRParameter);
    Task<PersonById> GetTaxonomyIdByPersonId(HRParameter HRParameter);

    Task<IEnumerable<GetHRRoles>> GetHRRoles(HRParameter HRParameter);
    Task<IEnumerable<PmolData>> GetLabourHistory(HRParameter HRParameter);
    Task<IEnumerable<CabDataDto>> HRPersonFilter(HRParameter HRParameter);
    Task<string> RemoveHr(HRParameter HRParamter);
    Task<List<HRLabourPmolPr>> GetLabourPmolPr(HRParameter HRParameter);
    Task<HRContractorList> CreateHrContractorList(HRParameter HRParameter);
    Task<List<HRContractorList>> GetHrContractorList(HRParameter HRParameter);
    Task<List<string>> DeleteHrContractorList(HRParameter HRParameter);
    Task<List<GetHRContractTypes>> GetHrContractTypes(HRParameter HRParameter);

}

public class HRParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public CreateHRDto CreateHR { get; set; }
    public List<string> IdList { get; set; }
    public FilterHR Filter { get; set; }
    public IVPRepository IVPRepository { get; set; }
    public IPmolRepository IPmolRepository { get; set; }

    public CabPersonFilterModel CabPersonFilter { get; set; }
    public HRContractorList HRContractorListDto { get; set; }

}