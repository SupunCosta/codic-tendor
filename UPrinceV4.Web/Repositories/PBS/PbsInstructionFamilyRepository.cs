using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class PbsInstructionFamilyRepository : IPbsInstructionFamilyRepository
{
    public IEnumerable<PbsInstructionFamily> GetInstructionFamilyList(
        PbsInstructionFamilyRepositoryParameter pbsInstructionFamilyParameter)
    {
        var lang = pbsInstructionFamilyParameter.Lang;
        if (string.IsNullOrEmpty(pbsInstructionFamilyParameter.Lang) ||
            pbsInstructionFamilyParameter.Lang.Contains("en"))
            lang = "en";

        var sql = @"SELECT
                               PbsInstructionFamilyLocalizedData.InstructionFamilyID AS Id
                              ,PbsInstructionFamilyLocalizedData.Family
                              ,PbsInstructionFamilyLocalizedData.Type
                              FROM dbo.PbsInstructionFamilyLocalizedData
                              WHERE PbsInstructionFamilyLocalizedData.LocaleCode = @lang
                              order by PbsInstructionFamilyLocalizedData.Family
                                ";
        var param = new { lang };
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionFamilyParameter.ContractingUnitSequenceId,
            pbsInstructionFamilyParameter.ProjectSequenceId, pbsInstructionFamilyParameter.TenantProvider);
        using var dbConnection = new SqlConnection(connectionString);
        var result = dbConnection.Query<PbsInstructionFamily>(sql, param);
        

        return result;
    }
}