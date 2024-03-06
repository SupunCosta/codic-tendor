using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureMapsToolkit.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IShiftRepository
{
    Task<IEnumerable<Shift>> GetShift(ApplicationDbContext context, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<ShiftListDto>> GetShiftV2(ShiftParameter ShiftParameter);


    Task<IEnumerable<Shift>> GetShiftByUser(ApplicationDbContext context, IHttpContextAccessor contextAccessor,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<Shift>> GetShiftByDate(ApplicationDbContext context, TimeZone timeZone,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<ShiftDetails> GetShiftDetails(ApplicationDbContext context, string shiftId,
        ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository, string lang,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<Shift>> FilterShifts(ApplicationDbContext context, ShiftFilter filter,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<Shift>> GetLastWeekShifts(ApplicationDbContext context, TimeZone timeZone, int startDateTime,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider,
        IEnumerable<Shift> model);

    Task<Shift> ChangeState(ApplicationDbContext context, string shiftId, string state, string lang,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<ExcelReadDto>> ReadShiftsForExcel(ApplicationDbContext context,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider,
        ShiftFilter filter, IQrCodeRepository iQrCodeRepository, string lang, string userId);

    Task<IEnumerable<ExcelReadDto>> ReadAllShiftsForExcel(ApplicationDbContext context,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider,
        IQrCodeRepository iQrCodeRepository, string lang, string userId);

    Task<string> OptimizeDatabase(string ContractingUnitSequenceId, string ProjectSequenceId,
        ITenantProvider iTenantProvider, string userId);

    Task<double> CalculateDistance(double latitude, double longitude, double previousLatitude,
        double previousLongitude, ITenantProvider iTenantProvider, IConfiguration Configuration, bool isTruck);

    // Task<QuarterDayForecast> GetWeatherForecast(double latitude, double longitude, DateTime date, string time,
    //     ITenantProvider iTenantProvider, IConfiguration Configuration);
    //     double previousLongitude, ITenantProvider iTenantProvider, IConfiguration Configuration,bool isTruck );

    Task<QuarterDayForecast> GetWeatherForecast(double latitude, double longitude, DateTime date, string time,
        ITenantProvider iTenantProvider, IConfiguration Configuration);
}

public class ShiftParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }

    public string Title { get; set; }

    //public POFilter Filter { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> idList { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public ShiftFilter Filter { get; set; }
}