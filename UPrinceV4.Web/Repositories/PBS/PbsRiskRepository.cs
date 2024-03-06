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

public class PbsRiskRepository : IPbsRiskRepository
{
    public async Task<string> CreatePbsRisk(PbsRiskParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        var sql = "select * from PbsRisk where PbsProductId = @PbsProductId AND RiskId = @RiskId ";

        var param = new
        {
            pbsParameters.PbsRiskCreateDto.PbsProductId, pbsParameters.PbsRiskCreateDto.RiskId
        };
        IEnumerable<PbsRisk> PbsRisk = null;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            PbsRisk = dbConnection.Query<PbsRisk>(sql, param);
            
        }

        string Id = null;
        if (!PbsRisk.Any())
        {
            var query = @"INSERT INTO dbo.PbsRisk VALUES
                                            (
                                              @Id
                                              ,@PbsProductId
                                               ,@RiskId
                                                )";
            using (var connection = new SqlConnection(connectionString))
            {
                var affectedRows = connection.ExecuteAsync(query, new
                {
                    Id = Guid.NewGuid().ToString(),
                    pbsParameters.PbsRiskCreateDto.PbsProductId,
                    pbsParameters.PbsRiskCreateDto.RiskId
                }).Result;
                connection.Close();
            }
        }

        return Id;
    }

    public async Task DeletePbsRisk(PbsRiskParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        foreach (var id in pbsParameters.IdList)
            using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Execute("DELETE FROM PbsRisk WHERE Id = @id ", new { id });

                
            }
    }

    public async Task<IEnumerable<RiskReadDapperDto>> GetPbsRiskByPbsProductId(PbsRiskParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var lang = pbsParameters.Lang;

        var sql = @"SELECT
                              PbsRisk.Id AS PbsRiskId
                             ,Risk.Name AS Name
                             ,Risk.Id AS ID
                             ,Risk.PersonId AS CabPersonId
                             ,Risk.RiskDetails AS RiskDetails
                             ,Risk.SequenceCode AS SequenceCode
                             ,RiskType.RiskTypeId AS [Key]
                             ,RiskType.Type AS Text
                             ,RiskStatus.RiskStatusId AS [Key]
                             ,RiskStatus.Status AS Text
                            FROM dbo.PbsRisk
                            INNER JOIN dbo.Risk
                              ON PbsRisk.RiskId = Risk.Id
                            LEFT OUTER JOIN dbo.RiskType
                              ON Risk.RiskTypeId = RiskType.RiskTypeId
                            LEFT OUTER JOIN dbo.RiskStatus 
                              ON Risk.RiskStatusId = RiskStatus.RiskStatusId
                            WHERE (RiskType.LanguageCode = @lang
                            OR Risk.RiskTypeId IS NULL) AND
                              (RiskStatus.LanguageCode = @lang
                            OR Risk.RiskStatusId IS NULL)
                            AND PbsRisk.PbsProductId = @id";


        var parameters = new { lang, id = pbsParameters.PbsProductId };
        IEnumerable<RiskReadDapperDto> pbsRisks = null;

        using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            pbsRisks = await dbConnection
                .QueryAsync<RiskReadDapperDto, RiskTypeDapperDto,
                    RiskStatusDapperDto, RiskReadDapperDto>
                (sql, (riskReadDapperDto, riskTypeDapperDto, riskStatusDapperDto) =>
                {
                    riskReadDapperDto.RiskType = riskTypeDapperDto;
                    riskReadDapperDto.RiskStatus = riskStatusDapperDto;
                    return riskReadDapperDto;
                }, splitOn: "Key,Key", param: parameters);

            
        }

        if (pbsRisks.FirstOrDefault() != null)
        {
            var sqlPerson = @"SELECT CabPerson.Id , CabPerson.FullName
                               FROM CabPerson
                               where CabPerson.Id =@Id";


            IEnumerable<RiskPersonDto> RiskPersonDtoList = null;
            foreach (var rissk in pbsRisks)
            {
                var parameters1 = new { id = rissk.CabPersonId };
                using (var dbConnection =
                       new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString))
                {
                    await dbConnection.OpenAsync();
                    RiskPersonDtoList = await dbConnection.QueryAsync<RiskPersonDto>(sqlPerson, parameters1);
                    

                    rissk.Person = RiskPersonDtoList.FirstOrDefault();
                }
            }
        }

        return pbsRisks;
    }

    public async Task<IEnumerable<RiskReadDapperDto>> GetAllPbsRiskByPbsProductId(PbsRiskParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var riskDto =
            (List<RiskReadDapperDto>)await GetPbsRiskByPbsProductId(pbsParameters);
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

        var param = new { pbsParameters.PbsProductId };
        IEnumerable<string> Idlist = null;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            Idlist = dbConnection.Query<string>(sql, param);
            
        }

        foreach (var id in Idlist)
        {
            pbsParameters.PbsProductId = id;
            var dto = await GetPbsRiskByPbsProductId(pbsParameters);
            riskDto.AddRange(dto);
        }

        return riskDto;
    }
}