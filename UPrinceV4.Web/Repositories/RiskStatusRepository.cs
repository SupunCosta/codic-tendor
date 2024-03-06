using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class RiskStatusRepository : IRiskStatusRepository
{
    public async Task<IEnumerable<RiskStatus>> GetRiskStatusList(
        RiskStatusRepositoryParameter riskStatusRepositoryParameter)
    {
        var lang = riskStatusRepositoryParameter.Lang;
        var sql =
            "SELECT RiskStatus.Status ,RiskStatus.RiskStatusId AS Id ,RiskStatus.RiskStatusId AS RiskStatusId FROM dbo.RiskStatus WHERE RiskStatus.LanguageCode ='" +
            lang + "'";

        using IDbConnection dbConnection =
            new SqlConnection(riskStatusRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
        var result = dbConnection.QueryAsync<RiskStatus>(sql).Result;
        

        return result;
    }

    public async Task<RiskStatus> GetRiskStatusById(RiskStatusRepositoryParameter riskStatusRepositoryParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddRiskStatus(RiskStatusRepositoryParameter riskStatusRepositoryParameter)
    {
        var dbContext = riskStatusRepositoryParameter.ApplicationDbContext;
        riskStatusRepositoryParameter.RiskStatus.Id = Guid.NewGuid().ToString();
        dbContext.Add(riskStatusRepositoryParameter.RiskStatus);
        await dbContext.SaveChangesAsync();
        return riskStatusRepositoryParameter.RiskStatus.Id;
    }

    public bool DeleteRiskStatus(RiskStatusRepositoryParameter riskStatusRepositoryParameter)
    {
        throw new NotImplementedException();
    }
}