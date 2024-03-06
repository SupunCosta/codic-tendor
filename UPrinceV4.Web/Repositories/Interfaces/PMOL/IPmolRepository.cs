using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Repositories.Interfaces.PMOL;

public interface IPmolRepository
{
    Task<IEnumerable<PmolShortcutpaneDataDto>> GetShortcutpaneData(PmolParameter PmolParameter);
    Task<IEnumerable<PmolListDto>> GetPmolList(PmolParameter PmolParameter);
    Task<PmolDropdown> GetDropdownData(PmolParameter PmolParameter);
    Task<PmolGetByIdDto> GetPmolById(PmolParameter PmolParameter);
    Task<Pmol> CreateHeader(PmolParameter PmolParameter, bool isClone);
    Task<string> CreatePmolStartHandshake(PmolParameter pmolParameter);
    Task<string> CreatePmolStopHandshake(PmolParameter pmolParameter);
    Task<string> CreatePmolExtraWork(PmolParameter pmolParameter);
    Task<string> CreateLocation(PmolParameter pmolParameter, bool isLocationEmpty);
    Task<PmolStopHandshakeReadDto> GetStopHandShakesByPmolId(PmolParameter pmolParameter);
    Task<MapLocation> GetLocationByPmolId(PmolParameter pmolParameter);
    Task<bool> DeletePmolExtraWork(PmolParameter pmolParameter);
    Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserId(PmolParameter pmolParameter);

    Task<IEnumerable<ProjectWithPm>> ReadPmolProjectsPM(PmolParameter pmolParameter);
    Task<IEnumerable<PmolReport>> ReadPmolProjectsPMWithDetail(PmolParameter pmolParameter);


    Task<IEnumerable<ProjectWithPm>> ReadBorByProjectsPM(PmolParameter pmolParameter);

    Task<IEnumerable<ProjectWithPm>> ReadPbsByProjectsPM(PmolParameter pmolParameter);


    Task<string> AddPmolStopHandshakeDocuments(PmolParameter pmolParameter);
    Task<PmolExtraWorkReadDto> GetExtraWorkByPmolId(PmolParameter pmolParameter);
    Task<bool> DeletePmolExtraWorkFiles(PmolParameter pmolParameter);
    Task<string> ClonePmol(PmolParameter PmolParameter);
    Task<string> UpdateStartTime(PmolParameter PmolParameter);
    Task<string> UpdateEndTime(PmolParameter PmolParameter);
    Task<List<PmolExtraWorkFilesReadDto>> ReadAudioByPmolId(PmolParameter pmolParameter);
    Task<string> UpdateFinishedStatus(PmolParameter pmolParameter);
    Task<string> UploadImageForMobile(PmolParameter pmolParameter);
    Task<string> UploadAudioForMobile(PmolParameter pmolParameter);
    Task<string> ApprovePmol(PmolParameter pmolParameter);
    Task<string> UpdateCommentMobile(PmolParameter pmolParameter);
    Task<string> ReadLaborCalculation(PmolParameter pmolParameter);

