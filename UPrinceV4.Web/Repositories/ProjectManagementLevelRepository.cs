using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectManagementLevelRepository : IProjectManagementLevelRepository
{
    public async Task<string> CreateProjectManagementLevel(ApplicationDbContext context,
        ProjectManagementLevelCreateDto projectManagementLevelCreateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteProjectManagementLevel(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Currency> GetProjectManagementLevelById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProjectManagementLevelDto>> GetProjectManagementLevels(
        ApplicationDbContext context, string lang, ITenantProvider iTenantProvider)
    {
        var sql =
            "SELECT ProjectManagementLevel.Name AS Name ,ProjectManagementLevel.IsDefault AS IsDefault ,ProjectManagementLevel.ListingOrder AS ListingOrder" +
            " ,ProjectManagementLevel.ProjectManagementLevelId AS Id FROM dbo.ProjectManagementLevel WHERE " +
            "ProjectManagementLevel.LanguageCode =@lang ORDER BY ListingOrder";
        
        return iTenantProvider.orgSqlConnection().Query<ProjectManagementLevelDto>(sql , new {lang = lang}).ToList();
    }

    public async Task<string> UpdateProjectManagementLevel(ApplicationDbContext context,
        ProjectManagementLevelCreateDto projectManagementLevelCreateDto)
    {
        throw new NotImplementedException();
    }
}