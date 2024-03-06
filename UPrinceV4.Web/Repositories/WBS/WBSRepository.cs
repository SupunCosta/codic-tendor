using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Dapper;
using MickiesoftMuiltitenant.Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Kiota.Abstractions;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Issue;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.WBS;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.WBS;

public class WbsRepository : IWbsRepository
{
    public async Task<string> CreateWbs(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);

        // var options = new DbContextOptions<ApplicationDbContext>();
        // var applicationDbContext =
        //     new ApplicationDbContext(options, wbsParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        var wbsData = connection.Query<WbsTaxonomy>(@"SELECT * FROM dbo.WbsTaxonomy WHERE Id = @Id", wbsParameter.WbsTaxonomy).FirstOrDefault();

        if (wbsData == null)
        {
            //var idGenerator = new IdGenerator();
           // wbsParameter.WbsTaxonomy.SequenceId = idGenerator.GenerateId(applicationDbContext, "WBS-", "WbsSequence");
            wbsParameter.WbsTaxonomy.CreatedBy = wbsParameter.UserId;
            wbsParameter.WbsTaxonomy.CreatedDateTime = DateTime.UtcNow;
            
            const string insertWbs = @"INSERT INTO dbo.WbsTaxonomy ( Id ,WbsTaxonomyLevelId ,ParentId ,CreatedDateTime ,CreatedBy,SequenceId ,Title,IsDefault,TemplateId ) VALUES ( @Id ,@WbsTaxonomyLevelId ,@ParentId ,@CreatedDateTime ,@CreatedBy,@SequenceId ,@Title,@IsDefault,@TemplateId);";

            await connection.ExecuteAsync(insertWbs, wbsParameter.WbsTaxonomy);

            if (wbsParameter.WbsTaxonomy.WbsTaxonomyLevelId == "e1ce52c0-058b-prod-afbd-1d2d24105ebc")//product
            {
                var wbsProductCreateParam = new WbsProductCreate()
                {
                    Id = wbsParameter.WbsTaxonomy.Id,
                    Name = wbsParameter.WbsTaxonomy.Title,
                    WbsTaxonomyId = wbsParameter.WbsTaxonomy.ParentId,
                    Documents = new List<WbsProductDocuments>(),
                    Tags = new List<WbsProductTags>(),
                    ToPerson = new List<WbsProductTo>(),
                    Cc = new List<WbsProductCc>()
                };

                wbsParameter.wbsProductCreate = wbsProductCreateParam;
                await WbsProductCreate(wbsParameter);
            }
            
            if (wbsParameter.WbsTaxonomy.WbsTaxonomyLevelId == "i1ce52c0-058b-issu-afbd-1d2d24105ebc") //task
            {

                var pbs = connection.Query<string>("Select ProductId from PbsProduct Where Id =@Id",
                    new { Id = wbsParameter.WbsTaxonomy.ParentId }).FirstOrDefault();
                var wbsTaskCreateParam = new WbsTaskCreate()
                {
                    Id = wbsParameter.WbsTaxonomy.Id,
                    Name = wbsParameter.WbsTaxonomy.Title,
                    WbsTaxonomyId = wbsParameter.WbsTaxonomy.ParentId,
                    Date = DateTime.UtcNow,
                    ProductId = pbs,
                    Tags= new List<WbsTaskTags>(),
                    ToPerson = new List<WbsTaskTo>(),
                    Cc = new List<WbsTaskCc>(),
                    Documents = new List<WbsTaskDocuments>(),
                    Instructions = new List<TaskInstruction>()
                };

                wbsParameter.wbsTaskCreate = wbsTaskCreateParam;
                await WbsTaskCreate(wbsParameter);
            }
            
            if (wbsParameter.WbsTaxonomy.WbsTaxonomyLevelId == "pouoe52c0-hvkhl-lbjm-jvhj-1d2d241ouyi") //issue
            {
                var pbs = connection.Query<string>("Select ProductId from PbsProduct Where Id =@Id",
                    new { Id = wbsParameter.WbsTaxonomy.ParentId }).FirstOrDefault();
                var issueParam = new IssueParameter()
                {
                    ContractingUnitSequenceId = wbsParameter.ContractingUnitSequenceId,
                    ProjectSequenceId = wbsParameter.ProjectSequenceId,
                    Lang = wbsParameter.Lang,
                    TenantProvider = wbsParameter.TenantProvider,
                    IssueHeader = new IssueHeaderCreateDto()
                    {
                        Id = wbsParameter.WbsTaxonomy.Id,
                        Title = wbsParameter.WbsTaxonomy.Title,
                        WbsId = wbsParameter.WbsTaxonomy.ParentId,
                        CreatedBy = wbsParameter.UserId,
                        Tags = new List<TagDto>(),
                        ToPerson = new List<IssueTo>(),
                        Cc = new List<IssueCc>(),
                        Documents = new List<DocDto>(),
                        DateRaised = DateTime.UtcNow,
                        DecisionDate = DateTime.UtcNow,
                        ClosureDate = DateTime.UtcNow,
                        LastUpdate = DateTime.UtcNow,
                        Status = "0e1b34bc-f2c3-isis-8250-9666ee96ae59",
                        ProductId = pbs
                        
                    }

                };
                await wbsParameter.iIssueRepository.CreateIssue(issueParam);
            }
            
            
            
        }

        else
        {
            wbsParameter.WbsTaxonomy.UpdatedBy = wbsParameter.UserId;
            wbsParameter.WbsTaxonomy.UpdatedDateTime = DateTime.UtcNow;
            
            const string update = @"UPDATE dbo.WbsTaxonomy SET WbsTaxonomyLevelId =@WbsTaxonomyLevelId ,ParentId =@ParentId ,UpdatedDateTime =@UpdatedDateTime ,UpdatedBy =@UpdatedBy,Title =@Title,IsDefault = @IsDefault WHERE Id =@Id ;";

            await connection.ExecuteAsync(update, wbsParameter.WbsTaxonomy);
        }

