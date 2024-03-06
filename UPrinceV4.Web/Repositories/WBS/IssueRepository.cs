using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Issue;
using UPrinceV4.Web.Data.WBS;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.WBS;

public class IssueRepository : IIssueRepository
{
    public async Task<string> CreateIssue(IssueParameter issueParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(issueParameter.ContractingUnitSequenceId,
            issueParameter.ProjectSequenceId, issueParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        await using var dbConnection = new SqlConnection(issueParameter.TenantProvider.GetTenant().ConnectionString);

        var issue = connection
            .Query<string>(@"SELECT Id FROM dbo.IssueHeader WHERE Id = @Id", new { issueParameter.IssueHeader.Id })
            .FirstOrDefault();

        if (issue == null)
        {
            if (issueParameter.IssueHeader.Id == null)
            {
                if (issueParameter.IssueHeader.WbsId != null)
                {
                    var wbsTax = connection.Query<WbsTaxonomy>("Select * From WbsTaxonomy Where Id = @Id",
                        new { Id = issueParameter.IssueHeader.WbsId }).FirstOrDefault();

                    issueParameter.IssueHeader.Id = Guid.NewGuid().ToString();
                    
                    var taxonomyParam = new WbsTaxonomy()
                    {
                        Id = issueParameter.IssueHeader.Id,
                        Title = issueParameter.IssueHeader.Title,
                        ParentId = issueParameter.IssueHeader.WbsId,
                        CreatedBy = issueParameter.UserId,
                        CreatedDateTime = DateTime.UtcNow,
                        WbsTaxonomyLevelId = "pouoe52c0-hvkhl-lbjm-jvhj-1d2d241ouyi", //issue
                        TemplateId = wbsTax?.TemplateId
                    };

                    const string insertWbs =
                        @"INSERT INTO dbo.WbsTaxonomy ( Id ,WbsTaxonomyLevelId ,ParentId ,CreatedDateTime ,CreatedBy,SequenceId ,Title,IsDefault,TemplateId ) VALUES ( @Id ,@WbsTaxonomyLevelId ,@ParentId ,@CreatedDateTime ,@CreatedBy,@SequenceId ,@Title,@IsDefault,@TemplateId);";

                    await connection.ExecuteAsync(insertWbs, taxonomyParam);

                }
            }
            
            issueParameter.IssueHeader.CreatedBy = issueParameter.UserId;
                //issueParameter.IssueHeader.RaisedBy = issueParameter.UserId;
                issueParameter.IssueHeader.CreatedDateTime = DateTime.UtcNow;

                var sql =
                    @"INSERT INTO dbo.IssueHeader ( Id,Title ,Type ,Status ,Priority ,Severity ,DateRaised ,LastUpdate ,DecisionDate ,ClosureDate ,RaisedBy,CreatedBy,CreatedDateTime,Decision,Responsible,WbsId,ProductId ) VALUES ( @Id,@Title ,@Type ,@Status ,@Priority ,@Severity ,@DateRaised ,@LastUpdate ,@DecisionDate ,@ClosureDate ,@RaisedBy,@CreatedBy,@CreatedDateTime,@Decision,@Responsible,@WbsId,@ProductId);";

                await connection.ExecuteAsync(sql, issueParameter.IssueHeader);
                
                
            
        }

        else
        {
            const string insertWbs =
                @"Update dbo.WbsTaxonomy Set ParentId = @WbsId ,Title = @Title Where Id = @Id";

            await connection.ExecuteAsync(insertWbs, issueParameter.IssueHeader);
            
            issueParameter.IssueHeader.UpdatedBy = issueParameter.UserId;
            //issueParameter.IssueHeader.RaisedBy = issueParameter.UserId;
            issueParameter.IssueHeader.UpdatedDateTime = DateTime.UtcNow;

            var sql = @"UPDATE dbo.IssueHeader SET Id = @Id,Title = @Title ,Type = @Type ,Status = @Status ,Priority = @Priority ,Severity = @Severity ,DateRaised = @DateRaised ,LastUpdate = @LastUpdate ,DecisionDate = @DecisionDate ,ClosureDate = @ClosureDate ,RaisedBy = @RaisedBy,UpdatedByBy = @UpdatedBy,UpdatedDateTime = @UpdatedDateTime,Decision = @Decision,Responsible = @Responsible,WbsId = @WbsId,ProductId = @ProductId WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, issueParameter.IssueHeader);
            
            
        }

        if (issueParameter.IssueHeader.Tags != null)
        {
            await connection.ExecuteAsync(@"DELETE FROM dbo.IssueTags WHERE IssueId = @IssueId",new {IssueID = issueParameter.IssueHeader.Id });;
            foreach (var i in issueParameter.IssueHeader.Tags)
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO dbo.IssueTags ( Id ,Name ,IssueId ) VALUES ( @Id,@Name,@IssueID )",
                    new { Id = Guid.NewGuid(), i.Name, IssueID = issueParameter.IssueHeader.Id });
            }
        }
        
