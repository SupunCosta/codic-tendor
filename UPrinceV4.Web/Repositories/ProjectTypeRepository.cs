using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectTypeRepository : IProjectTypeRepository
{
    public async Task<string> CreateProjectType(ApplicationDbContext context,
        ProjectTypeCreateDto projectTypeCreateDto)
    {
        throw new NotImplementedException();
    }

    public bool DeleteProjectType(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Currency> GetProjectTypeById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProjectTypeDto>> GetProjectTypes(ApplicationDbContext context, string lang,
        ITenantProvider iTenantProvider)
    {
        
        var sql =
            "SELECT ProjectType.Name ,ProjectType.IsDefault ,ProjectType.LanguageCode ,ProjectType.ProjectTypeId AS Id FROM dbo.ProjectType WHERE ProjectType.LanguageCode = @lang order by Name";
        return iTenantProvider.orgSqlConnection().Query<ProjectTypeDto>(sql, new {lang = lang}).ToList();;
    }

    public async Task<string> UpdateProjectType(ApplicationDbContext context,
        ProjectTypeCreateDto projectTypeCreateDto)
    {
        throw new NotImplementedException();
    }
}