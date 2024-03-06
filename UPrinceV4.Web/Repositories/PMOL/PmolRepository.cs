using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Repositories.PMOL;

public class PmolRepository : IPmolRepository
{
    public async Task<IEnumerable<PmolListDto>> GetPmolList(PmolParameter PmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);


        var query = @"SELECT PMol.Id, PMol.ProjectMoleculeId, PMol.Name, PMol.Title AS Title
                                ,PMol.ExecutionDate, PMolType.Name AS Type, PMolStatus.Name AS Status, PMolType.TypeId
                                ,PMolStatus.StatusId, PMol.ForemanId AS ForemanId
                                FROM dbo.PMol LEFT OUTER JOIN dbo.PMolType ON PMol.TypeId = PMolType.TypeId
                                LEFT OUTER JOIN dbo.PMolStatus ON PMol.StatusId = PMolStatus.StatusId
                                WHERE (PMolType.LanguageCode = @lang OR PMol.TypeId IS NULL)
                                AND (PMolStatus.LanguageCode = @lang  OR PMol.StatusId IS NULL) AND PMol.IsDeleted = 0 
";
        var sb = new StringBuilder(query);
        if (PmolParameter.filter.Title != null)
        {
            PmolParameter.filter.Title = PmolParameter.filter.Title.Replace("'", "''");
            sb.Append(" AND PMol.Title like '%" + PmolParameter.filter.Title + "%' ");
        }

        if (PmolParameter.filter.TypeId != null)
            sb.Append(" AND PMol.TypeId = '" + PmolParameter.filter.TypeId + "' ");
        if (PmolParameter.filter.StatusId != null)
            sb.Append("  AND PMol.StatusId = '" + PmolParameter.filter.StatusId + "' ");
        if (PmolParameter.filter.ExecutionDate != null)
        {
            var gmt = FindGmtDatetime(PmolParameter);
            sb.Append("  AND ExecutionDate BETWEEN '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
        }

        if (PmolParameter.filter.Date != null) sb = FilterByDate(sb, PmolParameter);
        switch (PmolParameter.filter.Status)
        {
            case 100:
                sb.Append(" AND ExecutionDate IS NULL ");
                break;
            case 200:
                sb.Append(" AND ExecutionDate IS NOT NULL AND IsFinished=0 ");
                break;
            case 300:
                sb.Append(" AND IsFinished=1 ");
                break;
        }

        if (PmolParameter.filter.Sorter.Attribute == null)
            sb.Append(" ORDER BY CAST(SUBSTRING(PMol.ProjectMoleculeId,6,20) AS INT) desc");

        if (PmolParameter.filter.Sorter.Attribute != null)
            switch (PmolParameter.filter.Sorter.Attribute.ToLower())
            {
                case "title":
                    sb.Append("ORDER BY CAST(SUBSTRING(PMol.ProjectMoleculeId,6,20) AS INT) " +
                              PmolParameter.filter.Sorter.Order);
                    break;
                case "type":
                    sb.Append("ORDER BY PMolType.Name " + PmolParameter.filter.Sorter.Order);
                    break;
                case "status":
                    sb.Append("ORDER BY PMolStatus.Name " + PmolParameter.filter.Sorter.Order);
                    break;
                case "executiondate":
                    sb.Append("ORDER BY PMol.ExecutionDate " + PmolParameter.filter.Sorter.Order);
                    break;
            }

        var parameters = new { lang = PmolParameter.Lang };
        IEnumerable<PmolListDto> data;
        await using (var connection = new SqlConnection(connectionString))
        {
            data = await connection.QueryAsync<PmolListDto>(sb.ToString(), parameters);
        }

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, PmolParameter.TenantProvider);

        foreach (var dto in data)
            if (dto.ForemanId != null)
            {
                IEnumerable<CabPerson> personList =
                    applicationDbContext.CabPerson.Where(p => p.Id == dto.ForemanId).ToList();
                if (personList.Count() != 0)
                    //dto.Foreman = applicationDbContext.CabPerson.Where(p => p.Id == dto.ForemanId).FirstOrDefault().FullName;

                    dto.Foreman = personList.FirstOrDefault()?.FullName;
                await using (var connection = new SqlConnection(connectionString))
                {
                    dto.IsJobNotDone = connection.Query<string>(
                        "SELECT PmolTeamRole.Id FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.IsDeleted = 0 AND PMolPlannedWorkLabour.PmolId = @pmolId  AND PmolTeamRole.IsJobDone = 1 AND PmolTeamRole.IsDeleted = 0",
                        new { pmolId = dto.Id }).Any();
                }
            }

        if (PmolParameter.filter.Foreman != null)
            data = data.Where(d =>
                d.Foreman != null &&
                d.Foreman.Contains(PmolParameter.filter.Foreman, StringComparison.OrdinalIgnoreCase));
        if (PmolParameter.filter.Sorter.Attribute != null)
            if (PmolParameter.filter.Sorter.Attribute.ToLower().Equals("foreman"))
                data = PmolParameter.filter.Sorter.Order == "asc"
                    ? data.OrderBy(d => d.Foreman)
                    : data.OrderByDescending(d => d.Foreman);

