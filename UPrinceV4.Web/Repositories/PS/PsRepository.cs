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
using Newtonsoft.Json;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Data.TAX;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Repositories.Interfaces.PS;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Repositories.PS;

public class PsRepository : IPsRepository
{
    public async Task<PsHeader> CreatePsHeader(PsParameter parameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        string id;
        await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
        {
            var headerExist = context.PsHeader
                .FirstOrDefault(p => p.Id == parameter.PsHeaderCreateDto.Id);
            var isExist = false;
            if (headerExist == null)
            {
                isExist = false;
                var header = new PsHeader
                {
                    Name = parameter.PsHeaderCreateDto.Name,
                    Date = DateTime.UtcNow,
                    //header.ProjectCostId = parameter.PsHeaderCreateDto.ProjectCostId;
                    ProjectStatusId = parameter.PsHeaderCreateDto.ProjectStatusId,
                    ProjectTypeId = parameter.PsHeaderCreateDto.ProjectTypeId,
                    WorkPeriodFrom = parameter.PsHeaderCreateDto.WorkPeriodFrom,
                    WorkPeriodTo = parameter.PsHeaderCreateDto.WorkPeriodTo,
                    PurchaseOrderNumber = parameter.PsHeaderCreateDto.PurchaseOrderNumber,
                    Id = parameter.PsHeaderCreateDto.Id
                };
                var idGenerator = new IdGenerator();
                header.ProgressStatementId =
                    idGenerator.GenerateId(parameter.ApplicationDbContext, "PS-", "PsSequenceCode");
                header.Title = header.ProgressStatementId + " - " + header.Name;
                header.ProjectSequenceCode = parameter.ProjectSequenceId;
                header.GeneralLedgerId = parameter.PsHeaderCreateDto.GeneralLedgerId;
                header.InvoiceComment = parameter.PsHeaderCreateDto.InvoiceComment;
                context.PsHeader.Add(header);
                await context.SaveChangesAsync();
                headerExist = header;
                id = header.Id;
            }
            else
            {
                isExist = true;
                headerExist.Name = parameter.PsHeaderCreateDto.Name;
                //header.ProjectCostId = parameter.PsHeaderCreateDto.ProjectCostId;
                headerExist.ProjectStatusId = parameter.PsHeaderCreateDto.ProjectStatusId;
                headerExist.ProjectTypeId = parameter.PsHeaderCreateDto.ProjectTypeId;
                headerExist.WorkPeriodFrom = parameter.PsHeaderCreateDto.WorkPeriodFrom;
                headerExist.WorkPeriodTo = parameter.PsHeaderCreateDto.WorkPeriodTo;
                headerExist.PurchaseOrderNumber = parameter.PsHeaderCreateDto.PurchaseOrderNumber;
                headerExist.Title = headerExist.ProgressStatementId + " - " + headerExist.Name;
                headerExist.ProjectSequenceCode = parameter.ProjectSequenceId;
                headerExist.GeneralLedgerId = parameter.PsHeaderCreateDto.GeneralLedgerId;
                headerExist.InvoiceComment = parameter.PsHeaderCreateDto.InvoiceComment;
                context.PsHeader.Update(headerExist);
                context.SaveChanges();
                id = headerExist.Id;
            }

            var projectUpdateQuery = @"UPDATE dbo.ProjectDefinition 
                                                        SET
                                                          GeneralLedgerId = @GeneralLedgerId
                                                        WHERE
                                                          SequenceCode = @SequenceCode
                                                      ";

            var paramProject = new
            {
                parameter.PsHeaderCreateDto.GeneralLedgerId,
                SequenceCode = parameter.ProjectSequenceId
            };

            using (var connection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString))
            {
                connection.Query(projectUpdateQuery, paramProject);
                connection.Close();
            }

            CreateHistory(parameter, isExist);
            return headerExist;
        }
    }

    public async Task<PsDropdownData> GetDropdownData(PmolParameter pmolParameter)
    {
        var lang = pmolParameter.Lang;

        var dropdownData = new PsDropdownData();
        using IDbConnection dbConnection =
            new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);
        var query = @"select StatusId as [Key], Name as Text  from PsStatus where LanguageCode = @lang
                                    ORDER BY DisplayOrder;
                                    select TypeId as [Key], Name as Text  from PsType where LanguageCode = @lang
                                    ORDER BY Name;
                                    select ProjectDefinition.Id, ProjectTime.EndDate, CabPerson.FullName AS ContactPersonName,
                                    CabEmail.EmailAddress AS ContactPersonEmail , CabCompany.Name AS Customer
                                    from ProjectDefinition
                                    left outer join  ProjectTime on ProjectDefinition.Id=ProjectTime.ProjectId
                                    left outer join  ProjectTeam on ProjectDefinition.Id = ProjectTeam.ProjectId
                                    left outer join  ProjectTeamRole on ProjectTeam.Id = ProjectTeamRole.ProjectTeamId
                                    left outer join  CabPerson on ProjectTeamRole.CabPersonId = CabPerson.Id
                                    left outer join CabPersonCompany on CabPerson.Id = CabPersonCompany.PersonId
                                    left outer join CabCompany on CabCompany.Id = CabPersonCompany.CompanyId
                                    left outer join CabEmail on CabPersonCompany.EmailId = CabEmail.Id
                                    where ProjectDefinition.SequenceCode = @id
                                    AND RoleId = '907b7af0-b132-4951-a2dc-6ab82d4cd40d';
                                    select EndDate 
									from ProjectDefinition left outer join  ProjectTime on ProjectDefinition.Id=ProjectTime.ProjectId
									where SequenceCode = @id
                                    ";
        var param = new { lang, id = pmolParameter.ProjectSequenceId };


        using var multi = await dbConnection.QueryMultipleAsync(query, param);
        dropdownData.Status = multi.Read<PmolDropdownDto>();
        dropdownData.Type = multi.Read<PmolDropdownDto>();
        var project = multi.Read<projectForPsDto>().FirstOrDefault();
        var date = multi.Read<DateTime?>().FirstOrDefault();

        if (date != null) dropdownData.ProjectCompletionDate = date.ToString();

        if (project != null)
        {
            var customer = new PsCustomerReadDto
            {
                ContactPersonName = project.ContactPersonName,
                ContactPersonEmail = project.ContactPersonEmail,
                CustomerName = project.Customer
            };
            dropdownData.Customer = customer;
        }

        return dropdownData;
    }

    public async Task<string> GetPsTitleById(PsParameter psParameter)
    {
        var query = @"SELECT
                                  PsHeader.Title
                                FROM dbo.PsHeader
                                WHERE PsHeader.ProgressStatementId = @id";

        var connectionString = ConnectionString.MapConnectionString(psParameter.ContractingUnitSequenceId,
            psParameter.ProjectSequenceId, psParameter.TenantProvider);
        var parameters = new { id = psParameter.PsId };
        await using var dbConnection = new SqlConnection(connectionString);
        var result = dbConnection.Query<string>(query, parameters).FirstOrDefault();
        
        return result;
    }

    public async Task<IEnumerable<PsFilterReadDto>> PsFilter(PsParameter psParameter)
    {
        var lang = psParameter.Lang;

        var projectQuery = @"SELECT
                                      ProjectDefinition.Name AS Project
                                     ,ProjectTime.EndDate AS Date
                                    FROM dbo.ProjectTime
                                    LEFT OUTER JOIN dbo.ProjectDefinition
                                      ON ProjectTime.ProjectId = ProjectDefinition.Id
                                    WHERE ProjectDefinition.SequenceCode IN @sqCode";

        IEnumerable<ProjectDefinitionMobDto> project = null;


        // with cost filter
        var query = @"SELECT
                              PsHeader.Id AS Id
                             ,PsHeader.ProgressStatementId AS ProgressStatementId
                             ,PsHeader.Title AS Title
                             ,PsHeader.Date AS Date
                             ,PsStatus.Name AS Status
                             ,PsHeader.TotalAmount AS TotalAmount
                             ,PsHeader.ProjectSequenceCode AS ProjectSequenceCode
                            FROM dbo.PsHeader
                            LEFT OUTER JOIN dbo.PsStatus
                              ON PsHeader.ProjectStatusId = PsStatus.StatusId
                            WHERE PsStatus.LanguageCode = @lang";
        var sb = new StringBuilder(query);
        var filter = psParameter.Filter;
        if (filter != null)
        {
            //LIKE '%" + filter.Title + "%'"
            if (filter.Title != null)
            {
                filter.Title = filter.Title.Replace("'", "''");

                sb.Append(" AND PsHeader.Title LIKE '%" + filter.Title + "%'");

            }
               
            if (filter.Status != null) sb.Append(" AND PsStatus.StatusId ='" + filter.Status + "'");

            if (filter.TotalAmount != null)
                sb.Append(" AND PsHeader.TotalAmount LIKE '%" + filter.TotalAmount + "%'");


            if (filter.Date != null)
            {
                //sb.Append(" AND PsHeader.Date ='" + filter.Date+"'");
                var dt = (DateTime)filter.Date;
                dt = dt.ToUniversalTime();
                sb.Append(" AND PsHeader.Date BETWEEN '" + dt + "' AND '" + dt.AddHours(24) + "'");
                //Date BETWEEN '"+ filter.StrDate + "' AND '"+ filter.EndDate + "'"
                //sb = FilterByDate(sb, psParameter);
            }

            if (filter.Sorter != null && filter.Sorter.Attribute != null)
            {
                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("asc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " ASC");

                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
            }
        }

        var connectionString = ConnectionString.MapConnectionString(psParameter.ContractingUnitSequenceId,
            psParameter.ProjectSequenceId, psParameter.TenantProvider);
        var parameters = new { lang };
        IEnumerable<PsFilterReadDto> result;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var sqlStr = sb.ToString();
            result = dbConnection.QueryAsync<PsFilterReadDto>(sqlStr, parameters).Result;
            
        }

        var projectParameters = new { sqCode = result.Select(ps => ps.ProjectSequenceCode).Distinct() };
        await using (var dbConnection =
                     new SqlConnection(psParameter.TenantProvider.GetTenant().ConnectionString))
        {
            // dbConnection.OpenAsync();
            project = dbConnection.Query<ProjectDefinitionMobDto>(@"SELECT
  ProjectDefinition.SequenceCode as SequenceCode
 , ProjectDefinition.Title
 , ProjectDefinition.ContractingUnitId
FROM dbo.ProjectDefinition
WHERE ProjectDefinition.SequenceCode IN @sqCode
", projectParameters);
            
        }

        foreach (var Ps in result)
            Ps.ProjectSequenceCode = project
                .Where(p => p.SequenceCode != null && p.SequenceCode == Ps.ProjectSequenceCode).FirstOrDefault()
                ?.Title;

        if (filter?.ProjectSequenceCode != null)
        {
            filter.ProjectSequenceCode =
                filter?.ProjectSequenceCode.Replace("'", "''");
            result = result.Where(x =>
                x.ProjectSequenceCode != null && x.ProjectSequenceCode.ToLower()
                    .Contains(filter.ProjectSequenceCode.ToLower()));
            // sb.Append(" AND PsHeader.ProjectSequenceCode LIKE '%" + filter.ProjectSequenceCode + "%'");

        }
            
        return result;
    }

    public async Task<PsHeaderReadDto> ReadPsById(PsParameter psParameter)
    {
        var lang = psParameter.Lang;

        var query =
            @"select PsHeader.Id,PsHeader.ProgressStatementId,PsHeader.Name, PsHeader.Title,PsStatus.StatusId AS ProjectStatusId, PsStatus.Name AS ProjectStatus ,PsHeader.Date,PsHeader.InvoiceComment,
                                 PsType.TypeId ProjectTypeId, PsType.Name AS ProjectType, PsHeader.WorkPeriodFrom, PsHeader.WorkPeriodTo, PsHeader.PurchaseOrderNumber, PsHeader.TotalAmount , PsHeader.ProjectSequenceCode, PsHeader.GeneralLedgerId
                                 from PsHeader
                                 LEFT OUTER JOIN PsStatus on PsStatus.StatusId= PsHeader.ProjectStatusId
                                 LEFT OUTER JOIN PsType on PsType.TypeId =PsHeader.ProjectTypeId 
                                 WHERE (PsStatus.LanguageCode = @lang OR PsHeader.ProjectStatusId IS NULL)
                                 AND (PsType.LanguageCode = @lang OR PsHeader.ProjectTypeId IS NULL)
                                 AND PsHeader.ProgressStatementId = @id";

        var connectionString = ConnectionString.MapConnectionString(psParameter.ContractingUnitSequenceId,
            psParameter.ProjectSequenceId, psParameter.TenantProvider);
        var parameters = new { lang, id = psParameter.PsId };
        PsHeaderReadDto result;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            result = dbConnection.Query<PsHeaderReadDto>(query, parameters).FirstOrDefault();
            
        }

        var resourcequery =
            @"select PsResource.ConsumedQuantity, PsResource.ConsumedQuantityMou, PsResource.CostToMou, PsResource.CpcId, PsResource.CpcResourceNumber, ProjectCost.ProductTitle,
                                        PsResource.CpcResourceTypeId, PsResource.CpcTitle, PsResource.Id, PsResource.MouId, PsResource.ProjectCostId, PsResource.PsId, PsResource.SoldQuantity,
                                        PsResource.SpToMou, PsResource.Status,PsResource.TotalCost, CpcResourceTypeLocalizedData.Label AS CpcResourceType, CpcBasicUnitOfMeasureLocalizedData.Label AS Mou, PsResource.GeneralLedgerId
                                        from PsResource
                                        LEFT OUTER JOIN CpcResourceTypeLocalizedData on CpcResourceTypeLocalizedData.CpcResourceTypeId=PsResource.CpcResourceTypeId
                                        LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = PsResource.MouId
                                        LEFT OUTER JOIN ProjectCost on PsResource.ProjectCostId = ProjectCost.Id
                                        WHERE (CpcResourceTypeLocalizedData.LanguageCode = @lang OR PsResource.CpcResourceTypeId IS NULL)
                                        AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR PsResource.MouId IS NULL)
                                        AND PsResource.PsId = @id AND PsResource.CpcResourceTypeId != 'cbc3a26-cbc-cbc-cbc-89655304cbc' ";

        var cbcResQuery =
            @"select PsResource.ConsumedQuantity, PsResource.ConsumedQuantityMou, PsResource.CostToMou, PsResource.CpcId, PsResource.CpcResourceNumber, PsResource.ProductTitle,
                                        PsResource.CpcResourceTypeId, PsResource.CpcTitle, PsResource.Id, PsResource.MouId, PsResource.ProjectCostId, PsResource.PsId, PsResource.SoldQuantity,
                                        PsResource.SpToMou, PsResource.Status,PsResource.TotalCost, CpcResourceTypeLocalizedData.Label AS CpcResourceType, PsResource.GeneralLedgerId
                                        from PsResource
                                        LEFT OUTER JOIN CpcResourceTypeLocalizedData on CpcResourceTypeLocalizedData.CpcResourceTypeId=PsResource.CpcResourceTypeId
                                        WHERE (CpcResourceTypeLocalizedData.LanguageCode = 'en' OR PsResource.CpcResourceTypeId IS NULL)
                                        AND PsResource.PsId = @id AND PsResource.CpcResourceTypeId = 'cbc3a26-cbc-cbc-cbc-89655304cbc'";

        var cuResourceQuery = @"SELECT
                                              PsResource.ConsumedQuantity
                                             ,PsResource.ConsumedQuantityMou
                                             ,PsResource.CostToMou
                                             ,PsResource.CpcId
                                             ,PsResource.CpcResourceNumber
                                             ,PsResource.CpcResourceTypeId
                                             ,PsResource.CpcTitle
                                             ,PsResource.Id
                                             ,PsResource.MouId
                                             ,PsResource.ProjectCostId
                                             ,PsResource.PsId
                                             ,PsResource.SoldQuantity
                                             ,PsResource.SpToMou
                                             ,PsResource.Status
                                             ,PsResource.TotalCost
                                             ,CpcResourceTypeLocalizedData.Label AS CpcResourceType
                                             ,CpcBasicUnitOfMeasureLocalizedData.Label AS Mou
                                             ,PsResource.ProductTitle
                                             ,PsResource.GeneralLedgerId
                                            FROM dbo.PsResource
                                            LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                              ON CpcResourceTypeLocalizedData.CpcResourceTypeId = PsResource.CpcResourceTypeId
                                            LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
                                              ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = PsResource.MouId
                                            LEFT OUTER JOIN dbo.ProjectCost
                                              ON PsResource.ProjectCostId = ProjectCost.Id
                                            WHERE (CpcResourceTypeLocalizedData.LanguageCode = @lang
                                            OR PsResource.CpcResourceTypeId IS NULL)
                                            AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
                                            OR PsResource.MouId IS NULL) AND PsResource.PsId = @id";

        string resourceString;
        resourceString = psParameter.ProjectSequenceId == null ? cuResourceQuery : resourcequery;

        var accountingNumber = @"SELECT
  CabCompany.AccountingNumber
 ,ProjectDefinition.ProjectScopeStatusId
 ,ProjectScopeStatus.Name AS ProjectScope
