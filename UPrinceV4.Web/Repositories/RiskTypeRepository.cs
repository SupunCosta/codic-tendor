using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class RiskTypeRepository : IRiskTypeRepository
{
    public async Task<IEnumerable<RiskType>> GetRiskTypeList(
        RiskTypeRepositoryParameter riskTypeRepositoryParameter)
    {
        var sql =
            "SELECT RiskType.Type ,RiskType.RiskTypeId AS Id ,RiskType.RiskTypeId AS RiskTypeId FROM dbo.RiskType WHERE RiskType.LanguageCode ='" +
            riskTypeRepositoryParameter.Lang + "'";

        using IDbConnection dbConnection =
            new SqlConnection(riskTypeRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
        var result = dbConnection.QueryAsync<RiskType>(sql).Result;
        

        return result;
    }

    public async Task<RiskType> GetRiskTypeById(RiskTypeRepositoryParameter riskTypeRepositoryParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddRiskType(RiskTypeRepositoryParameter riskTypeRepositoryParameter)
    {
        var dbContext = riskTypeRepositoryParameter.ApplicationDbContext;
        riskTypeRepositoryParameter.RiskType.Id = Guid.NewGuid().ToString();
        dbContext.Add(riskTypeRepositoryParameter.RiskType);
        await dbContext.SaveChangesAsync();
        return riskTypeRepositoryParameter.RiskType.Id;
    }

    public bool DeleteRiskType(RiskTypeRepositoryParameter riskTypeRepositoryParameter)
    {
        throw new NotImplementedException();
    }
}