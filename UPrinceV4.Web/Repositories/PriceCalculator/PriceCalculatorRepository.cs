using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.PriceCalculator;
using UPrinceV4.Web.Repositories.Interfaces.PriceCalaculator;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PriceCalculator;

public class PriceCalculatorRepository : IPriceCalculatorRepository
{
    public async Task<string> CreatePriceCalculatorTaxonomy(PriceCalculatorParameter priceCalculatorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(priceCalculatorParameter.ContractingUnitSequenceId,
            priceCalculatorParameter.ProjectSequenceId, priceCalculatorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var taxonomy =
            connection.Query<PriceCalculatorTaxonomy>("SELECT * FROM PriceCalculatorTaxonomy Where Id = @Id",
                new { priceCalculatorParameter.CreatePriceCalculatorTaxonomy.Id }).FirstOrDefault();


        if (taxonomy == null)
        {
            var insertQuery =
                @"INSERT INTO dbo.PriceCalculatorTaxonomy ( Id ,Title ,ParentId ,PriceCalculatorTaxonomyLevelId ,Value ,[Order] ) VALUES ( @Id ,@Title ,@ParentId ,@PriceCalculatorTaxonomyLevelId ,@Value ,@Order )";

            await connection.ExecuteAsync(insertQuery, priceCalculatorParameter.CreatePriceCalculatorTaxonomy);
        }
        else
        {
            var updateQuery =
                "Update PriceCalculatorTaxonomy Set Title  = @Title, ParentId = @ParentId ,PriceCalculatorTaxonomyLevelId  = @PriceCalculatorTaxonomyLevelId, Value = @Value, [Order] = @Order Where Id = @Id";

            await connection.ExecuteAsync(updateQuery, priceCalculatorParameter.CreatePriceCalculatorTaxonomy);
        }


        return priceCalculatorParameter.CreatePriceCalculatorTaxonomy.Id;
    }

    public async Task<List<PriceCalculatorTaxonomy>> GetPriceCalculatorTaxonomy(
        PriceCalculatorParameter priceCalculatorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(priceCalculatorParameter.ContractingUnitSequenceId,
            priceCalculatorParameter.ProjectSequenceId, priceCalculatorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var taxonomy =
            connection.Query<PriceCalculatorTaxonomy>("SELECT * FROM PriceCalculatorTaxonomy order by [Order] Asc")
                .ToList();

        return taxonomy;
    }

    public async Task<string> DeletePriceCalculatorTaxonomy(PriceCalculatorParameter priceCalculatorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(priceCalculatorParameter.ContractingUnitSequenceId,
            priceCalculatorParameter.ProjectSequenceId, priceCalculatorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var param = new { priceCalculatorParameter.Id };

        var allData = connection.Query<string>(@"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM PriceCalculatorTaxonomy
                                                                            WHERE Id = @Id                                                                         
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM PriceCalculatorTaxonomy t
                                                                            INNER JOIN ret r
                                                                            ON t.ParentId = r.Id
                                                                           )
                                                                            SELECT
                                                                            Id
                                                                            FROM ret", param);

        var sql = "DELETE FROM PriceCalculatorTaxonomy WHERE Id = @Id";

        foreach (var item in allData)
        {
            var taxonomy =
                connection.Query<PriceCalculatorTaxonomy>(sql, new { Id = item }).ToList();
        }


        return priceCalculatorParameter.Id;
    }

    public async Task<List<GetPriceCalculatorTaxonomyLevel>> GetPriceCalculatorTaxonomyLevels(
        PriceCalculatorParameter priceCalculatorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(priceCalculatorParameter.ContractingUnitSequenceId,
            priceCalculatorParameter.ProjectSequenceId, priceCalculatorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var taxonomyLevels =
            connection.Query<GetPriceCalculatorTaxonomyLevel>(
                "SELECT LevelId As [Key] , Name As Text FROM PriceCalculatorTaxonomyLevel Where LanguageCode = @lang order by DisplayOrder Asc",
                new { lang = priceCalculatorParameter.Lang }).ToList();

        return taxonomyLevels;
    }
}