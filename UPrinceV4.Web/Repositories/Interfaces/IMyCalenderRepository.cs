using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IMyCalenderRepository
{
    Task<IEnumerable<TeamsWithPmolDto>> Teams(MyCalenderParameter myCalenderParameter);
    Task<IEnumerable<TeamsWithPmolDto>> MyCalenderListData(MyCalenderParameter myCalenderParameter);
    Task<IEnumerable<TeamsWithPmolDto>> MyCalenderListDataForCu(MyCalenderParameter myCalenderParameter);
    Task<IEnumerable<MyCalanderProjectDto>> MyCalenderProjectFlter(MyCalenderParameter myCalenderParameter);
    Task<IEnumerable<PbsTreeStructure>> GetMyCalenderPbsTaxonomy(MyCalenderParameter myCalenderParameter); 
    Task<Pmol> MyCalenderCreatePmol(MyCalenderParameter MyCalenderParameter);

}

public class MyCalenderParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public bool IsMyEnv { get; set; }
    public GetTeamDto GetTeamDto { get; set; }
    public MyCalenderGetTeamDto MyCalenderGetTeamDto { get; set; }
    public CalenderGetTeamDto CalenderGetTeamDto { get; set; }
    public bool IsLabourHistory { get; set; }
    public IShiftRepository iShiftRepository { get; set; }
    public IConfiguration Configuration { get; set; }
    public ProjectSearchMyCalender ProjectSearchMyCalender { get; set; }
    public IPmolRepository iPmolRepository;
    public IPmolResourceRepository iPmolResourceRepository;
    
    public ProjectCreateMycal ProjectCreateMycal;
    public IBorRepository _iBorRepository;
    public IBorResourceRepository _iBorResourceRepository;
    public ICoporateProductCatalogRepository _iCoporateProductCatalogRepository;
    public IVPRepository VpRepository;

}