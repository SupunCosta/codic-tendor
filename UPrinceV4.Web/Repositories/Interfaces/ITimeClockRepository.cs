using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ITimeClockRepository
{
    Task<IEnumerable<TimeClock>> GetTimeClock(ApplicationDbContext context, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<TimeClock>> GetTimeClockByDate(ApplicationDbContext context,
        IHttpContextAccessor contextAccessor, IShiftRepository iTShiftRepository, TimeZone timeZone, string lang,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<TimeClock> GetTimeClockById(ApplicationDbContext context, string id, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<TimeClock>> GetTimeClockByShiftId(ApplicationDbContext context, string shiftId,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<PagedResult<T>> GetTimeClockPagedResult<T>(ApplicationDbContext context, int pageNo, string lang,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<string> CreateTimeClock(ApplicationDbContext context, CreateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<string> UpdateTimeClock(ApplicationDbContext context, UpdateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    bool DeleteTimeClock(ApplicationDbContext context, string id, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<string> CreateTimeClockForAll(ApplicationDbContext context, CreateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<string> UpdatePmolLabourJobDone(PmolJobDone JobDone,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider, string userId);
    
    Task<string> CreateTimeClockChanged(ApplicationDbContext context, CreateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);
}