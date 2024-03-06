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

public class ProjectToleranceStateRepository : IProjectToleranceStateRepository
{
    public async Task<string> CreateProjectToleranceState(ApplicationDbContext context,
        ProjectToleranceStateCreateDto projectToleranceStateCreateDto)
    {
        throw new NotImplementedException();
    }

    public bool DeleteProjectToleranceState(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Currency> GetProjectToleranceStateById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProjectToleranceStateDto>> GetProjectToleranceStates(ApplicationDbContext context,
        string lang, ITenantProvider iTenantProvider)
    {

        var sql =
            "SELECT ProjectToleranceState.Name ,ProjectToleranceState.IsDefault ,ProjectToleranceState.ListingOrder ,ProjectToleranceState.LanguageCode" +
            " ,ProjectToleranceState.ProjectToleranceStateId AS Id FROM dbo.ProjectToleranceState WHERE " +
            "ProjectToleranceState.LanguageCode ='" + lang + "' ORDER BY ListingOrder";

        using IDbConnection dbConnection = new SqlConnection(iTenantProvider.GetTenant().ConnectionString);
        var result = iTenantProvider.orgSqlConnection().Query<ProjectToleranceStateDto>(sql).ToList();
        

        return result;
    }

    public async Task<string> UpdateProjectToleranceState(ApplicationDbContext context,
        ProjectToleranceStateCreateDto projectToleranceStateCreateDto)
    {
        throw new NotImplementedException();
    }
}