        return data;
    }


    public async Task<IEnumerable<PmolShortcutpaneDataDto>> GetShortcutpaneData(PmolParameter PmolParameter)
    {
        var query = @"select * from PMolShortcutpaneData where LanguageCode = @lang order by DisplayOrder ";
        var parameters = new { lang = PmolParameter.Lang };
        await using (var connection = new SqlConnection(PmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            return await connection.QueryAsync<PmolShortcutpaneDataDto>(query, parameters);
        }
    }

    public async Task<PmolDropdown> GetDropdownData(PmolParameter PmolParameter)
    {
        var dropdown = new PmolDropdown();

        var query = @"select StatusId as [Key], Name as Text  from PMolStatus where LanguageCode = @lang
                              ORDER BY DisplayOrder; select TypeId as [Key], Name as Text  from PMolType where LanguageCode = @lang order by Name";
        var parameters = new { lang = PmolParameter.Lang };
        await using (var dbConnection = new SqlConnection(PmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var muilti = await dbConnection.QueryMultipleAsync(query, parameters);
            dropdown.Status = await muilti.ReadAsync<PmolDropdownDto>();
            dropdown.Type = await muilti.ReadAsync<PmolDropdownDto>();
        }

        return dropdown;
    }


    public async Task<PmolGetByIdDto> GetPmolById(PmolParameter PmolParameter)
    {
        var _log4net = LogManager.GetLogger(typeof(PmolRepository));

        try
        {
            var lang = PmolParameter.Lang;
            string idBor = null;
            var borQuery = @"SELECT BorId FROM PMol WHERE PMol.ProjectMoleculeId =@id";
            string personId;

            var cuConnection = PmolParameter.TenantProvider.cuSqlConnection();
            var projectConnection = PmolParameter.TenantProvider.projectSqlConnection();
            var orgSqlConnection = PmolParameter.TenantProvider.orgSqlConnection();
            // aawait using var cuConnection = new SqlConnection(cuConnectionString);

            var isShiftStarted = cuConnection
                .Query("Select * From Shifts Where UserId = @UserId AND EndDateTime IS NULL",
                    new { UserId = PmolParameter.UserId }).Any();

            var query = @"
                               select PMol.Id,ProjectMoleculeId,PMol.Name,ForemanMobileNumber, ExecutionDate, ForemanId,Comment,ExecutionStartTime,ExecutionEndTime,PMol.IsDeleted,PMol.TypeId,StatusId,PMol.Title, Bor.ItemId as BorId, Pmol.LocationId, PMol.Title AS HeaderTitle, PMol.PmolType, PmolType.Type AS TypeNo, Pmol.IsFinished,Pmol.EndDateTime,Pmol.StartDateTime,
                               PbsProduct.Id AS ProductId, PbsProduct.Title AS ProductTitle,PMol.IsBreak,@isShiftStarted As IsShiftStart
                               from PMol
                               inner join Bor on PMol.BorId = Bor.id 
                               inner join PbsProduct on Bor.PbsProductId = PbsProduct.Id
                               left outer join PmolType on PMol.TypeId= PmolType.TypeId 
                               where (PMolType.LanguageCode = @lang OR PMol.TypeId IS NULL)
                               AND PMol.ProjectMoleculeId =@id
                             ";

            var query2 = @"
                               select PMol.Id,ProjectMoleculeId,PMol.Name,ForemanMobileNumber, ExecutionDate, ForemanId,Comment,PMol.IsDeleted,PMol.TypeId,StatusId,PMol.Title, Pmol.LocationId, PMol.Title AS HeaderTitle, PMol.PmolType, PmolType.Type AS TypeNo, Pmol.IsFinished,Pmol.EndDateTime,Pmol.StartDateTime,ExecutionStartTime,ExecutionEndTime,
                               PbsProduct.Id AS ProductId, PbsProduct.Title AS ProductTitle,PMol.IsBreak,@isShiftStarted As IsShiftStart
                               from PMol
                               
                               inner join PbsProduct on PMol.ProductId = PbsProduct.Id
                               left outer join PmolType on PMol.TypeId= PmolType.TypeId 
                               where (PMolType.LanguageCode = @lang OR PMol.TypeId IS NULL)
                               AND PMol.ProjectMoleculeId =@id
                             ";

            var breaks = @"SELECT * FROM dbo.PmolBreak WHERE PmolBreak.EndDateTime IS NULL AND PmolId = @mid";

            // var connectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            //     PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);
            var parameters = new { id = PmolParameter.Id, lang, isShiftStarted = isShiftStarted };
            // await using (var dbConnection = new SqlConnection(connectionString))
            // {
            //    
            // }
            idBor = projectConnection.Query<string>(borQuery, parameters).FirstOrDefault();

            PmolGetByIdDto result = null;

            if (idBor != null)
                // await using (var dbConnection = new SqlConnection(connectionString))
                // {
                result = projectConnection.Query<PmolGetByIdDto>(query, parameters).FirstOrDefault();
            if (result != null)
            {
                result.PmolBreak = projectConnection.Query<PmolBreak>(breaks, new { mid = result.Id })
                    .FirstOrDefault();
            }

            //}
            else
                // await using (var dbConnection = new SqlConnection(connectionString))
                // {
                result = projectConnection.Query<PmolGetByIdDto>(query2, parameters).FirstOrDefault();

            if (result != null)
            {
                result.PmolBreak = projectConnection.Query<PmolBreak>(breaks, new { mid = result.Id })
                    .FirstOrDefault();
            }


            //}

            var options1 = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options1, PmolParameter.TenantProvider);

            if (result != null && result.ForemanId != null)
                result.Foreman = applicationDbContext.CabPerson.FirstOrDefault(p => p.Id == result.ForemanId)
                    ?.FullName;
            personId = applicationDbContext.CabPersonCompany.FirstOrDefault(p => p.Oid == PmolParameter.UserId)
                ?.PersonId;


            var parameter = new BorParameter
            {
                Lang = lang,
                ProjectSequenceId = PmolParameter.ProjectSequenceId,
                ContractingUnitSequenceId = PmolParameter.ContractingUnitSequenceId,
                TenantProvider = PmolParameter.TenantProvider
            };
            if (result != null && result.BorId != null)
            {
                parameter.Id = result.BorId;
                var bor = await PmolParameter.borRepository.GetBorById(parameter);
                result.Bor = bor;
            }


            IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyLogDto = null;
            var historyQuery =
                @"SELECT   PMolHistoryLog.ChangedTime AS DateTime  ,PMolHistoryLog.ChangedByUserId AS Oid,PMolHistoryLog.RevisionNumber AS RevisionNumber 
                                    FROM dbo.PMolHistoryLog WHERE PMolHistoryLog.PmolId =@id ORDER BY RevisionNumber";

            var historyparameters = new { id = result.Id };
            
            historyLogDto =
                projectConnection.Query<ProjectDefinitionHistoryLogDapperDto>(historyQuery, historyparameters);
            

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
                userName = orgSqlConnection.Query<string>(historyUserQuery, historyUserParameter);
                log.CreatedByUser = userName.FirstOrDefault();
                
            }

            if (historyLogDto.Count() >= 2)
            {
                var historydto = historyLogDto.Last();
                log.UpdatedDateTime = historydto.DateTime;
                log.RevisionNumber = historydto.RevisionNumber;
                var historyUserParameter = new { userId = historydto.Oid };
                userName = orgSqlConnection.Query<string>(historyUserQuery, historyUserParameter);
                log.UpdatedByUser = userName.FirstOrDefault();
                
            }

            // await using (var connection = new SqlConnection(PmolParameter.TenantProvider.GetTenant().ConnectionString))
            // {
            result.ProjectDefinition = orgSqlConnection
                .Query<ProjectDefinitionMobDto>(
                    "SELECT Title, SequenceCode FROM dbo.ProjectDefinition WHERE SequenceCode = @SequenceCode",
                    new { SequenceCode = PmolParameter.ProjectSequenceId }).FirstOrDefault();
            result.ProjectDefinition.CuId = parameter.ContractingUnitSequenceId;
            // }

            var selectConsumable =
                @"SELECT BorConsumable.Id FROM dbo.BorConsumable WHERE BorConsumable.BorProductId = @BorProductId";
            var selectLabour = @"SELECT BorLabour.Id FROM dbo.BorLabour WHERE BorLabour.BorProductId = @BorProductId";
            var selectMaterials =
                @"SELECT BorMaterial.Id FROM dbo.BorMaterial WHERE BorMaterial.BorProductId = @BorProductId";
            var selectTools = @"SELECT BorTools.Id FROM dbo.BorTools  WHERE BorTools.BorProductId = @BorProductId";

            IEnumerable<PbsResourcesForBorDto> consumable;
            IEnumerable<PbsResourcesForBorDto> material;
            IEnumerable<PbsResourcesForBorDto> tools;
            IEnumerable<PbsResourcesForBorDto> labour;

            var parm = new { BorProductId = idBor };
            // await using (var connection = new SqlConnection(connectionString))
            // {
            consumable = projectConnection.Query<PbsResourcesForBorDto>(selectConsumable, parm);
            material = projectConnection.Query<PbsResourcesForBorDto>(selectMaterials, parm);
            tools = projectConnection.Query<PbsResourcesForBorDto>(selectTools, parm);
            labour = projectConnection.Query<PbsResourcesForBorDto>(selectLabour, parm);

            result.MaterialCount = material.Count().ToString();
            result.ConsumableCount = consumable.Count().ToString();
            result.ToolsCount = tools.Count().ToString();
            result.LabourCount = labour.Count().ToString();

            var isStart = projectConnection.Query<PmolLabourTime>(
                "SELECT PmolLabourTime.* FROM PmolLabourTime LEFT OUTER JOIN PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0",
                new { PmolId = result.Id }).Any();

            var isLabourStart = projectConnection.Query<PmolLabourTime>(
                "SELECT PmolLabourTime.* FROM PmolLabourTime LEFT OUTER JOIN PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 AND PmolTeamRole.CabPersonId = @personId Order By PmolLabourTime.StartDateTime desc ",
                new { PmolId = result.Id, personId }).FirstOrDefault();

            result.IsStarted = isStart;

            if (isLabourStart != null)
                result.IsBreak = isLabourStart.IsBreak;
            else
                result.IsBreak = true;
            //result.IsLabourStarted = isLabourStart;
            //}

            PmolParameter.Id = result.Id;
            result.IsForeman = await IsForeman(PmolParameter);


            result.historyLog = log;


            return result;
        }
        catch (Exception ex)
        {
            _log4net.Info("Exception thrown");
            _log4net.Error(ex.ToString());
            throw new Exception(ex.Message);
        }
    }

    public async Task<PmolDto> GetPmolByIdNew(PmolParameter PmolParameter)
    {
        var lang = PmolParameter.Lang;
        string idBor = null;
        var borQuery = @"SELECT BorId FROM PMol WHERE PMol.ProjectMoleculeId =@id";


        var query =
            @"select PMol.Id,ProjectMoleculeId,PMol.Name,ForemanMobileNumber, ExecutionDate, ForemanId,Comment,ExecutionStartTime,ExecutionEndTime,PMol.IsDeleted,PMol.TypeId,StatusId,PMol.Title,PMol.PmolLotId, Bor.ItemId as BorId, Pmol.LocationId, PMol.Title AS HeaderTitle, PMol.PmolType, PmolType.Type AS TypeNo, Pmol.IsFinished,Pmol.EndDateTime,Pmol.StartDateTime,
                               PbsProduct.Id AS ProductId, PbsProduct.Title AS ProductTitle,PMol.IsBreak
                               from PMol
                               inner join Bor on PMol.BorId = Bor.id 
                               inner join PbsProduct on Bor.PbsProductId = PbsProduct.Id
                               left outer join PmolType on PMol.TypeId= PmolType.TypeId 
                               where (PMolType.LanguageCode = @lang OR PMol.TypeId IS NULL)
                               AND PMol.ProjectMoleculeId =@id;SELECT  * FROM dbo.PmolBreak INNER JOIN dbo.PMol  ON PmolBreak.PmolId = PMol.Id WHERE PmolBreak.EndDateTime IS NULL AND PMol.ProjectMoleculeId =  @id
                             ";

        var query2 = @"
                               select PMol.Id,ProjectMoleculeId,PMol.Name,ForemanMobileNumber, ExecutionDate, ForemanId,Comment,ExecutionStartTime,ExecutionEndTime,PMol.IsDeleted,PMol.TypeId,StatusId,PMol.Title, Pmol.LocationId, PMol.Title AS HeaderTitle, PMol.PmolType, PmolType.Type AS TypeNo, Pmol.IsFinished,Pmol.EndDateTime,Pmol.StartDateTime,PMol.PmolLotId,
                               PbsProduct.Id AS ProductId, PbsProduct.Title AS ProductTitle,PMol.IsBreak
                               from PMol
                               
                               inner join PbsProduct on PMol.ProductId = PbsProduct.Id
                               left outer join PmolType on PMol.TypeId= PmolType.TypeId 
                               where (PMolType.LanguageCode = @lang OR PMol.TypeId IS NULL)
                               AND PMol.ProjectMoleculeId =@id ; SELECT  * FROM dbo.PmolBreak INNER JOIN dbo.PMol  ON PmolBreak.PmolId = PMol.Id WHERE PmolBreak.EndDateTime IS NULL AND PMol.ProjectMoleculeId =  @id
                             ";


        var connectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);

        var parameters = new { id = PmolParameter.Id, lang };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            idBor = dbConnection.Query<string>(borQuery, parameters).FirstOrDefault();
        }

        PmolGetByIdDto result = null;

        if (idBor != null)
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                await dbConnection.OpenAsync();
                using (var multi = await dbConnection.QueryMultipleAsync(query, parameters))
                {
                    result = multi.Read<PmolGetByIdDto>().FirstOrDefault();
                    if (result != null) result.PmolBreak = multi.Read<PmolBreak>().FirstOrDefault();
                }
            }
        else
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                await dbConnection.OpenAsync();
                using (var multi = await dbConnection.QueryMultipleAsync(query2, parameters))
                {
                    result = multi.Read<PmolGetByIdDto>().FirstOrDefault();
                    if (result != null) result.PmolBreak = multi.Read<PmolBreak>().FirstOrDefault();
                }
            }

        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, PmolParameter.TenantProvider);

        if (result != null && result.ForemanId != null)
            result.Foreman = applicationDbContext.CabPerson.FirstOrDefault(p => p.Id == result.ForemanId)
                ?.FullName;

        var parameter = new BorParameter
        {
            Lang = lang,
            ProjectSequenceId = PmolParameter.ProjectSequenceId,
            ContractingUnitSequenceId = PmolParameter.ContractingUnitSequenceId,
            TenantProvider = PmolParameter.TenantProvider
        };

        string pbsLocation = null;
        if (result.BorId != null)
        {
            parameter.Id = result.BorId;
            // var bor = await PmolParameter.borRepository.GetBorById(parameter);
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                {
                    var queryX = @"SELECT
                   Bor.Id AS Id
                       ,Bor.ItemId
                       ,Bor.Title AS BorTitle
                       ,Bor.BorStatusId
                       ,Bor.Name
                       ,CONCAT(Bor.ItemId, ' - ', Bor.Name) AS HeaderTitle
                       ,Bor.BorTypeId
                       ,Bor.StartDate
                       ,Bor.EndDate
                       ,PbsProduct.ProductId
                       ,PbsProduct.Name
                       ,PbsProduct.Id
                       ,PbsProduct.Title AS Title
                       ,PbsProduct.PbsProductItemTypeId
                       ,PbsProduct.PbsLocation
                       ,PbsProductItemTypeLocalizedData.Label AS PbsProductItemType
                       FROM dbo.Bor
                       LEFT OUTER JOIN dbo.PbsProduct
                       ON Bor.PbsProductId = PbsProduct.Id
                   LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData
                       ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId
                   WHERE (PbsProductItemTypeLocalizedData.LanguageCode = @lang
                   OR PbsProduct.PbsProductItemTypeId IS NULL)
                   AND Bor.ItemId = @ItemId";
                    var paramNew = new { ItemId = result.BorId, lang };

                    result.Bor = dbConnection.Query<BorGetByIdDto, BorGetByIdProductDto,
                        BorGetByIdDto>(queryX, (bor, product) =>
                    {
                        bor.Product = product;
                        return bor;
                    }, paramNew, splitOn: "ProductId").FirstOrDefault();

                    result.Bor.Product.UtilityTaxonomyParentId =
                        dbConnection.Query<string>(
                            "SELECT t.PbsTaxonomyNodeId AS UtilityTaxonomyParentId FROM PbsProduct p INNER JOIN PbsProductTaxonomy t ON p.Id = t.PbsProductId WHERE (t.PbsTaxonomyId = '6e54725c-e396-4ce4-88f3-a6e9678a0389') AND p.IsDeleted = 0 AND p.Id = @Id",
                            new { result.Bor.Product.Id }).FirstOrDefault();
                    result.Bor.Product.LocationTaxonomyParentId =
                        dbConnection.Query<string>(
                            "SELECT t.PbsTaxonomyNodeId AS LocationTaxonomyParentId FROM PbsProduct p INNER JOIN PbsProductTaxonomy t ON p.Id = t.PbsProductId WHERE (t.PbsTaxonomyId = 'ab294299-f251-41a8-94bd-3ae0150df134') AND p.IsDeleted = 0 AND p.Id = @Id",
                            new { result.Bor.Product.Id }).FirstOrDefault();
                    pbsLocation = result.Bor.Product.PbsLocation;
                }
            }
        }

        IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyLogDto = null;
        var historyQuery =
            @"SELECT   PMolHistoryLog.ChangedTime AS DateTime  ,PMolHistoryLog.ChangedByUserId AS Oid,PMolHistoryLog.RevisionNumber AS RevisionNumber 
                                    FROM dbo.PMolHistoryLog WHERE PMolHistoryLog.PmolId =@id ORDER BY RevisionNumber";

        var historyparameters = new { id = result.Id };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            historyLogDto =
                dbConnection.Query<ProjectDefinitionHistoryLogDapperDto>(historyQuery, historyparameters);
        }

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
            using (var connection = new SqlConnection(PmolParameter.TenantProvider.GetTenant().ConnectionString))
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
            using (var connection = new SqlConnection(PmolParameter.TenantProvider.GetTenant().ConnectionString))
            {
                userName = connection.Query<string>(historyUserQuery, historyUserParameter);

                log.UpdatedByUser = userName.FirstOrDefault();
            }
        }

        await using (var connection = new SqlConnection(PmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            result.ProjectDefinition = connection
                .Query<ProjectDefinitionMobDto>(
                    "SELECT Title, SequenceCode FROM dbo.ProjectDefinition WHERE SequenceCode = @SequenceCode",
                    new { SequenceCode = PmolParameter.ProjectSequenceId }).FirstOrDefault();
            result.ProjectDefinition.CuId = parameter.ContractingUnitSequenceId;
        }

        var selectConsumable =
            @"SELECT BorConsumable.Id FROM dbo.BorConsumable WHERE BorConsumable.BorProductId = @BorProductId;SELECT BorLabour.Id FROM dbo.BorLabour WHERE BorLabour.BorProductId = @BorProductId;SELECT BorMaterial.Id FROM dbo.BorMaterial WHERE BorMaterial.BorProductId = @BorProductId;SELECT BorTools.Id FROM dbo.BorTools  WHERE BorTools.BorProductId = @BorProductId";

        IEnumerable<PbsResourcesForBorDto> consumable;
        IEnumerable<PbsResourcesForBorDto> material;
        IEnumerable<PbsResourcesForBorDto> tools;
        IEnumerable<PbsResourcesForBorDto> labour;

        var parm = new { BorProductId = idBor };
        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var multi = await connection.QueryMultipleAsync(selectConsumable, parm))
            {
                consumable = multi.Read<PbsResourcesForBorDto>().ToList();
                material = multi.Read<PbsResourcesForBorDto>().ToList();
                tools = multi.Read<PbsResourcesForBorDto>().ToList();
                labour = multi.Read<PbsResourcesForBorDto>().ToList();
            }

            result.MaterialCount = material.Count().ToString();
            result.ConsumableCount = consumable.Count().ToString();
            result.ToolsCount = tools.Count().ToString();
            result.LabourCount = labour.Count().ToString();
        }

        result.historyLog = log;

        var pmolDto = new PmolDto
        {
            Header = result
        };

        //Instruction
        var sql = @"with name_tree as 
                                   (
                                    select Id, PbsProductId, PbsTaxonomyNodeId
                                    from PbsProductTaxonomy
                                    where PbsProductId = @PbsProductId
                                    union all
                                    select c.Id, c.PbsProductId, c.PbsTaxonomyNodeId
                                    from PbsProductTaxonomy c
                                    join name_tree p on p.PbsTaxonomyNodeId = c.PbsProductId
                                    )
                                    select PbsTaxonomyNodeId
                                    from name_tree
                                    where PbsTaxonomyNodeId is not null ";


        var sql2 = @"SELECT
                      Instructions.*
                    ,PbsInstruction.Id As PbsInstructionId
                    ,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
                     ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
                     ,PbsInstructionFamilyLocalizedData.Family AS Text
                    FROM dbo.PbsInstruction
                    LEFT OUTER JOIN dbo.Instructions
                      ON PbsInstruction.InstructionsId = Instructions.Id
                    LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
                      ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
                    WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
                    OR Instructions.PbsInstructionFamilyId IS NULL)
                    AND PbsInstruction.PbsProductId IN @PbsProductId
                    AND Instructions.InstructionType = @InstructionType AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false'";

        var sqlLink =
            @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

        var param = new { PbsProductId = result.ProductId };

        var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
        await using (var connection = new SqlConnection(connectionString))
        {
            var Idlist = connection.Query<string>(sql, param).ToList();
            Idlist.Add(result.ProductId);

            var instructionDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    }, new { lang = PmolParameter.Lang, PbsProductId = Idlist, InstructionType = 100 },
                    splitOn: "Key").ToList();

            foreach (var instructionLoadDtoss in instructionDto)
                instructionLoadDtoss.PbsInstructionLink = connection
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();
            pbsInstructionLoadAllPmolDto.Technical = instructionDto;


            var instructionenvironmentalDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    }, new { lang = PmolParameter.Lang, PbsProductId = Idlist, InstructionType = 200 },
                    splitOn: "Key").ToList();

            foreach (var instructionLoadDtoss in instructionenvironmentalDto)
                instructionLoadDtoss.PbsInstructionLink = connection
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();

            pbsInstructionLoadAllPmolDto.Environmental = instructionenvironmentalDto;


            var instructionsafetyDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    }, new { lang = PmolParameter.Lang, PbsProductId = Idlist, InstructionType = 300 },
                    splitOn: "Key").ToList();

            foreach (var instructionLoadDtoss in instructionsafetyDto)
                instructionLoadDtoss.PbsInstructionLink = connection
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();

            pbsInstructionLoadAllPmolDto.Safety = instructionsafetyDto;

            var instructionhealthDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    }, new { lang = PmolParameter.Lang, PbsProductId = Idlist, InstructionType = 400 },
                    splitOn: "Key").ToList();

            foreach (var instructionLoadDtoss in instructionhealthDto)
                instructionLoadDtoss.PbsInstructionLink = connection
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();

            pbsInstructionLoadAllPmolDto.Health = instructionhealthDto;
            
        }

        pmolDto.Instruction = pbsInstructionLoadAllPmolDto;

        //PlannedResource
        var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
        var mPmolResourceReadAllDtoExtra = new PmolResourceReadAllDto();
        var parametersResource = new { id = pmolDto.Header.Id, lang };

        //ReadLabour
        var isWeb = true;
        var queryReadLabour = @"
                               SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                               FROM PMolPlannedWorkLabour con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                               LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                               WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                               AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0;
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkConsumable con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0;
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkMaterial con
                                INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId IS NULL)
                                AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0;
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId ,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkTools con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0;
                                ";
        var extra =
            @"SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                               FROM PMolPlannedWorkLabour con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                               LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                               WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                               AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0;
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkConsumable con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0;
                                    SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                    con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                    FROM PMolPlannedWorkMaterial con
                                    INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                    LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                    WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                    AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0;
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId ,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkTools con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0";
        var sqlNickName =
            @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
            lang + "%'";

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            using (var multi = await dbConnection.QueryMultipleAsync(queryReadLabour, parametersResource))
            {
                mPmolResourceReadAllDto.Labour = multi.Read<PmolResourceReadDto>().ToList();
                mPmolResourceReadAllDto.Consumable = multi.Read<PmolResourceReadDto>().ToList();
                mPmolResourceReadAllDto.Material = multi.Read<PmolResourceReadDto>().ToList();
                mPmolResourceReadAllDto.Tools = multi.Read<PmolResourceReadDto>().ToList();
            }

            using (var multi = await dbConnection.QueryMultipleAsync(extra, parametersResource))
            {
                mPmolResourceReadAllDtoExtra.Labour = multi.Read<PmolResourceReadDto>().ToList();
                mPmolResourceReadAllDtoExtra.Consumable = multi.Read<PmolResourceReadDto>().ToList();
                mPmolResourceReadAllDtoExtra.Material = multi.Read<PmolResourceReadDto>().ToList();
                mPmolResourceReadAllDtoExtra.Tools = multi.Read<PmolResourceReadDto>().ToList();
            }

            if (lang != "nl")
            {
                foreach (var pR in mPmolResourceReadAllDto.Labour)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }

                foreach (var pR in mPmolResourceReadAllDto.Consumable)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }

                foreach (var pR in mPmolResourceReadAllDto.Material)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }

                foreach (var pR in mPmolResourceReadAllDto.Tools)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }

                foreach (var pR in mPmolResourceReadAllDtoExtra.Labour)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }

                foreach (var pR in mPmolResourceReadAllDtoExtra.Consumable)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }

                foreach (var pR in mPmolResourceReadAllDtoExtra.Material)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }

                foreach (var pR in mPmolResourceReadAllDtoExtra.Tools)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Count() > 0)
                        pR.Title = pR.ResourceNumber + " - " + resultNickNames.First().NickName;
                }
            }
        }

        pmolDto.PlannedResource = mPmolResourceReadAllDto;
        pmolDto.ExtraResource = mPmolResourceReadAllDtoExtra;

        //GetAllPbsQualityByPbsProductId
        var options = new DbContextOptions<ShanukaDbContext>();
        var qualityDto = new List<QualityDapperDto>();
        var sqlGetAllPbsQualityByPbsProductId = @"with name_tree as 
                                   (
                                    select Id, PbsProductId, PbsTaxonomyNodeId
                                    from PbsProductTaxonomy
                                    where PbsProductId = @PbsProductId
                                    union all
                                    select c.Id, c.PbsProductId, c.PbsTaxonomyNodeId
                                    from PbsProductTaxonomy c
                                    join name_tree p on p.PbsTaxonomyNodeId = c.PbsProductId
                                    )
                                    select PbsTaxonomyNodeId
                                    from name_tree
                                    where PbsTaxonomyNodeId is not null ";

        var sql2GetAllPbsQualityByPbsProductId = @"SELECT
              Quality.Id as Id
             ,Quality.Id as PbsQualityId
             ,PbsQuality.PbsProductId
             ,PbsQuality.QualityId
             ,Quality.IsSaved
             ,Quality.IsDeleted
             ,Quality.Name
             ,Quality.Criteria
             ,Quality.Tolerance
             ,Quality.Method
             ,Quality.Skills
             ,Quality.SequenceCode
             ,Quality.ProjectDefinitionId
             ,PbsQuality.Id  AS Uid
            FROM dbo.PbsQuality
            INNER JOIN dbo.Quality
              ON PbsQuality.QualityId = Quality.Id WHERE PbsQuality.PbsProductId In @PbsProductId";

        var IdlistGetAllPbsQualityByPbsProductId = new List<string>();
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            IdlistGetAllPbsQualityByPbsProductId =
                dbConnection.Query<string>(sqlGetAllPbsQualityByPbsProductId, param).ToList();
            IdlistGetAllPbsQualityByPbsProductId.Add(result.ProductId);
            qualityDto = dbConnection.Query<QualityDapperDto>(sql2GetAllPbsQualityByPbsProductId,
                new { PbsProductId = IdlistGetAllPbsQualityByPbsProductId }).ToList();
            
        }

        pmolDto.Quality = qualityDto;

        //GetCompetenceByPbsId
        var sqlGetCompetenceByPbsId = @"SELECT
                            PbsSkillExperience.PbsProductId
                            ,PbsSkillExperience.Id
                            ,PbsSkillLocalizedData.PbsSkillId AS SkillId
                            ,PbsSkillLocalizedData.Label AS Skill
                            ,PbsExperienceLocalizedData.PbsExperienceId AS ExperienceId
                            ,PbsExperienceLocalizedData.Label AS Experience
                             FROM dbo.PbsSkillExperience
                             LEFT OUTER JOIN dbo.PbsSkillLocalizedData
                            ON PbsSkillExperience.PbsSkillId = PbsSkillLocalizedData.PbsSkillId
                            LEFT OUTER JOIN dbo.PbsExperienceLocalizedData
                            ON PbsSkillExperience.PbsExperienceId = PbsExperienceLocalizedData.PbsExperienceId
                            WHERE PbsSkillLocalizedData.LanguageCode = @lang
                            AND PbsExperienceLocalizedData.LanguageCode = @lang 
                            AND PbsSkillExperience.PbsProductId = @id
                            order by Id";

        using (var dbConnection = new SqlConnection(connectionString))
        {
            var resultGetCompetenceByPbsId =
                dbConnection.Query<PbsSkillExperienceDto>(sqlGetCompetenceByPbsId,
                    new { id = pmolDto.Header.ProductId, lang });
            pmolDto.Competencies = resultGetCompetenceByPbsId;
        }

        PmolParameter.Id = pmolDto.Header.Id;
        //ReadJournal

        var sqlReadJournal = "select * from PmolJournal where PmolId = @Id";

        var paramReadJournal = new { PmolParameter.Id };
        PmolJournalCreateDto journal;
        using (var dbConnection = new SqlConnection(connectionString))
        {
            journal = await dbConnection.QueryFirstOrDefaultAsync<PmolJournalCreateDto>(sqlReadJournal,
                paramReadJournal);
        }

        if (journal != null)
        {
            var sqlPicture = "select * from PMolJournalPicture where PmolJournalId = @Id";

            var paramPicture = new { journal.Id };
            using (var dbConnection = new SqlConnection(connectionString))
            {
                journal.PictureList =
                    await dbConnection.QueryAsync<PmolJournalPictureCreateDto>(sqlPicture, paramPicture);
                journal.WhatsLeftToDo = dbConnection.Query<string>(
                    "SELECT Message FROM  PmolTeamRole  LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @pmolId AND PmolTeamRole.IsJobDone = 1",
                    new { pmolId = PmolParameter.Id }).FirstOrDefault();
            }

            pmolDto.Journal = journal;
        }
        else
        {
            var jjj = new PmolJournalCreateDto();

            using (var dbConnection = new SqlConnection(connectionString))
            {
                jjj.PmolId = PmolParameter.Id;
                jjj.WhatsLeftToDo = dbConnection.Query<string>(
                    "SELECT Message FROM  PmolTeamRole  LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @pmolId AND PmolTeamRole.IsJobDone = 1",
                    new { pmolId = PmolParameter.Id }).FirstOrDefault();
            }

            pmolDto.Journal = jjj;
        }


        //GetLocationByPmolId

        var connectionString1 = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);
        var context2 = new ShanukaDbContext(options, connectionString1, PmolParameter.TenantProvider);

        var MapLocation = context2.MapLocation.Where(L => L.Id == pmolDto.Header.LocationId).Include(m => m.Address)
            .Include(m => m.Position)
            .ToList().FirstOrDefault();
        if (MapLocation == null)
        {
            if (pbsLocation != null)
            {
                MapLocation = context2.MapLocation.Where(L => L.Id == pbsLocation).Include(m => m.Address)
                    .Include(m => m.Position)
                    .ToList().FirstOrDefault();
                if (MapLocation == null)
                {
                    var context = new ShanukaDbContext(options, PmolParameter.TenantProvider.GetTenant().ConnectionString,
                        PmolParameter.TenantProvider);

                    var projectDef =
                        context.ProjectDefinition.FirstOrDefault(p =>
                            p.SequenceCode == PmolParameter.ProjectSequenceId);
                    if (projectDef != null)
                        MapLocation = context.MapLocation.Where(l => l.Id == projectDef.LocationId)
                            .Include(m => m.Address).Include(m => m.Position)
                            .ToList().FirstOrDefault();
                }

            }
        }

        pmolDto.MapLocation = MapLocation;

        // var pmol = context.PMol.FirstOrDefault(p => p.Id == PmolParameter.Id);
        // if (pmol != null)
        // {
        //     //var context = new ShanukaDbContext(options, PmolParameter.TenantProvider.GetTenant().ConnectionString, PmolParameter.TenantProvider);
        //
        //     var MapLocation = context.MapLocation.Where(L => L.Id == pmolDto.Header.LocationId).Include(m => m.Address)
        //         .Include(m => m.Position)
        //         .ToList().FirstOrDefault();
        //     if (MapLocation == null)
        //     {
        //         var projectDef =
        //             context.ProjectDefinition.FirstOrDefault(p =>
        //                 p.SequenceCode == PmolParameter.ProjectSequenceId);
        //         if (projectDef != null)
        //             MapLocation = context.MapLocation.Where(l => l.Id == projectDef.LocationId)
        //                 .Include(m => m.Address).Include(m => m.Position)
        //                 .ToList().FirstOrDefault();
        //     }
        //
        //     pmolDto.MapLocation = MapLocation;
        //}

        //GetExtraWorkByPmolId
        pmolDto.ExtraWork = await GetExtraWorkByPmolId(PmolParameter);

        //GetStopHandShakesByPmolId
        pmolDto.StopHandshake = await GetStopHandShakesByPmolId(PmolParameter);

        //ReadPmolServiceByPmolId
        pmolDto.PmolService = await ReadPmolServiceByPmolId(PmolParameter);

        return pmolDto;
    }

    public async Task<Pmol> CreateHeader(PmolParameter PmolParameter, bool isClone)
    {
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, PmolParameter.TenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);
        var cuConnectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            null, PmolParameter.TenantProvider);
        var shanukaDbContext = new ShanukaDbContext(options, connectionString, PmolParameter.TenantProvider);
        string pmolStatusId = null;

        if (PmolParameter.PmolDto.StatusId != null)
        {
            pmolStatusId = PmolParameter.PmolDto.StatusId;
        }
        else
        {
            var pmolStatus = shanukaDbContext.PMolStatus.FirstOrDefault(p => p.Name.Equals("Pending Development"));

            if (pmolStatus != null) pmolStatusId = pmolStatus.StatusId;
        }

        PmolParameter.PmolDto.LocationId ??= await CreateLocation(PmolParameter, true);

        Pmol pmol;

        await using var connection = new SqlConnection(connectionString);

        await using (var context = shanukaDbContext)
        {
            var isExist = context.PMol.Any(p => p.Id == PmolParameter.PmolDto.Id);

            if (isExist == false)
            {
                pmol = new Pmol
                {
                    Comment = PmolParameter.PmolDto.Comment,
                    ExecutionDate = PmolParameter.PmolDto.ExecutionDate,
                    ForemanId = PmolParameter.PmolDto.ForemanId,
                    ForemanMobileNumber = PmolParameter.PmolDto.ForemanMobileNumber,
                    Id = PmolParameter.PmolDto.Id,
                    IsDeleted = false,
                    ExecutionStartTime = PmolParameter.PmolDto.ExecutionStartTime,
                    ExecutionEndTime = PmolParameter.PmolDto.ExecutionEndTime,
                    Name = PmolParameter.PmolDto.Name,
                    BorId = PmolParameter.PmolDto.PmolType == "sub" ? null : PmolParameter.PmolDto.Bor.Id,
                    StatusId = pmolStatusId,
                    TypeId = PmolParameter.PmolDto.TypeId,
                    LocationId = PmolParameter.PmolDto.LocationId,
                    IsFinished = PmolParameter.PmolDto.IsFinished,
                    StartDateTime = null,
                    EndDateTime = null,
                    IsBreak = true,
                    PmolLotId = PmolParameter.PmolDto.PmolLotId
                };

                if (pmol.ExecutionStartTime == null && pmol.ExecutionEndTime == null)
                {
                    pmol.ExecutionStartTime = "05:00";
                    pmol.ExecutionEndTime = "14:00";
                }

                var idGenerator = new IdGenerator();
                pmol.ProjectMoleculeId = idGenerator.GenerateId(applicationDbContext, "PMOL-", "PmolSequenceCode");
                pmol.Title = pmol.ProjectMoleculeId + " - " + PmolParameter.PmolDto.Name;
                pmol.PmolType = PmolParameter.PmolDto.PmolType;
                if (isClone != true)
                    pmol.ProductId = context.PbsProduct
                        .FirstOrDefault(p => p.ProductId == PmolParameter.PmolDto.ProductId)
                        ?.Id;
                else
                    pmol.ProductId = PmolParameter.PmolDto.ProductId;
                pmol.ProjectSequenceCode = PmolParameter.ProjectSequenceId;
                pmol.ParentId = PmolParameter.PmolDto.ParentId;

                context.PMol.Add(pmol);
                await context.SaveChangesAsync();

                if (isClone == false)
                {
                    var pmolResourceParameter = new PmolResourceParameter
                    {
                        ContextAccessor = PmolParameter.ContextAccessor,
                        ContractingUnitSequenceId = PmolParameter.ContractingUnitSequenceId,
                        Lang = PmolParameter.Lang,
                        ProjectSequenceId = PmolParameter.ProjectSequenceId,
                        TenantProvider = PmolParameter.TenantProvider,
                        Id = PmolParameter.PmolDto.Bor.Id
                    };
                    await CopyResourcesFromBorToPmol(pmolResourceParameter, pmol.Id);

                    if (!PmolParameter.isMyCal)
                    {
                        //from pbs cbc
                        var cbcCopy = @"INSERT INTO dbo.PmolCbcResources
                                SELECT
                                NEWID(),
                                @pmolId,
                                pcr.LotId AS LotId,
                                pcr.ArticleNo AS ArticleNo,
                                pcr.Quantity AS Quantity,
                                pcr.ConsumedQuantity AS ConsumedQuantity
                                FROM
                                PbsCbcResources pcr
                                WHERE pcr.PbsId = @PbsId
                                GROUP BY pcr.Id,pcr.LotId,pcr.ArticleNo,pcr.Quantity,pcr.ConsumedQuantity";

                        await connection.ExecuteAsync(cbcCopy, new { pmolId = pmol.Id, PbsId = pmol.ProductId });
                    }

                }

                if (!isClone)
                {
                    if (!PmolParameter.isMyCal)
                    {
                        if (PmolParameter.PmolDto.ForemanId != null)
                        {
                            ForemanAddToPmol(PmolParameter, connectionString, false);
                        }
                        else
                        {

                            var pmolLabourItems = connection.Query<PmolTeamRole>(
                                    @"SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.RoleId = 'Foreman' AND PmolTeamRole.IsDeleted = 0 ",
                                    new { PmolId = PmolParameter.PmolDto.Id })
                                .FirstOrDefault();

                            if (pmolLabourItems != null)
                                await connection.ExecuteAsync("Update PmolTeamRole Set IsDeleted = 1 WHERE Id = @Id",
                                    new { pmolLabourItems.Id });
                        }
                    }
                }
            }
            else
            {
                pmol = context.PMol.FirstOrDefault(p => p.Id == PmolParameter.PmolDto.Id);

                if (!isClone)
                {
                    if (PmolParameter.PmolDto.ForemanId != null)
                    {
                        if (PmolParameter.PmolDto.ForemanId != pmol.ForemanId)
                            ForemanAddToPmol(PmolParameter, connectionString, false);
                    }
                    else
                    {
                        //await using var connection = new SqlConnection(connectionString);

                        var pmolLabourItems = connection.Query<PmolTeamRole>(
                                @"SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.RoleId = 'Foreman' AND PmolTeamRole.IsDeleted = 0 ",
                                new { PmolId = PmolParameter.PmolDto.Id })
                            .FirstOrDefault();

                        if (pmolLabourItems != null)
                        {
                            await connection.ExecuteAsync("Update PmolTeamRole Set IsDeleted = 1 WHERE Id = @Id",
                                new { pmolLabourItems.Id });

                            if (PmolParameter.PmolDto.ExecutionDate != null)
                            {
                                var resParam = new PmolResourceParameter
                                {
                                    ContextAccessor = PmolParameter.ContextAccessor,
                                    ContractingUnitSequenceId = PmolParameter.ContractingUnitSequenceId,
                                    ProjectSequenceId = PmolParameter.ProjectSequenceId,
                                    Lang = PmolParameter.Lang,
                                    TenantProvider = PmolParameter.TenantProvider,
                                    RemovePersonFromPmol = new RemovePersonFromPmol()
                                    {
                                        CuConnectionString = cuConnectionString,
                                        ProjectConnectionString = connectionString,
                                        PmolId = PmolParameter.PmolDto.Id,
                                        CabPersonId = pmolLabourItems.CabPersonId,
                                        ExecutionDate = PmolParameter.PmolDto.ExecutionDate.Value
                                    }
                                };
                                await PmolParameter.IPmolResourceRepository.RemovePersonFromPmol(resParam);
                            }
                        }
                    }
                }

                if (pmol != null)
                {
                    pmol.Comment = PmolParameter.PmolDto.Comment;
                    pmol.ExecutionDate = PmolParameter.PmolDto.ExecutionDate;
                    pmol.ExecutionStartTime = PmolParameter.PmolDto.ExecutionStartTime;
                    pmol.ExecutionEndTime = PmolParameter.PmolDto.ExecutionEndTime;
                    pmol.ForemanId = PmolParameter.PmolDto.ForemanId;
                    pmol.ForemanMobileNumber = PmolParameter.PmolDto.ForemanMobileNumber;
                    pmol.Name = PmolParameter.PmolDto.Name;
                    pmol.BorId = PmolParameter.PmolDto.PmolType == "sub" ? null : PmolParameter.PmolDto.Bor.Id;
                    pmol.StatusId = pmolStatusId;
                    pmol.TypeId = PmolParameter.PmolDto.TypeId;
                    pmol.Title = pmol.ProjectMoleculeId + " - " + PmolParameter.PmolDto.Name;
                    pmol.IsFinished = PmolParameter.PmolDto.IsFinished;
                    pmol.PmolType = PmolParameter.PmolDto.PmolType;
                    pmol.PmolLotId = PmolParameter.PmolDto.PmolLotId;
                    context.PMol.Update(pmol);
                }

                context.SaveChanges();
            }

            await PmolStatusUpdate(PmolParameter, pmolStatusId, pmol.ProjectMoleculeId);

            await CreateHistory(PmolParameter, isExist);
            await AddUserRole(PmolParameter, PmolParameter.TenantProvider);
            return pmol;
        }
    }

    public async void ForemanAddToPmol(PmolParameter pmolParameter, string projectconString, bool isForeman)
    {
        string connectionString = null;
        if (isForeman)
            connectionString = projectconString;
        else
            connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var cuConnectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId, null,
            pmolParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        await using var cuConnection = new SqlConnection(cuConnectionString);
        await using var dbConnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

        var cabPersonCompanyId = dbConnection
            .Query<string>("SELECT Id FROM dbo.CabPersonCompany  WHERE PersonId = @PersonId ",
                new { PersonId = pmolParameter.PmolDto.ForemanId }).FirstOrDefault();


        var mTeamMemberDto = new List<TeamMemberDto>();

        var mTeam = new TeamMemberDto()
        {
            PersonId = pmolParameter.PmolDto.ForemanId,
            CabPersonCompanyId = cabPersonCompanyId,
            RoleId = "Foreman"
        };

        mTeamMemberDto.Add(mTeam);
        var vpParameter = new VPParameter
        {
            TenantProvider = pmolParameter.TenantProvider,
            Configuration = pmolParameter.Configuration
        };
        if (pmolParameter.PmolDto.ExecutionDate != null)
        {
            var mAddMutipleTeamMembers = new AddMutipleTeamMembers()
            {
                ProjectSequenceCode = pmolParameter.ProjectSequenceId,
                ContractingUnit = pmolParameter.ContractingUnitSequenceId,
                PmolId = pmolParameter.PmolDto.Id,
                ExecutionDate = pmolParameter.PmolDto.ExecutionDate.Value,
                Team = mTeamMemberDto
            };

            vpParameter.AddMutipleTeamMembers = mAddMutipleTeamMembers;
        }

        var insertSql =
            @"INSERT INTO dbo.PmolTeamRole ( Id ,CabPersonId ,RoleId ,RequiredQuantity ,ConsumedQuantity ,Type ,PmolLabourId ,IsDeleted ) VALUES ( @Id ,@CabPersonId ,@RoleId ,0.0 ,0.0 ,@Type ,@PmolLabourId ,0 )";

        var pmolLabourItems = connection.Query<PmolTeamRole>(
                @"SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.RoleId = 'Foreman' AND PmolTeamRole.IsDeleted = 0 AND PmolPlannedWorkLabour.IsDeleted = 0",
                new { PmolId = pmolParameter.PmolDto.Id })
            .FirstOrDefault();

        if (pmolLabourItems != null)
        {
            await connection.ExecuteAsync("Update PmolTeamRole Set IsDeleted = 1 WHERE Id = @Id",
                new { pmolLabourItems.Id });

            if (pmolParameter.PmolDto.ExecutionDate != null)
            {
                var resParam = new PmolResourceParameter
                {
                    ContextAccessor = pmolParameter.ContextAccessor,
                    ContractingUnitSequenceId = pmolParameter.ContractingUnitSequenceId,
                    ProjectSequenceId = pmolParameter.ProjectSequenceId,
                    Lang = pmolParameter.Lang,
                    TenantProvider = pmolParameter.TenantProvider,
                    Configuration = pmolParameter.Configuration,
                    RemovePersonFromPmol = new RemovePersonFromPmol()
                    {
                        CuConnectionString = cuConnectionString,
                        ProjectConnectionString = connectionString,
                        PmolId = pmolParameter.PmolDto.Id,
                        CabPersonId = pmolLabourItems.CabPersonId,
                        ExecutionDate = pmolParameter.PmolDto.ExecutionDate.Value
                    }
                };
                await pmolParameter.IPmolResourceRepository.RemovePersonFromPmol(resParam);
            }

            var teamParam = new
            {
                Id = Guid.NewGuid().ToString(),
                CabPersonId = pmolParameter.PmolDto.ForemanId,
                RoleId = "Foreman",
                Type = "Planned",
                pmolLabourItems.PmolLabourId
            };

            await connection.ExecuteAsync(insertSql, teamParam);

            if (pmolParameter.PmolDto.ExecutionDate != null)
            {
                var otherMembers = connection.Query<TeamMemberDto>(
                    "Select CabPersonId As PersonId,RoleId From PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND  PmolTeamRole.IsDeleted = 0 AND PmolPlannedWorkLabour.IsDeleted = 0 AND CabPersonId != @CabPersonId",
                    new { CabPersonId = pmolParameter.PmolDto.ForemanId, PmolId = pmolParameter.PmolDto.Id }).ToList();

                otherMembers.ForEach(x => x.CabPersonCompanyId = dbConnection
                    .Query<string>("SELECT Id FROM dbo.CabPersonCompany  WHERE PersonId = @PersonId ",
                        new { PersonId = x.PersonId }).FirstOrDefault());

                vpParameter.AddMutipleTeamMembers.Team.AddRange(otherMembers);
                await pmolParameter.IVpRepository.AddMultipleMembersToPmol(vpParameter, true);
            }
        }

        else
        {
            var labourTeamExist = connection
                .Query<PmolPlannedWorkLabour>(
                    "Select * From PmolPlannedWorkLabour Where PmolId = @PmolId AND IsDeleted = 0",
                    new { PmolId = pmolParameter.PmolDto.Id }).FirstOrDefault();

            if (labourTeamExist == null)
            {
                var labourInsertSql =
                    @"INSERT INTO dbo.PMolPlannedWorkLabour ( Id ,CoperateProductCatalogId ,RequiredQuantity ,ConsumedQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted ) VALUES ( @Id ,@CoperateProductCatalogId ,0.0 ,0.0 ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type ,0 )";

                var labourTeamParam = new
                {
                    Id = Guid.NewGuid().ToString(),
                    CoperateProductCatalogId = pmolParameter.Configuration.GetValue<string>("DefaultCpc"),
                    CpcBasicUnitofMeasureId = "cce5fd6a-91e1-4fc0-b3f6-2c462eaf2500",
                    PmolId = pmolParameter.PmolDto.Id,
                    Type = "Planned"
                };

                await connection.ExecuteAsync(labourInsertSql, labourTeamParam);

                var teamParam = new
                {
                    Id = Guid.NewGuid().ToString(),
                    CabPersonId = pmolParameter.PmolDto.ForemanId,
                    RoleId = "Foreman",
                    Type = "Planned",
                    PmolLabourId = labourTeamParam.Id
                };

                await connection.ExecuteAsync(insertSql, teamParam);

                if (pmolParameter.PmolDto.ExecutionDate != null)
                {
                    var otherMembers = connection.Query<TeamMemberDto>(
                            "Select CabPersonId As PersonId,RoleId From PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND  PmolTeamRole.IsDeleted = 0 AND PmolPlannedWorkLabour.IsDeleted = 0 AND CabPersonId != @CabPersonId",
                            new { CabPersonId = pmolParameter.PmolDto.ForemanId, PmolId = pmolParameter.PmolDto.Id })
                        .ToList();

                    otherMembers.ForEach(x => x.CabPersonCompanyId = dbConnection
                        .Query<string>("SELECT Id FROM dbo.CabPersonCompany  WHERE PersonId = @PersonId ",
                            new { PersonId = x.PersonId }).FirstOrDefault());

                    vpParameter.AddMutipleTeamMembers.Team.AddRange(otherMembers);

                    await pmolParameter.IVpRepository.AddMultipleMembersToPmol(vpParameter, true);
                }
            }
            else
            {
                var teamParam = new
                {
                    Id = Guid.NewGuid().ToString(),
                    CabPersonId = pmolParameter.PmolDto.ForemanId,
                    RoleId = "Foreman",
                    Type = "Planned",
                    PmolLabourId = labourTeamExist.Id
                };

                await connection.ExecuteAsync(insertSql, teamParam);

                if (pmolParameter.PmolDto.ExecutionDate != null)
                {
                    var otherMembers = connection.Query<TeamMemberDto>(
                            "Select CabPersonId As PersonId,RoleId From PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND  PmolTeamRole.IsDeleted = 0 AND PmolPlannedWorkLabour.IsDeleted = 0 AND CabPersonId != @CabPersonId",
                            new { CabPersonId = pmolParameter.PmolDto.ForemanId, PmolId = pmolParameter.PmolDto.Id })
                        .ToList();

                    otherMembers.ForEach(x => x.CabPersonCompanyId = dbConnection
                        .Query<string>("SELECT Id FROM dbo.CabPersonCompany  WHERE PersonId = @PersonId ",
                            new { PersonId = x.PersonId }).FirstOrDefault());

                    vpParameter.AddMutipleTeamMembers.Team.AddRange(otherMembers);

                    await pmolParameter.IVpRepository.AddMultipleMembersToPmol(vpParameter, true);
                }
            }
        }
    }

    public async Task<string> CreatePmolStartHandshake(PmolParameter pmolParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider))
        {
            var pmolObj =
                context.PMol.FirstOrDefault(p => p.Id == pmolParameter.pmolStartHandshakeCreateDto.PmolId);
            if (pmolObj != null)
            {
                pmolObj.LocationId = pmolParameter.pmolStartHandshakeCreateDto.LocationId;
                context.PMol.Update(pmolObj);
                context.SaveChanges();
                return pmolObj.Id;
            }

            return "No Pmol for the PmolId";
        }
    }

    public async Task<string> CreatePmolStopHandshake(PmolParameter pmolParameter)
    {
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, pmolParameter.TenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var idGenerator = new IdGenerator();
        var sqCode = idGenerator.GenerateId(applicationDbContext, "PMOLSH-", "PmolStopHandshake");
        var pmolStopHandshake = new PmolStopHandshake
        {
            SequenceCode = sqCode,
            Id = Guid.NewGuid().ToString()
        };
        if (pmolParameter.pmolStopHandshakeCreateDto != null)
        {
            var pmolHsDto = pmolParameter.pmolStopHandshakeCreateDto;
            pmolStopHandshake.Link = pmolHsDto.Link;
            pmolStopHandshake.Name = pmolHsDto.Name;
            pmolStopHandshake.PmolId = pmolHsDto.PmolId;
            pmolStopHandshake.CabPersonlId = pmolHsDto.CabId;
            await using (var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider))
            {
                context.PMolStopHandshake.Add(pmolStopHandshake);
                await context.SaveChangesAsync();
            }

            return pmolStopHandshake.Id;
        }

        return null;
    }

    public async Task<string> AddPmolStopHandshakeDocuments(PmolParameter pmolParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        if (pmolParameter.pmolStopHandshakeCreateDocumentsDto != null)
        {
            await using (var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider))
            {
                if (pmolParameter.pmolStopHandshakeCreateDocumentsDto != null)
                {
                    var docList = pmolParameter.pmolStopHandshakeCreateDocumentsDto.DocLinks;
                    foreach (var docLink in docList)
                    {
                        var pmolStopHandshakeDocument = new PmolStopHandshakeDocument
                        {
                            Id = Guid.NewGuid().ToString(),
                            Link = docLink,
                            PmolId = pmolParameter.pmolStopHandshakeCreateDocumentsDto.PmolId
                        };
                        context.PMolStopHandshakeDocument.Add(pmolStopHandshakeDocument);
                        await context.SaveChangesAsync();
                    }
                }
            }

            return pmolParameter.pmolStopHandshakeCreateDocumentsDto.PmolId;
        }

        return null;
    }

    public async Task<string> CreatePmolExtraWork(PmolParameter pmolParameter)
    {
        // var options1 = new DbContextOptions<ApplicationDbContext>();
        // var applicationDbContext = new ApplicationDbContext(options1, pmolParameter.TenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);
        var pmolExtraWorkDto = pmolParameter.PmolExtraWorkCreateDto;
        var extraWork = context.PMolExtraWork.Where(e => e.PmolId == pmolExtraWorkDto.PmolId).FirstOrDefault();
        string extraWorkId = null;
        if (pmolExtraWorkDto != null)
        {
            if (extraWork == null)
            {
                var pmolExtraWork = new PmolExtraWork
                {
                    Description = pmolExtraWorkDto.Description,
                    PmolId = pmolExtraWorkDto.PmolId,
                    Id = Guid.NewGuid().ToString()
                };
                context.PMolExtraWork.Add(pmolExtraWork);
                await context.SaveChangesAsync();

                extraWorkId = pmolExtraWork.Id;
            }
            else
            {
                extraWork.Description = pmolExtraWorkDto.Description;
                context.PMolExtraWork.Update(extraWork);
                await context.SaveChangesAsync();

                extraWorkId = extraWork.Id;
            }

            if (pmolExtraWorkDto.ExtraWorkFiles != null)
            {
                var files = pmolExtraWorkDto.ExtraWorkFiles;
                foreach (var file in files)
                {
                    var pmolExtraWorkFile = new PmolExtraWorkFiles
                    {
                        Link = file.Link,
                        Type = file.Type,
                        Title = file.Title,
                        PmolExtraWorkId = extraWorkId
                    };

                    if (file.Id == null)
                    {
                        pmolExtraWorkFile.Id = Guid.NewGuid().ToString();
                        context.PMolExtraWorkFiles.Add(pmolExtraWorkFile);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        pmolExtraWorkFile.Id = file.Id;
                        context.PMolExtraWorkFiles.Update(pmolExtraWorkFile);
                        await context.SaveChangesAsync();
                    }
                }
            }

            return extraWorkId;
        }

        return null;
    }

    public async Task<string> CreateLocation(PmolParameter pmolParameter, bool IsLocationEmpty)
    {
        var projectLocation = pmolParameter.Location;
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);


        if (IsLocationEmpty)
        {
            var dbConnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);
            var options1 = new DbContextOptions<ApplicationDbContext>();
            var dbContext1 =
                new ApplicationDbContext(options1, pmolParameter.TenantProvider);
            var mapLocationId = dbConnection
                .Query<string>("Select LocationId From ProjectDefinition Where SequenceCode = @SequenceCode ",
                    new { SequenceCode = pmolParameter.ProjectSequenceId }).FirstOrDefault();


            if (mapLocationId != null)
            {
                var mapLocation = dbContext1.MapLocation.Where(L => L.Id == mapLocationId).Include(m => m.Address)
                    .Include(m => m.Position).FirstOrDefault();

                projectLocation = mapLocation;
                projectLocation.Id = Guid.NewGuid().ToString();
            }
        }

        if (projectLocation != null)
        {
            var isExist = context.MapLocation.Where(x => x.Id == projectLocation.Id).Any();

            if (!isExist)
            {
                projectLocation.BoundingBoxId = null;
                projectLocation.ViewportId = null;
                projectLocation.DataSourcesId = null;
                if (projectLocation.Id == null) projectLocation.Id = Guid.NewGuid().ToString();

                if (projectLocation.Position != null)
                {
                    var position = projectLocation.Position;
                    position.Id = Guid.NewGuid().ToString();
                    context.Position.Add(position);
                    await context.SaveChangesAsync();
                }

                if (projectLocation.Viewport != null)
                {
                    var viewPort = projectLocation.Viewport;
                    if (viewPort.TopLeftPoint != null)
                    {
                        var tlp = viewPort.TopLeftPoint;
                        tlp.Id = Guid.NewGuid().ToString();
                        context.Position.Add(tlp);
                        await context.SaveChangesAsync();
                    }

                    if (viewPort.BtmRightPoint != null)
                    {
                        var brp = viewPort.BtmRightPoint;
                        brp.Id = Guid.NewGuid().ToString();
                        context.Position.Add(brp);
                        await context.SaveChangesAsync();
                    }

                    viewPort.Id = Guid.NewGuid().ToString();
                    context.BoundingPoint.Add(viewPort);
                    await context.SaveChangesAsync();
                }

                if (projectLocation.BoundingBox != null)
                {
                    var boundingBox = projectLocation.BoundingBox;
                    if (boundingBox.TopLeftPoint != null)
                    {
                        var tlp = boundingBox.TopLeftPoint;
                        tlp.Id = Guid.NewGuid().ToString();
                        context.Position.Add(tlp);
                        await context.SaveChangesAsync();
                    }

                    if (boundingBox.BtmRightPoint != null)
                    {
                        var brp = boundingBox.BtmRightPoint;
                        brp.Id = Guid.NewGuid().ToString();
                        context.Position.Add(brp);
                        await context.SaveChangesAsync();
                    }

                    boundingBox.Id = Guid.NewGuid().ToString();
                    context.BoundingPoint.Add(boundingBox);
                    await context.SaveChangesAsync();
                }

                if (projectLocation.Address != null)
                {
                    var address = projectLocation.Address;
                    address.Id = Guid.NewGuid().ToString();
                    context.Address.Add(address);
                    await context.SaveChangesAsync();
                }

                if (projectLocation.DataSources != null)
                {
                    var dataSource = projectLocation.DataSources;
                    if (dataSource.Geometry != null)
                    {
                        var geometry = dataSource.Geometry;
                        //if (geometry.Id == null)
                        //{
                        geometry.Id = Guid.NewGuid().ToString();
                        //}
                        context.Geometry.Add(geometry);
                        await context.SaveChangesAsync();
                    }

                    dataSource.Id = Guid.NewGuid().ToString();
                    context.DataSources.Add(dataSource);
                    await context.SaveChangesAsync();
                }

                context.MapLocation.Add(projectLocation);
                await context.SaveChangesAsync();
                return projectLocation.Id;
            }
        }
        else
        {
            return null;
        }

        return projectLocation.Id;
    }

    public async Task<PmolStopHandshakeReadDto> GetStopHandShakesByPmolId(PmolParameter pmolParameter)
    {
        var query =
            @"SELECT PMolStopHandshake.Name AS Name,PMolStopHandshake.Link AS Link,PMolStopHandshake.PmolId AS PmolId
                FROM dbo.PMolStopHandshake WHERE PMolStopHandshake.PmolId = @PmolId; SELECT PMolStopHandshakeDocument.Link AS Link FROM dbo.PMolStopHandshakeDocument WHERE PMolStopHandshakeDocument.PmolId = @PmolId;";


        var param = new { PmolId = pmolParameter.Id };

        var pmolStopHandshakeReadDto = new PmolStopHandshakeReadDto();

        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            using (var multi = dbConnection.QueryMultiple(query, param))
            {
                pmolStopHandshakeReadDto.StopHandshakes = multi.Read<PmolStopHandshakeReadObj>().ToList();

                pmolStopHandshakeReadDto.DocLinks = multi.Read<string>().ToList();
            }
        }

        return pmolStopHandshakeReadDto;
    }

    public async Task<MapLocation> GetLocationByPmolId(PmolParameter pmolParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);

        var pmol = context.PMol.FirstOrDefault(p => p.Id == pmolParameter.Id);
        if (pmol != null)
        {
            var mapLocation = context.MapLocation.Where(L => L.Id == pmol.LocationId).Include(m => m.Address)
                .Include(m => m.Position).ToList().FirstOrDefault();
            if (mapLocation == null)
            {
                var projectDef =
                    context.ProjectDefinition.FirstOrDefault(p =>
                        p.SequenceCode == pmolParameter.ProjectSequenceId);
                if (projectDef != null)
                    mapLocation = context.MapLocation.Where(L => L.Id == projectDef.LocationId)
                        .Include(m => m.Address).Include(m => m.Position).ToList().FirstOrDefault();
            }

            return mapLocation;
        }

        return null;
    }

    public async Task<bool> DeletePmolExtraWork(PmolParameter pmolParameter)
    {
        DeleteData(pmolParameter, "PMolExtraWork");
        DeleteData(pmolParameter, "PMolExtraWorkFiles");
        return true;
    }


    public IEnumerable<ProjectWithPm> ProjectPm(string connection)
    {
        var projectWithPm = @"SELECT
                                                   ProjectTeam.ProjectId
                                                 ,ProjectTeam.ContractingUnitId
                                                 ,ProjectTeamRole.CabPersonId
                                                 ,ProjectDefinition.SequenceCode
                                                 ,ProjectDefinition.Name
                                                 ,CabPerson.FullName
                                                 ,CabPerson.Id As CabPersonId
                                                 ,ProjectDefinition.ProjectConnectionString
                                                 ,ProjectDefinition.Title AS ProjectTitle
                                                FROM dbo.ProjectTeamRole
                                                INNER JOIN dbo.ProjectTeam
                                                  ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                                                INNER JOIN dbo.ProjectDefinition
                                                  ON ProjectTeam.ProjectId = ProjectDefinition.Id
                                                INNER JOIN dbo.CabPerson
                                                  ON ProjectTeamRole.CabPersonId = CabPerson.Id
                                                WHERE ProjectDefinition.IsDeleted = 0 AND ProjectTeamRole.RoleId IN ('1666e217-2b80-4acd-b48b-b041fe263fb9', '476127cb-62db-4af7-ac8e-d4a722f8e142','266a5f47-3489-484b-8dae-e4468c5329dn3')";

        IEnumerable<ProjectWithPm> projectWithPmList = null;
        using var connectionDb = new SqlConnection(connection);

        connectionDb.Open();
        projectWithPmList = connectionDb.Query<ProjectWithPm>(projectWithPm);
        return projectWithPmList;
    }

    public async Task<IEnumerable<ProjectWithPm>> UpdateProjectPm(PmolParameter pmolParameter)
    {
        var projectWithPmList = ProjectPm(pmolParameter.TenantProvider.GetTenant().ConnectionString);


        foreach (var project in projectWithPmList)
            if (project != null && project.ProjectConnectionString != null)
            {
                await using var connection =
                    new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

                var updateQuery =
                    "UPDATE dbo.ProjectDefinition SET ProjectManagerId = @ProjectManagerId WHERE Id = @Id ";

                await connection.ExecuteAsync(updateQuery,
                    new { ProjectManagerId = project.CabPersonId, Id = project.ProjectId });

                var query =
                    @" select ProjectDefinition.Id, ProjectTime.EndDate, CabPerson.FullName AS ContactPersonName,
                                    CabEmail.EmailAddress AS ContactPersonEmail , CabCompany.Name AS CustomerName
                                    from ProjectDefinition
                                    left outer join  ProjectTime on ProjectDefinition.Id=ProjectTime.ProjectId
                                    left outer join  ProjectTeam on ProjectDefinition.Id = ProjectTeam.ProjectId
                                    left outer join  ProjectTeamRole on ProjectTeam.Id = ProjectTeamRole.ProjectTeamId
                                    left outer join  CabPerson on ProjectTeamRole.CabPersonId = CabPerson.Id
                                    left outer join CabPersonCompany on CabPerson.Id = CabPersonCompany.PersonId
                                    left outer join CabCompany on CabCompany.Id = CabPersonCompany.CompanyId
                                    left outer join CabEmail on CabPersonCompany.EmailId = CabEmail.Id
                                    where ProjectDefinition.SequenceCode = @id
                                    AND RoleId = '910b7af0-b132-4951-a2dc-6ab82d4cd40d';
                                    ";
                var param = new { id = project.SequenceCode };
            }

        return projectWithPmList;
    }


    public async Task<IEnumerable<PmolReport>> ReadPmolProjectsPMWithDetail(PmolParameter pmolParameter)
    {
        var projectWithPmList = ProjectPm(pmolParameter.TenantProvider.GetTenant().ConnectionString);
        var data = new List<PmolReport>();

        foreach (var project in projectWithPmList)
            if (project != null && project.ProjectConnectionString != null)
            {
                var query = @"SELECT
  PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS PMolTypeType
 ,PMolStatus.Name AS Status
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,CorporateProductCatalog.ResourceTitle
 ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU
 ,'Consumable' AS ResourceType
 ,PMolPlannedWorkConsumable.RequiredQuantity
 ,PMolPlannedWorkConsumable.ConsumedQuantity
 ,PMolPlannedWorkConsumable.Type AS Type
 ,PMol.Title AS PMolTitle
 ,Bor.Title AS BorTitle
 ,PbsProduct.Title AS PbsProductTitle
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
INNER JOIN dbo.PMolPlannedWorkConsumable
  ON PMolPlannedWorkConsumable.PmolId = PMol.Id
INNER JOIN dbo.CorporateProductCatalog
  ON PMolPlannedWorkConsumable.CoperateProductCatalogId = CorporateProductCatalog.Id
LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
  ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = CorporateProductCatalog.CpcBasicUnitOfMeasureId
INNER JOIN dbo.PbsProduct
  ON Bor.PbsProductId = PbsProduct.Id
WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)
AND (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0 UNION SELECT
  PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS PMolTypeType
 ,PMolStatus.Name AS Status
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,CorporateProductCatalog.ResourceTitle
 ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU
 ,'Labour' AS ResourceType
 ,PMolPlannedWorkLabour.RequiredQuantity
 ,PMolPlannedWorkLabour.ConsumedQuantity
 ,PMolPlannedWorkLabour.Type AS Type
 ,PMol.Title AS PMolTitle
 ,Bor.Title AS BorTitle
 ,PbsProduct.Title AS PbsProductTitle
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
INNER JOIN dbo.PMolPlannedWorkLabour
  ON PMolPlannedWorkLabour.PmolId = PMol.Id
INNER JOIN dbo.CorporateProductCatalog
  ON PMolPlannedWorkLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
  ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = CorporateProductCatalog.CpcBasicUnitOfMeasureId
INNER JOIN dbo.PbsProduct
  ON Bor.PbsProductId = PbsProduct.Id
WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)
AND (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0 UNION SELECT
  PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS PMolTypeType
 ,PMolStatus.Name AS Status
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,CorporateProductCatalog.ResourceTitle
 ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU
 ,'Tools' AS ResourceType
 ,PMolPlannedWorkTools.RequiredQuantity
 ,PMolPlannedWorkTools.ConsumedQuantity
 ,PMolPlannedWorkTools.Type AS Type
 ,PMol.Title AS PMolTitle
 ,Bor.Title AS BorTitle
 ,PbsProduct.Title AS PbsProductTitle
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
INNER JOIN dbo.PMolPlannedWorkTools
  ON PMolPlannedWorkTools.PmolId = PMol.Id
INNER JOIN dbo.CorporateProductCatalog
  ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id
LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
  ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = CorporateProductCatalog.CpcBasicUnitOfMeasureId
INNER JOIN dbo.PbsProduct
  ON Bor.PbsProductId = PbsProduct.Id
WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)
AND (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0 UNION SELECT
  PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS PMolTypeType
 ,PMolStatus.Name AS Status
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,CorporateProductCatalog.ResourceTitle
 ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU
 ,'Material' AS ResourceType
 ,PMolPlannedWorkMaterial.RequiredQuantity
 ,PMolPlannedWorkMaterial.ConsumedQuantity
 ,PMolPlannedWorkMaterial.Type AS Type
 ,PMol.Title AS PMolTitle
 ,Bor.Title AS BorTitle
 ,PbsProduct.Title AS PbsProductTitle
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
INNER JOIN dbo.PMolPlannedWorkMaterial
  ON PMolPlannedWorkMaterial.PmolId = PMol.Id
INNER JOIN dbo.CorporateProductCatalog
  ON PMolPlannedWorkMaterial.CoperateProductCatalogId = CorporateProductCatalog.Id
LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
  ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = CorporateProductCatalog.CpcBasicUnitOfMeasureId
INNER JOIN dbo.PbsProduct
  ON Bor.PbsProductId = PbsProduct.Id
WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)
AND (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0";
                var sb = new StringBuilder(query);

                var connectionString = project.ProjectConnectionString;

                await using (var connection = new SqlConnection(connectionString))
                {
                    var PmolReportList = connection
                        .Query<PmolReport>(sb.ToString(), new { lang = pmolParameter.Lang }).ToList();
                    foreach (var pr in PmolReportList)
                    {
                        pr.ProjectManagerName = project.FullName;
                        pr.ProjectTitle = project.ProjectTitle;
                    }

                    data.AddRange(PmolReportList);
                }
            }

        return data;
    }

    public async Task<IEnumerable<ProjectWithPm>> ReadPmolProjectsPM(PmolParameter pmolParameter)
    {
        var projectWithPmList = ProjectPm(pmolParameter.TenantProvider.GetTenant().ConnectionString);


        foreach (var project in projectWithPmList)
            if (project != null && project.ProjectConnectionString != null)
            {
                var query =
                    @"SELECT PMol.Id, PMol.ProjectMoleculeId, PMol.Name, CONCAT(PMol.ProjectMoleculeId,' ', PMol.Name) AS Title
                                    ,PMol.ExecutionDate, PMolType.Name AS Type, PMolStatus.Name AS Status, PMolType.TypeId
                                    ,PMolStatus.StatusId, PMol.ForemanId AS ForemanId
                                    ,PMolType.Type AS TypeNo, PMol.StartDateTime, PMol.EndDateTime
                                    ,Bor.PbsProductId AS PbsId
                                    FROM dbo.PMol LEFT OUTER JOIN dbo.PMolType ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolStatus ON PMol.StatusId = PMolStatus.StatusId
                                    LEFT OUTER JOIN dbo.Bor ON PMol.BorId = Bor.Id
                                    WHERE (PMolType.LanguageCode = @lang OR PMol.TypeId IS NULL)
                                    AND (PMolStatus.LanguageCode = @lang  OR PMol.StatusId IS NULL) AND PMol.IsDeleted = 0";

                var querySql =
                    @"SELECT  con.Type, cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkConsumable con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND  con.IsDeleted=0;

                                SELECT con.Type,  cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkMaterial con
                                INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId IS NULL)
                                AND con.PmolId = @id AND  con.IsDeleted=0;

                                SELECT con.Type, cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                               FROM PMolPlannedWorkLabour con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                               LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                               WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                               AND con.PmolId = @id  AND con.IsDeleted=0;

                               SELECT con.Type,  cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId ,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkTools con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.IsDeleted=0";
                var sb = new StringBuilder(query);

                var connectionString = project.ProjectConnectionString;
                IEnumerable<PmolListDtoForMobile> data = null;
                await using (var connection = new SqlConnection(connectionString))
                {
                    project.Pmols = connection
                        .Query<PmolListDtoForMobile>(sb.ToString(), new { lang = pmolParameter.Lang }).ToList();
                }
            }

        return projectWithPmList;
    }


    public async Task<IEnumerable<ProjectWithPm>> ReadBorByProjectsPM(PmolParameter pmolParameter)
    {
        var projectWithPmList = ProjectPm(pmolParameter.TenantProvider.GetTenant().ConnectionString);
        foreach (var project in projectWithPmList)
            if (project != null && project.ProjectConnectionString != null)
            {
                var query =
                    @"SELECT Bor.Id AS Id, Bor.Id AS [Key], Bor.ItemId, Bor.Title AS BorTitle, Bor.Name,dbo.PbsProduct.Id AS PbsId, PbsProduct.ProductId
                                ,PbsProduct.Name Product, PbsProduct_1.Name AS LocationParent, PbsProduct_2.Name AS UtilityParent
                                FROM dbo.Bor
                                LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_1 ON Bor.LocationParentId = PbsProduct_1.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_2 ON Bor.UtilityParentId = PbsProduct_2.Id 
                                WHERE Bor.IsDeleted = 0 ";
                var sb = new StringBuilder(query);

                var connectionString = project.ProjectConnectionString;
                IEnumerable<BorListDto> data = null;
                await using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    project.BorList = connection
                        .Query<BorListDto>(sb.ToString(), new { lang = pmolParameter.Lang }).ToList();
                }
            }

        return projectWithPmList;
    }


    public async Task<IEnumerable<ProjectWithPm>> ReadPbsByProjectsPM(PmolParameter pmolParameter)
    {
        var projectWithPmList = ProjectPm(pmolParameter.TenantProvider.GetTenant().ConnectionString);


        foreach (var project in projectWithPmList)
            if (project != null && project.ProjectConnectionString != null)
            {
                var query = @"SELECT
                                  PbsProductItemTypeLocalizedData.Label AS PbsProductItemType
                                 ,PbsProduct.ProductId
                                 ,PbsProduct.Title AS Title
                                 ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState
                                 ,PbsProductStatusLocalizedData.Label AS PbsProductStatus
                                 ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId

                                FROM dbo.PbsProduct
                                LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData
                                  ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId
                                LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData
                                  ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId
                                LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData
                                  ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId
                                LEFT OUTER JOIN PbsTaxonomyLevelLocalizedData
                                  ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId
                                WHERE (PbsProductItemTypeLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsProductItemTypeId IS NULL)
                                AND (PbsToleranceStateLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsToleranceStateId IS NULL)
                                AND (PbsProductStatusLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsProductStatusId IS NULL)
                                AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsTaxonomyLevelId IS NULL)
                                AND IsDeleted = 0
                                AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'P'";
                var sb = new StringBuilder(query);

                var connectionString = project.ProjectConnectionString;
                IEnumerable<BorListDto> data = null;
                await using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    project.PbsList = connection.Query<Pbs>(sb.ToString(), new { lang = pmolParameter.Lang })
                        .ToList();
                }
            }

        return projectWithPmList;
    }


    public async Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserId(PmolParameter pmolParameter)
    {
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext1 = new ApplicationDbContext(options1, pmolParameter.TenantProvider);
        var personCompany =
            applicationDbContext1.CabPersonCompany.FirstOrDefault(p => p.Oid.Equals(pmolParameter.UserId));
        var foreman = applicationDbContext1.CabPerson.Where(p => p.Id == personCompany.PersonId).FirstOrDefault();

        if (personCompany != null)
        {
            var pmolListDtos = new List<PmolListDtoForMobile>();
            var projectConnectionList =
                GetProjectConnectionsByUser(personCompany.Oid, pmolParameter.TenantProvider);
            foreach (var projectCon in projectConnectionList)
                if (projectCon != null && projectCon.ProjectConnectionString != null && projectCon.Id != null)
                {
//                     var query = @"SELECT
//   PMol.Id
//  ,PMol.ProjectMoleculeId
//  ,PMol.Name
//  ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
//  ,PMol.ExecutionDate
//  ,PMolType.Name AS Type
//  ,PMolStatus.Name AS Status
//  ,PMolType.TypeId
//  ,PMolStatus.StatusId
//  ,PMol.ForemanId AS ForemanId
//  ,PMolType.Type AS TypeNo
//  ,PMol.StartDateTime
//  ,PMol.EndDateTime
//  ,Bor.PbsProductId AS PbsId
//  ,PMol.IsBreak
// FROM dbo.PMol
// LEFT OUTER JOIN dbo.PMolType
//   ON PMol.TypeId = PMolType.TypeId
// LEFT OUTER JOIN dbo.PMolStatus
//   ON PMol.StatusId = PMolStatus.StatusId
// LEFT OUTER JOIN dbo.Bor
//   ON PMol.BorId = Bor.Id
// WHERE (PMolType.LanguageCode = @lang
// OR PMol.TypeId IS NULL)
// AND (PMolStatus.LanguageCode = @lang
// OR PMol.StatusId IS NULL)
// AND PMol.IsDeleted = 0
// AND PMol.IsFinished = 0
// AND PMol.ForemanId = @ForemanId";

                    var query = @"SELECT
  PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS Type
 ,PMolStatus.Name AS Status
 ,PMolType.TypeId
 ,PMolStatus.StatusId
 ,PMol.ForemanId AS ForemanId
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,Bor.PbsProductId AS PbsId
 ,PMol.IsBreak
 ,PMol.IsFinished
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
  LEFT OUTER JOIN dbo.PMolPlannedWorkLabour 
  ON PMol.Id = PMolPlannedWorkLabour.PmolId
  LEFT OUTER JOIN dbo.PmolTeamRole  
  ON PMolPlannedWorkLabour.Id = PmolTeamRole.PmolLabourId
WHERE (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0
AND PMol.IsFinished = 0
  AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 AND PmolTeamRole.CabPersonId = @ForemanId";
                    var sb = new StringBuilder(query);

                    var connectionString = projectCon.ProjectConnectionString;
                    var parameters = new { lang = pmolParameter.Lang, ForemanId = personCompany.PersonId };
                    IEnumerable<PmolListDtoForMobile> data = null;
                    var orderDictionary = new Dictionary<string, PmolListDtoForMobile>();

                    await using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        data = connection.Query<PmolListDtoForMobile>(sb.ToString(), parameters);
                    }


                    if (data != null && data.Any())
                    {
                        foreach (var dto in data)
                        {
                            dto.ProjectId = projectCon.Id;
                            var projectDefinitionMobDto = new ProjectDefinitionMobDto();
                            projectDefinitionMobDto.CuId = projectCon.CuId;
                            projectDefinitionMobDto.SequenceCode = projectCon.SequenceCode;
                            projectDefinitionMobDto.Title = projectCon.SequenceCode + " - " + projectCon.Name;
                            dto.ProjectDefinition = projectDefinitionMobDto;
                            dto.Foreman = foreman.FullName;

                            await using (var connection = new SqlConnection(connectionString))
                            {
                                var isLabourStart = connection.Query<PmolLabourTime>(
                                    "SELECT PmolLabourTime.* FROM PmolLabourTime LEFT OUTER JOIN PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 AND PmolTeamRole.CabPersonId = @personId AND PmolLabourTime.Type = '8' ",
                                    new { PmolId = dto.Id, personId = personCompany.PersonId }).Any();
                                dto.IsEnded = isLabourStart;
                            }

                            dto.IsForeman = dto.ForemanId == personCompany.PersonId;
                        }

                        pmolListDtos.AddRange(data);
                    }
                }

            var pmolListDtosX = new List<PmolListDtoForMobile>(); // please dont change
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime != null)
                .OrderBy(s => s.StartDateTime));
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime == null));
            return pmolListDtosX;
        }

        return null;
    }

    public async Task<PmolExtraWorkReadDto> GetExtraWorkByPmolId(PmolParameter pmolParameter)
    {
        var query = @"SELECT
                                  PMolExtraWork.Id
                                 ,PMolExtraWork.Description
                                 ,PMolExtraWork.PmolId
                                FROM dbo.PMolExtraWork
                                WHERE PMolExtraWork.PmolId = '"
                    + pmolParameter.Id + "'";

        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        PmolExtraWorkReadDto result;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            result = dbConnection.Query<PmolExtraWorkReadDto>(query).FirstOrDefault();
        }

        if (result != null)
        {
            var query1 = @"SELECT
                                      PMolExtraWorkFiles.Link
                                     ,PMolExtraWorkFiles.Title
                                        ,PMolExtraWorkFiles.Id
                                        ,PMolExtraWorkFiles.Type
                                        ,PMolExtraWorkFiles.Length
                                        ,PMolExtraWorkFiles.PmolExtraWorkId
                                    FROM dbo.PMolExtraWorkFiles
                                    WHERE PMolExtraWorkFiles.PmolExtraWorkId = '" + result.Id
                + "' AND PMolExtraWorkFiles.Type = '2'";

            var connectionString1 = ConnectionString.MapConnectionString(
                pmolParameter.ContractingUnitSequenceId, pmolParameter.ProjectSequenceId,
                pmolParameter.TenantProvider);
            List<PmolExtraWorkFilesReadDto> audioList = null;
            using (var dbConnection = new SqlConnection(connectionString))
            {
                audioList = dbConnection.Query<PmolExtraWorkFilesReadDto>(query1).ToList();
                
                result.AudioList = audioList;
            }

            var query2 = @"SELECT
                                      PMolExtraWorkFiles.Link
                                     ,PMolExtraWorkFiles.Title
                                    ,PMolExtraWorkFiles.Id, PMolExtraWorkFiles.Type
                                    FROM dbo.PMolExtraWorkFiles
                                    WHERE PMolExtraWorkFiles.PmolExtraWorkId = '" + result.Id
                + "' AND PMolExtraWorkFiles.Type = '1'";

            List<PmolExtraWorkFilesReadDto> imageList = null;
            using (var dbConnection = new SqlConnection(connectionString1))
            {
                imageList = dbConnection.Query<PmolExtraWorkFilesReadDto>(query2).ToList();
                
                result.ImageList = imageList;
            }

            return result;
        }

        return null;
    }

    public async Task<string> ClonePmol(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var pmol = await GetPmolById(pmolParameter);

        var resourceParameter = new PmolResourceParameter();
        resourceParameter.ContractingUnitSequenceId = pmolParameter.ContractingUnitSequenceId;
        resourceParameter.Id = pmol.Id;
        resourceParameter.Lang = pmolParameter.Lang;
        resourceParameter.ProjectSequenceId = pmolParameter.ProjectSequenceId;
        resourceParameter.TenantProvider = pmolParameter.TenantProvider;
        var consumable = await pmolParameter.IPmolResourceRepository.ReadConsumable(resourceParameter);
        var material = await pmolParameter.IPmolResourceRepository.ReadMaterial(resourceParameter);
        var labour = await pmolParameter.IPmolResourceRepository.ReadLabour(resourceParameter);
        var tools = await pmolParameter.IPmolResourceRepository.ReadTools(resourceParameter);

        var pmolCreateDto = new PmolCreateDto();
        pmolCreateDto.Comment = pmol.Comment;
        //pmolCreateDto.ExecutionDate = pmol.ExecutionDate;
        pmolCreateDto.ForemanId = pmol.ForemanId;
        pmolCreateDto.ForemanMobileNumber = pmol.ForemanMobileNumber;
        pmolCreateDto.Id = Guid.NewGuid().ToString();
        pmolCreateDto.Name = pmol.Name;
        var borGetByIdDto = new BorGetByIdDto();
        if (pmol.PmolType != "sub") borGetByIdDto.Id = pmol.Bor.Id;
        pmolCreateDto.Bor = borGetByIdDto;

        pmolCreateDto.StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477";
        pmolCreateDto.TypeId = pmol.TypeId;
        pmolCreateDto.PmolType = pmol.PmolType;
        pmolCreateDto.ProductId = pmol.ProductId;
        pmolCreateDto.ParentId = pmol.Id;
        pmolParameter.PmolDto = pmolCreateDto;
        pmolCreateDto.LocationId = pmol.LocationId;

        var clonedPmolId = await CreateHeader(pmolParameter, true);

        if (pmol.PmolType == "sub")
        {
            var param = new PmolParameter();
            var serviceCreate = new PmolServiceCreateDto();
            param.Id = pmol.Id;
            param.ContractingUnitSequenceId = pmolParameter.ContractingUnitSequenceId;
            param.ProjectSequenceId = pmolParameter.ProjectSequenceId;
            param.TenantProvider = pmolParameter.TenantProvider;
            var service = await ReadPmolServiceByPmolId(param);
            if (service != null)
            {
                serviceCreate.PmolId = pmolCreateDto.Id;
                serviceCreate.Mou = service.Mou;
                serviceCreate.Quantity = service.Quantity;
                serviceCreate.TotalPrice = service.TotalPrice;
                serviceCreate.UnitPrice = service.UnitPrice;
                serviceCreate.Comments = service.Comments;
                serviceCreate.Documents = service.Documents;
                param.PmolServiceCreate = serviceCreate;
                await CreatePmolService(param);
            }
        }

        foreach (var con in consumable)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            await pmolParameter.IPmolResourceRepository.CreateConsumable(resourceParameter);
        }

        foreach (var con in material)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            await pmolParameter.IPmolResourceRepository.CreateMaterial(resourceParameter);
        }

        foreach (var con in labour)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            var Id = await pmolParameter.IPmolResourceRepository.CreateLabour(resourceParameter);

            var addTeamMember =
                @"INSERT INTO dbo.PmolTeamRole ( Id ,CabPersonId ,RoleId ,Type ,PmolLabourId ) VALUES ( @Id ,@CabPersonId ,@RoleId ,@Type ,@PmolLabourId );";
            using (var connection =
                   new SqlConnection(connectionString))
            {
                var team = connection
                    .Query<PmolTeamRole>("SELECT * FROM dbo.PmolTeamRole WHERE PmolLabourId = @Id AND IsDeleted = 0",
                        new { con.Id }).ToList();

                foreach (var i in team)
                {
                    var param2 = new
                    {
                        Id = Guid.NewGuid(),
                        i.CabPersonId,
                        i.RoleId,
                        i.Type,
                        PmolLabourId = Id
                    };
                    await connection.ExecuteAsync(addTeamMember, param2);
                }
            }
        }

        foreach (var con in tools)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            await pmolParameter.IPmolResourceRepository.CreateTools(resourceParameter);
        }

        return pmolCreateDto.Id;
    }

    public async Task<string> ClonePmolDayPlanning(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.DayPlanPmolClone.ContractingUinit,
            pmolParameter.DayPlanPmolClone.ProjectSequenceCode, pmolParameter.TenantProvider);

        await using var projectConnection = new SqlConnection(connectionString);

        var pmol = await GetPmolById(pmolParameter);

        var dateTime = DateTime.ParseExact(pmol.ExecutionEndTime, "HH:mm",
            CultureInfo.InvariantCulture);
        var resourceParameter = new PmolResourceParameter();
        resourceParameter.ContractingUnitSequenceId = pmolParameter.ContractingUnitSequenceId;
        resourceParameter.Id = pmol.Id;
        resourceParameter.Lang = pmolParameter.Lang;
        resourceParameter.ProjectSequenceId = pmolParameter.ProjectSequenceId;
        resourceParameter.TenantProvider = pmolParameter.TenantProvider;
        resourceParameter.VpRepository = pmolParameter.IVpRepository;
        var consumable = await pmolParameter.IPmolResourceRepository.ReadConsumable(resourceParameter);
        var material = await pmolParameter.IPmolResourceRepository.ReadMaterial(resourceParameter);
        var labour = await pmolParameter.IPmolResourceRepository.ReadLabour(resourceParameter);
        var tools = await pmolParameter.IPmolResourceRepository.ReadToolsForDayPlanning(resourceParameter);

        var pmolCreateDto = new PmolCreateDto();
        pmolCreateDto.Comment = pmol.Comment;
        pmolCreateDto.ExecutionDate = pmol.ExecutionDate;
        pmolCreateDto.ForemanId = pmol.ForemanId;
        pmolCreateDto.ForemanMobileNumber = pmol.ForemanMobileNumber;
        pmolCreateDto.Id = Guid.NewGuid().ToString();
        pmolCreateDto.Name = pmol.Name;
        var borGetByIdDto = new BorGetByIdDto();
        if (pmol.PmolType != "sub") borGetByIdDto.Id = pmol.Bor.Id;
        pmolCreateDto.Bor = borGetByIdDto;

        pmolCreateDto.StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477";
        pmolCreateDto.TypeId = pmolParameter.DayPlanPmolClone.TypeId;
        pmolCreateDto.PmolType = pmol.PmolType;
        pmolCreateDto.ProductId = pmol.ProductId;
        pmolCreateDto.ParentId = pmol.Id;
        pmolParameter.PmolDto = pmolCreateDto;
        pmolCreateDto.LocationId = pmol.LocationId;
        pmolCreateDto.ExecutionStartTime = pmol.ExecutionEndTime;

        var hqLat = pmolParameter.Configuration.GetValue<string>("HQLat").ToDouble();
        var hqLon = pmolParameter.Configuration.GetValue<string>("HQLong").ToDouble();

        using (var connection =
               new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var mapLocation = projectConnection
                .Query<Position>(
                    "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                    new { Id = pmol.LocationId }).FirstOrDefault();
            if (mapLocation != null)
            {
                // var duration = pmolParameter.IShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(),
                //     mapLocation.Lon.ToDouble(),
                //     50.93654767100221, 3.1299880230856334, pmolParameter.TenantProvider, pmolParameter.Configuration);
                var duration = pmolParameter.IShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(),
                    mapLocation.Lon.ToDouble(),
                    hqLat, hqLon, pmolParameter.TenantProvider, pmolParameter.Configuration,
                    false);

                if (await duration != 0)
                {
                    var endTime = dateTime.AddSeconds(await duration);
                    pmolCreateDto.ExecutionEndTime = endTime.ToString("HH:mm");
                }
                else
                {
                    var time = dateTime.AddHours(2);
                    pmolCreateDto.ExecutionEndTime = time.ToString("HH:mm");
                }
            }
            else
            {
                var time = dateTime.AddHours(2);
                pmolCreateDto.ExecutionEndTime = time.ToString("HH:mm");
            }
        }

        var clonedPmolId = await CreateHeader(pmolParameter, true);

        if (pmol.PmolType == "sub")
        {
            var param = new PmolParameter();
            var serviceCreate = new PmolServiceCreateDto();
            param.Id = pmol.Id;
            param.ContractingUnitSequenceId = pmolParameter.ContractingUnitSequenceId;
            param.ProjectSequenceId = pmolParameter.ProjectSequenceId;
            param.TenantProvider = pmolParameter.TenantProvider;
            var service = await ReadPmolServiceByPmolId(param);
            if (service != null)
            {
                serviceCreate.PmolId = pmolCreateDto.Id;
                serviceCreate.Mou = service.Mou;
                serviceCreate.Quantity = service.Quantity;
                serviceCreate.TotalPrice = service.TotalPrice;
                serviceCreate.UnitPrice = service.UnitPrice;
                serviceCreate.Comments = service.Comments;
                serviceCreate.Documents = service.Documents;
                param.PmolServiceCreate = serviceCreate;
                await CreatePmolService(param);
            }
        }

        foreach (var con in consumable)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            await pmolParameter.IPmolResourceRepository.CreateConsumable(resourceParameter);
        }

        foreach (var con in material)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            await pmolParameter.IPmolResourceRepository.CreateMaterial(resourceParameter);
        }

        var addTeamMember =
            @"INSERT INTO dbo.PmolTeamRole ( Id ,CabPersonId ,RoleId ,Type ,PmolLabourId ) VALUES ( @Id ,@CabPersonId ,@RoleId ,@Type ,@PmolLabourId );";
        foreach (var con in labour)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            resourceParameter.VpRepository = pmolParameter.IVpRepository;
            var Id = await pmolParameter.IPmolResourceRepository.CreateLabour(resourceParameter);

            using (var connection =
                   new SqlConnection(connectionString))
            {
                var team = connection
                    .Query<PmolTeamRole>("SELECT * FROM dbo.PmolTeamRole WHERE PmolLabourId = @Id",
                        new { con.Id }).ToList();

                foreach (var i in team)
                {
                    var param2 = new
                    {
                        Id = Guid.NewGuid(),
                        i.CabPersonId,
                        i.RoleId,
                        i.Type,
                        PmolLabourId = Id
                    };
                    await connection.ExecuteAsync(addTeamMember, param2);
                }
            }
        }

        foreach (var con in tools)
        {
            var dto = new PmolResourceCreateDto();

            dto.CorporateProductCatalogId = con.CorporateProductCatalogId;
            dto.CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId;
            dto.Environment = "local";
            dto.Id = null;
            dto.PmolId = pmolCreateDto.Id;
            dto.Required = con.Required;
            dto.ResourceNumber = con.ResourceNumber;
            dto.Type = "Planned";
            resourceParameter.ResourceCreateDto = dto;
            await pmolParameter.IPmolResourceRepository.CreateTools(resourceParameter);
        }

        using (var connection =
               new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var mOrganizationTeamPmol = connection
                .Query<OrganizationTeamPmol>("SELECT * FROM dbo.OrganizationTeamPmol WHERE PmolId = @Id",
                    new { pmol.Id }).FirstOrDefault();
            if (mOrganizationTeamPmol != null)
            {
                var insert =
                    @"INSERT INTO OrganizationTeamPmol ( Id ,OrganizationTeamId ,PmolId ,ExecutionDate ,StartTime ,EndTime ,ContractingUnit ,Project ) VALUES ( @Id ,@OrganizationTeamId ,@PmolId ,@ExecutionDate ,@StartTime ,@EndTime ,@ContractingUnit ,@Project)";

                var param = new
                {
                    Id = Guid.NewGuid(),
                    mOrganizationTeamPmol.OrganizationTeamId,
                    PmolId = pmolCreateDto.Id,
                    pmol.ExecutionDate,
                    StartTime = pmol.ExecutionStartTime,
                    EndTime = pmol.ExecutionEndTime,
                    mOrganizationTeamPmol.ContractingUnit,
                    mOrganizationTeamPmol.Project
                };
                await connection.QueryAsync(insert, param);
            }
        }

        return pmolCreateDto.Id;
    }

    public async Task<bool> DeletePmolExtraWorkFiles(PmolParameter pmolParameter)
    {
        var state = false;
        // var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        // var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);

        foreach (var id in pmolParameter.IdList)
        {
            var query = "delete from PMolStopHandshakeDocument where id = '" + id + "'";
            await using (var connection = new SqlConnection(connectionString))
            {
                var affectedRows = connection.ExecuteAsync(query);
                connection.Close();
            }

            state = true;
        }

        return state;
    }

    public async Task<string> UpdateStartTime(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var shanukaDbContext = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);
        string pmolStatusId = null;
        var pmolStatus = shanukaDbContext.PMolStatus.FirstOrDefault(p => p.Name.Equals("In Development"));
        if (pmolStatus != null) pmolStatusId = pmolStatus.StatusId;

        var date = DateTime.UtcNow;
        var query = "update PMol set StartDateTime = @Date, StatusId = @PmolStatusId , IsBreak = 0 where Id = @Id";
        var param = new { pmolParameter.Id, Date = date, PmolStatusId = pmolStatusId };
        await using (var connection = new SqlConnection(connectionString))
        {
            var affectedRows = connection.QuerySingleOrDefaultAsync(query, param).Result;
            connection.Close();
        }

        return pmolParameter.Id;
    }

    public async Task<string> UpdateEndTime(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var date = DateTime.UtcNow;
        var query = "update PMol set EndDateTime = @Date, IsBreak = 0 where Id = @Id";
        var queryTime = @"SELECT
  CONVERT(TIME, CONVERT(DATETIME, EndDateTime) - CONVERT(DATETIME, StartDateTime)) AS TotalTime,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0) AS StartDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0) AS EndDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, CONVERT(TIME, CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0)) - CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0))))) / 15) * 15, 0) AS TotalTimeRoundNearestDateFormat
 , DATEDIFF(MINUTE, EndDateTime, StartDateTime) AS dif
