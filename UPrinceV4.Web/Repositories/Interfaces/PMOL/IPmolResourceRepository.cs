using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Repositories.Interfaces.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.PMOL;

public interface IPmolResourceRepository
{
    Task<string> ReadConsumableFromBor(PmolResourceParameter parameter, string PmolId);
    Task<string> ReadLabourFromBor(PmolResourceParameter parameter, string PmolId);
    Task<string> ReadMaterialFromBor(PmolResourceParameter parameter, string PmolId);

    Task<string> ReadToolsFromBor(PmolResourceParameter parameter, string PmolId);

    //Task<string>  CopyResourcesFromBorToPmol(PmolResourceParameter parameter, string PmolId);
    Task<string> CreateConsumable(PmolResourceParameter parameter);
    Task<string> CreateMaterial(PmolResourceParameter parameter);
    Task<string> CreateLabour(PmolResourceParameter parameter);
    Task<string> CreateLabourForMobileApp(PmolResourceParameter parameter);
    Task<string> CreateTools(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadConsumable(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadMaterial(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadLabour(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadTools(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadToolsForDayPlanning(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadExtraConsumable(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadExtraMaterial(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadExtraLabour(PmolResourceParameter parameter);
    Task<IEnumerable<PmolResourceReadDto>> ReadExtraTools(PmolResourceParameter parameter);
    Task<string> DeleteConsumable(PmolResourceParameter parameter);
    Task<string> DeleteLabour(PmolResourceParameter parameter);
    Task<string> DeleteMaterial(PmolResourceParameter parameter);
    Task<string> DeleteTools(PmolResourceParameter parameter);
    Task<string> CreateTeamRole(PmolResourceParameter parameter);
    Task<IEnumerable<PmolTeamRoleReadDto>> ReadPlannedTeamMember(PmolResourceParameter parameter);
    Task<IEnumerable<PmolTeamRoleReadDto>> ReadExtraTeamMember(PmolResourceParameter parameter);
    Task<string> DeleteTeamMember(PmolResourceParameter parameter);
    Task<string> CreateMaterialForMobile(PmolResourceParameter parameter);
    Task<string> CreateLabourForMobile(PmolResourceParameter parameter);
    Task<string> CreateConsumableForMobile(PmolResourceParameter parameter);
    Task<string> CreateToolsForMobile(PmolResourceParameter parameter);
    Task<IEnumerable<PmolTeamRoleReadDto>> ReadPlannedTeamMemberForMobile(PmolResourceParameter parameter);
    Task<IEnumerable<PmolTeamRoleReadDto>> ReadExtraTeamMemberForMobile(PmolResourceParameter parameter);
    Task<string> CreateLabourTeamForMobileApp(PmolResourceParameter parameter);
    Task<string> UpdateLabourTeamForMobileApp(PmolResourceParameter parameter);
    Task<string> RemovePersonFromPmol(PmolResourceParameter parameter);

}

public class PmolResourceParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string Id { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public PmolResourceCreateDto ResourceCreateDto { get; set; }
    public PmolResourceCreateMobileDto ResourceCreateMobileDto { get; set; }
    public ICoporateProductCatalogRepository ICoporateProductCatalogRepository { get; set; }
    public List<string> IdList { get; set; }
    public PmolTeamRoleCreateDto pmolTeamCreateDto { get; set; }

    public bool isWeb { get; set; } = false;
    public IVPRepository VpRepository;
    
    public RemovePersonFromPmol RemovePersonFromPmol;
    public IConfiguration Configuration { get; set; }

}