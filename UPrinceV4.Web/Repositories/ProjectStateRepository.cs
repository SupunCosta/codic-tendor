using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectStateRepository : IProjectStateRepository
{
    public async Task<string> CreateProjectState(ApplicationDbContext context,
        ProjectStateCreateDto projectStateCreateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteProjectState(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Currency> GetProjectStateById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProjectState>> GetProjectStates(ApplicationDbContext context, string lang,
        ITenantProvider iTenantProvider)
    {
        var sql =
            @"SELECT ProjectStateId AS Id ,Label AS Name , ProjectStateId FROM ProjectStateLocalizedData WHERE LanguageCode = @lang ORDER BY  DisplayOrder ASC";
        return iTenantProvider.orgSqlConnection().Query<ProjectState>(sql, new { lang });;
    }

    public async Task<string> UpdateProjectState(ApplicationDbContext context,
        ProjectStateCreateDto projectStateCreateDto)
    {
        throw new NotImplementedException();
    }
}