FROM dbo.PMol WHERE Id = @Id";
        var queryBreakTime = @"SELECT * FROM dbo.PmolBreak WHERE PmolId = @Id AND EndDateTime IS NOT NULL";
        var queryLabour = @"SELECT
  PMolPlannedWorkLabour.*
 ,PmolTeamRole.*
FROM dbo.PMolPlannedWorkLabour
LEFT OUTER JOIN dbo.PmolTeamRole
  ON PMolPlannedWorkLabour.Id = PmolTeamRole.PmolLabourId  WHERE PmolId =@Id";

        var updateResource =
            @"UPDATE dbo.PMolPlannedWorkLabour SET ConsumedQuantity = @ConsumedQuantity WHERE Id = @Id";

        var UpdateTeam = @"UPDATE dbo.PmolTeamRole SET ConsumedQuantity =  @ConsumedQuantity WHERE Id = @Id";

        var param = new { pmolParameter.Id, Date = date };
        double totalUnits = 0;
        await using (var connection = new SqlConnection(connectionString))
        {
            //var orderDictionary = new Dictionary<string, PmolResourceLabourDto>();
            var affectedRows2 =
                connection.Execute(
                    @"UPDATE dbo.PmolBreak SET EndDateTime = @EndDateTime ,IsBreak = 0 WHERE PmolId = @PmolId AND EndDateTime IS NULL",
                    new { EndDateTime = DateTime.UtcNow, PmolId = pmolParameter.Id });
            var affectedRows = connection.QuerySingleOrDefaultAsync(query, param).Result;
            var PmolTime = connection.QuerySingleOrDefaultAsync<PmolTime>(queryTime, param).Result;
            var PmolBreakTime = connection.Query<PmolBreakCal>(queryBreakTime, param);
            var diff = PmolTime.EndDateTimeRoundNearest.Subtract(PmolTime.StartDateTimeRoundNearest);

            var diffAll = new TimeSpan();
            foreach (var b in PmolBreakTime)
                if (b.EndDateTime != null)
                {
                    var d = b.EndDateTime.Subtract(b.StartDateTime);
                    diffAll = diffAll + d;
                    //diffAll.add
                }

            var diffBreak = diffAll.RoundToNearestMinutes(15);

            diff = diff.Subtract(diffBreak);

            if (diff != null)
            {
                if (diff.Days <= 0)
                {
                    totalUnits += diff.Hours;
                    var m = diff.Minutes / 60.0;
                    totalUnits += m;
                }
                else
                {
                    totalUnits = -1;
                }
            }

            var listLabourResource = connection.Query<PmolResourceLabourDto>(
                @"SELECT * FROM dbo.PMolPlannedWorkLabour WHERE PMolPlannedWorkLabour.PmolId = @Id", param);

            foreach (var laborR in listLabourResource)
            {
                laborR.Team = connection
                    .Query<PmolTeamRoleReadDto>(
                        @"SELECT PmolTeamRole.* FROM dbo.PmolTeamRole  WHERE PmolLabourId = @PmolLabourId",
                        new { PmolLabourId = laborR.Id }).ToList();


                connection.Execute(updateResource,
                    new
                    {
                        ConsumedQuantity = laborR.Team.Count > 0 ? totalUnits * laborR.Team.Count : totalUnits,
                        laborR.Id
                    });
                foreach (var labort in laborR.Team)
                    connection.Execute(UpdateTeam, new { ConsumedQuantity = totalUnits, labort.Id });
            }

            connection.Close();
        }

        return pmolParameter.Id;
    }


    public async Task<string> UpdateEndTimeByForeman(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var date = DateTime.UtcNow;
        var query = "update PMol set EndDateTime = @Date, IsBreak = 0 where Id = @Id";
        var queryTime = @"SELECT
  CONVERT(TIME, CONVERT(DATETIME, EndDateTime) - CONVERT(DATETIME, StartDateTime)) AS TotalTime,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0) AS StartDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0) AS EndDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, CONVERT(TIME, CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0)) - CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0))))) / 15) * 15, 0) AS TotalTimeRoundNearestDateFormat
 , DATEDIFF(MINUTE, EndDateTime, StartDateTime) AS dif
