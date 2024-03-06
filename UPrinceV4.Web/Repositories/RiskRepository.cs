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
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class RiskRepository : IRiskRepository
{
    public async Task<IEnumerable<RiskReadDto>> GetRiskList(RiskRepositoryParameter riskRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            riskRepositoryParameter.ContractingUnitSequenceId, riskRepositoryParameter.ProjectSequenceId,
            riskRepositoryParameter.TenantProvider);

        await using (var context =
                     new ShanukaDbContext(options, connectionString, riskRepositoryParameter.TenantProvider))
        {
            var riskList = context.Risk.Where(r => r.IsDeleted == false) /*.Include(p => p.Person)*/
                .Include(p => p.RiskStatus).Include(p => p.RiskType).ToList();
            var dtoList = new List<RiskReadDto>();
            foreach (var risk in riskList)
            {
                var riskDto = new RiskReadDto
                {
                    SequenceCode = risk.SequenceCode,
                    Name = risk.Name,
                    Title = risk.Title,
                    HeaderTitle = risk.HeaderTitle,
                    RiskDetails = risk.RiskDetails
                };
                riskDto.RiskStatus = risk.RiskStatus;
                riskDto.RiskType = risk.RiskType;

                await using var connection =
                    new SqlConnection(riskRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

                var person = new RiskPersonDto
                {
                    Id = risk.PersonId,
                    FullName = connection.Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Id ",
                        new { Id = risk.PersonId }).FirstOrDefault()
                };

                riskDto.Person = person;

                dtoList.Add(riskDto);
            }

            return dtoList;
        }
    }

    public async Task<RiskReadDtoDapper> GetRiskById(RiskRepositoryParameter riskRepositoryParameter)
    {
        var lang = riskRepositoryParameter.Lang;
        if (string.IsNullOrEmpty(riskRepositoryParameter.Lang) || lang.ToLower().Contains("en")) lang = "en";

        IEnumerable<RiskHistoryLogDapperDto> historyLogDto = null;
        IEnumerable<RiskReadDtoDapper> result = null;

        var sql =
            "SELECT  Risk.Name  ,Risk.Id  ,Risk.SequenceCode  ,Risk.RiskDetails , Risk.PersonId  ,CONCAT(Risk.SequenceCode, ' - ', Risk.Name) AS Title  ," +
            " RiskType.RiskTypeId AS Id  ,RiskType.Type  ,RiskStatus.RiskStatusId AS Id  ,RiskStatus.Status,CabPerson.Id,CabPerson.FullName " +
            " FROM dbo.Risk LEFT OUTER JOIN dbo.RiskType   ON Risk.RiskTypeId = RiskType.RiskTypeId  LEFT OUTER JOIN dbo.RiskStatus   ON Risk.RiskStatusId = RiskStatus.RiskStatusId " +
            " LEFT OUTER JOIN dbo.CabPerson   ON Risk.PersonId = CabPerson.Id WHERE (RiskType.LanguageCode = '" + lang +
            "' OR Risk.RiskTypeId IS NULL) AND (RiskStatus.LanguageCode = '" + lang +
            "' OR Risk.RiskStatusId IS NULL)" +
            " AND Risk.SequenceCode = '" + riskRepositoryParameter.RiskId + "'";

        var historyQuery =
            @"SELECT RiskHistoryLog.ChangedTime AS DateTime ,RiskHistoryLog.ChangedByUserId AS Oid,RiskHistoryLog.RevisionNumber AS RevisionNumber 
            FROM dbo.RiskHistoryLog WHERE RiskHistoryLog.RiskId =@id ORDER BY RevisionNumber";


        var connectionString = ConnectionString.MapConnectionString(
            riskRepositoryParameter.ContractingUnitSequenceId, riskRepositoryParameter.ProjectSequenceId,
            riskRepositoryParameter.TenantProvider);
        IEnumerable<RiskReadDtoDapper> results;


        await using var dbConnection = new SqlConnection(connectionString);
        {
            await dbConnection.OpenAsync();
            results = await dbConnection
                .QueryAsync<RiskReadDtoDapper, RiskTypeDto, RiskStatusDto, RiskPersonDto, RiskReadDtoDapper>
                (sql, (risk, riskType, riskStatus, riskPerson) =>
                {
                    risk.Id = risk.Id;
                    risk.RiskType = riskType;
                    risk.RiskStatus = riskStatus;
                    risk.Person = riskPerson;

                    return risk;
                }, splitOn: "Id,Id,Id");
            

            await using var connection =
                new SqlConnection(riskRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

            var historyparameters = new { id = results.First().Id };
            await using (var dbConnection1 = new SqlConnection(connectionString))
            {
                historyLogDto =
                    dbConnection1.Query<RiskHistoryLogDapperDto>(historyQuery, historyparameters);
                if (dbConnection1.State != ConnectionState.Closed) dbConnection1.Close();
            }

            var person = new RiskPersonDto
            {
                Id = results.ToList().FirstOrDefault().PersonId,
                FullName = connection.Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Id ",
                    new { Id = results.ToList().FirstOrDefault().PersonId }).FirstOrDefault()
            };
            //
            var log = new RiskHistoryLogDto();
            var historyUserQuery =
                @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [User] FROM ApplicationUser WHERE ApplicationUser.Oid =@userId";
            IEnumerable<string> userName = null;
            if (historyLogDto.Any())
            {
                var historydto = historyLogDto.First();
                log.CreatedDateTime = historydto.DateTime;
                log.RevisionNumber = historydto.RevisionNumber;
                var historyUserParameter = new { userId = historydto.Oid };
                await using (var connection2 =
                             new SqlConnection(riskRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    userName = connection2.Query<string>(historyUserQuery, historyUserParameter);
                    log.CreatedByUser = userName.FirstOrDefault();
                    if (connection2.State != ConnectionState.Closed) connection2.Close();
                }
            }

            if (historyLogDto.Count() >= 2)
            {
                var historydto = historyLogDto.Last();
                log.UpdatedDateTime = historydto.DateTime;
                log.RevisionNumber = historydto.RevisionNumber;
                var historyUserParameter = new { userId = historydto.Oid };
                using (var connection3 =
                       new SqlConnection(riskRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    userName = connection3.Query<string>(historyUserQuery, historyUserParameter);
                    log.UpdatedByUser = userName.FirstOrDefault();
                    if (connection3.State != ConnectionState.Closed) connection3.Close();
                }
            }

            results.ToList().FirstOrDefault().RiskHistoryLog = log;
            //
            results.ToList().FirstOrDefault().Person = person;
            return results.ToList().FirstOrDefault();
        }
    }

    public async Task<Risk> AddRisk(RiskRepositoryParameter riskRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            riskRepositoryParameter.ContractingUnitSequenceId, riskRepositoryParameter.ProjectSequenceId,
            riskRepositoryParameter.TenantProvider);
        await using (var context =
                     new ShanukaDbContext(options, connectionString, riskRepositoryParameter.TenantProvider))
        {
            var risk = riskRepositoryParameter.Risk;
            var existingRisk = context.Risk.FirstOrDefault(r => r.Id == risk.Id);
            var dbContext = riskRepositoryParameter.ApplicationDbContext;
            Risk returnRisk = null;
            var jsonProduct = JsonConvert.SerializeObject(risk, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var historyLog = new RiskHistoryLog
            {
                Id = Guid.NewGuid().ToString(),
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = riskRepositoryParameter.ChangedUser.OId,
                HistoryLog = jsonProduct
            };

            if (existingRisk != null)
            {
                existingRisk.Name = risk.Name;
                existingRisk.RiskDetails = risk.RiskDetails;
                existingRisk.PersonId = risk.PersonId;
                existingRisk.RiskTypeId = risk.RiskTypeId;
                existingRisk.RiskStatusId = risk.RiskStatusId;
                context.Update(existingRisk);
                await context.SaveChangesAsync();
                historyLog.RiskId = risk.Id;
                historyLog.Action = HistoryState.UPDATED.ToString();
                returnRisk = existingRisk;
            }
            else
            {
                risk.Id = Guid.NewGuid().ToString();
                risk.SequenceCode = new IdGenerator().GenerateId(dbContext, "R", "RiskId");
                context.Add(risk);
                await context.SaveChangesAsync();
                historyLog.RiskId = risk.Id;
                historyLog.Action = HistoryState.ADDED.ToString();
                returnRisk = risk;
            }

            var projectCon = ConnectionString.MapConnectionString(
                riskRepositoryParameter.ContractingUnitSequenceId, riskRepositoryParameter.ProjectSequenceId,
                riskRepositoryParameter.TenantProvider);
            var projectConOptions = new DbContextOptions<ShanukaDbContext>();
            var applicationDbContext = new ShanukaDbContext(projectConOptions, projectCon,
                riskRepositoryParameter.TenantProvider);
            applicationDbContext.RiskHistoryLog.Add(historyLog);
            await applicationDbContext.SaveChangesAsync();
            return returnRisk;
        }
    }

    public bool DeleteRisk(RiskRepositoryParameter riskRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            riskRepositoryParameter.ContractingUnitSequenceId, riskRepositoryParameter.ProjectSequenceId,
            riskRepositoryParameter.TenantProvider);
        using var context =
            new ShanukaDbContext(options, connectionString, riskRepositoryParameter.TenantProvider);
        var isUpdated = false;
        foreach (var id in riskRepositoryParameter.IdList)
        {
            var risk = context.Risk.FirstOrDefault(p => p.Id == id);
            if (risk != null)
            {
                risk.IsDeleted = true;
                context.Risk.Update(risk);
                context.SaveChanges();
                isUpdated = true;
            }
        }

        return isUpdated;
    }

    public async Task<RiskDropdown> GetRiskDropdownData(RiskRepositoryParameter riskRepositoryParameter)
    {
        var riskDropdown = new RiskDropdown
        {
            RiskTypes = await riskRepositoryParameter.IRiskTypeRepository.GetRiskTypeList(riskRepositoryParameter
                .RiskTypeRepositoryParameter),
            RiskStatus = await riskRepositoryParameter.IRiskStatusRepository.GetRiskStatusList(riskRepositoryParameter
                .RiskStatusRepositoryParameter)
        };
        return riskDropdown;
    }

    public async Task<IEnumerable<RiskReadDtoDapper>> Filter(RiskRepositoryParameter riskRepositoryParameter)
    {
        var lang = riskRepositoryParameter.Lang;
        var sql =
            "SELECT  Risk.Name  ,Risk.Id  ,Risk.SequenceCode  ,Risk.RiskDetails , Risk.PersonId  ,CONCAT(Risk.SequenceCode, ' - ', Risk.Name) AS Title  ," +
            "RiskType.RiskTypeId AS Id  ,RiskType.Type  ,RiskStatus.RiskStatusId AS Id  ,RiskStatus.Status  ,CabPerson.Id AS Id  ,CabPerson.FullName " +
            "FROM dbo.Risk LEFT OUTER JOIN dbo.RiskType   ON Risk.RiskTypeId = RiskType.RiskTypeId LEFT OUTER JOIN dbo.CabPerson   ON Risk.PersonId = CabPerson.Id " +
            "LEFT OUTER JOIN dbo.RiskStatus   ON Risk.RiskStatusId = RiskStatus.RiskStatusId WHERE (RiskType.LanguageCode = '" +
            lang + "' OR Risk.RiskTypeId IS NULL) AND (RiskStatus.LanguageCode = '"
            + lang + "' OR Risk.RiskStatusId IS NULL) AND (Risk.IsDeleted = 0) ";

        var sb = new StringBuilder(sql);
        if (riskRepositoryParameter.RiskFilterModel != null)
        {
            if (riskRepositoryParameter.RiskFilterModel.Title != null)
            {
                riskRepositoryParameter.RiskFilterModel.Title = riskRepositoryParameter.RiskFilterModel.Title.Replace("'", "''");
                sb.Append(" AND CONCAT(Risk.SequenceCode, ' - ', Risk.Name) LIKE '%" +
                          riskRepositoryParameter.RiskFilterModel.Title + "%'");

            }
               
            if (riskRepositoryParameter.RiskFilterModel.TypeId != null)
                sb.Append(" AND Risk.RiskTypeId = '" + riskRepositoryParameter.RiskFilterModel.TypeId + "'");

            if (riskRepositoryParameter.RiskFilterModel.StateId != null)
                sb.Append(" AND Risk.RiskStatusId ='" + riskRepositoryParameter.RiskFilterModel.StateId + "'");

            if (riskRepositoryParameter.RiskFilterModel.PersonId != null)
                sb.Append(" AND Risk.PersonId ='" + riskRepositoryParameter.RiskFilterModel.PersonId + "'");

            if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute != null &&
                riskRepositoryParameter.RiskFilterModel.SortingModel.Order != null)
            {
                if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute.ToLower() == "title")
                {
                    if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute != null &&
                        riskRepositoryParameter.RiskFilterModel.SortingModel.Order.ToLower().Equals("asc"))
                        sb.Append(" ORDER BY " + riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute +
                                  " ASC");

                    if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute != null &&
                        riskRepositoryParameter.RiskFilterModel.SortingModel.Order.ToLower().Equals("desc"))
                        sb.Append(" ORDER BY " + riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute +
                                  " DESC");
                }
                else if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute == "riskType")
                {
                    sb.Append(" ORDER BY RiskType.Type " + riskRepositoryParameter.RiskFilterModel.SortingModel.Order);
                }
                else if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute == "riskStatus")
                {
                    sb.Append(" ORDER BY RiskStatus.Status " +
                              riskRepositoryParameter.RiskFilterModel.SortingModel.Order);
                }
            }
            else
            {
                sb.Append(" ORDER BY SequenceCode DESC");
            }
        }

        var options = new DbContextOptions<ShanukaDbContext>();

        var connectionString = ConnectionString.MapConnectionString(
            riskRepositoryParameter.ContractingUnitSequenceId, riskRepositoryParameter.ProjectSequenceId,
            riskRepositoryParameter.TenantProvider);

        using var context =
            new ShanukaDbContext(options, connectionString, riskRepositoryParameter.TenantProvider);

        IEnumerable<RiskReadDtoDapper> results;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            {
                await dbConnection.OpenAsync();
                results = await dbConnection
                    .QueryAsync<RiskReadDtoDapper, RiskTypeDto, RiskStatusDto, RiskPersonDto,
                        RiskReadDtoDapper>(sb.ToString(), (risk, riskType, riskStatus, riskPerson) =>
                    {
                        risk.Person = riskPerson;
                        risk.RiskType = riskType;
                        risk.RiskStatus = riskStatus;
                        return risk;
                    }, splitOn: "Id,Id,Id");
                

                var list = results;

                foreach (var risks in list)
                {
                    await using var connection =
                        new SqlConnection(riskRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

                    var person = new RiskPersonDto
                    {
                        Id = risks.PersonId,
                        FullName = connection.Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Id ",
                            new { Id = risks.PersonId }).FirstOrDefault()
                    };

                    risks.Person = person;
                }

                if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute != null &&
                    riskRepositoryParameter.RiskFilterModel.SortingModel.Order != null)
                {
                    if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute.ToLower() == "person")
                    {
                        if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute != null &&
                            riskRepositoryParameter.RiskFilterModel.SortingModel.Order.ToLower().Equals("asc"))
                            list = list.OrderBy(c => c.Person.FullName).ToList();
                        if (riskRepositoryParameter.RiskFilterModel.SortingModel.Attribute != null &&
                            riskRepositoryParameter.RiskFilterModel.SortingModel.Order.ToLower().Equals("desc"))
                            list = list.OrderByDescending(c => c.Person.FullName).ToList();
                    }
                }
                else
                {
                    sb.Append(" ORDER BY SequenceCode DESC");
                }

                if (riskRepositoryParameter.RiskFilterModel.PbsProductId != null)
                {
                    var pbsList = context.PbsRisk
                        .Where(x => x.PbsProductId == riskRepositoryParameter.RiskFilterModel.PbsProductId)
                        .ToList();

                    list = list
                        .Where(x => pbsList.All(c => c.RiskId != x.Id)).ToList();
                }

                return list;
            }
        }
    }
}