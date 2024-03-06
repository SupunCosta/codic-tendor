using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.GD.Vehicle;

namespace UPrinceV4.Web.Repositories.Interfaces.GD;

public interface IGDRepository
{
    Task<string> CreateToken(GDParameter GDParameter);
    Task<List<Vehicle>> GetVehicles(GDParameter GDParameter);
    Task<List<VehiclePosition>> GetVehiclePosition(GDParameter GDParameter);
    Task<Bar[]> GetVehicleStatus(GDParameter GDParameter);
}

public class GDParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> IdList { get; set; }
    public VehiclePositionDto VehiclePositionDto { get; set; }
}