FROM dbo.PMol WHERE Id = @Id";
        var queryBreakTime = @"SELECT * FROM dbo.PmolBreak WHERE PmolId = @Id AND EndDateTime IS NOT NULL";

        var updateResource =
            @"UPDATE dbo.PMolPlannedWorkLabour SET ConsumedQuantity = @ConsumedQuantity WHERE Id = @Id";

        var param = new { pmolParameter.Id, Date = date };
        double totalUnits = 0;
        await using (var connection = new SqlConnection(connectionString))
        {
            var affectedRows2 =
                connection.Execute(
                    @"UPDATE dbo.PmolBreak SET EndDateTime = @EndDateTime ,IsBreak = 0 WHERE PmolId = @PmolId AND EndDateTime IS NULL",
                    new { EndDateTime = DateTime.UtcNow, PmolId = pmolParameter.Id });
            var affectedRows = connection.QuerySingleOrDefaultAsync(query, param).Result;
            var PmolTime = connection.QuerySingleOrDefaultAsync<PmolTime>(queryTime, param).Result;
            var PmolBreakTime = connection.Query<PmolBreakCal>(queryBreakTime, param);
            var diff = PmolTime.EndDateTimeRoundNearest.Subtract(PmolTime.StartDateTimeRoundNearest);

            var diffAll = new TimeSpan();
            foreach (var b in PmolBreakTime)
                if (b.EndDateTime != null)
                {
                    var d = b.EndDateTime.Subtract(b.StartDateTime);
                    diffAll = diffAll + d;
                }

            var diffBreak = diffAll.RoundToNearestMinutes(15);

            diff = diff.Subtract(diffBreak);

            if (diff != null)
            {
                if (diff.Days <= 0)
                {
                    totalUnits += diff.Hours;
                    var m = diff.Minutes / 60.0;
                    totalUnits += m;
                }
                else
                {
                    totalUnits = -1;
                }
            }

            var listLabourResource = connection.Query<PmolResourceLabourDto>(
                @"SELECT * FROM dbo.PMolPlannedWorkLabour WHERE PMolPlannedWorkLabour.PmolId = @Id", param);

            double? allConsumedQuantity = 0;

            foreach (var laborR in listLabourResource)
            {
                laborR.Team = connection
                    .Query<PmolTeamRoleReadDto>(
                        @"SELECT PmolTeamRole.* FROM dbo.PmolTeamRole  WHERE (PmolLabourId = @PmolLabourId AND PmolTeamRole.ConsumedQuantity != 0)",
                        new { PmolLabourId = laborR.Id }).ToList();

                foreach (var labort in laborR.Team)
                    allConsumedQuantity += labort.ConsumedQuantity;


                connection.Execute(updateResource,
                    new
                    {
                        ConsumedQuantity = allConsumedQuantity,
                        laborR.Id
                    });
            }

            connection.Close();
        }

        return pmolParameter.Id;
    }

    public async Task<string> BreakPmolStop(PmolParameter pmolParameter)
    {
        var connectionString1 = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        await using (var dbConnection = new SqlConnection(connectionString1))
        {
            dbConnection.Execute(
                @"UPDATE dbo.PmolBreak SET EndDateTime = @EndDateTime , IsBreak = 0 WHERE PmolId = @PmolId AND EndDateTime IS NULL",
                new { EndDateTime = DateTime.UtcNow, PmolId = pmolParameter.Id });
            dbConnection.Execute(@"update PMol set IsBreak = 0 where Id = @Id", new { pmolParameter.Id });

            
            return pmolParameter.Id;
        }
    }

    public async Task<string> BreakPmol(PmolParameter pmolParameter)
    {
        var connectionString1 = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        await using (var dbConnection = new SqlConnection(connectionString1))
        {
            dbConnection.Execute(
                @"INSERT INTO dbo.PmolBreak ( Id ,PmolId ,StartDateTime ,IsBreak ) VALUES ( NEWID() ,@PmolId ,@StartDateTime , 1 )",
                new { StartDateTime = DateTime.UtcNow, PmolId = pmolParameter.Id });
            dbConnection.Execute(@"update PMol set IsBreak = 1 where Id = @Id", new { pmolParameter.Id });

            
            return pmolParameter.Id;
        }
    }

    public async Task<List<PmolExtraWorkFilesReadDto>> ReadAudioByPmolId(PmolParameter pmolParameter)
    {
        var query1 = @"SELECT
                                      PMolExtraWorkFiles.Link
                                     ,PMolExtraWorkFiles.Title
                                        ,PMolExtraWorkFiles.Id
                                        ,PMolExtraWorkFiles.Type
                                        ,PMolExtraWorkFiles.PmolExtraWorkId
                                    FROM dbo.PMolExtraWorkFiles
                                    WHERE PMolExtraWorkFiles.PmolExtraWorkId = '" + pmolParameter.Id
            + "' AND PMolExtraWorkFiles.Type = '2'";

        var connectionString1 = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        List<PmolExtraWorkFilesReadDto> audioList = null;
        await using (var dbConnection = new SqlConnection(connectionString1))
        {
            audioList = dbConnection.Query<PmolExtraWorkFilesReadDto>(query1).ToList();
            
            return audioList;
        }
    }

    public async Task<string> UpdateFinishedStatus(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var shanukaDbContext = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);
        string pmolStatusId = null;
        var pmolStatus = shanukaDbContext.PMolStatus.FirstOrDefault(p => p.Name.Equals("In Review"));
        if (pmolStatus != null) pmolStatusId = pmolStatus.StatusId;

        var date = DateTime.UtcNow;
        var query = "update PMol set IsFinished = 1, StatusId = @PmolStatusId where Id  = @Id";
        var param = new { pmolParameter.Id, PmolStatusId = pmolStatusId };
        await using (var connection = new SqlConnection(connectionString))
        {
            var affectedRows = connection.QuerySingleOrDefaultAsync(query, param).Result;
            connection.Close();
        }

        return pmolParameter.Id;
    }

    public async Task<string> UploadImageForMobile(PmolParameter pmolParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);

        string pmolId = pmolParameter.formData["pmolId"];
        var extraWork = context.PMolExtraWork.FirstOrDefault(e => e.PmolId == pmolId);
        string id = null;
        if (extraWork == null)
        {
            var pmolExtraWork = new PmolExtraWork
            {
                PmolId = pmolId,
                Id = Guid.NewGuid().ToString()
            };
            context.PMolExtraWork.Add(pmolExtraWork);
            await context.SaveChangesAsync();
            id = pmolExtraWork.Id;
        }
        else
        {
            id = extraWork.Id;
        }


        var client = new FileClient();

        foreach (var file in pmolParameter.formData.Files)
        {
            var url = client.PersistPhotoInNewFolder(file?.FileName, pmolParameter.TenantProvider, file,
                "UploadExtraworkFiles");
            var pmolExtraWorkFile = new PmolExtraWorkFiles
            {
                Link = url,
                Type = "1",
                PmolExtraWorkId = id,
                Id = Guid.NewGuid().ToString()
            };

            context.PMolExtraWorkFiles.Add(pmolExtraWorkFile);
            await context.SaveChangesAsync();
        }

        return id;
    }

    public async Task<string> UploadAudioForMobile(PmolParameter pmolParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);

        string pmolId = pmolParameter.formData["pmolId"];
        // string title = pmolParameter.formData["title"];
        var extraWork = context.PMolExtraWork.Where(e => e.PmolId == pmolId).FirstOrDefault();
        string id = null;
        if (extraWork == null)
        {
            var pmolExtraWork = new PmolExtraWork();
            pmolExtraWork.PmolId = pmolId;
            pmolExtraWork.Id = Guid.NewGuid().ToString();

            context.PMolExtraWork.Add(pmolExtraWork);
            context.SaveChanges();
            id = pmolExtraWork.Id;
        }
        else
        {
            id = extraWork.Id;
        }


        var client = new FileClient();
        var url = client.PersistPhotoInNewFolder(pmolParameter.formData.Files.FirstOrDefault()?.FileName,
            pmolParameter.TenantProvider, pmolParameter.formData.Files.FirstOrDefault(), "UploadExtraworkFiles");
        var pmolExtraWorkFile = new PmolExtraWorkFiles
        {
            Link = url,
            Type = "2",
            Title = pmolParameter.formData["title"],
            PmolExtraWorkId = id,
            Length = pmolParameter.formData["length"],
            Id = Guid.NewGuid().ToString()
        };

        context.PMolExtraWorkFiles.Add(pmolExtraWorkFile);
        await context.SaveChangesAsync();

        return pmolExtraWorkFile.Id;
    }

    public async Task<string> ApprovePmol(PmolParameter pmolParameter)
    {
        string pmolType = null;
        string projectMoleculeId = null;

        var param = new { pmolParameter.Id };
        // var lessUsedMaterialQuery = "select * from PMolPlannedWorkMaterial where (PmolId = @Id) and (ConsumedQuantity <= (RequiredQuantity/2))";

        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);


        // await using (var connection = new SqlConnection(connectionString))
        // {
        //     var teamRoles = connection.Query<PmolLabourTime>(
        //         "SELECT  PmolLabourTime.* FROM  PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id LEFT OUTER JOIN PmolLabourTime ON PmolTeamRole.Id = PmolLabourTime.LabourId WHERE PMolPlannedWorkLabour.PmolId = @pmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0",
        //         new {pmolId = pmolParameter.Id}).ToList();
        //
        //     if (teamRoles.DistinctBy(x => x.LabourId).Count() > teamRoles.Count(x => x.Type == "8"))
        //     {
        //         return "False";
        //     }
        // }


        //IEnumerable<PmolPlannedWorkMaterial> lessUsedMaterial = null;

        //await using (var connection = new SqlConnection(connectionString))
        //{
        //    lessUsedMaterial = connection.Query<PmolPlannedWorkMaterial>(lessUsedMaterialQuery, param);
        //    if (lessUsedMaterial.Count() == 0) {
        //        lessUsedMaterial = null; 
        //    }
        //    connection.Close();
        //}

        // if (lessUsedMaterial == null) {
        var query = "update PMol set StatusId ='7bcb4e8d-8e8c-487d-8170-6b91c89fc3da' where Id =@Id";
        
            var affectedRows = connection.QuerySingleOrDefaultAsync(query, param).Result;
            pmolType = connection.Query<string>("SELECT PmolType FROM PMol WHERE Id = @Id", param).FirstOrDefault();
            // projectMoleculeId = connection.Query<string>("SELECT ProjectMoleculeId FROM PMol WHERE Id = @Id", param)
            //     .FirstOrDefault();
            

        IProjectDefinitionRepository _iProjectDefinitionRepository = new ProjectDefinitionRepository();


        var projectDefinition = await _iProjectDefinitionRepository.GetProjectDefinitionById(
            pmolParameter.ApplicationDbContext, pmolParameter.ProjectSequenceId, pmolParameter.Lang,
            pmolParameter.TenantProvider);

        if (projectDefinition.ProjectCostConversionCreateDto.TravelConversionOption == "200")
        {
        }


        var consumableSql =
            @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                        PmolTypeId,PmolType
                                       ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                        ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                    SELECT
                                     NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkConsumable.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkConsumable.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkConsumable.ConsumedQuantity * CorporateProductCatalog.InventoryPrice )
                                     ,0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkConsumable
                                      ON PMol.Id = PMolPlannedWorkConsumable.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkConsumable.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.Id = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkConsumable.IsDeleted = 0";

        var consumablParam = new
            { projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolParameter.Id };
        
            await connection.QueryAsync(consumableSql, consumablParam);
        
        var toolsSql =
            @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                    PmolTypeId,PmolType
                                    ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                    ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                    SELECT
                                     NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkTools.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkTools.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkTools.ConsumedQuantity * CorporateProductCatalog.InventoryPrice ), 0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkTools
                                      ON PMol.Id = PMolPlannedWorkTools.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.Id = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkTools.IsDeleted = 0";

        var toolsParam = new
            { projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolParameter.Id };
            await connection.QueryAsync(toolsSql, toolsParam);

        var materialSql =
            @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                       PmolTypeId,PmolType
                                       ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                       ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                       SELECT
                                       NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkMaterial.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkMaterial.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkMaterial.ConsumedQuantity * CorporateProductCatalog.InventoryPrice ), 0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkMaterial
                                      ON PMol.Id = PMolPlannedWorkMaterial.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkMaterial.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.Id = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkMaterial.IsDeleted = 0";

        var materialParam = new
            { projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolParameter.Id };
        
            await connection.QueryAsync(materialSql, materialParam);
            
        var labourSql =
            @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                     PmolTypeId,PmolType
                                     ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                     ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                    SELECT
                                     NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkLabour.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkLabour.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkLabour.ConsumedQuantity * CorporateProductCatalog.InventoryPrice ), 0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkLabour
                                      ON PMol.Id = PMolPlannedWorkLabour.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.Id = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkLabour.IsDeleted = 0";

        var labourParam = new
            { projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolParameter.Id };

        var serviceSql =
            @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle,
                                    PmolTypeId, PmolType
                                    , IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                    ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                      SELECT
                                       NEWID()
                                       ,@projectSq
                                       ,PbsProduct.Id
                                       ,PbsProduct.Title
                                       ,NULL
                                       ,NULL
                                       ,PMol.Id
                                       ,PMol.Title
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,PMol.StartDateTime
                                        ,NULL
                                        ,NULL
                                       ,PMolType.TypeId AS OriginalPmolTypeId
                                       ,PMolType.Name AS OriginalPmolType
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,PmolService.Quantity
                                       ,PmolService.MouId
                                       ,PmolService.UnitPrice
                                       ,PmolService.TotalPrice
                                       ,0
                                    FROM dbo.PMol
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PmolService
                                      ON PMol.Id = PmolService.PmolId
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON PMol.ProductId = PbsProduct.Id
                                    WHERE PMolType.LanguageCode = @lang AND PMol.Id = @pmolId";
        
            await connection.QueryAsync(labourSql, labourParam);
            //  await connection.QueryAsync(serviceSql, labourParam);
            if (pmolType == "sub") await connection.QueryAsync(serviceSql, labourParam);
            if (projectDefinition.ProjectCostConversionCreateDto.LoadingConversionOption == "200")
            {
                var convertSql =
                    @"UPDATE dbo.ProjectCost SET OriginalPmolType = 'Work' , OriginalPmolTypeId = '5bb656-f708-4a0d-9973-3d834ffe757d01' WHERE PmolId = '08b3e47d-76eb-4641-a5de-954888f3122a' AND OriginalPmolTypeId = '848e5e-622d-4783-95e6-4092004eb5eaff'";
                connection.Execute(convertSql);
            }

            if (projectDefinition.ProjectCostConversionCreateDto.TravelConversionOption == "200")
            {
                var convertSql =
                    @"UPDATE dbo.ProjectCost SET OriginalPmolType = 'Work' , OriginalPmolTypeId = '5bb656-f708-4a0d-9973-3d834ffe757d01' WHERE PmolId = '08b3e47d-76eb-4641-a5de-954888f3122a' AND OriginalPmolTypeId = '3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1'";
                connection.Execute(convertSql);
            }
            
        await UpdateBor(pmolParameter);

        var pbsCbcRes = connection.Query<PbsCbcResources>(
            @"Select * from PbsCbcResources Where PbsId = (SELECT ProductId FROM PMol WHERE Id = @Id)", param).ToList();
        
        var pmolCbcRes = connection.Query<PmolCbcResources>(
            @"SELECT * FROM PmolCbcResources  WHERE PmolId IN (SELECT Id FROM PMol  WHERE ProductId = (SELECT ProductId FROM PMol WHERE Id = @Id))", param).ToList();

        var resUpdateQuery = "Update PbsCbcResources Set ConsumedQuantity = @ConsumedQuantity Where Id = @Id";
        foreach (var res in pbsCbcRes)
        {
            var consQuantity = pmolCbcRes.Where(x => x.ArticleNo == res.ArticleNo).Sum( c => c.ConsumedQuantity.ToDouble());

            await connection.ExecuteAsync(resUpdateQuery, new { ConsumedQuantity = consQuantity.ToString("0.00"), Id = res.Id });
        }
        
        
        return pmolParameter.Id;
        //}

        //else
        //{
        //    pmolParameter.IsApproved = false;
        //    return pmolParameter.IsApproved.ToString();

        //}
    }

    public async Task<string> UpdateCommentMobile(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var updateSQL = @"UPDATE dbo.PMol SET Comment = @Comment WHERE Id = @Id";
        var param = new { pmolParameter.PmolCreateCommentDto.Id, pmolParameter.PmolCreateCommentDto.Comment };
        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.QueryAsync(updateSQL, param);

            connection.Close();
        }

        return "Ok";
    }


    public async Task<string> ReadLaborCalculation(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var date = DateTime.UtcNow;
        // string query = "update PMol set EndDateTime = @Date, IsBreak = 0 where Id = @Id";
        var queryTime = @"SELECT
  CONVERT(TIME, CONVERT(DATETIME, EndDateTime) - CONVERT(DATETIME, StartDateTime)) AS TotalTime,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0) AS StartDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0) AS EndDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, CONVERT(TIME, CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0)) - CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0))))) / 15) * 15, 0) AS TotalTimeRoundNearestDateFormat
 , DATEDIFF(MINUTE, EndDateTime, StartDateTime) AS dif