        return wbsParameter.WbsTaxonomy.Id;
    }

    public async Task<string> CreateTemplate(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, wbsParameter.TenantProvider);
        
        var idGenerator = new IdGenerator();
        wbsParameter.WbsTemplate.SequenceCode = idGenerator.GenerateId(applicationDbContext, "Template-", "WbsTemplateSequence");
        wbsParameter.WbsTemplate.Title = wbsParameter.WbsTemplate.SequenceCode + " " + wbsParameter.WbsTemplate.Name;
        wbsParameter.WbsTemplate.CreatedBy = wbsParameter.UserId;
        wbsParameter.WbsTemplate.CreatedDateTime = DateTime.UtcNow;

        await using var connection = new SqlConnection(connectionString);
        
        await connection.ExecuteAsync(@"INSERT INTO dbo.WbsTemplate ( Id ,Title,IsDelete,IsDefault ,CreatedDateTime ,CreatedBy,Name,SequenceCode ) VALUES ( @Id ,@Title,@IsDelete,@IsDefault ,@CreatedDateTime ,@CreatedBy,@Name,@SequenceCode );", wbsParameter.WbsTemplate);

        return wbsParameter.WbsTemplate.Id;
    }

    public async Task<IEnumerable<WbsTemplate>> WbsFilter(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            null, wbsParameter.TenantProvider);

        const string sql = @"SELECT
                              Id
                             ,Title
                             ,IsDelete
                             ,IsDefault
                             ,CreatedDateTime
                             ,UpdatedDateTime
                             ,CreatedBy
                             ,Name
                             ,SequenceCode
                            FROM dbo.WbsTemplate
                            WHERE IsDelete = 0 ";

        var sb = new StringBuilder(sql);

        if (wbsParameter.WbsFilter.Title != null)
        {
            sb.Append("AND Title LIKE '%"+wbsParameter.WbsFilter.Title+"%'");
        }

        if (wbsParameter.WbsFilter.Sorter.Attribute == null)
        {
            //sb.Append("ORDER BY CAST(SUBSTRING(WbsTemplate.SequenceCode,9,20) AS INT) desc ");
            sb.Append("ORDER BY WbsTemplate.Title desc ");
        }

        if (wbsParameter.WbsFilter.Sorter.Attribute != null)
        {
            switch (wbsParameter.WbsFilter.Sorter.Attribute.ToLower())
            {
                case "title":
                    sb.Append("ORDER BY WbsTemplate.Title " +
                              wbsParameter.WbsFilter.Sorter.Order); 
                    break;
            }
        }
        
        await using var connection = new SqlConnection(connectionString);

        var data = await connection.QueryAsync<WbsTemplate>(sb.ToString(), new{lang = wbsParameter.Lang});

        return data;
    }

    public async Task<List<WbsTaxonomyList>> GetWbsTaxonomyByTemplate(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
       // var result = connection.Query<WbsTaxonomyList>(@"SELECT * FROM dbo.WbsTaxonomy WHERE TemplateId = @Id", new { wbsParameter.Id }).ToList();
       var data =   WbsTaxonomyList(wbsParameter).Result;
       
       var result =   data.Where(x => x.TemplateId == wbsParameter.Id ).ToList();
       
        return result;
    }

    public async Task<WbsDropDownData> GetWbsDropdown(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);
        const string query =
            @"select wtl.LevelId as [Key], wtl.Name as Text  from WbsTaxonomyLevels wtl where LanguageCode = @lang ORDER BY DisplayOrder;
                select StatusId as [Key], Name as Text  from WbsTaskStatus where LanguageCode = @lang ORDER BY DisplayOrder;
                select StatusId as [Key], Name as Text  from WbsTaskDeliveryStatus where LanguageCode = @lang ORDER BY DisplayOrder;";
        
        var mWbsDropDownData= new WbsDropDownData();
        
        
        var multi = await connection.QueryMultipleAsync(query,new{lang = wbsParameter.Lang});
        mWbsDropDownData.WbsTaxonomyLevel = multi.Read<WbsTaxonomyLevelDto>();
        mWbsDropDownData.Status = multi.Read<WbsTaxonomyLevelDto>();
        mWbsDropDownData.DeliveryStatus = multi.Read<WbsTaxonomyLevelDto>();


        return mWbsDropDownData;
    }
    
    public async Task<WbsTaxonomyList> GetWbsById(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);

        const string sql = @"SELECT
                      dbo.WbsTaxonomy.Id
                     ,dbo.WbsTaxonomy.WbsTaxonomyLevelId
                     ,dbo.WbsTaxonomy.ParentId
                     ,dbo.WbsTaxonomy.CreatedDateTime
                     ,dbo.WbsTaxonomy.UpdatedDateTime
                     ,dbo.WbsTaxonomy.CreatedBy
                     ,dbo.WbsTaxonomy.UpdatedBy
                     ,dbo.WbsTaxonomy.Name
                     ,dbo.WbsTaxonomy.SequenceId
                     ,dbo.WbsTaxonomy.Title
                     ,dbo.WbsTaxonomy.TemplateId
                     ,wtl.Name AS WbsTaxonomyLevel
                    FROM dbo.WbsTaxonomy
                    LEFT OUTER JOIN WbsTaxonomyLevels wtl
                    ON WbsTaxonomy.WbsTaxonomyLevelId = wtl.LevelId
                    WHERE wtl.LanguageCode = @lang AND WbsTaxonomy.Id = @Id";
        
        var wbsData = connection.Query<WbsTaxonomyList>(sql, new{wbsParameter.Id,lang = wbsParameter.Lang}).FirstOrDefault();

        const string cabPersonSql = @"SELECT cp.FullName FROM CabPerson cp JOIN CabPersonCompany cpc ON cp.Id = cpc.PersonId WHERE cpc.Oid = @person ";

        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        if (wbsData?.CreatedBy != null)
        {
            wbsData.CreatedBy = dbConnection.Query<string>(cabPersonSql, new { person = wbsData.CreatedBy }).FirstOrDefault();
        }
        
        if (wbsData?.UpdatedBy != null)
        {
            wbsData.UpdatedBy = dbConnection.Query<string>(cabPersonSql, new { person = wbsData.UpdatedBy }).FirstOrDefault();
        }

        return wbsData;
    }
    
    public async Task<List<WbsTaxonomyList>> WbsTaxonomyList(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);

        var result = new List<WbsTaxonomyList>();
        const string sql = @"SELECT
                      dbo.WbsTaxonomy.Id
                     ,dbo.WbsTaxonomy.WbsTaxonomyLevelId
                     ,dbo.WbsTaxonomy.ParentId
                     ,dbo.WbsTaxonomy.CreatedDateTime
                     ,dbo.WbsTaxonomy.UpdatedDateTime
                     ,dbo.WbsTaxonomy.CreatedBy
                     ,dbo.WbsTaxonomy.UpdatedBy
                     ,dbo.WbsTaxonomy.Name
                     ,dbo.WbsTaxonomy.SequenceId
                     ,dbo.WbsTaxonomy.Title
                     ,dbo.WbsTaxonomy.TemplateId
                     ,wtl.Name AS WbsTaxonomyLevel
                    FROM dbo.WbsTaxonomy
                    LEFT OUTER JOIN WbsTaxonomyLevels wtl
                    ON WbsTaxonomy.WbsTaxonomyLevelId = wtl.LevelId
                    WHERE wtl.LanguageCode = @lang AND WbsTaxonomy.ParentId IS NUll ORDER BY dbo.WbsTaxonomy.Title";
        
        await using var connection = new SqlConnection(connectionString);
        
        var data =  connection.Query<WbsTaxonomyList>(sql, new{lang = wbsParameter.Lang,wbsParameter.Id});

        foreach (var node in data)
        {
            result.Add(node);
            
            var childPbs = connection.Query<WbsTaxonomyList>(@"WITH ret
                                                                                    AS
                                                                                    (SELECT
                                                                                        *
                                                                                      FROM WbsTaxonomy
                                                                                      WHERE Id = @Id
                                                                                      UNION ALL
                                                                                      SELECT
                                                                                        t.*
                                                                                      FROM WbsTaxonomy t
                                                                                      INNER JOIN ret r
                                                                                        ON t.ParentId = r.Id)
                                                                                    SELECT
                                                                                      ret.*
                                                                                     ,CASE
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN pp.Id
                                                                                        ELSE NULL
                                                                                      END AS PbsId
                                                                                     ,CASE
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN pp.ProductId
                                                                                        ELSE NULL
                                                                                      END AS ProductId
                                                                                    FROM ret
                                                                                    LEFT OUTER JOIN PbsProduct pp
                                                                                      ON ret.Id = pp.Id
                                                                                    WHERE ret.Id != @Id ", new { Id = node.Id }).ToList().OrderBy(e => e.CreatedDateTime).ToList();

            result.AddRange(childPbs);
            
        }
        
        return result;
    }
    
    public async Task<List<PbsTreeStructure>> WbsProductFilter(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId = "COM-0001";

        var cuConnectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var cuConnection = new SqlConnection(cuConnectionString);
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var data = new List<PbsTreeStructure>();
        var sql = @"SELECT DISTINCT
                      ProjectDefinition.SequenceCode AS SequenceCode
                     ,ProjectDefinition.Name AS Name
                     ,ProjectDefinition.Id
                     ,ProjectDefinition.Title AS Title
                     ,ProjectDefinition.ProjectConnectionString
                     ,CabCompany.SequenceCode As Cu
                    FROM dbo.ProjectDefinition
                    LEFT OUTER JOIN dbo.ProjectUserRole
                      ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                    LEFT OUTER JOIN dbo.UserRole
                      ON ProjectUserRole.UsrRoleId = UserRole.Id
                    LEFT OUTER JOIN dbo.ApplicationUser
                      ON UserRole.ApplicationUserOid = ApplicationUser.OId
                    LEFT OUTER JOIN dbo.CabCompany
                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                    WHERE ApplicationUser.OId = @oid 
                    AND ProjectDefinition.IsDeleted = 0 ";
        
                var sb = new StringBuilder(sql);

                if (wbsParameter.WbsProductFilter.ProjectTitle != null)
                {
                    wbsParameter.WbsProductFilter.ProjectTitle =
                        wbsParameter.WbsProductFilter.ProjectTitle.Replace("'", "''");

                    sb.Append(" AND ProjectDefinition.Title like '%" + wbsParameter.WbsProductFilter.ProjectTitle +
                              "%'  ");
                }
                
                var param = new { lang = wbsParameter.Lang, oid = wbsParameter.UserId, cuId = wbsParameter.ContractingUnitSequenceId };

                var userProjects = dbConnection.Query<UserProjectList>(sb.ToString(), param).OrderBy(x => x.SequenceCode);

                foreach (var projects in userProjects)
                {
                    await using var projectConnection = new SqlConnection(projects.ProjectConnectionString);
                    
                     
                     var pbs = projectConnection.Query<PbsTreeStructure>( @"SELECT
                      dbo.WbsTaxonomy.Id
                     ,dbo.WbsTaxonomy.ParentId
                     ,dbo.WbsTaxonomy.WbsTaxonomyLevelId
                     ,dbo.WbsTaxonomy.TemplateId
                     ,CASE
                      WHEN dbo.WbsTaxonomy.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN pp.Id
                      ELSE NULL
                      END AS PbsId
                     ,CASE
                      WHEN dbo.WbsTaxonomy.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN pp.ProductId 
                      ELSE NULL
                      END AS PbsSequenceId
                     ,CASE
                      WHEN dbo.WbsTaxonomy.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN  CONCAT(pp.ProductId,' - ',dbo.WbsTaxonomy.Title) 
                      ELSE dbo.WbsTaxonomy.Title
                     END AS Title
                     ,CASE
                      WHEN dbo.WbsTaxonomy.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN  'Product'
                      WHEN dbo.WbsTaxonomy.WbsTaxonomyLevelId = 'i1ce52c0-058b-issu-afbd-1d2d24105ebc' THEN  'Task'
                      WHEN dbo.WbsTaxonomy.WbsTaxonomyLevelId = 'pouoe52c0-hvkhl-lbjm-jvhj-1d2d241ouyi' THEN  'Issue'
                      WHEN dbo.WbsTaxonomy.WbsTaxonomyLevelId = '41ce52c0-058b-wbs-afbd-1d2d24105ebc' THEN  'Wbs'
                      ELSE null
                     END AS Type,
                     wt.Title AS Wbs
                    FROM dbo.WbsTaxonomy LEFT OUTER JOIN PbsProduct pp ON WbsTaxonomy.Id = pp.Id
                    LEFT OUTER JOIN WbsTaxonomy wt ON WbsTaxonomy.ParentId = wt.Id 
                    ORDER BY dbo.WbsTaxonomy.Title asc").ToList();

                    if (pbs.Any())
                    {
                        
                        if (wbsParameter.WbsProductFilter.ProductTitle != null)
                        {
                            pbs = pbs.Where(x => x.Title != null).ToList();
                            wbsParameter.WbsProductFilter.ProductTitle =
                                wbsParameter.WbsProductFilter.ProductTitle.Replace("'", "''");
                            pbs = pbs.Where(x => x.Title.ToLower().Contains(wbsParameter.WbsProductFilter.ProductTitle.ToLower())).ToList();
                            pbs.ForEach(x => x.ParentId = projects.SequenceCode);
                            
                        }

                        if (pbs.Any())
                        {
                            var projectNode = new PbsTreeStructure()
                            {
                                Id = projects.SequenceCode,
                                ProjectSequenceId = projects.SequenceCode,
                                Title = projects.Title,
                                Cu = projects.Cu,
                                Type = "Project"
                            };
                            
                            data.Add(projectNode);
                            
                             pbs.Where(c => c.ParentId == null).ToList().ForEach(x => x.ParentId = projects.SequenceCode);
                             
                             
                             var parentPbs = pbs.Where(x => x.ParentId == projects.SequenceCode);

                             foreach (var item in parentPbs)
                             {
                                 try
                                 {

                                 
                                     item.ProjectSequenceId = projects.SequenceCode;
                                     item.Cu = projects.Cu;
                                     item.ProjectTitle = projects.Title;

                                     var childPbs = projectConnection.Query<PbsTreeStructure>(@"WITH ret
                                                                                    AS
                                                                                    (SELECT
                                                                                        *
                                                                                      FROM WbsTaxonomy
                                                                                      WHERE Id = @Id
                                                                                      UNION ALL
                                                                                      SELECT
                                                                                        t.*
                                                                                      FROM WbsTaxonomy t
                                                                                      INNER JOIN ret r
                                                                                        ON t.ParentId = r.Id)
                                                                                    SELECT
                                                                                       ret.Id,
                                                                                      ret.WbsTaxonomyLevelId,
                                                                                      ret.ParentId,
                                                                                      ret.CreatedDateTime,
                                                                                      ret.CreatedBy,
                                                                                      ret.TemplateId
                                                                                     ,CASE
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN pp.Id
                                                                                        ELSE NULL
                                                                                      END AS PbsId
                                                                                     ,CASE
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN pp.ProductId 
                                                                                        ELSE NULL
                                                                                      END AS PbsSequenceId
                                                                                      ,CASE
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN  CONCAT(pp.ProductId,' - ',ret.Title) 
                                                                                        ELSE ret.Title
                                                                                      END AS Title
                                                                                      ,CASE
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'e1ce52c0-058b-prod-afbd-1d2d24105ebc' THEN  'Product'
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'i1ce52c0-058b-issu-afbd-1d2d24105ebc' THEN  'Task'
                                                                                        WHEN ret.WbsTaxonomyLevelId = 'pouoe52c0-hvkhl-lbjm-jvhj-1d2d241ouyi' THEN  'Issue'
                                                                                        WHEN ret.WbsTaxonomyLevelId = '41ce52c0-058b-wbs-afbd-1d2d24105ebc' THEN  'Wbs'
                                                                                        ELSE null
                                                                                     END AS Type,
                                                                                     wt.Title AS Wbs
                                                                                    FROM ret
                                                                                    LEFT OUTER JOIN PbsProduct pp
                                                                                      ON ret.Id = pp.Id
                                                                                    LEFT OUTER JOIN WbsTaxonomy wt
                                                                                        ON ret.ParentId = wt.Id
                                                                                    WHERE ret.Id != @Id
                                                                                    OPTION (MAXRECURSION 1000)",
                                         new { Id = item.Id }).ToList();
                                 
                                     childPbs.ForEach(x => x.ProjectSequenceId = projects.SequenceCode);
                                     childPbs.ForEach(x => x.ProjectTitle = projects.Title);
                                     childPbs.ForEach(x => x.Cu = projects.Cu);
                                 
                                     data.Add(item);
                                     data.AddRange(childPbs);
                                 
                                 }
                                 catch (Exception e)
                                 {
                                     Console.WriteLine(projects.SequenceCode + " - " + item.Id);
                                     throw;
                                 }

                             }

                             
                        }
                        
                    }

                }
                
        
        return data;
    }
    
    public async Task<string> WbsTaskCreate(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsTask = connection
            .Query<WbsTask>("Select * From WbsTask Where Id = @Id", new { Id = wbsParameter.wbsTaskCreate.Id })
            .FirstOrDefault();
        

        if (wbsTask == null)
        {
            if (wbsParameter.wbsTaskCreate.Id == null)
            {
                wbsParameter.wbsTaskCreate.Id = Guid.NewGuid().ToString();

                if (wbsParameter.wbsTaskCreate.WbsTaxonomyId != null)
                {
                    var wbsTax = connection.Query<WbsTaxonomy>("Select * From WbsTaxonomy Where Id = @Id",
                        new { Id = wbsParameter.wbsTaskCreate.WbsTaxonomyId }).FirstOrDefault();
                    
                    var taxonomyParam = new WbsTaxonomy()
                    {
                        Id = wbsParameter.wbsTaskCreate.Id,
                        Title = wbsParameter.wbsTaskCreate.Name,
                        ParentId = wbsParameter.wbsTaskCreate.WbsTaxonomyId,
                        CreatedBy = wbsParameter.UserId,
                        CreatedDateTime = DateTime.UtcNow,
                        WbsTaxonomyLevelId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc",
                        TemplateId = wbsTax?.TemplateId
                    };

                    const string insertWbs =
                        @"INSERT INTO dbo.WbsTaxonomy ( Id ,WbsTaxonomyLevelId ,ParentId ,CreatedDateTime ,CreatedBy,SequenceId ,Title,IsDefault,TemplateId ) VALUES ( @Id ,@WbsTaxonomyLevelId ,@ParentId ,@CreatedDateTime ,@CreatedBy,@SequenceId ,@Title,@IsDefault,@TemplateId);";

                    await connection.ExecuteAsync(insertWbs, taxonomyParam);
                }
            }
            
            const string taskInsert = @"INSERT INTO dbo.WbsTask ( Id ,WbsTaxonomyId ,ProductId ,Name ,Note  ,Date,CreationDate,DeliveryStatusId,StatusId,CompletionDate,DisplayOrder,IsFavourite,IssueId ) VALUES ( @Id ,@WbsTaxonomyId ,@ProductId ,@Name ,@Note ,@Date,@CreationDate,@DeliveryStatusId,@StatusId,@CompletionDate,@DisplayOrder,@IsFavourite,@IssueId )";

            var maxDisplayOrder = connection.Query<int>(@"SELECT CASE
                                                                WHEN MAX(DisplayOrder) IS NULL THEN 0
                                                                ELSE MAX(DisplayOrder)
                                                                END AS count
                                                                FROM WbsProduct").FirstOrDefault();

            wbsParameter.wbsTaskCreate.StatusId ??= "0e1b34bc-f2c3-4778-8250-9666ee96ae59";//pending development
            wbsParameter.wbsTaskCreate.DeliveryStatusId ??= "i1ce52c0-058b-issu-afbd-1d2d24105ebc";//overdue

            var taskParam = new WbsTask
            {
                Id = wbsParameter.wbsTaskCreate.Id,
                WbsTaxonomyId = wbsParameter.wbsTaskCreate.WbsTaxonomyId,
                ProductId = wbsParameter.wbsTaskCreate.ProductId,
                Name = wbsParameter.wbsTaskCreate.Name,
                Note = wbsParameter.wbsTaskCreate.Note,
                Date = wbsParameter.wbsTaskCreate.Date,
                CreationDate = DateTime.UtcNow,
                StatusId = wbsParameter.wbsTaskCreate.StatusId,
                DeliveryStatusId = wbsParameter.wbsTaskCreate.DeliveryStatusId,
                CompletionDate = wbsParameter.wbsTaskCreate.CompletionDate,
                DisplayOrder = maxDisplayOrder + 1,
                IsFavourite = false,
                IssueId = wbsParameter.wbsTaskCreate.IssueId
            };

            await connection.ExecuteAsync(taskInsert, taskParam);

            const string ccInsert = @"INSERT INTO dbo.WbsTaskCc ( Id ,PersonId ,TaskId ) VALUES ( @Id ,@PersonId ,@TaskId )";

            foreach (var cc in wbsParameter.wbsTaskCreate.Cc)
            {
                cc.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = cc.EmailAddress }).FirstOrDefault();
                
                var ccParam = new WbsTaskCc
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = cc.PersonId,
                    TaskId = taskParam.Id
                };

                await connection.ExecuteAsync(ccInsert, ccParam);

            }
            
            const string toInsert = @"INSERT INTO dbo.WbsTaskTo ( Id ,PersonId ,TaskId ) VALUES ( @Id ,@PersonId ,@TaskId )";

            var increment = 0;
            foreach (var to in wbsParameter.wbsTaskCreate.ToPerson)
            {
                to.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = to.EmailAddress }).FirstOrDefault();
                
                var toParam = new WbsTaskTo
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = to.PersonId,
                    TaskId = taskParam.Id,
                    IsDefault = increment == 0

                };

                await connection.ExecuteAsync(toInsert, toParam);

                increment++;
            }

            const string tagsInsert =
                @"INSERT INTO dbo.WbsTaskTags ( Id ,Name ,TaskId ) VALUES ( @Id ,@Name ,@TaskId )";
            
            foreach (var tag in wbsParameter.wbsTaskCreate.Tags)
            {
                
                var tagParam = new WbsTaskTags
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = tag.Name,
                    TaskId = taskParam.Id
                };

                await connection.ExecuteAsync(tagsInsert, tagParam);

            }
            
            const string docsInsert =
                @"INSERT INTO dbo.WbsTaskDocuments ( Id ,Link ,TaskId,SharepointId ) VALUES ( @Id ,@Link ,@TaskId,@SharepointId )";
            
            foreach (var doc in wbsParameter.wbsTaskCreate.Documents)
            {
                
                var docParam = new WbsTaskDocuments
                {
                    Id = Guid.NewGuid().ToString(),
                    Link = doc.Link,
                    TaskId = taskParam.Id,
                    SharepointId = doc.SharepointId
                };

                await connection.ExecuteAsync(docsInsert, docParam);

            }
            
            const string instructionInsert =
                @"INSERT INTO dbo.WbsTaskInstruction ( Id ,InstructionId ,TaskId ) VALUES ( @Id ,@InstructionId ,@TaskId )";
            
            foreach (var ins in wbsParameter.wbsTaskCreate.Instructions.Where(x => x.IsChecked))
            {
                
                var insParam = new WbsTaskInstruction
                {
                    Id = Guid.NewGuid().ToString(),
                    InstructionId = ins.Id,
                    TaskId = taskParam.Id
                };

                await connection.ExecuteAsync(instructionInsert, insParam);

            }
            

            if (wbsParameter.wbsTaskCreate.Email != null)
            {
                const string emailInsert = @"INSERT INTO dbo.WbsTaskEmail ( Id ,EmailId ,TaskId ) VALUES ( @Id ,@EmailId ,@TaskId )";
                const string productEmailInsert = @"INSERT INTO dbo.WbsProductEmai ( Id ,EmailId ,WbsProductId ) VALUES ( @Id ,@EmailId ,@WbsProductId )";


                foreach (var email in wbsParameter.wbsTaskCreate.Email)
                {
                    var emailParam = new WbsTaskEmail
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmailId = email.EmailId,
                        TaskId = taskParam.Id
                    };
                    
                    await connection.ExecuteAsync(emailInsert, emailParam);


                    if (wbsParameter.wbsTaskCreate.WbsTaxonomyId != null)
                    {
                        var productEmailParam = new WbsProductEmai
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmailId = email.EmailId,
                            WbsProductId = wbsParameter.wbsTaskCreate.WbsTaxonomyId
                        };
                        await connection.ExecuteAsync(productEmailInsert, productEmailParam);

                    }

                }
            }
        }
        else
        {
            const string updateTask =
                @"UPDATE dbo.WbsTask SET WbsTaxonomyId = @WbsTaxonomyId ,ProductId = @ProductId ,Name = @Name ,Note = @Note ,Date = @Date ,DeliveryStatusId = @DeliveryStatusId ,StatusId = @StatusId ,CompletionDate = @CompletionDate WHERE Id = @Id";
            
            var taskParam = new WbsTask
            {
                Id = wbsParameter.wbsTaskCreate.Id,
                WbsTaxonomyId = wbsParameter.wbsTaskCreate.WbsTaxonomyId,
                ProductId = wbsParameter.wbsTaskCreate.ProductId,
                Name = wbsParameter.wbsTaskCreate.Name,
                Note = wbsParameter.wbsTaskCreate.Note,
                Date = wbsParameter.wbsTaskCreate.Date,
                StatusId = wbsParameter.wbsTaskCreate.StatusId,
                DeliveryStatusId = wbsParameter.wbsTaskCreate.DeliveryStatusId,
                CompletionDate =  wbsParameter.wbsTaskCreate.CompletionDate
            };

            await connection.ExecuteAsync(updateTask, taskParam);
            
            const string insertWbs =
                @"Update dbo.WbsTaxonomy Set ParentId = @WbsTaxonomyId ,Title = @Name Where Id = @Id";

            await connection.ExecuteAsync(insertWbs, taskParam);

            const string ccInsert = @"INSERT INTO dbo.WbsTaskCc ( Id ,PersonId ,TaskId ) VALUES ( @Id ,@PersonId ,@TaskId )";

            await connection.ExecuteAsync("Delete From WbsTaskCc Where TaskId = @TaskId ",
                new { TaskId = wbsParameter.wbsTaskCreate.Id });
            
            foreach (var cc in wbsParameter.wbsTaskCreate.Cc)
            {
                
                cc.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = cc.EmailAddress }).FirstOrDefault();
                
                var ccParam = new WbsTaskCc
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = cc.PersonId,
                    TaskId = taskParam.Id
                };
                
                await connection.ExecuteAsync(ccInsert, ccParam);


            }
            
            const string toInsert = @"INSERT INTO dbo.WbsTaskTo ( Id ,PersonId ,TaskId ) VALUES ( @Id ,@PersonId ,@TaskId )";

            await connection.ExecuteAsync("Delete From WbsTaskTo Where TaskId = @TaskId ",
                new { TaskId = wbsParameter.wbsTaskCreate.Id });

            var increment = 0;
            foreach (var to in wbsParameter.wbsTaskCreate.ToPerson)
            {
                
                to.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = to.EmailAddress }).FirstOrDefault();
                
                var toParam = new WbsTaskTo
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = to.PersonId,
                    TaskId = taskParam.Id,
                    IsDefault = increment == 0
                };

                await connection.ExecuteAsync(toInsert, toParam);

                increment++;
            }

            const string tagsInsert =
                @"INSERT INTO dbo.WbsTaskTags ( Id ,Name ,TaskId ) VALUES ( @Id ,@Name ,@TaskId )";
            
            await connection.ExecuteAsync("Delete From WbsTaskTags Where TaskId = @TaskId ",
                new { TaskId = wbsParameter.wbsTaskCreate.Id });
            
            foreach (var tag in wbsParameter.wbsTaskCreate.Tags)
            {
                
                var tagParam = new WbsTaskTags
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = tag.Name,
                    TaskId = taskParam.Id
                };

                await connection.ExecuteAsync(tagsInsert, tagParam);

            }
            
            const string docsInsert =
                @"INSERT INTO dbo.WbsTaskDocuments ( Id ,Link ,TaskId,SharepointId ) VALUES ( @Id ,@Link ,@TaskId,@SharepointId )";
            
            await connection.ExecuteAsync("Delete From WbsTaskDocuments Where TaskId = @TaskId ",
                new { TaskId = wbsParameter.wbsTaskCreate.Id });
            
            foreach (var doc in wbsParameter.wbsTaskCreate.Documents)
            {
                
                var docParam = new WbsTaskDocuments
                {
                    Id = Guid.NewGuid().ToString(),
                    Link = doc.Link,
                    TaskId = taskParam.Id,
                    SharepointId = doc.SharepointId
                };

                await connection.ExecuteAsync(docsInsert, docParam);

            }
            
            const string instructionInsert =
                @"INSERT INTO dbo.WbsTaskInstruction ( Id ,InstructionId ,TaskId ) VALUES ( @Id ,@InstructionId ,@TaskId )";
            
            await connection.ExecuteAsync("Delete From WbsTaskInstruction Where TaskId = @TaskId ",
                new { TaskId = wbsParameter.wbsTaskCreate.Id });
            
            foreach (var ins in wbsParameter.wbsTaskCreate.Instructions.Where(x => x.IsChecked))
            {
                
                var insParam = new WbsTaskInstruction
                {
                    Id = Guid.NewGuid().ToString(),
                    InstructionId = ins.Id,
                    TaskId = taskParam.Id
                };

                await connection.ExecuteAsync(instructionInsert, insParam);

            }
            
           
            // if (wbsParameter.wbsTaskCreate.Email != null)
            // {
            //
            //     await connection.ExecuteAsync("Delete From WbsTaskEmail Where TaskId = @TaskId ",
            //         new { TaskId = wbsParameter.wbsTaskCreate.Id });
            //
            //     const string emailInsert = @"INSERT INTO dbo.WbsTaskEmail ( Id ,EmailId ,TaskId ) VALUES ( @Id ,@EmailId ,@TaskId )";
            //
            //     foreach (var email in wbsParameter.wbsTaskCreate.Email)
            //     {
            //         var emailParam = new WbsTaskEmail
            //         {
            //             Id = Guid.NewGuid().ToString(),
            //             EmailId = email.EmailId,
            //             TaskId = taskParam.Id
            //         };
            //         await connection.ExecuteAsync(emailInsert, emailParam);
            //     }
            // }
        }

        return wbsParameter.wbsTaskCreate.Id;
    }

    public async Task<GetWbsTask> WbsTaskGetById(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);
        
        var data = connection.Query<GetWbsTask>("Select WbsTask.*,pp.Title AS PbsTitle,pp.Id As PbsId,wt.TemplateId,ih.Title AS IssueTitle FROM WbsTask LEFT OUTER JOIN PbsProduct pp ON WbsTask.ProductId = pp.ProductId LEFT OUTER JOIN WbsTaxonomy wt ON WbsTask.Id = wt.Id LEFT OUTER JOIN IssueHeader ih ON WbsTask.IssueId = ih.Id where WbsTask.Id = @Id", new { Id = wbsParameter.Id })
            .FirstOrDefault();
        
        if (data != null)
        {
            var persons = dbConnection.Query<CabDataDapperDto>("SELECT CabPerson.FullName,CabPerson.Id AS PersonId,ce.EmailAddress AS Email FROM CabPerson LEFT OUTER JOIN CabPersonCompany cpc ON CabPerson.Id = cpc.PersonId LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id");
            
            var CcList = connection.Query<WbsTaskCc>("Select * From WbsTaskCc Where TaskId = @TaskId",
                new { TaskId = wbsParameter.Id }).ToList();

            foreach (var item in CcList)
            {
                item.PersonName = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.FullName;
                item.EmailAddress = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.Email;
            }

            data.Cc = CcList;
            
            
            var toList = connection.Query<WbsTaskTo>("Select * From WbsTaskTo Where TaskId = @TaskId ORDER BY IsDefault DESC ",
                            new { TaskId = wbsParameter.Id }).ToList();
            
                        foreach (var item in toList)
                        {
                            item.PersonName = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.FullName;
                            item.EmailAddress = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.Email;

                        }
            
            data.ToPerson = toList;
            
            data.Documents = connection.Query<WbsTaskDocuments>("Select * From WbsTaskDocuments Where TaskId = @TaskId",
                new { TaskId = wbsParameter.Id }).ToList();
            
            data.Tags = connection.Query<WbsTaskTags>("Select * From WbsTaskTags Where TaskId = @TaskId",
                new { TaskId = wbsParameter.Id }).ToList();

            var instructionsList = connection.Query<TaskInstruction>(@"SELECT
                                                                                             CONCAT(Instructions.SequenceCode,' - ',Instructions.Name) AS Title
                                                                                            ,PbsInstruction.Id
                                                                                            FROM dbo.PbsInstruction
                                                                                            LEFT OUTER JOIN dbo.Instructions
                                                                                              ON PbsInstruction.InstructionsId = Instructions.Id
                                                                                            WHERE PbsInstruction.PbsProductId = (Select Id From PbsProduct Where ProductId =  @PbsProductId )
                                                                                              AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false'",new{PbsProductId = data.ProductId}).ToList();

            var taskInstructions = connection
                .Query<string>("Select InstructionId From WbsTaskInstruction Where TaskId = @TaskId",
                    new { TaskId = data.Id }).ToList();
            
             instructionsList.Where(x => taskInstructions.Contains(x.Id)).ToList().ForEach(c => c.IsChecked = true);
            
            data.Instructions = instructionsList;

            data.TimeLine = new TaskTimeLine()
            {
                CreationDate = data.CreationDate
            };

            data.Email = connection
                .Query<WbsTaskEmail>("Select * From WbsTaskEmail Where TaskId = @TaskId", new { TaskId = data.Id })
                .ToList();
            
            wbsParameter.Id = data.Id;
            data.Conversations = await GetWbsConversation(wbsParameter);
                
            data.Cu = wbsParameter.ContractingUnitSequenceId;
            data.ProjectSequenceId = wbsParameter.ProjectSequenceId;

            var wbsQuery = @"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM WbsTaxonomy
                                                                            WHERE Id = @Id
                                                                            
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM WbsTaxonomy t
                                                                            INNER JOIN ret r
                                                                            ON r.ParentId = t.Id
                                                                            )
                                                                            SELECT
                                                                             *
                                                                            FROM ret WHERE ret.Id != @Id AND WbsTaxonomyLevelId = '41ce52c0-058b-wbs-afbd-1d2d24105ebc'";

            data.Wbs = connection.Query<WbsTaxonomy>(wbsQuery, new { Id = data.Id }).FirstOrDefault()?.Title;
            data.ProjectTitle = dbConnection
                .Query<string>("SELECT Title FROM ProjectDefinition WHERE SequenceCode = @SequenceCode ",
                    new { SequenceCode = wbsParameter.ProjectSequenceId }).FirstOrDefault();
        }

        return data;
    }

    public async Task<string> WbsTaskDelete(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);


        await connection.ExecuteAsync("Delete From WbsTask Where Id = @Id", new { Id = wbsParameter.Id });

        return wbsParameter.Id;
    }
    
    public async Task<List<WbsTaskFilterResultsDto>> WbsTaskGroupFilter(WbsParameter wbsParameter)
    {
       // wbsParameter.ContractingUnitSequenceId ??= "COM-0001";
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);


        var results = new List<WbsTaskFilterResultsDto>();
        var filteredResults = new List<WbsTaskFilterResults>();

        var query = "Select WbsTask.*,WbsTask.WbsTaxonomyId As ParentId,WbsTaskTags.Name As Tag From WbsTask Left Outer Join WbsTaskTags ON WbsTask.Id = WbsTaskTags.TaskId Where WbsTask.Id IS NOT NULL";

        var sb = new StringBuilder(query);
        if (wbsParameter.wbsTaskFilterDto.Date != null)
        {
            sb.Append(" AND WbsTask.Date = '" + wbsParameter.wbsTaskFilterDto.Date + "'");
        }
        if (wbsParameter.wbsTaskFilterDto.StartDate != null && wbsParameter.wbsTaskFilterDto.EndDate != null)
        {
            sb.Append(" AND WbsTask.Date BETWEEN '" + wbsParameter.wbsTaskFilterDto.StartDate + "' AND '" + wbsParameter.wbsTaskFilterDto.EndDate +"'");
        }
        if (wbsParameter.wbsTaskFilterDto.Name != null)
        {
            sb.Append(" AND WbsTask.Name Like '%"+ wbsParameter.wbsTaskFilterDto.Name + "%'");
        }
        if (wbsParameter.wbsTaskFilterDto.StatusId != null)
        {
            sb.Append(" AND StatusId = '" + wbsParameter.wbsTaskFilterDto.StatusId + "'");
        }
        
        if (wbsParameter.wbsTaskFilterDto.IsFavourite != null && wbsParameter.wbsTaskFilterDto.IsFavourite.Value)
        {
            
            sb.Append(" AND IsFavourite = '" + wbsParameter.wbsTaskFilterDto.IsFavourite + "'");
        }
        if (wbsParameter.wbsTaskFilterDto.Tag != null)
        {
            sb.Append("  AND WbsTaskTags.Name Like '%"+ wbsParameter.wbsTaskFilterDto.Tag + "%'");
        }
        if (wbsParameter.wbsTaskFilterDto.ShownBy != null)
        {
            switch (wbsParameter.wbsTaskFilterDto.ShownBy)
            {
                case "openTask":
                    sb.Append(" AND StatusId != 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                    break;
                case "completedTask":
                    sb.Append(" AND StatusId = 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                    break;
            }
        }

        var data = connection.Query<WbsTaskFilterResults>(sb.ToString()).ToList();
        var taskDocs = connection.Query<WbsTaskDocuments>("Select * From WbsTaskDocuments").ToList();
        var productEmails = connection.Query<WbsProductEmai>("Select * From WbsProductEmai").ToList();
        var persons = dbConnection.Query<CabPerson>("Select * From CabPerson");
        var project = dbConnection.Query<ProjectDefinitionMobDto>("Select SequenceCode,Title From ProjectDefinition");


        if (wbsParameter.wbsTaskFilterDto.GroupItem == "Date")
        {
            var groupData = data.DistinctBy(x => x.Id).GroupBy(x => x.Date?.Date).OrderByDescending(c => c.Key);

            
            foreach (var item in groupData)
            {
                foreach (var listItem in item)
                {
                    listItem.TotalAttachment = taskDocs.Count(x => x.TaskId == listItem.Id);
                    listItem.TotalMail = productEmails.Count(x => x.WbsProductId == listItem.ParentId);

                    wbsParameter.getWbsTaskInstructionList = new GetWbsTaskInstructionList()
                    {
                        TaskId = listItem.Id,
                        ProductId = listItem.ProductId
                    };
                    var instructionList = await GetWbsTaskInstructionList(wbsParameter);
                    listItem.TotalInstructions = instructionList.Count;
                    listItem.TotalCheckedInstructions = instructionList.Count(x => x.IsChecked);
                    
                    var toList = connection.Query<WbsTaskTo>("Select * From WbsTaskTo Where TaskId = @TaskId AND PersonId IS NOT NULL",
                        new { TaskId = listItem.Id }).ToList();

                    foreach (var to in toList)
                    {
                        to.PersonName = persons.FirstOrDefault(x => x.Id == to.PersonId)?.FullName;
                    }

                    listItem.ToPerson = toList ?? new List<WbsTaskTo>();
                    
                    
                    var ccList = connection.Query<WbsTaskCc>("Select * From WbsTaskCc Where TaskId = @TaskId AND PersonId IS NOT NULL",
                        new { TaskId = listItem.Id }).ToList();
        
                    foreach (var cc in ccList)
                    {
                        cc.PersonName = persons.FirstOrDefault(x => x.Id == cc.PersonId)?.FullName;
                    }
        
                    listItem.Cc = ccList ?? new List<WbsTaskCc>();

                    // listItem.To = toList.FirstOrDefault()?.PersonId;
                    // listItem.PersonName = toList.FirstOrDefault()?.PersonName;
                    listItem.ProjectSequenceId = wbsParameter.ProjectSequenceId;
                    listItem.Cu = wbsParameter.ContractingUnitSequenceId;
                    listItem.ProjectTitle = project
                        .FirstOrDefault(x => x.SequenceCode == wbsParameter.ProjectSequenceId).Title;
                    
                    if (listItem.Date?.Date == DateTime.UtcNow.Date)
                    {
                        listItem.DeliveryStatusId = "jdjj52c0-058b-issu-afbd-1d2d24105ebc";
                    }
                    if (listItem.Date?.Date < DateTime.UtcNow.Date)
                    {
                        listItem.DeliveryStatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc";
                    }
                    if (listItem.Date?.Date > DateTime.UtcNow.Date && listItem.Date?.Date <= DateTime.UtcNow.AddDays(7).Date )
                    {
                        listItem.DeliveryStatusId = "2210e768-msms-po02-Lot5-ee367a82ad22";
                    }
                    if (listItem.Date?.Date > DateTime.UtcNow.AddDays(7).Date )
                    {
                        listItem.DeliveryStatusId = "12a22319-8ca7-temp-b588-6fab99474130";
                    }
                }
                
                filteredResults = wbsParameter.wbsTaskFilterDto.DeliveryStatusId != null ? item.Where(x => x.DeliveryStatusId == wbsParameter.wbsTaskFilterDto.DeliveryStatusId).ToList() : item.ToList();

                if (wbsParameter.wbsTaskFilterDto.IsMyTask != null)
                {
                    if (wbsParameter.wbsTaskFilterDto.IsMyTask.Value)
                    {
                        var personId = dbConnection.Query<CabPersonCompany>(@"Select * From CabPersonCompany Where Oid = @Oid",new{Oid = wbsParameter.UserId}).FirstOrDefault().PersonId;

                        filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => v.PersonId.Contains(personId))).ToList();
                    }
                    else
                    {
                        filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                    }

                }

                if (wbsParameter.wbsTaskFilterDto.IsCc != null)
                {
                    if (wbsParameter.wbsTaskFilterDto.IsCc.Value)
                    {

                        filteredResults = filteredResults.Where(x => x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                    }
                }
                
                if (wbsParameter.wbsTaskFilterDto.IsTo != null)
                {
                    if (wbsParameter.wbsTaskFilterDto.IsTo.Value)
                    {

                        filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                    }
                }
                
                // if (wbsParameter.wbsTaskFilterDto.IsTo != null && wbsParameter.wbsTaskFilterDto.IsCc != null)
                // {
                //     if (wbsParameter.wbsTaskFilterDto.IsTo.Value && wbsParameter.wbsTaskFilterDto.IsCc.Value)
                //     {
                //
                //         filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();
                //
                //     }
                // }

                var obj = new WbsTaskFilterResultsDto()
                {
                    GroupItem = item.Key?.ToString("yyyy-MM-dd"),
                    FilterList = filteredResults
                };

                if (obj.FilterList.Any())
                {
                    if (wbsParameter.wbsTaskFilterDto.Sorter?.Attribute != null)
                    {
                        obj.FilterList = wbsParameter.wbsTaskFilterDto.Sorter?.Attribute.ToLower() switch
                        {
                            "isfavourite" => obj.FilterList.OrderByDescending(x => x.IsFavourite).ToList(),
                            "creationdate" => obj.FilterList.OrderByDescending(x => x.CreationDate).ToList(),
                            "date" => obj.FilterList.OrderByDescending(x => x.Date).ToList(),
                            _ => obj.FilterList
                        };
                    }
                    results.Add(obj);
                }

            }
        }

        if (wbsParameter.wbsTaskFilterDto.GroupItem == "Tags")
        {
            var groupData = data.GroupBy(x => x.Tag).OrderByDescending(c => c.Key);
            
            foreach (var item in groupData)
            {
                foreach (var listItem in item)
                {
                    listItem.TotalAttachment = taskDocs.Count(x => x.TaskId == listItem.Id);
                    listItem.TotalMail = productEmails.Count(x => x.WbsProductId == listItem.ParentId);
                    
                    wbsParameter.getWbsTaskInstructionList = new GetWbsTaskInstructionList()
                    {
                        TaskId = listItem.Id,
                        ProductId = listItem.ProductId
                    };
                    var instructionList = await GetWbsTaskInstructionList(wbsParameter);
                    listItem.TotalInstructions = instructionList.Count;
                    listItem.TotalCheckedInstructions = instructionList.Count(x => x.IsChecked);
                    
                    var toList = connection.Query<WbsTaskTo>("Select * From WbsTaskTo Where TaskId = @TaskId AND PersonId IS NOT NULL",
                        new { TaskId = listItem.Id }).ToList();

                    foreach (var to in toList)
                    {
                        to.PersonName = persons.FirstOrDefault(x => x.Id == to.PersonId)?.FullName;
                    }

                    listItem.ToPerson = toList ?? new List<WbsTaskTo>();
                    
                    var ccList = connection.Query<WbsTaskCc>("Select * From WbsTaskCc Where TaskId = @TaskId AND PersonId IS NOT NULL",
                        new { TaskId = listItem.Id }).ToList();
        
                    foreach (var cc in ccList)
                    {
                        cc.PersonName = persons.FirstOrDefault(x => x.Id == cc.PersonId)?.FullName;
                    }
        
                    listItem.Cc = ccList ?? new List<WbsTaskCc>();
                    // listItem.To = toList.FirstOrDefault()?.PersonId;
                    // listItem.PersonName = toList.FirstOrDefault()?.PersonName;
 
                    if (listItem.Date?.Date == DateTime.UtcNow.Date)
                    {
                        listItem.DeliveryStatusId = "jdjj52c0-058b-issu-afbd-1d2d24105ebc";
                    }
                    if (listItem.Date?.Date < DateTime.UtcNow.Date)
                    {
                        listItem.DeliveryStatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc";
                    }
                    if (listItem.Date?.Date > DateTime.UtcNow.Date && listItem.Date?.Date <= DateTime.UtcNow.AddDays(7).Date )
                    {
                        listItem.DeliveryStatusId = "2210e768-msms-po02-Lot5-ee367a82ad22";
                    }
                    if (listItem.Date?.Date > DateTime.UtcNow.AddDays(7).Date )
                    {
                        listItem.DeliveryStatusId = "12a22319-8ca7-temp-b588-6fab99474130";
                    }
                    listItem.ProjectSequenceId = wbsParameter.ProjectSequenceId;
                    listItem.Cu = wbsParameter.ContractingUnitSequenceId;
                    listItem.ProjectTitle = project
                        .FirstOrDefault(x => x.SequenceCode == wbsParameter.ProjectSequenceId)?.Title;
                }
                
                filteredResults = wbsParameter.wbsTaskFilterDto.DeliveryStatusId != null ? item.Where(x => x.DeliveryStatusId == wbsParameter.wbsTaskFilterDto.DeliveryStatusId).ToList() : item.ToList();

                if (wbsParameter.wbsTaskFilterDto.IsMyTask != null)
                {
                    if (wbsParameter.wbsTaskFilterDto.IsMyTask.Value)
                    {
                        var personId = dbConnection.Query<CabPersonCompany>(@"Select * From CabPersonCompany Where Oid = @Oid",new{Oid = wbsParameter.UserId}).FirstOrDefault().PersonId;

                        filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => v.PersonId.Contains(personId))).ToList();
                    }
                    else
                    {
                        filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                    }

                }
                
                if (wbsParameter.wbsTaskFilterDto.IsCc != null)
                {
                    if (wbsParameter.wbsTaskFilterDto.IsCc.Value)
                    {

                        filteredResults = filteredResults.Where(x => x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                    }
                }
                
                if (wbsParameter.wbsTaskFilterDto.IsTo != null)
                {
                    if (wbsParameter.wbsTaskFilterDto.IsTo.Value)
                    {

                        filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                    }
                }
                
                // if (wbsParameter.wbsTaskFilterDto.IsTo != null && wbsParameter.wbsTaskFilterDto.IsCc != null)
                // {
                //     if (wbsParameter.wbsTaskFilterDto.IsTo.Value && wbsParameter.wbsTaskFilterDto.IsCc.Value)
                //     {
                //
                //         filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();
                //
                //     }
                // }
                var obj = new WbsTaskFilterResultsDto()
                {
                    GroupItem = item.Key,
                    FilterList = filteredResults
                };

                if (obj.FilterList.Any())
                {
                    if (wbsParameter.wbsTaskFilterDto.Sorter?.Attribute != null)
                    {
                        obj.FilterList = wbsParameter.wbsTaskFilterDto.Sorter?.Attribute.ToLower() switch
                        {
                            "isfavourite" => obj.FilterList.OrderByDescending(x => x.IsFavourite).ToList(),
                            "creationdate" => obj.FilterList.OrderByDescending(x => x.CreationDate).ToList(),
                            "date" => obj.FilterList.OrderByDescending(x => x.Date).ToList(),
                            _ => obj.FilterList
                        };
                    }
                    results.Add(obj);
                }

            }
        }

        return results;
    }
    
    public async Task<List<WbsTaskFilterResults>> WbsTaskFilter(WbsParameter wbsParameter)
    {
        //wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        if (wbsParameter.ProjectSequenceId == null && wbsParameter.ContractingUnitSequenceId == null)
        {
            return await WbsAllProjectsTaskFilter(wbsParameter);
        }
        
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);


        var query = "Select WbsTask.*,WbsTaxonomyId As ParentId,WbsTaskTags.Name As Tag From WbsTask Left Outer Join WbsTaskTags ON WbsTask.Id = WbsTaskTags.TaskId Where WbsTask.Id IS NOT NULL";

        var sb = new StringBuilder(query);

        // if (wbsParameter.IsCheckList)
        // {
        if (wbsParameter.IdList != null)
        {
            if (wbsParameter.IdList.Any())
            {
                sb.Append(" AND WbsTask.Id IN @Ids ");
            }
        }
        // }
        
        if (wbsParameter.ProductGetById != null)
        {
            sb.Append(" AND WbsTask.ProductId = '" + wbsParameter.ProductGetById + "'");
        }
        if (wbsParameter.IssueGetById != null)
        {
            sb.Append(" AND WbsTask.IssueId = '" + wbsParameter.IssueGetById + "'");
        }
        

        if (wbsParameter.wbsTaskFilterDto.Date != null)
        {
            sb.Append(" AND WbsTask.Date = '" + wbsParameter.wbsTaskFilterDto.Date + "'");
        }
        if (wbsParameter.wbsTaskFilterDto.StartDate != null && wbsParameter.wbsTaskFilterDto.EndDate != null)
        {
            sb.Append(" AND WbsTask.Date BETWEEN '" + wbsParameter.wbsTaskFilterDto.StartDate + "' AND '" + wbsParameter.wbsTaskFilterDto.EndDate +"'");
        }
        if (wbsParameter.wbsTaskFilterDto.Name != null)
        {
            sb.Append(" AND WbsTask.Name Like '%"+ wbsParameter.wbsTaskFilterDto.Name + "%'");
        }
        if (wbsParameter.wbsTaskFilterDto.StatusId != null)
        {
            sb.Append(" AND StatusId = '" + wbsParameter.wbsTaskFilterDto.StatusId + "'");
        }
        
        if (wbsParameter.wbsTaskFilterDto.IsFavourite != null && wbsParameter.wbsTaskFilterDto.IsFavourite.Value)
        {
            
            sb.Append(" AND IsFavourite = '" + wbsParameter.wbsTaskFilterDto.IsFavourite + "'");
        }
        if (wbsParameter.wbsTaskFilterDto.Tag != null)
        {
            sb.Append("  AND WbsTaskTags.Name Like '%"+ wbsParameter.wbsTaskFilterDto.Tag + "%'");
        }
        if (wbsParameter.wbsTaskFilterDto.ShownBy != null)
        {
            switch (wbsParameter.wbsTaskFilterDto.ShownBy)
            {
                case "openTask":
                    sb.Append(" AND StatusId != 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                    break;
                case "completedTask":
                    sb.Append(" AND StatusId = 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                    break;
            }
        }

        if (wbsParameter.wbsTaskFilterDto.Sorter?.Attribute != null)
        {
            sb.Append(" Order By " + wbsParameter.wbsTaskFilterDto.Sorter.Attribute + " Desc");
        }
        else
        {
            sb.Append(" Order By WbsTask.Name asc" );
        }
        var data =  connection.Query<WbsTaskFilterResults>(sb.ToString(),new{Ids = wbsParameter.IdList}).ToList().DistinctBy(x => x.Id).ToList();
        var taskDocs = connection.Query<WbsTaskDocuments>("Select * From WbsTaskDocuments").ToList();
        var productEmails = connection.Query<WbsProductEmai>("Select * From WbsProductEmai").ToList();
        var persons = dbConnection.Query<CabPerson>("Select * From CabPerson");
        var project = dbConnection.Query<ProjectDefinitionMobDto>("Select SequenceCode,Title From ProjectDefinition");

        
        foreach (var listItem in data)
        {
           
                listItem.TotalAttachment = taskDocs.Count(x => x.TaskId == listItem.Id);
                listItem.TotalMail = productEmails.Count(x => x.WbsProductId == listItem.ParentId);
        
                wbsParameter.getWbsTaskInstructionList = new GetWbsTaskInstructionList()
                {
                    TaskId = listItem.Id,
                    ProductId = listItem.ProductId
                };
                var instructionList = await GetWbsTaskInstructionList(wbsParameter);
                listItem.TotalInstructions = instructionList.Count;
                listItem.TotalCheckedInstructions = instructionList.Count(x => x.IsChecked);
                
                var toList = connection.Query<WbsTaskTo>("Select * From WbsTaskTo Where TaskId = @TaskId AND PersonId IS NOT NULL",
                    new { TaskId = listItem.Id }).ToList();
        
                foreach (var item in toList)
                {
                    item.PersonName = persons.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
                }
        
                listItem.ToPerson = toList;
                
                var ccList = connection.Query<WbsTaskCc>("Select * From WbsTaskCc Where TaskId = @TaskId AND PersonId IS NOT NULL",
                    new { TaskId = listItem.Id }).ToList();
        
                foreach (var item in ccList)
                {
                    item.PersonName = persons.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
                }
        
                listItem.Cc = ccList;
                // listItem.To = toList.FirstOrDefault()?.PersonId;
                // listItem.PersonName = toList.FirstOrDefault()?.PersonName;
                listItem.ProjectSequenceId = wbsParameter.ProjectSequenceId;
                listItem.Cu = wbsParameter.ContractingUnitSequenceId;
                listItem.ProjectTitle = project
                    .FirstOrDefault(x => x.SequenceCode == wbsParameter.ProjectSequenceId)?.Title;
        
                if (listItem.Date?.Date == DateTime.UtcNow.Date)
                {
                    listItem.DeliveryStatusId = "jdjj52c0-058b-issu-afbd-1d2d24105ebc";
                }
                if (listItem.Date?.Date < DateTime.UtcNow.Date)
                {
                    listItem.DeliveryStatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc";
                }
                if (listItem.Date?.Date > DateTime.UtcNow.Date && listItem.Date?.Date <= DateTime.UtcNow.AddDays(7).Date )
                {
                    listItem.DeliveryStatusId = "2210e768-msms-po02-Lot5-ee367a82ad22";
                }
                if (listItem.Date?.Date > DateTime.UtcNow.AddDays(7).Date )
                {
                    listItem.DeliveryStatusId = "12a22319-8ca7-temp-b588-6fab99474130";
                }
        
        }
        
        if (wbsParameter.wbsTaskFilterDto.DeliveryStatusId != null)
        {
            data = data.Where(x => x.DeliveryStatusId == wbsParameter.wbsTaskFilterDto.DeliveryStatusId).ToList();
        }

        if (wbsParameter.wbsTaskFilterDto.IsMyTask != null)
        {
            if (wbsParameter.wbsTaskFilterDto.IsMyTask.Value)
            {
                var personId = dbConnection.Query<CabPersonCompany>(@"Select * From CabPersonCompany Where Oid = @Oid",new{Oid = wbsParameter.UserId}).FirstOrDefault().PersonId;

                data = data.Where(x => x.ToPerson.Any(v => v.PersonId.Contains(personId))).ToList();
            }
            else
            {
                data = data.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

            }

        }
        
        if (wbsParameter.wbsTaskFilterDto.IsCc != null)
        {
            if (wbsParameter.wbsTaskFilterDto.IsCc.Value)
            {

                data = data.Where(x => x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

            }
        }
                
        if (wbsParameter.wbsTaskFilterDto.IsTo != null)
        {
            if (wbsParameter.wbsTaskFilterDto.IsTo.Value)
            {

                data = data.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

            }
        }
                
        // if (wbsParameter.wbsTaskFilterDto.IsTo != null && wbsParameter.wbsTaskFilterDto.IsCc != null)
        // {
        //     if (wbsParameter.wbsTaskFilterDto.IsTo.Value && wbsParameter.wbsTaskFilterDto.IsCc.Value)
        //     {
        //
        //         data = data.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();
        //
        //     }
        // }
        
        return data;
    }
    
    public async Task<List<WbsTaskFilterResults>> WbsAllProjectsTaskFilter(WbsParameter wbsParameter)
    {
        //wbsParameter.ContractingUnitSequenceId ??= "COM-0001";
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var sql = @"SELECT DISTINCT
                      ProjectDefinition.SequenceCode AS SequenceCode
                     ,ProjectDefinition.Name AS Name
                     ,ProjectDefinition.Id
                     ,ProjectDefinition.Title AS Title
                     ,ProjectDefinition.ProjectConnectionString
                     ,CabCompany.SequenceCode As Cu
                    FROM dbo.ProjectDefinition
                    LEFT OUTER JOIN dbo.ProjectUserRole
                      ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                    LEFT OUTER JOIN dbo.UserRole
                      ON ProjectUserRole.UsrRoleId = UserRole.Id
                    LEFT OUTER JOIN dbo.ApplicationUser
                      ON UserRole.ApplicationUserOid = ApplicationUser.OId
                    LEFT OUTER JOIN dbo.CabCompany
                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                    WHERE ApplicationUser.OId = @oid 
                    AND ProjectDefinition.IsDeleted = 0 ";

        var param = new
            { lang = wbsParameter.Lang, oid = wbsParameter.UserId };

        var userProjects = dbConnection.Query<UserProjectList>(sql, param);

        
        var query = "Select WbsTask.*,WbsTaxonomyId As ParentId,WbsTaskTags.Name As Tag From WbsTask Left Outer Join WbsTaskTags ON WbsTask.Id = WbsTaskTags.TaskId Where WbsTask.Id IS NOT NULL";

        var sb = new StringBuilder(query);

        // if (wbsParameter.IsCheckList)
        // {
        //     if (wbsParameter.IdList != null)
        //     {
        //         if (wbsParameter.IdList.Any())
        //         {
        //             sb.Append(" AND WbsTask.Id IN @Ids ");
        //         }
        //     }
        // }

        if (wbsParameter.wbsTaskFilterDto.Date != null)
        {
            sb.Append(" AND WbsTask.Date = '" + wbsParameter.wbsTaskFilterDto.Date + "'");
        }
        if (wbsParameter.wbsTaskFilterDto.StartDate != null && wbsParameter.wbsTaskFilterDto.EndDate != null)
        {
            sb.Append(" AND WbsTask.Date BETWEEN '" + wbsParameter.wbsTaskFilterDto.StartDate + "' AND '" + wbsParameter.wbsTaskFilterDto.EndDate +"'");
        }
        if (wbsParameter.wbsTaskFilterDto.Name != null)
        {
            sb.Append(" AND WbsTask.Name Like '%"+ wbsParameter.wbsTaskFilterDto.Name + "%'");
        }
        if (wbsParameter.wbsTaskFilterDto.StatusId != null)
        {
            sb.Append(" AND StatusId = '" + wbsParameter.wbsTaskFilterDto.StatusId + "'");
        }
        
        if (wbsParameter.wbsTaskFilterDto.IsFavourite != null && wbsParameter.wbsTaskFilterDto.IsFavourite.Value)
        {
            
            sb.Append(" AND IsFavourite = '" + wbsParameter.wbsTaskFilterDto.IsFavourite + "'");
        }
        if (wbsParameter.wbsTaskFilterDto.Tag != null)
        {
            sb.Append("  AND WbsTaskTags.Name Like '%"+ wbsParameter.wbsTaskFilterDto.Tag + "%'");
        }
        if (wbsParameter.wbsTaskFilterDto.ShownBy != null)
        {
            switch (wbsParameter.wbsTaskFilterDto.ShownBy)
            {
                case "openTask":
                    sb.Append(" AND StatusId != 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                    break;
                case "completedTask":
                    sb.Append(" AND StatusId = 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                    break;
            }
        }

        if (wbsParameter.wbsTaskFilterDto.Sorter?.Attribute != null)
        {
            sb.Append(" Order By " + wbsParameter.wbsTaskFilterDto.Sorter.Attribute + " Desc");
        }
        else
        {
            sb.Append(" Order By Date Desc" );
        }

        var persons = dbConnection.Query<CabPerson>("Select * From CabPerson");

        var results = new List<WbsTaskFilterResults>();
        
        foreach (var projects in userProjects)
        {
            await using var connection = new SqlConnection(projects.ProjectConnectionString);

            var data = connection.Query<WbsTaskFilterResults>(sb.ToString()).ToList()
                .DistinctBy(x => x.Id).ToList();
            var taskDocs = connection.Query<WbsTaskDocuments>("Select * From WbsTaskDocuments").ToList();
            var productEmails = connection.Query<WbsProductEmai>("Select * From WbsProductEmai").ToList();


            foreach (var listItem in data)
            {
                listItem.TotalAttachment = taskDocs.Count(x => x.TaskId == listItem.Id);
                listItem.TotalMail = productEmails.Count(x => x.WbsProductId == listItem.ParentId);

                wbsParameter.getWbsTaskInstructionList = new GetWbsTaskInstructionList()
                {
                    TaskId = listItem.Id,
                    ProductId = listItem.ProductId
                };
                var instructionList = await GetWbsTaskInstructionList(wbsParameter);
                listItem.TotalInstructions = instructionList.Count;
                listItem.TotalCheckedInstructions = instructionList.Count(x => x.IsChecked);

                var toList = connection.Query<WbsTaskTo>(
                    "Select * From WbsTaskTo Where TaskId = @TaskId AND PersonId IS NOT NULL",
                    new { TaskId = listItem.Id }).ToList();

                foreach (var item in toList)
                {
                    item.PersonName = persons.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
                }

                listItem.ToPerson = toList;

                var ccList = connection.Query<WbsTaskCc>(
                    "Select * From WbsTaskCc Where TaskId = @TaskId AND PersonId IS NOT NULL",
                    new { TaskId = listItem.Id }).ToList();

                foreach (var item in ccList)
                {
                    item.PersonName = persons.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
                }

                listItem.Cc = ccList;
                // listItem.To = toList.FirstOrDefault()?.PersonId;
                // listItem.PersonName = toList.FirstOrDefault()?.PersonName;
                listItem.ProjectSequenceId = projects.SequenceCode;
                listItem.Cu = projects.Cu;
                listItem.ProjectTitle = projects.Title;

                if (listItem.Date?.Date == DateTime.UtcNow.Date)
                {
                    listItem.DeliveryStatusId = "jdjj52c0-058b-issu-afbd-1d2d24105ebc";
                }

                if (listItem.Date?.Date < DateTime.UtcNow.Date)
                {
                    listItem.DeliveryStatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc";
                }

                if (listItem.Date?.Date > DateTime.UtcNow.Date &&
                    listItem.Date?.Date <= DateTime.UtcNow.AddDays(7).Date)
                {
                    listItem.DeliveryStatusId = "2210e768-msms-po02-Lot5-ee367a82ad22";
                }

                if (listItem.Date?.Date > DateTime.UtcNow.AddDays(7).Date)
                {
                    listItem.DeliveryStatusId = "12a22319-8ca7-temp-b588-6fab99474130";
                }

            }

            if (wbsParameter.wbsTaskFilterDto.DeliveryStatusId != null)
            {
                data = data.Where(x => x.DeliveryStatusId == wbsParameter.wbsTaskFilterDto.DeliveryStatusId).ToList();
            }

            if (wbsParameter.wbsTaskFilterDto.IsMyTask != null)
            {
                if (wbsParameter.wbsTaskFilterDto.IsMyTask.Value)
                {
                    var personId = dbConnection
                        .Query<CabPersonCompany>(@"Select * From CabPersonCompany Where Oid = @Oid",
                            new { Oid = wbsParameter.UserId }).FirstOrDefault().PersonId;

                    data = data.Where(x => x.ToPerson.Any(v => v.PersonId.Contains(personId))).ToList();
                }
                else
                {
                    data = data.Where(x =>
                            x.ToPerson.Any(v =>
                                wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) ||
                            x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))))
                        .ToList();

                }

            }

            if (wbsParameter.wbsTaskFilterDto.IsCc != null)
            {
                if (wbsParameter.wbsTaskFilterDto.IsCc.Value)
                {

                    data = data.Where(x =>
                            x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))))
                        .ToList();

                }
            }

            if (wbsParameter.wbsTaskFilterDto.IsTo != null)
            {
                if (wbsParameter.wbsTaskFilterDto.IsTo.Value)
                {

                    data = data.Where(x =>
                            x.ToPerson.Any(v =>
                                wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))))
                        .ToList();

                }
            }
            
            results.AddRange(data);
        }

        // if (wbsParameter.wbsTaskFilterDto.IsTo != null && wbsParameter.wbsTaskFilterDto.IsCc != null)
        // {
        //     if (wbsParameter.wbsTaskFilterDto.IsTo.Value && wbsParameter.wbsTaskFilterDto.IsCc.Value)
        //     {
        //
        //         data = data.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();
        //
        //     }
        // }
        
        return results;
    }
    
    public async Task<List<WbsTaskFilterResultsDto>> WbsTaskAllProjectsGroupFilter(WbsParameter wbsParameter)
    {
        //wbsParameter.ContractingUnitSequenceId ??= "COM-0001";
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);
        
        var results = new List<WbsTaskFilterResultsDto>();
        var allProjectsFilteredResults = new List<WbsTaskFilterResults>();

        var personCompany = dbConnection.Query<CabPersonCompany>(@"Select * From CabPersonCompany");
        
        var sql = @"SELECT DISTINCT
                      ProjectDefinition.SequenceCode AS SequenceCode
                     ,ProjectDefinition.Name AS Name
                     ,ProjectDefinition.Id
                     ,ProjectDefinition.Title AS Title
                     ,ProjectDefinition.ProjectConnectionString
                     ,CabCompany.SequenceCode As Cu
                    FROM dbo.ProjectDefinition
                    LEFT OUTER JOIN dbo.ProjectUserRole
                      ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                    LEFT OUTER JOIN dbo.UserRole
                      ON ProjectUserRole.UsrRoleId = UserRole.Id
                    LEFT OUTER JOIN dbo.ApplicationUser
                      ON UserRole.ApplicationUserOid = ApplicationUser.OId
                    LEFT OUTER JOIN dbo.CabCompany
                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                    WHERE ApplicationUser.OId = @oid 
                    AND ProjectDefinition.IsDeleted = 0 ";

        var param = new
            { lang = wbsParameter.Lang, oid = wbsParameter.UserId };

        var userProjects = dbConnection.Query<UserProjectList>(sql, param);

        foreach (var projects in userProjects)
        {
            var filteredResults = new List<WbsTaskFilterResults>();

            await using var connection = new SqlConnection(projects.ProjectConnectionString);

            var query =
                "Select WbsTask.*,WbsTask.WbsTaxonomyId As ParentId,WbsTaskTags.Name As Tag From WbsTask Left Outer Join WbsTaskTags ON WbsTask.Id = WbsTaskTags.TaskId Where WbsTask.Id IS NOT NULL";

            var sb = new StringBuilder(query);
            if (wbsParameter.wbsTaskFilterDto.Date != null)
            {
                sb.Append(" AND WbsTask.Date = '" + wbsParameter.wbsTaskFilterDto.Date + "'");
            }
            if (wbsParameter.wbsTaskFilterDto.StartDate != null && wbsParameter.wbsTaskFilterDto.EndDate != null)
            {
                sb.Append(" AND WbsTask.Date BETWEEN '" + wbsParameter.wbsTaskFilterDto.StartDate + "' AND '" + wbsParameter.wbsTaskFilterDto.EndDate +"'");
            }

            if (wbsParameter.wbsTaskFilterDto.Name != null)
            {
                sb.Append(" AND WbsTask.Name Like '%" + wbsParameter.wbsTaskFilterDto.Name + "%'");
            }

            if (wbsParameter.wbsTaskFilterDto.StatusId != null)
            {
                sb.Append(" AND StatusId = '" + wbsParameter.wbsTaskFilterDto.StatusId + "'");
            }

            if (wbsParameter.wbsTaskFilterDto.IsFavourite != null && wbsParameter.wbsTaskFilterDto.IsFavourite.Value)
            {

                sb.Append(" AND IsFavourite = '" + wbsParameter.wbsTaskFilterDto.IsFavourite + "'");
            }

            if (wbsParameter.wbsTaskFilterDto.Tag != null)
            {
                sb.Append("  AND WbsTaskTags.Name Like '%" + wbsParameter.wbsTaskFilterDto.Tag + "%'");
            }

            if (wbsParameter.wbsTaskFilterDto.ShownBy != null)
            {
                switch (wbsParameter.wbsTaskFilterDto.ShownBy)
                {
                    case "openTask":
                        sb.Append(" AND StatusId != 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                        break;
                    case "completedTask":
                        sb.Append(" AND StatusId = 'vvvv969c-f3e8-49ac-9746-51e7e23f2782'");
                        break;
                }
            }

            var vvvv = connection.Query<WbsTaskFilterResults>(sb.ToString()).ToList();

            var data = vvvv.DistinctBy(v => v.Id).ToList();
            var taskDocs = connection.Query<WbsTaskDocuments>("Select * From WbsTaskDocuments").ToList();
            var productEmails = connection.Query<WbsProductEmai>("Select * From WbsProductEmai").ToList();
            var persons = dbConnection.Query<CabPerson>("Select * From CabPerson");

            foreach (var listItem in data)
            {
                listItem.TotalAttachment = taskDocs.Count(x => x.TaskId == listItem.Id);
                listItem.TotalMail = productEmails.Count(x => x.WbsProductId == listItem.ParentId);

                wbsParameter.getWbsTaskInstructionList = new GetWbsTaskInstructionList()
                {
                    TaskId = listItem.Id,
                    ProductId = listItem.ProductId
                };
                var instructionList = await GetWbsTaskInstructionList(wbsParameter);
                listItem.TotalInstructions = instructionList.Count;
                listItem.TotalCheckedInstructions = instructionList.Count(x => x.IsChecked);

                var toList = connection.Query<WbsTaskTo>(
                    "Select * From WbsTaskTo Where TaskId = @TaskId AND PersonId IS NOT NULL",
                    new { TaskId = listItem.Id }).ToList();

                foreach (var to in toList)
                {
                    to.PersonName = persons.FirstOrDefault(x => x.Id == to.PersonId)?.FullName;
                }

                listItem.ToPerson = toList ?? new List<WbsTaskTo>();


                var ccList = connection.Query<WbsTaskCc>(
                    "Select * From WbsTaskCc Where TaskId = @TaskId AND PersonId IS NOT NULL",
                    new { TaskId = listItem.Id }).ToList();

                foreach (var cc in ccList)
                {
                    cc.PersonName = persons.FirstOrDefault(x => x.Id == cc.PersonId)?.FullName;
                }

                listItem.Cc = ccList ?? new List<WbsTaskCc>();

                // listItem.To = toList.FirstOrDefault()?.PersonId;
                // listItem.PersonName = toList.FirstOrDefault()?.PersonName;
                listItem.ProjectSequenceId = projects.SequenceCode;
                listItem.Cu = projects.Cu;
                listItem.ProjectTitle = projects.Title;

                if (listItem.Date?.Date == DateTime.UtcNow.Date)
                {
                    listItem.DeliveryStatusId = "jdjj52c0-058b-issu-afbd-1d2d24105ebc";
                }

                if (listItem.Date?.Date < DateTime.UtcNow.Date)
                {
                    listItem.DeliveryStatusId = "i1ce52c0-058b-issu-afbd-1d2d24105ebc";
                }

                if (listItem.Date?.Date > DateTime.UtcNow.Date &&
                    listItem.Date?.Date <= DateTime.UtcNow.AddDays(7).Date)
                {
                    listItem.DeliveryStatusId = "2210e768-msms-po02-Lot5-ee367a82ad22";
                }

                if (listItem.Date?.Date > DateTime.UtcNow.AddDays(7).Date)
                {
                    listItem.DeliveryStatusId = "12a22319-8ca7-temp-b588-6fab99474130";
                }
            }

            filteredResults = wbsParameter.wbsTaskFilterDto.DeliveryStatusId != null
                ? data.Where(x => x.DeliveryStatusId == wbsParameter.wbsTaskFilterDto.DeliveryStatusId).ToList()
                : data.ToList();

            if (wbsParameter.wbsTaskFilterDto.IsMyTask != null)
            {
                if (wbsParameter.wbsTaskFilterDto.IsMyTask.Value)
                {
                    var personId = personCompany.FirstOrDefault(x => x.Oid == wbsParameter.UserId)?.PersonId;

                    filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => v.PersonId.Contains(personId)))
                        .ToList();
                }
                else
                {
                    filteredResults = filteredResults.Where(x =>
                            x.ToPerson.Any(v =>
                                wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) ||
                            x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))))
                        .ToList();

                }

            }
            
            if (wbsParameter.wbsTaskFilterDto.IsCc != null)
            {
                if (wbsParameter.wbsTaskFilterDto.IsCc.Value)
                {

                    filteredResults = filteredResults.Where(x => x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                }
            }
                
            if (wbsParameter.wbsTaskFilterDto.IsTo != null)
            {
                if (wbsParameter.wbsTaskFilterDto.IsTo.Value)
                {

                    filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();

                }
            }
                
            // if (wbsParameter.wbsTaskFilterDto.IsTo != null && wbsParameter.wbsTaskFilterDto.IsCc != null)
            // {
            //     if (wbsParameter.wbsTaskFilterDto.IsTo.Value && wbsParameter.wbsTaskFilterDto.IsCc.Value)
            //     {
            //
            //         filteredResults = filteredResults.Where(x => x.ToPerson.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId))) || x.Cc.Any(v => wbsParameter.wbsTaskFilterDto.PersonId.Any(c => c.Contains(v.PersonId)))).ToList();
            //
            //     }
            // }
            
            allProjectsFilteredResults.AddRange(filteredResults);
        }

        foreach (var groupData in allProjectsFilteredResults.GroupBy(x => x.Date?.Date).OrderByDescending(v => v.Key))
        {
            
            var obj = new WbsTaskFilterResultsDto()
            {
                GroupItem = groupData.Key?.ToString("yyyy-MM-dd"),
                FilterList = groupData.ToList()
            };
        
            if (obj.FilterList.Any())
            {
                if (wbsParameter.wbsTaskFilterDto.Sorter?.Attribute != null)
                {
                    obj.FilterList = wbsParameter.wbsTaskFilterDto.Sorter?.Attribute.ToLower() switch
                    {
                        "isfavourite" => obj.FilterList.OrderByDescending(x => x.IsFavourite).ToList(),
                        "creationdate" => obj.FilterList.OrderByDescending(x => x.CreationDate).ToList(),
                        "date" => obj.FilterList.OrderByDescending(x => x.Date).ToList(),
                        _ => obj.FilterList
                    };
                }
                results.Add(obj);
            }
        }
        
        return results;
    }
    
    public async Task<List<TaskInstruction>> GetWbsTaskInstructionList(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        var data = connection.Query<TaskInstruction>(@"SELECT
                                                                                             CONCAT(Instructions.SequenceCode,' - ',Instructions.Name) AS Title
                                                                                            ,PbsInstruction.Id
                                                                                            FROM dbo.PbsInstruction
                                                                                            LEFT OUTER JOIN dbo.Instructions
                                                                                              ON PbsInstruction.InstructionsId = Instructions.Id
                                                                                            WHERE PbsInstruction.PbsProductId = (Select Id From PbsProduct Where ProductId =  @PbsProductId )
                                                                                              AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false'",new{PbsProductId = wbsParameter.getWbsTaskInstructionList.ProductId}).ToList();
        
        var taskInstructions = connection
            .Query<string>("Select InstructionId From WbsTaskInstruction Where TaskId = @TaskId",
                new { TaskId = wbsParameter.getWbsTaskInstructionList.TaskId }).ToList();
            
        data.Where(x => taskInstructions.Contains(x.Id)).ToList().ForEach(c => c.IsChecked = true);
        
        return data;
    }


    public async Task<List<WbsTaskFilterResults>> GetWbsList(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);


        var query = "Select * From WbsTask";

        var sb = new StringBuilder(query);
       
        var data = connection.Query<WbsTaskFilterResults>(sb.ToString()).DistinctBy(x => x.Id).ToList();
        var taskDocs = connection.Query<WbsTaskDocuments>("Select * From WbsTaskDocuments").ToList();
        var taskEmails = connection.Query<WbsTaskTags>("Select * From WbsTaskEmail").ToList();
        var persons = dbConnection.Query<CabPerson>("Select * From CabPerson");
        
        foreach (var listItem in data)
        {
           
                listItem.TotalAttachment = taskDocs.Count(x => x.TaskId == listItem.Id);
                listItem.TotalMail = taskEmails.Count(x => x.TaskId == listItem.Id);
                
                var toList = connection.Query<WbsTaskTo>("Select * From WbsTaskTo Where TaskId = @TaskId",
                    new { TaskId = listItem.Id }).ToList();

                foreach (var item in toList)
                {
                    item.PersonName = persons.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
                }

                listItem.ToPerson = toList;
                listItem.To = toList.FirstOrDefault()?.PersonId;
                listItem.PersonName = toList.FirstOrDefault()?.PersonName;


        }
        
        return data;
    }
    
    public async Task<string> WbsProductCreate(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var pbsParameters = new PbsParameters
        {
            ProjectSequenceId = wbsParameter.ProjectSequenceId,
            ContractingUnitSequenceId = wbsParameter.ContractingUnitSequenceId,
            TenantProvider = wbsParameter.TenantProvider,
            ChangedUser =new ApplicationUser()
        };

        var wbsProduct = connection
            .Query<WbsProduct>("Select * From WbsProduct Where Id = @Id", new { wbsParameter.wbsProductCreate.Id })
            .FirstOrDefault();
        
        if (wbsProduct == null)
        {
            if (wbsParameter.wbsProductCreate.Id == null)
            {
                wbsParameter.wbsProductCreate.Id = Guid.NewGuid().ToString();

                if (wbsParameter.wbsProductCreate.WbsTaxonomyId != null)
                {
                    var wbsTax = connection.Query<WbsTaxonomy>("Select * From WbsTaxonomy Where Id = @Id",
                        new { Id = wbsParameter.wbsProductCreate.WbsTaxonomyId }).FirstOrDefault();
                    
                    var taxonomyParam = new WbsTaxonomy()
                    {
                        Id = wbsParameter.wbsProductCreate.Id,
                        Title = wbsParameter.wbsProductCreate.Name,
                        ParentId = wbsParameter.wbsProductCreate.WbsTaxonomyId,
                        CreatedBy = wbsParameter.UserId,
                        CreatedDateTime = DateTime.UtcNow,
                        WbsTaxonomyLevelId = "e1ce52c0-058b-prod-afbd-1d2d24105ebc",
                        TemplateId = wbsTax?.TemplateId
                    };

                    const string insertWbs =
                        @"INSERT INTO dbo.WbsTaxonomy ( Id ,WbsTaxonomyLevelId ,ParentId ,CreatedDateTime ,CreatedBy,SequenceId ,Title,IsDefault,TemplateId ) VALUES ( @Id ,@WbsTaxonomyLevelId ,@ParentId ,@CreatedDateTime ,@CreatedBy,@SequenceId ,@Title,@IsDefault,@TemplateId);";

                    await connection.ExecuteAsync(insertWbs, taxonomyParam);
                }
            }
            const string taskInsert = @"INSERT INTO dbo.WbsProduct ( Id ,WbsTaxonomyId ,ProductId ,Name ,Note  ,Date,CreationDate,DeliveryStatusId,StatusId,CompletionDate,DisplayOrder,IsFavourite ) VALUES ( @Id ,@WbsTaxonomyId ,@ProductId ,@Name ,@Note ,@Date,@CreationDate,@DeliveryStatusId,@StatusId,@CompletionDate,@DisplayOrder, @IsFavourite )";

            var maxDisplayOrder = connection.Query<int>(@"SELECT CASE
                                                                WHEN MAX(DisplayOrder) IS NULL THEN 0
                                                                ELSE MAX(DisplayOrder)
                                                                END AS count
                                                                FROM WbsProduct").FirstOrDefault();
            
            var dtoNew = new PbsProductCreateDto
            {
                Id = wbsParameter.wbsProductCreate.Id,
                Name = wbsParameter.wbsProductCreate.Name,
                PbsType = "regular",
                PbsProductItemTypeId = "aa0c8e3c-f716-4f92-afee-851d485164da",
                Scope = "0",
                PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
                Contract = "yes"
            };
            
            pbsParameters.PbsDto = dtoNew;

            var pbs = await wbsParameter.IPbsRepository.CreatePbs(pbsParameters).ConfigureAwait(false);
            
            var taskParam = new WbsProduct
            {
                Id = wbsParameter.wbsProductCreate.Id,
                WbsTaxonomyId = wbsParameter.wbsProductCreate.WbsTaxonomyId,
                ProductId = pbs.ProductId,
                Name = wbsParameter.wbsProductCreate.Name,
                Note = wbsParameter.wbsProductCreate.Note,
                Date = wbsParameter.wbsProductCreate.Date,
                CreationDate = DateTime.UtcNow,
                StatusId = wbsParameter.wbsProductCreate.StatusId,
                DeliveryStatusId = wbsParameter.wbsProductCreate.DeliveryStatusId,
                CompletionDate = wbsParameter.wbsProductCreate.CompletionDate,
                DisplayOrder = maxDisplayOrder + 1,
                IsFavourite = false
            };

            await connection.ExecuteAsync(taskInsert, taskParam);

            const string ccInsert = @"INSERT INTO dbo.WbsProductCc ( Id ,PersonId ,WbsProductId ) VALUES ( @Id ,@PersonId ,@WbsProductId )";

            foreach (var cc in wbsParameter.wbsProductCreate.Cc)
            {
                cc.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = cc.EmailAddress }).FirstOrDefault();
                
                var ccParam = new WbsProductCc
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = cc.PersonId,
                    WbsProductId = taskParam.Id
                };

                await connection.ExecuteAsync(ccInsert, ccParam);

            }
            
            const string toInsert = @"INSERT INTO dbo.WbsProductTo ( Id ,PersonId ,WbsProductId ) VALUES ( @Id ,@PersonId ,@WbsProductId )";

            var increment = 0;
            foreach (var to in wbsParameter.wbsProductCreate.ToPerson)
            {
                to.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = to.EmailAddress }).FirstOrDefault();

                
                var toParam = new WbsProductTo
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = to.PersonId,
                    WbsProductId = taskParam.Id,
                    IsDefault = increment == 0
                };

                await connection.ExecuteAsync(toInsert, toParam);

                increment++;

            }

            const string tagsInsert =
                @"INSERT INTO dbo.WbsProductTags ( Id ,Name ,WbsProductId ) VALUES ( @Id ,@Name ,@WbsProductId )";
            
            foreach (var tag in wbsParameter.wbsProductCreate.Tags)
            {
                
                var tagParam = new WbsProductTags
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = tag.Name,
                    WbsProductId = taskParam.Id
                };

                await connection.ExecuteAsync(tagsInsert, tagParam);

            }
            
            const string docsInsert =
                @"INSERT INTO dbo.WbsProductDocuments ( Id ,Link ,WbsProductId ) VALUES ( @Id ,@Link ,@WbsProductId )";
            
            foreach (var doc in wbsParameter.wbsProductCreate.Documents)
            {
                
                var docParam = new WbsProductDocuments
                {
                    Id = Guid.NewGuid().ToString(),
                    Link = doc.Link,
                    WbsProductId = taskParam.Id
                };

                await connection.ExecuteAsync(docsInsert, docParam);

            }
            
            
        }
        else
        {
            const string update = @"UPDATE dbo.WbsTaxonomy SET ParentId = @ParentId,Title =@Title WHERE Id =@Id ;";

            await connection.ExecuteAsync(update,
                new { Id = wbsParameter.wbsProductCreate.Id, Title = wbsParameter.wbsProductCreate.Name, ParentId = wbsParameter.wbsProductCreate.WbsTaxonomyId });
            
            var dtoNew = new PbsProductCreateDto
            {
                Id = wbsParameter.wbsProductCreate.Id,
                Name = wbsParameter.wbsProductCreate.Name,
                PbsType = "regular",
                PbsProductItemTypeId = "aa0c8e3c-f716-4f92-afee-851d485164da",
                Scope = "0",
                PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
                Contract = "yes"
            };
            
            pbsParameters.PbsDto = dtoNew;

            var pbs = await wbsParameter.IPbsRepository.CreatePbs(pbsParameters).ConfigureAwait(false);
            
            const string updateTask =
                @"UPDATE dbo.WbsProduct SET WbsTaxonomyId = @WbsTaxonomyId ,ProductId = @ProductId ,Name = @Name ,Note = @Note ,Date = @Date ,DeliveryStatusId = @DeliveryStatusId ,StatusId = @StatusId ,CompletionDate = @CompletionDate WHERE Id = @Id";
            
            var taskParam = new WbsProduct
            {
                Id = wbsParameter.wbsProductCreate.Id,
                WbsTaxonomyId = wbsParameter.wbsProductCreate.WbsTaxonomyId,
                ProductId = wbsParameter.wbsProductCreate.ProductId,
                Name = wbsParameter.wbsProductCreate.Name,
                Note = wbsParameter.wbsProductCreate.Note,
                Date = wbsParameter.wbsProductCreate.Date,
                StatusId = wbsParameter.wbsProductCreate.StatusId,
                DeliveryStatusId = wbsParameter.wbsProductCreate.DeliveryStatusId,
                CompletionDate =  wbsParameter.wbsProductCreate.CompletionDate
            };

            await connection.ExecuteAsync(updateTask, taskParam);

            const string ccInsert = @"INSERT INTO dbo.WbsProductCc ( Id ,PersonId ,WbsProductId ) VALUES ( @Id ,@PersonId ,@WbsProductId )";

            await connection.ExecuteAsync("Delete From WbsProductCc Where WbsProductId = @WbsProductId ",
                new { WbsProductId = wbsParameter.wbsProductCreate.Id });
            
            foreach (var cc in wbsParameter.wbsProductCreate.Cc)
            {
                cc.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = cc.EmailAddress }).FirstOrDefault();
                
                var ccParam = new WbsProductCc
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = cc.PersonId,
                    WbsProductId = taskParam.Id
                };

                await connection.ExecuteAsync(ccInsert, ccParam);

            }
            
            const string toInsert = @"INSERT INTO dbo.WbsProductTo ( Id ,PersonId ,WbsProductId ) VALUES ( @Id ,@PersonId ,@WbsProductId )";

            await connection.ExecuteAsync("Delete From WbsProductTo Where WbsProductId = @WbsProductId ",
                new { WbsProductId = wbsParameter.wbsProductCreate.Id });

            var increment = 0;
            foreach (var to in wbsParameter.wbsProductCreate.ToPerson)
            {
                
                to.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = to.EmailAddress }).FirstOrDefault();
                
                var toParam = new WbsProductTo
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = to.PersonId,
                    WbsProductId = taskParam.Id,
                    IsDefault = increment == 0
                };

                await connection.ExecuteAsync(toInsert, toParam);

                increment++;
            }

            const string tagsInsert =
                @"INSERT INTO dbo.WbsProductTags ( Id ,Name ,WbsProductId ) VALUES ( @Id ,@Name ,@WbsProductId )";
            
            await connection.ExecuteAsync("Delete From WbsProductTags Where WbsProductId = @WbsProductId ",
                new { WbsProductId = wbsParameter.wbsProductCreate.Id });
            
            foreach (var tag in wbsParameter.wbsProductCreate.Tags)
            {
                
                var tagParam = new WbsProductTags
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = tag.Name,
                    WbsProductId = taskParam.Id
                };

                await connection.ExecuteAsync(tagsInsert, tagParam);

            }
            
            const string docsInsert =
                @"INSERT INTO dbo.WbsProductDocuments ( Id ,Link ,WbsProductId ) VALUES ( @Id ,@Link ,@WbsProductId )";
            
            await connection.ExecuteAsync("Delete From WbsProductDocuments Where WbsProductId = @WbsProductId ",
                new { WbsProductId = wbsParameter.wbsProductCreate.Id });
            
            foreach (var doc in wbsParameter.wbsProductCreate.Documents)
            {
                
                var docParam = new WbsProductDocuments
                {
                    Id = Guid.NewGuid().ToString(),
                    Link = doc.Link,
                    WbsProductId = taskParam.Id
                };

                await connection.ExecuteAsync(docsInsert, docParam);

            }
            
            
        }

        return wbsParameter.wbsProductCreate.Id;
    }
    
    public async Task<GetWbsProduct> WbsProductGetById(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var data = connection.Query<GetWbsProduct>("Select WbsProduct.*,pp.Title As PbsTitle from WbsProduct LEFT OUTER JOIN PbsProduct pp ON WbsProduct.Id = pp.Id where WbsProduct.Id = @Id", new { Id = wbsParameter.Id })
            .FirstOrDefault();

        if (data != null)
        {
            var persons = dbConnection.Query<CabDataDapperDto>("SELECT CabPerson.FullName,CabPerson.Id AS PersonId,ce.EmailAddress AS Email FROM CabPerson LEFT OUTER JOIN CabPersonCompany cpc ON CabPerson.Id = cpc.PersonId LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id");
            
            var CcList = connection.Query<WbsProductCc>("Select * From WbsProductCc Where WbsProductId = @WbsProductId",
                new { WbsProductId = wbsParameter.Id }).ToList();

            foreach (var item in CcList)
            {
                item.PersonName = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.FullName;
                item.EmailAddress = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.Email;

            }

            data.Cc = CcList;
            
            
            var toList = connection.Query<WbsProductTo>("Select * From WbsProductTo Where WbsProductId = @WbsProductId ORDER BY IsDefault DESC",
                            new { WbsProductId = wbsParameter.Id }).ToList();
            
                        foreach (var item in toList)
                        {
                            item.PersonName = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.FullName;
                            item.EmailAddress = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.Email;

                        }
            
            data.ToPerson = toList;
            
            data.Documents = connection.Query<WbsProductDocuments>("Select * From WbsProductDocuments Where WbsProductId = @WbsProductId",
                new { WbsProductId = wbsParameter.Id }).ToList();
            
            data.Tags = connection.Query<WbsProductTags>("Select * From WbsProductTags Where WbsProductId = @WbsProductId",
                new { WbsProductId = wbsParameter.Id }).ToList();

            var wbsFilter = new WbsTaskFilterDto();
            wbsParameter.wbsTaskFilterDto = wbsFilter;
            wbsParameter.ProductGetById = data.ProductId;
            // var taskList =  WbsTaskFilter(wbsParameter).Result.Where(x => x.ProductId == data.ProductId).ToList();
            
            var taskList =  await WbsTaskFilter(wbsParameter);
            
            data.Tasks = taskList;

            var convList = new List<GetConversationDto>();
            if (data.Tasks.Any())
            {
                foreach (var item in data.Tasks)
                {
                    wbsParameter.Id = item.Id;
                    var conv = new GetConversationDto()
                    {
                        Task = item.Name,
                        Conversations = await GetWbsConversation(wbsParameter)
                    };
                    
                    convList.Add(conv);
                }

                data.Conversations = convList;

            }
        }

        return data;
    }
    
    public async Task<string> WbsTaskIsFavourite(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        if (wbsParameter.TaskIsFavouriteDto != null)
        {
            const string query = "Update WbsTask Set IsFavourite = @IsFavourite Where Id = @Id";

            await connection.ExecuteAsync(query, new { Id = wbsParameter.TaskIsFavouriteDto.Id, IsFavourite = wbsParameter.TaskIsFavouriteDto.IsFavourite });
        }

        return wbsParameter.Id;
    }
    
    public async Task<string> WbsProductDelete(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await connection.ExecuteAsync("Delete From WbsProduct Where Id = @Id", new { Id = wbsParameter.Id });

        return wbsParameter.Id;
    }
    
    public async Task<string> CopyWbsToProject(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        var cuConnectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            null, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        await using var cuConnection = new SqlConnection(cuConnectionString);

        var wbsTaxonomys = cuConnection.Query<WbsTaxonomy>("Select * From WbsTaxonomy Where TemplateId = @TemplateId",
            new { TemplateId = wbsParameter.Id }).ToList();

        var wbsProduct = cuConnection.Query<WbsProduct>("Select * From WbsProduct Where Id IN @Ids",
            new { Ids = wbsTaxonomys.Select(x => x.Id).ToList() }).ToList();

        var pbsList = cuConnection.Query<PbsProduct>("Select * From PbsProduct Where ProductId IN @Ids",
            new { Ids = wbsProduct.Select(x => x.ProductId).ToList() }).ToList();
        
        var wbsTask = cuConnection.Query<WbsTask>("Select * From WbsTask Where Id IN @Ids",
            new { Ids = wbsTaxonomys.Select(x => x.Id).ToList() }).ToList();
        
        var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);
        
        const string insertWbs = @"INSERT INTO dbo.WbsTaxonomy ( Id ,WbsTaxonomyLevelId ,ParentId ,CreatedDateTime ,CreatedBy,SequenceId ,Title,IsDefault,TemplateId ) VALUES ( @Id ,@WbsTaxonomyLevelId ,@ParentId ,@CreatedDateTime ,@CreatedBy,@SequenceId ,@Title,@IsDefault,@TemplateId);";
        const string wProductInsert = @"INSERT INTO dbo.WbsProduct ( Id ,WbsTaxonomyId ,ProductId ,Name ,Note  ,Date,CreationDate,DeliveryStatusId,StatusId,CompletionDate,DisplayOrder,IsFavourite ) VALUES ( @Id ,@WbsTaxonomyId ,@ProductId ,@Name ,@Note ,@Date,@CreationDate,@DeliveryStatusId,@StatusId,@CompletionDate,@DisplayOrder, @IsFavourite )";
        const string wbsTaskInsert = @"INSERT INTO dbo.WbsTask ( Id ,WbsTaxonomyId ,ProductId ,Name ,Note  ,Date,CreationDate,DeliveryStatusId,StatusId,CompletionDate,DisplayOrder,IsFavourite ) VALUES ( @Id ,@WbsTaxonomyId ,@ProductId ,@Name ,@Note ,@Date,@CreationDate,@DeliveryStatusId,@StatusId,@CompletionDate,@DisplayOrder, @IsFavourite )";

        const string createSql = @"INSERT INTO [dbo].[PbsProduct] VALUES (@Id ,@ProductId ,@Name ,@PbsProductItemTypeId ,@PbsProductStatusId ,@Scope ,@Contract ,@ProductPurpose ,@ProductComposition ,@ProductDerivation ,@ProductFormatPresentation ,@PbsToleranceStateId ,@PbsQualityResponsibilityId ,@IsDeleted ,@NodeType ,@PbsTaxonomyLevelId ,@Title ,@PbsType ,@ProjectSequenceCode ,@ParentId ,@EndDate ,@StartDate ,@MachineTaxonomy ,@Quantity ,@MeasurementCode ,@Mou ,@QualityProducerId ,@WbsTaxonomyId, @PbsLotId,@PbsLocation)";
        
        foreach (var wbs in wbsTaxonomys)
        {
            await connection.ExecuteAsync(insertWbs, wbs);
        }
        
        foreach (var wProduct in wbsProduct)
        {
            await connection.ExecuteAsync(wProductInsert, wProduct);
        }
        
        foreach (var pbs in pbsList)
        {
            await connection.ExecuteAsync(createSql, pbs);
        }
        
        foreach (var task in wbsTask)
        {
            await connection.ExecuteAsync(wbsTaskInsert, task);
        }

        await CopyPbsInstructions(wbsParameter, pbsList);
        
        var taskInstructionList = cuConnection.Query<WbsTaskInstruction>("Select * From WbsTaskInstruction Where TaskId IN @Ids",
            new { Ids = wbsTask.Select(x => x.Id).ToList() });
            
        const string taskInstructionInsert =
            @"INSERT INTO dbo.WbsTaskInstruction ( Id ,TaskId, InstructionId ) VALUES ( @Id ,@TaskId, @InstructionId )";
            
        foreach (var ins in taskInstructionList)
        {
            await connection.ExecuteAsync(taskInstructionInsert, ins);
        }
        
        const string ccInsert = @"INSERT INTO dbo.WbsProductCc ( Id ,PersonId ,WbsProductId ) VALUES ( @Id ,@PersonId ,@WbsProductId )";

        var ccList = cuConnection.Query<WbsProductCc>("Select * From WbsProductCc Where WbsProductId IN @Ids",
            new { Ids = wbsProduct.Select(x => x.Id).ToList() });
        
            foreach (var cc in ccList)
            {
                await connection.ExecuteAsync(ccInsert, cc);
            }
            
            var toList = cuConnection.Query<WbsProductTo>("Select * From WbsProductTo Where WbsProductId IN @Ids",
                new { Ids = wbsProduct.Select(x => x.Id).ToList() });
            
            const string toInsert = @"INSERT INTO dbo.WbsProductTo ( Id ,PersonId ,WbsProductId ) VALUES ( @Id ,@PersonId ,@WbsProductId )";

            foreach (var to in toList)
            {
                await connection.ExecuteAsync(toInsert, to);
            }

            var tagList = cuConnection.Query<WbsProductTags>("Select * From WbsProductTags Where WbsProductId IN @Ids",
                new { Ids = wbsProduct.Select(x => x.Id).ToList() });
            
            const string tagsInsert =
                @"INSERT INTO dbo.WbsProductTags ( Id ,Name ,WbsProductId ) VALUES ( @Id ,@Name ,@WbsProductId )";
            
            foreach (var tag in tagList)
            {
                await connection.ExecuteAsync(tagsInsert, tag);
            }
            
            var docList = cuConnection.Query<WbsProductDocuments>("Select * From WbsProductDocuments Where WbsProductId IN @Ids",
                new { Ids = wbsProduct.Select(x => x.Id).ToList() });
            
            const string docsInsert =
                @"INSERT INTO dbo.WbsProductDocuments ( Id ,Link ,WbsProductId ) VALUES ( @Id ,@Link ,@WbsProductId )";
            
            foreach (var doc in docList)
            {
                await connection.ExecuteAsync(docsInsert, doc);
            }
            
            const string taskCcInsert = @"INSERT INTO dbo.WbsTaskCc ( Id ,PersonId ,TaskId ) VALUES ( @Id ,@PersonId ,@TaskId )";

            var taskCcList = cuConnection.Query<WbsTaskCc>("Select * From WbsTaskCc Where TaskId IN @Ids",
                new { Ids = wbsTask.Select(x => x.Id).ToList() });
        
            foreach (var cc in taskCcList)
            {
                await connection.ExecuteAsync(taskCcInsert, cc);
            }
            
            var taskToList = cuConnection.Query<WbsTaskTo>("Select * From WbsTaskTo Where TaskId IN @Ids",
                new { Ids = wbsTask.Select(x => x.Id).ToList() });
            
            const string tasktoInsert = @"INSERT INTO dbo.WbsTaskTo ( Id ,PersonId ,TaskId ) VALUES ( @Id ,@PersonId ,@TaskId )";

            foreach (var to in taskToList)
            {
                await connection.ExecuteAsync(tasktoInsert, to);
            }

            var tasktagList = cuConnection.Query<WbsTaskTags>("Select * From WbsTaskTags Where TaskId IN @Ids",
                new { Ids = wbsTask.Select(x => x.Id).ToList() });
            
            const string tasktagsInsert =
                @"INSERT INTO dbo.WbsTaskTags ( Id ,Name ,TaskId ) VALUES ( @Id ,@Name ,@TaskId )";
            
            foreach (var tag in tasktagList)
            {
                await connection.ExecuteAsync(tasktagsInsert, tag);
            }
            
            var taskdocList = cuConnection.Query<WbsTaskDocuments>("Select * From WbsTaskDocuments Where TaskId IN @Ids",
                new { Ids = wbsTask.Select(x => x.Id).ToList() });
            
            const string taskdocsInsert =
                @"INSERT INTO dbo.WbsTaskDocuments ( Id ,Link ,TaskId ) VALUES ( @Id ,@Link ,@TaskId )";
            
            foreach (var doc in taskdocList)
            {
                await connection.ExecuteAsync(taskdocsInsert, doc);
            }
            
            
        return wbsParameter.Id;
    }

    private async Task<List<PbsProduct>> CopyPbsInstructions(WbsParameter wbsParameter,List<PbsProduct> pbsList)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        var cuConnectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            null, wbsParameter.TenantProvider);
        
        await using var cuConnection = new SqlConnection(cuConnectionString);
        
        var pbsInstructionList = cuConnection.Query<PbsInstruction>("SELECT * FROM dbo.PbsInstruction WHERE PbsProductId IN @Ids",
            new { Ids = pbsList.Select(x => x.Id).ToList() }).ToList();
        
        var instructionList = cuConnection.Query<Instructions>("SELECT * FROM dbo.Instructions WHERE Id IN @Ids",
            new { Ids = pbsInstructionList.Select(x => x.InstructionsId).ToList() }).ToList();
        
        var pbsInstructionLinkList = cuConnection.Query<PbsInstructionLink>("SELECT * FROM dbo.PbsInstructionLink WHERE PbsInstructionId IN @Ids",
            new { Ids = pbsInstructionList.Select(x => x.Id).ToList() }).ToList();

        await using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.BulkInsertAndSelect(data: instructionList, identityInsert: true);
            connection.BulkInsertAndSelect(data: pbsInstructionList, identityInsert: true);
            connection.BulkInsertAndSelect(data: pbsInstructionLinkList, identityInsert: true);
            connection.Close();
        }
        
        return pbsList;
    }

    public async Task<string> CreateWbsConversation(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        foreach (var conversation in wbsParameter.WbsConversationDto)
        {
            
            const string insertWbs =
                @"INSERT INTO dbo.WbsConversation ( Id ,Type ,ItemId ,Subject ,Content ,DateTime ,FromPersonId ) VALUES ( @Id ,@Type ,@ItemId ,@Subject ,@Content ,@DateTime ,@FromPersonId );";

            
            conversation.Id = Guid.NewGuid().ToString();
            conversation.FromPersonId = dbConnection.Query<string>("SELECT CabPerson.Id FROM CabPerson LEFT OUTER JOIN CabPersonCompany cpc ON CabPerson.Id = cpc.PersonId LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id Where ce.EmailAddress = @EmailAddress",new{EmailAddress = conversation.From }).FirstOrDefault();

            await connection.ExecuteAsync(insertWbs, conversation);

            const string ccInsert =
                @"INSERT INTO dbo.WbsConversationCc ( Id ,PersonId ,WbsConversationId ) VALUES ( @Id ,@PersonId ,@WbsConversationId )";

            if (conversation.Cc != null)
            {
                foreach (var cc in conversation.Cc)
                {
                    cc.PersonId ??= dbConnection
                        .Query<string>(
                            "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                            new { email = cc.EmailAddress }).FirstOrDefault();
                    cc.Id = Guid.NewGuid().ToString();
                    cc.WbsConversationId = conversation.Id;
                    await connection.ExecuteAsync(ccInsert, cc);
                }
            }

            const string toInsert =
                @"INSERT INTO dbo.WbsConversationTo ( Id ,PersonId ,WbsConversationId ) VALUES ( @Id ,@PersonId ,@WbsConversationId )";

            if (conversation.ToPerson != null)
            {
                foreach (var to in conversation.ToPerson)
                {
                    to.PersonId ??= dbConnection
                        .Query<string>(
                            "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                            new { email = to.EmailAddress }).FirstOrDefault();

                    to.Id = Guid.NewGuid().ToString();
                    to.WbsConversationId = conversation.Id;
                    await connection.ExecuteAsync(toInsert, to);
                }
            }

            const string attachmentInsert =
                @"INSERT INTO dbo.WbsConversationAttachments ( Id ,Link ,WbsConversationId ) VALUES ( @Id ,@Link ,@WbsConversationId )";

            if (conversation.Attachments != null)
            {
                foreach (var attachment in conversation.Attachments)
                {
                    attachment.Id = Guid.NewGuid().ToString();
                    attachment.WbsConversationId = conversation.Id;
                    await connection.ExecuteAsync(attachmentInsert, attachment);
                }
            }
        }

        return "ok";
    }
    
    public async Task<List<WbsConversationDto>> GetWbsConversation(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var persons = dbConnection.Query<CabDataDapperDto>("SELECT CabPerson.FullName,CabPerson.Id AS PersonId,ce.EmailAddress AS Email FROM CabPerson LEFT OUTER JOIN CabPersonCompany cpc ON CabPerson.Id = cpc.PersonId LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id");

        
        var allConversation = connection.Query<WbsConversationDto>("Select * From WbsConversation Where ItemId = @ItemId",
            new { ItemId = wbsParameter.Id }).ToList();

        foreach (var conv in allConversation)
        {
            conv.From = persons.FirstOrDefault(x => x.PersonId == conv.FromPersonId)?.Email;
            conv.FromPersonName = persons.FirstOrDefault(x => x.PersonId == conv.FromPersonId)?.FullName;

            var CcList = connection.Query<WbsConversationCc>("Select * From WbsConversationCc Where WbsConversationId = @WbsConversationId",
                new { WbsConversationId = conv.Id }).ToList();
            
            foreach (var item in CcList)
            {
                item.PersonName = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.FullName;
                item.EmailAddress = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.Email;

            }

            conv.Cc = CcList;
            
            var toList = connection.Query<WbsConversationTo>("Select * From WbsConversationTo Where WbsConversationId = @WbsConversationId",
                new { WbsConversationId = conv.Id }).ToList();
            
            foreach (var item in toList)
            {
                item.PersonName = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.FullName;
                item.EmailAddress = persons.FirstOrDefault(x => x.PersonId == item.PersonId)?.Email;

            }
            
            conv.ToPerson = toList;
            
            conv.Attachments = connection.Query<WbsConversationAttachments>("Select * From WbsConversationAttachments Where WbsConversationId = @WbsConversationId",
                new { WbsConversationId = conv.Id }).ToList();

        }
        
        return allConversation;
    }

    private static string ExtractPropertyValuesForInsertQuery<T>(List<T> items)
    {
        if (items == null || items.Count == 0)
        {
            return string.Empty;
        }

        var result = new List<string>();

        var properties = typeof(T).GetProperties();

        foreach (var item in items)
        {
            var values = properties.Select(prop => FormatPropertyValue(prop.GetValue(item))).ToArray();
            var formatted = $"({string.Join(",", values)})";
            result.Add(formatted);
        }
        
        var concatedString =  result.Aggregate((x, y) => x + ", " + y);

        return concatedString;
    }

    private static string FormatPropertyValue(object value)
    {
        return value is string or DateTime ? $"'{value}'" : value.ToString();
    }
    
    public async Task<string> WbsEditTemplateName(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            null, wbsParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);
        wbsParameter.WbsTemplateUpdate.Title =
            wbsParameter.WbsTemplateUpdate.SequenceCode + " " + wbsParameter.WbsTemplateUpdate.Name;
        await connection.ExecuteAsync(@"UPDATE dbo.WbsTemplate SET Title = @Title,Name = @Name WHERE Id = @Id", wbsParameter.WbsTemplateUpdate);
        return wbsParameter.WbsTemplateUpdate.Id;
    }

    public async Task<List<WbsTaskCheckListDto>> GetWbChecklistById(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId = "COM-0001";

        var cuConnectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        await using var dbConnection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var results = new List<WbsTaskCheckListDto>();
        const string sql = @"SELECT DISTINCT
                      ProjectDefinition.SequenceCode AS SequenceCode
                     ,ProjectDefinition.Name AS Name
                     ,ProjectDefinition.Id
                     ,ProjectDefinition.Title AS Title
                     ,ProjectDefinition.ProjectConnectionString
                     ,CabCompany.SequenceCode As Cu
                    FROM dbo.ProjectDefinition
                    LEFT OUTER JOIN dbo.ProjectUserRole
                      ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                    LEFT OUTER JOIN dbo.UserRole
                      ON ProjectUserRole.UsrRoleId = UserRole.Id
                    LEFT OUTER JOIN dbo.ApplicationUser
                      ON UserRole.ApplicationUserOid = ApplicationUser.OId
                    LEFT OUTER JOIN dbo.CabCompany
                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                    WHERE ApplicationUser.OId = @oid 
                    AND ProjectDefinition.IsDeleted = 0 ";

        var param = new
            { lang = wbsParameter.Lang, oid = wbsParameter.UserId, cuId = wbsParameter.ContractingUnitSequenceId };

        var userProjects = dbConnection.Query<UserProjectList>(sql, param);

        foreach (var projects in userProjects)
        {
            try
            {


                await using var projectConnection = new SqlConnection(projects.ProjectConnectionString);

                var emailTasks = projectConnection
                    .Query<string>("Select TaskId From WbsTaskEmail Where EmailId = @Id ", new { wbsParameter.Id })
                    .ToList();
                if (emailTasks.Any())
                {

                    wbsParameter.wbsTaskFilterDto = new WbsTaskFilterDto();
                    wbsParameter.ProjectSequenceId = projects.SequenceCode;
                    wbsParameter.IdList = emailTasks;
                    // wbsParameter.IsCheckList = true;
                    var taskList = await WbsTaskFilter(wbsParameter);
                    
                    // taskList = taskList.Where(x => emailTasks.Any(c => c.Contains(x.Id))).ToList();

                    if (taskList.Any())
                    {
                        var node = new WbsTaskCheckListDto()
                        {
                            GroupItem = projects.Title,
                            FilterList = new List<WbsTaskFilterResultsDto>()
                        };
                        var groupList = taskList.GroupBy(x => x.ProductId);

                        foreach (var product in groupList)
                        {
                            var obj = new WbsTaskFilterResultsDto()
                            {
                                GroupItem = product.Key,
                                FilterList = product.ToList()
                            };

                            if (obj.FilterList.Any())
                            {
                                node.FilterList.Add(obj);
                            }
                        }

                        results.Add(node);
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception("dfndsjnjnj" + projects.SequenceCode + " - " + e );
            }
        }

        return results;
    }
    
    public async Task<WbsProductEmai> AddWbsProductEmails(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);

        const string sql = "Insert Into WbsProductEmai Values (@Id,@EmailId,@WbsProductId)";

        wbsParameter.WbsProductEmai.Id = Guid.NewGuid().ToString();

        await connection.ExecuteAsync(sql, wbsParameter.WbsProductEmai);
       
         
        return wbsParameter.WbsProductEmai;
    }

    public async Task<List<WbsTaskTags>> GetTagList(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        var tagList = connection.Query<WbsTaskTags>("SELECT * FROM WbsTaskTags");

        return tagList.DistinctBy(x => x.Name).ToList();
    }
    
    public async Task<string> WbsDragAndDrop(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("UPDATE dbo.WbsTaxonomy SET ParentId = @ParentId,UpdatedBy = @UserId WHERE Id = @Id", new {wbsParameter.WbsDragAndDrop.Id,wbsParameter.WbsDragAndDrop.ParentId,wbsParameter.UserId});

        return wbsParameter.WbsDragAndDrop.Id;
    }
    

    public async Task<string> WbsTaskStatusUpdate(WbsParameter wbsParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("UPDATE dbo.WbsTask SET StatusId = @StatusId WHERE Id = @Id", new {Id = wbsParameter.TaskStatusUpdateDto.Id,StatusId = wbsParameter.TaskStatusUpdateDto.StatusId});

        return wbsParameter.TaskStatusUpdateDto.Id;
    }

    public async Task<string> UploadWbsDocument1(WbsParameter wbsParameter)
    {
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE ProjectTitle = @ProjectTitle AND Wbs = @Wbs AND Product = @Product",
            wbsParameter.UploadWbsDocumentDto).FirstOrDefault();
        
        var token =  UploadWbsDocumentGetToken(wbsParameter);
        
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            var site = wbsParameter.UploadWbsDocumentDto.ProjectTitle.Replace(" ", "");
            var siteResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/uprince.sharepoint.com:/sites/{site}");

            string siteId;
            if (siteResponse.IsSuccessStatusCode)
            {
                
                var siteContent = await siteResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphSiteResponse>(siteContent);
                
                siteId = dto.Id;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {siteResponse.StatusCode}");
            }
            
            var driveResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/{siteId}/drives");

            string driveId;
            if (driveResponse.IsSuccessStatusCode)
            {
                
                var driveContent = await driveResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphDriveResponse>(driveContent);
                
                driveId = dto.Value.FirstOrDefault().Id;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {driveResponse.StatusCode}");
            }

            if (wbsDoc == null )
            {
                var jsonBody = $"{{\"name\": \"{wbsParameter.UploadWbsDocumentDto.Wbs}\", \"folder\": {{}}, \"@microsoft.graph.conflictBehavior\": \"rename\"}}";

                var folderCreateContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var createFolderResponse = await client.PostAsync($"https://graph.microsoft.com/v1.0/drives/{driveId}/root:/General:/children", folderCreateContent);
            }

            foreach (var doc in wbsParameter.doc)
            {
                string fileName = Path.GetFileName(doc.FileName);

                var requestUrl = $"https://graph.microsoft.com/v1.0/drives/{driveId}/root:/General/{wbsParameter.UploadWbsDocumentDto.Wbs}/{fileName}:/content";

                await using (var stream = doc.OpenReadStream())
                {
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var fileUploadResponse = await client.PutAsync(requestUrl, content);

                    if (fileUploadResponse.IsSuccessStatusCode)
                    {
                        var fileUploadContent = await fileUploadResponse.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<FileUploadResponse>(fileUploadContent);

                        var docData = new WbsDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectTitle = wbsParameter.UploadWbsDocumentDto.ProjectTitle,
                            Product = wbsParameter.UploadWbsDocumentDto.Product,
                            FileName = fileName,
                            FileUrl = dto.WebUrl,
                            Wbs = wbsParameter.UploadWbsDocumentDto.Wbs
                        };

                        await connection.ExecuteAsync(
                            @"INSERT INTO dbo.WbsDocument ( Id ,ProjectTitle ,Product ,FileName ,FileUrl ,Wbs ) VALUES ( @Id ,@ProjectTitle ,@Product ,@FileName ,@FileUrl ,@Wbs );",
                            docData);
                    }
                    else
                    {
                        return fileUploadResponse.Content.ToString();
                    }
                }
            }
            return "Ok";
        }
        
    }

    public async Task<UploadWbsDocumentMetaDataDto> UploadWbsDocument(WbsParameter wbsParameter)
    {
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var token =  UploadWbsDocumentGetToken(wbsParameter);
        
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            var site = wbsParameter.UploadWbsDocumentDto.ProjectTitle.Replace(" ", "");
            var siteResponse =
                await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/uprince.sharepoint.com:/sites/{site}");

            string siteId;
            if (siteResponse.IsSuccessStatusCode)
            {
                
                var siteContent = await siteResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphSiteResponse>(siteContent);
                
                siteId = dto.Id;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {siteResponse.StatusCode}");
            }
            
            var driveResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/{siteId}/drives");

            string driveId;
            if (driveResponse.IsSuccessStatusCode)
            {
                
                var driveContent = await driveResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphDriveResponse>(driveContent);
                
                driveId = dto.Value.FirstOrDefault().Id;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {driveResponse.StatusCode}");
            }
            
            var folderListResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/drives/{driveId}/root:/General:/children/{wbsParameter.UploadWbsDocumentDto.Wbs}");

            string folderId = null;
            if (folderListResponse.IsSuccessStatusCode)
            {
                var folderList = await folderListResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<SiteListValue>(folderList);
                folderId = dto.Id;
            }
            
            if (folderId == null )
            {
                var jsonBody = $"{{\"name\": \"{wbsParameter.UploadWbsDocumentDto.Wbs}\", \"folder\": {{}}, \"@microsoft.graph.conflictBehavior\": \"rename\"}}";

                var folderCreateContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var createFolderResponse = await client.PostAsync($"https://graph.microsoft.com/v1.0/drives/{driveId}/root:/General:/children", folderCreateContent);
                
                var folderContent = await createFolderResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphSiteResponse>(folderContent);
                
                folderId = dto.Id;
                
                var docData = new WbsDocument()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectTitle = wbsParameter.UploadWbsDocumentDto.ProjectTitle,
                    Product = wbsParameter.UploadWbsDocumentDto.Product,
                    Wbs = wbsParameter.UploadWbsDocumentDto.Wbs,
                    FolderId = folderId
                };

                await connection.ExecuteAsync(
                    @"INSERT INTO dbo.WbsDocument ( Id ,ProjectTitle ,Product,Wbs,FolderId) VALUES ( @Id ,@ProjectTitle ,@Product,@Wbs,@FolderId);",
                    docData);
            }

            var data = new UploadWbsDocumentMetaDataDto()
            {
                SiteId = siteId,
                DriveId = driveId,
                FolderId = folderId
            };
            
            foreach (var doc in wbsParameter.doc)
            {
                string fileName = Path.GetFileName(doc.FileName);

                var requestUrl = $"https://graph.microsoft.com/v1.0/drives/{driveId}/root:/General/{wbsParameter.UploadWbsDocumentDto.Wbs}/{fileName}:/content";

                await using (var stream = doc.OpenReadStream())
                {
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var fileUploadResponse = await client.PutAsync(requestUrl, content);

                    if (fileUploadResponse.IsSuccessStatusCode)
                    {
                        var fileUploadContent = await fileUploadResponse.Content.ReadAsStringAsync();
                        var dto = JsonConvert.DeserializeObject<FileUploadResponse>(fileUploadContent);
                        data.SharepointFileId = dto.Id;
                        data.Link = dto.WebUrl;
                        
                        var docData = new WbsDocument()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectTitle = wbsParameter.UploadWbsDocumentDto.ProjectTitle,
                            Product = wbsParameter.UploadWbsDocumentDto.Product,
                            FileName = fileName,
                            FileUrl = dto.WebUrl,
                            Wbs = wbsParameter.UploadWbsDocumentDto.Wbs,
                            SiteId = siteId,
                            FileId = dto.Id
                        };

                        await connection.ExecuteAsync(
                            @"INSERT INTO dbo.WbsDocument ( Id ,ProjectTitle ,Product ,FileName ,FileUrl ,Wbs,SiteId ) VALUES ( @Id ,@ProjectTitle ,@Product ,@FileName ,@FileUrl ,@Wbs,@SiteId);",
                            docData);
                    }
                    else
                    {
                        throw new HttpRequestException($"Request failed with status code {fileUploadResponse.StatusCode}");
                    }
                }
            }
            
            return data;
        }
    }


    public async Task<UploadWbsDocumentMetaDataDto> UploadWbsDocumentMetaData(WbsParameter wbsParameter)
    {
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var project = connection
            .Query<ProjectDefinition>(@"SELECT * FROM dbo.ProjectDefinition WHERE Title = @ProjectTitle",
                wbsParameter.UploadWbsDocumentDto).FirstOrDefault();
        
        await using var pConnection = new SqlConnection(project?.ProjectConnectionString);

        var wbsParentSql = @"with name_tree as
                                                             (SELECT
                                                               WbsTaxonomy.Id
                                                              ,WbsTaxonomy.WbsTaxonomyLevelId
                                                              ,WbsTaxonomy.Title
                                                              ,WbsTaxonomy.ParentId
                                                             FROM dbo.WbsTaxonomy
                                                             WHERE WbsTaxonomy.Id = @WbsId
                                                               UNION ALL
                                                               SELECT c.Id, c.WbsTaxonomyLevelId,c.Title,c.ParentId
                                                               FROM dbo.WbsTaxonomy c
                                                               JOIN name_tree p on p.ParentId = c.Id)
                                                               select Id,Title,WbsTaxonomyLevelId,ParentId
                                                               from name_tree ";

        var wbsParentList = pConnection.Query<WbsTaxonomy>(wbsParentSql, wbsParameter.UploadWbsDocumentDto).ToList();

        var token =  UploadWbsDocumentGetToken(wbsParameter);
        
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

            var site = wbsParameter.UploadWbsDocumentDto.ProjectTitle.Replace(" ", "");
            var siteResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/uprince.sharepoint.com:/sites/{site}");

            string siteId;
            if (siteResponse.IsSuccessStatusCode)
            {
                
                var siteContent = await siteResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphSiteResponse>(siteContent);
                
                siteId = dto.Id;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {siteResponse.StatusCode}");
            }
            
            var driveResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/{siteId}/drives");

            string driveId;
            if (driveResponse.IsSuccessStatusCode)
            {
                
                var driveContent = await driveResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphDriveResponse>(driveContent);
                
                driveId = dto.Value.FirstOrDefault().Id;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {driveResponse.StatusCode}");
            }
            
            var folderListResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/drives/{driveId}/root:/General:/children/{wbsParameter.UploadWbsDocumentDto.Wbs}");

            string folderId = null;
            if (folderListResponse.IsSuccessStatusCode)
            {
                var folderList = await folderListResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<SiteListValue>(folderList);
                folderId = dto.Id;
            }
            
            if (folderId == null )
            {
                var jsonBody = $"{{\"name\": \"{wbsParameter.UploadWbsDocumentDto.Wbs}\", \"folder\": {{}}, \"@microsoft.graph.conflictBehavior\": \"rename\"}}";

                var folderCreateContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var createFolderResponse = await client.PostAsync($"https://graph.microsoft.com/v1.0/drives/{driveId}/root:/General:/children", folderCreateContent);
                
                var folderContent = await createFolderResponse.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<GraphSiteResponse>(folderContent);
                
                folderId = dto.Id;
                
                var docData = new WbsDocument()
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectTitle = wbsParameter.UploadWbsDocumentDto.ProjectTitle,
                    Product = wbsParameter.UploadWbsDocumentDto.Product,
                    Wbs = wbsParameter.UploadWbsDocumentDto.Wbs,
                    FolderId = folderId
                };

                await connection.ExecuteAsync(
                    @"INSERT INTO dbo.WbsDocument ( Id ,ProjectTitle ,Product,Wbs,FolderId) VALUES ( @Id ,@ProjectTitle ,@Product,@Wbs,@FolderId);",
                    docData);
            }
            
            var data = new UploadWbsDocumentMetaDataDto()
            {
                SiteId = siteId,
                DriveId = driveId,
                FolderId = folderId
            };

            return data;
        }
    }

    public async Task<UpdateWbsDocumentDto> UpdateWbsDocumentUploadData(WbsParameter wbsParameter)
    {
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        foreach (var i in wbsParameter.UpdateWbsDocumentDto.Url)
        {
            var docData = new WbsDocument()
            {
                Id = Guid.NewGuid().ToString(),
                ProjectTitle = wbsParameter.UpdateWbsDocumentDto.ProjectTitle,
                Product = wbsParameter.UpdateWbsDocumentDto.Product,
                FileName = i.Name,
                FileUrl = i.Url,
                FileId = i.Id,
                FileDownloadUrl = i.DownloadUrl,
                Wbs = wbsParameter.UpdateWbsDocumentDto.Wbs,
                FolderId = wbsParameter.UpdateWbsDocumentDto.FolderId,
                AttachmentId = i.AttachmentId,
                SiteId = wbsParameter.UpdateWbsDocumentDto.SiteId,
                MailId = i.MailId
            };

            await connection.ExecuteAsync(
                @"INSERT INTO dbo.WbsDocument ( Id ,ProjectTitle ,Product ,FileName ,FileUrl ,Wbs ,FolderId ,FileDownloadUrl ,FileId,AttachmentId,SiteId,MailId ) VALUES ( @Id ,@ProjectTitle ,@Product ,@FileName ,@FileUrl ,@Wbs ,@FolderId ,@FileDownloadUrl ,@FileId,@AttachmentId,@SiteId,@MailId);",
                docData);
        }

        return wbsParameter.UpdateWbsDocumentDto;

    }

    public async Task<List<WbsDocument>> GetWbsDocument(WbsParameter wbsParameter)
    {
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE ProjectTitle = @ProjectTitle AND Wbs = @Wbs AND Product = @Product",
            wbsParameter.UploadWbsDocumentDto).ToList();

        return wbsDoc;
    }

    public async Task<GetWbsDocumentIdByUrl> GetWbsDocumentIdByUrl(WbsParameter wbsParameter)
    {
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE AttachmentId = @AttachmentId",
            wbsParameter.GetWbsDocumentIdByUrlDto).FirstOrDefault();
        
        var token =  UploadWbsDocumentGetToken(wbsParameter);

        var mGetWbsDocumentIdByUrl = new GetWbsDocumentIdByUrl();
        if (wbsDoc != null)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);

                var siteResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/{wbsDoc.SiteId}/lists");

                if (siteResponse.IsSuccessStatusCode)
                {
                    var siteContent = await siteResponse.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<SiteList>(siteContent);

                    var listId = list.Value.Where(e => e.Name == "Shared Documents").Select(e => e.Id).FirstOrDefault();

                    if (listId != null)
                    {
                        var docResponse = await client.GetAsync($"https://graph.microsoft.com/v1.0/sites/{wbsDoc.SiteId}/lists/{listId}/items");

                        if (docResponse.IsSuccessStatusCode)
                        {
                            var docContent = await docResponse.Content.ReadAsStringAsync();
                            var dto = JsonConvert.DeserializeObject<SiteList>(docContent);

                            mGetWbsDocumentIdByUrl.ItemId = dto.Value.Where(e => e.WebUrl == wbsDoc.FileUrl)
                                .Select(e => e.Id).FirstOrDefault();
                            mGetWbsDocumentIdByUrl.SiteId = wbsDoc.SiteId;
                            mGetWbsDocumentIdByUrl.ListId = listId;
                        }

                    }

                }
            }
        }

        return mGetWbsDocumentIdByUrl;
    }

    public async Task<List<WbsDocument>> GetWbsDocumentIdByMailId(WbsParameter wbsParameter)
    {
       await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE MailId = @MailId",new{wbsParameter.UrlDto.MailId}).ToList();
        
        return wbsDoc;
    }

    public async Task<TaskDocDto> GetWbsDocumentIdByTaskId(WbsParameter wbsParameter)
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
        var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
        var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret);
        var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
        
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE FileUrl = @Url",new{wbsParameter.GetWbsDocumentIdByTaskId.Url}).FirstOrDefault();
        
        var data = new TaskDocDto();
        
        if(wbsDoc != null)
        {
            var siteResponse = await graphServiceClient.Sites[wbsDoc.SiteId].Lists.GetAsync();

            if (siteResponse.Value.Any())
            {
                var listId = siteResponse.Value.Where(e => e.Name == "Shared Documents").Select(e => e.Id).FirstOrDefault();

                if (listId != null)
                {
                    var docResponse = await graphServiceClient.Sites[wbsDoc.SiteId].Lists[listId].Items.GetAsync();

                    if (docResponse.Value.Any())
                    {
                            
                        var itemId = docResponse.Value.Where(e => e.WebUrl == wbsDoc.FileUrl)
                            .Select(e => e.Id).FirstOrDefault();
                            
                        var item = await graphServiceClient.Sites[wbsDoc.SiteId].Lists[listId].Items[itemId].GetAsync();
                        data.Fields = item.Fields;
                        
                        var columns = await graphServiceClient.Sites[wbsDoc.SiteId].Lists[listId].ContentTypes[item.ContentType.Id].Columns.GetAsync();

                        data.Columns = columns;
                    }
                }
            }
        }
        
        return data;
    }
    
    public async Task<string> ReadWbsDocumentIdByUrl(WbsParameter wbsParameter)
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
        var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
        var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret);
        var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
        
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);
        
        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE FileUrl = @Url",new{wbsParameter.GetWbsDocumentIdByTaskId.Url}).FirstOrDefault();
        
        if(wbsDoc != null)
        {
            var drives = await graphServiceClient.Sites[wbsDoc.SiteId].Drives.GetAsync();
            var driveId = drives.Value.FirstOrDefault(x => x.Name == "Documents").Id;
            
            var file = await graphServiceClient.Drives[driveId].Items[wbsDoc.FileId].Content.GetAsync();

            StreamReader reader = new StreamReader(file);
            string text = reader.ReadToEnd();

            return text;
        }

        else
        {
            throw new Exception("url not found");
        }
        
    }
    
    public async Task<ContentTypeCollectionResponse> GetContentTypes(WbsParameter wbsParameter)
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
        var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
        var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret);
        var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
        
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE ProjectTitle = @ProjectTitle",new{wbsParameter.GetWbsDocumentIdByTaskId.ProjectTitle}).FirstOrDefault();
        
        var data = new ContentTypeCollectionResponse();
        
        if(wbsDoc != null)
        {
            var list = await graphServiceClient.Sites[wbsDoc.SiteId].Lists.GetAsync();

            var listId = (list?.Value).FirstOrDefault(e => e.DisplayName == "Documents")?.Id;
            
            data = await graphServiceClient.Sites[wbsDoc.SiteId].Lists[listId].ContentTypes.GetAsync();

            //data.Content = await graphServiceClient.Drives[drive?.Id].Items["01ZUJSK2VABKGB7QOVDZAL7VN7T6EGEIYU"].Content.GetAsync();
        }
        
        return data;
    }

    public async Task<ListItem> UpdateFile(WbsParameter wbsParameter)
    {
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
        var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
        var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret);
        var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
        
        await using var connection = new SqlConnection(wbsParameter.TenantProvider.GetTenant().ConnectionString);

        var wbsDoc = connection.Query<WbsDocument>(
            @"SELECT * FROM dbo.WbsDocument WHERE FileUrl = @Url",new{wbsParameter.UpdateFileDto.Url}).FirstOrDefault();

        var responce = new ListItem();
        if(wbsDoc != null)
        {
            var list = await graphServiceClient.Sites[wbsDoc.SiteId].Lists.GetAsync();

            var listId = (list?.Value).FirstOrDefault(e => e.DisplayName == "Documents")?.Id;
            
            var item = await graphServiceClient.Sites[wbsDoc.SiteId].Lists[listId].Items.GetAsync();
            
            if (item.Value.Any())
            {

                var itemId = item.Value.Where(e => e.WebUrl == wbsDoc.FileUrl)
                    .Select(e => e.Id).FirstOrDefault();
                
                responce = await graphServiceClient.Sites[wbsDoc.SiteId].Lists[listId].Items[itemId].PatchAsync(wbsParameter.UpdateFileDto.ListItem);
            }

        }
        
        return responce;
    }
    private async Task<TokenRequest> UploadWbsDocumentGetToken(WbsParameter wbsParameter)
    {
        var _httpClient = new HttpClient();
        var requestURL = "https://login.microsoftonline.com/3d438826-fdde-4b8b-89d1-1b9b4feeaa20/oauth2/v2.0/token?";
        var content = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "client_id", "f9ec3629-065f-4065-9dee-f42c22ae74e5" },
            { "client_secret", "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd" },
            { "grant_type", "client_credentials" },
            { "scope", "https://graph.microsoft.com/.default" },
        });

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(requestURL))
        {
            Content = content
        };

        var token = new TokenRequest();
        using (var response = await _httpClient.SendAsync(httpRequestMessage))
        {
            var responseStream = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<TokenRequest>(responseStream);
        }
        return token;
    }
    
    public async Task<List<string>> WbsTaskDocumentsDelete(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);


        await connection.ExecuteAsync("Delete From WbsTaskDocuments Where Id IN @Ids", new { Ids = wbsParameter.IdList });

        return wbsParameter.IdList;
    }
    
    public async Task<List<string>> WbsProductDocumentsDelete(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId, wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);


        await connection.ExecuteAsync("Delete From WbsProductDocuments Where Id IN @Ids", new { Ids = wbsParameter.IdList });

        return wbsParameter.IdList;
    }
    
    public async Task<string> WbsTaskDateUpdate(WbsParameter wbsParameter)
    {
        wbsParameter.ContractingUnitSequenceId ??= "COM-0001";

        var connectionString = ConnectionString.MapConnectionString(wbsParameter.ContractingUnitSequenceId,
            wbsParameter.ProjectSequenceId,wbsParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(@"UPDATE dbo.WbsTask SET Date = @Date WHERE Id = @TaskId", wbsParameter.WbsTaskDateUpdateDto);

        return wbsParameter.WbsTaskDateUpdateDto.TaskId;
    }
    
    
    
    public class TokenRequest
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
    
    public class GraphSiteResponse
    {
        public string Id { get; set; }
    }
    
    public class SiteListValue
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string WebUrl { get; set; }
    }
    
    public class SiteList
    {
        public List<SiteListValue> Value { get; set; }
    }
    
    
    
    public class GraphDriveResponse
    {
        public List<GraphSiteResponse> Value { get; set; }
    }
    
    public class FileUploadResponse
    {
        public string WebUrl { get; set; }
        public string Id { get; set; }
    }
    
    

}