    Task<string> BreakPmol(PmolParameter PmolParameter);
    Task<string> BreakPmolStop(PmolParameter PmolParameter);
    Task<string> CreatePmolService(PmolParameter PmolParameter);
    Task<PmolServiceGetByIdDto> ReadPmolServiceByPmolId(PmolParameter PmolParameter);
    IEnumerable<ProjectWithPm> ProjectPm(string connection);
    Task<PmolDto> GetPmolByIdNew(PmolParameter PmolParameter);
    Task<IEnumerable<ProjectWithPm>> UpdateProjectPm(PmolParameter pmolParameter);
    Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserIdMobile(PmolParameter pmolParameter);
    Task<bool> ForemanCheckMobile(PmolParameter pmolParameter);
    Task<string> UpdateLabourStartTime(PmolParameter PmolParameter);
    Task<string> ClonePmolDayPlanning(PmolParameter PmolParameter);
    Task<string> UpdateLabourpmolEndTime(PmolParameter PmolParameter);
    Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserIdOfLabour(PmolParameter pmolParameter);
    Task<string> BreakLabourStop(PmolParameter PmolParameter);
    Task<string> BreakLabour(PmolParameter PmolParameter);
    Task<string> UpdateEndTimeByForeman(PmolParameter PmolParameter);
    Task<bool> IsForeman(PmolParameter PmolParameter);
    Task<string> UpdatePmolStart(PmolParameter PmolParameter);
    Task<string> UpdatePmolStop(PmolParameter PmolParameter);
    void ForemanAddToPmol(PmolParameter pmolParameter, string projectconString, bool isForeman);
    Task<string> UpdateUserCurrentPmol(PmolParameter pmolParameter);
    Task<string> UpdatePmolLabourEndTime(PmolParameter pmolParameter);
    Task<IEnumerable<PmolPlannedWorkLabourDto>> GetPMolPlannedWorkLabour(PmolParameter pmolParameter);
    Task<BuPersonWithCompetence> GetBuPersonWithCompetence(PmolParameter pmolParameter);
    Task<IEnumerable<PmolData>> GetPmolByPerson(PmolParameter pmolParameter);
    Task<IEnumerable<TrAppDto>> GetTrAppData(PmolParameter pmolParameter);
    Task<string> CreatePersonCommentCard(PmolParameter pmolParameter);
    Task<IEnumerable<PmolPersonCommentCardDto>> GetPersonCommentCard(PmolParameter pmolParameter);
    Task<string> AddPersonComment(PmolParameter pmolParameter);
    Task<string> PmolStatusUpdate(PmolParameter pmolParameter,string statusId,string pmolId);
    Task<IEnumerable<PmolPersonCommentCardDto>> GetPmolCommentCard(PmolParameter pmolParameter);
    Task<IEnumerable<PmolDataForPrint>> PmolDataForPrint(PmolParameter pmolParameter);
    Task<string> ClonePmolMultipleDays(PmolParameter pmolParameter);
    Task<PmolCbcResources> AddPmolCbcResource(PmolParameter pmolParameter);
    Task<List<string>> DeletePmolCbcResource(PmolParameter pmolParameter);
    Task<List<GetPmolCbcResourcesDto>> GetPmolCbcResourcesById(PmolParameter pmolParameter);
    Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserIdMobileChanged(PmolParameter pmolParameter);

}

public class PmolParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public PmolFilter filter { get; set; }
    public string Id { get; set; }
    public PmolCreateDto PmolDto { get; set; }

    public PmolCreateCommentDto PmolCreateCommentDto { get; set; }

    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string AuthToken { get; set; }

    public BorParameter borParameter { get; set; }
    public IBorRepository borRepository { get; set; }
    public PmolStopHandshakeCreateDto pmolStopHandshakeCreateDto { get; set; }
    public PmolStopHandshakeCreateDocumentsDto pmolStopHandshakeCreateDocumentsDto { get; set; }
    public PmolStartHandshakeCreateDto pmolStartHandshakeCreateDto { get; set; }
    public PmolExtraWorkCreateDto PmolExtraWorkCreateDto { get; set; }
    public MapLocation Location { get; set; }
    public IPmolResourceRepository IPmolResourceRepository { get; set; }
    public string UserId { get; set; }
    public List<string> IdList { get; set; }
    public IFormCollection formData { get; set; }
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public PmolServiceCreateDto PmolServiceCreate { get; set; }
    public bool IsApproved { get; set; }
    public DayPlanPmolClone DayPlanPmolClone { get; set; }
    public IConfiguration Configuration { get; set; }

    public PmolLabours PmolLabours { get; set; }

    public PmolLabourEndTime PmolLabourEndTime { get; set; }

    public IShiftRepository IShiftRepository { get; set; }
    public GetPmolByPersonDto GetPmolByPersonDto { get; set; }
    public PmolPersonCommentCardDto PmolPersonCommentCardDto { get; set; }
    public PmolPersonCommentDto PmolPersonComment { get; set; }
    public PmolStatusUpdateDto PmolStatusUpdateDto { get; set; }
    public PmolPerson PmolPerson { get; set; }
    public PmolDataForPrintDto PmolDataForPrintDto { get; set; }
    public bool isMyCal { get; set; }
    public IVPRepository IVpRepository { get; set; }
    
    public MultiplePmolClone MultiplePmolClone { get; set; }
    public PmolCbcResources PmolCbcResources { get; set; }


}

public class PmolDataDto
{
    public PmolDropdown PmolDropdown { get; set; }

    public PbsInstructionsDropdown PbsInstructionsDropdown { get; set; }

    public BorDropdownData BorDropdown { get; set; }

    public PbsTreeStructureDto PbsTreeStructureDto { get; set; }
    public IEnumerable<Roles> Roles { get; set; }

    public RiskDropdown RiskDropdown { get; set; }
}