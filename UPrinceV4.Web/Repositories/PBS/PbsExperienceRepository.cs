using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class PbsExperienceRepository : IPbsExperienceRepository
{
    public IEnumerable<PbsExperienceDto> GetExperienceList(
        PbsExperienceRepositoryParameter pbsExperienceRepositoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            pbsExperienceRepositoryParameter.ContractingUnitSequenceId,
            pbsExperienceRepositoryParameter.ProjectSequenceId,
            pbsExperienceRepositoryParameter.TenantProvider);
        var lang = pbsExperienceRepositoryParameter.Lang;
        var query = @"
                              SELECT
                              PbsExperienceLocalizedData.PbsExperienceId AS [key]
                            ,PbsExperienceLocalizedData.Label AS Text
                            FROM dbo.PbsExperienceLocalizedData
                            WHERE PbsExperienceLocalizedData.LanguageCode = @lang
                            order by PbsExperienceLocalizedData.Label
                             ";

        var parameters = new { lang };
        using var dbConnection = new SqlConnection(connectionString);
        var result = dbConnection.Query<PbsExperienceDto>(query, parameters);
        
        return result;
    }
}