FROM dbo.ProjectDefinition
LEFT OUTER JOIN dbo.ProjectTime
  ON ProjectDefinition.Id = ProjectTime.ProjectId
LEFT OUTER JOIN dbo.ProjectTeam
  ON ProjectDefinition.Id = ProjectTeam.ProjectId
LEFT OUTER JOIN dbo.ProjectTeamRole
  ON ProjectTeam.Id = ProjectTeamRole.ProjectTeamId
LEFT OUTER JOIN dbo.CabPerson
  ON ProjectTeamRole.CabPersonId = CabPerson.Id
LEFT OUTER JOIN dbo.CabPersonCompany
  ON CabPerson.Id = CabPersonCompany.PersonId
LEFT OUTER JOIN dbo.CabCompany
  ON CabCompany.Id = CabPersonCompany.CompanyId
LEFT OUTER JOIN dbo.ProjectScopeStatus
  ON ProjectDefinition.ProjectScopeStatusId = ProjectScopeStatus.StatusId
WHERE ProjectDefinition.SequenceCode = @project
AND ProjectTeamRole.RoleId = '907b7af0-b132-4951-a2dc-6ab82d4cd40d'
AND ProjectScopeStatus.LanguageCode = @lang"; //customer role id

        var accountingNumberParam = new { project = result.ProjectSequenceCode, lang };
        PsProjectDto accountNo;
        await using (var dbConnection =
                     new SqlConnection(psParameter.TenantProvider.GetTenant().ConnectionString))
        {
            accountNo = dbConnection.Query<PsProjectDto>(accountingNumber, accountingNumberParam)
                .FirstOrDefault();
            
        }

        var resourceParameters = new { lang, id = result.Id };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var psResources =
                dbConnection.Query<PsResourceReadDto>(resourceString, resourceParameters).ToList();
            
            var cbcResources =
                dbConnection.Query<PsResourceReadDto>(cbcResQuery, resourceParameters).ToList();
            psResources.AddRange(cbcResources);

            result.Resources = psResources;
        }

        IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyLogDto = null;
        var historyQuery =
            @"SELECT   PsHistoryLog.ChangedTime AS DateTime  ,PsHistoryLog.ChangedByUserId AS Oid,PsHistoryLog.RevisionNumber AS RevisionNumber 
                                        FROM dbo.PsHistoryLog WHERE PsHistoryLog.PsHeaderId =@id ORDER BY RevisionNumber";


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
            await using (var connection =
                         new SqlConnection(psParameter.TenantProvider.GetTenant().ConnectionString))
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
            await using (var connection =
                         new SqlConnection(psParameter.TenantProvider.GetTenant().ConnectionString))
            {
                userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                log.UpdatedByUser = userName.FirstOrDefault();
                
            }
        }

        if (accountNo != null)
        {
            result.AccountingNumber = accountNo.AccountingNumber;
            result.ProjectScopeStatusId = accountNo.ProjectScopeStatusId;
            result.ProjectScope = accountNo.ProjectScope;
        }

        result.historyLog = log;

        var gLQuery = @"SELECT Name FROM GenaralLederNumber WHERE Id = @Id";

        var glParameters = new { Id = result.GeneralLedgerId };
        await using (var dbConnection =
                     new SqlConnection(psParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dbConnection.Open();
            result.GeneralLedgerValue = dbConnection.Query<string>(gLQuery, glParameters).FirstOrDefault();
            
        }

        return result;
    }

    public async Task<PsHeaderReadDto> ReadExcel(PsParameter psParameter)
    {
        var lang = psParameter.Lang;

        var query =
            @"select PsHeader.Id,PsHeader.ProgressStatementId,PsHeader.Name, PsHeader.Title,PsStatus.StatusId AS ProjectStatusId, PsStatus.Name AS ProjectStatus ,PsHeader.Date,
                                 PsType.TypeId ProjectTypeId, PsType.Name AS ProjectType, PsHeader.WorkPeriodFrom, PsHeader.WorkPeriodTo, PsHeader.PurchaseOrderNumber, PsHeader.TotalAmount , PsHeader.ProjectSequenceCode, PsHeader.GeneralLedgerId
                                 from PsHeader
                                 LEFT OUTER JOIN PsStatus on PsStatus.StatusId= PsHeader.ProjectStatusId
                                 LEFT OUTER JOIN PsType on PsType.TypeId =PsHeader.ProjectTypeId 
                                 WHERE (PsStatus.LanguageCode = @lang OR PsHeader.ProjectStatusId IS NULL)
                                 AND (PsType.LanguageCode = @lang OR PsHeader.ProjectTypeId IS NULL)
                                 AND PsHeader.ProgressStatementId=@id";

        var connectionString = ConnectionString.MapConnectionString(psParameter.ContractingUnitSequenceId,
            psParameter.ProjectSequenceId, psParameter.TenantProvider);
        var parameters = new { lang, id = psParameter.PsId };
        PsHeaderReadDto result;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            result = dbConnection.Query<PsHeaderReadDto>(query, parameters).FirstOrDefault();
            
        }

        var resourcequery =
            @"select PsResource.ConsumedQuantity, PsResource.ConsumedQuantityMou, PsResource.CostToMou, PsResource.CpcId, PsResource.CpcResourceNumber, ProjectCost.ProductTitle,
                                        PsResource.CpcResourceTypeId, PsResource.CpcTitle, PsResource.Id, PsResource.MouId, PsResource.ProjectCostId, PsResource.PsId, PsResource.SoldQuantity,
                                        PsResource.SpToMou, PsResource.Status,PsResource.TotalCost, CpcResourceTypeLocalizedData.Label AS CpcResourceType, CpcBasicUnitOfMeasureLocalizedData.Label AS Mou, PsResource.GeneralLedgerId
                                        from PsResource
                                        LEFT OUTER JOIN CpcResourceTypeLocalizedData on CpcResourceTypeLocalizedData.CpcResourceTypeId=PsResource.CpcResourceTypeId
                                        LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = PsResource.MouId
                                        LEFT OUTER JOIN ProjectCost on PsResource.ProjectCostId = ProjectCost.Id
                                        WHERE (CpcResourceTypeLocalizedData.LanguageCode = @lang OR PsResource.CpcResourceTypeId IS NULL)
                                        AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR PsResource.MouId IS NULL)
                                        AND PsResource.PsId = @id";

        var cuResourceQuery = @"SELECT
                                              PsResource.ConsumedQuantity
                                             ,PsResource.ConsumedQuantityMou
                                             ,PsResource.CostToMou
                                             ,PsResource.CpcId
                                             ,PsResource.CpcResourceNumber
                                             ,PsResource.CpcResourceTypeId
                                             ,PsResource.CpcTitle
                                             ,PsResource.Id
                                             ,PsResource.MouId
                                             ,PsResource.ProjectCostId
                                             ,PsResource.PsId
                                             ,PsResource.SoldQuantity
                                             ,PsResource.SpToMou
                                             ,PsResource.Status
                                             ,PsResource.TotalCost
                                             ,CpcResourceTypeLocalizedData.Label AS CpcResourceType
                                             ,CpcBasicUnitOfMeasureLocalizedData.Label AS Mou
                                             ,PsResource.ProductTitle
                                             ,PsResource.GeneralLedgerId
                                            FROM dbo.PsResource
                                            LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                              ON CpcResourceTypeLocalizedData.CpcResourceTypeId = PsResource.CpcResourceTypeId
                                            LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
                                              ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = PsResource.MouId
                                            LEFT OUTER JOIN dbo.ProjectCost
                                              ON PsResource.ProjectCostId = ProjectCost.Id
                                            WHERE (CpcResourceTypeLocalizedData.LanguageCode = @lang
                                            OR PsResource.CpcResourceTypeId IS NULL)
                                            AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
                                            OR PsResource.MouId IS NULL) AND PsResource.PsId = @id";

        string resourceString;
        if (psParameter.ProjectSequenceId == null)
            resourceString = cuResourceQuery;
        else
            resourceString = resourcequery;


        var resourceParameters = new { lang, id = result.Id };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var psResources =
                dbConnection.Query<PsResourceReadDto>(resourceString, resourceParameters);
            

            result.Resources = psResources;
        }

        IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyLogDto = null;
        var historyQuery =
            @"SELECT   PsHistoryLog.ChangedTime AS DateTime  ,PsHistoryLog.ChangedByUserId AS Oid,PsHistoryLog.RevisionNumber AS RevisionNumber 
                                        FROM dbo.PsHistoryLog WHERE PsHistoryLog.PsHeaderId =@id ORDER BY RevisionNumber";

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
            using (var connection =
                   new SqlConnection(psParameter.TenantProvider.GetTenant().ConnectionString))
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
                   new SqlConnection(psParameter.TenantProvider.GetTenant().ConnectionString))
            {
                userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                log.UpdatedByUser = userName.FirstOrDefault();
                
            }
        }

        result.historyLog = log;
        return result;
    }

    public async Task<string> CreatePsResource(PsParameter parameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        string id;
        using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
        {
            var deletequery = @"DELETE FROM dbo.PsResource WHERE PsId = @Id";

            var parm = new { Id = parameter.PsResourceCreateDto.PsId };
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(deletequery, parm);
            }

            if (parameter.PsResourceCreateDto.Resources != null)
                foreach (var dto in parameter.PsResourceCreateDto.Resources)
                {
                    if (dto.CpcResourceTypeId == "cbc3a26-cbc-cbc-cbc-89655304cbc")//cbc
                    {
                        dto.ProjectCostId = null;
                    }
                    var query = @"INSERT INTO dbo.PsResource
                                        ( Id,PsId,CpcId,CpcResourceNumber,CpcTitle,ConsumedQuantity,CostToMou,TotalCost,SoldQuantity,SpToMou,Status,CpcResourceTypeId,ConsumedQuantityMou,MouId,ProjectCostId,ProductTitle,GeneralLedgerId)
                                        VALUES
                                        ( @Id,@PsId,@CpcId,@CpcResourceNumber,@CpcTitle,@ConsumedQuantity,@CostToMou,@TotalCost,@SoldQuantity,@SpToMou,@Status,@CpcResourceTypeId,@ConsumedQuantityMou,@MouId,@ProjectCostId,@ProductTitle,@GeneralLedgerId);";


                    var parameters = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        parameter.PsResourceCreateDto.PsId,
                        dto.CpcId,
                        dto.CpcResourceNumber,
                        dto.CpcTitle,
                        dto.ConsumedQuantity,
                        dto.CostToMou,
                        dto.TotalCost,
                        dto.SoldQuantity,
                        dto.SpToMou,
                        dto.Status,
                        dto.CpcResourceTypeId,
                        dto.ConsumedQuantityMou,
                        dto.MouId,
                        dto.ProjectCostId,
                        dto.ProductTitle,
                        parameter.PsResourceCreateDto.GeneralLedgerId
                    };


                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.ExecuteAsync(query, parameters);
                    }
                }

            await UpdateTotal(parameter);

            return parameter.PsResourceCreateDto.PsId;
        }
    }

    public async Task<string> ApprovePs(PsParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        var cuConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            null, parameter.TenantProvider);

        var psUpdateQuery = @"update PsHeader set ProjectStatusId = '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'
                                    where Id = @id";


        var param = new { id = parameter.PsId };

        var readPsHeader = @"SELECT
                                          *
                                        FROM dbo.PsHeader
                                        WHERE PsHeader.Id = @id";
        PsHeader mPsHeader;

        using (var connection = new SqlConnection(connectionString))
        {
            mPsHeader = connection.Query<PsHeader>(readPsHeader, param).SingleOrDefault();
            connection.Close();
        }

        parameter.PsId = mPsHeader.ProgressStatementId;
        var mPsHeaderReadDto = await ReadPsById(parameter);
        var newId = Guid.NewGuid().ToString();

        var psHeader = @"INSERT INTO dbo.PsHeader
                                (
                                  Id
                                 ,ProgressStatementId
                                 ,Name
                                 ,Title
                                 ,ProjectTypeId
                                 ,ProjectStatusId
                                 ,WorkPeriodFrom
                                 ,WorkPeriodTo
                                 ,PurchaseOrderNumber
                                 ,Date
                                 ,TotalAmount
                                 ,ProjectSequenceCode
                                 ,GeneralLedgerId
                                 ,InvoiceComment  
                                )
                                VALUES
                                (
                                  @id
                                 ,@ProgressStatementId
                                 ,@Name
                                 ,@Title
                                 ,@ProjectTypeId
                                 ,@ProjectStatusId
                                 ,@WorkPeriodFrom
                                 ,@WorkPeriodTo
                                 ,@PurchaseOrderNumber
                                 ,GETDATE()
                                 ,@TotalAmount
                                 ,@ProjectSequenceCode
                                 ,@GeneralLedgerId
                                 ,@InvoiceComment
                                );";
        var paramInsert = new
        {
            id = newId,
            mPsHeaderReadDto.ProgressStatementId,
            mPsHeaderReadDto.Name,
            mPsHeaderReadDto.Title,
            mPsHeaderReadDto.ProjectTypeId,
            mPsHeaderReadDto.ProjectStatusId,
            mPsHeaderReadDto.WorkPeriodFrom,
            mPsHeaderReadDto.WorkPeriodTo,
            mPsHeaderReadDto.PurchaseOrderNumber,
            mPsHeaderReadDto.TotalAmount,
            ProjectSequenceCode = parameter.ProjectSequenceId,
            mPsHeaderReadDto.GeneralLedgerId,
            mPsHeaderReadDto.InvoiceComment
        };


        var instertResource = @"
                                        INSERT INTO dbo.PsResource
                                        (
                                          Id
                                         ,PsId
                                         ,CpcId
                                         ,CpcResourceNumber
                                         ,CpcTitle
                                         ,ConsumedQuantity
                                         ,CostToMou
                                         ,TotalCost
                                         ,SoldQuantity
                                         ,SpToMou
                                         ,Status
                                         ,CpcResourceTypeId
                                         ,ConsumedQuantityMou
                                         ,MouId
                                        ,ProductTitle
                                        ,GeneralLedgerId
                                        )
                                        VALUES
                                        (
                                          @Id
                                         ,@PsId
                                         ,@CpcId
                                         ,@CpcResourceNumber
                                         ,@CpcTitle
                                         ,@ConsumedQuantity
                                         ,@CostToMou
                                         ,@TotalCost
                                         ,@SoldQuantity
                                         ,@SpToMou
                                         ,@Status
                                         ,@CpcResourceTypeId
                                         ,@ConsumedQuantityMou
                                         ,@MouId
                                         ,@ProductTitle
                                         ,@GeneralLedgerId
                                        );";


        await using (var connection = new SqlConnection(cuConnectionString))
        {
            connection.Query(psHeader, paramInsert);
            connection.Close();
        }

        await using (var connection = new SqlConnection(connectionString))
        {
            connection.Query(psUpdateQuery, param);
            connection.Close();
        }

        foreach (var mPsResourceReadDto in mPsHeaderReadDto.Resources)
            await using (var connection = new SqlConnection(cuConnectionString))
            {
                var paramInsertResource = new
                {
                    id = Guid.NewGuid().ToString(),
                    PsId = newId,
                    mPsResourceReadDto.CpcId,
                    mPsResourceReadDto.CpcResourceNumber,
                    mPsResourceReadDto.CpcTitle,
                    mPsResourceReadDto.ConsumedQuantity,
                    mPsResourceReadDto.CostToMou,
                    mPsResourceReadDto.TotalCost,
                    mPsResourceReadDto.SoldQuantity,
                    mPsResourceReadDto.SpToMou,
                    mPsResourceReadDto.Status,
                    mPsResourceReadDto.CpcResourceTypeId,
                    mPsResourceReadDto.ConsumedQuantityMou,
                    mPsResourceReadDto.MouId,
                    mPsResourceReadDto.ProductTitle,
                    mPsHeaderReadDto.GeneralLedgerId
                };
                connection.Query(instertResource, paramInsertResource);
                connection.Close();
            }

        var options = new DbContextOptions<ShanukaDbContext>();
        await using (var context = new ShanukaDbContext(options, cuConnectionString, parameter.TenantProvider))
        {
            var jsonProduct = JsonConvert.SerializeObject(mPsHeader, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var historyLog = new PsHistoryLog
            {
                Id = Guid.NewGuid().ToString(),
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = parameter.UserId,
                HistoryLog = jsonProduct,
                PsHeaderId = newId,
                Action = HistoryState.ADDED.ToString()
            };
            context.PsHistoryLog.Add(historyLog);
            await context.SaveChangesAsync();
            return parameter.PsId;
        }
    }

    public async Task<string> CUApprovePs(PsParameter parameter)
    {
        PsHeader ps;
        var cuConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            null, parameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);


        var psUpdateQuery = @"update PsHeader set ProjectStatusId = '4010e768-3e06-4702-b337-ee367a82addb'
                                    where ProgressStatementId = @id";

        await using (var connection = new SqlConnection(connectionString))
        {
            ps = connection.Query<PsHeader>("SELECT * FROM PsHeader WHERE Id = @id", new { id = parameter.PsId })
                .FirstOrDefault();
        }

        ProjectDefinition mProjectDefinition;
        Tax mTax;
        var options = new DbContextOptions<ShanukaDbContext>();
        await using (var contextProject = new ShanukaDbContext(options,
                         parameter.TenantProvider.GetTenant().ConnectionString, parameter.TenantProvider))
        {
            mProjectDefinition =
                contextProject.ProjectDefinition.FirstOrDefault(p => p.SequenceCode == parameter.ProjectSequenceId);
            if (mProjectDefinition != null && mProjectDefinition.VATId != null)
                mTax = contextProject.Tax.FirstOrDefault(T => T.Id == mProjectDefinition.VATId);
            else
                mTax = contextProject.Tax.FirstOrDefault(T => T.IsDefault == true);
        }

        var idGenerator = new IdGenerator();
        var invoiceId =
            idGenerator.GenerateId(parameter.ApplicationDbContext, "INV-", "InvoiceSequenceCode");
        var invoiceCreationQuery = @"insert into Invoice
                                                select NEWID(), @invoiceId, PsHeader.Name, CONCAT(@invoiceId,' - ', PsHeader.Name),'d60aad0b-2e84-482b-ad25-618d80d49477',
                                                PsHeader.WorkPeriodFrom, PsHeader.WorkPeriodTo,PsHeader.PurchaseOrderNumber, @vat, PsHeader.TotalAmount , @id, GETDATE(), PsHeader.ProjectSequenceCode,  PsHeader.Title
                                                from PsHeader 
                                                where Id = @id";


        var readPsHeader = @"SELECT
                                          *
                                        FROM dbo.PsHeader
                                        WHERE PsHeader.ProgressStatementId = @id";
        PsHeader mPsHeader;

        var param = new { id = ps.ProgressStatementId, invoiceId };

        await using (var connection = new SqlConnection(cuConnectionString))
        {
            mPsHeader = connection.Query<PsHeader>(readPsHeader, param).SingleOrDefault();
            connection.Close();
        }

        if (mTax != null)
        {
            var paramInvoce = new
            {
                id = mPsHeader?.Id,
                invoiceId,
                vat = mPsHeader?.TotalAmount +
                      mPsHeader?.TotalAmount * double.Parse(mTax.Name.Split("%").First()) / 100
            };


            await using (var connection = new SqlConnection(connectionString))
            {
                var affectedRows1 = connection.QuerySingleOrDefaultAsync(psUpdateQuery, param).Result;
                connection.Close();
            }

            await using (var connection = new SqlConnection(cuConnectionString))
            {
                var affectedRows1 = connection.QuerySingleOrDefaultAsync(psUpdateQuery, param).Result;
                var affectedRows2 = connection.QuerySingleOrDefaultAsync(invoiceCreationQuery, paramInvoce).Result;
                connection.Close();
            }
        }

        return parameter.PsId;
    }

    public async Task<PsCustomerReadDto> GetCustomer(PmolParameter pmolParameter)
    {
        var lang = pmolParameter.Lang;


        PsCustomerReadDto dropdownData = null;

        var query =
            @" select ProjectDefinition.Id, ProjectTime.EndDate, CabPerson.FullName AS ContactPersonName, CabPerson.Id AS CabPersonId,
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
        var param = new { lang, id = pmolParameter.ProjectSequenceId };


        using var connection = new SqlConnection(pmolParameter.TenantProvider.GetTenant().ConnectionString);
        dropdownData = connection.Query<PsCustomerReadDto>(query, param).FirstOrDefault();

        return dropdownData;
    }

    private void CreateHistory(PsParameter parameter, bool isExist)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        using var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider);
        var jsonProduct = JsonConvert.SerializeObject(parameter.PsHeaderCreateDto, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var historyLog = new PsHistoryLog
        {
            Id = Guid.NewGuid().ToString(),
            ChangedTime = DateTime.UtcNow,
            ChangedByUserId = parameter.UserId,
            HistoryLog = jsonProduct,
            PsHeaderId = parameter.PsHeaderCreateDto.Id
        };

        if (isExist == false)
            historyLog.Action = HistoryState.ADDED.ToString();
        else
            historyLog.Action = HistoryState.UPDATED.ToString();

        context.PsHistoryLog.Add(historyLog);
        context.SaveChanges();
    }

    private StringBuilder FilterByDate(StringBuilder sb, PmolParameter PmolParameter)
    {
        switch (PmolParameter.filter.Date)
        {
            case 0:
            case 1:
            case -1:
            {
                var gmt = FindGmtDatetime(PmolParameter);
                sb.Append("  AND ExecutionDate BETWEEN '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
                break;
            }
            case -7:
            case -14:
                //sb.Append("  AND ExecutionDate BETWEEN '" + gmt + "' AND '" + gmt.AddDays(7) + "' ");
                break;
            case -30:
            case -60:
                //GmtMonthDto gmt = FindGmtMonth(PmolParameter);
                //sb.Append("  AND ExecutionDate BETWEEN '" + gmt.FirstDay + "' AND '" + gmt.LastDay + "' ");
                break;
        }

        return sb;
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

    public async Task<string> UpdateTotal(PsParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        var query = "update PsHeader set TotalAmount = @total where Id = @id";
        var updateUsed = @"UPDATE dbo.ProjectCost 
                                        SET
                                          isUsed = 1 
                                        WHERE
                                          Id IN (SELECT
                                          PsResource.ProjectCostId
                                        FROM dbo.PsResource
                                        WHERE PsResource.PsId = @id
                                        ) ";
        var param = new
            { id = parameter.PsResourceCreateDto.PsId, total = parameter.PsResourceCreateDto.GrandTotal };
        await using (var connection = new SqlConnection(connectionString))
        {
            connection.Query(query, param);
            connection.Query(updateUsed, param);

            connection.Close();
        }

        return "ok";
    }
}