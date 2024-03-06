using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Repositories;

public class TimeClockRepository : ITimeClockRepository
{
    public async Task<IEnumerable<TimeClock>> GetTimeClock(ApplicationDbContext context1,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, iTenantProvider);
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        IEnumerable<TimeClock> model;
        using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
        {
            model = context.TimeClock.Include(t => t.Location)
                .Include(t => t.QRCode)
                .ToList().OrderByDescending(d => d.StartDateTime);
        }

        foreach (var clock in model)
        {
            clock.QRCode.ProjectDefinition = applicationDbContext.ProjectDefinition
                .Where(p => p.Id == clock.QRCode.ProjectId).FirstOrDefault();
            clock.User = applicationDbContext.CabPersonCompany.Where(c => c.Oid == clock.UserId)
                .Include(c => c.Person).FirstOrDefault();
        }

        return model;
    }

    public async Task<TimeClock> GetTimeClockById(ApplicationDbContext context1, string id,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        try
        {
            var options1 = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options1, iTenantProvider);
            var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);

            TimeClock timeClock;
            using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
            {
                timeClock = context.TimeClock.Include(t => t.Location)
                    .Where(t => t.Id == id).FirstOrDefault();
            }

            timeClock.QRCode.ProjectDefinition = applicationDbContext.ProjectDefinition
                .Where(p => p.Id == timeClock.QRCode.ProjectId).FirstOrDefault();
            timeClock.User = applicationDbContext.CabPersonCompany.Where(c => c.Oid == timeClock.UserId)
                .Include(c => c.Person).FirstOrDefault();
            return timeClock;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<TimeClock>> GetTimeClockByShiftId(ApplicationDbContext context1, string shiftId,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        try
        {
            var options1 = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options1, iTenantProvider);
            var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
            IEnumerable<TimeClock> timeClock;
            using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
            {
                timeClock = context.TimeClock.Include(t => t.Location)
                    .Include(t => t.QRCode).Where(t => t.ShiftId == shiftId);
            }

            foreach (var clock in timeClock)
            {
                clock.QRCode.ProjectDefinition = applicationDbContext.ProjectDefinition
                    .Where(p => p.Id == clock.QRCode.ProjectId).FirstOrDefault();
                clock.User = applicationDbContext.CabPersonCompany.Where(c => c.Oid == clock.UserId)
                    .Include(c => c.Person).FirstOrDefault();
            }

            return timeClock;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> CreateTimeClock(ApplicationDbContext context1, CreateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<TimeClockDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
      
        using (var context = new TimeClockDbContext(options, connectionString, iTenantProvider))
        {
            var oid = contextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var isShiftStarted = context.Shifts.Any(s => s.UserId == oid && s.EndDateTime == null);
            if (isShiftStarted && timeClockDto.Type == 4) throw new Exception("You have already started a shift");

            if (timeClockDto.Type != 4)
            {
                UpdateEndTime(oid, timeClockDto.StartDateTime,  ContractingUnitSequenceId, null,
                    iTenantProvider);
            }

            var isQrExist = context.QRCode.Any(x => x.Id == timeClockDto.QRCodeId);
            if (isQrExist == false)
            {
                var message = ApiErrorMessages
                    .GetErrorMessage(iTenantProvider, ErrorMessageKey.InvalidQrCode, lang).Message;
                throw new Exception(message);
            }

            var qr = context.QRCode.FirstOrDefault(q => q.Id == timeClockDto.QRCodeId);
            if (timeClockDto.Type != qr.Type && qr.Type != 3)
            {
                var message = ApiErrorMessages
                    .GetErrorMessage(iTenantProvider, ErrorMessageKey.InvalidQrCode, lang).Message;
                throw new Exception(message);
            }

            if (timeClockDto.Type == 0)
            {
                var projectConnectionString =
                    ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
                await using var connection = new SqlConnection(projectConnectionString);

                var vehicleExists = connection
                    .Query<string>(
                        "select Id FROM PMolPlannedWorkTools WHERE PmolId = @pmolId AND CoperateProductCatalogId = @cpcId",
                        new { pmolId = timeClockDto.PmolId, cpcId = qr?.VehicleNo }).Any();

                if (!vehicleExists)
                {
                    var cpcExist = connection.Query<CorporateProductCatalog>(
                        "Select * from CorporateProductCatalog Where Id = @Id",
                        new { Id = qr?.VehicleNo });

                    var mouId = cpcExist.FirstOrDefault()?.CpcBasicUnitOfMeasureId;
                    if (!cpcExist.Any())
                    {
                        var pbsParameters = new PbsResourceParameters
                        {
                            Lang = lang,
                            TenantProvider = iTenantProvider,
                            ContractingUnitSequenceId = ContractingUnitSequenceId
                        };
                        mouId = await timeClockDto.PbsResourceRepository.CopyCpcFromCuToProject(pbsParameters,
                            qr?.VehicleNo, projectConnectionString, "cu");
                    }

                    var insertSql = @"INSERT INTO dbo.PMolPlannedWorkTools ( Id ,CoperateProductCatalogId ,RequiredQuantity ,ConsumedQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted ) VALUES ( @Id ,@CoperateProductCatalogId ,0.0 ,0.0 ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type ,0 )";

                    var param = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        CoperateProductCatalogId = qr?.VehicleNo,
                        CpcBasicUnitofMeasureId = mouId,
                        timeClockDto.PmolId,
                        Type = "Planned"
                    };

                    await connection.ExecuteAsync(insertSql, param);
                }
            }

            var timeClock = new TimeClock();
            string locationId;
            if (timeClockDto.Location == null)
                locationId = null;
            else
                locationId = await CreateLocation( timeClockDto, ContractingUnitSequenceId,
                    iTenantProvider);

            timeClock.UserId = oid;

            var shift = context.Shifts.Where(u => u.UserId == oid).OrderByDescending(t => t.StartDateTime)
                .FirstOrDefault();
            if (timeClockDto.Type == 4 )
            {
                timeClock.ShiftId = await CreateShift( timeClockDto, timeClock.UserId,
                    ContractingUnitSequenceId,  iTenantProvider);
            }
            else
            {
                var user = timeClock.UserId;
                if (shift != null) timeClock.ShiftId = shift.Id;
            }

            timeClock.Id = Guid.NewGuid().ToString();
            timeClock.EndDateTime = null;
            if (timeClockDto.Type == 1)
            {
                //IEnumerable<TimeClock> projectNullRecords = context.TimeClock.Where(t => t.ShiftId == timeClock.ShiftId && t.ProjectId == null);
                timeClock.ProjectId = qr.ProjectId;
                UpdateProjectId(iTenantProvider, timeClock.ShiftId, timeClock.ProjectId, connectionString);
            }

            if (timeClockDto.Type == 5)
            {
                var lastWorkTimeClock = context.TimeClock
                    .Where(t => t.Type == 1 && t.ShiftId == timeClock.ShiftId)
                    .OrderByDescending(t => t.StartDateTime).FirstOrDefault();
                if (lastWorkTimeClock != null)
                {
                    timeClock.ProjectId = lastWorkTimeClock.ProjectId;

                    //IEnumerable<TimeClock> projectNullRecords = context.TimeClock.Where(t => t.ShiftId == timeClock.ShiftId && t.ProjectId == null);
                    UpdateProjectId(iTenantProvider, timeClock.ShiftId, lastWorkTimeClock.ProjectId,
                        connectionString);
                }

                var isStopped = context.TimeClock.Where(t => t.ShiftId == shift.Id).Any(x => x.Type == 5);
                if (isStopped) throw new Exception("youHaveAlreadyStoppedThisShift");

                timeClock.EndDateTime = timeClockDto.StartDateTime;
            }

            timeClock.FromLocation = timeClockDto.FromLocation;
            timeClock.StartDateTime = timeClockDto.StartDateTime;
            timeClock.ToLocation = timeClockDto.ToLocation;
            timeClock.Type = timeClockDto.Type;
            timeClock.LocationId = locationId;
            timeClock.QRCodeId = timeClockDto.QRCodeId;
            timeClock.PmolId = timeClockDto.PmolId;

            context.TimeClock.Add(timeClock);
            await context.SaveChangesAsync();

            if (timeClockDto.Type == 5)
                UpdateShift( shift, timeClockDto, ContractingUnitSequenceId, null,
                    iTenantProvider);

            return timeClock.Id;
        }
    }
    
    public async Task<string> CreateTimeClockChanged(ApplicationDbContext context1, CreateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
       
        var options = new DbContextOptions<TimeClockDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
      
        using (var context = new TimeClockDbContext(options, connectionString, iTenantProvider))
        {
            var oid = contextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            if (timeClockDto.Type != 4)
                UpdateEndTime(oid, timeClockDto.StartDateTime, ContractingUnitSequenceId, null,
                    iTenantProvider);

           
            var isQrExist = context.QRCode.Any(x => x.Id == timeClockDto.QRCodeId);
            if (isQrExist == false)
            {
                var message = ApiErrorMessages
                    .GetErrorMessage(iTenantProvider, ErrorMessageKey.InvalidQrCode, lang).Message;
                throw new Exception(message);
            }

            var qr = context.QRCode.Where(q => q.Id == timeClockDto.QRCodeId);
            if (timeClockDto.Type != qr.First().Type && qr.First().Type != 3)
            {
                var message = ApiErrorMessages
                    .GetErrorMessage(iTenantProvider, ErrorMessageKey.InvalidQrCode, lang).Message;
                throw new Exception(message);
            }

            if (timeClockDto.Type == 0)
            {
                var projectConnectionString =
                    ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
                await using var connection = new SqlConnection(projectConnectionString);

                if (qr.FirstOrDefault()?.VehicleNo != null)
                {
                    var vehicleExists = connection
                        .Query<string>(
                            "select Id FROM PMolPlannedWorkTools WHERE PmolId = @pmolId AND CoperateProductCatalogId = @cpcId",
                            new { pmolId = timeClockDto.PmolId, cpcId = qr.FirstOrDefault().VehicleNo }).Any();

                    if (!vehicleExists)
                    {
                        var cpcExist = connection.Query<CorporateProductCatalog>(
                            "Select * from CorporateProductCatalog Where Id = @Id",
                            new { Id = qr.FirstOrDefault().VehicleNo });

                        var mouId = cpcExist.FirstOrDefault()?.CpcBasicUnitOfMeasureId;
                        if (!cpcExist.Any())
                        {
                            var pbsParameters = new PbsResourceParameters
                            {
                                Lang = lang,
                                TenantProvider = iTenantProvider,
                                ContractingUnitSequenceId = ContractingUnitSequenceId
                            };
                            mouId = await timeClockDto.PbsResourceRepository.CopyCpcFromCuToProject(pbsParameters,
                                qr.FirstOrDefault().VehicleNo, projectConnectionString, "cu");
                        }

                        var insertSql =
                            @"INSERT INTO dbo.PMolPlannedWorkTools ( Id ,CoperateProductCatalogId ,RequiredQuantity ,ConsumedQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted ) VALUES ( @Id ,@CoperateProductCatalogId ,0.0 ,0.0 ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type ,0 )";

                        var param = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            CoperateProductCatalogId = qr.FirstOrDefault().VehicleNo,
                            CpcBasicUnitofMeasureId = mouId,
                            timeClockDto.PmolId,
                            Type = "Planned"
                        };

                        await connection.ExecuteAsync(insertSql, param);
                    }
                }
            }
            

            var timeClock = new TimeClock();
            string locationId;
            if (timeClockDto.Location == null)
                locationId = null;
            else
                locationId = await CreateLocation( timeClockDto, ContractingUnitSequenceId,
                    iTenantProvider);

            timeClock.UserId = oid;

            var shift = context.Shifts.Where(u => u.UserId == oid).OrderByDescending(t => t.StartDateTime)
                .FirstOrDefault();
            if (!timeClockDto.IsShiftStart )
            {
                timeClock.ShiftId = await CreateShift(timeClockDto, timeClock.UserId,
                    ContractingUnitSequenceId,  iTenantProvider);
            }
            else
            {
                if (shift != null) timeClock.ShiftId = shift.Id;
            }

            timeClock.Id = Guid.NewGuid().ToString();
            timeClock.EndDateTime = null;
            if (timeClockDto.Type == 1)
            {
                //IEnumerable<TimeClock> projectNullRecords = context.TimeClock.Where(t => t.ShiftId == timeClock.ShiftId && t.ProjectId == null);
                timeClock.ProjectId = qr.FirstOrDefault().ProjectId;
                UpdateProjectId(iTenantProvider, timeClock.ShiftId, timeClock.ProjectId, connectionString);
            }

            if (timeClockDto.Type == 5)
            {
                var lastWorkTimeClock = context.TimeClock
                    .Where(t => t.Type == 1 && t.ShiftId == timeClock.ShiftId)
                    .OrderByDescending(t => t.StartDateTime).FirstOrDefault();
                if (lastWorkTimeClock != null)
                {
                    timeClock.ProjectId = lastWorkTimeClock.ProjectId;

                    //IEnumerable<TimeClock> projectNullRecords = context.TimeClock.Where(t => t.ShiftId == timeClock.ShiftId && t.ProjectId == null);
                    UpdateProjectId(iTenantProvider, timeClock.ShiftId, lastWorkTimeClock.ProjectId,
                        connectionString);
                }

                var isStopped = context.TimeClock.Where(t => t.ShiftId == shift.Id).Any(x => x.Type == 5);
                if (isStopped) throw new Exception("youHaveAlreadyStoppedThisShift");

                timeClock.EndDateTime = timeClockDto.StartDateTime;
            }

            timeClock.FromLocation = timeClockDto.FromLocation;
            timeClock.StartDateTime = timeClockDto.StartDateTime;
            timeClock.ToLocation = timeClockDto.ToLocation;
            timeClock.Type = timeClockDto.Type;
            timeClock.LocationId = locationId;
            timeClock.QRCodeId = timeClockDto.QRCodeId;
            timeClock.PmolId = timeClockDto.PmolId;

            context.TimeClock.Add(timeClock);
            await context.SaveChangesAsync();

            if (timeClockDto.Type == 5)
                UpdateShift(shift, timeClockDto, ContractingUnitSequenceId, null,
                    iTenantProvider);

            return timeClock.Id;
        }
    }

    public async Task<string> CreateTimeClockForAll(ApplicationDbContext context1, CreateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        
        var options = new DbContextOptions<TimeClockDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
        using (var context = new TimeClockDbContext(options, connectionString, iTenantProvider))
        {
            var oid = contextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var isShiftStarted = context.Shifts.Any(s => s.UserId == oid && s.EndDateTime == null);
            if (isShiftStarted && timeClockDto.Type == 4) throw new Exception("You have already started a shift");

            if (timeClockDto.Type != 4 && timeClockDto.IsForeman)
                UpdateEndTime(oid, timeClockDto.StartDateTime,  ContractingUnitSequenceId, null,
                    iTenantProvider);


            var isQrExist = context.QRCode.Any(x => x.Id == timeClockDto.QRCodeId);
            if (isQrExist == false)
            {
                var message = ApiErrorMessages
                    .GetErrorMessage(iTenantProvider, ErrorMessageKey.InvalidQrCode, lang).Message;
                throw new Exception(message);
            }

            var qr = context.QRCode.Where(q => q.Id == timeClockDto.QRCodeId);
            if (timeClockDto.Type != qr.First().Type && qr.First().Type != 3)
            {
                var message = ApiErrorMessages
                    .GetErrorMessage(iTenantProvider, ErrorMessageKey.InvalidQrCode, lang).Message;
                throw new Exception(message);
            }

            var timeClock = new TimeClock();
            string locationId;
            if (timeClockDto.Location == null)
                locationId = null;
            else
                locationId = await CreateLocation( timeClockDto, ContractingUnitSequenceId,
                    iTenantProvider);

            timeClock.UserId = oid;

            var shift = context.Shifts.Where(u => u.UserId == oid).OrderByDescending(t => t.StartDateTime)
                .FirstOrDefault();
            if (timeClockDto.Type == 4)
            {
                timeClock.ShiftId = await CreateShift(timeClockDto, timeClock.UserId,
                    ContractingUnitSequenceId,  iTenantProvider);
            }
            else
            {
                var user = timeClock.UserId;
                if (shift != null) timeClock.ShiftId = shift.Id;
            }

            timeClock.Id = Guid.NewGuid().ToString();
            timeClock.EndDateTime = null;
            if (timeClockDto.Type == 1)
            {
                timeClock.ProjectId = qr.FirstOrDefault().ProjectId;
                UpdateProjectId(iTenantProvider, timeClock.ShiftId, timeClock.ProjectId, connectionString);
            }

            if (timeClockDto.Type == 5)
            {
                var lastWorkTimeClock = context.TimeClock
                    .Where(t => t.Type == 1 && t.ShiftId == timeClock.ShiftId)
                    .OrderByDescending(t => t.StartDateTime).FirstOrDefault();
                if (lastWorkTimeClock != null)
                {
                    timeClock.ProjectId = lastWorkTimeClock.ProjectId;

                    //IEnumerable<TimeClock> projectNullRecords = context.TimeClock.Where(t => t.ShiftId == timeClock.ShiftId && t.ProjectId == null);
                    UpdateProjectId(iTenantProvider, timeClock.ShiftId, lastWorkTimeClock.ProjectId,
                        connectionString);
                }

                var isStopped = context.TimeClock.Where(t => t.ShiftId == shift.Id).Any(x => x.Type == 5);
                if (isStopped) throw new Exception("youHaveAlreadyStoppedThisShift");

                timeClock.EndDateTime = timeClockDto.StartDateTime;
            }

            timeClock.FromLocation = timeClockDto.FromLocation;
            timeClock.StartDateTime = timeClockDto.StartDateTime;
            timeClock.ToLocation = timeClockDto.ToLocation;
            timeClock.Type = timeClockDto.Type;
            timeClock.LocationId = locationId;
            timeClock.QRCodeId = timeClockDto.QRCodeId;

            context.TimeClock.Add(timeClock);
            await context.SaveChangesAsync();

            if (timeClockDto.Type == 5)
                UpdateShift(shift, timeClockDto, ContractingUnitSequenceId, null,
                    iTenantProvider);

            return timeClock.Id;
        }
    }

    public async Task<string> UpdateTimeClock(ApplicationDbContext context1, UpdateTimeClockDto timeClockDto,
        IHttpContextAccessor contextAccessor, string lang, string contractingUnitSequenceId,
        string projectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<TimeClockDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, null, iTenantProvider);
        await using (var context = new TimeClockDbContext(options, connectionString, iTenantProvider))
        {
            var locationId = timeClockDto.Location == null
                ? null
                : await UpdateLocation( timeClockDto,  projectSequenceId, iTenantProvider);

            var timeClock = new TimeClock
            {
                Id = timeClockDto.Id,
                EndDateTime = timeClockDto.EndDateTime,
                FromLocation = timeClockDto.FromLocation,
                StartDateTime = timeClockDto.StartDateTime,
                ToLocation = timeClockDto.ToLocation,
                Type = timeClockDto.Type,
                LocationId = locationId,
                QRCodeId = timeClockDto.QRCodeId
            };
            var oid = contextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            timeClock.UserId = context.ApplicationUser.First(u => u.OId == oid).Id;
            context.TimeClock.Update(timeClock);
            await context.SaveChangesAsync();

            return timeClock.Id;
        }
    }

    public bool DeleteTimeClock(ApplicationDbContext context1, string id, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<TimeClockDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
        using (var context = new TimeClockDbContext(options, connectionString, iTenantProvider))
        {
            var timeClock = (from a in context.TimeClock
                where a.Id == id
                select a).Single();
            context.TimeClock.Remove(timeClock);
            context.SaveChanges();
            return true;
        }
    }


    public async Task<IEnumerable<TimeClock>> GetTimeClockByDate(ApplicationDbContext context1,
        IHttpContextAccessor contextAccessor, IShiftRepository iTShiftRepository, TimeZone timeZone, string lang,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var applicationDbContext = new ProjectDefinitionDbContext(options1, iTenantProvider);

        var options = new DbContextOptions<TimeClockDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        IEnumerable<TimeClock> time;
        var oid = contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

        using (var context = new TimeClockDbContext(options, connectionString, iTenantProvider))
        {
            var model = context.Shifts.Where(s => s.UserId == oid).OrderByDescending(s => s.StartDateTime)
                .FirstOrDefault();

            if (model == null || model.EndDateTime != null)
                //string message = ApiErrorMessages.GetErrorMessage(iTenantProvider, ErrorMessageKey.PleaseStartAShift, lang).Message;
                throw new Exception("PleaseStartAShift");

            time = context.TimeClock.Where(t => t.ShiftId == model.Id)
                .Include(q => q.QRCode)
                .ThenInclude(q => q.CorporateProductCatalog)
                .OrderBy(t => t.StartDateTime).ToList();
        }

        foreach (var t in time)
            t.QRCode.ProjectDefinition = applicationDbContext.ProjectDefinition
                .Where(p => p.Id == t.QRCode.ProjectId).FirstOrDefault();

        return time;
    }


    public async Task<PagedResult<T>> GetTimeClockPagedResult<T>(ApplicationDbContext context1, int pageNo,
        string lang, string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
            using (var context = new ShanukaDbContext(options, connectionString, iTenantProvider))
            {
                var pagedResult = new PagedResult<T>();
                pagedResult.CurrentPage = pageNo;
                var numOfRecords = context.TimeClock.Count();
                var pageCount = numOfRecords / pagedResult.PageSize;
                if (numOfRecords % pagedResult.PageSize != 0) pageCount += 1;

                if (pageCount < pageNo)
                {
                    var message = ApiErrorMessages
                        .GetErrorMessage(iTenantProvider, ErrorMessageKey.NoMoreRecordsFound, lang).Message;
                    throw new Exception(message);
                }

                pagedResult.PageCount = pageCount;
                var list = context.TimeClock.Skip(pagedResult.PageSize * (pageNo - 1)).Take(pagedResult.PageSize)
                    .ToList();
                pagedResult.Results = (IList<T>)list;
                return pagedResult;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> UpdatePmolLabourJobDone(PmolJobDone JobDone,
        IHttpContextAccessor contextAccessor, string lang, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider, string userId)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractingUnitSequenceId,
            ProjectSequenceId, iTenantProvider);

        await using var tenetConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);


        await using var connection = new SqlConnection(connectionString);

        var cabPerson = tenetConnection
            .Query<CabDataDapperDto>("select * from CabPersonCompany where Oid =@Oid", new { Oid = userId })
            .FirstOrDefault();

        var selectlabour =
            "SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.CabPersonId = @PersonId AND PmolTeamRole.IsDeleted = 0";

        var labour = connection
            .Query<PmolTeamRole>(selectlabour, new { JobDone.PmolId, cabPerson.PersonId })
            .FirstOrDefault();

        await connection.ExecuteAsync(
            "Update PmolTeamRole Set IsJobDone = @IsJobDone , Message = @Message Where Id = @Id",
            new { labour.Id, IsJobDone = 1, JobDone.Message });

        return JobDone.PmolId;
    }

    public void UpdateEndTime(string userId, DateTime endTime,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<TimeClockDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
        using (var context = new TimeClockDbContext(options, connectionString, iTenantProvider))
        {
            var time = context.TimeClock.Where(t => t.UserId == userId)
                .OrderByDescending(t => t.StartDateTime).FirstOrDefault();
            if (time != null)
            {
                time.EndDateTime = endTime;
                context.TimeClock.Update(time);
                context.SaveChanges();
            }
        }
    }

    public void UpdateProjectId(ITenantProvider iTenantProvider, string shiftId, string projectId, string con)
    {
        try
        {
            var options = new DbContextOptions<TimeClockDbContext>();
            using (var context = new TimeClockDbContext(options, con, iTenantProvider))
            {
                IList<TimeClock> projectNullRecords =
                    context.TimeClock.Where(t => t.ShiftId == shiftId && t.ProjectId == null).ToList();
                foreach (var time in projectNullRecords)
                {
                    time.ProjectId = projectId;
                    context.TimeClock.Update(time);
                    context.SaveChanges();
                }
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateLocation( CreateTimeClockDto timeClock,
        string ContractingUnitSequenceId,ITenantProvider iTenantProvider)
    {
        try
        {
            var options = new DbContextOptions<LocationDbContext>();
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
            using (var context = new LocationDbContext(options, connectionString, iTenantProvider))
            {
                var location = new Location();
                location.Id = Guid.NewGuid().ToString();
                location.Altitude = timeClock.Location.Altitude;
                location.Heading = timeClock.Location.Heading;
                location.HorizontalAccuracy = timeClock.Location.HorizontalAccuracy;
                location.Latitude = timeClock.Location.Latitude;
                location.Longitude = timeClock.Location.Longitude;
                location.Speed = timeClock.Location.Speed;
                location.VerticleAccuracy = timeClock.Location.VerticleAccuracy;
                context.Location.Add(location);
                await context.SaveChangesAsync();
                return location.Id;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> CreateShift( CreateTimeClockDto timeClock,
        string userId, string ContractingUnitSequenceId, ITenantProvider iTenantProvider)
    {
        try
        {
            var options = new DbContextOptions<ShiftDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
            using (var context = new ShiftDbContext(options, connectionString, iTenantProvider))
            {
                var shift = new Shift();
                shift.Id = Guid.NewGuid().ToString();
                shift.StartDateTime = timeClock.StartDateTime;
                shift.EndDateTime = null;
                shift.UserId = userId;
                shift.Status = "In Review";
                var defaultWorkflowId = context.WorkflowState.FirstOrDefault(w => w.State == "In Review");
                if (defaultWorkflowId != null) shift.WorkflowStateId = defaultWorkflowId.Id;
                context.Shifts.Add(shift);
                await context.SaveChangesAsync();
                return shift.Id;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async void UpdateShift( Shift shift,
        CreateTimeClockDto timeClock, string ContractingUnitSequenceId, string ProjectSequenceId,
        ITenantProvider iTenantProvider)
    {
        try
        {
            var options = new DbContextOptions<ShiftDbContext>();
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, ProjectSequenceId, iTenantProvider);
            using (var context = new ShiftDbContext(options, connectionString, iTenantProvider))
            {
                shift.EndDateTime = timeClock.StartDateTime;
                context.Shifts.Update(shift);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> UpdateLocation( UpdateTimeClockDto timeClock,
        string ContractingUnitSequenceId, ITenantProvider iTenantProvider)
    {
        try
        {
            var options = new DbContextOptions<LocationDbContext>();
            var connectionString =
                ConnectionString.MapConnectionString(ContractingUnitSequenceId, null, iTenantProvider);
            using (var context = new LocationDbContext(options, connectionString, iTenantProvider))
            {
                var location = new Location();
                location.Id = timeClock.Location.Id;
                location.Altitude = timeClock.Location.Altitude;
                location.Heading = timeClock.Location.Heading;
                location.HorizontalAccuracy = timeClock.Location.HorizontalAccuracy;
                location.Latitude = timeClock.Location.Latitude;
                location.Longitude = timeClock.Location.Longitude;
                location.Speed = timeClock.Location.Speed;
                location.VerticleAccuracy = timeClock.Location.VerticleAccuracy;
                context.Location.Update(location);
                await context.SaveChangesAsync();
                return location.Id;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}