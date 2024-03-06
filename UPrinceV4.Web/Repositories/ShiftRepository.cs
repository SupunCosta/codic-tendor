using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AzureMapsToolkit;
using AzureMapsToolkit.Common;
using AzureMapsToolkit.Weather;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Repositories;

public class ShiftRepository : IShiftRepository
{
    public async Task<IEnumerable<Shift>> GetShift(ApplicationDbContext context1, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options2 = new DbContextOptions<ApplicationDbContext>();

        var applicationDbContext = new ApplicationDbContext(options2, iTenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);

        IEnumerable<Shift> model;
        using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
        {
            model = context.Shifts
                .Include(s => s.WorkflowState)
                .ToList().OrderByDescending(d => d.StartDateTime);
        }

        foreach (var s in model)
        {
            s.User = applicationDbContext.CabPersonCompany.Where(c => c.Oid == s.UserId)
                .Include(c => c.Person)
                .Include(c => c.Company).FirstOrDefault();

            if (s.User == null)
            {
                var user = applicationDbContext.ApplicationUser.Where(u => u.OId == s.UserId)
                    .FirstOrDefault();
                if (user != null)
                {
                    if (user.FirstName == null && user.LastName == null) user.FirstName = user.Email;

                    var person = new CabPerson
                    {
                        Id = user.OId,
                        FirstName = user.FirstName,
                        FullName = user.FirstName + " " + user.LastName
                    };
                    var company = new CabCompany
                    {
                        Name = user.Company
                    };

                    var shiftUser = new CabPersonCompany
                    {
                        Person = person,
                        PersonId = person.Id,
                        Company = company
                    };
                    if (user.Company == null) shiftUser.Company.Name = " ";

                    s.User = shiftUser;
                }
            }

            if (s.User != null)
            {
                if (s.User.Company == null)
                {
                    var company = new CabCompany
                    {
                        Name = ""
                    };
                    s.User.Company = company;
                }

                if (s.User.Person == null)
                {
                    var user = applicationDbContext.ApplicationUser.Where(u => u.OId == s.UserId)
                        .FirstOrDefault();
                    var person = new CabPerson
                    {
                        Id = user.OId,
                        FirstName = user.FirstName,
                        FullName = user.FirstName + " " + user.LastName
                    };
                    s.User.Person = person;
                }
            }
        }

        return model;
    }

    public async Task<IEnumerable<ShiftListDto>> GetShiftV2(ShiftParameter ShiftParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ShiftParameter.ContractingUnitSequenceId,
            null, ShiftParameter.TenantProvider);

        var parameters = new { lang = ShiftParameter.Lang };

        var query = @"SELECT
                              Shifts.StartDateTime
                             ,Shifts.EndDateTime
                             ,Shifts.UserId
                             ,Shifts.Id
                             ,WorfFlowStatusLocalizedData.Name AS Status
                            FROM dbo.Shifts
                            LEFT JOIN dbo.WorfFlowStatusLocalizedData  
                              ON Shifts.WorkflowStateId = WorfFlowStatusLocalizedData.StatusId                        
                            WHERE WorfFlowStatusLocalizedData.LanguageCode = @lang";

        var sb = new StringBuilder(query);


        if (ShiftParameter.Filter.StatusId != null)
            sb.Append(" AND Shifts.WorkflowStateId = '" + ShiftParameter.Filter.StatusId + "' ");


        if (ShiftParameter.Filter.StartDate != null)
        {
            // DateTime gmt = FindGmtDatetime(pOParameter);
            //sb.Append("  AND LastModifiedDate BETWEEN '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
        }

        if (ShiftParameter.Filter.StartDateTime != null)
        {
            var timeZone = new TimeZone();
            timeZone.offset = ShiftParameter.Filter.OffSet;
            var days = Convert.ToDouble(ShiftParameter.Filter.StartDateTime);


            if (ShiftParameter.Filter.StartDateTime == -7 || ShiftParameter.Filter.StartDateTime == -14)
            {
                timeZone.date = ShiftParameter.Filter.LocalDate;
                //sb.Append("  AND LastModifiedDate BETWEEN '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");

                //model = await GetLastWeekShifts(context1, timeZone, (int)filter.StartDateTime, ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider, model);

                var delta = DayOfWeek.Monday - timeZone.date.DayOfWeek;
                if (delta > 0)
                    delta -= 7;
                timeZone.date = timeZone.date.AddDays(delta);

                if ((int)ShiftParameter.Filter.StartDateTime == -14) timeZone.date = timeZone.date.AddDays(-7);

                var finalOffset = FormatOffset(timeZone);

                var date = timeZone.date - timeZone.date.TimeOfDay;
                if (finalOffset > 0)
                    date = date.AddHours(finalOffset * -1);
                else if (finalOffset < 0) date = date.AddHours(finalOffset);

                sb.Append("  AND StartDateTime BETWEEN '" + date + "' AND '" + date.AddDays(+7) + "' ");

                //model = model.Where(d => d.StartDateTime > date && d.StartDateTime < date.AddDays(+7));
            }
            else
            {
                timeZone.date = ShiftParameter.Filter.LocalDate.AddDays(days);
                //model = await GetShiftByDate2(context1, timeZone, ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider, model);
                //var x = model.Where(m => m.User == null);

                var finalOffset = FormatOffset(timeZone);
                var date = timeZone.date - timeZone.date.TimeOfDay;
                if (finalOffset > 0)
                    date = date.AddHours(finalOffset * -1);
                else if (finalOffset < 0) date = date.AddHours(finalOffset);

                sb.Append("  AND StartDateTime BETWEEN '" + date + "' AND '" + date.AddHours(24) + "' ");

                //model = model
                //.Where(d => d.StartDateTime >= date && d.StartDateTime <= date.AddHours(24));
            }
        }

