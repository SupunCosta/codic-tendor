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
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class QualityRepository : IQualityRepository
{
    public async Task<IEnumerable<Quality>> GetQualityList(QualityRepositoryParameter qualityRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            qualityRepositoryParameter.ContractingUnitSequenceId, qualityRepositoryParameter.ProjectSequenceId,
            qualityRepositoryParameter.TenantProvider);
        await using (var context =
                     new ShanukaDbContext(options, connectionString, qualityRepositoryParameter.TenantProvider))
        {
            var qualityList = context.Quality.Where(q => q.IsDeleted == false).ToList();
            return qualityList;
        }
    }

    public async Task<Quality> GetQualityById(QualityRepositoryParameter qualityRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        IEnumerable<QualityHistoryLogDapperDto> historyLogDto = null;

        var connectionString = ConnectionString.MapConnectionString(
            qualityRepositoryParameter.ContractingUnitSequenceId, qualityRepositoryParameter.ProjectSequenceId,
            qualityRepositoryParameter.TenantProvider);
        await using (var context =
                     new ShanukaDbContext(options, connectionString, qualityRepositoryParameter.TenantProvider))
        {
            var quality = context.Quality.FirstOrDefault(q =>
                q.IsDeleted == false && q.SequenceCode.Equals(qualityRepositoryParameter.QualityId));

            var historyQuery =
                @"SELECT QualityHistoryLog.ChangedTime AS DateTime ,QualityHistoryLog.ChangedByUserId AS Oid,QualityHistoryLog.RevisionNumber AS RevisionNumber 
            FROM dbo.QualityHistoryLog WHERE QualityHistoryLog.QualityId =@id ORDER BY RevisionNumber";

            var historyparameters = new { id = quality.Id };
            await using (var dbConnection1 = new SqlConnection(connectionString))
            {
                historyLogDto =
                    dbConnection1.Query<QualityHistoryLogDapperDto>(historyQuery, historyparameters);
                if (dbConnection1.State != ConnectionState.Closed) dbConnection1.Close();
            }

            //
            var log = new QualityHistoryLogDto();
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
                             new SqlConnection(qualityRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
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
                       new SqlConnection(qualityRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    userName = connection3.Query<string>(historyUserQuery, historyUserParameter);
                    log.UpdatedByUser = userName.FirstOrDefault();
                    if (connection3.State != ConnectionState.Closed) connection3.Close();
                }
            }

            quality.QualityHistoryLogDto = log;
            //

            return quality;
        }
    }

    public async Task<Quality> AddQuality(QualityRepositoryParameter qualityRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            qualityRepositoryParameter.ContractingUnitSequenceId, qualityRepositoryParameter.ProjectSequenceId,
            qualityRepositoryParameter.TenantProvider);
        await using (var context =
                     new ShanukaDbContext(options, connectionString, qualityRepositoryParameter.TenantProvider))
        {
            var quality = qualityRepositoryParameter.Quality;
            var existingQuality = context.Quality.FirstOrDefault(r => r.Id == quality.Id);
            var dbContext = qualityRepositoryParameter.ApplicationDbContext;

            Quality returnQuality = null;
            var jsonProduct = JsonConvert.SerializeObject(quality, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var historyLog = new QualityHistoryLog
            {
                Id = Guid.NewGuid().ToString(),
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = qualityRepositoryParameter.ChangedUser.OId,
                HistoryLog = jsonProduct
            };

            if (existingQuality != null)
            {
                existingQuality.Name = quality.Name;
                existingQuality.Criteria = quality.Criteria;
                existingQuality.Method = quality.Method;
                existingQuality.Skills = quality.Skills;
                existingQuality.Tolerance = quality.Tolerance;
                context.Update(existingQuality);
                await context.SaveChangesAsync();
                historyLog.QualityId = existingQuality.Id;
                historyLog.Action = HistoryState.UPDATED.ToString();
                returnQuality = existingQuality;
            }
            else
            {
                quality.Id = Guid.NewGuid().ToString();
                quality.SequenceCode = new IdGenerator().GenerateId(dbContext, "Q", "QualityId");
                context.Add(quality);
                await context.SaveChangesAsync();
                historyLog.QualityId = quality.Id;
                historyLog.Action = HistoryState.ADDED.ToString();
                returnQuality = quality;
            }

            var projectCon = ConnectionString.MapConnectionString(
                qualityRepositoryParameter.ContractingUnitSequenceId,
                qualityRepositoryParameter.ProjectSequenceId, qualityRepositoryParameter.TenantProvider);
            var projectConOptions = new DbContextOptions<ShanukaDbContext>();
            var applicationDbContext = new ShanukaDbContext(projectConOptions, projectCon,
                qualityRepositoryParameter.TenantProvider);
            applicationDbContext.QualityHistoryLog.Add(historyLog);
            await applicationDbContext.SaveChangesAsync();

            return returnQuality;
        }
    }

    public bool DeleteQuality(QualityRepositoryParameter qualityRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            qualityRepositoryParameter.ContractingUnitSequenceId, qualityRepositoryParameter.ProjectSequenceId,
            qualityRepositoryParameter.TenantProvider);
        using var context =
            new ShanukaDbContext(options, connectionString, qualityRepositoryParameter.TenantProvider);
        var isUpdated = false;
        foreach (var id in qualityRepositoryParameter.IdList)
        {
            var quality = context.Quality.FirstOrDefault(p => p.Id == id);
            if (quality != null)
            {
                quality.IsDeleted = true;
                context.Quality.Update(quality);
                context.SaveChanges();
                isUpdated = true;
            }
        }

        return isUpdated;
    }

    public async Task<IEnumerable<Quality>> Filter(QualityRepositoryParameter qualityRepositoryParameter)
    {
        var lang = qualityRepositoryParameter.Lang;
        var sb = new StringBuilder();
        sb.Append("select * from Quality ");

        if (qualityRepositoryParameter.QualityFilterModel.Title != null)
        {
            qualityRepositoryParameter.QualityFilterModel.Title = qualityRepositoryParameter.QualityFilterModel.Title.Replace("'", "''");
            sb.Append(" where CONCAT(SequenceCode, ' - ', Name) like '%" +
                      qualityRepositoryParameter.QualityFilterModel.Title + "%' ");

        }
            
        if (qualityRepositoryParameter.QualityFilterModel.SortingModel.Attribute == null)
            sb.Append(" order by SequenceCode desc");
        else
            sb.Append("order by SequenceCode " +
                      qualityRepositoryParameter.QualityFilterModel.SortingModel.Order);

        var options = new DbContextOptions<ShanukaDbContext>();

        var connectionString = ConnectionString.MapConnectionString(
            qualityRepositoryParameter.ContractingUnitSequenceId, qualityRepositoryParameter.ProjectSequenceId,
            qualityRepositoryParameter.TenantProvider);

        using var context =
            new ShanukaDbContext(options, connectionString, qualityRepositoryParameter.TenantProvider);

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            var result = await dbConnection.QueryAsync<Quality>(sb.ToString());
            

            if (qualityRepositoryParameter.QualityFilterModel.PbsProductId != null)
            {
                var pbsList = context.PbsQuality
                    .Where(x => x.PbsProductId == qualityRepositoryParameter.QualityFilterModel.PbsProductId)
                    .ToList();

                result = result
                    .Where(x => pbsList.All(c => c.QualityId != x.Id)).ToList();
            }

            return result;
        }
    }
}