FROM dbo.PMol WHERE Id = @Id";
        var queryBreakTime = @"SELECT * FROM dbo.PmolBreak WHERE PmolId = @Id AND EndDateTime IS NOT NULL";
        var queryLabour = @"SELECT
  PMolPlannedWorkLabour.*
 ,PmolTeamRole.*
FROM dbo.PMolPlannedWorkLabour
LEFT OUTER JOIN dbo.PmolTeamRole
  ON PMolPlannedWorkLabour.Id = PmolTeamRole.PmolLabourId  WHERE PmolId =@Id";

        // var updateResource = @"UPDATE dbo.PMolPlannedWorkLabour SET ConsumedQuantity = @ConsumedQuantity WHERE Id = @Id";

        //var UpdateTeam = @"UPDATE dbo.PmolTeamRole SET ConsumedQuantity =  @ConsumedQuantity WHERE Id = @Id";

        var param = new { pmolParameter.Id, Date = date };
        double totalUnits = 0;
        await using (var connection = new SqlConnection(connectionString))
        {
            //var orderDictionary = new Dictionary<string, PmolResourceLabourDto>();
            var affectedRows2 =
                connection.Execute(
                    @"UPDATE dbo.PmolBreak SET EndDateTime = @EndDateTime ,IsBreak = 0 WHERE PmolId = @PmolId AND EndDateTime IS NULL",
                    new { EndDateTime = DateTime.UtcNow, PmolId = pmolParameter.Id });
            // var affectedRows = connection.QuerySingleOrDefaultAsync(query, param).Result;
            var PmolTime = connection.QuerySingleOrDefaultAsync<PmolTime>(queryTime, param).Result;
            var PmolBreakTime = connection.Query<PmolBreakCal>(queryBreakTime, param);
            var diff = PmolTime.EndDateTimeRoundNearest.Subtract(PmolTime.StartDateTimeRoundNearest);

            var diffAll = new TimeSpan();
            foreach (var b in PmolBreakTime)
                if (b.EndDateTime != null)
                {
                    var d = b.EndDateTime.Subtract(b.StartDateTime);
                    diffAll = diffAll + d;
                    //diffAll.add
                }

            var diffBreak = diffAll.RoundToNearestMinutes(15);

            diff = diff.Subtract(diffBreak);

            if (diff != null)
            {
                if (diff.Days <= 0)
                {
                    totalUnits += diff.Hours;
                    var m = diff.Minutes / 60.0;
                    totalUnits += m;
                }
                else
                {
                    totalUnits = -1;
                }
            }

            var listLabourResource = connection.Query<PmolResourceLabourDto>(
                @"SELECT * FROM dbo.PMolPlannedWorkLabour WHERE PMolPlannedWorkLabour.PmolId = @Id", param);

            foreach (var laborR in listLabourResource)
            {
                laborR.Team = connection
                    .Query<PmolTeamRoleReadDto>(
                        @"SELECT PmolTeamRole.* FROM dbo.PmolTeamRole  WHERE PmolLabourId = @PmolLabourId",
                        new { PmolLabourId = laborR.Id }).ToList();


                //connection.Execute(updateResource, new { ConsumedQuantity = laborR.Team.Count > 0 ? totalUnits * laborR.Team.Count : totalUnits, Id = laborR.Id });
                //foreach (var labort in laborR.Team)
                //{
                //    connection.Execute(UpdateTeam, new { ConsumedQuantity = totalUnits, Id = labort.Id });
                //}
                var paramz = new
                {
                    ConsumedQuantity = laborR.Team.Count > 0 ? totalUnits * laborR.Team.Count : totalUnits,
                    laborR.Id, totalUnits
                };
            }

            connection.Close();
        }

        return pmolParameter.Id;
    }

    public async Task<string> CreatePmolService(PmolParameter pmolParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        PmolServiceGetByIdDto data;
        PmolServiceDocuments docData;
        string newId = null;
        if (pmolParameter.PmolServiceCreate.PmolId != null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<PmolServiceGetByIdDto>("SELECT * FROM PmolService WHERE PmolId =@PmolId",
                    new { pmolParameter.PmolServiceCreate.PmolId }).FirstOrDefault();
            }

            if (data == null)
            {
                var insertQuery =
                    @"INSERT INTO PmolService ([Id], [PmolId], [Quantity], [MouId], [UnitPrice], [TotalPrice], [Comments]) VALUES (@Id, @PmolId, @Quantity, @MouId, @UnitPrice, @TotalPrice, @Comments)";
                newId = Guid.NewGuid().ToString();
                var parameters = new
                {
                    Id = newId,
                    pmolParameter.PmolServiceCreate.PmolId,
                    pmolParameter.PmolServiceCreate.Quantity,
                    MouId = pmolParameter.PmolServiceCreate.Mou,
                    pmolParameter.PmolServiceCreate.UnitPrice,
                    pmolParameter.PmolServiceCreate.TotalPrice,
                    pmolParameter.PmolServiceCreate.Comments
                };

                await using (var connection = new SqlConnection(connectionString))
                {
                    connection.Execute(insertQuery, parameters);
                }

                if (pmolParameter.PmolServiceCreate.Documents.FirstOrDefault() != null)
                    foreach (var doc in pmolParameter.PmolServiceCreate.Documents)
                    {
                        var docInsert =
                            @"INSERT INTO PmolServiceDocuments ([Id], [Link], [PmolId], [ServiceId]) VALUES (@Id, @Link, @PmolId, @ServiceId)";

                        var param = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            Link = doc,
                            pmolParameter.PmolServiceCreate.PmolId,
                            ServiceId = newId
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Execute(docInsert, param);
                        }
                    }
            }
            else
            {
                var updateQuery =
                    @"UPDATE PmolService SET  Quantity = @Quantity, MouId = @MouId, UnitPrice = @UnitPrice, TotalPrice = @TotalPrice, Comments = @Comments WHERE Id = @Id";

                var parameters = new
                {
                    data.Id,
                    pmolParameter.PmolServiceCreate.Quantity,
                    MouId = pmolParameter.PmolServiceCreate.Mou,
                    pmolParameter.PmolServiceCreate.UnitPrice,
                    pmolParameter.PmolServiceCreate.TotalPrice,
                    pmolParameter.PmolServiceCreate.Comments
                };

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Execute(updateQuery, parameters);
                    docData = connection
                        .Query<PmolServiceDocuments>("SELECT * FROM PmolServiceDocuments WHERE PmolId =@PmolId",
                            new { pmolParameter.PmolServiceCreate.PmolId }).FirstOrDefault();
                }

                if (pmolParameter.PmolServiceCreate.Documents.FirstOrDefault() != null)
                {
                    if (docData == null)
                        foreach (var doc in pmolParameter.PmolServiceCreate.Documents)
                        {
                            var docInsert =
                                @"INSERT INTO PmolServiceDocuments ([Id], [Link], [PmolId], [ServiceId]) VALUES (@Id, @Link, @PmolId, @ServiceId)";

                            var param = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                Link = doc,
                                pmolParameter.PmolServiceCreate.PmolId,
                                ServiceId = data.Id
                            };

                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Execute(docInsert, param);
                            }
                        }
                    else
                        foreach (var doc in pmolParameter.PmolServiceCreate.Documents)
                        {
                            var docInsert =
                                @"INSERT INTO PmolServiceDocuments ([Id], [Link], [PmolId], [ServiceId]) VALUES (@Id, @Link, @PmolId, @ServiceId)";

                            var param = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                Link = doc,
                                pmolParameter.PmolServiceCreate.PmolId,
                                ServiceId = data.Id
                            };

                            string docLink;

                            using (var connection = new SqlConnection(connectionString))
                            {
                                docLink = connection
                                    .Query<string>("SELECT Id FROM PmolServiceDocuments WHERE Link =@doc",
                                        new { doc }).FirstOrDefault();
                            }

                            if (docLink == null)
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(docInsert, param);
                                }
                        }
                }
            }
        }

        return newId;
    }

    public async Task<PmolServiceGetByIdDto> ReadPmolServiceByPmolId(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        IEnumerable<PmolServiceGetByIdDto> data;
        List<string> doc;

        using (var connection = new SqlConnection(connectionString))
        {
            data = connection.Query<PmolServiceGetByIdDto>(
                "SELECT *,MouId AS Mou FROM PmolService WHERE PmolId =@PmolId", new { PmolId = pmolParameter.Id });
        }

        if (data.FirstOrDefault() != null)
        {
            var documents = @"SELECT Link FROM dbo.PmolServiceDocuments WHERE PmolId =@Id";

            using (var connection = new SqlConnection(connectionString))
            {
                doc = connection.Query<string>(documents, new { pmolParameter.Id }).ToList();
                data.FirstOrDefault().PmolTitle = connection
                    .Query<string>("SELECT Title FROM PMol WHERE Id = @Id", new { pmolParameter.Id })
                    .FirstOrDefault();
            }

            using (var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
            {
                data.FirstOrDefault().ProjectTitle = connection
                    .Query<string>("SELECT Title FROM ProjectDefinition WHERE SequenceCode = @SequenceCode",
                        new { SequenceCode = pmolParameter.ProjectSequenceId }).FirstOrDefault();
            }

            if (doc.FirstOrDefault() != null) data.FirstOrDefault().Documents = doc;
        }

        return data.FirstOrDefault();
    }

    public async Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserIdMobile(PmolParameter pmolParameter)
    {
        var options1 = new DbContextOptions<CabPersonCompanyDbContext>();
        var applicationDbContext1 = new CabPersonCompanyDbContext(options1, pmolParameter.TenantProvider);
        var personCompany =
            applicationDbContext1.CabPersonCompany.FirstOrDefault(p => p.Oid.Equals(pmolParameter.UserId));
        // var foreman = applicationDbContext1.CabPerson.Where(p => p.Id == personCompany.PersonId).FirstOrDefault();


        if (personCompany != null)
        {
            var cuConnectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId, null,
                pmolParameter.TenantProvider);

            await using var cuConnection = new SqlConnection(cuConnectionString);

            var isShiftStarted = cuConnection
                .Query("Select * From Shifts Where UserId = @UserId AND EndDateTime IS NULL",
                    new { UserId = pmolParameter.UserId }).Any();


            var pmolListDtos = new List<PmolListDtoForMobile>();
            var projectConnectionList =
                GetProjectConnectionsByUser(personCompany.Oid, pmolParameter.TenantProvider);
            foreach (var projectCon in projectConnectionList)
                if (projectCon != null && projectCon.ProjectConnectionString != null && projectCon.Id != null)
                {
                    var query = @"SELECT
  DISTINCT PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS Type
 ,PMolStatus.Name AS Status
 ,PMolType.TypeId
 ,PMolStatus.StatusId
 ,PMol.ForemanId AS ForemanId
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,REPLACE(PMol.ExecutionStartTime,':','') AS ExecutionTime
 ,Bor.PbsProductId AS PbsId
 ,PmolTeamRole.CabPersonId, CASE WHEN PmolTeamRole.CabPersonID IS NULL THEN 1 ELSE 0 END AS IsForeman
 ,PMol.IsBreak
 ,@isShiftStarted As IsShiftStart
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
LEFT OUTER JOIN dbo.PMolPlannedWorkLabour
  ON PMol.Id = PMolPlannedWorkLabour.PmolId
LEFT OUTER JOIN dbo.PmolTeamRole
  ON PMolPlannedWorkLabour.Id = PmolTeamRole.PmolLabourId
WHERE (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0
AND PMol.IsFinished = 0
AND ((PmolTeamRole.CabPersonId = @CabPersonId AND PmolTeamRole.IsDeleted = 0) OR PMol.ForemanId = @ForemanId)
ORDER BY PMol.Executiondate DESC";
                    var sb = new StringBuilder(query);

                    var connectionString = projectCon.ProjectConnectionString;
                    var parameters = new
                    {
                        lang = pmolParameter.Lang, CabPersonId = personCompany.PersonId,
                        ForemanId = personCompany.PersonId,
                        isShiftStarted = isShiftStarted
                    };
                    IEnumerable<PmolListDtoForMobile> data = null;
                    IEnumerable<PmolLabourTimeIsLabour> times = null;
                    // var orderDictionary = new Dictionary<string, PmolListDtoForMobile>();


                    await using var connection = new SqlConnection(connectionString);


                    connection.Open();
                    data = connection.Query<PmolListDtoForMobile>(sb.ToString(), parameters).ToList();
                    data = data.DistinctBy(x => x.ProjectMoleculeId);
                    
                    // times = connection.Query<PmolLabourTimeIsLabour>(
                    //     "SELECT PMolPlannedWorkLabour.PmolId ,PmolTeamRole.CabPersonId FROM dbo.PmolLabourTime LEFT OUTER JOIN dbo.PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN dbo.PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERe PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 ");


                    if (data.Any())
                        foreach (var dto in data)
                        {
                            var foreman = applicationDbContext1.CabPerson
                                .FirstOrDefault(p => p.Id == dto.ForemanId);

                            //dto.IsLabourStarted = times.Where(t=> t.CabPersonId == personCompany.PersonId && t.PmolId == dto.ProjectMoleculeId).Any();
                            dto.ProjectId = projectCon.Id;
                            var projectDefinitionMobDto = new ProjectDefinitionMobDto();
                            projectDefinitionMobDto.CuId = projectCon.CuId;
                            projectDefinitionMobDto.SequenceCode = projectCon.SequenceCode;
                            projectDefinitionMobDto.Title = projectCon.SequenceCode + " - " + projectCon.Name;
                            dto.ProjectDefinition = projectDefinitionMobDto;
                            dto.Foreman = foreman?.FullName;

                            var isStart = connection.Query<PmolLabourTime>(
                                "SELECT PmolLabourTime.* FROM PmolLabourTime LEFT OUTER JOIN PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0",
                                new { PmolId = dto.Id }).Any();

                            var isLabourStart = connection.Query<PmolLabourTime>(
                                "SELECT PmolLabourTime.* FROM PmolLabourTime LEFT OUTER JOIN PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 AND PmolTeamRole.CabPersonId = @personId Order By PmolLabourTime.StartDateTime desc ",
                                new { PmolId = dto.Id, personId = personCompany.PersonId });

                            dto.IsStarted = isStart;

                            if (isLabourStart.Any())
                            {
                                dto.IsBreak = isLabourStart.FirstOrDefault().IsBreak;

                                if (isLabourStart.LastOrDefault().StartDateTime != null)
                                {
                                    dto.StartDateTime = isLabourStart.LastOrDefault().StartDateTime;
                                    dto.EndDateTime = null;
                                }

                                else
                                {
                                    var lTime = isLabourStart.Where(x => x.StartDateTime != null)
                                        .OrderByDescending(v => v.StartDateTime);
                                    if (lTime.Any())
                                        dto.StartDateTime = lTime.LastOrDefault().StartDateTime;
                                    else
                                        dto.StartDateTime = null;

                                    dto.EndDateTime = isLabourStart.LastOrDefault().EndDateTime;
                                }
                            }
                            else
                            {
                                dto.IsBreak = true;
                                dto.StartDateTime = null;
                            }

                            //dto.IsLabourStarted = isLabourStart;
                            pmolParameter.Id = dto.Id;
                            pmolParameter.ProjectSequenceId = projectCon.SequenceCode;
                            pmolParameter.ContractingUnitSequenceId = projectCon.CuId;
                            dto.IsForeman = await IsForeman(pmolParameter);
                        }

                    pmolListDtos.AddRange(data);
                }


            var pmolListDtosX = new List<PmolListDtoForMobile>(); // please dont change
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime != null)
                .OrderBy(s => s.StartDateTime));
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime == null));

            return pmolListDtosX.OrderBy(x => x.ExecutionDate).ThenBy(v => v.ExecutionTime);
        }

        return null;
    }
    
    public async Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserIdMobileChanged(PmolParameter pmolParameter)
    {
        var options1 = new DbContextOptions<CabPersonCompanyDbContext>();
        var applicationDbContext1 = new CabPersonCompanyDbContext(options1, pmolParameter.TenantProvider);
        var personCompany =
            applicationDbContext1.CabPersonCompany.FirstOrDefault(p => p.Oid.Equals(pmolParameter.UserId));
        // var foreman = applicationDbContext1.CabPerson.Where(p => p.Id == personCompany.PersonId).FirstOrDefault();


        if (personCompany != null)
        {
            var cuConnectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId, null,
                pmolParameter.TenantProvider);

            await using var cuConnection = new SqlConnection(cuConnectionString);

            var isShiftStarted = cuConnection
                .Query("Select * From Shifts Where UserId = @UserId AND EndDateTime IS NULL",
                    new { UserId = pmolParameter.UserId }).Any();


            var pmolListDtos = new List<PmolListDtoForMobile>();
            var projectConnectionList =
                GetProjectConnectionsByUser(personCompany.Oid, pmolParameter.TenantProvider);
            foreach (var projectCon in projectConnectionList)
                if (projectCon != null && projectCon.ProjectConnectionString != null && projectCon.Id != null)
                {
                    var query = @"SELECT
  DISTINCT PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS Type
 ,PMolStatus.Name AS Status
 ,PMolType.TypeId
 ,PMolStatus.StatusId
 ,PMol.ForemanId AS ForemanId
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,REPLACE(PMol.ExecutionStartTime,':','') AS ExecutionTime
 ,Bor.PbsProductId AS PbsId
 ,PmolTeamRole.CabPersonId, CASE WHEN PmolTeamRole.CabPersonID IS NULL THEN 1 ELSE 0 END AS IsForeman
 ,PMol.IsBreak
 ,@isShiftStarted As IsShiftStart
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
LEFT OUTER JOIN dbo.PMolPlannedWorkLabour
  ON PMol.Id = PMolPlannedWorkLabour.PmolId
LEFT OUTER JOIN dbo.PmolTeamRole
  ON PMolPlannedWorkLabour.Id = PmolTeamRole.PmolLabourId
WHERE (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0
AND PMol.IsFinished = 0
AND ((PmolTeamRole.CabPersonId = @CabPersonId AND PmolTeamRole.IsDeleted = 0) OR PMol.ForemanId = @ForemanId)
ORDER BY PMol.Executiondate DESC";
                    var sb = new StringBuilder(query);

                    var connectionString = projectCon.ProjectConnectionString;
                    var parameters = new
                    {
                        lang = pmolParameter.Lang, CabPersonId = personCompany.PersonId,
                        ForemanId = personCompany.PersonId,
                        isShiftStarted = isShiftStarted
                    };
                    IEnumerable<PmolListDtoForMobile> data = null;
                    IEnumerable<PmolLabourTimeIsLabour> times = null;
                    // var orderDictionary = new Dictionary<string, PmolListDtoForMobile>();


                    await using var connection = new SqlConnection(connectionString);


                    connection.Open();
                    data = connection.Query<PmolListDtoForMobile>(sb.ToString(), parameters).ToList();
                    data = data.DistinctBy(x => x.ProjectMoleculeId);
                    
                    // times = connection.Query<PmolLabourTimeIsLabour>(
                    //     "SELECT PMolPlannedWorkLabour.PmolId ,PmolTeamRole.CabPersonId FROM dbo.PmolLabourTime LEFT OUTER JOIN dbo.PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN dbo.PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERe PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 ");


                    if (data.Any())
                        foreach (var dto in data)
                        {
                            var foreman = applicationDbContext1.CabPerson
                                .FirstOrDefault(p => p.Id == dto.ForemanId);

                            //dto.IsLabourStarted = times.Where(t=> t.CabPersonId == personCompany.PersonId && t.PmolId == dto.ProjectMoleculeId).Any();
                            dto.ProjectId = projectCon.Id;
                            var projectDefinitionMobDto = new ProjectDefinitionMobDto();
                            projectDefinitionMobDto.CuId = projectCon.CuId;
                            projectDefinitionMobDto.SequenceCode = projectCon.SequenceCode;
                            projectDefinitionMobDto.Title = projectCon.SequenceCode + " - " + projectCon.Name;
                            dto.ProjectDefinition = projectDefinitionMobDto;
                            dto.Foreman = foreman?.FullName;

                            var isStart = connection.Query<PmolLabourTime>(
                                "SELECT PmolLabourTime.* FROM PmolLabourTime LEFT OUTER JOIN PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0",
                                new { PmolId = dto.Id }).Any();

                            var isLabourStart = connection.Query<PmolLabourTime>(
                                "SELECT PmolLabourTime.* FROM PmolLabourTime LEFT OUTER JOIN PmolTeamRole ON PmolLabourTime.LabourId = PmolTeamRole.Id LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 AND PmolTeamRole.CabPersonId = @personId Order By PmolLabourTime.StartDateTime desc ",
                                new { PmolId = dto.Id, personId = personCompany.PersonId });

                            dto.IsFinished = isLabourStart.Any(x => x.Type == "8");
                            dto.IsStarted = isStart;

                            if (isLabourStart.Any())
                            {
                                dto.IsBreak = isLabourStart.FirstOrDefault().IsBreak;

                                if (isLabourStart.LastOrDefault().StartDateTime != null)
                                {
                                    dto.StartDateTime = isLabourStart.LastOrDefault().StartDateTime;
                                    dto.EndDateTime = null;
                                }

                                else
                                {
                                    var lTime = isLabourStart.Where(x => x.StartDateTime != null)
                                        .OrderByDescending(v => v.StartDateTime);
                                    if (lTime.Any())
                                        dto.StartDateTime = lTime.LastOrDefault().StartDateTime;
                                    else
                                        dto.StartDateTime = null;

                                    dto.EndDateTime = isLabourStart.LastOrDefault().EndDateTime;
                                }
                            }
                            else
                            {
                                dto.IsBreak = true;
                                dto.StartDateTime = null;
                            }

                            //dto.IsLabourStarted = isLabourStart;
                            pmolParameter.Id = dto.Id;
                            pmolParameter.ProjectSequenceId = projectCon.SequenceCode;
                            pmolParameter.ContractingUnitSequenceId = projectCon.CuId;
                            dto.IsForeman = await IsForeman(pmolParameter);
                        }

                    pmolListDtos.AddRange(data);
                }


            var pmolListDtosX = new List<PmolListDtoForMobile>(); // please dont change
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime != null)
                .OrderBy(s => s.StartDateTime));
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime == null));

            return pmolListDtosX.Where(c => c.IsFinished == false).OrderBy(x => x.ExecutionDate).ThenBy(v => v.ExecutionTime);
        }

        return null;
    }

    public async Task<bool> ForemanCheckMobile(PmolParameter pmolParameter)
    {
        var isExist = false;

        var connectionString1 = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var query = "select * from Pmol where Id = @Id and ForemanId = @ForemanId";

        var query2 = "select * from CabPersonCompany where Oid =@Oid";

        PmolGetByIdDto currentPmol;
        CabDataDapperDto cabPerson;

        var parameters2 = new { Oid = pmolParameter.UserId };
        await using (var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            cabPerson = connection.Query<CabDataDapperDto>(query2, parameters2).FirstOrDefault();
            connection.Close();
        }

        var parameters = new { pmolParameter.Id, ForemanId = cabPerson.PersonId };

        await using (var connection = new SqlConnection(connectionString1))
        {
            currentPmol = connection.Query<PmolGetByIdDto>(query, parameters).FirstOrDefault();
            connection.Close();
        }

        if (currentPmol != null)
        {
            isExist = true;
            return isExist;
        }

        return isExist;
    }

    public async Task<string> UpdateLabourStartTime(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var query = "select * from CabPersonCompany where Oid =@Oid";

        CabDataDapperDto cabPerson;

        var parameters = new { Oid = pmolParameter.UserId };
        await using (var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            cabPerson = connection.Query<CabDataDapperDto>(query, parameters).FirstOrDefault();
            connection.Close();
        }

        var query2 = "select * from PmolTeamRole where CabPersonId = @CabPersonId and IsDeleted = 0";
        var query3 = "select * from PMolPlannedWorkLabour where PmolId = @PmolId and IsDeleted = 0";
        IEnumerable<PmolTeamRoleReadDto> currentUserTeamRoles;
        IEnumerable<PlannedWorkLabourOfPMol> pmolPlannedWorkLabourList;

        var parameters2 = new { CabPersonId = cabPerson.PersonId };
        var parameters3 = new { PmolId = pmolParameter.Id };
        await using (var connection = new SqlConnection(connectionString))
        {
            currentUserTeamRoles = connection.Query<PmolTeamRoleReadDto>(query2, parameters2).ToList();
            pmolPlannedWorkLabourList = connection.Query<PlannedWorkLabourOfPMol>(query3, parameters3).ToList();
            connection.Close();
        }

        PmolTeamRoleReadDto currentWorker = null;
        foreach (var pmolPlannedWorkLabour in pmolPlannedWorkLabourList)
        foreach (var currentUserTeamRole in currentUserTeamRoles)
            if (currentUserTeamRole.PmolLabourId == pmolPlannedWorkLabour.Id)
                currentWorker = currentUserTeamRole;

        var labour = currentWorker;

        var date = DateTime.UtcNow;

        if (labour != null)
        {
            var query4 =
                "insert into PmolLabourTime (Id, LabourId, StartDateTime, EndDateTime) values (NEWID(), @LabourId, @Date, null) ";
            var param = new { LabourId = labour.PmolLabourId, Date = date };
            await using (var connection = new SqlConnection(connectionString))
            {
                var affectedRows = connection.QuerySingleOrDefaultAsync(query4, param).Result;
                connection.Close();
            }

            return labour.Id;
        }

        return null;
    }

    public async Task<IEnumerable<PmolListDtoForMobile>> GetPmolByUserIdOfLabour(PmolParameter pmolParameter)
    {
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext1 = new ApplicationDbContext(options1, pmolParameter.TenantProvider);
        var personCompany =
            applicationDbContext1.CabPersonCompany.FirstOrDefault(p => p.Oid.Equals(pmolParameter.UserId));
        var foreman = applicationDbContext1.CabPerson.Where(p => p.Id == personCompany.PersonId).FirstOrDefault();

        if (personCompany != null)
        {
            var pmolListDtos = new List<PmolListDtoForMobile>();
            var projectConnectionList =
                GetProjectConnectionsByUser(personCompany.Oid, pmolParameter.TenantProvider);
            foreach (var projectCon in projectConnectionList)
                if (projectCon != null && projectCon.ProjectConnectionString != null && projectCon.Id != null)
                {
                    var query = @"SELECT
  DISTINCT PMol.Id
 ,PMol.ProjectMoleculeId
 ,PMol.Name
 ,CONCAT(PMol.ProjectMoleculeId, ' ', PMol.Name) AS Title
 ,PMol.ExecutionDate
 ,PMolType.Name AS Type
 ,PMolStatus.Name AS Status
 ,PMolType.TypeId
 ,PMolStatus.StatusId
 ,PMol.ForemanId AS ForemanId
 ,PMolType.Type AS TypeNo
 ,PMol.StartDateTime
 ,PMol.EndDateTime
 ,Bor.PbsProductId AS PbsId
 ,PMol.IsBreak
FROM dbo.PMol
LEFT OUTER JOIN dbo.PMolType
  ON PMol.TypeId = PMolType.TypeId
LEFT OUTER JOIN dbo.PMolStatus
  ON PMol.StatusId = PMolStatus.StatusId
LEFT OUTER JOIN dbo.Bor
  ON PMol.BorId = Bor.Id
LEFT OUTER JOIN dbo.PMolPlannedWorkLabour
  ON PMol.Id = PMolPlannedWorkLabour.PmolId
LEFT OUTER JOIN dbo.PmolTeamRole
  ON PMolPlannedWorkLabour.Id = PmolTeamRole.PmolLabourId
WHERE (PMolType.LanguageCode = @lang
OR PMol.TypeId IS NULL)
AND (PMolStatus.LanguageCode = @lang
OR PMol.StatusId IS NULL)
AND PMol.IsDeleted = 0
AND PMol.IsFinished = 0
AND (PmolTeamRole.CabPersonId = @CabPersonId)
AND PmolTeamRole.IsDeleted = 0
ORDER BY PMol.Executiondate DESC";
                    var sb = new StringBuilder(query);

                    var connectionString = projectCon.ProjectConnectionString;
                    var parameters = new { lang = pmolParameter.Lang, CabPersonId = personCompany.PersonId };
                    IEnumerable<PmolListDtoForMobile> data = null;
                    var orderDictionary = new Dictionary<string, PmolListDtoForMobile>();

                    await using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        data = connection.Query<PmolListDtoForMobile>(sb.ToString(), parameters);
                    }


                    if (data != null && data.Any())
                    {
                        foreach (var dto in data)
                        {
                            dto.ProjectId = projectCon.Id;
                            var projectDefinitionMobDto = new ProjectDefinitionMobDto();
                            projectDefinitionMobDto.CuId = projectCon.CuId;
                            projectDefinitionMobDto.SequenceCode = projectCon.SequenceCode;
                            projectDefinitionMobDto.Title = projectCon.SequenceCode + " - " + projectCon.Name;
                            dto.ProjectDefinition = projectDefinitionMobDto;
                            dto.Foreman = foreman.FullName;
                        }

                        pmolListDtos.AddRange(data);
                    }
                }

            var pmolListDtosX = new List<PmolListDtoForMobile>(); // please dont change
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime != null)
                .OrderBy(s => s.StartDateTime));
            pmolListDtosX.AddRange(pmolListDtos.Where(pmol => pmol.StartDateTime == null));
            return pmolListDtosX;
        }

        return null;
    }

    public async Task<string> UpdateLabourpmolEndTime(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var query = "select * from CabPersonCompany where Oid =@Oid";

        CabDataDapperDto cabPerson;

        var parameters = new { Oid = pmolParameter.UserId };
        await using (var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            cabPerson = connection.Query<CabDataDapperDto>(query, parameters).FirstOrDefault();
            connection.Close();
        }

        var query2 = "select * from PmolTeamRole where CabPersonId = @CabPersonId and IsDeleted = 0";
        var query3 = "select * from PMolPlannedWorkLabour where PmolId = @PmolId and IsDeleted = 0";
        IEnumerable<PmolTeamRoleReadDto> currentUserTeamRoles;
        IEnumerable<PlannedWorkLabourOfPMol> pmolPlannedWorkLabourList;

        var parameters2 = new { CabPersonId = cabPerson.PersonId };
        var parameters3 = new { PmolId = pmolParameter.Id };
        await using (var connection = new SqlConnection(connectionString))
        {
            currentUserTeamRoles = connection.Query<PmolTeamRoleReadDto>(query2, parameters2).ToList();
            pmolPlannedWorkLabourList = connection.Query<PlannedWorkLabourOfPMol>(query3, parameters3).ToList();
            connection.Close();
        }

        PmolTeamRoleReadDto currentWorker = null;
        foreach (var pmolPlannedWorkLabour in pmolPlannedWorkLabourList)
        foreach (var currentUserTeamRole in currentUserTeamRoles)
            if (currentUserTeamRole.PmolLabourId == pmolPlannedWorkLabour.Id)
                currentWorker = currentUserTeamRole;

        var labour = currentWorker;

        var query4 = "select * from PmolLabourTime where LabourId = @LabourId";
        var query5 = "select * from PmolLabourBreak where LabourId = @LabourId";

        PmolLabourTimeRead pmolLabourTime;
        PmolLabourBreakRead pmolLabourBreak;
        var parameters4 = new { LabourId = labour.Id };
        await using (var connection = new SqlConnection(connectionString))
        {
            pmolLabourTime = connection.QuerySingleOrDefaultAsync<PmolLabourTimeRead>(query4, parameters4).Result;
            pmolLabourBreak = connection.QuerySingleOrDefaultAsync<PmolLabourBreakRead>(query5, parameters4).Result;
            connection.Close();
        }


        var date = DateTime.UtcNow;

        var query6 = "update PmolLabourTime set EndDateTime = @Date, IsBreak = 0 where LabourId = @LabourId";
        var queryTime = @"SELECT
  CONVERT(TIME, CONVERT(DATETIME, EndDateTime) - CONVERT(DATETIME, StartDateTime)) AS TotalTime,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0) AS StartDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0) AS EndDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, CONVERT(TIME, CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0)) - CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0))))) / 15) * 15, 0) AS TotalTimeRoundNearestDateFormat
 , DATEDIFF(MINUTE, EndDateTime, StartDateTime) AS dif