        if (ShiftParameter.Filter.EndDate != null && ShiftParameter.Filter.StartDate != null)
        {
            var timeZone = new TimeZone();
            timeZone.date = (DateTime)ShiftParameter.Filter.EndDate;
            timeZone.offset = ShiftParameter.Filter.OffSet;
            var formatter = new TimeZoneFormatter();
            ShiftParameter.Filter.EndDate = formatter.getUtcTimeFromLocalTime(timeZone);
            timeZone.date = (DateTime)ShiftParameter.Filter.StartDate;
            ShiftParameter.Filter.StartDate = formatter.getUtcTimeFromLocalTime(timeZone);

            var finalOffset = FormatOffset(timeZone);
            var date = timeZone.date - timeZone.date.TimeOfDay;
            if (finalOffset > 0)
                date = date.AddHours(finalOffset * -1);
            else if (finalOffset < 0) date = date.AddHours(finalOffset);

            var endDate = (DateTime)ShiftParameter.Filter.EndDate;
            var startDate = (DateTime)ShiftParameter.Filter.StartDate;

            if (endDate.Date == startDate.Date)
                sb.Append("  AND StartDateTime BETWEEN '" + date + "' AND '" + date.AddHours(24) + "' ");
            //shifts = shifts.Where(s => s.StartDateTime >= startDate && s.StartDateTime <= startDate.AddHours(24));
            else
                sb.Append("  AND StartDateTime BETWEEN '" + startDate + "' AND '" + endDate.AddHours(24) + "' ");
            //shifts = shifts.Where(s => s.StartDateTime >= startDate && s.StartDateTime <= endDate.AddHours(24));
            //model = context.Shifts
            //.ToList().OrderBy(d => d.StartDateTime)
            //.Where(d => d.StartDateTime >= date && d.StartDateTime <= date.AddHours(24));
            //model = filterClass.filterShiftByRange((DateTime)filter.EndDate, (DateTime)filter.StartDate, model);
        }
        else if (ShiftParameter.Filter.StartDate != null)
        {
            var timeZone = new TimeZone();
            timeZone.date = (DateTime)ShiftParameter.Filter.StartDate;
            timeZone.offset = ShiftParameter.Filter.OffSet;
            var formatter = new TimeZoneFormatter();
            ShiftParameter.Filter.EndDate = formatter.getUtcTimeFromLocalTime(timeZone);
            var startDate = (DateTime)ShiftParameter.Filter.StartDate;
            sb.Append("  AND StartDateTime BETWEEN '" + startDate + "' AND '" + startDate.AddHours(24) + "' ");

            //shifts = shifts.Where(s => s.StartDateTime >= startDate && s.StartDateTime <= startDate.AddHours(24));

            //model = filterClass.filterShiftByStartDate((DateTime)filter.StartDate, model);
        }


        if (ShiftParameter.Filter.Sorter.Attribute == null) sb.Append(" ORDER BY Shifts.StartDateTime");

        if (ShiftParameter.Filter.Sorter.Attribute != null)
        {
            if (ShiftParameter.Filter.Sorter.Attribute.ToLower().Equals("startdatetime"))
                sb.Append(" ORDER BY Shifts.StartDateTime " + ShiftParameter.Filter.Sorter.Order);

            if (ShiftParameter.Filter.Sorter.Attribute.ToLower().Equals("enddatetime"))
                sb.Append(" ORDER BY Shifts.EndDateTime " + ShiftParameter.Filter.Sorter.Order);

            if (ShiftParameter.Filter.Sorter.Attribute.ToLower().Equals("statusid"))
                sb.Append(" ORDER BY Shifts.WorkflowStateId " + ShiftParameter.Filter.Sorter.Order);
        }