        if (issueParameter.IssueHeader.Documents != null)
        {
            await connection.ExecuteAsync(@"DELETE FROM dbo.IssueDocument WHERE IssueId = @IssueId",new {IssueID = issueParameter.IssueHeader.Id });
            foreach (var i in issueParameter.IssueHeader.Documents)
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO dbo.IssueDocument ( Id ,Url ,IssueId ) VALUES ( @Id,@Url,@IssueID )",
                    new { Id = Guid.NewGuid(), Url = i.Url, IssueID = issueParameter.IssueHeader.Id });
            }
        }
        
        const string ccInsert = @"INSERT INTO dbo.IssueCc ( Id ,PersonId ,IssueId ) VALUES ( @Id ,@PersonId ,@IssueId )";

        if (issueParameter.IssueHeader.Cc != null)
        {
            await connection.ExecuteAsync(@"DELETE FROM dbo.IssueCc WHERE IssueId = @Id",
                new { issueParameter.IssueHeader.Id });
            foreach (var cc in issueParameter.IssueHeader.Cc)
            {
                cc.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = cc.EmailAddress }).FirstOrDefault();
                
                var ccParam = new IssueCc
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = cc.PersonId,
                    IssueId = issueParameter.IssueHeader.Id
                };

                await connection.ExecuteAsync(ccInsert, ccParam);

            }
        }

        if (issueParameter.IssueHeader.ToPerson != null)
        {
            await connection.ExecuteAsync(@"DELETE FROM dbo.IssueTo WHERE IssueId = @Id",
                new { issueParameter.IssueHeader.Id });
            const string toInsert = @"INSERT INTO dbo.IssueTo ( Id ,PersonId ,IssueId ) VALUES ( @Id ,@PersonId ,@IssueId )";

            var increment = 0;
            foreach (var to in issueParameter.IssueHeader.ToPerson)
            {
                to.PersonId ??= dbConnection
                    .Query<string>(
                        "Select cp.Id From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
                        new { email = to.EmailAddress }).FirstOrDefault();
                
                var toParam = new IssueTo
                {
                    Id = Guid.NewGuid().ToString(),
                    PersonId = to.PersonId,
                    IssueId = issueParameter.IssueHeader.Id,
                    IsDefault = increment == 0
                };

                await connection.ExecuteAsync(toInsert, toParam);

                increment++;
            }
        }

        return issueParameter.IssueHeader.Id;
    }

    public async Task<List<IssueFilterResults>> IssueFilter(IssueParameter issueParameter)
    {
        await using var dbConnection = new SqlConnection(issueParameter.TenantProvider.GetTenant().ConnectionString);

        var userProjects = new List<UserProjectList>();
        var results = new List<IssueFilterResults>();

        
            var projSql = @"SELECT DISTINCT
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
                { lang = issueParameter.Lang, oid = issueParameter.UserId };

            var sbProj = new StringBuilder(projSql);

            if (issueParameter.ContractingUnitSequenceId == null && issueParameter.ProjectSequenceId == null)
            {
                if (issueParameter.IssueFilterDto.Project != null)
                {
                    sbProj.Append(" And ProjectDefinition.SequenceCode = '" + issueParameter.IssueFilterDto.Project + "' ");
                }
            }
            else
            {
                sbProj.Append(" And ProjectDefinition.SequenceCode = '" + issueParameter.ProjectSequenceId + "' ");
                
            }

            
            userProjects = dbConnection.Query<UserProjectList>(sbProj.ToString(), param).ToList();

            var cabPerson = dbConnection.Query<CabPerson>(@"SELECT * FROM dbo.CabPerson").ToList();
            
            var sql = @"SELECT
                  ih.Id
                 ,ih.DateRaised
                 ,ih.Title
                 ,ih.LastUpdate
                 ,ih.DecisionDate
                 ,ih.ClosureDate
                 ,ih.RaisedBy
                 ,ih.CreatedBy
                 ,ih.CreatedDateTime
                 ,ih.UpdatedByBy
                 ,ih.UpdatedDateTime
                 ,ih.ProductId
                 ,ih.Responsible
                 ,ih.Priority
                 ,ih.Severity
                 ,ih.Status
                 ,ih.Type
                 ,ip.Name AS PriorityName
                 ,[is].Name AS SeverityName
                 ,is1.Name AS StatusName
                 ,it.Name AS TypeName
                 ,pp.Title AS ProductTitle
                FROM dbo.IssueHeader ih 
                LEFT OUTER JOIN IssuePriority ip
                ON ih.Priority = ip.PriorityId
                LEFT OUTER JOIN IssueSeverity [is]
                ON ih.Severity = [is].SeverityId
                LEFT OUTER JOIN IssueStatus is1
                ON ih.Status = is1.StatusId
                LEFT OUTER JOIN IssueType it
                ON ih.Type = it.TypeId
                LEFT OUTER JOIN PbsProduct pp
                ON ih.ProductId = pp.ProductId
                WHERE (ip.LanguageCode = @lang OR ip.LanguageCode IS NULL)
                AND ([is].LanguageCode = @lang OR [is].LanguageCode IS NULL)
                AND (is1.LanguageCode = @lang OR is1.LanguageCode IS NULL)
                AND (it.LanguageCode = @lang OR it.LanguageCode IS NULL) ";

                var sb = new StringBuilder(sql);

                if (issueParameter.IssueFilterDto.Title != null)
                {
                    sb.Append("AND ih.Title LIKE '%" + issueParameter.IssueFilterDto.Title + "%' ");
                }
                
                if (issueParameter.IssueFilterDto.StartDate != null && issueParameter.IssueFilterDto.EndDate != null)
                {
                    sb.Append(" AND ih.DateRaised Between '" + issueParameter.IssueFilterDto.StartDate + "' AND '" + issueParameter.IssueFilterDto.EndDate + "' ");
                }

                if (issueParameter.IssueFilterDto.Status != null)
                {
                    sb.Append("AND ih.Status = '" + issueParameter.IssueFilterDto.Status + "' ");
                }

                if (issueParameter.IssueFilterDto.Severity != null)
                {
                    sb.Append("AND ih.Severity = '" + issueParameter.IssueFilterDto.Severity + "' ");
                }

                if (issueParameter.IssueFilterDto.Priority != null)
                {
                    sb.Append("AND ih.Priority = '" + issueParameter.IssueFilterDto.Priority + "' ");
                }

                if (issueParameter.IssueFilterDto.Type != null)
                {
                    sb.Append("AND ih.Type = '" + issueParameter.IssueFilterDto.Type + "' ");
                }

                if (issueParameter.IssueFilterDto.ProductTitle != null)
                {
                    sb.Append("AND pp.Title LIKE '%" + issueParameter.IssueFilterDto.ProductTitle + "%' ");
                }
                
                if (issueParameter.IssueFilterDto.Responsible != null)
                {
                    sb.Append("AND ih.Responsible = '" + issueParameter.IssueFilterDto.Responsible + "' ");
                }

                if (issueParameter.IssueFilterDto.Sorter.Attribute == null)
                {
                    sb.Append("ORDER BY ih.Title ASC");
                }
                

                if (issueParameter.IssueFilterDto.Sorter.Attribute != null)
                    switch (issueParameter.IssueFilterDto.Sorter.Attribute.ToLower())
                    {
                        case "title":
                            sb.Append("ORDER BY ih.Title");
                            break;
                        case "type":
                            sb.Append("ORDER BY it.Name " + issueParameter.IssueFilterDto.Sorter.Order);
                            break;
                        case "status":
                            sb.Append("ORDER BY is1.Name " + issueParameter.IssueFilterDto.Sorter.Order);
                            break;
                        case "severity":
                            sb.Append("ORDER BY [is].Name " + issueParameter.IssueFilterDto.Sorter.Order);
                            break;
                        case "priority":
                            sb.Append("ORDER BY ip.Name " + issueParameter.IssueFilterDto.Sorter.Order);
                            break;
                        case "producttitle":
                            sb.Append("ORDER BY pp.Title " + issueParameter.IssueFilterDto.Sorter.Order);
                            break;
                    }

            foreach (var projects in userProjects)
            {
                await using var connection = new SqlConnection(projects.ProjectConnectionString);
                
                var data = connection.Query<IssueFilterResults>(sb.ToString(), new { lang = issueParameter.Lang }).ToList();

                var issueDocs = connection.Query<IssueDocument>("Select * From IssueDocument").ToList();

                foreach (var i in data)
                {
                    i.ResponsibleName = cabPerson.Where(e => e.Id == i.Responsible).Select(e => e.FullName)
                        .FirstOrDefault();
                    
                    i.TotalAttachment = issueDocs.Count(x => x.IssueId == i.Id);
                    i.TotalMail = 0;
                    i.TotalInstructions = 0;
                    i.TotalCheckedInstructions = 0;
                    
                    var toList = connection.Query<IssueTo>(
                        "Select * From IssueTo Where IssueId = @IssueId AND PersonId IS NOT NULL",
                        new { IssueId = i.Id }).ToList();

                    foreach (var item in toList)
                    {
                        item.PersonName = cabPerson.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
                    }

                    i.ToPerson = toList;

                    var ccList = connection.Query<IssueCc>(
                        "Select * From IssueCc Where IssueId = @IssueId AND PersonId IS NOT NULL",
                        new { IssueId = i.Id }).ToList();

                    foreach (var item in ccList)
                    {
                        item.PersonName = cabPerson.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
                    }

                    i.Cc = ccList;
                    
                    i.ProjectSequenceId = projects.SequenceCode;
                    i.Cu = projects.Cu;
                    i.ProjectTitle = projects.Title;
                }
                
                results.AddRange(data);
            }

            return results;
    }

    public async Task<IssueHeaderCreateDto> IssueGetById(IssueParameter issueParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(issueParameter.ContractingUnitSequenceId,
            issueParameter.ProjectSequenceId, issueParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        var data = connection.Query<IssueHeaderCreateDto>(@"SELECT * FROM dbo.IssueHeader WHERE Id = @Id",new{issueParameter.Id,lang = issueParameter.Lang}).FirstOrDefault();
        
        await using var dbConnection = new SqlConnection(issueParameter.TenantProvider.GetTenant().ConnectionString);
        var person = dbConnection
            .Query<CabPersonIssueDto>(
                @"SELECT FullName,cpc.Oid,CabPerson.Id FROM dbo.CabPerson LEFT OUTER JOIN CabPersonCompany cpc ON CabPerson.Id = cpc.PersonId")
            .ToList();

        if (data != null)
        {
            data.RaisedByName = person.Where(e => e.Id == data.RaisedBy).Select(e => e.FullName).FirstOrDefault();
            data.ResponsibleName = person.Where(e => e.Id == data.Responsible).Select(e => e.FullName).FirstOrDefault();
            data.CreatedBy = person.Where(e => e.Oid == data.CreatedBy).Select(e => e.FullName).FirstOrDefault();
            if (data.UpdatedBy != null)
            {
                data.UpdatedBy = person.Where(e => e.Oid == data.UpdatedBy).Select(e => e.FullName).FirstOrDefault();
            }

            data.Documents = connection.Query<DocDto>(@"SELECT * FROM dbo.IssueDocument WHERE IssueId = @Id", new { issueParameter.Id }).ToList();
            data.Tags = connection.Query<TagDto>(@"SELECT * FROM dbo.IssueTags WHERE IssueId = @Id", new { issueParameter.Id }).ToList();
            
            var CcList = connection.Query<IssueCc>("Select * From IssueCc Where IssueId = @IssueId",
                new { IssueId = data.Id }).ToList();

            foreach (var item in CcList)
            {
                item.PersonName = person.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;
            }

            data.Cc = CcList;
            
            
            var toList = connection.Query<IssueTo>("Select * From IssueTo Where IssueId = @IssueId ORDER BY IsDefault DESC",
                new { IssueId = data.Id }).ToList();
            
            foreach (var item in toList)
            {
                item.PersonName = person.FirstOrDefault(x => x.Id == item.PersonId)?.FullName;

            }
            
            data.ToPerson = toList;

            var wbsParameter = new WbsParameter()
            {
                wbsTaskFilterDto = new WbsTaskFilterDto(),
                ProjectSequenceId = issueParameter.ProjectSequenceId,
                IssueGetById = data.Id,
                ContractingUnitSequenceId = issueParameter.ContractingUnitSequenceId,
                Lang = issueParameter.Lang,
                TenantProvider = issueParameter.TenantProvider,
                ContextAccessor = issueParameter.ContextAccessor
            };
            // wbsParameter.IsCheckList = true;
            data.Tasks  = await issueParameter.iWbsRepository.WbsTaskFilter(wbsParameter);
            
            wbsParameter.Id = data.Id;
            data.Conversations =  await issueParameter.iWbsRepository.GetWbsConversation(wbsParameter);

        }
        return data;
    }

    public async Task<IssueDropDownData> GetIssueDropDownData(IssueParameter issueParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(issueParameter.ContractingUnitSequenceId,
            issueParameter.ProjectSequenceId, issueParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);
        const string query =
            @"SELECT PriorityId [Key], Name as Text FROM dbo.IssuePriority WHERE LanguageCode = @lang ORDER BY DisplayOrder;
            SELECT StatusId [Key], Name as Text FROM dbo.IssueStatus WHERE LanguageCode = @lang ORDER BY DisplayOrder;
            SELECT SeverityId [Key], Name as Text FROM dbo.IssueSeverity WHERE LanguageCode = @lang ORDER BY DisplayOrder;
            SELECT TypeId [Key], Name as Text FROM dbo.IssueType WHERE LanguageCode = @lang ORDER BY DisplayOrder;";
        
        var mDropDownData= new IssueDropDownData();

        var multi = await connection.QueryMultipleAsync(query,new{lang = issueParameter.Lang});
        mDropDownData.Priority = multi.Read<IssueDropDownDto>();
        mDropDownData.Status = multi.Read<IssueDropDownDto>();
        mDropDownData.Severity = multi.Read<IssueDropDownDto>();
        mDropDownData.Type = multi.Read<IssueDropDownDto>();
        
        return mDropDownData;
    }

    public async Task<List<string>> DeleteIssueDocuments(IssueParameter issueParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(issueParameter.ContractingUnitSequenceId,
            issueParameter.ProjectSequenceId, issueParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await connection.ExecuteAsync("Delete From IssueDocument Where Id IN @Ids", new { Ids = issueParameter.IdList });

        return issueParameter.IdList;
        
    }

}