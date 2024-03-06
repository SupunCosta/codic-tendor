using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class PbsSkillRepository : IPbsSkillRepository
{
    public IEnumerable<PbsSkillDto> GetSkillList(PbsSkillRepositoryParameter pbsSkillRepositoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            pbsSkillRepositoryParameter.ContractingUnitSequenceId,
            pbsSkillRepositoryParameter.ProjectSequenceId, pbsSkillRepositoryParameter.TenantProvider);
        var lang = pbsSkillRepositoryParameter.Lang;

        var query = @"
                              SELECT
                              PbsSkillLocalizedData.Label AS [Text]
                              ,PbsSkillLocalizedData.PbsSkillId AS [Key]
                              FROM dbo.PbsSkillLocalizedData
                              WHERE PbsSkillLocalizedData.LanguageCode = @lang
                              order by PbsSkillLocalizedData.Label
                             ";

        var parameters = new { lang };
        using var dbConnection = new SqlConnection(connectionString);
        var result = dbConnection.Query<PbsSkillDto>(query, parameters);
        
        return result;
    }
}