using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using ServiceStack;
using ServiceStack.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.GL;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.ProjectClassification;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Jobs;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;
using Properties = UPrinceV4.Web.Data.Properties;

namespace UPrinceV4.Web.Repositories;

public class ProjectDefinitionRepository : IProjectDefinitionRepository
{
    
    public async Task<ProjectCreateReturnResponse> CreateProjectDefinition(ApplicationDbContext contexts,
        ProjectDefinitionCreateDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        IProjectFinanceRepository iProjectFinanceRepository, IProjectTeamRepository projectTeamRepository,
        ITenantProvider iTenantProvider, ApplicationUser user, IPbsRepository _IPbsRepository)
    {
        var _log4net = LogManager.GetLogger(typeof(ProjectDefinitionRepository));

        try
        {
            var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
            var context =
                new ProjectDefinitionDbContext(options1,iTenantProvider);
            
            var response = new ProjectCreateReturnResponse();
            var project = new ProjectDefinition
            {
                SequenceCode = GetSequenceNumber(contexts),
                Description = projectDto.Description,
                Id = Guid.NewGuid().ToString()
            };
            if (projectDto.ProjectTeam != null) project.ContractingUnitId = projectDto.ProjectTeam.ContractingUnitId;

            project.ProjectConnectionString =
                CreateDatabase(project.SequenceCode, contexts, iTenantProvider, projectDto._iConfiguration);
            project.Action = HistoryState.ADDED.ToString();
            project.ProjTemplateId = projectDto.ProjectTemplateId;
            if (projectDto.ProjectTypeId == null)
            {
                project.ProjTypeId = context.ProjectType.First(t => t.IsDefault == true).Id;;
            }
            else
            {
                project.ProjTypeId = projectDto.ProjectTypeId;
            }

            if (projectDto.ProjectManagementLevelId == null)
            {
                var projectManagementLevelId = context.ProjectManagementLevel.First(t => t.IsDefault == true).Id;
                project.ProjManagementLevelId = projectManagementLevelId;
            }
            else
            {
                project.ProjManagementLevelId = projectDto.ProjectManagementLevelId;
            }

            if (projectDto.ProjectToleranceStateId == null)
            {
                var projectToleranceStateId = context.ProjectToleranceState.First(t => t.IsDefault == true).Id;
                project.ProjToleranceStateId = projectToleranceStateId;
            }
            else
            {
                project.ProjToleranceStateId = projectDto.ProjectToleranceStateId;
            }

            project.ProjectScopeStatusId = projectDto.ProjectScopeStatusId;
            project.ProjectStatus = projectDto.ProjectStatus;


            project.LocationId = AddProjectLocation( projectDto, null, iTenantProvider);
            project.Name = projectDto.Name;
            project.GeneralLedgerId = projectDto.GeneralLedgerId;
            project.Title = project.SequenceCode + " - " + project.Name;
            project.CustomerId = projectDto.CustomerId;
            project.Language = "3fc25097-c354-46c8-992a-1122810bbd2c"; //nl
            project.ProjectManagerId = projectDto.ProjectManagerId;
            project.SiteManagerId = projectDto.SiteManagerId;


            context.ProjectDefinition.Add(project);
            await context.SaveChangesAsync();

            var pId = project.Id;

            if (projectDto.ProjectManagerId != null)
                if (projectDto.ProjectTeam != null && projectDto.ProjectTeam.Id == null)
                {
                    projectDto.ProjectTeam.ProjectId = pId;
                    var pmAdd = new ProjectTeamRoleCreateDto
                    {
                        CabPersonId = projectDto.ProjectManagerId,
                        RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3",
                        IsAccessGranted = true,
                        status = "1"
                    };

                    projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                }

            if (projectDto.CustomerId != null)
                if (projectDto.ProjectTeam != null && projectDto.ProjectTeam.Id == null)
                {
                    projectDto.ProjectTeam.ProjectId = pId;
                    var pmAdd = new ProjectTeamRoleCreateDto
                    {
                        CabPersonId = projectDto.CustomerId,
                        RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d",
                        IsAccessGranted = false,
                        status = "1"
                    };

                    projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                }

            if (projectDto.SiteManagerId != null)
                if (projectDto.ProjectTeam != null && projectDto.ProjectTeam.Id == null)
                {
                    projectDto.ProjectTeam.ProjectId = pId;
                    var pmAdd = new ProjectTeamRoleCreateDto
                    {
                        CabPersonId = projectDto.SiteManagerId,
                        RoleId = "yyyyyyy-a513-45e0-a431-170dbd4yyyy",
                        IsAccessGranted = true,
                        status = "1"
                    };

                    projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                }

            CreateProjectCostConversion(pId, projectDto.ProjectCostConversionCreateDto, iTenantProvider);
            CreateKpi( projectDto.Kpi, pId,iTenantProvider);

            projectDto.ProjectFinance.ProjectId = pId;
            iProjectFinanceRepository.CreateProjectFinance(projectDto.ProjectFinance, iTenantProvider);

            projectDto.ProjectTime.ProjectId = pId;
            await iProjectTimeRepository.CreateProjectTime( projectDto.ProjectTime, user, iTenantProvider);

            if (projectDto.ProjectTeam != null)
            {
                projectDto.ProjectTeam.ProjectId = pId;
                await projectTeamRepository.CreateProjectTeam( projectDto.ProjectTeam, iTenantProvider,
                    user);
            }

            if (projectDto.ProjectClassification != null)
            {
                await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                var classification =
                    "INSERT INTO dbo.ProjectClassification ( Id ,ProjectId ,ProjectClassificationBuisnessUnit ,ProjectClassificationSizeId ,ProjectClassificationConstructionTypeId ,ProjectClassificationSectorId ) VALUES ( @Id ,@ProjectId ,@ProjectClassificationBuisnessUnit ,@ProjectClassificationSizeId ,@ProjectClassificationConstructionTypeId ,@ProjectClassificationSectorId )";

                var param = new
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectId = pId,
                    projectDto.ProjectClassification.ProjectClassificationBuisnessUnit,
                    projectDto.ProjectClassification.ProjectClassificationSizeId,
                    projectDto.ProjectClassification
                        .ProjectClassificationConstructionTypeId,
                    projectDto.ProjectClassification.ProjectClassificationSectorId
                };

                await connection.ExecuteAsync(classification, param);
            }

            var jsonProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var pdhl = new ProjectDefinitionHistoryLog
            {
                Action = HistoryState.ADDED.ToString(),
                ChangedByUserId = user.Id,
                Id = Guid.NewGuid().ToString(),
                HistoryLog = jsonProject,
                ChangedTime = DateTime.UtcNow,
                ProjectDefinitionId = project.Id
            };
            context.ProjectDefinitionHistoryLog.Add(pdhl);
            await context.SaveChangesAsync();
            response.ProjectId = project.Id;
            response.SequenceId = project.SequenceCode;

            var pbsParameters = new PbsParameters();
            var pbsDto = new PbsProductCreateDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = project.Name,
                Contract = "Yes",
                PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
                PbsType = "regular",
                PbsProductItemTypeId = "aa0c8e3c-f716-4f92-afee-851d485164da"
            };
            pbsParameters.ChangedUser = user;
            pbsParameters.PbsDto = pbsDto;
            pbsParameters.ProjectSequenceId = project.SequenceCode;
            pbsParameters.TenantProvider = iTenantProvider;
            await _IPbsRepository.CreatePbs(pbsParameters);

