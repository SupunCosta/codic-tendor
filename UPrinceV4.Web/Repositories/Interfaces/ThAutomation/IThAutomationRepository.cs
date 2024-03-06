using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.ThAutomation;
using UPrinceV4.Web.Repositories.Interfaces.PBS;

namespace UPrinceV4.Web.Repositories.Interfaces.ThAutomation;

public interface IThAutomationRepository
{
    Task<List<GetThProductWithTrucks>> GetProductsWithTrucks(ThAutomationParameter ThAutomationParameter);
    Task<List<GetThTrucksSchedule>> GetTruckAssignData(ThAutomationParameter ThAutomationParameter);
    Task<List<ThTruckWithProductData>> GetTruckWithProduct(ThAutomationParameter ThAutomationParameter);
    Task<GetThProductWithTrucksDto> AddTrucksToProduct(ThAutomationParameter ThAutomationParameter);
    Task<string> RemoveTruckFromDay(ThAutomationParameter ThAutomationParameter);
    Task<double> CalculateDistance(ThAutomationParameter ThAutomationParameter);
    Task<RemoveThProductDto> RemoveTHProduct(ThAutomationParameter ThAutomationParameter);
    Task<List<GetThProductWithTrucks>> GetProductsWithTrucksForMyEnv(ThAutomationParameter ThAutomationParameter);
    Task<GetThProductWithTrucksDto> AddTrucksToProductMyEnv(ThAutomationParameter ThAutomationParameter);
    Task<List<GetThTrucksSchedule>> GetTruckAssignDataForMyEnv(ThAutomationParameter ThAutomationParameter);
    Task<List<ThTruckWithProductData>> GetTruckWithProductForMyEnv(ThAutomationParameter ThAutomationParameter);

    Task<string> RemoveTruckFromDayForMyEnv(ThAutomationParameter ThAutomationParameter);
    Task<string> ThFileUpload(ThAutomationParameter ThAutomationParameter);
    Task<GetThTrucksSchedule> UpdateThProduct(ThAutomationParameter ThAutomationParameter);
    Task<CpcResourceFamilyLocalizedData> CreateVehicleResourceFamily(ThAutomationParameter ThAutomationParameter);
    Task<GetThProductWithTrucksDto> AddPumpsToProductMyEnv(ThAutomationParameter ThAutomationParameter);
    Task<List<ThTruckWithProductData>> GetPumpsWithProductForMyEnv(ThAutomationParameter ThAutomationParameter);
    Task<List<GetThResourceFamilies>> GetThResourceFamilies(ThAutomationParameter ThAutomationParameter);
    Task<ThTruckAvailabilityDto> AddTruckAvailableTimes(ThAutomationParameter ThAutomationParameter);
    Task<string> ThStockCreate(ThAutomationParameter ThAutomationParameter);
    Task<ThStockDelete> DeleteThStockAvailableTime(ThAutomationParameter ThAutomationParameter);
    Task<List<ThColorsDto>> GetColourCodes(ThAutomationParameter ThAutomationParameter);


}

public class ThAutomationParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> IdList { get; set; }
    public IConfiguration Configuration { get; set; }
    public ApplicationDbContext uPrinceCustomerContext { get; set; }
    public IFormFile File { get; set; }
    public ThProductWithTrucksDto ThProductWithTrucksDto { get; set; }
    public ThTruckWithProductDto ThTruckWithProductDto { get; set; }
    public GetThProductWithTrucksDto GetThProductWithTrucksDto { get; set; }
    public RemoveTruckFromDayDto RemoveTruckFromDayDto { get; set; }
    public TruckAssignDto TruckAssignDto { get; set; }
    public IShiftRepository _iShiftRepository { get; set; }
    public CaculateDistance CaculateDistance { get; set; }
    public RemoveThProductDto RemoveThProductDto { get; set; }
    public IPbsRepository iPbsRepository { get; set; }
    public ThPbsCreateDto ThPbsCreateDto { get; set; }
    public WeTransfer WeTransfer { get; set; }
    public IFormCollection Zip { get; set; }
    public GetThTrucksSchedule UpdateThProduct { get; set; }
    public CpcResourceFamilyLocalizedData cpcResourceFamily { get; set; }
    public ThResourceFamiliesDto ThResourceFamiliesDto { get; set; }
    public ThTruckAvailabilityDto ThTruckAvailabilityDto { get; set; }
    public ThStockCreate ThStockCreate { get; set; }
    public IStockRepository iStockRepository { get; set; }
    
    public ThStockDelete ThStockDelete { get; set; }

}