FROM dbo.PmolLabourTime WHERE LabourId = @LabourId";
        var queryBreakTime =
            @"SELECT * FROM dbo.PmolLabourBreak WHERE LabourId = @LabourId AND EndDateTime IS NOT NULL";


        var updateLabourConsumedQuantity =
            @"UPDATE dbo.PmolTeamRole SET ConsumedQuantity =  @ConsumedQuantity WHERE Id = @Id";

        var param1 = new { pmolLabourTime.LabourId, Date = date };
        double totalUnits = 0;

        await using (var connection = new SqlConnection(connectionString))
        {
            var affectedRows2 =
                connection.Execute(
                    @"UPDATE dbo.PmolLabourBreak SET EndDateTime = @EndDateTime ,IsBreak = 0 WHERE LabourId = @LabourId AND EndDateTime IS NULL",
                    new { EndDateTime = DateTime.UtcNow, LabourId = labour.Id });
            var affectedRows = connection.QuerySingleOrDefaultAsync(query6, param1).Result;
            var LabourTime = connection.QuerySingleOrDefaultAsync<LabourTime>(queryTime, param1).Result;
            var LabourBreakTime = connection.Query<LabourBreakCal>(queryBreakTime, param1);
            var diff = LabourTime.EndDateTimeRoundNearest.Subtract(LabourTime.StartDateTimeRoundNearest);

            var diffAll = new TimeSpan();
            foreach (var b in LabourBreakTime)
                if (b.EndDateTime != null)
                {
                    var d = b.EndDateTime.Subtract(b.StartDateTime);
                    diffAll = diffAll + d;
                }

            var diffBreak = diffAll.RoundToNearestMinutes(15);

            diff = diff.Subtract(diffBreak);

            if (diff != null)
            {
                if (diff.Days <= 0)
                {
                    totalUnits += diff.Hours;
                    var m = diff.Minutes / 60.0;
                    totalUnits += m;
                }
                else
                {
                    totalUnits = -1;
                }
            }

            connection.Execute(updateLabourConsumedQuantity,
                new
                {
                    ConsumedQuantity = totalUnits,
                    labour.Id
                });
        }

        return pmolParameter.Id;
    }

    public async Task<string> BreakLabourStop(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var query = "select * from CabPersonCompany where Oid =@Oid";

        CabDataDapperDto cabPerson;

        var parameters = new { Oid = pmolParameter.UserId };
        await using (var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            cabPerson = connection.Query<CabDataDapperDto>(query, parameters).FirstOrDefault();
            connection.Close();
        }

        var query2 = "select * from PmolTeamRole where CabPersonId = @CabPersonId and IsDeleted = 0";
        var query3 = "select * from PMolPlannedWorkLabour where PmolId = @PmolId and IsDeleted = 0";
        IEnumerable<PmolTeamRoleReadDto> currentUserTeamRoles;
        IEnumerable<PlannedWorkLabourOfPMol> pmolPlannedWorkLabourList;

        var parameters2 = new { CabPersonId = cabPerson.PersonId };
        var parameters3 = new { PmolId = pmolParameter.Id };
        await using (var connection = new SqlConnection(connectionString))
        {
            currentUserTeamRoles = connection.Query<PmolTeamRoleReadDto>(query2, parameters2).ToList();
            pmolPlannedWorkLabourList = connection.Query<PlannedWorkLabourOfPMol>(query3, parameters3).ToList();
            connection.Close();
        }

        PmolTeamRoleReadDto currentWorker = null;
        foreach (var pmolPlannedWorkLabour in pmolPlannedWorkLabourList)
        foreach (var currentUserTeamRole in currentUserTeamRoles)
            if (currentUserTeamRole.PmolLabourId == pmolPlannedWorkLabour.Id)
                currentWorker = currentUserTeamRole;

        var labour = currentWorker;

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            dbConnection.Execute(
                @"UPDATE dbo.PmolLabourBreak SET EndDateTime = @EndDateTime , IsBreak = 0 WHERE LabourId = @LabourId AND EndDateTime IS NULL",
                new { EndDateTime = DateTime.UtcNow, LabourId = labour.Id });
            dbConnection.Execute(@"update PmolLabourTime set IsBreak = 0 where LabourId = @LabourId",
                new { labour.Id });

            
            return pmolParameter.Id;
        }
    }

    public async Task<string> BreakLabour(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        var query = "select * from CabPersonCompany where Oid =@Oid";

        CabDataDapperDto cabPerson;

        var parameters = new { Oid = pmolParameter.UserId };
        await using (var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        {
            cabPerson = connection.Query<CabDataDapperDto>(query, parameters).FirstOrDefault();
            connection.Close();
        }

        var query2 = "select * from PmolTeamRole where CabPersonId = @CabPersonId and IsDeleted = 0";
        var query3 = "select * from PMolPlannedWorkLabour where PmolId = @PmolId and IsDeleted = 0";
        IEnumerable<PmolTeamRoleReadDto> currentUserTeamRoles;
        IEnumerable<PlannedWorkLabourOfPMol> pmolPlannedWorkLabourList;

        var parameters2 = new { CabPersonId = cabPerson.PersonId };
        var parameters3 = new { PmolId = pmolParameter.Id };
        await using (var connection = new SqlConnection(connectionString))
        {
            currentUserTeamRoles = connection.Query<PmolTeamRoleReadDto>(query2, parameters2).ToList();
            pmolPlannedWorkLabourList = connection.Query<PlannedWorkLabourOfPMol>(query3, parameters3).ToList();
            connection.Close();
        }

        PmolTeamRoleReadDto currentWorker = null;
        foreach (var pmolPlannedWorkLabour in pmolPlannedWorkLabourList)
        foreach (var currentUserTeamRole in currentUserTeamRoles)
            if (currentUserTeamRole.PmolLabourId == pmolPlannedWorkLabour.Id)
                currentWorker = currentUserTeamRole;

        var labour = currentWorker;

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            dbConnection.Execute(
                @"INSERT INTO dbo.PmolLabourBreak ( Id ,LabourId ,StartDateTime ,IsBreak ) VALUES ( NEWID() ,@LabourId ,@StartDateTime , 1 )",
                new { StartDateTime = DateTime.UtcNow, LabourId = labour.Id });
            dbConnection.Execute(@"update PmolLabourTime set IsBreak = 1 where LabourId = @LabourId",
                new { LabourId = labour.Id });

            
            return pmolParameter.Id;
        }
    }

    public async Task<bool> IsForeman(PmolParameter pmolParameter)
    {
        var isExist = false;

        var connectionString1 = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        // var cuConnection = pmolParameter.TenantProvider.cuSqlConnection();
        // var projectConnection = pmolParameter.TenantProvider.projectSqlConnection();
        var orgSqlConnection = pmolParameter.TenantProvider.orgSqlConnection();

        var query = "select * from Pmol where Id = @Id and ForemanId = @ForemanId";

        var query2 = "select * from CabPersonCompany where Oid =@Oid";

        PmolGetByIdDto currentPmol;
        CabDataDapperDto cabPerson;

        var parameters2 = new { Oid = pmolParameter.UserId };
        // await using (var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
        // {
        cabPerson = orgSqlConnection.Query<CabDataDapperDto>(query2, parameters2).FirstOrDefault();
        //connection.Close();
        // }

        var parameters = new { pmolParameter.Id, ForemanId = cabPerson.PersonId };

        await using (var connection = new SqlConnection(connectionString1))
        {
            currentPmol = connection.Query<PmolGetByIdDto>(query, parameters).FirstOrDefault();
            //connection.Close();
        }

        if (currentPmol != null)
        {
            isExist = true;
            return isExist;
        }

        return isExist;
    }

    public async Task<string> UpdatePmolStart(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        await using var tenetConnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

        var cabPerson = tenetConnection
            .Query<CabDataDapperDto>("select * from CabPersonCompany where Oid =@Oid",
                new { Oid = pmolParameter.UserId }).FirstOrDefault();
        connection.Close();


        var date = DateTime.UtcNow;

        if (pmolParameter.PmolLabours.IsForeman)
        {
            var query = "update PMol set StartDateTime = @Date, IsBreak = 0 where Id = @Id";
            var param = new { Id = pmolParameter.PmolLabours.PmolId, Date = date };

            var affectedRows = connection.QuerySingleOrDefaultAsync(query, param).Result;
        }

        var selectlabour =
            "SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.CabPersonId = @PersonId AND PmolTeamRole.IsDeleted = 0";
        var insertSql =
            @"INSERT INTO dbo.PmolLabourTime ( Id ,LabourId ,StartDateTime ,IsBreak , Type ) VALUES ( @Id ,@LabourId ,@StartDateTime  ,@IsBreak , @Type )";

        var labour = connection
            .Query<PmolTeamRole>(selectlabour, new { pmolParameter.PmolLabours.PmolId, cabPerson.PersonId })
            .FirstOrDefault();

        var updateSql =
            @"Update dbo.PmolLabourTime Set EndDateTime = @EndDateTime Where Id = @Id";

        if (labour != null)
        {
            var itemList =
                connection.Query<PmolLabourTime>(
                    "Select * From PmolLabourTime Where LabourId = @LabourId Order By StartDateTime desc",
                    new { LabourId = labour.Id }).FirstOrDefault();

            if (itemList != null) await connection.ExecuteAsync(updateSql, new { itemList.Id, EndDateTime = date });

            var param2 = new
            {
                Id = Guid.NewGuid().ToString(),
                LabourId = labour.Id,
                StartDateTime = date,
                pmolParameter.PmolLabours.IsBreak,
                pmolParameter.PmolLabours.Type
            };

            await connection.ExecuteAsync(insertSql, param2);
        }


        return pmolParameter.PmolLabours.PmolId;
    }

    public async Task<string> UpdatePmolStop(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        await using var tenetConnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

        var cabPerson = tenetConnection
            .Query<CabDataDapperDto>("select * from CabPersonCompany where Oid =@Oid",
                new { Oid = pmolParameter.UserId }).FirstOrDefault();


        var date = DateTime.UtcNow;

        if (pmolParameter.PmolLabours.IsForeman)
        {
            var query = "update PMol set EndDateTime = @Date, IsBreak = 0 where Id = @Id";
            var param = new { pmolParameter.Id, Date = date };

            await connection.QuerySingleOrDefaultAsync(query, param);
        }

        var selectlabour =
            "SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.CabPersonId = @PersonId AND PmolTeamRole.IsDeleted = 0";
        var updateSql =
            @"Update dbo.PmolLabourTime Set EndDateTime = @EndDateTime Where Id = @Id";

        var labour = connection
            .Query<PmolTeamRole>(selectlabour, new { PmolId = pmolParameter.Id, cabPerson.PersonId })
            .FirstOrDefault();

        var insertSql =
            @"INSERT INTO dbo.PmolLabourTime ( Id ,LabourId ,EndDateTime ,IsBreak , Type ) VALUES ( @Id ,@LabourId ,@EndDateTime  ,@IsBreak , @Type )";


        if (labour != null)
        {
            var itemList =
                connection.Query<PmolLabourTime>(
                    "Select * From PmolLabourTime Where LabourId = @LabourId Order By StartDateTime desc",
                    new { LabourId = labour.Id }).FirstOrDefault();

            if (itemList != null) await connection.ExecuteAsync(updateSql, new { itemList.Id, EndDateTime = date });

            var param2 = new
            {
                Id = Guid.NewGuid().ToString(),
                LabourId = labour.Id,
                EndDateTime = date,
                pmolParameter.PmolLabours.IsBreak,
                Type = "8"
            };

            await connection.ExecuteAsync(insertSql, param2);
        }

        // if (labour != null)
        // {
        //     var param2 = new
        //     {
        //         
        //         LabourId = labour.Id,
        //         EndDateTime = date,
        //     };
        //
        //     await connection.ExecuteAsync(updateSql, param2);
        // }

        var queryTime = @"SELECT
  CONVERT(TIME, CONVERT(DATETIME, EndDateTime) - CONVERT(DATETIME, StartDateTime)) AS TotalTime,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0) AS StartDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0) AS EndDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, CONVERT(TIME, CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0)) - CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0))))) / 15) * 15, 0) AS TotalTimeRoundNearestDateFormat
 , DATEDIFF(MINUTE, EndDateTime, StartDateTime) AS dif
FROM dbo.PmolLabourTime WHERE LabourId = @Id AND Type NOT IN ('8','6') ORDER BY StartDateTime ASC";

        var queryBreakTime = @"SELECT * FROM dbo.PmolLabourTime WHERE LabourId = @Id AND Type = '6'";

        var updateResource =
            @"UPDATE dbo.PMolPlannedWorkLabour SET ConsumedQuantity = @ConsumedQuantity WHERE Id = @Id";

        var UpdateTeam = @"UPDATE dbo.PmolTeamRole SET ConsumedQuantity =  @ConsumedQuantity WHERE Id = @Id";

        var param1 = new { pmolParameter.Id, Date = date };

        //var orderDictionary = new Dictionary<string, PmolResourceLabourDto>();


        var listLabourResource = connection.Query<PmolResourceLabourDto>(
            @"SELECT * FROM dbo.PMolPlannedWorkLabour WHERE PMolPlannedWorkLabour.PmolId = @Id", param1);

        foreach (var laborR in listLabourResource)
        {
            double subTotal = 0;

            laborR.Team = connection
                .Query<PmolTeamRoleReadDto>(
                    @"SELECT PmolTeamRole.* FROM dbo.PmolTeamRole  WHERE PmolLabourId = @PmolLabourId",
                    new { PmolLabourId = laborR.Id }).ToList();


            foreach (var labort in laborR.Team)
            {
                double totalUnits = 0;

                var PmolTime = connection.Query<PmolTime>(queryTime, new { labort.Id });
                var PmolBreakTime = connection.Query<PmolBreakCal>(queryBreakTime, new { labort.Id });
                //var kk = PmolTime.EndDateTimeRoundNearest.Subtract(PmolTime.StartDateTimeRoundNearest);

                var diff = new TimeSpan();
                foreach (var b in PmolTime)
                {
                    var d = b.EndDateTimeRoundNearest.Subtract(b.StartDateTimeRoundNearest);
                    diff = diff + d;
                    //diffAll.add
                }

                // var diff = PmolTime.LastOrDefault().EndDateTimeRoundNearest
                //     .Subtract(PmolTime.FirstOrDefault().StartDateTimeRoundNearest);

                var diffAll = new TimeSpan();
                foreach (var b in PmolBreakTime)
                    if (b.EndDateTime != null)
                    {
                        var d = b.EndDateTime.Subtract(b.StartDateTime);
                        diffAll = diffAll + d;
                        //diffAll.add
                    }

                var diffBreak = diffAll.RoundToNearestMinutes(15);

                diff = diff.Subtract(diffBreak);


                // var diffBreak = diffAll.RoundToNearestMinutes(15);
                //
                // diff = diff.Subtract(diffBreak);

                if (diff != null)
                {
                    if (diff.Days <= 0)
                    {
                        totalUnits += diff.Hours;
                        var m = diff.Minutes / 60.0;
                        totalUnits += m;
                    }
                    else
                    {
                        totalUnits = -1;
                    }
                }

                connection.Execute(UpdateTeam, new { ConsumedQuantity = totalUnits, labort.Id });

                subTotal += totalUnits;
            }

            connection.Execute(updateResource,
                new
                {
                    ConsumedQuantity = subTotal, laborR.Id
                });
        }


        return pmolParameter.PmolLabours.PmolId;
    }

    public async Task<string> UpdateUserCurrentPmol(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        await using var tenetConnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

        var cabPerson = tenetConnection
            .Query<CabDataDapperDto>("select * from CabPersonCompany where Oid =@Oid",
                new { Oid = pmolParameter.UserId }).FirstOrDefault();
        connection.Close();


        var currentPmol = tenetConnection
            .Query<UserCurrentPmol>("Select * From UserCurrentPmol Where CabPersonId = @CabPersonId ",
                new { CabPersonId = cabPerson.PersonId }).FirstOrDefault();

        if (currentPmol != null)
        {
            await tenetConnection.ExecuteAsync(
                "Update UserCurrentPmol Set PmolId = @PmolId, ProjectSequenceId = @ProjectSequenceId Where CabPersonId = @CabPersonId",
                new
                {
                    PmolId = pmolParameter.Id,
                    pmolParameter.ProjectSequenceId,
                    CabPersonId = cabPerson.PersonId
                });
        }
        else
        {
            var insertQuery = @"INSERT INTO dbo.UserCurrentPmol
                                        (
                                         Id
                                         ,PmolId
                                         ,CabPersonId
                                         ,ProjectSequenceId
                                        )
                                        VALUES
                                        (
                                         @Id
                                         ,@PmolId
                                         ,@CabPersonId
                                         ,@ProjectSequenceId
                                        )";

            var param = new
            {
                Id = Guid.NewGuid().ToString(),
                PmolId = pmolParameter.Id,
                CabPersonId = cabPerson.PersonId,
                pmolParameter.ProjectSequenceId
            };

            await tenetConnection.ExecuteAsync(insertQuery, param);
        }


        return pmolParameter.Id;
    }

    public async Task<string> UpdatePmolLabourEndTime(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);


        var insertSql =
            @"INSERT INTO dbo.PmolLabourTime ( Id ,LabourId ,EndDateTime ,IsBreak , Type ) VALUES ( @Id ,@LabourId ,@EndDateTime  ,@IsBreak , @Type )";

        var updateSql =
            @"Update dbo.PmolLabourTime Set EndDateTime = @EndDateTime Where Id = @Id";


        if (pmolParameter.PmolLabourEndTime.LabourId != null)
        {
            var itemList =
                connection.Query<PmolLabourTime>(
                    "Select * From PmolLabourTime Where LabourId = @LabourId Order By StartDateTime desc",
                    new { pmolParameter.PmolLabourEndTime.LabourId }).FirstOrDefault();

            if (itemList != null)
                await connection.ExecuteAsync(updateSql,
                    new { pmolParameter.PmolLabourEndTime.EndDateTime, itemList.Id });

            var param2 = new
            {
                Id = Guid.NewGuid().ToString(),
                pmolParameter.PmolLabourEndTime.LabourId,
                pmolParameter.PmolLabourEndTime.EndDateTime,
                IsBreak = 0,
                Type = "8"
            };

            await connection.ExecuteAsync(insertSql, param2);

            var queryTime = @"SELECT
  CONVERT(TIME, CONVERT(DATETIME, EndDateTime) - CONVERT(DATETIME, StartDateTime)) AS TotalTime,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0) AS StartDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0) AS EndDateTimeRoundNearest
 ,DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, CONVERT(TIME, CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, EndDateTime)) / 15) * 15, 0)) - CONVERT(DATETIME, DATEADD(MINUTE, (DATEDIFF(MINUTE, 0, DATEADD(SECOND, (15 * 60) / 2, StartDateTime)) / 15) * 15, 0))))) / 15) * 15, 0) AS TotalTimeRoundNearestDateFormat
 , DATEDIFF(MINUTE, EndDateTime, StartDateTime) AS dif