            return response;
        }
        catch (Exception ex)
        {
            _log4net.Info("Exception thrown project Create");
            _log4net.Error(ex.ToString());
            throw new Exception(ex.ToString());

        }
    }

    public async Task<bool> DeleteProjectDefinition(ApplicationDbContext context, string id, ApplicationUser user)
    {
        var project = (from a in context.ProjectDefinition
            where a.Id == id
            select a).Single();
        //project.ChangeByUserId = user.Id;
        //project.Action = HistoryState.DELETED.ToString();
        context.ProjectDefinition.Update(project);
        await context.SaveChangesAsync();
        context.ProjectDefinition.Remove(project);
        await context.SaveChangesAsync();
        var jsonProject = JsonConvert.SerializeObject(project, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var pdhl = new ProjectDefinitionHistoryLog
        {
            Action = HistoryState.DELETED.ToString(),
            ChangedByUserId = user.Id,
            Id = Guid.NewGuid().ToString(),
            HistoryLog = jsonProject,
            ChangedTime = DateTime.UtcNow,
            ProjectDefinitionId = project.Id
        };
        context.ProjectDefinitionHistoryLog.Add(pdhl);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProjectDefinition>> GetProjectDefinition(ApplicationDbContext context)
    {
        var model = context.ProjectDefinition.Include(p => p.ProjectKpi)
            .Include(p => p.ProjectTime).ThenInclude(t => t.CalendarTemplate)
            .Include(p => p.ProjectFinance).ThenInclude(f => f.Currency)
            .OrderBy(p => p.SequenceCode)
            .ToList();

        return model;
    }

    public async Task<ProjectDefinitionDto> GetProjectDefinitionById(ApplicationDbContext context, string id,
        string lang, ITenantProvider iTenantProvider)
    {
        var sql =
            @"SELECT ProjectDefinition.Id ,ProjectDefinition.Description AS Description,LocationId AS MapLocationId  ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Name AS Name 
                             ,ProjectDefinition.ProjManagementLevelId AS projectManagementLevelId  ,ProjectDefinition.ProjTemplateId AS projectTemplateId  ,ProjectDefinition.ProjToleranceStateId 
                             AS projectToleranceStateId ,ProjectDefinition.ProjTypeId AS projectTypeId  ,ProjectDefinition.Title AS Title ,ProjectDefinition.GeneralLedgerId,ProjectScopeStatusId
                             FROM dbo.ProjectDefinition WHERE ProjectDefinition.SequenceCode = '" + id + "'";

        ProjectDefinitionDto result = null;
        using (IDbConnection dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
        {
            result = dbConnection.Query<ProjectDefinitionDto>(sql).FirstOrDefault();
        }

        List<ProjectDefinitionHistoryLogDapperDto> historydDapperDtos = null;

        var sqlgel = "SELECT * FROM GenaralLederNumber WHERE id=@id";
        var parameters = new { id = result.GeneralLedgerId, ProjectId = result.Id, Lang = lang };

        var sql2 =
            "SELECT ProjectTime.Id ,ProjectTime.ExpectedEndDate  ,ProjectTime.StartDate  ,ProjectTime.EndDate  ,ProjectTime.CalendarTemplateId  ,CalendarTemplate.Id  ," +
            "CalendarTemplate.Name FROM dbo.ProjectTime LEFT OUTER JOIN dbo.CalendarTemplate   ON ProjectTime.CalendarTemplateId = CalendarTemplate.Id WHERE ProjectTime.ProjectId = @ProjectId";

        var sql3 =
            "SELECT ProjectFinance.TotalBudget  ,ProjectFinance.BudgetLabour  ,ProjectFinance.Id  ,ProjectFinance.BudgetMaterial,Invoiced,Paid" +
            "  ,ProjectFinance.CurrencyId  ,Currency.Id  ,Currency.Name FROM dbo.ProjectFinance LEFT OUTER JOIN dbo.Currency   ON ProjectFinance.CurrencyId = Currency.Id" +
            " WHERE ProjectFinance.ProjectId = @ProjectId";

        var sql4 =
            "SELECT  ProjectKPI.CustomLabelOne  ,ProjectKPI.Id  ,ProjectKPI.CustomPropOne  ,ProjectKPI.CustomLabelTwo  ,ProjectKPI.CustomPropTwo  ,ProjectKPI.CustomLabelThree  ,ProjectKPI.CustomPropThree FROM dbo.ProjectKPI" +
            " WHERE ProjectKPI.ProjectId = @ProjectId";

        var sql5 =
            "SELECT   ProjectDefinitionHistoryLog.ChangedTime AS DateTime  ,ApplicationUser.FirstName AS [User]  ,ProjectDefinitionHistoryLog.RevisionNumber AS RevisionNumber  " +
            "FROM dbo.ProjectDefinitionHistoryLog LEFT OUTER JOIN ApplicationUser  ON ProjectDefinitionHistoryLog.ChangedByUserId = ApplicationUser.Id  WHERE ProjectDefinitionHistoryLog.ProjectDefinitionId = @ProjectId ORDER BY RevisionNumber";

        var sql6 =
            "select ProjectTeam.Id, ProjectTeam.ContractingUnitId,  CabCompany.Name AS ContractingUnitName from ProjectTeam LEFT OUTER JOIN CabCompany on ProjectTeam.ContractingUnitId = CabCompany.Id where ProjectId =  @ProjectId";

        var sql7 =
            @"SELECT ProjectTeamRole.Id,CabPerson.FullName AS CabPersonName,CabPerson.Id AS CabPersonId,Role.RoleName,Role.RoleId AS RoleId,ProjectTeamRole.status ,ProjectTeamRole.IsAccessGranted ,Role.LanguageCode FROM dbo.ProjectTeamRole LEFT OUTER JOIN dbo.CabPerson ON ProjectTeamRole.CabPersonId = CabPerson.Id LEFT OUTER JOIN dbo.Role ON ProjectTeamRole.RoleId = Role.RoleId LEFT OUTER JOIN dbo.ProjectTeam
  ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id WHERE ProjectTeam.ProjectId = @ProjectId AND Role.LanguageCode = @Lang";

        using (IDbConnection dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
        {
            result.ProjectTime = dbConnection.Query<ProjectTime, CalendarTemplate, ProjectTime>(sql2,
                (projectTime, calendarTemplate) =>
                {
                    projectTime.CalendarTemplate = calendarTemplate;
                    return projectTime;
                }, parameters).FirstOrDefault();

            result.GenaralLedgerNumber =
                dbConnection.Query<GeneralLedgerNumber>(sqlgel, parameters).FirstOrDefault();

            result.ProjectFinance = dbConnection.Query<ProjectFinance, Currency, ProjectFinance>(sql3,
                (projectFinance, currency) =>
                {
                    projectFinance.Currency = currency;
                    return projectFinance;
                }, parameters).FirstOrDefault();

            result.ProjectKpi = dbConnection.Query<ProjectKPI>(sql4, parameters).FirstOrDefault();

            result.ProjectTeam = dbConnection.Query<ProjectTeamUpdateDto>(sql6, parameters).FirstOrDefault();

            if (result.ProjectTeam != null)
                result.ProjectTeam.TeamRoleList = dbConnection.Query<ProjectTeamRoleReadDto>(sql7, parameters);

            historydDapperDtos = dbConnection.Query<ProjectDefinitionHistoryLogDapperDto>(sql5, parameters)
                .ToList();

           
        }

        var historyLogDto = new ProjectDefinitionHistoryLogDto();
        if (historydDapperDtos.Any())
        {
            var dto = historydDapperDtos.FirstOrDefault();
            historyLogDto.CreatedDateTime = dto.DateTime;
            historyLogDto.CreatedByUser = dto.User;
        }

        if (historydDapperDtos.Count >= 2)
        {
            var dto = historydDapperDtos.LastOrDefault();
            historyLogDto.UpdatedDateTime = dto.DateTime;
            historyLogDto.UpdatedByUser = dto.User;
            historyLogDto.RevisionNumber = dto.RevisionNumber;
        }

        result.ProjectHistory = historyLogDto;
        var mapLocation = context.MapLocation.Where(L => L.Id == result.MapLocationId).Include(m => m.Address)
            .Include(m => m.Position).FirstOrDefault();
        result.Location = mapLocation;

        var projectCostConversion =
            context.ProjectCostConversion.FirstOrDefault(p => p.ProjectId.Equals(result.Id));
        if (projectCostConversion != null)
            result.ProjectCostConversionCreateDto = new ProjectCostConversionCreateDto
            {
                ProjectId = projectCostConversion.ProjectId,
                LoadingConversionOption = projectCostConversion.LoadingConversionOption,
                TravelConversionOption = projectCostConversion.TravelConversionOption
            };
        else
            result.ProjectCostConversionCreateDto = new ProjectCostConversionCreateDto();

        return result;
    }

    public async Task<PagedResult<T>> GetProjectDefinitionPagedResult<T>(ApplicationDbContext context, int pageNo)
    {
        throw new NotImplementedException();
    }

    public async Task<ProjectDefinitionDto> GetProjectDefinitionByIdNew(
        ProjectDefinitionParameter ProjectDefinitionParameter)
    {
        var sql =
            @"SELECT ProjectDefinition.Id ,ProjectDefinition.Description AS Description,LocationId AS MapLocationId  ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Name AS Name 
                             ,ProjectDefinition.ProjManagementLevelId AS projectManagementLevelId  ,ProjectDefinition.ProjTemplateId AS projectTemplateId  ,ProjectDefinition.ProjToleranceStateId 
                             AS projectToleranceStateId ,ProjectDefinition.ProjTypeId AS projectTypeId  ,ProjectDefinition.Title AS Title ,ProjectDefinition.GeneralLedgerId,ProjectScopeStatusId,
                             ProjectStatus,Language,CustomerId, CabPerson.FullName As CustomerName ,ProjectManagerId, cb.FullName As ProjectManagerName , SiteManagerId , cb1.FullName As SiteManagerName
                            FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.CabPerson ON ProjectDefinition.CustomerId = CabPerson.Id  LEFT OUTER JOIN dbo.CabPerson cb ON ProjectDefinition.ProjectManagerId = cb.Id LEFT OUTER JOIN dbo.CabPerson cb1 ON ProjectDefinition.SiteManagerId = cb1.Id WHERE ProjectDefinition.SequenceCode =  @SequenceCode";
        var sql2 =
            @"SELECT ProjectTime.Id ,ProjectTime.ExpectedEndDate  ,ProjectTime.StartDate  ,ProjectTime.EndDate  , ProjectTime.TenderStartDate, ProjectTime.TenderEndDate, ProjectTime.CalendarTemplateId  ,CalendarTemplate.Id  ,CalendarTemplate.Name FROM dbo.ProjectTime LEFT OUTER JOIN dbo.CalendarTemplate   ON ProjectTime.CalendarTemplateId = CalendarTemplate.Id WHERE ProjectTime.ProjectId = @ProjectId;
        SELECT * FROM GenaralLederNumber WHERE id=@Id;
        SELECT ProjectFinance.TotalBudget  ,ProjectFinance.BudgetLabour  ,ProjectFinance.Id  ,ProjectFinance.BudgetMaterial,Invoiced,Paid,CustomerBudgetSpent ,CustomerBudget ,DifferenceEstimatedCostAndTenderBudget ,DifferenceTenderAndCustomer ,ExpectedTotalProjectCost ,ExtraWork ,MinAndExtraWork ,MinWork ,TenderBudget ,ToBeInvoiced ,ProjectFinance.CurrencyId  ,Currency.Id  ,Currency.Name FROM dbo.ProjectFinance LEFT OUTER JOIN dbo.Currency   ON ProjectFinance.CurrencyId = Currency.Id WHERE ProjectFinance.ProjectId = @ProjectId;
        SELECT  ProjectKPI.CustomLabelOne  ,ProjectKPI.Id  ,ProjectKPI.CustomPropOne  ,ProjectKPI.CustomLabelTwo  ,ProjectKPI.CustomPropTwo  ,ProjectKPI.CustomLabelThree  ,ProjectKPI.CustomPropThree FROM dbo.ProjectKPI WHERE ProjectKPI.ProjectId = @ProjectId;
        select ProjectTeam.Id, ProjectTeam.ContractingUnitId,  CabCompany.Name AS ContractingUnitName from ProjectTeam LEFT OUTER JOIN CabCompany on ProjectTeam.ContractingUnitId = CabCompany.Id where ProjectId =  @ProjectId;
        SELECT ProjectTeamRole.Id ,CabPerson.FullName AS CabPersonName ,CabPerson.Id AS CabPersonId ,Role.RoleName ,Role.RoleId AS RoleId ,ProjectTeamRole.status ,ProjectTeamRole.IsAccessGranted, ProjectTeamRole.Message, Role.LanguageCode ,CabCompany.name AS CompanyName ,CabEmail.EmailAddress AS Email FROM dbo.ProjectTeamRole LEFT OUTER JOIN dbo.CabPerson ON ProjectTeamRole.CabPersonId = CabPerson.Id LEFT OUTER JOIN dbo.Role ON ProjectTeamRole.RoleId = Role.RoleId LEFT OUTER JOIN dbo.ProjectTeam ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id LEFT OUTER JOIN dbo.CabPersonCompany ON CabPerson.Id = CabPersonCompany.PersonId LEFT OUTER JOIN dbo.CabCompany ON CabPersonCompany.CompanyId = CabCompany.Id LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id WHERE ProjectTeam.ProjectId = @ProjectId AND Role.LanguageCode = @Lang;
          SELECT   ProjectDefinitionHistoryLog.ChangedTime AS DateTime  ,ApplicationUser.FirstName AS [User]  ,ProjectDefinitionHistoryLog.RevisionNumber AS RevisionNumber FROM dbo.ProjectDefinitionHistoryLog LEFT OUTER JOIN ApplicationUser  ON ProjectDefinitionHistoryLog.ChangedByUserId = ApplicationUser.Id  WHERE ProjectDefinitionHistoryLog.ProjectDefinitionId = @ProjectId ORDER BY RevisionNumber;
          SELECT Id ,ProjectId ,ProjectClassificationBuisnessUnit ,ProjectClassificationSizeId ,ProjectClassificationConstructionTypeId ,ProjectClassificationSectorId FROM dbo.ProjectClassification WHERE ProjectId = @ProjectId";

        ProjectDefinitionDto result = null;
        List<ProjectDefinitionHistoryLogDapperDto> historydDapperDtos = null;


        // var parameters = new ProjectParam
        // {
        //     SequenceCode = ProjectDefinitionParameter.Id,
        //     Lang = ProjectDefinitionParameter.Lang,
        //     // Id = "",
        //     // ProjectId = "8e4303fe-65ed-40cb-9f96-2cb9016bad6a"
        // };

        var dbConnection = ProjectDefinitionParameter.TenantProvider.orgSqlConnection();


        result = dbConnection.QueryFirst<ProjectDefinitionDto>(sql,
            new { SequenceCode = ProjectDefinitionParameter.Id });

        using (var multi = await dbConnection.QueryMultipleAsync(sql2,
                   new
                   {
                       Id = result.GeneralLedgerId, ProjectId = result.Id, ProjectDefinitionParameter.Lang
                   }))
        {
            // parameters.Id = result.GeneralLedgerId;
            // parameters.ProjectId = result.Id;

            result.ProjectTime = multi.Read<ProjectTime, CalendarTemplate, ProjectTime>(
                (projectTime, calendarTemplate) =>
                {
                    projectTime.CalendarTemplate = calendarTemplate;
                    return projectTime;
                }).FirstOrDefault();
            result.GenaralLedgerNumber =
                multi.Read<GeneralLedgerNumber>().FirstOrDefault();

            result.ProjectFinance = multi.Read<ProjectFinance, Currency, ProjectFinance>(
                (projectFinance, currency) =>
                {
                    projectFinance.Currency = currency;
                    return projectFinance;
                }).FirstOrDefault();

            result.ProjectKpi = multi.Read<ProjectKPI>().FirstOrDefault();
            result.ProjectTeam = multi.Read<ProjectTeamUpdateDto>().FirstOrDefault();

            if (result.ProjectTeam != null)
                result.ProjectTeam.TeamRoleList = multi.Read<ProjectTeamRoleReadDto>();

            historydDapperDtos = multi.Read<ProjectDefinitionHistoryLogDapperDto>()
                .ToList();
            result.ProjectClassification = multi.Read<ProjectClassificationHeader>().FirstOrDefault();
        }

        result.CiawCode = dbConnection
            .Query<string>("SELECT CiawSiteCode FROM ProjectCiawSite WHERE ProjectId = @projectId",
                new { projectId = result.Id }).FirstOrDefault();

        if (result.CustomerId != null)
        {
            result.CustomerCompanyName = dbConnection
                .Query<string>(
                    "SELECT cc.Name FROM CabPersonCompany cpc LEFT OUTER JOIN CabCompany cc ON cpc.CompanyId = cc.Id WHERE cpc.PersonId = @PersonId",
                    new { PersonId = result.CustomerId }).FirstOrDefault();
        }


        var connectionString = ConnectionString.MapConnectionString(
            ProjectDefinitionParameter.ContractingUnitSequenceId,
            result.SequenceCode, ProjectDefinitionParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        result.WbsTemplateId = connection
            .Query<string>("SELECT TemplateId FROM WbsTaxonomy WHERE TemplateId IS NOT NULL ").FirstOrDefault();

        var historyLogDto = new ProjectDefinitionHistoryLogDto();
        if (historydDapperDtos.Any())
        {
            var dto = historydDapperDtos.FirstOrDefault();
            historyLogDto.CreatedDateTime = dto.DateTime;
            historyLogDto.CreatedByUser = dto.User;
        }

        if (historydDapperDtos.Count >= 2)
        {
            var dto = historydDapperDtos.LastOrDefault();
            historyLogDto.UpdatedDateTime = dto.DateTime;
            historyLogDto.UpdatedByUser = dto.User;
            historyLogDto.RevisionNumber = dto.RevisionNumber;
        }

        result.ProjectHistory = historyLogDto;
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var dbContext1 =
            new ProjectDefinitionDbContext(options1, ProjectDefinitionParameter.TenantProvider);
        var mapLocation = dbContext1.MapLocation.Where(L => L.Id == result.MapLocationId).Include(m => m.Address)
            .Include(m => m.Position).FirstOrDefault();
        result.Location = mapLocation;
        
        var projectCostConversion =
            dbContext1.ProjectCostConversion.FirstOrDefault(p => p.ProjectId.Equals(result.Id));

        if (projectCostConversion != null)
            result.ProjectCostConversionCreateDto = new ProjectCostConversionCreateDto
            {
                ProjectId = projectCostConversion.ProjectId,
                LoadingConversionOption = projectCostConversion.LoadingConversionOption,
                TravelConversionOption = projectCostConversion.TravelConversionOption
            };
        else
            result.ProjectCostConversionCreateDto = new ProjectCostConversionCreateDto();

        result.ProjectTime ??= new ProjectTime()
        {
            ProjectId = result.Id
        };

        result.ProjectFinance ??= new ProjectFinance()
        {
            ProjectId = result.Id
        };
        result.ProjectClassification ??= new ProjectClassificationHeader()
        {
            ProjectId = result.Id
        };
        if (result.Location != null)
        {
            result.Location.Address ??= new Address();

            if (result.Location.Position != null)
            {
                if (result.Location.Position.Lat == null && result.Location.Position.Lon == null)
                {
                    result.Location.Position.Lat = "0.00";
                    result.Location.Position.Lon = "0.00";
                }
            }
        }


        result.GenaralLedgerNumber ??= new GeneralLedgerNumber();

        return result;
    }

    public async Task<string> UpdateProjectDefinition(ApplicationDbContext context,
        ProjectDefinitionUpdateDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        IProjectFinanceRepository iProjectFinanceRepository, IProjectTeamRepository projectTeamRepository,
        ITenantProvider iTenantProvider, ApplicationUser user)
    {
        var _log4net = LogManager.GetLogger(typeof(ProjectDefinitionRepository));
        try
        {
            var project =
                context.ProjectDefinition.FirstOrDefault(p => p.Id == projectDto.Id);
            project.Description = projectDto.Description;
            project.ProjTemplateId = projectDto.ProjectTemplateId;
            project.ContractingUnitId = projectDto.ProjectTeam.ContractingUnitId;
            //project.ChangeByUserId = user.Id;
            //project.Action = HistoryState.UPDATED.ToString();
            if (projectDto.ProjectTypeId == null)
            {
                var projectId = context.ProjectType.First(t => t.IsDefault == true).Id;
                project.ProjTypeId = projectId;
            }
            else
            {
                project.ProjTypeId = projectDto.ProjectTypeId;
            }

            if (projectDto.ProjectManagementLevelId == null)
            {
                var projectManagementLevelId = context.ProjectManagementLevel.First(t => t.IsDefault == true).Id;
                project.ProjManagementLevelId = projectManagementLevelId;
            }
            else
            {
                project.ProjManagementLevelId = projectDto.ProjectManagementLevelId;
            }

            if (projectDto.ProjectToleranceStateId == null)
            {
                var projectToleranceStateId = context.ProjectToleranceState.First(t => t.IsDefault == true).Id;
                project.ProjToleranceStateId = projectToleranceStateId;
            }
            else
            {
                project.ProjToleranceStateId = projectDto.ProjectToleranceStateId;
            }

            project.ProjectScopeStatusId = projectDto.ProjectScopeStatusId;
            project.ProjectStatus = projectDto.ProjectStatus;

            project.LocationId = AddProjectLocation( null, projectDto, iTenantProvider);
            project.Name = projectDto.Name;
            project.GeneralLedgerId = projectDto.GeneralLedgerId;
            project.Title = project.SequenceCode + " - " + project.Name;
            project.CustomerId = projectDto.CustomerId;
            project.Language = projectDto.Language;
            project.ProjectManagerId = projectDto.ProjectManagerId;
            project.SiteManagerId = projectDto.SiteManagerId;


            context.ProjectDefinition.Update(project);
            await context.SaveChangesAsync();

            if (projectDto.Kpi != null) UpdateKpi(context, projectDto.Kpi, user);

            if (projectDto.ProjectCostConversionCreateDto != null)
                CreateProjectCostConversion(project.Id, projectDto.ProjectCostConversionCreateDto, iTenantProvider);

            if (projectDto.ProjectFinance != null)
                iProjectFinanceRepository.UpdateProjectFinance( projectDto.ProjectFinance, iTenantProvider);

            if (projectDto.ProjectTime != null)
                await iProjectTimeRepository.UpdateProjectTime( projectDto.ProjectTime, user, iTenantProvider);

            if (projectDto.ProjectManagerId != null)
            {
                if (projectDto.ProjectTeam.Id != null)
                {
                    var nmm = projectDto.ProjectTeam.TeamRoleList
                        .Where(x => x.RoleId == "266a5f47-3489-484b-8dae-e4468c5329dn3").FirstOrDefault();
                    if (nmm != null)
                        if (nmm.CabPersonId != projectDto.ProjectManagerId)
                        {
                            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                            var removeItem =
                                projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                    x.RoleId == "266a5f47-3489-484b-8dae-e4468c5329dn3");

                            if (removeItem != null)
                            {
                                var parameter = new ProjectTeamRoleParameter
                                {
                                    TenantProvider = iTenantProvider
                                };
                                var idList = new List<string> { removeItem.Id };
                                parameter.IdList = idList;
                                await projectTeamRepository.DeleteProjectAccess(parameter);
                            }

                            await connection.ExecuteAsync(
                                "Delete From ProjectTeamRole Where RoleId = '266a5f47-3489-484b-8dae-e4468c5329dn3' AND ProjectTeamId = @ProjectTeamId",
                                new { ProjectTeamId = projectDto.ProjectTeam.Id });

                            projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                        }

                    var pmExist = context.ProjectTeamRole
                        .FirstOrDefault(x =>
                            x.CabPersonId == projectDto.ProjectManagerId && x.ProjectTeamId == projectDto.ProjectTeam.Id);
                    if (pmExist == null)
                    {
                        projectDto.ProjectTeam.ProjectId = projectDto.Id;
                        var pmAdd = new ProjectTeamRoleCreateDto
                        {
                            CabPersonId = projectDto.ProjectManagerId,
                            RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3",
                            IsAccessGranted = true,
                            status = "1"
                        };

                        projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                    }
                    else
                    {
                        pmExist.RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3";
                        context.ProjectTeamRole.Update(pmExist);
                        await context.SaveChangesAsync();
                        var Item =
                            projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                x.CabPersonId == projectDto.ProjectManagerId);
                        if (Item != null)
                        {
                            projectDto.ProjectTeam.TeamRoleList.Remove(Item);
                            Item.RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3";
                            projectDto.ProjectTeam.TeamRoleList.Add(Item);
                        }
                    }
                }
            }
            else
            {
                var removeItem =
                    projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                        x.RoleId == "266a5f47-3489-484b-8dae-e4468c5329dn3");

                if (removeItem != null)
                {
                    var parameter = new ProjectTeamRoleParameter
                    {
                        TenantProvider = iTenantProvider
                    };
                    var idList = new List<string> { removeItem.Id };
                    parameter.IdList = idList;
                    await projectTeamRepository.DeleteProjectAccess(parameter);
                }

                projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                await connection.ExecuteAsync(
                    "Delete From ProjectTeamRole Where RoleId = '266a5f47-3489-484b-8dae-e4468c5329dn3' AND ProjectTeamId = @ProjectTeamId",
                    new { ProjectTeamId = projectDto.ProjectTeam.Id });
            }

            if (projectDto.CustomerId != null)
            {
                if (projectDto.ProjectTeam.Id != null)
                {
                    var nmm = projectDto.ProjectTeam.TeamRoleList
                        .Where(x => x.RoleId == "910b7af0-b132-4951-a2dc-6ab82d4cd40d").FirstOrDefault();
                    if (nmm != null)
                        if (nmm.CabPersonId != projectDto.CustomerId)
                        {
                            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);

                            var removeItem =
                                projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                    x.RoleId == "910b7af0-b132-4951-a2dc-6ab82d4cd40d");

                            if (removeItem != null)
                            {
                                var parameter = new ProjectTeamRoleParameter
                                {
                                    TenantProvider = iTenantProvider
                                };
                                var idList = new List<string> { removeItem.Id };
                                parameter.IdList = idList;
                                await projectTeamRepository.DeleteProjectAccess(parameter);
                            }

                            await connection.ExecuteAsync(
                                "Delete From ProjectTeamRole Where RoleId = '910b7af0-b132-4951-a2dc-6ab82d4cd40d' AND ProjectTeamId = @ProjectTeamId",
                                new { ProjectTeamId = projectDto.ProjectTeam.Id });

                            projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                        }

                    var pmExist = context.ProjectTeamRole
                        .FirstOrDefault(x =>
                            x.CabPersonId == projectDto.CustomerId && x.ProjectTeamId == projectDto.ProjectTeam.Id);
                    if (pmExist == null)
                    {
                        projectDto.ProjectTeam.ProjectId = projectDto.Id;
                        var pmAdd = new ProjectTeamRoleCreateDto
                        {
                            CabPersonId = projectDto.CustomerId,
                            RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d",
                            IsAccessGranted = true,
                            status = "1"
                        };

                        projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                    }
                    else
                    {
                        var parameter = new ProjectTeamRoleParameter
                        {
                            TenantProvider = iTenantProvider
                        };
                        var idList = new List<string> { pmExist.Id };
                        parameter.IdList = idList;
                        await projectTeamRepository.DeleteProjectAccess(parameter);


                        pmExist.RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d";
                        context.ProjectTeamRole.Update(pmExist);
                        await context.SaveChangesAsync();

                        var Item =
                            projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x => x.CabPersonId == projectDto.CustomerId);

                        if (Item != null)
                        {
                            projectDto.ProjectTeam.TeamRoleList.Remove(Item);
                            Item.RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d";
                            projectDto.ProjectTeam.TeamRoleList.Add(Item);
                        }
                    }
                }
            }
            else
            {
                var removeItem =
                    projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                        x.RoleId == "910b7af0-b132-4951-a2dc-6ab82d4cd40d");
                if (removeItem != null)
                {
                    var parameter = new ProjectTeamRoleParameter
                    {
                        TenantProvider = iTenantProvider
                    };
                    var idList = new List<string> { removeItem.Id };
                    parameter.IdList = idList;
                    await projectTeamRepository.DeleteProjectAccess(parameter);
                }

                projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                await connection.ExecuteAsync(
                    "Delete From ProjectTeamRole Where RoleId = '910b7af0-b132-4951-a2dc-6ab82d4cd40d' AND ProjectTeamId = @ProjectTeamId",
                    new { ProjectTeamId = projectDto.ProjectTeam.Id });
            }

            if (projectDto.SiteManagerId != null)
            {
                if (projectDto.ProjectTeam.Id != null)
                {
                    var nmm = projectDto.ProjectTeam.TeamRoleList
                        .Where(x => x.RoleId == "yyyyyyy-a513-45e0-a431-170dbd4yyyy").FirstOrDefault();
                    if (nmm != null)
                        if (nmm.CabPersonId != projectDto.SiteManagerId)
                        {
                            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                            var removeItem =
                                projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                    x.RoleId == "yyyyyyy-a513-45e0-a431-170dbd4yyyy");

                            if (removeItem != null)
                            {
                                var parameter = new ProjectTeamRoleParameter
                                {
                                    TenantProvider = iTenantProvider
                                };
                                var idList = new List<string> { removeItem.Id };
                                parameter.IdList = idList;
                                await projectTeamRepository.DeleteProjectAccess(parameter);
                            }

                            await connection.ExecuteAsync(
                                "Delete From ProjectTeamRole Where RoleId = 'yyyyyyy-a513-45e0-a431-170dbd4yyyy' AND ProjectTeamId = @ProjectTeamId",
                                new { ProjectTeamId = projectDto.ProjectTeam.Id });

                            projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                        }

                    var pmExist = context.ProjectTeamRole
                        .FirstOrDefault(x =>
                            x.CabPersonId == projectDto.SiteManagerId && x.ProjectTeamId == projectDto.ProjectTeam.Id);
                    if (pmExist == null)
                    {
                        projectDto.ProjectTeam.ProjectId = projectDto.Id;
                        var pmAdd = new ProjectTeamRoleCreateDto
                        {
                            CabPersonId = projectDto.SiteManagerId,
                            RoleId = "yyyyyyy-a513-45e0-a431-170dbd4yyyy",
                            IsAccessGranted = true,
                            status = "1"
                        };

                        projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                    }
                    else
                    {
                        pmExist.RoleId = "yyyyyyy-a513-45e0-a431-170dbd4yyyy";
                        context.ProjectTeamRole.Update(pmExist);
                        await context.SaveChangesAsync();

                        var Item =
                            projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                x.CabPersonId == projectDto.SiteManagerId);

                        if (Item != null)
                        {
                            projectDto.ProjectTeam.TeamRoleList.Remove(Item);
                            Item.RoleId = "yyyyyyy-a513-45e0-a431-170dbd4yyyy";
                            projectDto.ProjectTeam.TeamRoleList.Add(Item);
                        }
                    }
                }
            }
            else
            {
                var removeItem =
                    projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                        x.RoleId == "yyyyyyy-a513-45e0-a431-170dbd4yyyy");

                if (removeItem != null)
                {
                    var parameter = new ProjectTeamRoleParameter
                    {
                        TenantProvider = iTenantProvider
                    };
                    var idList = new List<string> { removeItem.Id };
                    parameter.IdList = idList;
                    await projectTeamRepository.DeleteProjectAccess(parameter);
                }

                projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                await connection.ExecuteAsync(
                    "Delete From ProjectTeamRole Where RoleId = 'yyyyyyy-a513-45e0-a431-170dbd4yyyy' AND ProjectTeamId = @ProjectTeamId",
                    new { ProjectTeamId = projectDto.ProjectTeam.Id });
            }

            if (projectDto.ProjectTeam != null)
            {
                projectDto.ProjectTeam.ProjectId = projectDto.Id;
                await projectTeamRepository.CreateProjectTeam( projectDto.ProjectTeam, iTenantProvider,
                    user);
            }


            if (projectDto.ProjectClassification != null)
            {
                var existClassification =
                    context.ProjectClassification.FirstOrDefault(p => p.ProjectId == projectDto.Id);

                await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                var param = new
                {
                    id = Guid.NewGuid().ToString(),
                    ProjectId = projectDto.Id,
                    projectDto.ProjectClassification.ProjectClassificationBuisnessUnit,
                    projectDto.ProjectClassification.ProjectClassificationSizeId,
                    projectDto.ProjectClassification
                        .ProjectClassificationConstructionTypeId,
                    projectDto.ProjectClassification.ProjectClassificationSectorId
                };
                if (existClassification != null)
                {
                    var classification =
                        "UPDATE dbo.ProjectClassification SET ProjectClassificationBuisnessUnit = @ProjectClassificationBuisnessUnit ,ProjectClassificationSizeId = @ProjectClassificationSizeId ,ProjectClassificationConstructionTypeId = @ProjectClassificationConstructionTypeId ,ProjectClassificationSectorId = @ProjectClassificationSectorId WHERE ProjectId = @ProjectId ;";

                    await connection.ExecuteAsync(classification, param);
                }
                else
                {
                    var classificationInsert =
                        "INSERT INTO dbo.ProjectClassification ( Id ,ProjectId ,ProjectClassificationBuisnessUnit ,ProjectClassificationSizeId ,ProjectClassificationConstructionTypeId ,ProjectClassificationSectorId ) VALUES ( @Id ,@ProjectId ,@ProjectClassificationBuisnessUnit ,@ProjectClassificationSizeId ,@ProjectClassificationConstructionTypeId ,@ProjectClassificationSectorId )";

                    await connection.ExecuteAsync(classificationInsert, param);
                }
            }

            var jsonProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var pdhl = new ProjectDefinitionHistoryLog
            {
                Action = HistoryState.UPDATED.ToString(),
                ChangedByUserId = user.Id,
                Id = Guid.NewGuid().ToString(),
                HistoryLog = jsonProject,
                ChangedTime = DateTime.UtcNow,
                ProjectDefinitionId = project.Id
            };
            context.ProjectDefinitionHistoryLog.Add(pdhl);
            await context.SaveChangesAsync();
            return project.Id;
    }
    catch (Exception ex)
    {
        _log4net.Info("Exception thrown project Create");
        _log4net.Error(ex.ToString());
        throw new Exception(ex.ToString());

    }
    }

    public async Task<IEnumerable<ProjectDefinitionHistory>> GetProjectDefinitionsHistory(
        ApplicationDbContext context)
    {
        return null;
    }

    public async Task<IEnumerable<AllProjectAttributes>> Filter(ApplicationDbContext context, ProjectFilter filter,
        string lang, ITenantProvider iTenantProvider)
    {
        var sql =
            "SELECT   ProjectDefinition.Name AS Name  ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Id AS ProjectDefinitionId  ,ProjectTemplate.Name AS ProjectTemplateName" +
            "  ,ProjectToleranceState.Name AS ProjectToleranceStateName  ,ProjectType.Name AS ProjectTypeName  ,ProjectManagementLevel.Name AS ProjectManagementLevelName, ProjectDefinition.Title AS Title,cc.SequenceCode As ContractingUnitId" +
            " FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectTemplate   ON ProjectDefinition.ProjTemplateId = ProjectTemplate.TemplateId" +
            " LEFT OUTER JOIN dbo.ProjectToleranceState   ON ProjectDefinition.ProjToleranceStateId = ProjectToleranceState.ProjectToleranceStateId" +
            " LEFT OUTER JOIN dbo.ProjectType   ON ProjectDefinition.ProjTypeId = ProjectType.ProjectTypeId LEFT OUTER JOIN dbo.ProjectManagementLevel" +
            "   ON ProjectDefinition.ProjManagementLevelId = ProjectManagementLevel.ProjectManagementLevelId LEFT OUTER JOIN CabCompany cc ON ProjectDefinition.ContractingUnitId = cc.Id WHERE (ProjectTemplate.LanguageCode = '" +
            lang + "'" +
            " OR ProjectDefinition.ProjTemplateId IS NULL) AND ProjectToleranceState.LanguageCode = '" + lang +
            "' AND ProjectType.LanguageCode = '" + lang + "' AND ProjectManagementLevel.LanguageCode = '" + lang +
            "'";

        var sb = new StringBuilder(sql);

        if (filter.ManagementLevelId != null)
            sb.Append(" AND ProjectDefinition.ProjManagementLevelId ='" + filter.ManagementLevelId + "'");

        if (filter.ProjectTypeId != null)
            sb.Append(" AND ProjectDefinition.ProjTypeId ='" + filter.ProjectTypeId + "'");

        if (filter.Title != null)
        {
            filter.Title = filter.Title.Replace("'", "''");
            sb.Append(" AND ProjectDefinition.Title like '%" + filter.Title + "%' ");
        }

        if (filter.ToleranceStateId != null)
            sb.Append(" AND ProjectDefinition.ProjToleranceStateId ='" + filter.ToleranceStateId + "'");

        if (filter.Sorter.Attribute == null)
            sb.Append(
                " ORDER BY cast((select SUBSTRING(ProjectDefinition.SequenceCode, PATINDEX('%[0-9]%', ProjectDefinition.SequenceCode), LEN(ProjectDefinition.SequenceCode))) as int) desc");

        if (filter.Sorter != null)
        {
            if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("asc"))
                sb.Append(" ORDER BY " + filter.Sorter.Attribute + " ASC");

            if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc"))
                sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
        }

        await using (var dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
        {
            var result = await dbConnection.QueryAsync<AllProjectAttributes>(sb.ToString());
            return result;
        }
    }

    public async Task<ProjectDropdown> GetDropdownData(ApplicationDbContext context, string lang,
        IProjectManagementLevelRepository projectManagementLevelRepository,
        IProjectToleranceStateRepository projectToleranceStateRepository,
        IProjectTypeRepository projectTypeRepository, IProjectStateRepository projectStateRepository,
        IProjectTemplateRepository projectTemplateRepository, ICurrencyRepository currencyRepository,
        ITenantProvider iTenantProvider, string ProjectSqCode, string Cu)
    {
        var dropdown = new ProjectDropdown();
        var projectManagementLevels =
            await projectManagementLevelRepository.GetProjectManagementLevels(context, lang, iTenantProvider);
        var projectTypes =
            await projectTypeRepository.GetProjectTypes(context, lang, iTenantProvider);
        var projectStates = await projectStateRepository.GetProjectStates(context, lang, iTenantProvider);
        var projectToleranceStates =
            await projectToleranceStateRepository.GetProjectToleranceStates(context, lang, iTenantProvider);
        var currencies = await currencyRepository.GetCurrencies(context);
        var projectTemplates =
            await projectTemplateRepository.GetProjectTemplates(context, lang, iTenantProvider);

        var query =
            @"SELECT Name, StatusId as Id, LanguageCode,DisplayOrder FROM ProjectScopeStatus WHERE LanguageCode = @lang ORDER BY DisplayOrder;
                SELECT Title AS [Text],Id AS [Key] FROM dbo.OrganizationTaxonomy WHERE OrganizationTaxonomyLevelId = 'oo10e768-3e06-po02-b337-ee367a82admn' AND ParentId = (SELECT Id FROM OrganizationTaxonomy WHERE Title = @Cu AND OrganizationTaxonomyLevelId = '2210e768-3e06-po02-b337-ee367a82adjj') ORDER BY Title;
                SELECT Name AS [Text], TypeId AS [Key] FROM ProjectClassificationSize WHERE LanguageCode = @lang ORDER BY DisplayOrder;
                SELECT Name AS [Text], TypeId AS [Key] FROM ProjectClassificationConstructionType WHERE LanguageCode = @lang ORDER BY DisplayOrder;
                SELECT Name AS [Text], TypeId AS [Key] FROM ProjectClassificationSector WHERE LanguageCode = @lang ORDER BY DisplayOrder;
                SELECT Name AS [Text], TypeId AS [Key] FROM ProjectLanguage ORDER BY DisplayOrder";


        var parameters = new { lang, Cu };
        var muilti = iTenantProvider.orgSqlConnection().QueryMultiple(query, parameters);
        dropdown.ProjectScopeStatus = muilti.Read<ProjectScopeStatus>();
        dropdown.ProjectClassificationBuisnessUnit = muilti.Read<ProjectClassificationTypeDto>();
        dropdown.ProjectClassificationSize = muilti.Read<ProjectClassificationTypeDto>();
        dropdown.ProjectClassificationConstructionType = muilti.Read<ProjectClassificationTypeDto>();
        dropdown.ProjectClassificationSector = muilti.Read<ProjectClassificationTypeDto>();
        dropdown.ProjectLanguage = muilti.Read<ProjectLanguageDto>();
        IEnumerable<RoleDto> role;

        role = iTenantProvider.orgSqlConnection().Query<RoleDto>(
            "SELECT Id, TenantId, LanguageCode,RoleName AS Name ,RoleId FROM role WHERE LanguageCode = @lang AND RoleId NOT in ('910b7af0-b132-4951-a2dc-6ab82d4cd40d','266a5f47-3489-484b-8dae-e4468c5329dn3')",
            new { lang, SequenceCode = ProjectSqCode });

        dropdown.Role = role;
        dropdown.Currencies = currencies;
        dropdown.ManagementLevels = projectManagementLevels;
        dropdown.States = projectStates;
        dropdown.Templates = projectTemplates;
        dropdown.ToleranceStates = projectToleranceStates;
        dropdown.Types = projectTypes;
        dropdown.GenaralLederNumber = await context.GenaralLederNumber.ToListAsync();

        return dropdown;
    }

    public async Task<string> UpdateProjectViewTime(ApplicationDbContext context, string projectId,
        ApplicationUser user, string lang, ITenantProvider iTenantProvider)
    {
        var project = context.ProjectDefinition.FirstOrDefault(p => p.Id == projectId);
        if (project != null)
        {
            var lspd = new LastSeenProjectDefinition
            {
                ProjectId = projectId,
                ViewedUserId = user.Id,
                ViewTime = DateTime.UtcNow
            };
            context.LastSeenProjectDefinition.Update(lspd);
            await context.SaveChangesAsync();
            return project.Id;
        }

        throw new Exception("projectNotFound");
    }

    public async Task<IEnumerable<ProjectDefinitionLastSeenDto>> ReadLastSeenProjects(ApplicationDbContext context,
        int numberOfRecords, string ApplicationUserid, ITenantProvider itenantProvider, string oid)
    {
        var sql = @"SELECT DISTINCT
  ProjectDefinition.SequenceCode AS SequenceCode
 ,CabCompany.SequenceCode AS ContractingUnitId
 ,ProjectDefinition.Name AS ProjectName
 ,cp.FullName AS ProjectManager
FROM dbo.ProjectDefinition
INNER JOIN dbo.CabCompany
  ON ProjectDefinition.ContractingUnitId = CabCompany.Id
  LEFT OUTER JOIN CabPerson cp ON ProjectDefinition.ProjectManagerId = cp.Id
WHERE ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.Id IN @Ids";

        var lastSql = @"SELECT 
  *
FROM dbo.LastSeenProjectDefinition
WHERE LastSeenProjectDefinition.ViewedUserId = @ViewedUserId AND LastSeenProjectDefinition.ProjectId IN @projects
ORDER BY LastSeenProjectDefinition.ViewTime DESC";
        IEnumerable<ProjectDefinitionLastSeenDto> result;
        IEnumerable<LastSeenProjectDefinition> resultLastSeenProjectDefinition;
        List<string> projects;

        var projectSql = @"SELECT DISTINCT
                              ProjectDefinition.Id
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectUserRole
                              ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                            LEFT OUTER JOIN dbo.UserRole
                              ON ProjectUserRole.UsrRoleId = UserRole.Id
                            LEFT OUTER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserOid = ApplicationUser.OId
                           WHERE ApplicationUser.OId = @oid
                             AND ProjectDefinition.IsDeleted = 0";

        await using (var connection = new SqlConnection(itenantProvider.GetTenant().ConnectionString))
        {
            projects = connection.Query<string>(projectSql, new { oid }).ToList();
        }

        await using (var connection = new SqlConnection(itenantProvider.GetTenant().ConnectionString))
        {
            resultLastSeenProjectDefinition = connection
                .QueryAsync<LastSeenProjectDefinition>(lastSql, new { ViewedUserId = ApplicationUserid, projects })
                .Result
                .ToList();
        }

        var resultOut = new List<LastSeenProjectDefinition>();
        var remainingHorsemen = new Dictionary<string, LastSeenProjectDefinition>();

        foreach (var p in resultLastSeenProjectDefinition)
            if (!remainingHorsemen.TryGetValue(p.ProjectId, out _))
            {
                remainingHorsemen.Add(p.ProjectId, p);
                resultOut.Add(p);
            }

        await using (var connection = new SqlConnection(itenantProvider.GetTenant().ConnectionString))
        {
            result = connection.QueryAsync<ProjectDefinitionLastSeenDto>(sql,
                new { Ids = resultOut.Select(r => r.ProjectId).Take(numberOfRecords).ToList() }).Result.ToList();
        }

        return result;
    }

    public async Task<IEnumerable<ProjectDefinitionHistoryLog>> GetProjectDefinitionsHistoryLog(
        ApplicationDbContext context)
    {
        var projectDefinitionHistoryLogs = context.ProjectDefinitionHistoryLog.ToList();
        return projectDefinitionHistoryLogs;
    }

    public async Task<IEnumerable<ProjectQrCodeDto>> ProjectsForQr(ApplicationDbContext context,
        ITenantProvider iTenantProvider, string title)
    {
        var sb =
            new StringBuilder("SELECT ProjectDefinition.Id AS [Key],  ProjectDefinition.Title AS Text ");
        if (title == "-1")
            sb.Append("FROM dbo.ProjectDefinition  ");
        else
            sb.Append("FROM dbo.ProjectDefinition where ProjectDefinition.Title like '%" + title?.Replace("'", "''") +
                      "%' ");

        sb.Append(
            "ORDER BY cast((select SUBSTRING(ProjectDefinition.SequenceCode, PATINDEX('%[0-9]%', ProjectDefinition.SequenceCode), LEN(ProjectDefinition.SequenceCode))) as int) desc");

        using var dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
        var param = new { title };
        await dbConnection.OpenAsync();
        var result = await dbConnection.QueryAsync<ProjectQrCodeDto>(sb.ToString(), param);
        return result;
    }


    public async Task<MapLocation> GetLocationByProjectId(string id, string lang, ITenantProvider itenantProvider)
    {
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var applicationDbContext = new ProjectDefinitionDbContext(options1, itenantProvider);

        var project = applicationDbContext.ProjectDefinition.FirstOrDefault(p => p.SequenceCode == id);
        var mapLocation = applicationDbContext.MapLocation.Where(L => L.Id == project.LocationId)
            .Include(m => m.Address).Include(m => m.Position)
            .Include(m => m.Viewport).ThenInclude(v => v.BtmRightPoint)
            .Include(m => m.Viewport).ThenInclude(v => v.TopLeftPoint)
            .Include(b => b.BoundingBox).ThenInclude(v => v.BtmRightPoint)
            .Include(b => b.BoundingBox).ThenInclude(v => v.TopLeftPoint)
            .Include(d => d.DataSources).ThenInclude(d => d.Geometry).ToList().FirstOrDefault();

        return mapLocation;
    }

    public async Task<projectData> GetProjectDataById(ApplicationDbContext uPrinceCustomerContext, string id,
        string lang, ITenantProvider itenantProvider)
    {
        var sql = @"SELECT
                     CabPerson.FullName AS PmName
                    ,ProjectDefinition.SequenceCode AS SequenceCode
                    , ProjectDefinition.Title AS Title
                    , CabPerson.BusinessPhoneId AS MobileNumber
                    , CabPerson.Image AS Image, dbo.Role.RoleName
                      FROM ProjectDefinition
                      LEFT OUTER JOIN dbo.ProjectTeam
                      ON ProjectTeam.ProjectId = ProjectDefinition.Id
                      LEFT OUTER JOIN ProjectTeamRole
                      ON ProjectTeamRole.ProjectTeamId=ProjectTeam.Id
                      LEFT OUTER JOIN dbo.Role
                     ON ProjectTeamRole.RoleId = Role.Id
                     LEFT OUTER JOIN dbo.CabPerson
                    ON ProjectTeamRole.CabPersonId = CabPerson.Id
                    WHERE ProjectDefinition.SequenceCode = '" + id + "'";

        using var dbConnection = new SqlConnection(itenantProvider.GetTenant().ConnectionString);
        await dbConnection.OpenAsync();
        var result = await dbConnection.QueryAsync<projectData>(sql);
        var projectDatas = result as projectData[] ?? result.ToArray();
        var pmRecord = projectDatas.FirstOrDefault(r => r.RoleName == "Project Manager");
        return pmRecord ?? projectDatas.FirstOrDefault();
    }

    public async Task<string> ProjectsUpdateVAT(ApplicationDbContext context, ITenantProvider iTenantProvider,
        ProjectDefinitionVATDto projectDefinitionVATDto)
    {
        try
        {
            var PDUpdateQuery = @"UPDATE dbo.ProjectDefinition 
                                                        SET
                                                          VATId = @VATId
                                                        WHERE
                                                          SequenceCode = @id";

            var isDefaultReset = @"UPDATE dbo.Tax SET IsDefault = 0 WHERE IsDefault = 1";

            var TAXUpdateQuery = @"UPDATE dbo.TAX SET IsDefault = 1 WHERE Id = @VATID";

            var param = new { id = projectDefinitionVATDto.Id, projectDefinitionVATDto.VATId };

            await using (var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
            {
                connection.Query(PDUpdateQuery, param);
                connection.Query(isDefaultReset);
                connection.Query(TAXUpdateQuery, param);
            }

            return projectDefinitionVATDto.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<string> AddProjectConfiguration(ApplicationDbContext context, ITenantProvider iTenantProvider,
        ProjectConfigurationDto projectConfiguration)
    {
        try
        {
            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);

            var insertquery =
                "INSERT INTO dbo.ProjectConfiguration ( Id ,ProjectId ,UnitPrice ) VALUES ( NEWID() ,@ProjectId ,@UnitPrice )";

            var updateQuery = "Update ProjectConfiguration Set UnitPrice = @UnitPrice Where Id =@Id ";

            var selectProjectConfig =
                connection.Query<ProjectConfiguration>(
                        "Select * from ProjectConfiguration where ProjectId = @ProjectId", projectConfiguration)
                    .FirstOrDefault();

            // if (projectConfiguration.ProjectId != null && projectConfiguration.UnitPrice == null)
            // {
            //     var result =
            //         connection.Query<ProjectConfiguration>(
            //             "Select * from ProjectConfiguration where ProjectId = @ProjectId", projectConfiguration).FirstOrDefault();
            //
            //     projectConfiguration.UnitPrice = result?.UnitPrice;
            // }
            // else
            // {

            if (selectProjectConfig != null)
            {
                if (projectConfiguration.IsRequestingData)
                {
                    projectConfiguration.UnitPrice = selectProjectConfig.UnitPrice;
                }
                else
                {
                    selectProjectConfig.UnitPrice = projectConfiguration.UnitPrice;
                    await connection.ExecuteAsync(updateQuery, selectProjectConfig);

                    if (projectConfiguration.UnitPrice == null)
                        await connection.ExecuteAsync("Delete from ProjectConfiguration Where Id = @Id",
                            selectProjectConfig);
                }
            }
            else
            {
                await connection.ExecuteAsync(insertquery, projectConfiguration);
            }

            //}


            return projectConfiguration.UnitPrice;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ProjectCreateReturnResponse> CreateProjectForTh(ApplicationDbContext context,
        ProjectDefinitionCreateDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        IProjectFinanceRepository iProjectFinanceRepository, IProjectTeamRepository projectTeamRepository,
        ITenantProvider iTenantProvider, ApplicationUser user, IPbsRepository _IPbsRepository)
    {
        try
        {
            var response = new ProjectCreateReturnResponse();
            var project = new ProjectDefinition
            {
                SequenceCode = GetSequenceNumber(context),
                Description = projectDto.Description,
                Id = Guid.NewGuid().ToString()
            };
            if (projectDto.ProjectTeam != null) project.ContractingUnitId = projectDto.ProjectTeam.ContractingUnitId;

            project.ProjectConnectionString = CreateDatabase(project.SequenceCode, context, iTenantProvider,
                projectDto._iConfiguration);
            project.Action = HistoryState.ADDED.ToString();
            project.ProjTemplateId = projectDto.ProjectTemplateId;
            if (projectDto.ProjectTypeId == null)
            {
                var projectId = context.ProjectType.First(t => t.IsDefault == true).Id;
                project.ProjTypeId = projectId;
            }
            else
            {
                project.ProjTypeId = projectDto.ProjectTypeId;
            }

            if (projectDto.ProjectManagementLevelId == null)
            {
                var projectManagementLevelId = context.ProjectManagementLevel.First(t => t.IsDefault == true).Id;
                project.ProjManagementLevelId = projectManagementLevelId;
            }
            else
            {
                project.ProjManagementLevelId = projectDto.ProjectManagementLevelId;
            }

            if (projectDto.ProjectToleranceStateId == null)
            {
                var projectToleranceStateId = context.ProjectToleranceState.First(t => t.IsDefault == true).Id;
                project.ProjToleranceStateId = projectToleranceStateId;
            }
            else
            {
                project.ProjToleranceStateId = projectDto.ProjectToleranceStateId;
            }

            project.ProjectScopeStatusId = projectDto.ProjectScopeStatusId;
            project.ProjectStatus = projectDto.ProjectStatus;


            project.LocationId = AddProjectLocation( projectDto, null, iTenantProvider);
            project.Name = projectDto.Name;
            project.GeneralLedgerId = projectDto.GeneralLedgerId;

            if (projectDto.SequenceId != null)
            {
                project.Title = projectDto.SequenceId + " - " + project.Name;
            }
            else
            {
                project.Title = project.SequenceCode + " - " + project.Name;
            }

            project.CustomerId = projectDto.CustomerId;
            project.Language = projectDto.Language;
            project.ProjectManagerId = projectDto.ProjectManagerId;
            project.SiteManagerId = projectDto.SiteManagerId;

            context.ProjectDefinition.Add(project);
            await context.SaveChangesAsync();

            var pId = project.Id;

            projectDto.ProjectTime.ProjectId = pId;
            await iProjectTimeRepository.CreateProjectTime( projectDto.ProjectTime, user, iTenantProvider);

            if (projectDto.ProjectTeam != null)
            {
                projectDto.ProjectTeam.ProjectId = pId;
                await projectTeamRepository.CreateProjectTeam( projectDto.ProjectTeam, iTenantProvider,
                    user);
            }


            var jsonProject = JsonConvert.SerializeObject(project, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var pdhl = new ProjectDefinitionHistoryLog
            {
                Action = HistoryState.ADDED.ToString(),
                ChangedByUserId = user.Id,
                Id = Guid.NewGuid().ToString(),
                HistoryLog = jsonProject,
                ChangedTime = DateTime.UtcNow,
                ProjectDefinitionId = project.Id
            };
            context.ProjectDefinitionHistoryLog.Add(pdhl);
            await context.SaveChangesAsync();
            response.ProjectId = project.Id;
            response.SequenceId = project.SequenceCode;
            response.ProjectConnectionString = project.ProjectConnectionString;

            return response;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    public async Task<string> UpdateProjectDefinitionForTh(
        ProjectDefinitionUpdateDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        ITenantProvider iTenantProvider, ApplicationUser user)
    {
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var context =
            new ProjectDefinitionDbContext(options1,iTenantProvider);
        var project =
            context.ProjectDefinition.FirstOrDefault(p => p.Id == projectDto.Id);
        project.Description = projectDto.Description;
        project.ProjTemplateId = projectDto.ProjectTemplateId;
        project.ContractingUnitId = projectDto.Cu;
        //project.ChangeByUserId = user.Id;
        //project.Action = HistoryState.UPDATED.ToString();
        if (projectDto.ProjectTypeId == null)
        {
            var projectId = context.ProjectType.First(t => t.IsDefault == true).Id;
            project.ProjTypeId = projectId;
        }
        else
        {
            project.ProjTypeId = projectDto.ProjectTypeId;
        }

        if (projectDto.ProjectManagementLevelId == null)
        {
            var projectManagementLevelId = context.ProjectManagementLevel.First(t => t.IsDefault == true).Id;
            project.ProjManagementLevelId = projectManagementLevelId;
        }
        else
        {
            project.ProjManagementLevelId = projectDto.ProjectManagementLevelId;
        }

        if (projectDto.ProjectToleranceStateId == null)
        {
            var projectToleranceStateId = context.ProjectToleranceState.First(t => t.IsDefault == true).Id;
            project.ProjToleranceStateId = projectToleranceStateId;
        }
        else
        {
            project.ProjToleranceStateId = projectDto.ProjectToleranceStateId;
        }

        project.ProjectScopeStatusId = projectDto.ProjectScopeStatusId;
        project.ProjectStatus = projectDto.ProjectStatus;

        project.LocationId = AddProjectLocation( null, projectDto,iTenantProvider);
        project.Name = projectDto.Name;
        project.GeneralLedgerId = projectDto.GeneralLedgerId;
        project.Title = project.SequenceCode + " - " + project.Name;
        project.CustomerId = projectDto.CustomerId;
        project.Language = projectDto.Language;
        project.ProjectManagerId = projectDto.ProjectManagerId;
        project.SiteManagerId = projectDto.SiteManagerId;


        context.ProjectDefinition.Update(project);
        await context.SaveChangesAsync();

        if (projectDto.ProjectTime != null)
            await iProjectTimeRepository.UpdateProjectTime( projectDto.ProjectTime, user, iTenantProvider);


        var jsonProject = JsonConvert.SerializeObject(project, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var pdhl = new ProjectDefinitionHistoryLog
        {
            Action = HistoryState.UPDATED.ToString(),
            ChangedByUserId = user.Id,
            Id = Guid.NewGuid().ToString(),
            HistoryLog = jsonProject,
            ChangedTime = DateTime.UtcNow,
            ProjectDefinitionId = project.Id
        };
        context.ProjectDefinitionHistoryLog.Add(pdhl);
        await context.SaveChangesAsync();
        return project.Id;
    }

    private void CreateProjectCostConversion(string pId,
        ProjectCostConversionCreateDto projectCostConversionCreateDto, ITenantProvider iTenantProvider)
    {
        
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var dbContext1 =
            new ProjectDefinitionDbContext(options1,iTenantProvider);
        
        var isExist = dbContext1.ProjectCostConversion
            .Any(c => c.ProjectId.Equals(projectCostConversionCreateDto.ProjectId));
        if (projectCostConversionCreateDto.ProjectId == "0" ||
            string.IsNullOrEmpty(projectCostConversionCreateDto.ProjectId) || isExist == false)
        {
            var projectCostConversion = new ProjectCostConversion
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = pId,
                LoadingConversionOption = projectCostConversionCreateDto.LoadingConversionOption,
                TravelConversionOption = projectCostConversionCreateDto.TravelConversionOption
            };
            dbContext1.ProjectCostConversion.Add(projectCostConversion);
            dbContext1.SaveChanges();
        }
        else
        {
            var existingprojectCostConversion = dbContext1.ProjectCostConversion
                .FirstOrDefault(c => c.ProjectId.Equals(projectCostConversionCreateDto.ProjectId));
            existingprojectCostConversion.LoadingConversionOption =
                projectCostConversionCreateDto.LoadingConversionOption;
            existingprojectCostConversion.TravelConversionOption =
                projectCostConversionCreateDto.TravelConversionOption;
            dbContext1.ProjectCostConversion.Update(existingprojectCostConversion);
            dbContext1.SaveChanges();
        }
    }

    public string AddProjectLocation( ProjectDefinitionCreateDto projectCreateDto,
        ProjectDefinitionUpdateDto projectUpdateDto, ITenantProvider iTenantProvider)
    {
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var context =
            new ProjectDefinitionDbContext(options1, iTenantProvider);
        
        MapLocation projectLocation = null;
        if (projectCreateDto != null) projectLocation = projectCreateDto.Location;

        if (projectUpdateDto != null) projectLocation = projectUpdateDto.Location;

        if (projectLocation != null)
        {
            //if (projectLocation.Id == null)
            //{
            projectLocation.Id = Guid.NewGuid().ToString();
            //}

            if (projectLocation.Position != null)
            {
                var position = projectLocation.Position;
                position.Id = Guid.NewGuid().ToString();
                context.Position.Add(position);
                context.SaveChanges();
            }

            if (projectLocation.Viewport != null)
            {
                var viewPort = projectLocation.Viewport;
                if (viewPort.TopLeftPoint != null)
                {
                    var tlp = viewPort.TopLeftPoint;
                    tlp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(tlp);
                    context.SaveChanges();
                }

                if (viewPort.BtmRightPoint != null)
                {
                    var brp = viewPort.BtmRightPoint;
                    brp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(brp);
                    context.SaveChanges();
                }

                viewPort.Id = Guid.NewGuid().ToString();
                context.BoundingPoint.Add(viewPort);
                context.SaveChanges();
            }

            if (projectLocation.BoundingBox != null)
            {
                var boundingBox = projectLocation.BoundingBox;
                if (boundingBox.TopLeftPoint != null)
                {
                    var tlp = boundingBox.TopLeftPoint;
                    tlp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(tlp);
                    context.SaveChanges();
                }

                if (boundingBox.BtmRightPoint != null)
                {
                    var brp = boundingBox.BtmRightPoint;
                    brp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(brp);
                    context.SaveChanges();
                }

                boundingBox.Id = Guid.NewGuid().ToString();
                context.BoundingPoint.Add(boundingBox);
                context.SaveChanges();
            }

            if (projectLocation.Address != null)
            {
                var address = projectLocation.Address;
                address.Id = Guid.NewGuid().ToString();
                context.Address.Add(address);
                context.SaveChanges();
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
                    context.SaveChanges();
                }

                dataSource.Id = Guid.NewGuid().ToString();
                context.DataSources.Add(dataSource);
                context.SaveChanges();
            }

            context.MapLocation.Add(projectLocation);
            context.SaveChanges();
            return projectLocation.Id;
        }

        return null;
    }

    public string AddProjectLocationCopy(ApplicationDbContext context, CreateNewProjectMyEnvDto projectCreateDto,
        CreateNewProjectMyEnvDto projectUpdateDto)
    {
        MapLocation projectLocation = null;
        if (projectCreateDto != null) projectLocation = projectCreateDto.Location;

        if (projectUpdateDto != null) projectLocation = projectUpdateDto.Location;

        if (projectLocation != null)
        {
            //if (projectLocation.Id == null)
            //{
            projectLocation.Id = Guid.NewGuid().ToString();
            //}

            if (projectLocation.Position != null)
            {
                var position = projectLocation.Position;
                position.Id = Guid.NewGuid().ToString();
                context.Position.Add(position);
                context.SaveChanges();
            }

            if (projectLocation.Viewport != null)
            {
                var viewPort = projectLocation.Viewport;
                if (viewPort.TopLeftPoint != null)
                {
                    var tlp = viewPort.TopLeftPoint;
                    tlp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(tlp);
                    context.SaveChanges();
                }

                if (viewPort.BtmRightPoint != null)
                {
                    var brp = viewPort.BtmRightPoint;
                    brp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(brp);
                    context.SaveChanges();
                }

                viewPort.Id = Guid.NewGuid().ToString();
                context.BoundingPoint.Add(viewPort);
                context.SaveChanges();
            }

            if (projectLocation.BoundingBox != null)
            {
                var boundingBox = projectLocation.BoundingBox;
                if (boundingBox.TopLeftPoint != null)
                {
                    var tlp = boundingBox.TopLeftPoint;
                    tlp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(tlp);
                    context.SaveChanges();
                }

                if (boundingBox.BtmRightPoint != null)
                {
                    var brp = boundingBox.BtmRightPoint;
                    brp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(brp);
                    context.SaveChanges();
                }

                boundingBox.Id = Guid.NewGuid().ToString();
                context.BoundingPoint.Add(boundingBox);
                context.SaveChanges();
            }

            if (projectLocation.Address != null)
            {
                var address = projectLocation.Address;
                address.Id = Guid.NewGuid().ToString();
                context.Address.Add(address);
                context.SaveChanges();
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
                    context.SaveChanges();
                }

                dataSource.Id = Guid.NewGuid().ToString();
                context.DataSources.Add(dataSource);
                context.SaveChanges();
            }

            context.MapLocation.Add(projectLocation);
            context.SaveChanges();
            return projectLocation.Id.ToString();
        }

        return null;
    }
    

    public string CreateDatabase(string SequenceCode, ApplicationDbContext context, ITenantProvider iTenantProvider,
        IConfiguration _iConfiguration)
    {
        // var idGenerator = new IdGenerator();
        // var newDbName = SequenceCode.Replace("-", "_");
        var connectionString = iTenantProvider.GetTenant().ConnectionString;
        var conn = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = SequenceCode
        };

        connectionString = conn.ConnectionString;
        var builder = new StringBuilder(connectionString);
        builder.Replace("Multiple Active Result Sets", "MultipleActiveResultSets");
        builder.Replace("Trust Server Certificate", "TrustServerCertificate");


        var nextSqNumber = GetNextSequenceNumber(context, _iConfiguration);
        var nxtNo = new string(nextSqNumber);

        CreateNextDb(nxtNo, iTenantProvider);
        return builder.ToString();
    }

    private static async void CreateNextDb(string SequenceCode, ITenantProvider iTenantProvider)
    {
        // StdSchedulerFactory factory = new StdSchedulerFactory();
        // IScheduler scheduler = await factory.GetScheduler();
        //
        // // and start it off
        // await scheduler.Start();
        //
        // IJobDetail job = JobBuilder.Create<DatabaseCopyJob>()
        //     .WithIdentity("DatabaseCopyJob", "DatabaseCopyJob")
        //     .Build();
        //
        //
        // ITrigger trigger = TriggerBuilder.Create()
        //     .WithIdentity("DatabaseCopyJob", "DatabaseCopyJob")
        //     .StartNow()
        //     .WithSimpleSchedule(x => x
        //         .WithIntervalInSeconds(10)
        //         .RepeatForever())
        //     .Build();
        //
        // // Tell Quartz to schedule the job using our trigger
        // await scheduler.ScheduleJob(job, trigger);


        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();

        // and start it off
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<DatabaseCopyJob>()
            .WithIdentity("DatabaseCopyJob", "DatabaseCopyJob")
            .UsingJobData("SequenceCode", SequenceCode)
            .UsingJobData("ConnectionString", iTenantProvider.GetTenant().ConnectionString)
            .Build();
        // JobKey jobKey = new JobKey("testJob");
        // JobDetail job = newJob(DatabaseCopyJob.class)
        // .withIdentity(jobKey)
        //     .storeDurably()
        //     .build();
        var replace = true;
        var durable = true;
        await scheduler.AddJob(job, replace, durable);

        await scheduler.TriggerJob(new JobKey("DatabaseCopyJob", "DatabaseCopyJob"));

        // var projectTemplateDbName = "UPrinceV4ProjectTemplate";
        // var copyQuery = "CREATE DATABASE " + SequenceCode + " AS COPY OF " + projectTemplateDbName;
        // await using (var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString))
        // {
        //     connection.Open();
        //     var command = new SqlCommand(copyQuery, connection);
        //     command.CommandTimeout = 600;
        //     await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        //     //Console.Write("ssss");
        // }
    }


    private string GetNextSequenceNumber(ApplicationDbContext context, IConfiguration _iConfiguration)
    {
        var property = context.Properties.FirstOrDefault(p => p.Key == "ProjectSequenceCode");
        string last;
        if (property == null)
        {
            var p = new Properties { Key = "ProjectSequenceCode", Value = "P0000" };
            context.Properties.Add(p);
            context.SaveChanges();
            last = "P0000";
        }
        else
        {
            last = property.Value;
        }

        var dbNo = _iConfiguration.GetValue<string>("DbNumber").ToInt();
        var len = last.Length;
        var prefix = last.Substring(0, 1);
        var numString = last.Substring(1, len - 1).TrimStart(new[] { '0' });
        var nextNum = int.Parse(numString) + dbNo;
        var numberLength = 0;
        if (nextNum == 0)
            numberLength = 1;
        else
            numberLength = (int)Math.Floor(Math.Log10(nextNum)) + 1;

        var numOfMissingZero = len - 1 - numberLength;
        var zeroString = "";
        for (var i = 1; i <= numOfMissingZero; i++) zeroString += "0";

        var sqNumber = prefix + zeroString + nextNum;
        return sqNumber;
    }

    private string GetSequenceNumber(ApplicationDbContext context)
    {
        var property = context.Properties.FirstOrDefault(p => p.Key == "ProjectSequenceCode");
        string last;
        if (property == null)
        {
            var p = new Properties { Key = "ProjectSequenceCode", Value = "P0000" };
            context.Properties.Add(p);
            context.SaveChanges();
            last = "P0000";
        }
        else
        {
            last = property.Value;
        }

        var len = last.Length;
        var prefix = last.Substring(0, 1);
        var numString = last.Substring(1, len - 1).TrimStart(new[] { '0' });
        var nextNum = int.Parse(numString) + 1;
        var numberLength = 0;
        if (nextNum == 0)
            numberLength = 1;
        else
            numberLength = (int)Math.Floor(Math.Log10(nextNum)) + 1;

        var numOfMissingZero = len - 1 - numberLength;
        var zeroString = "";
        for (var i = 1; i <= numOfMissingZero; i++) zeroString += "0";

        var sqNumber = prefix + zeroString + nextNum;

        if (property != null)
        {
            property.Value = sqNumber;
            context.Properties.Update(property);
        }

        context.SaveChanges();

        return sqNumber;
    }
    

    public void CreateKpi(ProjectKPICreateDto kpidto, string projectId, ITenantProvider iTenantProvider)
    {
        var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
        var context =
            new ProjectDefinitionDbContext(options1,iTenantProvider);
        var kpi = new ProjectKPI
        {
            CustomLabelOne = kpidto.CustomLabelOne,
            CustomLabelThree = kpidto.CustomLabelThree,
            CustomLabelTwo = kpidto.CustomLabelTwo,
            CustomPropOne = kpidto.CustomPropOne,
            CustomPropThree = kpidto.CustomPropThree,
            CustomPropTwo = kpidto.CustomPropTwo,
            Id = Guid.NewGuid().ToString(),
            ProjectId = projectId
        };
        //kpi.ChangeByUserId = user.Id;
        //kpi.Action = HistoryState.ADDED.ToString();
        context.ProjectKPI.Add(kpi);
        context.SaveChanges();
    }

    public void UpdateKpi(ApplicationDbContext context, ProjectKPIUpdateDto kpidto, ApplicationUser user)
    {
        if (kpidto.Id != null)
        {
            var kpi = new ProjectKPI
            {
                CustomLabelOne = kpidto.CustomLabelOne,
                CustomLabelThree = kpidto.CustomLabelThree,
                CustomLabelTwo = kpidto.CustomLabelTwo,
                CustomPropOne = kpidto.CustomPropOne,
                CustomPropThree = kpidto.CustomPropThree,
                CustomPropTwo = kpidto.CustomPropTwo,
                Id = kpidto.Id,
                ProjectId = kpidto.ProjectId
            };
            context.ProjectKPI.Update(kpi);
            context.SaveChanges();
        }
    }

    public async Task<string> UpdateProjectDefinitionCopy(ApplicationDbContext context,
        CreateNewProjectMyEnvDto projectDto, IProjectTimeRepository iProjectTimeRepository,
        IProjectFinanceRepository iProjectFinanceRepository, IProjectTeamRepository projectTeamRepository,
        ITenantProvider iTenantProvider, ApplicationUser user)
    {
        var project =
            context.ProjectDefinition.FirstOrDefault(p => p.Id == projectDto.Id);
        project.Description = projectDto.Description;
        project.ProjTemplateId = projectDto.ProjectTemplateId;
        project.ContractingUnitId = projectDto.ProjectTeam.ContractingUnitId;
        //project.ChangeByUserId = user.Id;
        //project.Action = HistoryState.UPDATED.ToString();
        if (projectDto.ProjectTypeId == null)
        {
            var projectId = context.ProjectType.First(t => t.IsDefault == true).Id;
            project.ProjTypeId = projectId;
        }
        else
        {
            project.ProjTypeId = projectDto.ProjectTypeId;
        }

        if (projectDto.ProjectManagementLevelId == null)
        {
            var projectManagementLevelId = context.ProjectManagementLevel.First(t => t.IsDefault == true).Id;
            project.ProjManagementLevelId = projectManagementLevelId;
        }
        else
        {
            project.ProjManagementLevelId = projectDto.ProjectManagementLevelId;
        }

        if (projectDto.ProjectToleranceStateId == null)
        {
            var projectToleranceStateId = context.ProjectToleranceState.First(t => t.IsDefault == true).Id;
            project.ProjToleranceStateId = projectToleranceStateId;
        }
        else
        {
            project.ProjToleranceStateId = projectDto.ProjectToleranceStateId;
        }

        project.ProjectScopeStatusId = projectDto.ProjectScopeStatusId;
        project.ProjectStatus = projectDto.ProjectStatus;

        project.LocationId = AddProjectLocationCopy(context, null, projectDto);
        project.Name = projectDto.Name;
        project.GeneralLedgerId = projectDto.GeneralLedgerId;
        project.Title = project.SequenceCode + " - " + project.Name;
        project.CustomerId = projectDto.CustomerId;
        project.Language = projectDto.Language;
        project.ProjectManagerId = projectDto.ProjectManagerId;
        project.SiteManagerId = projectDto.SiteManagerId;


        context.ProjectDefinition.Update(project);
        context.SaveChanges();

        // if (projectDto.Kpi != null) UpdateKpi(context, projectDto.Kpi, user);
        //
        // if (projectDto.ProjectCostConversionCreateDto != null)
        //     CreateProjectCostConversion(context, project.Id, projectDto.ProjectCostConversionCreateDto);
        //
        // if (projectDto.ProjectFinance != null)
        //     iProjectFinanceRepository.UpdateProjectFinance(context, projectDto.ProjectFinance, user, iTenantProvider);
        //
        // if (projectDto.ProjectTime != null)
        //     await iProjectTimeRepository.UpdateProjectTime(context, projectDto.ProjectTime, user);

        if (projectDto.ProjectManagerId != null)
        {
            if (projectDto.ProjectTeam.Id != null)
            {
                var nmm = projectDto.ProjectTeam.TeamRoleList
                    .Where(x => x.RoleId == "266a5f47-3489-484b-8dae-e4468c5329dn3").FirstOrDefault();
                if (nmm != null)
                    if (nmm.CabPersonId != projectDto.ProjectManagerId)
                    {
                        await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                        var removeItem =
                            projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                x.RoleId == "266a5f47-3489-484b-8dae-e4468c5329dn3");

                        if (removeItem != null)
                        {
                            var parameter = new ProjectTeamRoleParameter
                            {
                                TenantProvider = iTenantProvider
                            };
                            var idList = new List<string> { removeItem.Id };
                            parameter.IdList = idList;
                            await projectTeamRepository.DeleteProjectAccess(parameter);
                        }

                        await connection.ExecuteAsync(
                            "Delete From ProjectTeamRole Where RoleId = '266a5f47-3489-484b-8dae-e4468c5329dn3' AND ProjectTeamId = @ProjectTeamId",
                            new { ProjectTeamId = projectDto.ProjectTeam.Id });

                        projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                    }

                var pmExist = context.ProjectTeamRole
                    .FirstOrDefault(x =>
                        x.CabPersonId == projectDto.ProjectManagerId && x.ProjectTeamId == projectDto.ProjectTeam.Id);
                if (pmExist == null)
                {
                    projectDto.ProjectTeam.ProjectId = projectDto.Id;
                    var pmAdd = new ProjectTeamRoleCreateDto
                    {
                        CabPersonId = projectDto.ProjectManagerId,
                        RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3",
                        IsAccessGranted = true,
                        status = "1"
                    };

                    projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                }
                else
                {
                    pmExist.RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3";
                    context.ProjectTeamRole.Update(pmExist);
                    var Item =
                        projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                            x.CabPersonId == projectDto.ProjectManagerId);
                    projectDto.ProjectTeam.TeamRoleList.Remove(Item);
                    Item.RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3";
                    projectDto.ProjectTeam.TeamRoleList.Add(Item);
                }
            }
        }
        else
        {
            var removeItem =
                projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                    x.RoleId == "266a5f47-3489-484b-8dae-e4468c5329dn3");

            if (removeItem != null)
            {
                var parameter = new ProjectTeamRoleParameter
                {
                    TenantProvider = iTenantProvider
                };
                var idList = new List<string> { removeItem.Id };
                parameter.IdList = idList;
                await projectTeamRepository.DeleteProjectAccess(parameter);
            }

            projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
            await connection.ExecuteAsync(
                "Delete From ProjectTeamRole Where RoleId = '266a5f47-3489-484b-8dae-e4468c5329dn3' AND ProjectTeamId = @ProjectTeamId",
                new { ProjectTeamId = projectDto.ProjectTeam.Id });
        }

        if (projectDto.CustomerId != null)
        {
            if (projectDto.ProjectTeam.Id != null)
            {
                var nmm = projectDto.ProjectTeam.TeamRoleList
                    .Where(x => x.RoleId == "910b7af0-b132-4951-a2dc-6ab82d4cd40d").FirstOrDefault();
                if (nmm != null)
                    if (nmm.CabPersonId != projectDto.CustomerId)
                    {
                        await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);

                        var removeItem =
                            projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                x.RoleId == "910b7af0-b132-4951-a2dc-6ab82d4cd40d");

                        if (removeItem != null)
                        {
                            var parameter = new ProjectTeamRoleParameter
                            {
                                TenantProvider = iTenantProvider
                            };
                            var idList = new List<string> { removeItem.Id };
                            parameter.IdList = idList;
                            await projectTeamRepository.DeleteProjectAccess(parameter);
                        }

                        await connection.ExecuteAsync(
                            "Delete From ProjectTeamRole Where RoleId = '910b7af0-b132-4951-a2dc-6ab82d4cd40d' AND ProjectTeamId = @ProjectTeamId",
                            new { ProjectTeamId = projectDto.ProjectTeam.Id });

                        projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                    }

                var pmExist = context.ProjectTeamRole
                    .FirstOrDefault(x =>
                        x.CabPersonId == projectDto.CustomerId && x.ProjectTeamId == projectDto.ProjectTeam.Id);
                if (pmExist == null)
                {
                    projectDto.ProjectTeam.ProjectId = projectDto.Id;
                    var pmAdd = new ProjectTeamRoleCreateDto
                    {
                        CabPersonId = projectDto.CustomerId,
                        RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d",
                        IsAccessGranted = true,
                        status = "1"
                    };

                    projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                }
                else
                {
                    var parameter = new ProjectTeamRoleParameter
                    {
                        TenantProvider = iTenantProvider
                    };
                    var idList = new List<string> { pmExist.Id };
                    parameter.IdList = idList;
                    await projectTeamRepository.DeleteProjectAccess(parameter);


                    pmExist.RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d";
                    context.ProjectTeamRole.Update(pmExist);
                    var Item =
                        projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x => x.CabPersonId == projectDto.CustomerId);
                    projectDto.ProjectTeam.TeamRoleList.Remove(Item);
                    Item.RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d";
                    projectDto.ProjectTeam.TeamRoleList.Add(Item);
                }
            }
        }
        else
        {
            var removeItem =
                projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                    x.RoleId == "910b7af0-b132-4951-a2dc-6ab82d4cd40d");
            if (removeItem != null)
            {
                var parameter = new ProjectTeamRoleParameter
                {
                    TenantProvider = iTenantProvider
                };
                var idList = new List<string> { removeItem.Id };
                parameter.IdList = idList;
                await projectTeamRepository.DeleteProjectAccess(parameter);
            }

            projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
            await connection.ExecuteAsync(
                "Delete From ProjectTeamRole Where RoleId = '910b7af0-b132-4951-a2dc-6ab82d4cd40d' AND ProjectTeamId = @ProjectTeamId",
                new { ProjectTeamId = projectDto.ProjectTeam.Id });
        }

        if (projectDto.SiteManagerId != null)
        {
            if (projectDto.ProjectTeam.Id != null)
            {
                var nmm = projectDto.ProjectTeam.TeamRoleList
                    .Where(x => x.RoleId == "yyyyyyy-a513-45e0-a431-170dbd4yyyy").FirstOrDefault();
                if (nmm != null)
                    if (nmm.CabPersonId != projectDto.SiteManagerId)
                    {
                        await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
                        var removeItem =
                            projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                                x.RoleId == "yyyyyyy-a513-45e0-a431-170dbd4yyyy");

                        if (removeItem != null)
                        {
                            var parameter = new ProjectTeamRoleParameter
                            {
                                TenantProvider = iTenantProvider
                            };
                            var idList = new List<string> { removeItem.Id };
                            parameter.IdList = idList;
                            await projectTeamRepository.DeleteProjectAccess(parameter);
                        }

                        await connection.ExecuteAsync(
                            "Delete From ProjectTeamRole Where RoleId = 'yyyyyyy-a513-45e0-a431-170dbd4yyyy' AND ProjectTeamId = @ProjectTeamId",
                            new { ProjectTeamId = projectDto.ProjectTeam.Id });

                        projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
                    }

                var pmExist = context.ProjectTeamRole
                    .FirstOrDefault(x =>
                        x.CabPersonId == projectDto.SiteManagerId && x.ProjectTeamId == projectDto.ProjectTeam.Id);
                if (pmExist == null)
                {
                    projectDto.ProjectTeam.ProjectId = projectDto.Id;
                    var pmAdd = new ProjectTeamRoleCreateDto
                    {
                        CabPersonId = projectDto.SiteManagerId,
                        RoleId = "yyyyyyy-a513-45e0-a431-170dbd4yyyy",
                        IsAccessGranted = true,
                        status = "1"
                    };

                    projectDto.ProjectTeam.TeamRoleList.Add(pmAdd);
                }
                else
                {
                    pmExist.RoleId = "yyyyyyy-a513-45e0-a431-170dbd4yyyy";
                    context.ProjectTeamRole.Update(pmExist);
                    var Item =
                        projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                            x.CabPersonId == projectDto.SiteManagerId);
                    projectDto.ProjectTeam.TeamRoleList.Remove(Item);
                    Item.RoleId = "yyyyyyy-a513-45e0-a431-170dbd4yyyy";
                    projectDto.ProjectTeam.TeamRoleList.Add(Item);
                }
            }
        }
        else
        {
            var removeItem =
                projectDto.ProjectTeam.TeamRoleList.FirstOrDefault(x =>
                    x.RoleId == "yyyyyyy-a513-45e0-a431-170dbd4yyyy");

            if (removeItem != null)
            {
                var parameter = new ProjectTeamRoleParameter
                {
                    TenantProvider = iTenantProvider
                };
                var idList = new List<string> { removeItem.Id };
                parameter.IdList = idList;
                await projectTeamRepository.DeleteProjectAccess(parameter);
            }

            projectDto.ProjectTeam.TeamRoleList.Remove(removeItem);
            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
            await connection.ExecuteAsync(
                "Delete From ProjectTeamRole Where RoleId = 'yyyyyyy-a513-45e0-a431-170dbd4yyyy' AND ProjectTeamId = @ProjectTeamId",
                new { ProjectTeamId = projectDto.ProjectTeam.Id });
        }

        if (projectDto.ProjectTeam != null)
        {
            projectDto.ProjectTeam.ProjectId = projectDto.Id;
            await projectTeamRepository.CreateProjectTeam( projectDto.ProjectTeam, iTenantProvider,
                user);
        }


        if (projectDto.ProjectClassification != null)
        {
            var existClassification =
                context.ProjectClassification.FirstOrDefault(p => p.ProjectId == projectDto.Id);

            await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
            var param = new
            {
                id = Guid.NewGuid().ToString(),
                ProjectId = projectDto.Id,
                projectDto.ProjectClassification.ProjectClassificationBuisnessUnit,
                projectDto.ProjectClassification.ProjectClassificationSizeId,
                projectDto.ProjectClassification
                    .ProjectClassificationConstructionTypeId,
                projectDto.ProjectClassification.ProjectClassificationSectorId
            };
            if (existClassification != null)
            {
                var classification =
                    "UPDATE dbo.ProjectClassification SET ProjectClassificationBuisnessUnit = @ProjectClassificationBuisnessUnit ,ProjectClassificationSizeId = @ProjectClassificationSizeId ,ProjectClassificationConstructionTypeId = @ProjectClassificationConstructionTypeId ,ProjectClassificationSectorId = @ProjectClassificationSectorId WHERE ProjectId = @ProjectId ;";

                await connection.ExecuteAsync(classification, param);
            }
            else
            {
                var classificationInsert =
                    "INSERT INTO dbo.ProjectClassification ( Id ,ProjectId ,ProjectClassificationBuisnessUnit ,ProjectClassificationSizeId ,ProjectClassificationConstructionTypeId ,ProjectClassificationSectorId ) VALUES ( @Id ,@ProjectId ,@ProjectClassificationBuisnessUnit ,@ProjectClassificationSizeId ,@ProjectClassificationConstructionTypeId ,@ProjectClassificationSectorId )";

                await connection.ExecuteAsync(classificationInsert, param);
            }
        }

        var jsonProject = JsonConvert.SerializeObject(project, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var pdhl = new ProjectDefinitionHistoryLog
        {
            Action = HistoryState.UPDATED.ToString(),
            ChangedByUserId = user.Id,
            Id = Guid.NewGuid().ToString(),
            HistoryLog = jsonProject,
            ChangedTime = DateTime.UtcNow,
            ProjectDefinitionId = project.Id
        };
        context.ProjectDefinitionHistoryLog.Add(pdhl);
        await context.SaveChangesAsync();
        return project.Id;
    }


    public async Task<string> CreateTeam(ApplicationDbContext context, ITenantProvider iTenantProvider,
        CreateTeamDto createTeamDto, string oid)
    {
        await using var connection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);

        var name = connection
            .Query<string>(@"SELECT Title FROM dbo.ProjectDefinition WHERE SequenceCode = @Project", createTeamDto)
            .FirstOrDefault();

        var projectTeam = connection.Query<string>(@"SELECT
                                          cpc.Oid
                                        FROM dbo.ProjectDefinition pd
                                        LEFT OUTER JOIN dbo.ProjectTeam pt
                                          ON pd.Id = pt.ProjectId
                                        LEFT OUTER JOIN dbo.ProjectTeamRole ptr
                                          ON pt.Id = ptr.ProjectTeamId
                                        LEFT OUTER JOIN dbo.CabPersonCompany cpc
                                          ON ptr.CabPersonId = cpc.PersonId
                                        LEFT OUTER JOIN dbo.CabEmail ce
                                          ON cpc.EmailId = ce.Id
                                        WHERE pd.SequenceCode = @Project 
                                        AND cpc.EmailId IS NOT NULL", createTeamDto).ToList();

        var authentication = new
        {
            Authority =
                "https://uprinceusermanagementprod.b2clogin.com/3d438826-fdde-4b8b-89d1-1b9b4feeaa20/B2C_1_Web_v4_signup",
            Directory = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20", /* tenant id */
            Application = "f9ec3629-065f-4065-9dee-f42c22ae74e5", /* client id */
            ClientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd",
            OpenIdConnectGrantTypes = OpenIdConnectGrantTypes.ClientCredentials,
            Scope = "https://graph.microsoft.com/.default"
        };

        var app = ConfidentialClientApplicationBuilder.Create(authentication.Application)
            .WithClientSecret(authentication.ClientSecret)
            .WithAuthority(AzureCloudInstance.AzurePublic, authentication.Directory)
            .Build();
        
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
        var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
        var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";

        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret);
        var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);


        var requestBody = new Team
        {
            DisplayName = name,
            Description = name,
            Members = new List<ConversationMember>
            {
                new AadUserConversationMember
                {
                    OdataType = "#microsoft.graph.aadUserConversationMember",
                    Roles = new List<string>
                    {
                        "owner",
                    },
                    AdditionalData = new Dictionary<string, object>
                    {
                        {
                            "user@odata.bind" , $"https://graph.microsoft.com/v1.0/users('{oid}')"
                        },
                    },
                },
            },
            AdditionalData = new Dictionary<string, object>()
            {
                { "template@odata.bind", "https://graph.microsoft.com/v1.0/teamsTemplates('standard')" }
            }
            // Members = 
        };
        var result =  await graphServiceClient.Teams.PostAsync(requestBody).AsTaskResult();
        
        // if (result?.Id != null)
        // {
        //     foreach (var i in projectTeam)
        //     {
        //         if (i != oid)
        //         {
        //             var requestBody3 = new AadUserConversationMember
        //             {
        //                 Roles = new List<string>
        //                 {
        //                     "member"
        //                 },
        //                 AdditionalData = new Dictionary<string, object>
        //                 {
        //                     {
        //                         "user@odata.bind", $"https://graph.microsoft.com/v1.0/users('{i}')"
        //                     },
        //                 },
        //             };
        //         
        //             var result3 = await graphServiceClient.Teams[result?.Result.Id].Members.PostAsync(requestBody3);
        //         }
        //     }
        // }

        return name;
    }

    public class TokenRequest
    {
        [JsonProperty("token_type")] public string TokenType { get; set; }

        [JsonProperty("access_token")] public string AccessToken { get; set; }
    }

    public async Task<bool> ProjectDefinitionChanged(ApplicationDbContext context, string id, ApplicationUser user)
    {
        var project = (from a in context.ProjectDefinition
            where a.Id == id
            select a).Single();
        //project.ChangeByUserId = user.Id;
        //project.Action = HistoryState.DELETED.ToString();
        context.ProjectDefinition.Update(project);
        await context.SaveChangesAsync();
        context.ProjectDefinition.Remove(project);
        await context.SaveChangesAsync();
        var jsonProject = JsonConvert.SerializeObject(project, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var pdhl = new ProjectDefinitionHistoryLog
        {
            Action = HistoryState.DELETED.ToString(),
            ChangedByUserId = user.Id,
            Id = Guid.NewGuid().ToString(),
            HistoryLog = jsonProject,
            ChangedTime = DateTime.UtcNow,
            ProjectDefinitionId = project.Id
        };
        context.ProjectDefinitionHistoryLog.Add(pdhl);
        await context.SaveChangesAsync();
        return true;
    }
}