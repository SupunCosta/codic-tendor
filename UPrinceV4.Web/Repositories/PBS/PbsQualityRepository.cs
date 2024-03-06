using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class PbsQualityRepository : IPbsQualityRepository
{
    public async Task<string> CreatePbsQuality(PbsQualityParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        var sql = "select * from PbsQuality where PbsProductId = @PbsProductId AND QualityId = @QualityId ";

        var param = new
        {
            pbsParameters.PbsQualityCreateDto.PbsProductId, pbsParameters.PbsQualityCreateDto.QualityId
        };
        IEnumerable<PbsQuality> PbsQuality = null;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            PbsQuality = dbConnection.Query<PbsQuality>(sql, param);
            
        }

        var Id = Guid.NewGuid().ToString();
        if (!PbsQuality.Any())
        {
            var query = @"INSERT INTO dbo.PbsQuality VALUES
                                            (
                                              @Id
                                              ,@PbsProductId
                                               ,@QualityId
                                                )";
            await using (var connection = new SqlConnection(connectionString))
            {
                var affectedRows = connection.ExecuteAsync(query, new
                {
                    Id,
                    pbsParameters.PbsQualityCreateDto.PbsProductId,
                    pbsParameters.PbsQualityCreateDto.QualityId
                }).Result;
                connection.Close();
            }
        }

        return Id;
    }

    public async Task DeletePbsQuality(PbsQualityParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        foreach (var id in pbsParameters.IdList)
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                await dbConnection.ExecuteAsync("DELETE FROM dbo.PbsQuality WHERE Id = @id ", new { id });

                
            }
    }

    public async Task<IEnumerable<QualityDapperDto>> GetPbsQualityByPbsProductId(
        PbsQualityParameters pbsQualityParameters)
    {
        //var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            pbsQualityParameters.ContractingUnitSequenceId, pbsQualityParameters.ProjectSequenceId,
            pbsQualityParameters.TenantProvider);

        var sql = @"SELECT
  Quality.Id as Id
 ,Quality.Id as PbsQualityId
 ,PbsQuality.PbsProductId
 ,PbsQuality.QualityId
 ,Quality.IsSaved
 ,Quality.IsDeleted
 ,Quality.Name
 ,Quality.Criteria
 ,Quality.Tolerance
 ,Quality.Method
 ,Quality.Skills
 ,Quality.SequenceCode
 ,Quality.ProjectDefinitionId
 ,PbsQuality.Id  AS Uid
FROM dbo.PbsQuality
INNER JOIN dbo.Quality
  ON PbsQuality.QualityId = Quality.Id WHERE PbsQuality.PbsProductId = @PbsProductId";

        var param = new { pbsQualityParameters.PbsProductId };
        IEnumerable<QualityDapperDto> QualityDapperDtolist = null;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            QualityDapperDtolist = dbConnection.Query<QualityDapperDto>(sql, param);
            
        }

        return QualityDapperDtolist;
    }

    public async Task<IEnumerable<QualityDapperDto>> GetAllPbsQualityByPbsProductId(
        PbsQualityParameters pbsQualityParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsQualityParameters.ContractingUnitSequenceId,
            pbsQualityParameters.ProjectSequenceId, pbsQualityParameters.TenantProvider);
        var qualityDto = new List<QualityDapperDto>();
        var sql = @"with name_tree as 
                                   (
                                    select Id, PbsProductId, PbsTaxonomyNodeId
                                    from PbsProductTaxonomy
                                    where PbsProductId = @PbsProductId
                                    union all
                                    select c.Id, c.PbsProductId, c.PbsTaxonomyNodeId
                                    from PbsProductTaxonomy c
                                    join name_tree p on p.PbsTaxonomyNodeId = c.PbsProductId
                                    )
                                    select PbsTaxonomyNodeId
                                    from name_tree
                                    where PbsTaxonomyNodeId is not null ";

        var sql2 = @"SELECT
              Quality.Id as Id
             ,Quality.Id as PbsQualityId
             ,PbsQuality.PbsProductId
             ,PbsQuality.QualityId
             ,Quality.IsSaved
             ,Quality.IsDeleted
             ,Quality.Name
             ,Quality.Criteria
             ,Quality.Tolerance
             ,Quality.Method
             ,Quality.Skills
             ,Quality.SequenceCode
             ,Quality.ProjectDefinitionId
             ,PbsQuality.Id  AS Uid
            FROM dbo.PbsQuality
            INNER JOIN dbo.Quality
              ON PbsQuality.QualityId = Quality.Id WHERE PbsQuality.PbsProductId In @PbsProductId";

        var param = new { pbsQualityParameters.PbsProductId };
        var Idlist = new List<string>();
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            Idlist = dbConnection.Query<string>(sql, param).ToList();
            Idlist.Add(pbsQualityParameters.PbsProductId);
            qualityDto = dbConnection.Query<QualityDapperDto>(sql2, new { PbsProductId = Idlist }).ToList();

            
        }

        return qualityDto;
    }
}