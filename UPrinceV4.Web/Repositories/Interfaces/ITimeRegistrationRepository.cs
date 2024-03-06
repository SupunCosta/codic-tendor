using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.GD.Vehicle;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.GD;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ITimeRegistrationRepository
{
    Task<List<PmolCpcData>> GetLabourPmolVehicalesPositions(TimeRegistrationParameter TimeRegistrationParameter);
    Task<List<VtsData>> GetVtsDataByPerson(TimeRegistrationParameter TimeRegistrationParameter);
    Task<List<Vehicle>> GetVehicles(TimeRegistrationParameter TimeRegistrationParameter);
}

public class TimeRegistrationParameter
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
    public IGDRepository IGDRepository { get; set; }
    public GetLabourPmolVehicalesPositionsDto GetLabourPmolVehicalesPositionsDto { get; set; }
}