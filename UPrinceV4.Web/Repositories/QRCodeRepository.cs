using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class QrCodeRepository : IQrCodeRepository
{
    public async Task<string> CreateQrCode(ApplicationDbContext context1, CreateQRCodeDto qrDto,
        string createdUserId, string contractingUnitSequenceId, string projectSequenceId,
        ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<QRDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId, iTenantProvider);
        bool isExist;
        using (var context = new QRDbContext(options, connectionString, iTenantProvider))
        {
            var qr = new QRCode
            {
                Date = qrDto.Date,
                ProjectId = qrDto.ProjectId,
                VehicleNo = qrDto.VehicleNo,
                Location = qrDto.Location,
                PersonalId = qrDto.PersonalId,
                Type = qrDto.Type,
                ActivityTypeId = context.TimeClockActivityType.FirstOrDefault(a => a.TypeId == qrDto.Type)!.Id,
                CreatedByUserId = createdUserId,
                TravellerType = qrDto.TravellerType
            };

            if (qrDto.Id == null)
            {
                qr.Id = Guid.NewGuid().ToString();
                context.QRCode.Add(qr);
                await context.SaveChangesAsync();
                qrDto.Id = qr.Id;
                isExist = false;
            }
            else
            {
                qr.Id = qrDto.Id;
                context.QRCode.Update(qr);
                await context.SaveChangesAsync();
                isExist = true;
            }

            CreateHistory(qrDto, isExist, createdUserId, contractingUnitSequenceId, projectSequenceId,
                iTenantProvider);
            return qr.Id;
        }
    }

    public void DeleteQrCode(ApplicationDbContext context1, string id, string contractingUnitSequenceId,
        string projectSequenceId, ITenantProvider iTenantProvider)
    {
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, null, iTenantProvider);
        var query = "update QRCode set IsDeleted = 1 where Id =@Id";
        var param = new { Id = id };
        using var connection = new SqlConnection(connectionString);
        connection.QuerySingleOrDefaultAsync(query, param);
    }

    public async Task<IEnumerable<QRCodeDto>> Filter(ApplicationDbContext context1, QRCodeFilter filter,
        string lang, ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository,
        ITenantProvider iTenantProvider, string contractingUnitSequenceId, string projectSequenceId)
    {
        var page = 1;
        var PageSize = 10;
        var Offset = (page - 1) * PageSize;
        

        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, iTenantProvider);
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId, iTenantProvider);

        var sb = new StringBuilder();
        var query = @"
                        SELECT QRCode.Id AS Id, QRCode.ProjectId
                        ,CONCAT(CorporateProductCatalog.Title,CASE WHEN QRCode.TravellerType = '1' THEN ' - Driver' WHEN QRCode.TravellerType = '2' THEN ' - Passenger' END) AS VehicleNo,QRCode.Location ,QRCode.CreatedByUserId ,TimeClockActivityTypeLocalizedData.Label AS Type ,QRCode.Date 
                         FROM dbo.QRCode 
                        LEFT OUTER JOIN dbo.TimeClockActivityTypeLocalizedData ON QRCode.ActivityTypeId = TimeClockActivityTypeLocalizedData.TimeClockActivityTypeId
                        LEFT OUTER JOIN CorporateProductCatalog on QRCode.VehicleNo = CorporateProductCatalog.Id
                        WHERE TimeClockActivityTypeLocalizedData.LanguageCode = @lang
                        AND QRCode.IsDeleted = 0
                             ";
        sb.Append(query);
        if (filter.Location != null)
        {
            filter.Location = filter.Location.Replace("'", "''");
            sb.Append(" AND QRCode.Location LIKE '%" + filter.Location + "%' ");
        }

        if (filter.Type != null) sb.Append(" AND QRCode.Type = '" + filter.Type + "' ");

        if (filter.ProjectId != null) sb.Append(" AND QRCode.ProjectId = '" + filter.ProjectId + "' ");

        if (filter.VehicleNo != null)
            sb.Append(" AND CorporateProductCatalog.Title  LIKE '%" + filter.VehicleNo + "%' ");

        if (filter.Sorter.Attribute == null) sb.Append("order by TimeClockActivityTypeLocalizedData.DisplayOrder ");

        if (filter.Sorter.Attribute != null)
            switch (filter.Sorter.Attribute.ToLower())
            {
                case "type":
                    sb.Append("order by TimeClockActivityTypeLocalizedData.DisplayOrder " + filter.Sorter.Order);
                    break;
                case "vehicleno":
                    sb.Append("order by CorporateProductCatalog.ResourceNumber " + filter.Sorter.Order);
                    break;
                case "location":
                    sb.Append("order by QRCode.Location " + filter.Sorter.Order);
                    break;
            }

        var parameters = new { Offset, PageSize, lang };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var result = dbConnection.Query<QRCodeDto>(sb.ToString(), parameters);
            

            foreach (var dto in result)
            {
                var project = applicationDbContext.ProjectDefinition.Where(p => p.Id == dto.ProjectId)
                    .FirstOrDefault();
                if (project != null) dto.ProjectTitle = project.Title;
            }

            if (filter.Sorter.Attribute != null)
                if (filter.Sorter.Attribute.ToLower().Equals("projectid"))
                    result = filter.Sorter.Order switch
                    {
                        "asc" => result.OrderBy(r => r.ProjectTitle),
                        "desc" => result.OrderByDescending(r => r.ProjectTitle),
                        _ => result
                    };

            return result;
        }
    }

    public async Task<IEnumerable<QRCode>> GetQrCode(ApplicationDbContext context1, string lang,
        ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository, string contractingUnitSequenceId,
        string projectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<QRDbContext>();
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var applicationDbContext = new ProjectDefinitionDbContext(options1, iTenantProvider);
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId, iTenantProvider);
        await using (var context = new QRDbContext(options, connectionString, iTenantProvider))
        {
            IEnumerable<QRCode> model = context.QRCode.Include(q => q.CorporateProductCatalog)
                .ToList();
            foreach (var qr in model)
            {
                await iTimeClockActivityTypeRepository.GetTimeClockActivityTypeByTypeId(context1, qr.Type, lang);
                qr.ProjectDefinition = applicationDbContext.ProjectDefinition
                    .FirstOrDefault(p => p.Id == qr.ProjectId);
            }

            model = model.OrderBy(q => q.ActivityType.DisplayOrder);
            return model;
        }
    }

    public async Task<IEnumerable<QRCode>> GetQrCodeByType(ApplicationDbContext context1, int type,
        string contractingUnitSequenceId, string projectSequenceId, ITenantProvider iTenantProvider)
    {
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var applicationDbContext = new ProjectDefinitionDbContext(options1, iTenantProvider);
        var options = new DbContextOptions<QRDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId, iTenantProvider);
        await using (var context = new QRDbContext(options, connectionString, iTenantProvider))
        {
            IEnumerable<QRCode> qr = context.QRCode.Where(t => t.Type == type)
                .Include(q => q.CorporateProductCatalog)
                .OrderByDescending(q => q.Date);
            foreach (var q in qr)
                q.ProjectDefinition = applicationDbContext.ProjectDefinition
                    .FirstOrDefault(p => p.Id == q.ProjectId);

            return qr;
        }
    }

    public async Task<QRCodeDapperDto> GetTqrCodeById(ApplicationDbContext context, string id, string lang,
        ITenantProvider iTenantProvider, string contractingUnitSequenceId, string projectSequenceId)
    {
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var applicationDbContext = new ProjectDefinitionDbContext(options1, iTenantProvider);

        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId, iTenantProvider);

        IEnumerable<QRCodeDapperDto> results;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            {
                var query = @"SELECT QRCode.Id, QRCode.ProjectId, QRCode.Type
                                        ,QRCode.PersonalId, QRCode.Location, QRCode.Date, CorporateProductCatalog.Id AS VehicleCpcId, CorporateProductCatalog.Title AS VehicleNo,
                                        QRCode.TravellerType,QRCode.CreatedByUserId, TimeClockActivityTypeLocalizedData.TimeClockActivityTypeId AS TypeId
                                        ,TimeClockActivityTypeLocalizedData.Label AS Type
                                        FROM dbo.QRCode
                                        LEFT OUTER JOIN dbo.TimeClockActivityTypeLocalizedData
                                        ON QRCode.ActivityTypeId = TimeClockActivityTypeLocalizedData.TimeClockActivityTypeId
                                        LEFT OUTER JOIN CorporateProductCatalog on QRCode.VehicleNo = CorporateProductCatalog.Id
                                        WHERE TimeClockActivityTypeLocalizedData.LanguageCode = '" + lang +
                            "' AND QRCode.Id = '" + id + "'";
                results = dbConnection.Query<QRCodeDapperDto, TimeClockActivityTypeDto,
                    QRCodeDapperDto>(query, (qrCode, type) =>
                {
                    qrCode.ActivityType = type;
                    return qrCode;
                }, splitOn: "TypeId");
                

                var qr = results.FirstOrDefault();
                qr.ProjectDefinition = applicationDbContext.ProjectDefinition.Where(p => p.Id == qr.ProjectId)
                    .FirstOrDefault();

                IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyLogDto = null;
                var historyQuery =
                    @"SELECT   QrHistoryLog.ChangedTime AS DateTime  ,QrHistoryLog.ChangedByUserId AS Oid,QrHistoryLog.RevisionNumber AS RevisionNumber 
                            FROM dbo.QrHistoryLog WHERE QrHistoryLog.QRCodeId =@id ORDER BY RevisionNumber";

                var historyparameters = new { id = qr.Id };
                historyLogDto =
                    dbConnection.Query<ProjectDefinitionHistoryLogDapperDto>(historyQuery, historyparameters);
                

                var log = new ProjectDefinitionHistoryLogDto();
                var historyUserQuery =
                    @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [User] FROM ApplicationUser WHERE ApplicationUser.Oid =@userId";
                IEnumerable<string> userName = null;
                if (historyLogDto.Any())
                {
                    var historydto = historyLogDto.First();
                    log.CreatedDateTime = historydto.DateTime;
                    log.RevisionNumber = historydto.RevisionNumber;
                    var historyUserParameter = new { userId = historydto.Oid };
                    using (var connection =
                           new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
                    {
                        userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                        log.CreatedByUser = userName.FirstOrDefault();
                        
                    }
                }

                if (historyLogDto.Count() >= 2)
                {
                    var historydto = historyLogDto.Last();
                    log.UpdatedDateTime = historydto.DateTime;
                    log.RevisionNumber = historydto.RevisionNumber;
                    var historyUserParameter = new { userId = historydto.Oid };
                    using (var connection =
                           new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
                    {
                        userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                        log.UpdatedByUser = userName.FirstOrDefault();
                        
                    }
                }

                qr.History = log;


                return results.FirstOrDefault();
            }
        }
    }

    public async Task<string> UpdateQrCode(ApplicationDbContext context1, UpdateQRCodeDto qRCodeDto,
        string contractingUnitSequenceId, string projectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<QRDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId, iTenantProvider);
        await using (var context = new QRDbContext(options, connectionString, iTenantProvider))
        {
            var qr = new QRCode
            {
                Date = qRCodeDto.Date,
                Id = qRCodeDto.Id,
                ProjectId = qRCodeDto.ProjectId,
                VehicleNo = qRCodeDto.VehicleNo,
                Location = qRCodeDto.Location,
                PersonalId = qRCodeDto.PersonalId,
                Type = qRCodeDto.Type,
                TravellerType = qRCodeDto.TravellerType,
                ActivityTypeId = context.TimeClockActivityType.FirstOrDefault(a => a.TypeId == qRCodeDto.Type)!
                    .Id
            };
            context.QRCode.Update(qr);
            await context.SaveChangesAsync();
            return qr.Id;
        }
    }

    private void CreateHistory(CreateQRCodeDto qrDto, bool isExist, string createdUserId,
        string contractingUnitSequenceId, string projectSequenceId, ITenantProvider iTenantProvider)
    {
        var options = new DbContextOptions<QRDbContext>();
        var connectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId, iTenantProvider);
        using var context = new QRDbContext(options, connectionString, iTenantProvider);
        var jsonProduct = JsonConvert.SerializeObject(qrDto, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var historyLog = new QrHistoryLog
        {
            Id = Guid.NewGuid().ToString(),
            ChangedTime = DateTime.UtcNow,
            ChangedByUserId = createdUserId,
            HistoryLog = jsonProduct,
            QRCodeId = qrDto.Id
        };

        historyLog.Action = isExist == false ? HistoryState.ADDED.ToString() : HistoryState.UPDATED.ToString();

        context.QrHistoryLog.Add(historyLog);
        context.SaveChanges();
    }
}