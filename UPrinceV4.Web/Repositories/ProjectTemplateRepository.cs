using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectTemplateRepository : IProjectTemplateRepository
{
    public async Task<string> CreateProjectTemplate(ApplicationDbContext context,
        ProjectTemplateCreateDto projectTemplateCreateDto)
    {
        throw new NotImplementedException();
    }

    public bool DeleteProjectTemplate(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Currency> GetProjectTemplateById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProjectTemplateDto>> GetProjectTemplates(ApplicationDbContext context,
        string lang, ITenantProvider iTenantProvider)
    {
        var sql =
            "SELECT  ProjectTemplate.Name ,ProjectTemplate.LanguageCode ,ProjectTemplate.TemplateId AS Id FROM dbo.ProjectTemplate WHERE ProjectTemplate.LanguageCode ='" +
            lang + "' order by ProjectTemplate.Name";
        return iTenantProvider.orgSqlConnection().Query<ProjectTemplateDto>(sql).ToList();;
    }

    public async Task<string> UpdateProjectTemplate(ApplicationDbContext context,
        ProjectTemplateCreateDto projectTemplateCreateDto)
    {
        throw new NotImplementedException();
    }
}