        IEnumerable<ShiftListDto> data;
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            data = await connection.QueryAsync<ShiftListDto>(sb.ToString(), parameters);

            
        }

        List<ShiftUser> ShiftUserList;
        using (var connection =
               new SqlConnection(ShiftParameter.TenantProvider.GetTenant().ConnectionString))
        {
            ShiftUserList = connection.Query<ShiftUser>(
                    @"SELECT CONCAT(CabPerson.FullName, ' ( ', CabCompany.Name, ' )')  AS Name, CabPersonCompany.Oid FROM dbo.CabPersonCompany 
                                        INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id INNER JOIN dbo.CabCompany ON CabPersonCompany.CompanyId = CabCompany.Id")
                .ToList();
        }

        foreach (var shiftListDto in data)
            shiftListDto.User = ShiftUserList.Where(s => s.Oid == shiftListDto.UserId).FirstOrDefault()?.Name;

        if (ShiftParameter.Filter.UserName != null)
        {
            ShiftParameter.Filter.UserName =
                ShiftParameter.Filter.UserName.Replace("'", "''");
            data = data.Where(x =>
                x.User != null &&
                x.User.Contains(ShiftParameter.Filter.UserName, StringComparison.OrdinalIgnoreCase));
        }
        
        //data = data.Where(d => d.Foreman != null && d.Foreman.Contains(PmolParameter.filter.Foreman, StringComparison.OrdinalIgnoreCase));
        if (ShiftParameter.Filter.Sorter.Attribute != null)
            if (ShiftParameter.Filter.Sorter.Attribute.ToLower().Equals("username"))
            {
                //sb.Append("ORDER BY POHeader.Title " + pOParameter.Filter.Sorter.Order);
                if (ShiftParameter.Filter.Sorter.Order.ToLower().Equals("asc"))
                    data = from std in data
                        orderby std.User
                        select std;
                //  data = data.OrderBy(x => x.Supplier).ToList();
                else
                    //data = data.OrderByDescending(x => x.Supplier).ToList();
                    data = (from std in data
                        orderby std.User descending
                        select std).ToList();
            }

        return data;
    }


    public async Task<IEnumerable<Shift>> GetShiftByDate(ApplicationDbContext context1, TimeZone timeZone,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options2 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options2, iTenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        IEnumerable<Shift> model;
        using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
        {
            var finalOffset = FormatOffset(timeZone);
            var date = timeZone.date - timeZone.date.TimeOfDay;
            if (finalOffset > 0)
                date = date.AddHours(finalOffset * -1);
            else if (finalOffset < 0) date = date.AddHours(finalOffset);

            model = context.Shifts
                .ToList().OrderBy(d => d.StartDateTime)
                .Where(d => d.StartDateTime >= date && d.StartDateTime <= date.AddHours(24));
        }

        foreach (var s in model)
            s.User = applicationDbContext.CabPersonCompany.Where(c => c.Oid == s.UserId)
                .Include(c => c.Person)
                .Include(c => c.Company).FirstOrDefault();

        return model;
    }

    public async Task<IEnumerable<Shift>> GetShiftByUser(ApplicationDbContext context1,
        IHttpContextAccessor contextAccessor, string ContractingUnitSequenceId, string ProjectSequenceId,
        ITenantProvider iTenantProvider)
    {
        var options2 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options2, iTenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        IEnumerable<Shift> model;
        using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
        {
            var oid = contextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var userId = context.ApplicationUser.First(u => u.OId == oid).Id;
            model = context.Shifts.Where(s => s.UserId == userId)
                .ToList().OrderBy(d => d.StartDateTime);
            if (!model.Any()) throw new Exception("No available Shifts for the User");

            foreach (var s in model)
                s.User = applicationDbContext.CabPersonCompany.Where(c => c.Oid == s.UserId)
                    .Include(c => c.Person).FirstOrDefault();
        }

        return model;
    }

    public async Task<ShiftDetails> GetShiftDetails(ApplicationDbContext context1, string shiftId,
        ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository, string lang,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        try
        {
            var options2 = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options2, iTenantProvider);

            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
            using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
            {
                var shift = context.Shifts
                    .FirstOrDefault(s => s.Id == shiftId);

                shift.User = applicationDbContext.CabPersonCompany.Where(c => c.Oid == shift.UserId)
                    .Include(c => c.Person)
                    .Include(c => c.Company).FirstOrDefault();

                IEnumerable<TimeClock> times = context.TimeClock.Where(t => t.ShiftId == shiftId)
                    .OrderBy(t => t.StartDateTime);

                var shiftData = new ShiftDetails();
                shiftData.EndDateTime = shift.EndDateTime;
                shiftData.Date = shift.StartDateTime;
                //shiftData.Person = shift.User;
                shiftData.Status = shift.Status;
                shiftData.WorkflowState = context.WorkflowState.FirstOrDefault(w => w.Id == shift.WorkflowStateId);
                shiftData.CabPerson = shift.User;
                var breakTime = new TimeSpan(0, 0, 0);
                foreach (var t in times.Where(t => t.Type == 6 || t.Type == 4))
                    if (t.EndDateTime != null)
                    {
                        var d1 = (DateTime)t.EndDateTime;
                        var time = d1.Subtract(t.StartDateTime);
                        breakTime += time;
                    }

                DateTime d2;

                if (shift.EndDateTime != null)
                {
                    shiftData.IsShiftEnded = true;
                    d2 = (DateTime)shift.EndDateTime;
                }
                else
                {
                    shiftData.IsShiftEnded = false;
                    d2 = times.OrderByDescending(t => t.StartDateTime).FirstOrDefault().StartDateTime;
                }

                var t1 = d2 - shift.StartDateTime;
                shiftData.TotalTime = t1.Subtract(breakTime);
                //shiftData.TotalTime = new TimeSpan(Total.Days, Total.Hours, Total.Minutes, 0);

                var lst = new List<TimeClockDetails>();
                for (var t = 0; t < times.Count(); t++)
                {
                    var time = new TimeClockDetails();
                    var data =
                        await iTimeClockActivityTypeRepository.GetTimeClockActivityTypeByTypeId(context1,
                            times.ElementAt(t).Type, lang);
                    var type = new TimeClockActivityType();
                    type.Id = data.TimeClockActivityTypeId;
                    type.TypeId = data.TypeId;
                    type.Type = data.Label;
                    time.TimeClockId = times.ElementAt(t).Id;
                    time.ActivityType = type;
                    time.StartDateTime = times.ElementAt(t).StartDateTime;
                    if (times.ElementAt(t).EndDateTime == null)
                    {
                        time.ElapedTime = new TimeSpan(0);
                    }
                    else
                    {
                        var dateTime = (DateTime)times.ElementAt(t).EndDateTime;
                        time.ElapedTime = dateTime.Subtract(times.ElementAt(t).StartDateTime);
                    }

                    //if (t == 0)
                    //{
                    //    time.ElapedTime = new TimeSpan();
                    //}
                    //else
                    //{
                    //    DateTime d1 = (DateTime)times.ElementAt(t - 1).StartDateTime;
                    //    time.ElapedTime = time.StartDateTime.Subtract(d1);
                    //}
                    lst.Add(time);
                }

                shiftData.TimeRegistrations = lst;
                return shiftData;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [Obsolete]
    public async Task<IEnumerable<Shift>> FilterShifts(ApplicationDbContext context1, ShiftFilter filter,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
        {
            var model = await GetShift(context1, ContractingUnitSequenceId, ProjectSequenceId,
                iTenantProvider);
            if (filter == null) return model;

            if (filter.StartDateTime != null)
            {
                var timeZone = new TimeZone();
                timeZone.offset = filter.OffSet;
                var days = Convert.ToDouble(filter.StartDateTime);


                if (filter.StartDateTime == -7 || filter.StartDateTime == -14)
                {
                    timeZone.date = filter.LocalDate;
                    model = await GetLastWeekShifts(context1, timeZone, (int)filter.StartDateTime,
                        ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider, model);
                }
                else
                {
                    timeZone.date = filter.LocalDate.AddDays(days);
                    model = await GetShiftByDate2(context1, timeZone, ContractingUnitSequenceId, ProjectSequenceId,
                        iTenantProvider, model);
                    var x = model.Where(m => m.User == null);
                }
            }

            var filterClass = new ProjectFilterClass();
            if (filter.EndDate != null && filter.StartDate != null)
            {
                var timeZone = new TimeZone();
                timeZone.date = (DateTime)filter.EndDate;
                timeZone.offset = filter.OffSet;
                var formatter = new TimeZoneFormatter();
                filter.EndDate = formatter.getUtcTimeFromLocalTime(timeZone);
                timeZone.date = (DateTime)filter.StartDate;
                filter.StartDate = formatter.getUtcTimeFromLocalTime(timeZone);
                model = filterClass.filterShiftByRange((DateTime)filter.EndDate, (DateTime)filter.StartDate,
                    model);
            }
            else if (filter.StartDate != null)
            {
                var timeZone = new TimeZone();
                timeZone.date = (DateTime)filter.StartDate;
                timeZone.offset = filter.OffSet;
                var formatter = new TimeZoneFormatter();
                filter.EndDate = formatter.getUtcTimeFromLocalTime(timeZone);
                model = filterClass.filterShiftByStartDate((DateTime)filter.StartDate, model);
            }

            if (filter.UserName != null) model = filterClass.filterShiftByUserName(filter.UserName, model);

            if (filter.StatusId != null) model = filterClass.filterShiftByStatus((int)filter.StatusId, model);

            if (filter.Sorter.Order == "asc")
            {
                if (filter.Sorter.Attribute == "userName")
                {
                    model = model.OrderBy(m => m.User.Person.FullName);
                }
                else if (filter.Sorter.Attribute == "status")
                {
                    model = model.OrderBy(m => m.WorkflowState.Id);
                }
                else
                {
                    var param = filter.Sorter.Attribute.First().ToString().ToUpper() +
                                string.Join("", filter.Sorter.Attribute.Skip(1));
                    var propertyInfo = typeof(Shift).GetProperty(param);
                    model = model.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                }
            }

            if (filter.Sorter.Order == "desc")
            {
                if (filter.Sorter.Attribute == "userName")
                {
                    model = model.OrderByDescending(m => m.User.Person.FullName);
                }
                else if (filter.Sorter.Attribute == "status")
                {
                    model = model.OrderByDescending(m => m.WorkflowState.Id);
                }
                else
                {
                    var param = filter.Sorter.Attribute.First().ToString().ToUpper() +
                                string.Join("", filter.Sorter.Attribute.Skip(1));
                    var propertyInfo = typeof(Shift).GetProperty(param);
                    model = model.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                }
            }

            return model;
        }
    }

    public async Task<IEnumerable<Shift>> GetLastWeekShifts(ApplicationDbContext context1, TimeZone timeZone,
        int StartDateTime, string ContractingUnitSequenceId, string ProjectSequenceId,
        ITenantProvider iTenantProvider, IEnumerable<Shift> model)
    {
        var options2 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options2, iTenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
        {
            //int week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(timeZone.date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var delta = DayOfWeek.Monday - timeZone.date.DayOfWeek;
            if (delta > 0)
                delta -= 7;
            timeZone.date = timeZone.date.AddDays(delta);

            if (StartDateTime == -14) timeZone.date = timeZone.date.AddDays(-7);

            var finalOffset = FormatOffset(timeZone);

            var date = timeZone.date - timeZone.date.TimeOfDay;
            if (finalOffset > 0)
                date = date.AddHours(finalOffset * -1);
            else if (finalOffset < 0) date = date.AddHours(finalOffset);

            model = model.Where(d => d.StartDateTime > date && d.StartDateTime < date.AddDays(+7));

            return model;
        }
    }

    public async Task<Shift> ChangeState(ApplicationDbContext context1, string shiftId, string state, string lang,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
            using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
            {
                var shift = context.Shifts.FirstOrDefault(s => s.Id == shiftId);
                var wfState = context.WorkflowState.FirstOrDefault(ws => ws.Id == shift.WorkflowStateId);
                if (wfState.State.ToLower().Equals("approved"))
                {
                    var message = ApiErrorMessages
                        .GetErrorMessage(iTenantProvider, ErrorMessageKey.WorkflowCanNotChange, lang).Message;
                    throw new Exception(message);
                }

                var workFlowState = context.WorkflowState.FirstOrDefault(w => w.State == state);
                shift.WorkflowStateId = workFlowState.Id;
                shift.Status = workFlowState.State;
                context.Shifts.Update(shift);
                context.SaveChanges();
                return shift;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public async Task<IEnumerable<ExcelReadDto>> ReadShiftsForExcel(ApplicationDbContext context1,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider,
        ShiftFilter filter, IQrCodeRepository iQrCodeRepository, string lang, string userId)
    {
        try
        {
            var options1 = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options1, iTenantProvider);

            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);

            var sql = @"SELECT
  Shifts.Id AS ShiftId
 ,Shifts.UserId
 ,Shifts.EndDateTime AS ShiftEndDateTime
 ,Shifts.StartDateTime AS ShiftStartDateTime
 ,Shifts.Status
 ,Shifts.WorkflowStateId
 ,TimeClock.Id
 ,TimeClock.Type
 ,TimeClock.StartDateTime AS ActivityStartTime
 ,TimeClock.EndDateTime AS ActivityEndTime
 ,QRCode.Location AS Location
 ,QRCode.TravellerType
 ,TimeClock.ProjectId
 ,TimeClockActivityTypeLocalizedData.Label AS ActivityType
 ,CorporateProductCatalog.Title AS VehicleNo
 ,CONVERT(TIME, CONVERT(DATETIME, TimeClock.EndDateTime) - CONVERT(DATETIME, TimeClock.StartDateTime)) AS TotalTime
 ,Location.Latitude
 ,Location.Longitude
 ,TimeClockActivityTypeLocalizedData.Id AS TimeClockActivityTypeId
 ,DATEADD(MINUTE, DATEDIFF(MINUTE, 0, DATEADD(SECOND, 15 * 60 / 2, TimeClock.StartDateTime)) / 15 * 15, 0) AS StartDateTimeRoundNearest
 ,DATEADD(MINUTE, DATEDIFF(MINUTE, 0, DATEADD(SECOND, 15 * 60 / 2, TimeClock.EndDateTime)) / 15 * 15, 0) AS EndDateTimeRoundNearest
 ,DATEADD(MINUTE, DATEDIFF(MINUTE, 0, DATEADD(SECOND, 15 * 60 / 2, CONVERT(TIME, CONVERT(DATETIME, DATEADD(MINUTE, DATEDIFF(MINUTE, 0, DATEADD(SECOND, 15 * 60 / 2, TimeClock.EndDateTime)) / 15 * 15, 0)) - CONVERT(DATETIME, DATEADD(MINUTE, DATEDIFF(MINUTE, 0, DATEADD(SECOND, 15 * 60 / 2, TimeClock.StartDateTime)) / 15 * 15, 0))))) / 15 * 15, 0) AS TotalTimeRoundNearestDateFormat
FROM dbo.Shifts
INNER JOIN dbo.TimeClock
  ON Shifts.Id = TimeClock.ShiftId
INNER JOIN dbo.QRCode
  ON TimeClock.QRCodeId = QRCode.Id
LEFT OUTER JOIN dbo.TimeClockActivityTypeLocalizedData
  ON QRCode.ActivityTypeId = TimeClockActivityTypeLocalizedData.TimeClockActivityTypeId
LEFT OUTER JOIN dbo.CorporateProductCatalog
  ON QRCode.VehicleNo = CorporateProductCatalog.Id
LEFT OUTER JOIN dbo.Location
  ON TimeClock.LocationId = Location.Id
WHERE TimeClockActivityTypeLocalizedData.LanguageCode = @lang
AND Shifts.EndDateTime IS NOT NULL";
            var sb = new StringBuilder(sql);

            if (filter.StartDate != null && filter.EndDate != null)
            {
                if (filter.StartDate == filter.EndDate)
                {
                    var d = (DateTime)filter.StartDate;
                    var gmt = FindGmtDatetime(filter, d);
                    sb.Append("  AND Shifts.StartDateTime Between '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
                }
                else
                {
                    var d1 = (DateTime)filter.StartDate;
                    var d2 = (DateTime)filter.EndDate;
                    var gmtStart = FindGmtDatetime(filter, d1);
                    var gmtEnd = FindGmtDatetime(filter, d2);
                    sb.Append("  AND Shifts.StartDateTime Between '" + gmtStart + "' AND '" + gmtEnd.AddHours(24) +
                              "' ");
                }
            }
            else if (filter.EndDate != null)
            {
                var d = (DateTime)filter.EndDate;
                var gmt = FindGmtDatetime(filter, d);
                sb.Append("  AND Shifts.EndDateTime Between '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
            }
            else if (filter.StartDate != null)
            {
                var d = (DateTime)filter.StartDate;
                var gmt = FindGmtDatetime(filter, d);
                sb.Append("  AND Shifts.StartDateTime Between '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
            }

            if (filter.StartDateTime != null) sb = FilterByDate(sb, filter.LocalDate, filter);

            if (filter.StatusId != null) sb.Append(" AND Shifts.WorkflowStateId = " + filter.StatusId + "");

            sb.Append(" ORDER BY Shifts.StartDateTime desc, TimeClock.StartDateTime asc");

            IEnumerable<ExcelReadDto> result;

            var parameter = new { lang };
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<ExcelReadDto>(sb.ToString(), parameter).ToList();
            }

            List<projectData> projects;
            List<CabInfoDto> cabInfoDtos;
            var users = applicationDbContext.ApplicationUser.ToList();

            using (IDbConnection dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
            {
                var query = @"select Id, Title from ProjectDefinition;
                                    select  pc.Oid AS OId, p.FullName AS Employee, c.Name AS Employer from CabPersonCompany pc
                                    INNER JOIN CabPerson p ON p.Id = pc.PersonId
                                    INNER JOIN CabCompany c ON c.Id = pc.CompanyId
                                    ";
                using (var multi = dbConnection.QueryMultiple(query))
                {
                    projects = multi.Read<projectData>().ToList();
                    cabInfoDtos = multi.Read<CabInfoDto>().ToList();
                }

                foreach (var dto in result)
                {
                    dto.ProjectTitle = projects.Where(p => p.Id == dto.ProjectId).Select(p => p.Title)
                        .FirstOrDefault();

                    var cabInfoDto = cabInfoDtos.Where(p => p.OId == dto.UserId).FirstOrDefault();
                    if (cabInfoDto == null)
                    {
                        var user = users.Where(u => u.OId == dto.UserId).FirstOrDefault();
                        if (user != null)
                        {
                            dto.Employee = user.FirstName + user.LastName;
                            ;
                            dto.Employer = user.Company;
                        }
                    }
                    else
                    {
                        dto.Employee = cabInfoDto.Employee;
                        dto.Employer = cabInfoDto.Employer;
                    }
                }
            }


            if (filter.UserName != null)
            {
                filter.UserName =
                    filter.UserName.Replace("'", "''");
                result = result.Where(r =>
                    r.Employee != null && r.Employee.ToLower().Contains(filter.UserName.ToLower()));
            }

            UpdateWorkflowStatus(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider, result, userId,
                "ReadShiftsForExcel");
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task<IEnumerable<ExcelReadDto>> ReadAllShiftsForExcel(ApplicationDbContext context1,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider,
        IQrCodeRepository iQrCodeRepository, string lang, string userId)
    {
        try
        {
            var options1 = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options1, iTenantProvider);

            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);

            var sql =
                @"select Shifts.Id AS ShiftId, Shifts.UserId , Shifts.EndDateTime AS  ShiftEndDateTime, Shifts.StartDateTime AS ShiftStartDateTime,
                                Shifts.Status, Shifts.WorkflowStateId, TimeClock.Type, TimeClock.StartDateTime as ActivityStartTime,
                                TimeClock.EndDateTime as  ActivityEndTime, QRCode.Location AS Location, QRCode.TravellerType,TimeClock.ProjectId,
                                TimeClockActivityTypeLocalizedData.Label AS ActivityType, CorporateProductCatalog.Title AS VehicleNo,
                                CONVERT (TIME, CONVERT(DATETIME, TimeClock.EndDateTime) - CONVERT(DATETIME, TimeClock.StartDateTime)) as TotalTime,
                                Location.Latitude , Location.Longitude,
                                DATEADD( minute, ( DATEDIFF( minute, 0, DATEADD( second, ( 15 * 60 ) / 2, TimeClock.StartDateTime  ) ) / 15 ) * 15, 0 ) AS StartDateTimeRoundNearest,
								DATEADD( minute, ( DATEDIFF( minute, 0, DATEADD( second, ( 15 * 60 ) / 2, TimeClock.EndDateTime  ) ) / 15 ) * 15, 0 ) AS EndDateTimeRoundNearest,
                                CONVERT(VARCHAR(8),DATEADD( minute, ( DATEDIFF( minute, 0, DATEADD( second, ( 15 * 60 ) / 2, CONVERT (TIME, CONVERT(DATETIME, DATEADD( minute, ( DATEDIFF( minute, 0, DATEADD( second, ( 15 * 60 ) / 2, TimeClock.EndDateTime  ) ) / 15 ) * 15, 0 )) - CONVERT(DATETIME, DATEADD( minute, ( DATEDIFF( minute, 0, DATEADD( second, ( 15 * 60 ) / 2, TimeClock.StartDateTime  ) ) / 15 ) * 15, 0 )))  ) ) / 15 ) * 15, 0 ),108)  AS TotalTimeRoundNearest
                                from Shifts
                                INNER JOIN TimeClock on Shifts.Id = TimeClock.ShiftId
                                INNER JOIN QRCode on TimeClock.QRCodeId = QRCode.Id
                                LEFT OUTER JOIN TimeClockActivityTypeLocalizedData
                                ON QRCode.ActivityTypeId = TimeClockActivityTypeLocalizedData.TimeClockActivityTypeId
                                LEFT OUTER JOIN CorporateProductCatalog on QRCode.VehicleNo = CorporateProductCatalog.Id
                                LEFT OUTER JOIN Location on TimeClock.LocationId = Location.Id
                                WHERE TimeClockActivityTypeLocalizedData.LanguageCode = @lang
                                AND Shifts.EndDateTime is not null
                                ";

            var sb = new StringBuilder(sql);

            sb.Append(" ORDER BY Shifts.StartDateTime desc, TimeClock.StartDateTime asc");

            IEnumerable<ExcelReadDto> result;

            var parameter = new { lang };
            using (var connection = new SqlConnection(connectionString))
            {
                result = connection.Query<ExcelReadDto>(sb.ToString(), parameter).ToList();
            }

            List<projectData> projects;
            List<CabInfoDto> cabInfoDtos;
            var users = applicationDbContext.ApplicationUser.ToList();

            using (IDbConnection dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
            {
                var query = @"select Id, Title from ProjectDefinition;
                                    select  pc.Oid AS OId, p.FullName AS Employee, c.Name AS Employer from CabPersonCompany pc
                                    INNER JOIN CabPerson p ON p.Id = pc.PersonId
                                    INNER JOIN CabCompany c ON c.Id = pc.CompanyId
                                    ";
                using (var multi = dbConnection.QueryMultiple(query))
                {
                    projects = multi.Read<projectData>().ToList();
                    cabInfoDtos = multi.Read<CabInfoDto>().ToList();
                }

                foreach (var dto in result)
                {
                    dto.ProjectTitle = projects.Where(p => p.Id == dto.ProjectId).Select(p => p.Title)
                        .FirstOrDefault();

                    var cabInfoDto = cabInfoDtos.Where(p => p.OId == dto.UserId).FirstOrDefault();
                    if (cabInfoDto == null)
                    {
                        var user = users.Where(u => u.OId == dto.UserId).FirstOrDefault();
                        if (user != null)
                        {
                            dto.Employee = user.FirstName + user.LastName;
                            ;
                            dto.Employer = user.Company;
                        }
                    }
                    else
                    {
                        dto.Employee = cabInfoDto.Employee;
                        dto.Employer = cabInfoDto.Employer;
                    }
                }
            }


            UpdateWorkflowStatus(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider, result, userId,
                "ReadAllShiftsForExcel");
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> OptimizeDatabase(string ContractingUnitSequenceId, string ProjectSequenceId,
        ITenantProvider iTenantProvider, string UserId)
    {
        try
        {
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);

            var historyUpdate =
                @"INSERT INTO dbo.TimeClockHistory (Id,DataJson,HistoryLog,ChangedByUserId,Action,ChangedTime) VALUES ( @Id,@DataJson,@HistoryLog ,@ChangedByUserId,@Action,GETDATE());";
            var query = "delete from Shifts where WorkflowStateId = 4";
            var queryRead = "select * from Shifts where WorkflowStateId = 4";

            using (var connection = new SqlConnection(connectionString))
            {
                var shifts = connection.Query<Shift>(queryRead).ToList();
                var jsonProject = JsonConvert.SerializeObject(shifts, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                connection.Execute(historyUpdate, new
                {
                    Id = Guid.NewGuid().ToString(),
                    ChangedTime = DateTime.UtcNow,
                    ChangedByUserId = UserId,
                    Action = "Deleted",
                    DataJson = jsonProject,
                    HistoryLog = "OptimizeDatabase"
                });


                connection.Execute(query);
                connection.Close();
            }

            return "Ok";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<double> CalculateDistance(double latitude, double longitude, double previousLatitude,
        double previousLongitude, ITenantProvider iTenantProvider, IConfiguration Configuration, bool isTruck)
    {
        //string _address = "https://atlas.microsoft.com/route/directions/batch/sync/json?api-version=1.0&subscription-key=O0JczvPLEAonCkCAXASeOnKdiMpj_7qr5G2IcoGqKa4";
        //string _address = "https://atlas.microsoft.com/route/directions/batch/sync/json?api-version=1.0&subscription-key=1vju_5glNJUztg8XATTMHJjjaNdnlIuomHT5qp-lEh0";
        // var client = new HttpClient();
        // client.BaseAddress = new Uri(iTenantProvider.GetTenant().MapUrl);
        // // client.DefaultRequestHeaders.Accept.Add(
        // //     new MediaTypeWithQualityHeaderValue("application/json"));
        // client.DefaultRequestHeaders.Add("Accept", "application/json");
        // //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        //
        // AzureMap map = new AzureMap();
        // List<Query> batchItemList = new List<Query>();
        // Query query = new Query();
        // query.query = "?query=" + previousLatitude + "," + previousLongitude + ":" + latitude + "," + longitude +
        //               "&travelMode=car&routeType=fastest";
        // //query.query = "?query=7.4406723,80.4589001:6.8270834,79.880269&travelMode=car&routeType=fastest";
        // batchItemList.Add(query);
        // map.batchItems = batchItemList;
        // HttpResponseMessage response = await client.PostAsJsonAsync(iTenantProvider.GetTenant().MapUrl, map);
        // //response.EnsureSuccessStatusCode();
        // var result = await response.Content.ReadAsStringAsync();
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
        // if (myDeserializedClass.batchItems.FirstOrDefault().statusCode == 200)
        // {
        //     return myDeserializedClass.batchItems.FirstOrDefault().response.routes.FirstOrDefault().summary
        //         .travelTimeInSeconds;
        // }
        // else
        // {
        //     return 0;
        // }

        // var am = new AzureMapsToolkit.AzureMapsServices("LJgehqCoZFQyHnvKf4gDl1AVWh4ihTa9ZAgCGH5YY5Y");

        // var am = new AzureMapsToolkit.AzureMapsServices(Configuration.GetValue<String>("AzureMapKey"));
        //
        //
        //
        // var req = new RouteMatrixRequest
        // {
        //     RouteType = RouteType.Fastest,
        // };
        //
        // req.TravelMode = isTruck ? TravelMode.Truck : TravelMode.Car;
        //
        //
        // var origins = new List<Coordinate> {
        //
        //     new Coordinate { Longitude = previousLongitude, Latitude = previousLatitude }
        //     
        // };
        //
        //
        //
        // var destinations = new List<Coordinate> {
        //     new Coordinate { Longitude = longitude, Latitude = latitude }
        //    
        // };
        //
        // var (ResultUrl, _) = await am.GetRouteMatrix(req, origins, destinations);
        //
        // var matrixResponse = await am.GetRouteMatrixResult(ResultUrl);
        //
        // if (matrixResponse != null)
        // {
        //
        //     var hh = matrixResponse.Result.Matrix.FirstOrDefault().FirstOrDefault().Response.RouteSummary
        //         .TravelTimeInSeconds;
        //     
        //     return hh;
        // }
        // else
        // {
        //     return 0;
        // }

        var client = new HttpClient();

        var origin = previousLatitude + "," + previousLongitude;
        var destination = latitude + "," + longitude;

        var mode = isTruck ? "truck" : "car";


        var resultUrl =
            $"https://atlas.microsoft.com/route/directions/json?subscription-key=LJgehqCoZFQyHnvKf4gDl1AVWh4ihTa9ZAgCGH5YY5Y&api-version=1.0&query={origin}:{destination}&routeType=fastest&TravelMode={mode}&computeTravelTimeFor=all";
        var response = await client.GetAsync(resultUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var directionResults = JsonConvert.DeserializeObject<RouteDirectionsResponse>(content);

        if (directionResults != null)
        {
            var hh = directionResults.Routes?.FirstOrDefault()?.Summary.TravelTimeInSeconds;
            // var hh = matrixResponse.Result.Matrix.FirstOrDefault().FirstOrDefault().Response.RouteSummary
            //     .TravelTimeInSeconds;

            var jj = (double)hh/ 60;
            var kk = (double)Math.Round((decimal)jj);

            return kk;
        }

        return 0;
    }

    public async Task<QuarterDayForecast> GetWeatherForecast(double latitude, double longitude, DateTime date,
        string time, ITenantProvider iTenantProvider, IConfiguration Configuration)
    {
        var quarter = 0;
        var am = new AzureMapsServices(Configuration.GetValue<string>("AzureMapKey"));


        var Req = new GetForecastRequest
        {
            Duration = 15,
            Query = latitude + "," + longitude,
            Unit = Unit.metric
        };

        var data = await am.GetQuarterDayForecast(Req);

        // var gg = data.Result.Forecasts.FirstOrDefault();
        // var ff = DateTime.Parse(gg.Date).ToShortDateString();

        var tt = Regex.Replace(time, ":", ".").ToDouble();

        quarter = tt switch
        {
            >= 0.00 and < 6.00 => 0,
            >= 6.00 and < 12.00 => 1,
            >= 12.00 and < 18.00 => 2,
            >= 18.00 and < 25.00 => 3,
            _ => quarter
        };

        var result = data.Result?.Forecasts.Where(x =>
            DateTime.Parse(x.Date).ToShortDateString() == date.ToShortDateString() && x.Quarter == quarter);

        if (result.FirstOrDefault() != null)
            return result.FirstOrDefault();
        return null;
    }

    public async Task<IEnumerable<Shift>> GetShiftByDate2(ApplicationDbContext context1, TimeZone timeZone,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider,
        IEnumerable<Shift> model)
    {
        var options2 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options2, iTenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
        {
            var finalOffset = FormatOffset(timeZone);
            var date = timeZone.date - timeZone.date.TimeOfDay;
            if (finalOffset > 0)
                date = date.AddHours(finalOffset * -1);
            else if (finalOffset < 0) date = date.AddHours(finalOffset);

            model = model
                .Where(d => d.StartDateTime >= date && d.StartDateTime <= date.AddHours(24));
        }

        return model;
    }

    public double FormatOffset(TimeZone timeZone)
    {
        var offsetwithdot = timeZone.offset.Insert(timeZone.offset.Length - 2, ".");


        var minutes1 = offsetwithdot.Substring(4, 2);
        var minutes2 = Convert.ToDouble(minutes1) / 60;
        var min = minutes2.ToString(CultureInfo.InvariantCulture);
        var aStringBuilder = new StringBuilder(offsetwithdot);
        aStringBuilder.Remove(4, 2);
        var theString = aStringBuilder.ToString();

        string finalStringOffset;
        if (min == "0")
        {
            finalStringOffset = theString + min;
        }
        else
        {
            var aStringBuilder2 = new StringBuilder(min);
            aStringBuilder2.Remove(0, 2);
            var theString2 = aStringBuilder2.ToString();
            finalStringOffset = theString + theString2;
        }

        var finalOffset = Convert.ToDouble(finalStringOffset);
        return finalOffset;
    }

    private StringBuilder FilterByDate(StringBuilder sb, DateTime dateTime, ShiftFilter filter)
    {
        if (filter.StartDateTime == 0 || filter.StartDateTime == 1 || filter.StartDateTime == -1)
        {
            var gmt = FindGmtDatetime(filter, dateTime);
            sb.Append("  AND Shifts.StartDateTime BETWEEN '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
        }
        else if (filter.StartDateTime == -7 || filter.StartDateTime == -14)
        {
            var gmt = FindGmtWeek(filter);
            sb.Append("  AND Shifts.StartDateTime BETWEEN '" + gmt + "' AND '" + gmt.AddDays(7) + "' ");
        }

        return sb;
    }

    public DateTime FindGmtDatetime(ShiftFilter filter, DateTime dateTime)
    {
        var timeZone = new TimeZone();
        timeZone.offset = filter.OffSet;
        if (filter.StartDateTime == null)
        {
            timeZone.date = dateTime;
        }
        else
        {
            var days = Convert.ToDouble(filter.StartDateTime);
            var d = filter.LocalDate;
            timeZone.date = d.AddDays(days);
        }

        var finalOffset = FormatOffset(timeZone);
        var date = timeZone.date - timeZone.date.TimeOfDay;
        if (finalOffset > 0)
            date = date.AddHours(finalOffset * -1);
        else if (finalOffset < 0) date = date.AddHours(finalOffset);

        return date;
    }

    public DateTime FindGmtWeek(ShiftFilter filter)
    {
        var timeZone = new TimeZone();
        timeZone.offset = filter.OffSet;
        timeZone.date = filter.LocalDate;
        var delta = DayOfWeek.Monday - filter.LocalDate.DayOfWeek;
        if (delta > 0)
            delta -= 7;
        timeZone.date = timeZone.date.AddDays(delta);

        if (filter.StartDateTime == -14) timeZone.date = timeZone.date.AddDays(-7);

        var finalOffset = FormatOffset(timeZone);

        var date = timeZone.date - timeZone.date.TimeOfDay;
        if (finalOffset > 0)
            date = date.AddHours(finalOffset * -1);
        else if (finalOffset < 0) date = date.AddHours(finalOffset);

        return date;
    }

    private async void UpdateWorkflowStatus(string ContractingUnitSequenceId, string ProjectSequenceId,
        ITenantProvider iTenantProvider, IEnumerable<ExcelReadDto> shiftList, string UserId, string Action)
    {
        try
        {
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
            var historyUpdate =
                @"INSERT INTO dbo.TimeClockHistory (Id,DataJson,HistoryLog,ChangedByUserId,Action,ChangedTime) VALUES ( @Id,@DataJson,@HistoryLog ,@ChangedByUserId,@Action,GETDATE());";

            var query = "update Shifts set WorkflowStateId = 4 where WorkflowStateId = 2 AND Shifts.Id IN @Ids";
            using (var connection = new SqlConnection(connectionString))
            {
                var shiftListSplited = Split(shiftList.ToList(), 1000);
                foreach (var s in shiftListSplited)
                {
                    var jsonProject = JsonConvert.SerializeObject(s, Formatting.None,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    connection.Execute(historyUpdate, new
                    {
                        Id = Guid.NewGuid().ToString(),
                        ChangedTime = DateTime.UtcNow,
                        ChangedByUserId = UserId,
                        Action,
                        DataJson = jsonProject,
                        HistoryLog = "WorkflowStateId = 4"
                    });
                    var param = new { Ids = s.Select(s => s.ShiftId) };
                    connection.Execute(query, param);
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static List<List<T>> Split<T>(List<T> collection, int size)
    {
        var chunks = new List<List<T>>();
        var chunkCount = collection.Count() / size;

        if (collection.Count % size > 0)
            chunkCount++;

        for (var i = 0; i < chunkCount; i++)
            chunks.Add(collection.Skip(i * size).Take(size).ToList());

        return chunks;
    }
}