FROM dbo.PmolLabourTime WHERE LabourId = @Id AND Type NOT IN ('8','6') ORDER BY StartDateTime ASC";

            var queryBreakTime = @"SELECT * FROM dbo.PmolLabourTime WHERE LabourId = @Id AND Type = '6'";

            var updateResource =
                @"UPDATE dbo.PMolPlannedWorkLabour SET ConsumedQuantity = @ConsumedQuantity WHERE Id = @Id";

            var UpdateTeam = @"UPDATE dbo.PmolTeamRole SET ConsumedQuantity =  @ConsumedQuantity WHERE Id = @Id";

            var param1 = new { Id = pmolParameter.PmolLabourEndTime.LabourId };

            //var orderDictionary = new Dictionary<string, PmolResourceLabourDto>();


            var listLabourResource = connection.Query<PmolResourceLabourDto>(
                @"SELECT * FROM dbo.PMolPlannedWorkLabour WHERE PMolPlannedWorkLabour.PmolId = @Id", param1);

            foreach (var laborR in listLabourResource)
            {
                double subTotal = 0;

                laborR.Team = connection
                    .Query<PmolTeamRoleReadDto>(
                        @"SELECT PmolTeamRole.* FROM dbo.PmolTeamRole  WHERE PmolLabourId = @PmolLabourId",
                        new { PmolLabourId = laborR.Id }).ToList();


                foreach (var labort in laborR.Team)
                {
                    double totalUnits = 0;

                    var PmolTime = connection.Query<PmolTime>(queryTime, new { labort.Id });
                    var PmolBreakTime = connection.Query<PmolBreakCal>(queryBreakTime, new { labort.Id });
                    //var kk = PmolTime.EndDateTimeRoundNearest.Subtract(PmolTime.StartDateTimeRoundNearest);

                    // var diff = new TimeSpan();
                    // foreach (var b in PmolTime)
                    // {
                    //
                    //     var d = b.EndDateTimeRoundNearest.Subtract(b.StartDateTimeRoundNearest);
                    //     diff = diff + d;
                    //     //diffAll.add
                    // }

                    var diff = PmolTime.LastOrDefault().EndDateTimeRoundNearest
                        .Subtract(PmolTime.FirstOrDefault().StartDateTimeRoundNearest);

                    var diffAll = new TimeSpan();
                    foreach (var b in PmolBreakTime)
                        if (b.EndDateTime != null)
                        {
                            var d = b.EndDateTime.Subtract(b.StartDateTime);
                            diffAll = diffAll + d;
                            //diffAll.add
                        }

                    var diffBreak = diffAll.RoundToNearestMinutes(15);

                    diff = diff.Subtract(diffBreak);


                    // var diffBreak = diffAll.RoundToNearestMinutes(15);
                    //
                    // diff = diff.Subtract(diffBreak);

                    if (diff != null)
                    {
                        if (diff.Days <= 0)
                        {
                            totalUnits += diff.Hours;
                            var m = diff.Minutes / 60.0;
                            totalUnits += m;
                        }
                        else
                        {
                            totalUnits = -1;
                        }
                    }

                    connection.Execute(UpdateTeam, new { ConsumedQuantity = totalUnits, labort.Id });

                    subTotal += totalUnits;
                }

                connection.Execute(updateResource,
                    new
                    {
                        ConsumedQuantity = subTotal, laborR.Id
                    });
            }
        }


        return pmolParameter.PmolLabourEndTime.LabourId;
    }

    public async Task<IEnumerable<PmolPlannedWorkLabourDto>> GetPMolPlannedWorkLabour(PmolParameter pmolParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            var selectlabour = @"SELECT
                              PmolTeamRole.Id
                             ,PmolTeamRole.CabPersonId
                             ,PmolTeamRole.RoleId
                             ,PmolTeamRole.RequiredQuantity
                             ,PmolTeamRole.ConsumedQuantity
                             ,PmolTeamRole.Type
                             ,PmolTeamRole.PmolLabourId
                             ,PmolTeamRole.IsDeleted
                             ,PmolTeamRole.IsJobDone
                             ,PmolTeamRole.Message
                            FROM dbo.PmolTeamRole 
                            LEFT OUTER JOIN PMolPlannedWorkLabour ppwl ON PmolTeamRole.PmolLabourId = ppwl.Id
                            LEFT OUTER JOIN PMol p ON ppwl.PmolId = p.Id
                            WHERE p.ProjectMoleculeId = @Id";

            var labour = connection.Query<PmolTeamRole>(selectlabour, new { pmolParameter.Id });

            var mPmolPlannedWorkLabourDto = new List<PmolPlannedWorkLabourDto>();

            await using (var dbconnection =
                         new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString))
            {
                foreach (var i in labour)
                {
                    var name = dbconnection.Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Id",
                        new { Id = i.CabPersonId }).FirstOrDefault();

                    var person = new PmolPlannedWorkLabourDto();

                    person.Id = i.CabPersonId;
                    person.Name = name;
                    person.RoleId = i.RoleId;

                    mPmolPlannedWorkLabourDto.Add(person);
                }
            }

            return mPmolPlannedWorkLabourDto;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<PmolData>> GetPmolByPerson(PmolParameter pmolParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            var selectpmol = @"SELECT
                                PMol.*
                                    FROM dbo.PMolPlannedWorkLabour
                                    INNER JOIN dbo.PMol
                                    ON PMolPlannedWorkLabour.PmolId = PMol.Id
                                INNER JOIN dbo.PmolTeamRole
                                    ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id
                                WHERE Pmol.ExecutionDate = @Date AND PmolTeamRole.CabPersonId = @CabPersonId";

            var pmoldata = connection.Query<PmolData>(selectpmol,
                new
                {
                    date = pmolParameter.GetPmolByPersonDto.ExecutionDate, pmolParameter.GetPmolByPersonDto.CabPersonId
                }).ToList();

            var vehical =
                @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0";

            var tool =
                @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0 AND CorporateProductCatalog.ResourceFamilyId != '0c355800-91fd-4d99-8010-921a42f0ba04'";

            var pmolDatalist = new List<PmolData>();

            IEnumerable<PomlVehicle> vehicals;
            IEnumerable<PomlVehicle> tools;

            foreach (var i in pmoldata)
            {
                vehicals = connection.Query<PomlVehicle>(vehical,
                    new { i.Id, date = pmolParameter.GetPmolByPersonDto.ExecutionDate }).ToList();
                tools = connection
                    .Query<PomlVehicle>(tool, new { i.Id, date = pmolParameter.GetPmolByPersonDto.ExecutionDate })
                    .ToList();

                var pmolData1 = new PmolData();

                pmolData1 = i;

                if (vehicals.Any()) pmolData1.PomlVehical = (List<PomlVehicle>)vehicals;

                if (tools.Any()) pmolData1.PomlTool = (List<PomlVehicle>)tools;

                pmolDatalist.Add(pmolData1);
            }

            return pmolDatalist;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<TrAppDto>> GetTrAppData(PmolParameter pmolParameter)
    {
        
        await using var connection =  pmolParameter.TenantProvider.projectSqlConnection();
        await using var cuconnection =  pmolParameter.TenantProvider.cuSqlConnection();
        await using var dbconnection =  pmolParameter.TenantProvider.orgSqlConnection();

        var userId = dbconnection.Query<string>("SELECT Oid FROM dbo.CabPersonCompany WHERE PersonId = @Id",
            new { Id = pmolParameter.GetPmolByPersonDto.CabPersonId }).FirstOrDefault();

        var sql =
            @"SELECT TimeClock.StartDateTime AS StartTime ,TimeClock.EndDateTime AS EndTime ,TimeClock.PmolId ,TimeClock.Type AS TypeId ,TimeClock.UserId ,Location.Latitude ,Location.Longitude FROM dbo.TimeClock LEFT OUTER JOIN dbo.Location ON TimeClock.LocationId = Location.Id ORDER BY TimeClock.StartDateTime ASC";

        var trAppAll = cuconnection.Query<TrAppDto>(sql).ToList();

        trAppAll = trAppAll.Where(e =>
            e.UserId == userId && e.StartTime.Date == pmolParameter.GetPmolByPersonDto.ExecutionDate).ToList();

        var TrAppData = new List<TrAppDto>();

        foreach (var i in trAppAll)
        {
            var trApp = new TrAppDto();
            if (i.TypeId == "0")
                if (i.PmolId != null)
                {
                    var options = new DbContextOptions<ProjectDefinitionDbContext>();
                    var context = new ProjectDefinitionDbContext(options, pmolParameter.TenantProvider);

                    var pmol = connection.Query<Pmol>("SELECT * FROM dbo.PMol WHERE Id = @PmolId", new { i.PmolId })
                        .FirstOrDefault();

                    var MapLocation = context.MapLocation.Where(L => L.Id == pmol.LocationId).Include(m => m.Address)
                        .Include(m => m.Position)
                        .ToList().FirstOrDefault();
                    if (MapLocation == null)
                    {
                        var projectDef =
                            context.ProjectDefinition.FirstOrDefault(p =>
                                p.SequenceCode == pmolParameter.ProjectSequenceId);
                        if (projectDef != null)
                            MapLocation = context.MapLocation.Where(l => l.Id == projectDef.LocationId)
                                .Include(m => m.Address).Include(m => m.Position)
                                .ToList().FirstOrDefault();
                    }

                    var tool =
                        @"SELECT CorporateProductCatalog.ResourceTitle ,CorporateProductCatalog.Id FROM dbo.PMolPlannedWorkTools LEFT OUTER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMolPlannedWorkTools.PmolId = @PmolId AND CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04'";

                    var vechical = connection.Query<CorporateProductCatalog>(tool, new { i.PmolId }).FirstOrDefault();

                    trApp.Destination = MapLocation.Position;
                    trApp.CpcId = vechical.Id;
                    trApp.CpcTitle = vechical.ResourceTitle;
                }

            var sPosition = new Position();

            sPosition.Lat = i.Latitude;
            sPosition.Lon = i.Longitude;

            trApp.TypeId = i.TypeId;
            trApp.PmolId = i.PmolId;
            trApp.StartPoint = sPosition;
            trApp.StartTime = i.StartTime;
            trApp.EndTime = i.EndTime;
            trApp.ExecutionDate = pmolParameter.GetPmolByPersonDto.ExecutionDate.ToString();

            TrAppData.Add(trApp);
        }

        return TrAppData;
    }

    public async Task<string> CreatePersonCommentCard(PmolParameter pmolParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);
            await using var dbconnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

            if (pmolParameter.PmolPersonCommentCardDto.Id == null)
            {
                var pmolSelect = @"SELECT * FROM dbo.PMol WHERE Id = @Id";

                var pmolData =
                    connection.Query<Pmol>(pmolSelect, new { Id = pmolParameter.PmolPersonCommentCardDto.PmolId })
                        .FirstOrDefault();

                var creater = dbconnection
                    .Query<string>("SELECT PersonId FROM dbo.CabPersonCompany WHERE Oid = @UserId",
                        new { pmolParameter.UserId }).FirstOrDefault();

                var param = new
                {
                    Id = Guid.NewGuid(),
                    Creater = creater,
                    ActivityType = pmolData.PmolType,
                    ActivityName = pmolData.Title,
                    Date = DateTime.UtcNow,
                    IsAccept = "1",
                    pmolParameter.PmolPersonCommentCardDto.PmolId,
                    pmolParameter.PmolPersonCommentCardDto.CabPersonId
                };

                var createCard =
                    @"INSERT INTO dbo.PmolPersonCommentCard ( Id ,Creater ,ActivityType ,ActivityName ,Date ,PmolId ,CabPersonId ) VALUES ( @Id ,@Creater ,@ActivityType ,@ActivityName ,@Date ,@PmolId ,@CabPersonId );";

                await connection.ExecuteAsync(createCard, param);
            }

            else
            {
                var updateCard =
                    @"UPDATE dbo.PmolPersonCommentCard SET Id = @Id ,Creater = @Creater ,ActivityType = @ActivityType ,ActivityName = @ActivityName ,Comments = @Comments ,IsAccept = @IsAccept ,PmolId = @PmolId ,CabPersonId = @CabPersonId WHERE Id = @Id ;";

                var param = new
                {
                    pmolParameter.PmolPersonCommentCardDto.Id,
                    Creater = pmolParameter.UserId,
                    pmolParameter.PmolPersonCommentCardDto.ActivityType,
                    pmolParameter.PmolPersonCommentCardDto.ActivityName,
                    pmolParameter.PmolPersonCommentCardDto.Comments,
                    pmolParameter.PmolPersonCommentCardDto.IsAccept,
                    pmolParameter.PmolPersonCommentCardDto.PmolId,
                    pmolParameter.PmolPersonCommentCardDto.CabPersonId
                };

                await connection.ExecuteAsync(updateCard, param);
            }

            return pmolParameter.PmolPersonCommentCardDto.PmolId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<PmolPersonCommentCardDto>> GetPersonCommentCard(PmolParameter pmolParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);
            await using var dbconnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

            var mPmolPersonCommentCardDto = new List<PmolPersonCommentCardDto>();

            var getComment = @"SELECT
                                  Id
                                 ,Creater
                                 ,ActivityType
                                 ,ActivityName
                                 ,Comments
                                 ,Date
                                 ,IsAccept
                                 ,PmolId
                                 ,CabPersonId
                                FROM dbo.PmolPersonCommentCard WHERE PmolId = @PmolId AND CabPersonId Is Not Null";

            var mCommentCardData = connection.Query<PmolPersonCommentCardDto>(getComment,
                new
                {
                    pmolParameter.PmolPersonCommentCardDto.CabPersonId, pmolParameter.PmolPersonCommentCardDto.PmolId
                }).ToList();

            var mCommentData = connection
                .Query<PmolPersonCommentDto>(
                    "SELECT Id ,CommentCardId ,Comment ,CommentBy ,CommentTo ,Date FROM dbo.PmolPersonComment;")
                .ToList();

            foreach (var i in mCommentCardData)
            {
                var commentCard = new PmolPersonCommentCardDto();

                var comment = mCommentData.Where(e => e.CommentCardId == i.Id).ToList();

                commentCard = i;
                commentCard.CreaterName = dbconnection
                    .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Creater", new { i.Creater })
                    .FirstOrDefault();
                commentCard.CabPersonName = dbconnection
                    .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @CabPersonId", new { i.CabPersonId })
                    .FirstOrDefault();

                var mPersonComment = new List<PmolPersonCommentDto>();
                foreach (var n in comment)
                {
                    var personCommet = new PmolPersonCommentDto();

                    personCommet.CommentByName = dbconnection
                        .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Creater",
                            new { Creater = n.CommentBy })
                        .FirstOrDefault();
                    personCommet.CommentToName = dbconnection
                        .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Creater",
                            new { Creater = n.CommentTo })
                        .FirstOrDefault();
                    personCommet.Id = n.Id;
                    personCommet.Comment = n.Comment;
                    personCommet.Date = n.Date;
                    personCommet.CommentCardId = n.CommentCardId;
                    personCommet.CommentBy = n.CommentBy;
                    personCommet.CommentTo = n.CommentTo;
                    personCommet.IsAccept = n.IsAccept;

                    mPersonComment.Add(personCommet);
                }

                commentCard.PersonComment = mPersonComment;

                mPmolPersonCommentCardDto.Add(commentCard);
            }

            return mPmolPersonCommentCardDto;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<PmolPersonCommentCardDto>> GetPmolCommentCard(PmolParameter pmolParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);
            await using var dbconnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

            var mPmolPersonCommentCardDto = new List<PmolPersonCommentCardDto>();

            var getComment = @"SELECT
                                  Id
                                 ,Creater
                                 ,ActivityType
                                 ,ActivityName
                                 ,Comments
                                 ,Date
                                 ,IsAccept
                                 ,PmolId
                                 ,CabPersonId
                                FROM dbo.PmolPersonCommentCard WHERE PmolId = @PmolId AND CabPersonId Is Null";

            var mCommentCardData =
                connection.Query<PmolPersonCommentCardDto>(getComment,
                    new { pmolParameter.PmolPersonCommentCardDto.PmolId });

            var mCommentData = connection
                .Query<PmolPersonCommentDto>(
                    "SELECT Id ,CommentCardId ,Comment ,CommentBy ,CommentTo ,Date FROM dbo.PmolPersonComment;")
                .ToList();

            foreach (var i in mCommentCardData)
            {
                var commentCard = new PmolPersonCommentCardDto();

                var comment = mCommentData.Where(e => e.CommentCardId == i.Id).ToList();

                commentCard = i;
                commentCard.CreaterName = dbconnection
                    .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Creater", new { i.Creater })
                    .FirstOrDefault();
                commentCard.CabPersonName = dbconnection
                    .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @CabPersonId", new { i.CabPersonId })
                    .FirstOrDefault();

                var mPersonComment = new List<PmolPersonCommentDto>();
                foreach (var n in comment)
                {
                    var personCommet = new PmolPersonCommentDto();

                    personCommet.CommentByName = dbconnection
                        .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Creater",
                            new { Creater = n.CommentBy })
                        .FirstOrDefault();
                    personCommet.CommentToName = dbconnection
                        .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Creater",
                            new { Creater = n.CommentTo })
                        .FirstOrDefault();
                    personCommet.Id = n.Id;
                    personCommet.Comment = n.Comment;
                    personCommet.Date = n.Date;
                    personCommet.CommentCardId = n.CommentCardId;
                    personCommet.CommentBy = n.CommentBy;
                    personCommet.CommentTo = n.CommentTo;
                    personCommet.IsAccept = n.IsAccept;

                    mPersonComment.Add(personCommet);
                }

                commentCard.PersonComment = mPersonComment;

                mPmolPersonCommentCardDto.Add(commentCard);
            }

            return mPmolPersonCommentCardDto;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<PmolDataForPrint>> PmolDataForPrint(PmolParameter pmolParameter)
    {
        try
        {
            await using var dbconnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);
            var db = new List<ProjectDefinition>();
            var pmol = new List<PmolDataForPrint>();

            var selectProject =
                @"SELECT ProjectDefinition.Id, ProjectDefinition.Title,ProjectDefinition.SequenceCode,ProjectDefinition.ProjectConnectionString ,ProjectDefinition.ProjectManagerId ,CabCompany.SequenceCode AS ContractingUnitId,ProjectDefinition.LocationId,ProjectDefinition.SiteManagerId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId  WHERE ProjectDefinition.IsDeleted = 0 AND ProjectClassification.ProjectClassificationBuisnessUnit = @BuId ORDER BY ProjectDefinition.SequenceCode";

            var cabPerson = dbconnection.Query<CabPerson>("SELECT * FROM dbo.CabPerson ORDER BY FullName;").ToList();

            db = dbconnection.Query<ProjectDefinition>(selectProject, new { pmolParameter.PmolDataForPrintDto.BuId })
                .ToList();

            var vehical =
                @"SELECT CorporateProductCatalog.Title FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMolPlannedWorkTools.IsDeleted = 0 AND PMolPlannedWorkTools.PmolId = @PmolId ORDER BY CorporateProductCatalog.Title";

            var material =
                @"SELECT CorporateProductCatalog.Title FROM dbo.PMolPlannedWorkMaterial INNER JOIN dbo.PMol ON PMolPlannedWorkMaterial.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkMaterial.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMolPlannedWorkMaterial.IsDeleted = 0 AND PMolPlannedWorkMaterial.PmolId = @PmolId ORDER BY CorporateProductCatalog.Title";

            var teamQuery =
                @"SELECT PmolTeamRole.CabPersonId As Id FROM dbo.PMolPlannedWorkLabour LEFT OUTER JOIN dbo.PmolTeamRole ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 AND PmolTeamRole.CabPersonId IS NOT NULL AND PMolPlannedWorkLabour.PmolId = @PmolId";

            if (db.Any())
            {
                foreach (var project in db)
                {
                    await using var connection = new SqlConnection(project.ProjectConnectionString);

                    var siteManager = cabPerson.FirstOrDefault(e => e.Id == project.SiteManagerId)?.FullName;

                    string pmolSelect =
                        @"SELECT Id ,Title AS ProjectMolecule ,@Project AS Project ,@SiteManager AS SiteManager,LocationId AS Address,ExecutionStartTime AS Hour FROM dbo.PMol WHERE ExecutionDate = @Date ORDER BY ExecutionStartTime,Title";

                    var pmolData = connection.Query<PmolDataForPrint>(pmolSelect,
                        new
                        {
                            pmolParameter.PmolDataForPrintDto.Date, Project = project.Title, SiteManager = siteManager
                        }).ToList();

                    foreach (var i in pmolData)
                    {
                        i.Vehicle = connection.Query<string>(vehical, new { PmolId = i.Id }).ToList();

                        i.Material = connection.Query<string>(material, new { PmolId = i.Id }).ToList();

                        var options = new DbContextOptions<ShanukaDbContext>();

                        var context = new ShanukaDbContext(options,
                            pmolParameter.TenantProvider.GetTenant().ConnectionString, pmolParameter.TenantProvider);

                        var context2 = new ShanukaDbContext(options, project.ProjectConnectionString,
                            pmolParameter.TenantProvider);

                        var mapLocation = context2.MapLocation.Where(L => L.Id == i.Address).Include(m => m.Address)
                            .Include(m => m.Position)
                            .ToList().FirstOrDefault();

                        if (mapLocation != null)
                        {
                            i.Address = mapLocation.Address.FreeformAddress;
                        }

                        else
                        {
                            mapLocation = context.MapLocation.Where(L => L.Id == project.LocationId)
                                .Include(m => m.Address)
                                .Include(m => m.Position)
                                .ToList().FirstOrDefault();

                            i.Address = mapLocation?.Address.FreeformAddress;
                        }


                        var team = connection.Query<string>(teamQuery, new { PmolId = i.Id }).ToList();

                        i.Names = cabPerson.Where(e => team.Contains(e.Id)).ToList().Select(e => e.FullName).ToList();
                    }

                    pmol.AddRange(pmolData);
                }
            }

            return pmol;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> AddPersonComment(PmolParameter pmolParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);
            await using var dbconnection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

            var creater = dbconnection
                .Query<string>("SELECT PersonId FROM dbo.CabPersonCompany WHERE Oid = @UserId",
                    new { pmolParameter.UserId }).FirstOrDefault();

            if (pmolParameter.PmolPersonComment.Id == null)
            {
                var addComment =
                    @"INSERT INTO dbo.PmolPersonComment ( Id ,CommentCardId ,Comment ,CommentBy ,CommentTo ,Date,IsAccept ) VALUES ( @Id ,@CommentCardId ,@Comment ,@CommentBy ,@CommentTo ,@Date,@IsAccept );";

                var param = new
                {
                    Id = Guid.NewGuid(),
                    pmolParameter.PmolPersonComment.CommentCardId,
                    pmolParameter.PmolPersonComment.Comment,
                    CommentBy = creater,
                    pmolParameter.PmolPersonComment.CommentTo,
                    Date = DateTime.UtcNow,
                    pmolParameter.PmolPersonComment.IsAccept
                };

                await connection.ExecuteAsync(addComment, param);
            }

            else
            {
                var addComment =
                    @"UPDATE dbo.PmolPersonComment SET CommentCardId = @CommentCardId ,Comment = @Comment ,CommentBy = @CommentBy ,CommentTo = @CommentTo ,Date = @Date ,IsAccept = @IsAccept WHERE Id = @Id;";

                var param = new
                {
                    pmolParameter.PmolPersonComment.Id,
                    pmolParameter.PmolPersonComment.CommentCardId,
                    pmolParameter.PmolPersonComment.Comment,
                    CommentBy = creater,
                    pmolParameter.PmolPersonComment.CommentTo,
                    Date = DateTime.UtcNow,
                    pmolParameter.PmolPersonCommentCardDto.IsAccept
                };

                await connection.ExecuteAsync(addComment, param);
            }

            return pmolParameter.PmolPersonCommentCardDto.PmolId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> PmolStatusUpdate(PmolParameter pmolParameter, string statusId, string pmolId)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
                pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                "UPDATE dbo.PMol SET StatusId = @StatusId WHERE ProjectMoleculeId = @ProjectMoleculeId ;",
                new { ProjectMoleculeId = pmolId, StatusId = statusId });

            if (statusId == "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da") // Approve pmol
            {
                var pmolType = connection
                    .Query<string>("SELECT PmolType FROM PMol WHERE ProjectMoleculeId = @Id", new { Id = pmolId })
                    .FirstOrDefault();

                var consumableSql =
                    @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                        PmolTypeId,PmolType
                                       ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                        ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                    SELECT
                                     NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkConsumable.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkConsumable.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkConsumable.ConsumedQuantity * CorporateProductCatalog.InventoryPrice )
                                     ,0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkConsumable
                                      ON PMol.Id = PMolPlannedWorkConsumable.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkConsumable.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.ProjectMoleculeId = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkConsumable.IsDeleted = 0";

                var consumablParam = new
                {
                    projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolId
                };

                await connection.QueryAsync(consumableSql, consumablParam);

                var toolsSql =
                    @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                    PmolTypeId,PmolType
                                    ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                    ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                    SELECT
                                     NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkTools.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkTools.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkTools.ConsumedQuantity * CorporateProductCatalog.InventoryPrice ), 0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkTools
                                      ON PMol.Id = PMolPlannedWorkTools.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.ProjectMoleculeId = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkTools.IsDeleted = 0";

                var toolsParam = new
                {
                    projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolId
                };

                await connection.QueryAsync(toolsSql, toolsParam);

                var materialSql =
                    @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                       PmolTypeId,PmolType
                                       ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                       ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                       SELECT
                                       NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkMaterial.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkMaterial.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkMaterial.ConsumedQuantity * CorporateProductCatalog.InventoryPrice ), 0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkMaterial
                                      ON PMol.Id = PMolPlannedWorkMaterial.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkMaterial.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.ProjectMoleculeId = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkMaterial.IsDeleted = 0";

                var materialParam = new
                {
                    projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolId
                };

                await connection.QueryAsync(materialSql, materialParam);

                var labourSql =
                    @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle, 
                                     PmolTypeId,PmolType
                                     ,IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                     ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                    SELECT
                                     NEWID()
                                     ,@projectSq
                                     ,PbsProduct.Id AS ProductId
                                     ,PbsProduct.Title
                                     ,Bor.Id AS BorId
                                     ,Bor.Title
                                     ,PMol.Id 
                                     ,PMol.Title 
                                     ,NULL
                                     ,NULL
                                     ,CASE WHEN PMolPlannedWorkLabour.Type = 'Planned' THEN 1 ELSE 0 END
                                     ,PMol.StartDateTime
                                     ,CorporateProductCatalog.ResourceTypeId
                                     ,CpcResourceTypeLocalizedData.Label 
                                     ,PMolType.TypeId AS OriginalPmolTypeId
                                     ,PMolType.Name AS OriginalPmolType
                                     ,CorporateProductCatalog.ResourceNumber
									 ,CorporateProductCatalog.Title
                                     ,NULL
                                     ,NULL
                                     ,NULL
                                     ,PMolPlannedWorkLabour.ConsumedQuantity
                                     ,CorporateProductCatalog.CpcBasicUnitOfMeasureId
                                     ,CorporateProductCatalog.InventoryPrice
                                     ,(PMolPlannedWorkLabour.ConsumedQuantity * CorporateProductCatalog.InventoryPrice ), 0
                                    FROM dbo.PMol
                                    INNER JOIN dbo.Bor
                                      ON PMol.BorId = Bor.Id
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PMolPlannedWorkLabour
                                      ON PMol.Id = PMolPlannedWorkLabour.PmolId
                                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                                      ON PMolPlannedWorkLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                                    INNER JOIN dbo.CpcResourceTypeLocalizedData
                                      ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    WHERE PMol.ProjectMoleculeId = @pmolId
                                    AND PMolType.LanguageCode = @lang
                                    AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND PMolPlannedWorkLabour.IsDeleted = 0";

                var labourParam = new
                {
                    projectSq = pmolParameter.ProjectSequenceId, lang = pmolParameter.Lang, pmolId = pmolId
                };

                var serviceSql =
                    @"INSERT INTO ProjectCost (Id, ProjectSequenceCode, ProductId, ProductTitle, BorId, BorTitle, PmolId, PmolTitle,
                                    PmolTypeId, PmolType
                                    , IsPlannedResource, Date, ResourceTypeId, ResourceType, OriginalPmolTypeId, OriginalPmolType, ResourceNumber,
                                    ResourceTitle, BusinessId, PcTitle, PcStatus, ConsumedQuantity, Mou, CostMou, TotalCost, isUsed)
                                      SELECT
                                       NEWID()
                                       ,@projectSq
                                       ,PbsProduct.Id
                                       ,PbsProduct.Title
                                       ,NULL
                                       ,NULL
                                       ,PMol.Id
                                       ,PMol.Title
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,PMol.StartDateTime
                                        ,NULL
                                        ,NULL
                                       ,PMolType.TypeId AS OriginalPmolTypeId
                                       ,PMolType.Name AS OriginalPmolType
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,NULL
                                       ,PmolService.Quantity
                                       ,PmolService.MouId
                                       ,PmolService.UnitPrice
                                       ,PmolService.TotalPrice
                                       ,0
                                    FROM dbo.PMol
                                    LEFT OUTER JOIN dbo.PMolType
                                      ON PMol.TypeId = PMolType.TypeId
                                    LEFT OUTER JOIN dbo.PmolService
                                      ON PMol.Id = PmolService.PmolId
                                    LEFT OUTER JOIN dbo.PbsProduct
                                      ON PMol.ProductId = PbsProduct.Id
                                    WHERE PMolType.LanguageCode = @lang AND PMol.ProjectMoleculeId = @pmolId";

                await connection.QueryAsync(labourSql, labourParam);
                //  await connection.QueryAsync(serviceSql, labourParam);
                if (pmolType == "sub") await connection.QueryAsync(serviceSql, labourParam);
            }

            return pmolId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<BuPersonWithCompetence> GetBuPersonWithCompetence(PmolParameter pmolParameter)
    {
        try
        {
            await using var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);

            const string buWorker = @"SELECT
                              HRHeader.PersonId
                            FROM dbo.HRHeader 
                            WHERE SequenceId IS NOT NULL";

            var buWorkerList = connection.Query<string>(buWorker, new { pmolParameter.Id }).ToList();

            const string buWorkerData = @"SELECT
                                      CabPersonCompany.CompanyId
                                     ,CabPerson.FullName AS Name
                                     ,CabPerson.Id AS CabPersonId
                                     ,CabPersonCompany.Id AS CabPersonCompanyId
                                     ,CompetenciesTaxonomy.Title AS CompetenciesTaxonomyTitle
                                     ,CompetenciesTaxonomy.Id AS CompetenciesTaxonomyId
                                     ,CabPerson.Id
                                     ,HRHeader.Organization
                                     ,CabCompany.Name AS CompanyName
                                    FROM dbo.CabPersonCompany
                                    INNER JOIN dbo.CabPerson
                                      ON CabPersonCompany.PersonId = CabPerson.Id
                                       INNER JOIN dbo.CabCompany
                                      ON CabPersonCompany.CompanyId = CabCompany.Id
                                    LEFT OUTER JOIN dbo.CabCompetencies
                                      ON CabPersonCompany.PersonId = CabCompetencies.PersonId
                                    INNER JOIN dbo.CompetenciesTaxonomy
                                      ON CabCompetencies.CompetenciesTaxonomyId = CompetenciesTaxonomy.Id
                                    LEFT OUTER JOIN dbo.HRHeader
                                      ON CabPersonCompany.Id = HRHeader.PersonId
                                    WHERE CabPersonCompany.Id IN @Id AND  CabPersonCompany.CompanyId IS NOT NULL ORDER BY CabPerson.FullName ";

            var buWorkerDataList = connection
                .Query<WorkerData>(buWorkerData, new { Id = buWorkerList }).ToList();

            var mBuPersonWithCompetence = new BuPersonWithCompetence();

            var team1 = new BuPersonWithCompetenceTeam
            {
                Name = "Personeel",
                Id = "1"
            };
            var team2 = new BuPersonWithCompetenceTeam
            {
                Name = "Interim",
                Id = "2"
            };
            var team3 = new BuPersonWithCompetenceTeam
            {
                Name = "Onderaannemers",
                Id = "3"
            };

            buWorkerDataList.Where(c => pmolParameter.PmolPerson.CabPersonId.Any(y => y == c.CabPersonId)).Select(c =>
            {
                c.IsLabour = true;
                return c;
            }).ToList();

            var selectProject =
                @"SELECT ProjectDefinition.Id, ProjectDefinition.Title,ProjectDefinition.SequenceCode,ProjectDefinition.ProjectConnectionString ,ProjectDefinition.ProjectManagerId ,CabCompany.SequenceCode AS ContractingUnitId,ProjectDefinition.LocationId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId  WHERE ProjectDefinition.IsDeleted = 0 AND ProjectClassification.ProjectClassificationBuisnessUnit = @BuId ORDER BY ProjectDefinition.SequenceCode";

            var db = connection.Query<ProjectDefinition>(selectProject, new { pmolParameter.PmolPerson.BuId }).ToList();

            string pmolSearch =
                @"SELECT PmolTeamRole.CabPersonId FROM dbo.PMolPlannedWorkLabour INNER JOIN dbo.PMol ON PMolPlannedWorkLabour.PmolId = PMol.Id INNER JOIN dbo.PmolTeamRole ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE ExecutionDate = @Date AND PMol.IsDeleted = 0 AND PMolPlannedWorkLabour.IsDeleted = 0 AND PmolTeamRole.IsDeleted = 0 AND PmolTeamRole.CabPersonId NOT IN @Id";

            var pmolLabour = new List<string>();

            foreach (var project in db)
            {
                await using (var pConnection = new SqlConnection(project.ProjectConnectionString))
                {
                    var labourId = pConnection.Query<string>(pmolSearch,
                        new { pmolParameter.PmolPerson.Date, Id = pmolParameter.PmolPerson.CabPersonId }).ToList();

                    pmolLabour.AddRange(labourId);
                }
            }

            buWorkerDataList.Where(c => pmolLabour.Any(y => y == c.CabPersonId)).Select(c =>
            {
                c.CheckboxStatus = "1";
                return c;
            }).ToList();

            var absence =
                @"SELECT Person FROM dbo.AbsenceHeader WHERE @date BETWEEN AbsenceHeader.StartDate AND AbsenceHeader.EndDate";

            var absenceList = connection.Query<string>(absence, new { pmolParameter.PmolPerson.Date }).ToList();

            buWorkerDataList.Where(c => absenceList.Any(y => y == c.CabPersonCompanyId)).Select(c =>
            {
                c.CheckboxStatus = "2";
                return c;
            }).ToList();

            team1.CompetenceList = buWorkerDataList.DistinctBy(e => e.Id)
                .Where(e => e.CompanyId == pmolParameter.Configuration.GetValue<string>("CompanyId"))
                .GroupBy(e => e.CompetenciesTaxonomyId).OrderBy(e => e.First().CompetenciesTaxonomyTitle).ToList()
                .Select(i => new PersonCompetence
                    { Id = i.Key, Name = i.First().CompetenciesTaxonomyTitle, Worker = i.ToList() }).ToList();

            team2.CompetenceList = buWorkerDataList.DistinctBy(e => e.Id)
                .Where(e => e.CompanyId != pmolParameter.Configuration.GetValue<string>("CompanyId") &&
                            e.Organization == "2").GroupBy(e => e.CompetenciesTaxonomyId)
                .OrderBy(e => e.First().CompetenciesTaxonomyTitle).ToList().Select(i => new PersonCompetence
                    { Id = i.Key, Name = i.First().CompetenciesTaxonomyTitle, Worker = i.ToList() }).ToList();

            team3.CompetenceList = buWorkerDataList.DistinctBy(e => e.Id)
                .Where(e => e.CompanyId != pmolParameter.Configuration.GetValue<string>("CompanyId") &&
                            e.Organization == "1").GroupBy(e => e.CompetenciesTaxonomyId)
                .OrderBy(e => e.First().CompetenciesTaxonomyTitle).ToList().Select(i => new PersonCompetence
                    { Id = i.Key, Name = i.First().CompetenciesTaxonomyTitle, Worker = i.ToList() }).ToList();

            var team = new List<BuPersonWithCompetenceTeam>
            {
                team1,
                team2,
                team3
            };
            mBuPersonWithCompetence.Team = team;
            return mBuPersonWithCompetence;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    private StringBuilder FilterByDate(StringBuilder sb, PmolParameter PmolParameter)
    {
        if (PmolParameter.filter.Date == 0 || PmolParameter.filter.Date == 1 || PmolParameter.filter.Date == -1)
        {
            var gmt = FindGmtDatetime(PmolParameter);
            sb.Append("  AND ExecutionDate BETWEEN '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
        }
        else if (PmolParameter.filter.Date == -7 || PmolParameter.filter.Date == -14)
        {
            var gmt = FindGmtWeek(PmolParameter);
            sb.Append("  AND ExecutionDate BETWEEN '" + gmt + "' AND '" + gmt.AddDays(7) + "' ");
        }

        else if (PmolParameter.filter.Date == -30 || PmolParameter.filter.Date == -60)
        {
            var gmt = FindGmtMonth(PmolParameter);
            sb.Append("  AND ExecutionDate BETWEEN '" + gmt.FirstDay + "' AND '" + gmt.LastDay + "' ");
        }

        return sb;
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

    public DateTime FindGmtDatetime(PmolParameter PmolParameter)
    {
        var timeZone = new TimeZone
        {
            offset = PmolParameter.filter.Offset
        };
        if (PmolParameter.filter.Date == null)
        {
            timeZone.date = (DateTime)PmolParameter.filter.ExecutionDate;
        }
        else
        {
            var days = Convert.ToDouble(PmolParameter.filter.Date);
            var d = PmolParameter.filter.LocalDate;
            timeZone.date = d.AddDays(days);
        }

        var finalOffset = FormatOffset(timeZone);
        var date = timeZone.date - timeZone.date.TimeOfDay;
        if (finalOffset > 0)
            date = date.AddHours(finalOffset * -1);
        else if (finalOffset < 0) date = date.AddHours(finalOffset);
        return date;
    }

    public DateTime FindGmtWeek(PmolParameter PmolParameter)
    {
        var timeZone = new TimeZone
        {
            offset = PmolParameter.filter.Offset,
            date = PmolParameter.filter.LocalDate
        };
        var delta = DayOfWeek.Monday - PmolParameter.filter.LocalDate.DayOfWeek;
        if (delta > 0)
            delta -= 7;
        timeZone.date = timeZone.date.AddDays(delta);

        if (PmolParameter.filter.Date == -14) timeZone.date = timeZone.date.AddDays(-7);

        var finalOffset = FormatOffset(timeZone);

        var date = timeZone.date - timeZone.date.TimeOfDay;
        if (finalOffset > 0)
            date = date.AddHours(finalOffset * -1);
        else if (finalOffset < 0) date = date.AddHours(finalOffset);
        return date;
    }


    public GmtMonthDto FindGmtMonth(PmolParameter PmolParameter)
    {
        var timeZone = new TimeZone
        {
            offset = PmolParameter.filter.Offset,
            date = PmolParameter.filter.LocalDate
        };

        var dto = new GmtMonthDto();

        if (PmolParameter.filter.Date == -30)
        {
            dto.FirstDay = new DateTime(timeZone.date.Year, timeZone.date.Month, 1);
            dto.LastDay = dto.FirstDay.AddMonths(1).AddDays(-1);
        }
        else
        {
            dto.FirstDay = timeZone.date.Month == 1
                ? new DateTime(timeZone.date.Year - 1, 12, 1)
                : new DateTime(timeZone.date.Year, timeZone.date.Month - 1, 1);
            dto.LastDay = dto.FirstDay.AddMonths(1).AddDays(-1);
        }

        var finalOffset = FormatOffset(timeZone);

        switch (finalOffset)
        {
            // DateTime date = timeZone.date - timeZone.date.TimeOfDay;
            case > 0:
                dto.FirstDay = dto.FirstDay.AddHours(finalOffset * -1);
                dto.LastDay = dto.LastDay.AddHours(finalOffset * -1).AddHours(24);
                break;
            case < 0:
                dto.FirstDay = dto.FirstDay.AddHours(finalOffset);
                dto.LastDay = dto.LastDay.AddHours(finalOffset).AddHours(24);
                break;
        }

        return dto;
    }

    public async Task<string> CopyResourcesFromBorToPmol(PmolResourceParameter pmolParameter, string PmolId)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var param = new { pmolId = PmolId, borId = pmolParameter.Id };

        var consumableSql = @"insert into PMolPlannedWorkConsumable
                                         select  NEWID(), CorporateProductCatalogId, SUM(Required) AS Required,0, CpcBasicUnitOfMeasureLocalizedData.Id , @pmolId, 'Planned',0
                                         from BorConsumable 
                                         LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorConsumable.CorporateProductCatalogId
                                         LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                                         where BorProductId = @borId
                                         AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode='en' OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                         group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id";

        var labourSql = @"insert into PMolPlannedWorkLabour
                                    select  NEWID(), CorporateProductCatalogId, SUM(Required) AS Required,0, CpcBasicUnitOfMeasureLocalizedData.Id , @pmolId, 'Planned',0
                                    from BorLabour 
                                    LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorLabour.CorporateProductCatalogId
                                    LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                                    where BorProductId = @borId
                                    AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode='en' OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                    group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id";

        var materialSql = @"insert into PMolPlannedWorkMaterial
                                    select  NEWID(), CorporateProductCatalogId, SUM(Required) AS Required,0, CpcBasicUnitOfMeasureLocalizedData.Id , @pmolId, 'Planned',0
                                    from BorMaterial 
                                    LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorMaterial.CorporateProductCatalogId
                                    LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                                    where BorProductId = @borId
                                    AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode='en' OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                    group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id";

        var toolSql = @"insert into PMolPlannedWorkTools
                                    select  NEWID(), CorporateProductCatalogId, SUM(Required) AS Required,0, CpcBasicUnitOfMeasureLocalizedData.Id , @pmolId, 'Planned',0,'0'
                                    from BorTools 
                                    LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorTools.CorporateProductCatalogId
                                    LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                                    where BorProductId = @borId
                                    AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode='en' OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                    group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id";

        using (var connection = new SqlConnection(connectionString))
        {
            var affectedRows1 = connection.Execute(consumableSql, param);
            var affectedRows2 = connection.Execute(labourSql, param);
            var affectedRows3 = connection.Execute(materialSql, param);
            var affectedRows4 = connection.Execute(toolSql, param);
            connection.Close();
        }

        return pmolParameter.Id;
    }

    private async Task<string> AddUserRole(PmolParameter pmolParameter, ITenantProvider tenantProvider)
    {
        var foremanId = pmolParameter.PmolDto.ForemanId;
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, tenantProvider);
        var query = "select Oid from CabPersoncompany where PersonId='" + foremanId + "'";
        string result;
        await using (var dbConnection = new SqlConnection(tenantProvider.GetTenant().ConnectionString))
        {
            result = dbConnection.Query<string>(query).ToList().FirstOrDefault();
            
        }

        if (string.IsNullOrEmpty(result))
        {
            var queryEmail = @"SELECT
                                          CabEmail.EmailAddress AS Email
                                        FROM dbo.CabPersonCompany
                                        INNER JOIN dbo.CabEmail
                                          ON CabPersonCompany.EmailId = CabEmail.Id
                                        WHERE CabPersonCompany.PersonId = '" + foremanId + "'";
            string email;
            await using (var dbConnection = new SqlConnection(tenantProvider.GetTenant().ConnectionString))
            {
                email = dbConnection.Query<string>(queryEmail).ToList().FirstOrDefault();
                
            }

            string Oid = null;
            var queryOid = @"SELECT
                                      ApplicationUser.OId AS Oid
                                    FROM dbo.ApplicationUser
                                    WHERE ApplicationUser.Email = '" + email + "'";
            await using (var dbConnection = new SqlConnection(tenantProvider.GetTenant().ConnectionString))
            {
                Oid = dbConnection.Query<string>(queryOid).ToList().FirstOrDefault();
                
            }

            var IsNewUser = false;
            if (!string.IsNullOrEmpty(Oid))
            {
                var personCompany = applicationDbContext.CabPersonCompany
                    .FirstOrDefault(p => p.PersonId.Equals(foremanId));
                if (personCompany != null)
                {
                    personCompany.Oid = Oid;
                    applicationDbContext.CabPersonCompany.Update(personCompany);
                    await applicationDbContext.SaveChangesAsync();
                }
            }

            // Azure invitation comment by Shanuka
            // IsNewUser = true;
            // AzureInvitation.SendInvitation(email, "Hi..", "Azure Invitaion", "Kurt@mickiesoft.com", true,
            //     tenantProvider.GetTenant().CatelogConnectionString);
            // Oid = await AzureInvitation.GetUserByEmail(email,
            //     tenantProvider.GetTenant().CatelogConnectionString);
            // var personCompany = applicationDbContext.CabPersonCompany
            //     .FirstOrDefault(p => p.PersonId.Equals(foremanId));
            // if (personCompany != null && !string.IsNullOrEmpty(Oid))
            // {
            //     personCompany.Oid = Oid;
            //     applicationDbContext.CabPersonCompany.Update(personCompany);
            //     await applicationDbContext.SaveChangesAsync();
            // }
            AddAclData(applicationDbContext, Oid, pmolParameter.ProjectSequenceId, IsNewUser, email);
        }
        else
        {
            AddAclData(applicationDbContext, result, pmolParameter.ProjectSequenceId, false, null);
        }

        return "Ok";
    }

    private void AddAclData(ApplicationDbContext applicationDbContext, string Oid, string projectSqCode, bool IsNewUser,
        string newUserEmail)
    {
        if (IsNewUser)
        {
            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                OId = Oid,
                Email = newUserEmail
            };
            applicationDbContext.ApplicationUser.Add(applicationUser);
            applicationDbContext.SaveChanges();
        }

        var foremanRole = applicationDbContext.Role.FirstOrDefault(f => f.RoleId.Equals("Foreman"));
        if (foremanRole != null)
        {
            var existingUserRole = applicationDbContext.UserRole.FirstOrDefault(u =>
                u.ApplicationUserOid.Equals(Oid)
                && u.RoleId.Equals(foremanRole.RoleId));
            string userRoleId = null;
            if (existingUserRole == null)
            {
                var userRole = new UserRole
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationUserOid = Oid,
                    RoleId = foremanRole.RoleId
                };
                applicationDbContext.UserRole.Add(userRole);
                applicationDbContext.SaveChanges();
                userRoleId = userRole.Id;
            }
            else
            {
                userRoleId = existingUserRole.Id;
            }

            var projectDef =
                applicationDbContext.ProjectDefinition.FirstOrDefault(p => p.SequenceCode.Equals(projectSqCode));
            if (projectDef != null)
            {
                var existingProjectUser = applicationDbContext.ProjectUserRole.FirstOrDefault(p =>
                    p.UsrRoleId.Equals(userRoleId)
                    && p.ProjectDefinitionId.Equals(projectDef.Id));
                if (existingProjectUser == null)
                {
                    var projectUserRole = new ProjectUserRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        UsrRoleId = userRoleId,
                        ProjectDefinitionId = projectDef.Id
                    };
                    applicationDbContext.ProjectUserRole.Add(projectUserRole);
                    applicationDbContext.SaveChanges();
                }
            }
        }
        else
        {
            throw new Exception("Foreman job role is unavailable");
        }
    }

    private async Task<string> CreateHistory(PmolParameter PmolParameter, bool isExist)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, PmolParameter.TenantProvider))
        {
            var jsonProduct = JsonConvert.SerializeObject(PmolParameter.PmolDto, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var historyLog = new PmolHistoryLog
            {
                Id = Guid.NewGuid().ToString(),
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = PmolParameter.UserId,
                HistoryLog = jsonProduct,
                PmolId = PmolParameter.PmolDto.Id,
                Action = isExist == false ? HistoryState.ADDED.ToString() : HistoryState.UPDATED.ToString()
            };

            context.PMolHistoryLog.Add(historyLog);
            await context.SaveChangesAsync();
        }

        return "";
    }

    public void DeleteData(PmolParameter pmolParameter, string tableName)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        foreach (var id in pmolParameter.IdList)
        {
            using var dbConnection = new SqlConnection(connectionString);
            dbConnection.Execute("update " + tableName + " set IsDeleted = @val where Id = @id",
                new { val = true, id });

            
        }
    }

    private List<ProjectDefToLoadPmol> GetProjectConnectionsByUser(string oId, ITenantProvider tenantProvider)
    {
        var dbConnection = tenantProvider.orgSqlConnection();
        var sql = @"SELECT DISTINCT ProjectDefinition.SequenceCode AS SequenceCode,
                              ProjectDefinition.ProjectConnectionString AS ProjectConnectionString
                                ,ProjectDefinition.Id AS Id
                                ,ProjectDefinition.Name AS Name
                                ,ProjectDefinition.ContractingUnitId AS CuId
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectUserRole
                              ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                            INNER JOIN dbo.UserRole
                              ON ProjectUserRole.UsrRoleId = UserRole.Id
                            INNER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserOid = ApplicationUser.Oid
                            WHERE ApplicationUser.OId = @oId  AND IsDeleted = 0";

        return dbConnection.QueryAsync<ProjectDefToLoadPmol>(sql, new { oId }).Result.ToList();
    }

    private List<ProjectDefToLoadPmol> GetProjectConnectionsByUserForMobile(string oId, ITenantProvider tenantProvider)
    {
        var dbConnection = tenantProvider.Connection();
        var sql = @"SELECT DISTINCT ProjectDefinition.SequenceCode AS SequenceCode,
                              ProjectDefinition.ProjectConnectionString AS ProjectConnectionString
                                ,ProjectDefinition.Id AS Id
                                ,ProjectDefinition.Name AS Name
                                ,ProjectDefinition.ContractingUnitId AS CuId
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectUserRole
                              ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                            INNER JOIN dbo.UserRole
                              ON ProjectUserRole.UsrRoleId = UserRole.Id
                            INNER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserOid = ApplicationUser.Oid
                            WHERE ApplicationUser.OId = @oId  AND IsDeleted = 0 AND NOT IN ";

        return dbConnection.Query<ProjectDefToLoadPmol>(sql, new { oId }).ToList();
    }

    public async Task<string> UpdateBor(PmolParameter pmolParameter)
    {
        var dbConnection = pmolParameter.TenantProvider.Connection();
        var queryMaterial = @"UPDATE BorMaterial 
                                SET 
                                BorMaterial.Consumed = BorMaterial.Consumed + cost.ConsumedQuantity
                                FROM ProjectCost cost
                                inner join CorporateProductCatalog on CorporateProductCatalog.ResourceNumber = cost.ResourceNumber
                                where PmolId = @Id 
                                AND BorMaterial.CorporateProductCatalogId = CorporateProductCatalog.Id 
                                AND cost.ResourceNumber = CorporateProductCatalog.ResourceNumber
                                AND BorMaterial.BorProductId = cost.BorId AND cost.IsPlannedResource = 1";

        var queryConsumable = @"UPDATE BorConsumable 
                                           SET 
                                           BorConsumable.Consumed = BorConsumable.Consumed + cost.ConsumedQuantity
                                           FROM ProjectCost cost
                                           inner join CorporateProductCatalog on CorporateProductCatalog.ResourceNumber = cost.ResourceNumber
                                           where PmolId = @Id
                                           AND BorConsumable.CorporateProductCatalogId = CorporateProductCatalog.Id 
                                           AND cost.ResourceNumber = CorporateProductCatalog.ResourceNumber
                                           AND BorConsumable.BorProductId = cost.BorId AND cost.IsPlannedResource = 1";

        var queryLabour = @"UPDATE BorLabour
                                       SET 
                                       BorLabour.Consumed = BorLabour.Consumed + cost.ConsumedQuantity
                                       FROM ProjectCost cost
                                       inner join CorporateProductCatalog on CorporateProductCatalog.ResourceNumber = cost.ResourceNumber
                                       where PmolId = @Id 
                                       AND BorLabour.CorporateProductCatalogId = CorporateProductCatalog.Id 
                                       AND cost.ResourceNumber = CorporateProductCatalog.ResourceNumber
                                       AND BorLabour.BorProductId = cost.BorId AND cost.IsPlannedResource = 1";

        var queryTools = @"UPDATE BorTools
                                      SET 
                                      BorTools.Consumed = BorTools.Consumed + cost.ConsumedQuantity
                                      FROM ProjectCost cost
                                      inner join CorporateProductCatalog on CorporateProductCatalog.ResourceNumber = cost.ResourceNumber
                                      where PmolId = @Id
                                      AND BorTools.CorporateProductCatalogId = CorporateProductCatalog.Id 
                                      AND cost.ResourceNumber = CorporateProductCatalog.ResourceNumber
                                      AND BorTools.BorProductId = cost.BorId AND cost.IsPlannedResource = 1";


        var param = new { pmolParameter.Id };

        await dbConnection.QueryAsync(queryMaterial, param);
        await dbConnection.QueryAsync(queryConsumable, param);
        await dbConnection.QueryAsync(queryLabour, param);
        await dbConnection.QueryAsync(queryTools, param);

        return "Ok";
    }

    public async Task<string> ClonePmolMultipleDays(PmolParameter pmolParameter)
    {
        var dbConnection = pmolParameter.TenantProvider.Connection();
        var pmol = await GetPmolById(pmolParameter);

        var dateTime = DateTime.ParseExact(pmol.ExecutionEndTime, "HH:mm",
            CultureInfo.InvariantCulture);
        var resourceParameter = new PmolResourceParameter();
        resourceParameter.ContractingUnitSequenceId = pmolParameter.ContractingUnitSequenceId;
        resourceParameter.Id = pmol.Id;
        resourceParameter.Lang = pmolParameter.Lang;
        resourceParameter.ProjectSequenceId = pmolParameter.ProjectSequenceId;
        resourceParameter.TenantProvider = pmolParameter.TenantProvider;
        resourceParameter.VpRepository = pmolParameter.IVpRepository;
        var consumable = await pmolParameter.IPmolResourceRepository.ReadConsumable(resourceParameter);
        var material = await pmolParameter.IPmolResourceRepository.ReadMaterial(resourceParameter);
        var labour = await pmolParameter.IPmolResourceRepository.ReadLabour(resourceParameter);
        var tools = await pmolParameter.IPmolResourceRepository.ReadToolsForDayPlanning(resourceParameter);

        foreach (var item in pmolParameter.MultiplePmolClone.ExecutionDate)
        {
            var pmolCreateDto = new PmolCreateDto();
            pmolCreateDto.Comment = pmol.Comment;
            pmolCreateDto.ForemanId = pmol.ForemanId;
            pmolCreateDto.ForemanMobileNumber = pmol.ForemanMobileNumber;
            pmolCreateDto.Id = Guid.NewGuid().ToString();
            pmolCreateDto.Name = pmol.Name;
            var borGetByIdDto = new BorGetByIdDto();
            if (pmol.PmolType != "sub") borGetByIdDto.Id = pmol.Bor.Id;
            pmolCreateDto.Bor = borGetByIdDto;

            pmolCreateDto.StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477";
            pmolCreateDto.TypeId = pmolParameter.MultiplePmolClone.TypeId;
            pmolCreateDto.PmolType = pmol.PmolType;
            pmolCreateDto.ProductId = pmol.ProductId;
            pmolCreateDto.ParentId = pmol.Id;
            pmolParameter.PmolDto = pmolCreateDto;
            pmolCreateDto.LocationId = pmol.LocationId;
            pmolCreateDto.ExecutionStartTime = pmol.ExecutionEndTime;

            var hqLat = pmolParameter.Configuration.GetValue<string>("HQLat").ToDouble();
            var hqLon = pmolParameter.Configuration.GetValue<string>("HQLong").ToDouble();


            var mapLocation = dbConnection
                .Query<Position>(
                    "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                    new { Id = pmol.LocationId }).FirstOrDefault();
            if (mapLocation != null)
            {
                // var duration = pmolParameter.IShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(),
                //     mapLocation.Lon.ToDouble(),
                //     50.93654767100221, 3.1299880230856334, pmolParameter.TenantProvider, pmolParameter.Configuration);
                var duration = pmolParameter.IShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(),
                    mapLocation.Lon.ToDouble(),
                    hqLat, hqLon, pmolParameter.TenantProvider, pmolParameter.Configuration,
                    false);

                if (await duration != 0)
                {
                    var endTime = dateTime.AddSeconds(await duration);
                    pmolCreateDto.ExecutionEndTime = endTime.ToString("HH:mm");
                }
                else
                {
                    var time = dateTime.AddHours(2);
                    pmolCreateDto.ExecutionEndTime = time.ToString("HH:mm");
                }
            }
            else
            {
                var time = dateTime.AddHours(2);
                pmolCreateDto.ExecutionEndTime = time.ToString("HH:mm");
            }

            pmolCreateDto.ExecutionDate = item;

            await CreateHeader(pmolParameter, true);

            if (pmol.PmolType == "sub")
            {
                var param = new PmolParameter();
                var serviceCreate = new PmolServiceCreateDto();
                param.Id = pmol.Id;
                param.ContractingUnitSequenceId = pmolParameter.ContractingUnitSequenceId;
                param.ProjectSequenceId = pmolParameter.ProjectSequenceId;
                param.TenantProvider = pmolParameter.TenantProvider;
                var service = await ReadPmolServiceByPmolId(param);
                if (service != null)
                {
                    serviceCreate.PmolId = pmolCreateDto.Id;
                    serviceCreate.Mou = service.Mou;
                    serviceCreate.Quantity = service.Quantity;
                    serviceCreate.TotalPrice = service.TotalPrice;
                    serviceCreate.UnitPrice = service.UnitPrice;
                    serviceCreate.Comments = service.Comments;
                    serviceCreate.Documents = service.Documents;
                    param.PmolServiceCreate = serviceCreate;
                    await CreatePmolService(param);
                }
            }

            foreach (var con in consumable)
            {
                var dto = new PmolResourceCreateDto
                {
                    CorporateProductCatalogId = con.CorporateProductCatalogId,
                    CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId,
                    Environment = "local",
                    Id = null,
                    PmolId = pmolCreateDto.Id,
                    Required = con.Required,
                    ResourceNumber = con.ResourceNumber,
                    Type = "Planned"
                };

                resourceParameter.ResourceCreateDto = dto;
                await pmolParameter.IPmolResourceRepository.CreateConsumable(resourceParameter);
            }

            foreach (var con in material)
            {
                var dto = new PmolResourceCreateDto
                {
                    CorporateProductCatalogId = con.CorporateProductCatalogId,
                    CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId,
                    Environment = "local",
                    Id = null,
                    PmolId = pmolCreateDto.Id,
                    Required = con.Required,
                    ResourceNumber = con.ResourceNumber,
                    Type = "Planned"
                };

                resourceParameter.ResourceCreateDto = dto;
                await pmolParameter.IPmolResourceRepository.CreateMaterial(resourceParameter);
            }

            var addTeamMember =
                @"INSERT INTO dbo.PmolTeamRole ( Id ,CabPersonId ,RoleId ,Type ,PmolLabourId ) VALUES ( @Id ,@CabPersonId ,@RoleId ,@Type ,@PmolLabourId );";

            foreach (var con in labour)
            {
                var organizationteam = dbConnection.Query<OrganizationTeamPmol>(
                    "SELECT * FROM OrganizationTeamPmol WHERE PmolId = @Id",
                    new { pmol.Id }).FirstOrDefault();

                var dto = new PmolResourceCreateDto
                {
                    CorporateProductCatalogId = con.CorporateProductCatalogId,
                    CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId,
                    Environment = "local",
                    Id = null,
                    PmolId = pmolCreateDto.Id,
                    Required = con.Required,
                    ResourceNumber = con.ResourceNumber,
                    Type = "Planned",
                    OrganizationTeamId = organizationteam?.OrganizationTeamId
                };


                resourceParameter.ResourceCreateDto = dto;
                resourceParameter.VpRepository = pmolParameter.IVpRepository;
                resourceParameter.Configuration = pmolParameter.Configuration;
                var Id = await pmolParameter.IPmolResourceRepository.CreateLabour(resourceParameter);


                var team = dbConnection
                    .Query<PmolTeamRole>("SELECT * FROM dbo.PmolTeamRole WHERE PmolLabourId = @Id",
                        new { con.Id }).ToList();

                foreach (var param2 in team.Select(i => new
                         {
                             Id = Guid.NewGuid(),
                             i.CabPersonId,
                             i.RoleId,
                             i.Type,
                             PmolLabourId = Id
                         }))
                {
                    await dbConnection.ExecuteAsync(addTeamMember, param2);
                }
                //}
            }

            foreach (var con in tools)
            {
                var dto = new PmolResourceCreateDto
                {
                    CorporateProductCatalogId = con.CorporateProductCatalogId,
                    CpcBasicUnitOfMeasureId = con.CpcBasicUnitOfMeasureId,
                    Environment = "local",
                    Id = null,
                    PmolId = pmolCreateDto.Id,
                    Required = con.Required,
                    ResourceNumber = con.ResourceNumber,
                    Type = "Planned"
                };

                resourceParameter.ResourceCreateDto = dto;
                await pmolParameter.IPmolResourceRepository.CreateTools(resourceParameter);
            }
        }

        return pmol.Id;
    }

    public async Task<PmolCbcResources> AddPmolCbcResource(PmolParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        
        await using var dbConnection = new SqlConnection(connectionString);
        
        const string insertSql =
            "INSERT INTO dbo.PmolCbcResources ( Id ,PmolId ,LotId ,ArticleNo ,Quantity,ConsumedQuantity ) VALUES ( @Id ,@PmolId ,@LotId ,@ArticleNo ,@Quantity,'0' )";

        const string updateSql =
            "UPDATE PmolCbcResources SET Quantity = @Quantity,ConsumedQuantity = @ConsumedQuantity Where Id = @Id";

        if (pmolParameter.PmolCbcResources.Id == null)
        {
            pmolParameter.PmolCbcResources.Id = Guid.NewGuid().ToString();
            await dbConnection.ExecuteAsync(insertSql, pmolParameter.PmolCbcResources);
        }
        else
        {
            await dbConnection.ExecuteAsync(updateSql, pmolParameter.PmolCbcResources);
        }

        return pmolParameter.PmolCbcResources;
    }

    public async Task<List<string>> DeletePmolCbcResource(PmolParameter pmolParameter)
    {
        var dbConnection = pmolParameter.TenantProvider.Connection();

        const string sql = "Delete From PmolCbcResources Where Id IN @Ids";
        await dbConnection.ExecuteAsync(sql, new { Ids = pmolParameter.IdList });


        return pmolParameter.IdList;
    }

    public async Task<List<GetPmolCbcResourcesDto>> GetPmolCbcResourcesById(PmolParameter pmolParameter)
    {
        var dbConnection = pmolParameter.TenantProvider.Connection();
        
        const string sql = "GetPmolCbcResourcesById";
        // var sql = @"CREATE PROCEDURE GetPbsCbcResourcesById @Id nvarchar(450)
        //             AS
        //     SELECT pcr.id,pcr.ArticleNo,pcr.LotId,pcr.Quantity,pcr.PbsId,Concat(pcpd.ArticleNo,' - ',pcpd.Title) AS Title,pcpd.Unit FROM PbsCbcResources pcr LEFT OUTER JOIN ContractorTotalValuesPublished ctvp ON pcr.LotId = ctvp.LotId LEFT OUTER JOIN PublishedContractorsPdfData pcpd ON pcr.ArticleNo = pcpd.ArticleNo AND ctvp.CompanyId = pcpd.CompanyId 
        // WHERE pcr.PbsId = @Id AND ctvp.IsWinner = 1";

        var result = await dbConnection.QueryAsync<GetPmolCbcResourcesDto>(sql, commandType: CommandType.StoredProcedure,
            param: new { Id = pmolParameter.Id });

        return result.DistinctBy(x => x.Id).ToList();
    }
}