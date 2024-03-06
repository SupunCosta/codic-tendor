using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class PbsRepository : IPbsRepository
{
    public Task<List<ShortcutPaneData>> getShortcutPaneData(PbsParameters pbsParameters)
    {
        var typeList = GetProductItemType(pbsParameters).Result;
        var statusList = GetProductStatus(pbsParameters).Result;

        var list = new List<ShortcutPaneData>();

        foreach (var type in typeList)
        {
            var item1 = new ShortcutPaneData();
            item1.Id = type.Id;
            item1.Name = type.Name;
            item1.Type = "pbsProductItemTypeId";
            list.Add(item1);
        }

        foreach (var status in statusList)
        {
            var item2 = new ShortcutPaneData();
            item2.Id = status.Id;
            item2.Name = status.Name;
            item2.Type = "pbsProductStatusId";
            list.Add(item2);
        }

        var item = new ShortcutPaneData();
        item.Id = "0";
        item.Name = "inScope";
        item.Type = "scope";
        list.Add(item);

        var item3 = new ShortcutPaneData();
        item3.Id = "1";
        item3.Name = "outofScope";
        item3.Type = "scope";

        list.Add(item3);

        var itemT = new ShortcutPaneData();
        itemT.Id = "3";
        itemT.Name = "utilityTaxonomy";
        itemT.Type = "taxonomy";
        list.Add(itemT);

        var itemT2 = new ShortcutPaneData();
        itemT2.Id = "4";
        itemT2.Name = "locationTaxonomy";
        itemT2.Type = "taxonomy";

        list.Add(itemT2);

        var itemT3 = new ShortcutPaneData();
        itemT3.Id = "5";
        itemT3.Name = "ProductTaxonomy";
        itemT3.Type = "taxonomy";

        list.Add(itemT3);

        return Task.FromResult(list);
    }

    public async Task<PbsProduct> CreatePbs(PbsParameters parameters)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
                parameters.ProjectSequenceId, parameters.TenantProvider);

            var dbConnection = new SqlConnection(parameters.TenantProvider.GetTenant().ConnectionString);
            
            var sql = "select * from PbsProduct where Id = @Id";

            var param = new { parameters.PbsDto.Id };
            IEnumerable<PbsProduct> products = null;
            await using (var connection = new SqlConnection(connectionString))
            {
                products = connection.Query<PbsProduct>(sql, param);
            }


            var jsonProduct = JsonConvert.SerializeObject(parameters.PbsDto, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var historyLog = new PbsHistoryLog
            {
                Id = Guid.NewGuid().ToString(),
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = parameters.ChangedUser.OId,
                HistoryLog = jsonProduct
            };
            
            if (!products.Any())
            {
                // var PbsQualityResponsibilityId = CreatePbsQualityResponsibility(parameters);
                String PbsQualityResponsibilityId = null;
                var idGenerator = new IdGenerator();

                var createsql =
                    @"INSERT INTO [dbo].[PbsProduct] VALUES (@Id ,@ProductId ,@Name ,@PbsProductItemTypeId ,@PbsProductStatusId ,@Scope ,@Contract ,@ProductPurpose ,@ProductComposition ,@ProductDerivation ,@ProductFormatPresentation ,@PbsToleranceStateId ,@PbsQualityResponsibilityId ,@IsDeleted ,@NodeType ,@PbsTaxonomyLevelId ,@Title ,@PbsType ,@ProjectSequenceCode ,@ParentId ,@EndDate ,@StartDate ,@MachineTaxonomy ,@Quantity ,@MeasurementCode ,@Mou ,@QualityProducerId ,@WbsTaxonomyId,@PbsLotId,@PbsLocation)";
                string productId;
                if (parameters.PbsDto.ArticleNumber == null)
                {
                    var options = new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext =
                        new ApplicationDbContext(options, parameters.TenantProvider);
                    productId = idGenerator.GenerateId(applicationDbContext, "PBS-", "PbsSequenceCode");
                }

                else
                    productId = parameters.PbsDto.ArticleNumber;

                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(createsql, new
                    {
                        parameters.PbsDto.Id,
                        ProductId = productId,
                        parameters.PbsDto.Name,
                        parameters.PbsDto.PbsProductItemTypeId,
                        parameters.PbsDto.PbsProductStatusId,
                        parameters.PbsDto.Scope,
                        parameters.PbsDto.Contract,
                        parameters.PbsDto.ProductPurpose,
                        parameters.PbsDto.ProductComposition,
                        parameters.PbsDto.ProductDerivation,
                        parameters.PbsDto.ProductFormatPresentation,
                        parameters.PbsDto.PbsToleranceStateId,
                        PbsQualityResponsibilityId,
                        IsDeleted = 0,
                        NodeType = "P",
                        PbsTaxonomyLevelId = "93d01fef-ccc7-4592-9544-3949d67cbf76",
                        Title = productId + " - " + parameters.PbsDto.Name,
                        parameters.PbsDto.PbsType,
                        ProjectSequenceCode = parameters.ProjectSequenceId,
                        parameters.PbsDto.ParentId,
                        parameters.PbsDto.StartDate,
                        parameters.PbsDto.EndDate,
                        MachineTaxonomy = parameters.PbsDto.MachineTaxonomyParentId,
                        parameters.PbsDto.Quantity,
                        parameters.PbsDto.MeasurementCode,
                        parameters.PbsDto.Mou,
                        QualityProducerId = parameters.PbsDto.QualityProducerId,
                        parameters.PbsDto.WbsTaxonomyId,
                        PbsLotId = parameters.PbsDto.PbsLotId,
                        parameters.PbsDto.PbsLocation
                    });
                    connection.Close();
                }

                historyLog.PbsProductId = parameters.PbsDto.Id;
                historyLog.Action = HistoryState.ADDED.ToString();

                await AddPbsTreeIndex(productId, connectionString);
            }

            else
            {
                await using (var connection = new SqlConnection(connectionString))
                {
                    var pbsproduct = products.FirstOrDefault();

                    if (pbsproduct.ProductId != parameters.PbsDto.ParentId)
                    {
                        var updateSql =
                            @"update PbsProduct set Name = @Name ,PbsProductItemTypeId = @PbsProductItemTypeId ,PbsProductStatusId = @PbsProductStatusId ,Scope = @Scope ,ProductPurpose = @ProductPurpose ,ProductComposition = @ProductComposition ,ProductDerivation = @ProductDerivation ,ProductFormatPresentation = @ProductFormatPresentation ,PbsToleranceStateId = @PbsToleranceStateId ,PbsQualityResponsibilityId = @PbsQualityResponsibilityId ,PbsType = @PbsType ,ParentId = @ParentId ,StartDate = @StartDate ,EndDate = @EndDate ,MachineTaxonomy = @MachineTaxonomy ,Quantity = @Quantity ,MeasurementCode = @MeasurementCode ,Mou = @Mou ,Contract = @Contract ,Title = @Title ,QualityProducerId = @QualityProducerId ,WbsTaxonomyId = @WbsTaxonomyId, PbsLotId = @PbsLotId,PbsLocation = @PbsLocation where Id = @Id";
                        await connection.ExecuteAsync(updateSql, new
                        {
                            parameters.PbsDto.Id,
                            parameters.PbsDto.Name,
                            parameters.PbsDto.PbsProductItemTypeId,
                            parameters.PbsDto.PbsProductStatusId,
                            parameters.PbsDto.Scope,
                            parameters.PbsDto.Contract,
                            parameters.PbsDto.ProductPurpose,
                            parameters.PbsDto.ProductComposition,
                            parameters.PbsDto.ProductDerivation,
                            parameters.PbsDto.ProductFormatPresentation,
                            parameters.PbsDto.PbsToleranceStateId,
                            Title = pbsproduct.ProductId + " - " + parameters.PbsDto.Name,
                            parameters.PbsDto.PbsType,
                            parameters.PbsDto.ParentId,
                            PbsQualityResponsibilityId = CreatePbsQualityResponsibility(parameters),
                            parameters.PbsDto.StartDate,
                            parameters.PbsDto.EndDate,
                            MachineTaxonomy = parameters.PbsDto.MachineTaxonomyParentId,
                            parameters.PbsDto.Quantity,
                            parameters.PbsDto.MeasurementCode,
                            parameters.PbsDto.Mou,
                            QualityProducerId = parameters.PbsDto.QualityProducerId,
                            parameters.PbsDto.WbsTaxonomyId,
                            PbsLotId = parameters.PbsDto.PbsLotId,
                            parameters.PbsDto.PbsLocation
                        });
                    }

                    await connection.ExecuteAsync(@"UPDATE dbo.WbsTaxonomy SET Title = @Title WHERE Id = @Id;",
                        new { parameters.PbsDto.Id, Title = parameters.PbsDto.Name });
                }

                historyLog.PbsProductId = parameters.PbsDto.Id;
                historyLog.Action = HistoryState.UPDATED.ToString();
            }

            if (parameters.PbsDto.PbsProductStatusId == "4010e768-3e06-4702-b337-ee367a82addb")
            {
                var borUpdate =
                    @"UPDATE dbo.Bor SET BorStatusId = @BorStatusId WHERE PbsProductId= @PbsProductId";

                var param2 = new
                {
                    PbsProductId = parameters.PbsDto.Id,
                    BorStatusId = "4010e768-3e06-4702-b337-ee367a82addb"
                };

                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(borUpdate, param2);
                    // connection.Close();
                }
            }

            // var projectCon = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            //     parameters.ProjectSequenceId, parameters.TenantProvider);
            //var projectConOptions = new DbContextOptions<ShanukaDbContext>();
            // var projApplicationDbContext =
            //     new ShanukaDbContext(projectConOptions, connectionString, parameters.TenantProvider);
            await using (var connection = new SqlConnection(connectionString))
            {
                // projApplicationDbContext.PbsHistoryLog.Add(historyLog);
                // projApplicationDbContext.SaveChanges();
                var history =
                    @"INSERT INTO dbo.PbsHistoryLog ( Id ,HistoryLog ,ChangedByUserId ,Action ,ChangedTime ,PbsProductId ) VALUES ( @Id ,@HistoryLog ,@ChangedByUserId ,@Action ,@ChangedTime ,@PbsProductId );";
                await connection.ExecuteAsync(history, historyLog);
                // connection.Close();
            }


            CreatePbsProductTaxonomy(parameters, parameters.PbsDto.Id, true);


            await using var dbConnection1 = new SqlConnection(connectionString);

            var data = dbConnection1.Query<PbsProduct>(sql, param).FirstOrDefault();
            var iPbsResourceRepository = new PbsResourceRepository();
            var pbsResParam = new PbsResourceParameters()
            {
                ContractingUnitSequenceId = parameters.ContractingUnitSequenceId,
                ProjectSequenceId = parameters.ProjectSequenceId,
                TenantProvider = parameters.TenantProvider,
                ContextAccessor = parameters.ContextAccessor,
                Lang = parameters.Lang,
                PbsProductId = data?.ProductId
            };
            await iPbsResourceRepository.PbsParentDateAdjust(pbsResParam);

            return data;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }


    public async Task<string> CreatePbsNew(PbsParameters parameters)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, parameters.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            parameters.ProjectSequenceId, parameters.TenantProvider);

        var sql = "select * from PbsProduct where Id = @Id";

        var jsonProduct = JsonConvert.SerializeObject(parameters.PbsDto, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        var historyLog = new PbsHistoryLog
        {
            Id = Guid.NewGuid().ToString(),
            ChangedTime = DateTime.UtcNow,
            ChangedByUserId = parameters.ChangedUser.OId,
            HistoryLog = jsonProduct
        };


        var param = new { parameters.PbsDto.Id };
        PbsProduct products;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            products = dbConnection.Query<PbsProduct>(sql, param).FirstOrDefault();
        }

        string productId;
        string PbsQualityResponsibilityId = null;

        if (products != null)
        {
            productId = products.ProductId;
            historyLog.PbsProductId = parameters.PbsDto.Id;
            historyLog.Action = HistoryState.UPDATED.ToString();
        }

        else
        {
            PbsQualityResponsibilityId = CreatePbsQualityResponsibility(parameters);
            var idGenerator = new IdGenerator();
            productId = idGenerator.GenerateId(applicationDbContext, "PBS-", "PbsSequenceCode");
            historyLog.PbsProductId = parameters.PbsDto.Id;
            historyLog.Action = HistoryState.ADDED.ToString();
        }

        var header =
            @"MERGE INTO dbo.PbsProduct t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE SET Name = @Name ,PbsProductItemTypeId = @PbsProductItemTypeId ,PbsProductStatusId = @PbsProductStatusId ,Scope = @Scope ,Contract= @Contract ,ProductPurpose = @ProductPurpose ,ProductComposition = @ProductComposition ,ProductDerivation = @ProductDerivation ,ProductFormatPresentation = @ProductFormatPresentation ,PbsToleranceStateId = @PbsToleranceStateId ,PbsQualityResponsibilityId = @PbsQualityResponsibilityId ,Title = @Title ,PbsType = @PbsType ,ParentId = @ParentId ,StartDate = @StartDate ,EndDate = @EndDate ,MachineTaxonomy = @MachineTaxonomy WHEN NOT MATCHED THEN INSERT (Id ,ProductId ,Name ,PbsProductItemTypeId ,PbsProductStatusId ,Scope ,Contract ,ProductPurpose ,ProductComposition ,ProductDerivation ,ProductFormatPresentation ,PbsToleranceStateId ,PbsQualityResponsibilityId ,IsDeleted ,NodeType ,PbsTaxonomyLevelId ,Title ,PbsType ,ProjectSequenceCode ,ParentId ,EndDate ,StartDate ,MachineTaxonomy) VALUES (@Id ,@ProductId ,@Name ,@PbsProductItemTypeId ,@PbsProductStatusId ,@Scope ,@Contract ,@ProductPurpose ,@ProductComposition ,@ProductDerivation ,@ProductFormatPresentation ,@PbsToleranceStateId ,@PbsQualityResponsibilityId ,@IsDeleted ,@NodeType ,@PbsTaxonomyLevelId ,@Title ,@PbsType ,@ProjectSequenceCode ,@ParentId ,@EndDate ,@StartDate ,@MachineTaxonomy);";

        var parm = new
        {
            parameters.PbsDto.Id,
            ProductId = productId,
            parameters.PbsDto.Name,
            parameters.PbsDto.PbsProductItemTypeId,
            parameters.PbsDto.PbsProductStatusId,
            parameters.PbsDto.Scope,
            parameters.PbsDto.Contract,
            parameters.PbsDto.ProductPurpose,
            parameters.PbsDto.ProductComposition,
            parameters.PbsDto.ProductDerivation,
            parameters.PbsDto.ProductFormatPresentation,
            parameters.PbsDto.PbsToleranceStateId,
            PbsQualityResponsibilityId,
            IsDeleted = 0,
            NodeType = "P",
            PbsTaxonomyLevelId = "93d01fef-ccc7-4592-9544-3949d67cbf76",
            Title = productId + " - " + parameters.PbsDto.Name,
            parameters.PbsDto.PbsType,
            ProjectSequenceCode = parameters.ProjectSequenceId,
            parameters.PbsDto.ParentId,
            parameters.PbsDto.StartDate,
            parameters.PbsDto.EndDate,
            MachineTaxonomy = parameters.PbsDto.MachineTaxonomyParentId
        };

        var history =
            @"INSERT INTO dbo.PbsHistoryLog ( Id ,HistoryLog ,ChangedByUserId ,Action ,ChangedTime ,PbsProductId ) VALUES ( @Id ,@HistoryLog ,@ChangedByUserId ,@Action ,@ChangedTime ,@PbsProductId );";
        var hisparam = new
        {
            historyLog.Id,
            historyLog.HistoryLog,
            historyLog.ChangedByUserId,
            historyLog.Action,
            historyLog.ChangedTime,
            historyLog.PbsProductId
        };
        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(header, parm);
            await connection.ExecuteAsync(history, hisparam);
        }

        if (parameters.PbsDto.PbsProductStatusId == "4010e768-3e06-4702-b337-ee367a82addb")
        {
            var borUpdate =
                @"UPDATE dbo.Bor SET BorStatusId = @BorStatusId WHERE PbsProductId= @PbsProductId";

            var param2 = new
            {
                PbsProductId = parameters.PbsDto.Id,
                BorStatusId = "4010e768-3e06-4702-b337-ee367a82addb"
            };

            await using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(borUpdate, param2);
            }
        }

        CreatePbsProductTaxonomy(parameters, parameters.PbsDto.Id, true);

        return productId;
    }

    public async Task<IEnumerable<Pbs>> GetPbs(PbsParameters pbsParameters)
    {
        var sql = @"SELECT
                                  PbsProductItemTypeLocalizedData.Label AS PbsProductItemType
                                 ,PbsProduct.ProductId
                                ,PbsProduct.ProductId As Id
                                 ,PbsProduct.Title AS Title
                                 ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState
                                 ,PbsProductStatusLocalizedData.Label AS PbsProductStatus
                                 ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId
                                ,PbsProduct.ParentId AS ParentId
                                ,PbsProduct.Name AS Name
                                ,PbsProduct.Quantity
                                ,PbsProduct.MeasurementCode
                                ,PbsProduct.Mou
                                ,PbsProduct.QualityProducerId
                                FROM dbo.PbsProduct
                                LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData
                                  ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId
                                LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData
                                  ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId
                                LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData
                                  ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId
                                LEFT OUTER JOIN PbsTaxonomyLevelLocalizedData
                                  ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId
                                WHERE (PbsProductItemTypeLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsProductItemTypeId IS NULL)
                                AND (PbsToleranceStateLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsToleranceStateId IS NULL)
                                AND (PbsProductStatusLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsProductStatusId IS NULL)
                                AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsTaxonomyLevelId IS NULL)
                                AND IsDeleted = 0
                                AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'P' ";

        var sqlCbc = @"SELECT
                                  PbsProductItemTypeLocalizedData.Label AS PbsProductItemType
                                 ,PbsProduct.ProductId
                                ,PbsProduct.ProductId As Id
                                 ,PbsProduct.Title AS Title
                                 ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState
                                 ,PbsProductStatusLocalizedData.Label AS PbsProductStatus
                                 ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId
                                ,PbsProduct.ParentId AS ParentId
                                ,PbsProduct.Name AS Name
                                ,PbsProduct.Quantity
                                ,PbsProduct.MeasurementCode
                                ,PbsProduct.Mou
                                ,PbsProduct.QualityProducerId
                                ,CBCDynamicsAttributes.Key1
                                ,CBCDynamicsAttributes.Value1
                                ,CBCDynamicsAttributes.Key2
                                ,CBCDynamicsAttributes.Value2
                                ,CBCDynamicsAttributes.Key3
                                ,CBCDynamicsAttributes.Value3
                                FROM dbo.PbsProduct
                                LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData
                                  ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId
                                LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData
                                  ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId
                                LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData
                                  ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId
                                LEFT OUTER JOIN PbsTaxonomyLevelLocalizedData
                                  ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId
                                LEFT OUTER JOIN dbo.CBCDynamicsAttributes
                                  ON PbsProduct.ProductId = CBCDynamicsAttributes.ArticleNumber
                                WHERE (PbsProductItemTypeLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsProductItemTypeId IS NULL)
                                AND (PbsToleranceStateLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsToleranceStateId IS NULL)
                                AND (PbsProductStatusLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsProductStatusId IS NULL)
                                AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang
                                OR PbsProduct.PbsTaxonomyLevelId IS NULL)
                                AND IsDeleted = 0
                                AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'P' ";

        var sqlUtility = @"SELECT
  PbsProductItemTypeLocalizedData.Label AS PbsProductItemType
 ,PbsProduct.ProductId
 ,PbsProduct.Title AS Title
 ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState
 ,PbsProductStatusLocalizedData.Label AS PbsProductStatus
 ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId
FROM dbo.PbsProduct
LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData
  ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId
LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData
  ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId
LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData
  ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId
LEFT OUTER JOIN dbo.PbsTaxonomyLevelLocalizedData
  ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId
INNER JOIN dbo.PbsProductTaxonomy
  ON PbsProduct.Id = PbsProductTaxonomy.PbsProductId
WHERE PbsProductTaxonomy.PbsTaxonomyId IN ('6e54725c-e396-4ce4-88f3-a6e9678a0389')
AND (PbsProductItemTypeLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsProductItemTypeId IS NULL)
AND (PbsToleranceStateLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsToleranceStateId IS NULL)
AND (PbsProductStatusLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsProductStatusId IS NULL)
AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsTaxonomyLevelId IS NULL)
AND PbsProduct.IsDeleted = 0
AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'N'";


        var sqlLocation = @"SELECT
  PbsProductItemTypeLocalizedData.Label AS PbsProductItemType
 ,PbsProduct.ProductId
 ,PbsProduct.Title AS Title
 ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState
 ,PbsProductStatusLocalizedData.Label AS PbsProductStatus
 ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId
FROM dbo.PbsProduct
LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData
  ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId
LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData
  ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId
LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData
  ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId
LEFT OUTER JOIN dbo.PbsTaxonomyLevelLocalizedData
  ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId
INNER JOIN dbo.PbsProductTaxonomy
  ON PbsProduct.Id = PbsProductTaxonomy.PbsProductId
WHERE PbsProductTaxonomy.PbsTaxonomyId IN ('ab294299-f251-41a8-94bd-3ae0150df134')
AND (PbsProductItemTypeLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsProductItemTypeId IS NULL)
AND (PbsToleranceStateLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsToleranceStateId IS NULL)
AND (PbsProductStatusLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsProductStatusId IS NULL)
AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang
OR PbsProduct.PbsTaxonomyLevelId IS NULL)
AND PbsProduct.IsDeleted = 0
AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'N'";

        sql = pbsParameters.Filter.Taxonomy switch
        {
            "3" => sqlUtility,
            "4" => sqlLocation,
            _ => sql
        };

        StringBuilder sb;
        if (pbsParameters.ContractingUnitSequenceId == null && pbsParameters.ProjectSequenceId == null)
            sb = new StringBuilder(sqlCbc);
        else
            sb = new StringBuilder(sql);


        if (pbsParameters.Filter.PbsProductItemTypeId != null)
            sb.Append(
                "AND PbsProduct.PbsProductItemTypeId='" + pbsParameters.Filter.PbsProductItemTypeId + "' ");

        if (pbsParameters.Filter.PbsProductStatusId != null)
            sb.Append("AND PbsProduct.PbsProductStatusId='" + pbsParameters.Filter.PbsProductStatusId + "' ");

        if (pbsParameters.Filter.PbsToleranceStateId != null)
            sb.Append("AND PbsProduct.PbsToleranceStateId='" + pbsParameters.Filter.PbsToleranceStateId + "' ");

        if (pbsParameters.Filter.Title != null)
        {
            pbsParameters.Filter.Title = pbsParameters.Filter.Title.Replace("'", "''");
            sb.Append("AND PbsProduct.Title like '%" + pbsParameters.Filter.Title + "%' ");
        }

        if (pbsParameters.Filter.QualityProducerId != null)
        {
            sb.Append("AND PbsProduct.QualityProducerId = '" + pbsParameters.Filter.QualityProducerId + "' ");
        }

        if (pbsParameters.Filter.Scope != null)
            sb.Append("AND PbsProduct.Scope='" + pbsParameters.Filter.Scope + "' ");

        // if (pbsParameters.Filter.Sorter.Attribute == null) sb.Append(" order BY CAST(SUBSTRING(PbsProduct.ProductId,5,20) AS INT) desc ");

        if (pbsParameters.Filter.Sorter.Attribute != null)
        {
            if (pbsParameters.ContractingUnitSequenceId != null || pbsParameters.ProjectSequenceId != null)
                switch (pbsParameters.Filter.Sorter.Attribute.ToLower())
                {
                    case "title":
                        sb.Append(" order BY CAST(SUBSTRING(PbsProduct.ProductId,5,20) AS INT) " +
                                  pbsParameters.Filter.Sorter.Order);
                        break;
                    case "pbsproductitemtype":
                        sb.Append(" order by PbsProductItemTypeLocalizedData.Label " +
                                  pbsParameters.Filter.Sorter.Order);
                        break;
                    case "pbsproductstatus":
                        sb.Append(" order by PbsProductStatusLocalizedData.Label " + pbsParameters.Filter.Sorter.Order);
                        break;
                    case "pbstolerancestate":
                        sb.Append(" order by PbsToleranceStateLocalizedData.Label " +
                                  pbsParameters.Filter.Sorter.Order);
                        break;
                }
        }
        else
        {
            if (pbsParameters.ContractingUnitSequenceId != null || pbsParameters.ProjectSequenceId != null)
                sb.Append("ORDER BY CAST(SUBSTRING(PbsProduct.ProductId,5,20) AS INT) desc");
            else
                sb.Append("ORDER BY PbsProduct.ProductId ASC");
        }

        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var parameters = new { lang = pbsParameters.Lang };

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            var result = await dbConnection.QueryAsync<Pbs>(sb.ToString(), parameters);

            await using var tenetConnection =
                new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString);

            var persons = tenetConnection.Query<CabDataDapperDto>(
                "SELECT cp.Id AS PersonId,cp.FullName,cc.Name AS Organisation FROM CabPerson cp LEFT OUTER JOIN CabPersonCompany cpc ON cp.Id = cpc.PersonId LEFT OUTER JOIN CabCompany cc ON cpc.CompanyId = cc.Id");
            foreach (var pbs in result)
            {
                if (pbs.QualityProducerId != null)
                {
                    pbs.QualityProducerName =
                        persons.FirstOrDefault(x => x.PersonId == pbs.QualityProducerId)?.FullName;
                    pbs.QualityProducerCompany =
                        persons.FirstOrDefault(x => x.PersonId == pbs.QualityProducerId)?.Organisation;
                }
            }

            if (pbsParameters.Filter.Sorter.Attribute != null)
            {
                if (pbsParameters.Filter.Sorter.Attribute.ToLower() == "qualityproducername")
                {
                    if (pbsParameters.Filter.Sorter.Order == "desc")
                    {
                        result = result.OrderByDescending(e => e.QualityProducerName);
                    }
                    else
                    {
                        result = result.OrderBy(e => e.QualityProducerName);
                    }
                }
            }

            return result;
        }
    }


    public async Task<IEnumerable<PbsPo>> GetPbsPO(PbsParameters pbsParameters)
    {
        var sql =
            @"SELECT PbsProduct.Id ,PbsProductItemTypeLocalizedData.Label AS PbsProductItemType ,PbsProductItemTypeLocalizedData.PbsProductItemTypeId AS PbsProductItemTypeId ,PbsProduct.ProductId ,PbsProduct.Title AS Title ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState ,PbsToleranceStateLocalizedData.PbsToleranceStateId AS PbsToleranceStateId ,PbsProductStatusLocalizedData.Label AS PbsProductStatus ,PbsProductStatusLocalizedData.PbsProductStatusId AS PbsProductStatusId ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId ,PbsService.TotalPrice FROM dbo.PbsProduct LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId LEFT OUTER JOIN PbsTaxonomyLevelLocalizedData ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId LEFT OUTER JOIN PbsService ON PbsProduct.Id = PbsService.ProductId WHERE (PbsProductItemTypeLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductItemTypeId IS NULL) AND (PbsToleranceStateLocalizedData.LanguageCode = @lang OR PbsProduct.PbsToleranceStateId IS NULL) AND (PbsProductStatusLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductStatusId IS NULL) AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang OR PbsProduct.PbsTaxonomyLevelId IS NULL) AND IsDeleted = 0 AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'P'AND PbsProduct.ProductId NOT in (SELECT ProductId FROM POProduct GROUP BY ProductId)";

        var sqlUtility =
            @"SELECT PbsProductItemTypeLocalizedData.Label AS PbsProductItemType ,PbsProductItemTypeLocalizedData.PbsProductItemTypeId AS PbsProductItemTypeId ,PbsProduct.ProductId ,PbsProduct.Title AS Title ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState ,PbsToleranceStateLocalizedData.PbsToleranceStateId AS PbsToleranceStateId ,PbsProductStatusLocalizedData.Label AS PbsProductStatus ,PbsProductStatusLocalizedData.PbsProductStatusId AS PbsProductStatusId ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId ,PbsService.TotalPrice FROM dbo.PbsProduct LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId LEFT OUTER JOIN dbo.PbsTaxonomyLevelLocalizedData ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId INNER JOIN dbo.PbsProductTaxonomy ON PbsProduct.Id = PbsProductTaxonomy.PbsProductId LEFT OUTER JOIN PbsService ON PbsProduct.Id = PbsService.ProductId WHERE PbsProductTaxonomy.PbsTaxonomyId IN ('6e54725c-e396-4ce4-88f3-a6e9678a0389') AND (PbsProductItemTypeLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductItemTypeId IS NULL) AND (PbsToleranceStateLocalizedData.LanguageCode = @lang OR PbsProduct.PbsToleranceStateId IS NULL) AND (PbsProductStatusLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductStatusId IS NULL) AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang OR PbsProduct.PbsTaxonomyLevelId IS NULL) AND PbsProduct.IsDeleted = 0 AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'N'";


        var sqlLocation =
            @"SELECT PbsProductItemTypeLocalizedData.Label AS PbsProductItemType ,PbsProductItemTypeLocalizedData.PbsProductItemTypeId AS PbsProductItemTypeId ,PbsProduct.ProductId ,PbsProduct.Title AS Title ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState ,PbsToleranceStateLocalizedData.PbsToleranceStateId AS PbsToleranceStateId ,PbsProductStatusLocalizedData.Label AS PbsProductStatus ,PbsProductStatusLocalizedData.PbsProductStatusId AS PbsProductStatusId ,PbsProduct.PbsTaxonomyLevelId AS PbsTaxonomyLevelId ,PbsService.TotalPrice FROM dbo.PbsProduct LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId LEFT OUTER JOIN dbo.PbsTaxonomyLevelLocalizedData ON PbsProduct.PbsTaxonomyLevelId = PbsTaxonomyLevelLocalizedData.PbsTaxonomyLevelId INNER JOIN dbo.PbsProductTaxonomy ON PbsProduct.Id = PbsProductTaxonomy.PbsProductId LEFT OUTER JOIN PbsService ON PbsProduct.Id = PbsService.ProductId WHERE PbsProductTaxonomy.PbsTaxonomyId IN ('ab294299-f251-41a8-94bd-3ae0150df134') AND (PbsProductItemTypeLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductItemTypeId IS NULL) AND (PbsToleranceStateLocalizedData.LanguageCode = @lang OR PbsProduct.PbsToleranceStateId IS NULL) AND (PbsProductStatusLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductStatusId IS NULL) AND (PbsTaxonomyLevelLocalizedData.LanguageCode = @lang OR PbsProduct.PbsTaxonomyLevelId IS NULL) AND PbsProduct.IsDeleted = 0 AND PbsTaxonomyLevelLocalizedData.IsProduct = 1 AND PbsProduct.NodeType = 'N'";

        sql = pbsParameters.Filter.Taxonomy switch
        {
            "3" => sqlUtility,
            "4" => sqlLocation,
            _ => sql
        };

        var sb = new StringBuilder(sql);

        if (pbsParameters.Filter.PbsProductItemTypeId != null)
        {
            if (pbsParameters.Filter.PbsProductItemTypeId == "aa0c8e3c-f716-4f92-afee-851d485164da")
                sb.Append(
                    "AND PbsProduct.Id NOT in (SELECT PbsProductId FROM POResources where PbsProductId is not null GROUP BY PbsProductId) AND (PbsProduct.Id In (select PbsProductId from PbsLabour) OR PbsProduct.Id In (select PbsProductId from PbsTools)) AND PbsProduct.PbsProductItemTypeId='" +
                    pbsParameters.Filter.PbsProductItemTypeId + "' ");
            else
                sb.Append("AND PbsProduct.PbsProductItemTypeId='" + pbsParameters.Filter.PbsProductItemTypeId +
                          "' ");
        }

        if (pbsParameters.Filter.PbsProductStatusId != null)
            sb.Append("AND PbsProduct.PbsProductStatusId='" + pbsParameters.Filter.PbsProductStatusId + "' ");

        if (pbsParameters.Filter.PbsToleranceStateId != null)
            sb.Append("AND PbsProduct.PbsToleranceStateId='" + pbsParameters.Filter.PbsToleranceStateId + "' ");

        if (pbsParameters.Filter.Title != null)
        {
            pbsParameters.Filter.Title = pbsParameters.Filter.Title.Replace("'", "''");
            sb.Append("AND PbsProduct.Title like '%" + pbsParameters.Filter.Title + "%' ");
        }

        if (pbsParameters.Filter.Scope != null)
            sb.Append("AND PbsProduct.Scope='" + pbsParameters.Filter.Scope + "' ");

        if (pbsParameters.Filter.Sorter.Attribute == null) sb.Append("ORDER BY PbsProduct.ProductId DESC ");

        if (pbsParameters.Filter.Sorter.Attribute != null)
        {
            if (pbsParameters.Filter.Sorter.Attribute.ToLower().Equals("title"))
                sb.Append("order by PbsProduct.ProductId " + pbsParameters.Filter.Sorter.Order);

            if (pbsParameters.Filter.Sorter.Attribute.ToLower().Equals("pbsproductitemtype"))
                sb.Append("order by PbsProductItemTypeLocalizedData.Label " +
                          pbsParameters.Filter.Sorter.Order);

            if (pbsParameters.Filter.Sorter.Attribute.ToLower().Equals("pbsproductstatus"))
                sb.Append("order by PbsProductStatusLocalizedData.Label " + pbsParameters.Filter.Sorter.Order);

            if (pbsParameters.Filter.Sorter.Attribute.ToLower().Equals("pbstolerancestate"))
                sb.Append("order by PbsToleranceStateLocalizedData.Label " + pbsParameters.Filter.Sorter.Order);
        }

        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var parameters = new { lang = pbsParameters.Lang };

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            var result = await dbConnection.QueryAsync<PbsPo>(sb.ToString(), parameters);


            return result;
        }
    }

    public async Task<PbsDropdownData> GetPbsDropdown(PbsParameters pbsParameters)
    {
        var pbsDropdownData = new PbsDropdownData
        {
            Itemtypes = GetProductItemType(pbsParameters).Result,
            ProductStates = GetProductStatus(pbsParameters).Result,
            ToleranceStates = GetPbsToleranceState(pbsParameters).Result
        };

        return pbsDropdownData;
    }


    public async Task<PbsTreeStructureDto> GetTreeStructureData(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var sbUtility = new StringBuilder(
            "select p.Id, p.Name as Title, t.PbsTaxonomyNodeId AS ParentId, p.NodeType from PbsProduct p inner join PbsProductTaxonomy t on p.Id = t.PbsProductId where(t.PbsTaxonomyId = '6e54725c-e396-4ce4-88f3-a6e9678a0389' ) and p.IsDeleted = 0");
        var sbLocation = new StringBuilder(
            "select p.Id, p.Name as Title, t.PbsTaxonomyNodeId AS ParentId, p.NodeType from PbsProduct p inner join PbsProductTaxonomy t on p.Id = t.PbsProductId where (t.PbsTaxonomyId = 'ab294299-f251-41a8-94bd-3ae0150df134' ) and p.IsDeleted = 0");
        var product =
            new StringBuilder(
                "SELECT PbsProduct.ProductId As Id,PbsProduct.Title,PbsProduct.ParentId,PbsProduct.ProductId AS PbsSequenceId FROM dbo.PbsProduct where NodeType = 'P' AND IsDeleted = 0");
        var machine = new StringBuilder("SELECT Id,Title,ParentId FROM dbo.MachineTaxonmy");

        var dto = new PbsTreeStructureDto();
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            var resultUtility = dbConnection.Query<PbsTreeStructure>(sbUtility.ToString()).ToList();
            dto.utilityTaxonomy = resultUtility;
        }

        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            var resultLocation = dbConnection.Query<PbsTreeStructure>(sbLocation.ToString()).ToList();
            dto.locationTaxonomy = resultLocation;
        }

        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            var resultProduct = dbConnection.Query<PbsTreeStructure>(product.ToString()).ToList();
            dto.productTaxonomy = resultProduct;
        }

        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            var resultProduct = dbConnection.Query<PbsTreeStructure>(machine.ToString()).ToList();
            dto.machineTaxonomy = resultProduct;
        }

        return dto;
    }

    public async Task<PbsGetByIdDto> GetPbsById(PbsParameters pbsParameters)
    {
        var lang = pbsParameters.Lang;

        IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyLogDto = null;
        IEnumerable<PbsDto> result = null;
        var query =
            @"SELECT PbsProductItemTypeLocalizedData.Label AS PbsProductItemType ,PbsProductItemTypeLocalizedData.PbsProductItemTypeId ,PbsProduct.ProductId ,PbsProduct.Title AS Title ,CONCAT(PbsProduct.ProductId, ' - ', PbsProduct.Name) AS HeaderTitle ,PbsToleranceStateLocalizedData.Label AS PbsToleranceState ,PbsToleranceStateLocalizedData.PbsToleranceStateId ,PbsProductStatusLocalizedData.Label AS PbsProductStatus ,PbsProductStatusLocalizedData.PbsProductStatusId ,PbsProductTaxonomy.PbsTaxonomyNodeId AS Parent ,PbsTaxonomy.Name AS Taxonomy ,PbsProduct.Id ,PbsProduct.ProductId AS expr1 ,PbsProduct.Name ,PbsProduct.Scope ,PbsProduct.Contract ,PbsProduct.ProductPurpose ,PbsProduct.ProductComposition ,PbsProduct.ProductDerivation ,PbsProduct.ProductFormatPresentation ,PbsProduct.PbsType,PbsProduct.QualityProducerId ,PbsQualityResponsibility.Id AS PbsQualityResponsibilityId ,PbsQualityResponsibility.QualityApproverId AS QualityApproverId ,PbsQualityResponsibility.QualityProducerId AS QualityProducerId ,PbsQualityResponsibility.QualityReviewerId AS QualityReviewerId ,PbsProduct.NodeType ,PbsProduct.StartDate ,PbsProduct.EndDate ,PbsProduct.ParentId ,PbsProduct.Quantity ,PbsProduct.MeasurementCode , PbsProduct.Mou,MachineTaxonmy.Id AS MachineTaxonomyParentId,PbsProduct.WbsTaxonomyId , PbsProduct.PbsLotId,PbsProduct.PbsLocation FROM dbo.PbsProduct LEFT OUTER JOIN dbo.PbsProductItemTypeLocalizedData ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId LEFT OUTER JOIN dbo.PbsToleranceStateLocalizedData ON PbsProduct.PbsToleranceStateId = PbsToleranceStateLocalizedData.PbsToleranceStateId LEFT OUTER JOIN dbo.PbsProductStatusLocalizedData ON PbsProduct.PbsProductStatusId = PbsProductStatusLocalizedData.PbsProductStatusId LEFT OUTER JOIN dbo.PbsProductTaxonomy ON PbsProduct.Id = PbsProductTaxonomy.PbsProductId LEFT OUTER JOIN dbo.PbsTaxonomy ON PbsProductTaxonomy.PbsTaxonomyId = PbsTaxonomy.Id LEFT OUTER JOIN dbo.PbsQualityResponsibility ON PbsProduct.PbsQualityResponsibilityId = PbsQualityResponsibility.Id LEFT OUTER JOIN dbo.MachineTaxonmy ON PbsProduct.MachineTaxonomy = MachineTaxonmy.Id WHERE (PbsProductItemTypeLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductItemTypeId IS NULL) AND (PbsToleranceStateLocalizedData.LanguageCode = @lang OR PbsProduct.PbsToleranceStateId IS NULL) AND (PbsProductStatusLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductStatusId IS NULL) AND PbsProduct.ProductId = @id";

        var historyQuery =
            @"SELECT PbsHistoryLog.ChangedTime AS DateTime ,PbsHistoryLog.ChangedByUserId AS Oid,PbsHistoryLog.RevisionNumber AS RevisionNumber FROM dbo.PbsHistoryLog WHERE PbsHistoryLog.PbsProductId =@id ORDER BY RevisionNumber";

        var parameters = new { lang, id = pbsParameters.Id };

        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            result = dbConnection.Query<PbsDto>(query, parameters);
            var historyparameters = new { id = result.First().Id };
            historyLogDto =
                dbConnection.Query<ProjectDefinitionHistoryLogDapperDto>(historyQuery, historyparameters);
        }


        var dto = new PbsGetByIdDto();
        var loctaxonomy = result.Where(p => p.Taxonomy == "Location");
        var utiltaxonomy = result.Where(p => p.Taxonomy == "Utility");
        if (loctaxonomy.Any()) dto.LocationTaxonomyParentId = loctaxonomy.FirstOrDefault().Parent;

        if (utiltaxonomy.Any()) dto.UtilityTaxonomyParentId = utiltaxonomy.FirstOrDefault().Parent;

        var resultObj = result.FirstOrDefault();
        dto.PbsProductItemType = resultObj.PbsProductItemType;
        dto.PbsProductStatus = resultObj.PbsProductStatus;
        dto.PbsToleranceState = resultObj.PbsToleranceState;
        dto.ProductId = resultObj.ProductId;
        dto.Title = resultObj.Title;
        dto.Id = resultObj.Id;
        dto.Name = resultObj.Name;
        dto.Scope = resultObj.Scope;
        dto.Contract = resultObj.Contract;
        dto.ProductPurpose = resultObj.ProductPurpose;
        dto.ProductComposition = resultObj.ProductComposition;
        dto.ProductDerivation = resultObj.ProductDerivation;
        dto.ProductFormatPresentation = resultObj.ProductFormatPresentation;
        dto.QualityApproverId = resultObj.QualityApproverId;
        dto.QualityProducerId = resultObj.QualityProducerId;
        dto.QualityReviewerId = resultObj.QualityReviewerId;
        dto.PbsQualityResponsibilityId = resultObj.PbsQualityResponsibilityId;
        dto.PbsProductItemTypeId = resultObj.PbsProductItemTypeId;
        dto.PbsProductStatusId = resultObj.PbsProductStatusId;
        dto.PbsToleranceStateId = resultObj.PbsToleranceStateId;
        dto.HeaderTitle = resultObj.HeaderTitle;
        dto.PbsType = resultObj.PbsType;
        dto.StartDate = resultObj.StartDate;
        dto.EndDate = resultObj.EndDate;
        dto.ParentId = resultObj.ParentId;
        dto.MachineTaxonomyParentId = resultObj.MachineTaxonomyParentId;
        dto.MeasurementCode = resultObj.MeasurementCode;
        dto.Project = pbsParameters.ProjectSequenceId;
        dto.WbsTaxonomyId = resultObj.WbsTaxonomyId;
        dto.PbsLotId = resultObj.PbsLotId;
        dto.PbsLocation = resultObj.PbsLocation;
        var cabPersonConnectionString = pbsParameters.TenantProvider.GetTenant().ConnectionString;
        var qualityApproverQuery = @"SELECT CabPerson.FullName from CabPerson WHERE Id = @QualityApproverId";
        var qualityApproverParameters = new { resultObj.QualityApproverId };
        await using (var dbConnection = new SqlConnection(cabPersonConnectionString))
        {
            var qualityApproverName =
                dbConnection.Query<string>(qualityApproverQuery, qualityApproverParameters);

            dto.QualityApprover = qualityApproverName.FirstOrDefault();
            var qualityReviewerQuery = @"SELECT CabPerson.FullName from CabPerson WHERE Id = @qualityReviewerId";
            var qualityReviewerParameters = new { qualityReviewerId = resultObj.QualityReviewerId };

            var qualityReviewerName =
                dbConnection.Query<string>(qualityReviewerQuery, qualityReviewerParameters);


            dto.QualityReviewer = qualityReviewerName.FirstOrDefault();
        }


        var qualityProducerQuery = @"SELECT CabPerson.FullName from CabPerson WHERE Id = @qualityProducerId";
        var qualityProducerParameters = new { qualityProducerId = resultObj.QualityProducerId };
        string projectLocation = null;
        await using (var dbConnection = new SqlConnection(cabPersonConnectionString))
        {
            var qualityProducerName =
                dbConnection.Query<string>(qualityProducerQuery, qualityProducerParameters);
            dto.QualityProducer = qualityProducerName.FirstOrDefault();
            
            projectLocation = dbConnection.Query<string>(@"SELECT LocationId FROM dbo.ProjectDefinition WHERE SequenceCode = @ProjectSequenceId",new{pbsParameters.ProjectSequenceId}).FirstOrDefault();
        }


        var log = new ProjectDefinitionHistoryLogDto();
        var historyUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [User] FROM ApplicationUser WHERE ApplicationUser.Oid =@userId";
        IEnumerable<string> userName = null;
        if (historyLogDto.Any())
        {
            var historydto = historyLogDto.First();
            log.CreatedDateTime = historydto.DateTime;
            log.RevisionNumber = historydto.RevisionNumber;
            var historyUserParameter = new { userId = historydto.Oid };
            await using (var connection =
                         new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString))
            {
                userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                log.CreatedByUser = userName.FirstOrDefault();
                
            }
        }

        if (historyLogDto.Count() >= 2)
        {
            var historydto = historyLogDto.Last();
            log.UpdatedDateTime = historydto.DateTime;
            log.RevisionNumber = historydto.RevisionNumber;
            var historyUserParameter = new { userId = historydto.Oid };
            using (var connection =
                   new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString))
            {
                userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                log.UpdatedByUser = userName.FirstOrDefault();
                
            }
        }

        // if (pbsParameters.ContractingUnitSequenceId == null && pbsParameters.ProjectSequenceId == null)
        // await using (var connection =
        //              new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString))
        // {

        await using (var connection = new SqlConnection(connectionString))
        {
            dto.DynamicAttributes = connection
                .Query<GetCBCDynamicsAttributes>(
                    "Select * From CBCDynamicsAttributes Where ArticleNumber = @ArticleNumber",
                    new { ArticleNumber = dto.ProductId }).FirstOrDefault();
            var scopeOfWorkSql =
                @"SELECT PbsScopeOfWork.*,CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId AS [Key],CpcBasicUnitOfMeasureLocalizedData.Label AS [Text] from PbsScopeOfWork LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON PbsScopeOfWork.MouId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId WHERE ProductId = @productId AND CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang";

            dto.PbsScopeOfWork = connection
                .Query<PbsScopeOfWorkGetByIdDto, CpcBasicunitofMeasure, PbsScopeOfWorkGetByIdDto>(
                    scopeOfWorkSql,
                    (scopeOfWork, cpcMou) =>
                    {
                        scopeOfWork.Mou = cpcMou;

                        return scopeOfWork;
                    },
                    new
                    {
                        lang = pbsParameters.Lang,
                        productId = dto.Id
                    },
                    splitOn: "Key").FirstOrDefault();
        }
        dto.historyLog = log;

        if (dto.PbsLocation != null)
        {
            var options = new DbContextOptions<PbsDbContext>();
            
            var context2 = new PbsDbContext(options,connectionString, pbsParameters.TenantProvider);

            var mapLocation = context2.MapLocation.Where(l => l.Id == dto.PbsLocation).Include(m => m.Address)
                .Include(m => m.Position)
                .ToList().FirstOrDefault();

            if (mapLocation != null)
            {
                dto.MapLocation = mapLocation;
            }
        }
        else
        {
            var options = new DbContextOptions<PbsDbContext>();
            
            var context2 = new PbsDbContext(options,pbsParameters.TenantProvider.GetTenant().ConnectionString, pbsParameters.TenantProvider);

            var mapLocation = context2.MapLocation.Where(l => l.Id == projectLocation).Include(m => m.Address)
                .Include(m => m.Position)
                .ToList().FirstOrDefault();

            if (mapLocation != null)
            {
                dto.MapLocation = mapLocation;
            }
        }

        var pbsResParam = new PbsResourceParameters()
        {
            TenantProvider = pbsParameters.TenantProvider,
            ContractingUnitSequenceId = pbsParameters.ContractingUnitSequenceId,
            ProjectSequenceId = pbsParameters.ProjectSequenceId,
            Lang = pbsParameters.Lang,
            ContextAccessor = pbsParameters.ContextAccessor,
            IdList = [dto.Id]
        };
        var consData =
            await pbsParameters.IPbsResourceRepository.GetPbsResourceConsolidatedQuantity(pbsResParam,
                "PbsLabour");

        dto.ConsolidatedDuration =
                consData.FirstOrDefault(v => v.PbsId == dto.Id)?.TotalConsolidatedQuantity;
        
        return dto;
    }

    public async Task DeletePbs(PbsParameters pbsParameters)
    {
        var options = new DbContextOptions<PbsDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        await using (var context = new PbsDbContext(options, connectionString, pbsParameters.TenantProvider))
        {
            foreach (var id in pbsParameters.IdList)
            {
                var product = context.PbsProduct.FirstOrDefault(x => x.Id == id);
                var pbs = context.PbsProduct.FirstOrDefault(i => i.Id == id);
                pbs.IsDeleted = true;
                context.PbsProduct.Update(pbs);
                await context.SaveChangesAsync();

                using IDbConnection dbConnection = new SqlConnection(connectionString);
                await dbConnection.ExecuteAsync("Update PbsProduct Set ParentId = null where ParentId = @ParentId ",
                    new { ParentId = product.ProductId });
            }
        }
    }

    public async Task<TaxonomyLevelReadDto> GetTaxonomyLevels(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var lang = pbsParameters.Lang;
        if (string.IsNullOrEmpty(pbsParameters.Lang) || pbsParameters.Lang.Contains("en")) lang = "en";

        var sbUtility = new StringBuilder(
            "select PbsTaxonomyLevelId AS Id, Label AS Name, Level, IsSearchable from PbsTaxonomyLevelLocalizedData where TaxonomyId = '6e54725c-e396-4ce4-88f3-a6e9678a0389' and LanguageCode ='" +
            lang + "' order by Level");
        var sbLocation = new StringBuilder(
            "select PbsTaxonomyLevelId AS Id, Label AS Name, Level, IsSearchable from PbsTaxonomyLevelLocalizedData where TaxonomyId = 'ab294299-f251-41a8-94bd-3ae0150df134' and LanguageCode ='" +
            lang + "' order by Level");
        var machine =
            new StringBuilder(
                "SELECT Id,Name,LevelId,LanguageCode FROM dbo.MachineTaxonomyLevel WHERE LanguageCode = '" +
                lang + "'");
        var dto = new TaxonomyLevelReadDto();
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            var resultUtility = dbConnection.Query<TaxonomyLevelDto>(sbUtility.ToString()).ToList();


            dto.UtilityTaxonomyLevels = resultUtility;
        }

        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            var resultLocation = dbConnection.Query<TaxonomyLevelDto>(sbLocation.ToString()).ToList();


            dto.LocationTaxonomyLevels = resultLocation;
        }

        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            var resultLocation = dbConnection.Query<TaxonomyLevelDto>(machine.ToString()).ToList();


            dto.MachineTaxonomyLevels = resultLocation;
        }

        return dto;
    }

    public async Task<string> CreateNode(PbsParameters parameters)
    {
        try
        {
            var options = new DbContextOptions<PbsDbContext>();
            var options2 = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options2, parameters.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
                parameters.ProjectSequenceId, parameters.TenantProvider);
            await using (var context = new PbsDbContext(options, connectionString, parameters.TenantProvider))
            {
                var jsonProduct = JsonConvert.SerializeObject(parameters.PbsDto, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                var historyLog = new PbsHistoryLog
                {
                    Id = Guid.NewGuid().ToString(),
                    ChangedTime = DateTime.UtcNow,
                    ChangedByUserId = parameters.ChangedUser.OId,
                    HistoryLog = jsonProduct
                };
                if (parameters.nodeDto.IsEdit)
                {
                    var product = context.PbsProduct.Where(p => p.Id == parameters.nodeDto.Id)
                        .FirstOrDefault();
                    product.Name = parameters.nodeDto.Title;
                    product.Title = product.ProductId + " - " + product.Name;
                    context.PbsProduct.Update(product);
                    context.SaveChanges();
                    historyLog.PbsProductId = product.Id;
                    historyLog.Action = HistoryState.UPDATED.ToString();
                }
                else
                {
                    if (string.IsNullOrEmpty(parameters.nodeDto.Key))
                    {
                        var product = new PbsProduct();
                        product.Name = parameters.nodeDto.Title;
                        product.Id = parameters.nodeDto.Id;
                        product.NodeType = "N";
                        product.PbsTaxonomyLevelId = parameters.nodeDto.PbsTaxonomyLevelId;
                        var idGenerator = new IdGenerator();
                        product.ProductId = idGenerator.GenerateId(applicationDbContext, "PBS-", "PbsSequenceCode");
                        product.Title = product.ProductId + " - " + product.Name;
                        context.PbsProduct.Add(product);
                        context.SaveChanges();
                        historyLog.PbsProductId = product.Id;
                        historyLog.Action = HistoryState.ADDED.ToString();

                        var newTaxonomy = new PbsProductTaxonomy();
                        newTaxonomy.Id = Guid.NewGuid().ToString();
                        newTaxonomy.PbsProductId = parameters.nodeDto.Id;
                        newTaxonomy.PbsTaxonomyNodeId = parameters.nodeDto.ParentId;
                        newTaxonomy.PbsTaxonomyId = context.PbsTaxonomyLevelLocalizedData
                            .Where(t => t.PbsTaxonomyLevelId == parameters.nodeDto.PbsTaxonomyLevelId)
                            .FirstOrDefault().TaxonomyId;
                        context.PbsProductTaxonomy.Add(newTaxonomy);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var newTaxonomy = new PbsProductTaxonomy();
                        newTaxonomy.Id = Guid.NewGuid().ToString();
                        newTaxonomy.PbsProductId = parameters.nodeDto.Key;
                        newTaxonomy.PbsTaxonomyNodeId = parameters.nodeDto.ParentId;
                        newTaxonomy.PbsTaxonomyId = context.PbsTaxonomyLevelLocalizedData
                            .Where(t => t.PbsTaxonomyLevelId == parameters.nodeDto.PbsTaxonomyLevelId)
                            .FirstOrDefault().TaxonomyId;
                        context.PbsProductTaxonomy.Add(newTaxonomy);
                        await context.SaveChangesAsync();
                    }
                }

                return parameters.nodeDto.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PbsProductDto>> GetProductByTaxonomyLevel(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var query =
            @"select Id AS [Key], Name AS Text from PbsProduct where PbsTaxonomyLevelId = @taxonomyId AND Title like '%" +
            pbsParameters.taxonomyLevelFilter.Title?.Replace("'", "''") + "%' ";
        var parameters = new { taxonomyId = pbsParameters.taxonomyLevelFilter.PbsTaxonomyLevelId };

        using (var dbConnection = new SqlConnection(connectionString))
        {
            var result = dbConnection.Query<PbsProductDto>(query, parameters);


            return result;
        }
    }

    public async Task<IEnumerable<PbsProductDto>> GetPbsFilterHasBor(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        var query = @"SELECT
                                     DISTINCT PbsProduct.ProductId AS [Key]
                                     ,PbsProduct.Title AS [Text]
                                    FROM dbo.Bor
                                    INNER JOIN dbo.PbsProduct
                                      ON Bor.PbsProductId = PbsProduct.Id
                                    WHERE PbsProduct.Title like '%" + pbsParameters.Filter.Title?.Replace("'", "''") +
                    "%' AND (PbsProduct.PbsType Not IN('sub') OR PbsProduct.PbsType IS NULL) ";


        var query2 = @"SELECT
                                      PbsProduct.ProductId AS [Key]
                                     ,PbsProduct.Title AS [Text]									 
                                    FROM dbo.PbsProduct
                                    
                                    WHERE PbsProduct.Title like '%" + pbsParameters.Filter.Title?.Replace("'", "''") +
                     "%' AND PbsProduct.PbsType='sub' order by ProductId asc";


        if (pbsParameters.Filter.Type != "sub")
        {
            await using var dbConnection = new SqlConnection(connectionString);
            var result = dbConnection.Query<PbsProductDto>(query);


            return result;
        }

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var result = dbConnection.Query<PbsProductDto>(query2);

            return result;
        }
    }


    public async Task<string> ClonePbs(PbsParameters parameters)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, parameters.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
                parameters.ProjectSequenceId, parameters.TenantProvider);

            var projectCon = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
                parameters.ProjectSequenceId, parameters.TenantProvider);
            var projectConOptions = new DbContextOptions<ShanukaDbContext>();
            var projApplicationDbContext =
                new ShanukaDbContext(projectConOptions, projectCon, parameters.TenantProvider);

            var query = "SELECT [Id] FROM[dbo].[PbsProduct] where[ProductId] =@Id";

            var parameters2 = new
            {
                parameters.PbsCloneDto.Id
            };

            IEnumerable<PbsCloneDto> dataid;
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dataid = dbConnection.Query<PbsCloneDto>(query, parameters2);
            }

            var jsonProduct = JsonConvert.SerializeObject(parameters.PbsDto, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            var historyLog = new PbsHistoryLog
            {
                Id = Guid.NewGuid().ToString(),
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = parameters.ChangedUser.OId,
                HistoryLog = jsonProduct
            };

            var NewId = Guid.NewGuid().ToString();

            if (dataid != null)
            {
                var idGenerator = new IdGenerator();
                var ProductId = idGenerator.GenerateId(applicationDbContext, "PBS-", "PbsSequenceCode");
                var CloneHeader =
                    @"INSERT INTO dbo.PbsProduct (Id, ProductId, Name, PbsProductItemTypeId, PbsProductStatusId, Scope, Contract, ProductPurpose, ProductComposition, ProductDerivation, ProductFormatPresentation, PbsToleranceStateId, PbsQualityResponsibilityId, IsDeleted, NodeType, PbsTaxonomyLevelId, Title, PbsType, ProjectSequenceCode)
                                                 SELECT @NewId
                                                      ,@ProductId
                                                      ,[Name]
                                                      ,[PbsProductItemTypeId]
                                                      ,[PbsProductStatusId]
                                                      ,[Scope]
                                                      ,[Contract]
                                                      ,[ProductPurpose]
                                                      ,[ProductComposition]
                                                      ,[ProductDerivation]
                                                      ,[ProductFormatPresentation]
                                                      ,[PbsToleranceStateId]
                                                      ,[PbsQualityResponsibilityId]
                                                      ,[IsDeleted]
                                                      ,[NodeType]
                                                      ,[PbsTaxonomyLevelId]
                                                      ,(SELECT CONCAT(@ProductId,' - ',(SELECT Name FROM PbsProduct where ProductId = @Id)))
                                                      ,[PbsType]
                                                      ,[ProjectSequenceCode]
                                                  FROM [dbo].[PbsProduct]
                                                  WHERE [dbo].[PbsProduct].ProductId = @Id";

                var parameters1 = new
                {
                    parameters.PbsCloneDto.Id,
                    NewId,
                    ProductId
                };

                IEnumerable<PbsCloneDto> data;
                await using (var dbConnection = new SqlConnection(connectionString))
                {
                    data = dbConnection.Query<PbsCloneDto>(CloneHeader, parameters1);
                }

                historyLog.PbsProductId = NewId;
                historyLog.Action = HistoryState.ADDED.ToString();
                projApplicationDbContext.PbsHistoryLog.Add(historyLog);
                await projApplicationDbContext.SaveChangesAsync();

                var parameters7 = new
                {
                    parameters.PbsCloneDto.Id
                };


                var pbsResourceParameters = new PbsResourceParameters
                {
                    PbsProductId = dataid.FirstOrDefault()?.Id,
                    ContractingUnitSequenceId = parameters.ContractingUnitSequenceId,
                    ProjectSequenceId = parameters.ProjectSequenceId,
                    TenantProvider = parameters.TenantProvider
                };
                var service =
                    await parameters.IPbsResourceRepository.ReadServiceByPbsProduct(pbsResourceParameters);
                var serviceCreate = new PbsServiceCreateDto();
                if (service.FirstOrDefault() != null)
                {
                    serviceCreate.ProductId = NewId;
                    serviceCreate.Mou = service.FirstOrDefault().Mou;
                    serviceCreate.Quantity = service.FirstOrDefault().Quantity;
                    serviceCreate.TotalPrice = service.FirstOrDefault().TotalPrice;
                    serviceCreate.UnitPrice = service.FirstOrDefault().UnitPrice;
                    serviceCreate.Comments = service.FirstOrDefault().Comments;
                    serviceCreate.Documents = service.FirstOrDefault().Documents;
                    pbsResourceParameters.PbsServiceCreateDto = serviceCreate;
                    await parameters.IPbsResourceRepository.CreatePbsService(pbsResourceParameters);
                }

                //Clone Taxonomy_Level
                var taxonomiesquery =
                    @"SELECT [Id] FROM [dbo].[PbsProductTaxonomy] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";


                IEnumerable<PbsCloneDto> taxonomiesdata;
                await using (var dbConnection = new SqlConnection(connectionString))
                {
                    taxonomiesdata = dbConnection.Query<PbsCloneDto>(taxonomiesquery, parameters7);
                }

                foreach (var b in taxonomiesdata)
                {
                    var taxonomyquery = @"INSERT INTO [dbo].[PbsProductTaxonomy]
                                                           ([Id]
                                                           ,[PbsProductId]
                                                           ,[PbsTaxonomyId]
                                                           ,[PbsTaxonomyNodeId])
                                                     VALUES
                                                           (@TaxonomyId
                                                           ,@ProductId
                                                           ,(SELECT [PbsTaxonomyId] FROM [dbo].[PbsProductTaxonomy] WHERE Id= @Id )
                                                           ,(SELECT [PbsTaxonomyNodeId] FROM [dbo].[PbsProductTaxonomy] WHERE Id = @Id))";


                    var parameters12 = new
                    {
                        b.Id,
                        TaxonomyId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };
                    IEnumerable<PbsCloneDto> taxonomydata;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        taxonomydata = dbConnection.Query<PbsCloneDto>(taxonomyquery, parameters12);
                    }
                }

                //Colne Competencies
                var competenciesquery =
                    @"SELECT [Id] FROM [dbo].[PbsSkillExperience] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> competenciesdata;
                await using (var dbConnection = new SqlConnection(connectionString))
                {
                    competenciesdata = dbConnection.Query<PbsCloneDto>(competenciesquery, parameters7);
                }

                foreach (var a in competenciesdata)
                {
                    var competencquery = @"INSERT INTO [dbo].[PbsSkillExperience]
                                                                   ([Id]
                                                                   ,[PbsProductId]
                                                                   ,[PbsSkillId]
                                                                   ,[PbsExperienceId])
                                                             VALUES
                                                                   (@CompetencId
                                                                   ,@ProductId 
                                                                   ,(select [PbsSkillId] from [dbo].[PbsSkillExperience] where Id = @Id)
                                                                   ,(select [PbsExperienceId] from [dbo].[PbsSkillExperience] where Id = @Id))";

                    var parameters11 = new
                    {
                        a.Id,
                        CompetencId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };
                    IEnumerable<PbsCloneDto> competencdata;
                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        competencdata = dbConnection.Query<PbsCloneDto>(competencquery, parameters11);
                    }
                }


                //Clone Material
                var materialsquery =
                    @"SELECT [Id] FROM [dbo].[PbsMaterial] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> materialsdata;
                await using (var dbConnection = new SqlConnection(connectionString))
                {
                    materialsdata = dbConnection.Query<PbsCloneDto>(materialsquery, parameters7);
                }

                foreach (var m in materialsdata)
                {
                    var matquery = @"INSERT INTO [dbo].[PbsMaterial]
                                               ([Id]
                                               ,[PbsProductId]
                                               ,[CoperateProductCatalogId]
                                               ,[Quantity])
                                         VALUES
                                               (@MatId
                                               ,@ProductId
                                               ,(SELECT [CoperateProductCatalogId] FROM [dbo].[PbsMaterial] where Id = @Id)
                                               ,(SELECT [Quantity] FROM [dbo].[PbsMaterial] where Id = @Id))";

                    var parameters3 = new
                    {
                        m.Id,
                        MatId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };
                    IEnumerable<PbsCloneDto> matdata;
                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        matdata = dbConnection.Query<PbsCloneDto>(matquery, parameters3);
                    }
                }

                //Clone Tool
                var toolsquery =
                    @"SELECT [Id] FROM [dbo].[PbsTools] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> toolsdata;
                using (var dbConnection = new SqlConnection(connectionString))
                {
                    toolsdata = dbConnection.Query<PbsCloneDto>(toolsquery, parameters7);
                }

                foreach (var t in toolsdata)
                {
                    var toolquery = @"INSERT INTO [dbo].[PbsTools]
                                               ([Id]
                                               ,[PbsProductId]
                                               ,[CoperateProductCatalogId]
                                               ,[Quantity])
                                         VALUES
                                               (@ToolId
                                               ,@ProductId
                                               ,(SELECT [CoperateProductCatalogId] FROM [dbo].[PbsTools] where Id = @Id)
                                               ,(SELECT [Quantity] FROM [dbo].[PbsTools] where Id = @Id))";

                    var parameters4 = new
                    {
                        t.Id,
                        ToolId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };
                    //IEnumerable<PbsCloneDto> tooldata;
                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        dbConnection.Query<PbsCloneDto>(toolquery, parameters4);
                    }
                }

                //Clone Consumables
                var consumablesquery =
                    @"SELECT [Id] FROM [dbo].[PbsConsumable] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> consumablesdata;
                using (var dbConnection = new SqlConnection(connectionString))
                {
                    consumablesdata = dbConnection.Query<PbsCloneDto>(consumablesquery, parameters7);
                }

                foreach (var c in consumablesdata)
                {
                    var consumablequery = @"INSERT INTO [dbo].[PbsConsumable]
                                               ([Id]
                                               ,[PbsProductId]
                                               ,[CoperateProductCatalogId]
                                               ,[Quantity])
                                         VALUES
                                               (@ConsumablesId
                                               ,@ProductId
                                               ,(SELECT [CoperateProductCatalogId] FROM [PbsConsumable] where Id = @Id)
                                               ,(SELECT [Quantity] FROM [dbo].[PbsConsumable] where Id = @Id))";

                    var parameters5 = new
                    {
                        c.Id,
                        ConsumablesId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };
                    IEnumerable<PbsCloneDto> consumabledata;
                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        consumabledata = dbConnection.Query<PbsCloneDto>(consumablequery, parameters5);
                    }
                }

                //Clone  Labour
                var laboursquery =
                    @"SELECT [Id] FROM [dbo].[PbsLabour] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> laboursdata;
                await using (var dbConnection = new SqlConnection(connectionString))
                {
                    laboursdata = dbConnection.Query<PbsCloneDto>(laboursquery, parameters7);
                }

                foreach (var l in laboursdata)
                {
                    var labourquery = @"INSERT INTO [dbo].[PbsLabour]
                                               ([Id]
                                               ,[PbsProductId]
                                               ,[CoperateProductCatalogId]
                                               ,[Quantity])
                                         VALUES
                                               (@labourId
                                               ,@ProductId
                                               ,(SELECT [CoperateProductCatalogId] FROM [dbo].[PbsLabour] where Id = @Id)
                                               ,(SELECT [Quantity] FROM [dbo].[PbsLabour] where Id = @Id))";

                    var parameters6 = new
                    {
                        l.Id,
                        labourId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };

                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        dbConnection.Query<PbsCloneDto>(labourquery, parameters6);
                    }
                }

                //Clone Intructions
                var intructionsquery =
                    @"SELECT [Id] FROM [dbo].[PbsInstruction] WHERE PbsProductId = (SELECT  TOP 1 [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> intructionsdata;
                await using (var dbConnection = new SqlConnection(connectionString))
                {
                    intructionsdata = dbConnection.Query<PbsCloneDto>(intructionsquery, parameters7);
                }

                foreach (var i in intructionsdata)
                {
                    var intructionquery = @"INSERT INTO [dbo].[PbsInstruction]
                                                                   ([Id]
                                                                   ,[PbsProductId]
                                                                   ,[PbsInstructionFamilyId]
                                                                   ,[InstructionsDetails]
                                                                   ,[IsDeleted]
                                                                   ,[IsSaved]
                                                                   ,[Name]
                                                                   ,[SequenceCode]
                                                                   ,[InstructionType])

                                                         SELECT	 @InsructionId
                                                           ,@ProductId
                                                           ,[PbsInstructionFamilyId]
                                                           ,[InstructionsDetails]
                                                           ,[IsDeleted]
                                                           ,[IsSaved]
                                                           ,[Name]
                                                           ,[SequenceCode]
                                                           ,[InstructionType]
                                                         FROM [dbo].[PbsInstruction] 
                                                         WHERE [Id] =@Id";
                    var parameters8 = new
                    {
                        i.Id,
                        InsructionId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };


                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        dbConnection.Query<PbsCloneDto>(intructionquery, parameters8);
                    }
                }

                //Clone Risk
                var risksquery =
                    @"SELECT [Id] FROM [dbo].[PbsRisk] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> risksdata;
                using (var dbConnection = new SqlConnection(connectionString))
                {
                    risksdata = dbConnection.Query<PbsCloneDto>(risksquery, parameters7);
                }

                foreach (var n in risksdata)
                {
                    var riskquery = @"INSERT INTO [dbo].[PbsRisk]
                                                                   ([Id]
                                                                   ,[PbsProductId]
                                                                   ,[RiskId])

                                                         SELECT	 @RiskId
                                                           ,@ProductId
                                                           ,[RiskId]
                                                         FROM [dbo].[PbsRisk] 
                                                         WHERE [Id] =@Id";
                    var parameters9 = new
                    {
                        n.Id,
                        RiskId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };

                    IEnumerable<PbsCloneDto> riskdata;
                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        riskdata = dbConnection.Query<PbsCloneDto>(riskquery, parameters9);
                    }
                }

                //Clone Quality
                var qualityquery =
                    @"SELECT [Id] FROM [dbo].[PbsQuality] WHERE PbsProductId = (SELECT [Id] FROM [dbo].[PbsProduct] where ProductId = @Id)";

                IEnumerable<PbsCloneDto> qualitydata;
                using (var dbConnection = new SqlConnection(connectionString))
                {
                    qualitydata = dbConnection.Query<PbsCloneDto>(qualityquery, parameters7);
                }

                foreach (var n in qualitydata)
                {
                    var quaquery = @"INSERT INTO [dbo].[PbsQuality]
                                                                   ([Id]
                                                                   ,[PbsProductId]
                                                                   ,[QualityId])

                                                         SELECT	 @QuaId
                                                           ,@ProductId
                                                           ,[QualityId]
                                                         FROM [dbo].[PbsQuality]
                                                         WHERE [Id] =@Id";
                    var parameters10 = new
                    {
                        n.Id,
                        QuaId = Guid.NewGuid().ToString(),
                        ProductId = NewId
                    };

                    //IEnumerable<PbsCloneDto> quadata;
                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        dbConnection.Query<PbsCloneDto>(quaquery, parameters10);
                    }
                }
            }

            return NewId;
        }

        catch (Exception e)
        {
            throw e;
        }
    }

    public Task<ProductResourceListGetByIdsDto> ProductResourcesByIdById(PbsParameters pbsParameters)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            var lang = pbsParameters.Lang;

            var data = new ProductResourceListGetByIdsDto();

            var labourQuery = @"select PbsLabour.Id,
                                        PbsLabour.PbsProductId,
                                        PbsLabour.CoperateProductCatalogId As CorporateProductCatalogId,
                                        PbsLabour.Quantity,
                                        PbsProduct.Title As PbsTitle,
                                        CpcBasicUnitOfMeasureLocalizedData.Label As Mou,
                                        CorporateProductCatalog.Title,
                                        CorporateProductCatalog.ResourceNumber,
                                        CorporateProductCatalog.InventoryPrice
                                        from PbsLabour left outer join PbsProduct on PbsLabour.PbsProductId = PbsProduct.Id
                                        left outer join CorporateProductCatalog on PbsLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                                        inner join CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId =CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId 
                                        where (CpcBasicUnitOfMeasureLocalizedData.LanguageCode =@lang AND PbsLabour.PbsProductId IN @Ids)";

            var toolsQuery = @"select PbsTools.Id,
                                        PbsTools.PbsProductId,
                                        PbsTools.CoperateProductCatalogId As CorporateProductCatalogId,
                                        PbsTools.Quantity,
                                        PbsProduct.Title As PbsTitle,
                                        CpcBasicUnitOfMeasureLocalizedData.Label As Mou,
                                        CorporateProductCatalog.Title,
                                        CorporateProductCatalog.ResourceNumber,
                                        CorporateProductCatalog.InventoryPrice
                                        from PbsTools left outer join PbsProduct on PbsTools.PbsProductId = PbsProduct.Id
                                        left outer join CorporateProductCatalog on PbsTools.CoperateProductCatalogId = CorporateProductCatalog.Id
                                        inner join CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId =CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId 
                                        where (CpcBasicUnitOfMeasureLocalizedData.LanguageCode =@lang AND PbsTools.PbsProductId In @Ids)";
            var param = new
            {
                Ids = pbsParameters.IdList, lang
            };
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                data.Tools = dbConnection.Query<GetToolsForPbsDto>(toolsQuery, param);
                data.Labour = dbConnection.Query<GetLabourForPbsDto>(labourQuery, param);
            }

            return Task.FromResult(data);
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public Task<IEnumerable<PbsTreeStructurefroProjectPlanningDto>> GetUtilityTaxonomyForProjectPlanning(
        PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        try
        {
            var utility =
                @"select p.Id, p.Title as Title, t.PbsTaxonomyNodeId AS ParentId,p.ProductId AS PbsSequenceId,p.StartDate,p.EndDate,'PBS' AS Type from PbsProduct p inner join PbsProductTaxonomy t on p.Id = t.PbsProductId where(t.PbsTaxonomyId = '6e54725c-e396-4ce4-88f3-a6e9678a0389' ) and p.IsDeleted = 0";

            var sb = new StringBuilder(utility);

            if ((pbsParameters.PbsTreeStructureFilter.FromDate != null) &
                (pbsParameters.PbsTreeStructureFilter.ToDate != null))
                // product.Append("AND (( t1.startdate >= '" + VPParameter.PbsTreeStructureFilter.FromDate +"' AND t1.enddate <= '" + VPParameter.PbsTreeStructureFilter.ToDate + "') OR ( t1.startdate >= '" + VPParameter.PbsTreeStructureFilter.FromDate +
                //                "' AND t1.startdate <= '" + VPParameter.PbsTreeStructureFilter.ToDate + "')) ");
                sb.Append(" AND (( p.StartDate BETWEEN '" + pbsParameters.PbsTreeStructureFilter.FromDate + "' AND '" +
                          pbsParameters.PbsTreeStructureFilter.ToDate + "') OR ( p.EndDate BETWEEN '" +
                          pbsParameters.PbsTreeStructureFilter.FromDate +
                          "' AND '" + pbsParameters.PbsTreeStructureFilter.ToDate + "') OR ( p.StartDate <= '" +
                          pbsParameters.PbsTreeStructureFilter.FromDate +
                          "' AND p.EndDate >= '" + pbsParameters.PbsTreeStructureFilter.ToDate + "')) ");

            if (pbsParameters.PbsTreeStructureFilter.Title != null)
            {
                pbsParameters.PbsTreeStructureFilter.Title =
                    pbsParameters.PbsTreeStructureFilter.Title.Replace("'", "''");
                sb.Append(" AND p.Title LIKE '%" + pbsParameters.PbsTreeStructureFilter.Title + "%'");
            }

            IEnumerable<PbsTreeStructurefroProjectPlanningDto> resultUtility;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                resultUtility = dbConnection.Query<PbsTreeStructurefroProjectPlanningDto>(sb.ToString()).ToList();
            }

            return Task.FromResult(resultUtility);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public Task<IEnumerable<PbsTreeStructurefroProjectPlanningDto>> GetLocationTaxonomyForProjectPlanning(
        PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        try
        {
            var location = @"SELECT
                                  p.id
                                 ,p.Title AS Title
                                 ,t.PbsTaxonomyNodeId AS ParentId
                                 ,p.EndDate
                                 ,p.StartDate
                                 ,p.PbsType AS Type
                                FROM PbsProduct p
                                INNER JOIN PbsProductTaxonomy t
                                  ON p.id = t.PbsProductId
                                WHERE (t.PbsTaxonomyId = 'ab294299-f251-41a8-94bd-3ae0150df134')
                                AND p.IsDeleted = 0";

            var sb = new StringBuilder(location);

            if ((pbsParameters.PbsTreeStructureFilter.FromDate != null) &
                (pbsParameters.PbsTreeStructureFilter.ToDate != null))
                // product.Append("AND (( t1.startdate >= '" + VPParameter.PbsTreeStructureFilter.FromDate +"' AND t1.enddate <= '" + VPParameter.PbsTreeStructureFilter.ToDate + "') OR ( t1.startdate >= '" + VPParameter.PbsTreeStructureFilter.FromDate +
                //                "' AND t1.startdate <= '" + VPParameter.PbsTreeStructureFilter.ToDate + "')) ");
                sb.Append(" AND (( p.StartDate BETWEEN '" + pbsParameters.PbsTreeStructureFilter.FromDate + "' AND '" +
                          pbsParameters.PbsTreeStructureFilter.ToDate + "') OR ( p.EndDate BETWEEN '" +
                          pbsParameters.PbsTreeStructureFilter.FromDate +
                          "' AND '" + pbsParameters.PbsTreeStructureFilter.ToDate + "') OR ( p.StartDate <= '" +
                          pbsParameters.PbsTreeStructureFilter.FromDate +
                          "' AND p.EndDate >= '" + pbsParameters.PbsTreeStructureFilter.ToDate + "')) ");

            if (pbsParameters.PbsTreeStructureFilter.Title != null)
            {
                pbsParameters.PbsTreeStructureFilter.Title =
                    pbsParameters.PbsTreeStructureFilter.Title.Replace("'", "''");
                sb.Append(" AND p.Title LIKE '%" + pbsParameters.PbsTreeStructureFilter.Title + "%'");
            }

            IEnumerable<PbsTreeStructurefroProjectPlanningDto> resultlocation;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                resultlocation = dbConnection.Query<PbsTreeStructurefroProjectPlanningDto>(sb.ToString()).ToList();
            }

            return Task.FromResult(resultlocation);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> ExcelUpload(PbsParameters parameters)
    {
        var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            parameters.ProjectSequenceId, parameters.TenantProvider);

        var PbsList = new List<PbsProduct>();

        var allListArray = new List<string>();
        var allDynListArray = new List<string>();

        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("Delete From PbsProduct");
        await connection.ExecuteAsync("Delete From CBCDynamicsAttributes");


        // Parallel.ForEach(parameters.UploadExcelDto, dto =>
        // {
        foreach (var dto in parameters.UploadExcelDto)
        {
            dto.Title = dto.Title?.Replace("'", "''");


            // PbsParameters pbsParameters = new PbsParameters
            // {
            //     ContractingUnitSequenceId = parameters.ContractingUnitSequenceId,
            //     ProjectSequenceId = parameters.ProjectSequenceId,
            //     ChangedUser = parameters.ChangedUser
            // };
            // pbsParameters.ChangedUser = parameters.ChangedUser;
            // pbsParameters.Lang = parameters.Lang;
            // pbsParameters.ContextAccessor = parameters.ContextAccessor;
            // pbsParameters.TenantProvider = parameters.TenantProvider;
            // var dtoNew = new PbsProductCreateDto
            // {
            //     Id = Guid.NewGuid().ToString(),
            //     ParentId = dto.ParentId,
            //     Name = dto.Title,
            //     PbsType = "sub",
            //     PbsProductItemTypeId = "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            //     Scope = "0",
            //     PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            //     PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
            //     Contract = "Lot",
            //     Quantity = dto.Quantity,
            //     Mou = dto.Mou,
            //     MeasurementCode = dto.MeasurementCode,
            //     ProductId = dto.Id,
            //     ArticleNumber = dto.Id,
            //      //NodeType = "P",
            //     // PbsTaxonomyLevelId = "93d01fef-ccc7-4592-9544-3949d67cbf76",
            //      //Title = dto.Id +"-"+dto.Title
            // };

            var values =
                @"('{0}' ,'{1}' ,'{2}' ,'48a7dd9c-55ac-4e7c-a2f3-653811c0eb14' ,'d60aad0b-2e84-482b-ad25-618d80d49477' ,'0' ,'Lot' ,'004eb795-8bba-47e8-9049-d14774ab0b18' ,0 ,'P' ,'93d01fef-ccc7-4592-9544-3949d67cbf76' ,'{3}' ,'sub' ,'{4}' ,'{5}' ,'{6}' ,'{7}' )";

            var formattedString = string.Format(values, Guid.NewGuid().ToString(), dto.Id, dto.Title,
                dto.Id + "-" + dto.Title, dto.ParentId, dto.Quantity, dto.MeasurementCode, dto.Mou);

            allListArray.Add(formattedString);

            // var pbsParam = new
            // {
            //     Id = Guid.NewGuid().ToString(),
            //     dto.ParentId,
            //     Name = dto.Title,
            //     dto.Quantity,
            //     dto.Mou,
            //     dto.MeasurementCode,
            //     ProductId = dto.Id,
            //     ArticleNumber = dto.Id,
            //     Title = dto.Id + "-" + dto.Title
            // };
            // pbsParameters.PbsDto = dtoNew;

            // PbsList.Add(dtoNew);

            //await CreatePbs(pbsParameters).ConfigureAwait(false);

            //connection.Execute(insertPbs, pbsParam);

            // var attributesSql = @"INSERT INTO dbo.CBCDynamicsAttributes
            //                         (
            //                           Id
            //                          ,ArticleNumber
            //                          ,Key1
            //                          ,Value1
            //                          ,Key2
            //                          ,Value2
            //                          ,Key3
            //                          ,Value3
            //                          ,Key4
            //                          ,Value4
            //                          ,Key5
            //                          ,Value5
            //                         )
            //                         VALUES
            //                         (
            //                           @Id
            //                          ,@ArticleNumber
            //                          ,@Key1
            //                          ,@Value1
            //                          ,@Key2
            //                          ,@Value2
            //                          ,@Key3
            //                          ,@Value3
            //                          ,@Key4
            //                          ,@Value4
            //                          ,@Key5
            //                          ,@Value5
            //                         )";

            dto.Value1 = dto.Value1?.Replace("'", "''");
            dto.Value2 = dto.Value2?.Replace("'", "''");
            dto.Value3 = dto.Value3?.Replace("'", "''");
            dto.Value4 = dto.Value4?.Replace("'", "''");
            dto.Value5 = dto.Value5?.Replace("'", "''");
            dto.Key1 = dto.Key1?.Replace("'", "''");
            dto.Key2 = dto.Key2?.Replace("'", "''");
            dto.Key3 = dto.Key3?.Replace("'", "''");
            dto.Key4 = dto.Key4?.Replace("'", "''");
            dto.Key5 = dto.Key5?.Replace("'", "''");


            var dynValues = @"('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}' )";

            var dynFormattedString = string.Format(dynValues, Guid.NewGuid().ToString(), dto.Id, dto.Value1, dto.Value2,
                dto.Value3, dto.Value4, dto.Value5, dto.Key1, dto.Key2, dto.Key3, dto.Key4, dto.Key5);

            allDynListArray.Add(dynFormattedString);

            // var param = new CBCDynamicsAttributes
            // {
            //     Id = Guid.NewGuid().ToString(),
            //     ArticleNumber = dto.Id,
            //     Value1 = dto.Value1,
            //     Value2 = dto.Value2,
            //     Value3 = dto.Value3,
            //     Value4 = dto.Value4,
            //     Value5 = dto.Value5,
            //     Key1 = dto.Key1,
            //     Key2 = dto.Key2,
            //     Key3 = dto.Key3,
            //     Key4 = dto.Key4,
            //     Key5 = dto.Key5
            // };

            //connection.Execute(attributesSql, param);
        }

        var chunkedArray = allListArray.Chunk(500);

        foreach (var chunk in chunkedArray)
        {
            var insertPbs = @"INSERT INTO dbo.PbsProduct
                            (
                              Id
                             ,ProductId
                             ,Name
                             ,PbsProductItemTypeId
                             ,PbsProductStatusId
                             ,Scope
                             ,Contract
                             ,PbsToleranceStateId
                             ,IsDeleted
                             ,NodeType
                             ,PbsTaxonomyLevelId
                             ,Title
                             ,PbsType
                             ,ParentId
                             ,Quantity
                             ,MeasurementCode
                             ,Mou
                            )
                            VALUES ";

            var concatedString = chunk.Aggregate((x, y) => x + ", " + y);

            insertPbs += concatedString;

            await connection.ExecuteAsync(insertPbs);
        }

        var dynChunkedArray = allDynListArray.Chunk(500);

        foreach (var chunk in dynChunkedArray)
        {
            var attributesSql = @"INSERT INTO dbo.CBCDynamicsAttributes
                                    (
                                      Id
                                     ,ArticleNumber
                                     ,Key1
                                     ,Value1
                                     ,Key2
                                     ,Value2
                                     ,Key3
                                     ,Value3
                                     ,Key4
                                     ,Value4
                                     ,Key5
                                     ,Value5
                                    )
                                    VALUES ";

            var dynConcatedString = chunk.Aggregate((x, y) => x + ", " + y);

            attributesSql += dynConcatedString;

            await connection.ExecuteAsync(attributesSql);
        }


        // DapperPlusManager.Entity<PbsProduct>()
        //       .Table("PbsProduct")
        //       //.Map(m => m.Id, nameof(PbsProduct.Id))
        //       .Map(m => m.ParentId, nameof(PbsProduct.ParentId))
        //       .Map(m => m.Name, nameof(PbsProduct.Name))
        //       .Map(m => m.PbsType, nameof(PbsProduct.PbsType))
        //       .Map(m => m.PbsProductItemTypeId, nameof(PbsProduct.PbsProductItemTypeId))
        //       .Map(m => m.Scope, nameof(PbsProduct.Scope))
        //       .Map(m => m.PbsProductStatusId, nameof(PbsProduct.PbsProductStatusId))
        //       .Map(m => m.PbsToleranceStateId, nameof(PbsProduct.PbsToleranceStateId))
        //       .Map(m => m.Contract, nameof(PbsProduct.Contract))
        //       .Map(m => m.Quantity, nameof(PbsProduct.Quantity))
        //       .Map(m => m.Mou, nameof(PbsProduct.Mou))
        //       .Map(m => m.MeasurementCode, nameof(PbsProduct.MeasurementCode))
        //       .Map(m => m.ProductId, nameof(PbsProduct.ProductId))
        //       .Map(m => m.NodeType, nameof(PbsProduct.NodeType))
        //       .Map(m => m.PbsTaxonomyLevelId, nameof(PbsProduct.PbsTaxonomyLevelId))
        //       .Map(m => m.Title, nameof(PbsProduct.Title))
        //       .Key(x => x.Id);
        //
        // connection.BulkInsert(PbsList);


        //});
        return "ok";
    }

    public async Task<List<PbsTreeStructure>> GetMachineTaxonomyForProjectPlanning(
        PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        var location = @"SELECT Id,Title,ParentId FROM dbo.MachineTaxonmy Where Id IS NOT NULL ";

        var sb = new StringBuilder(location);

        if (pbsParameters.PbsTreeStructureFilter.Title != null)
        {
            pbsParameters.PbsTreeStructureFilter.Title = pbsParameters.PbsTreeStructureFilter.Title.Replace("'", "''");
            sb.Append(" AND Title LIKE '%" + pbsParameters.PbsTreeStructureFilter.Title + "%'");
        }


        List<PbsTreeStructure> resultmachine;
        var resultPbs = new List<PbsTreeStructure>();

        await using var dbConnection = new SqlConnection(connectionString);

        resultmachine = dbConnection.Query<PbsTreeStructure>(sb.ToString()).ToList();


        foreach (var item in resultmachine)
        {
            var pbs = dbConnection
                .Query<PbsTreeStructure>(
                    "SELECT Id, Title,StartDate,EndDate ,ProductId As PbsSequenceId , MachineTaxonomy AS ParentId FROM PbsProduct WHERE MachineTaxonomy = @MachineTaxonomy",
                    new { MachineTaxonomy = item.Id }).ToList();

            resultPbs.AddRange(pbs);
        }

        resultmachine.AddRange(resultPbs);

        return resultmachine;
    }

    public async Task<string> CreatePbsScopeOfWork(
        PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        await using var connection = new SqlConnection(connectionString);


        var sql = @"INSERT INTO dbo.PbsScopeOfWork
                                (
                                  Id
                                 ,ProductId
                                 ,Quantity
                                 ,MouId
                                 ,UnitPrice
                                 ,TotalPrice
                                )
                                VALUES
                                (
                                 @Id
                                 ,@ProductId
                                 ,@Quantity
                                 ,@MouId
                                 ,@UnitPrice
                                 ,@TotalPrice
                                ) ";

        var updateSql = @"UPDATE dbo.PbsScopeOfWork 
                            SET
                 
                                Quantity = @Quantity
                                ,MouId =@MouId
                                ,UnitPrice = @UnitPrice
                                ,TotalPrice = @TotalPrice
                            WHERE
                                Id = @Id";

        if (pbsParameters.PbsSquareMeters.Id != null)
        {
            var isExist = connection.Query<PbsScopeOfWork>("Select * From PbsScopeOfWork Where Id = @Id",
                new { pbsParameters.PbsSquareMeters.Id }).FirstOrDefault();

            if (isExist != null)
            {
                var updateParam = new
                {
                    pbsParameters.PbsSquareMeters.Id,
                    pbsParameters.PbsSquareMeters.Quantity,
                    pbsParameters.PbsSquareMeters.MouId,
                    pbsParameters.PbsSquareMeters.UnitPrice,
                    pbsParameters.PbsSquareMeters.TotalPrice
                };

                await connection.ExecuteAsync(updateSql, updateParam);
            }
            else
            {
                var insertParam = new
                {
                    pbsParameters.PbsSquareMeters.Id,
                    pbsParameters.PbsSquareMeters.ProductId,
                    pbsParameters.PbsSquareMeters.Quantity,
                    pbsParameters.PbsSquareMeters.MouId,
                    pbsParameters.PbsSquareMeters.UnitPrice,
                    pbsParameters.PbsSquareMeters.TotalPrice
                };

                await connection.ExecuteAsync(sql, insertParam);
            }
        }

        return pbsParameters.PbsSquareMeters.Id;
    }

    public async Task<IEnumerable<PbsForWeekPlanDto>> GetPbsLabour(PbsParameters pbsParameters)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            var query = @"SELECT
                              PbsLabour.Id AS PbsLabourId
                             ,PbsLabour.PbsProductId
                             ,CorporateProductCatalog.Title AS CpcTitle
                             ,CorporateProductCatalog.Id AS CpcId
                             ,PbsLabour.Quantity
                            FROM dbo.PbsLabour
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON PbsLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                            LEFT OUTER JOIN dbo.PbsProduct
                              ON PbsLabour.PbsProductId = PbsProduct.Id
                            WHERE PbsProduct.ProductId = @Id";

            var pbslabour = connection.Query<PbsForWeekPlanDto>(query, new { pbsParameters.Id }).ToList();

            return pbslabour;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<AllPbsForWeekPlanDto>> GetAllPbsLabour(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var pbs = connection.Query<AllPbsForWeekPlanDto>("SELECT Id AS PbsId FROM dbo.PbsProduct;").ToList();

        foreach (var i in pbs)
        {
            var query = @"SELECT
                              PbsLabour.Id AS PbsLabourId
                             ,PbsLabour.PbsProductId
                             ,CorporateProductCatalog.Title AS CpcTitle
                             ,CorporateProductCatalog.Id AS CpcId
                             ,PbsLabour.Quantity
                            FROM dbo.PbsLabour
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON PbsLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                            LEFT OUTER JOIN dbo.PbsProduct
                              ON PbsLabour.PbsProductId = PbsProduct.Id
                            WHERE PbsProduct.Id = @PbsId";

            var pbslabour = connection.Query<PbsForWeekPlanDto>(query, new { i.PbsId }).ToList();

            i.PbsLabour = pbslabour;
        }

        return pbs;
    }

    public async Task<GetAllPmolLabourDto> GetAllPmolLabour(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        await using var dbconnection = new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString);

        var cabPerson = dbconnection.Query<CabPerson>("SELECT * FROM dbo.CabPerson").ToList();

        var pmolLabourItem = connection.Query<GetAllPmolLabourDto>(
                "SELECT PMolPlannedWorkLabour.Id ,PMolPlannedWorkLabour.CoperateProductCatalogId AS CpcId ,PMolPlannedWorkLabour.PmolId FROM dbo.PMolPlannedWorkLabour INNER JOIN dbo.PMol ON PMolPlannedWorkLabour.PmolId = PMol.Id WHERE ProjectMoleculeId = @PmolSequenceId AND PMolPlannedWorkLabour.CoperateProductCatalogId = @CpcId",
                new { pbsParameters.GetPmolLabourDto.PmolSequenceId, pbsParameters.GetPmolLabourDto.CpcId })
            .FirstOrDefault();

        var labour = connection
            .Query<PmolLabour>(
                "SELECT PmolTeamRole.CabPersonId ,Role.RoleName ,Role.RoleId ,PmolTeamRole.PmolLabourId FROM dbo.PmolTeamRole LEFT OUTER JOIN dbo.Role ON PmolTeamRole.RoleId = Role.RoleId WHERE IsDeleted = '0';")
            .ToList();

        pmolLabourItem.PmolLabour = labour.Where(e => e.PmolLabourId == pmolLabourItem.Id).ToList();

        foreach (var n in pmolLabourItem.PmolLabour)
        {
            var person = cabPerson.Where(e => e.Id == n.CabPersonId).FirstOrDefault();
            n.CabPersonName = person.FullName;
        }

        var labourList = pmolLabourItem.PmolLabour.OrderBy(x => x.CabPersonName).ToList();
        pmolLabourItem.PmolLabour = labourList;

        return pmolLabourItem;
    }

    private async Task<IEnumerable<PbsProductItemTypeDto>> GetProductItemType(PbsParameters pbsParameters)
    {
        try
        {
            var query = @"
                              select type.PbsProductItemTypeId as Id, type.Label as Name 
                              from PbsProductItemTypeLocalizedData type where LanguageCode= @lang
                              order by type.Label
                             ";

            var parameters = new { lang = pbsParameters.Lang };
            using (var dbConnection =
                   new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString))
            {
                await dbConnection.OpenAsync();
                var result = await dbConnection.QueryAsync<PbsProductItemTypeDto>(query, parameters);


                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private async Task<IEnumerable<PbsProductStatusDto>> GetProductStatus(PbsParameters pbsParameters)
    {
        try
        {
            var query = @"
                                select status.PbsProductStatusId as Id, status.Label as Name
                                from PbsProductStatusLocalizedData status where LanguageCode= @lang
                                order by DisplayOrder
                             ";

            var parameters = new { lang = pbsParameters.Lang };
            using (var dbConnection =
                   new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString))
            {
                await dbConnection.OpenAsync();
                var result = await dbConnection.QueryAsync<PbsProductStatusDto>(query, parameters);


                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private async Task<IEnumerable<PbsToleranceStateDto>> GetPbsToleranceState(PbsParameters pbsParameters)
    {
        try
        {
            var query = @"
                               select state.PbsToleranceStateId as Id, state.Label as Name
                               from PbsToleranceStateLocalizedData state where LanguageCode= @lang
                               order by state.Label
                             ";

            var parameters = new { lang = pbsParameters.Lang };
            using (var dbConnection =
                   new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString))
            {
                await dbConnection.OpenAsync();
                var result = await dbConnection.QueryAsync<PbsToleranceStateDto>(query, parameters);


                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private async void CreatePbsProductTaxonomy(PbsParameters parameters, string id, bool isIdExist)
    {
        //var options1 = new DbContextOptions<ApplicationDbContext>();
        var options = new DbContextOptions<PbsDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            parameters.ProjectSequenceId, parameters.TenantProvider);
        using (var context = new PbsDbContext(options, connectionString, parameters.TenantProvider))
        {
            if (isIdExist == false)
            {
                var newTaxonomy = new PbsProductTaxonomy
                {
                    Id = Guid.NewGuid().ToString(),
                    PbsProductId = id
                };
                context.PbsProductTaxonomy.Add(newTaxonomy);
                await context.SaveChangesAsync();
            }
            else
            {
                if (parameters.PbsDto.UtilityTaxonomyParentId == null)
                {
                    var nodeTaxonomy = context.PbsProductTaxonomy
                        .FirstOrDefault(p => p.PbsProductId == id);
                    if (nodeTaxonomy != null)
                    {
                        context.PbsProductTaxonomy.Remove(nodeTaxonomy);
                        await context.SaveChangesAsync();
                    }
                }

                if (parameters.PbsDto.LocationTaxonomyParentId == null)
                {
                    var nodeTaxonomy = context.PbsProductTaxonomy
                        .FirstOrDefault(p => p.PbsProductId == id);
                    if (nodeTaxonomy != null)
                    {
                        context.PbsProductTaxonomy.Remove(nodeTaxonomy);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (parameters.PbsDto.UtilityTaxonomyParentId != null ||
                parameters.PbsDto.LocationTaxonomyParentId != null)
            {
                var utilityId = context.PbsTaxonomy.FirstOrDefault(p => p.Name == "Utility")?.Id;
                var locationId = context.PbsTaxonomy.FirstOrDefault(p => p.Name == "Location")?.Id;

                var utilityTaxonomy = context.PbsProductTaxonomy
                    .Where(p => p.PbsProductId == id).FirstOrDefault(p => p.PbsTaxonomyId == utilityId);

                var locationTaxonomy = context.PbsProductTaxonomy
                    .Where(p => p.PbsProductId == id).FirstOrDefault(p => p.PbsTaxonomyId == locationId);

                if (parameters.PbsDto.UtilityTaxonomyParentId != null)
                    if (utilityTaxonomy == null)
                    {
                        var nodeTaxonomy = context.PbsProductTaxonomy
                            .Where(p => p.PbsProductId == id)
                            .FirstOrDefault(p => p.PbsTaxonomyNodeId == null);
                        if (nodeTaxonomy != null)
                        {
                            nodeTaxonomy.PbsTaxonomyNodeId = parameters.PbsDto.UtilityTaxonomyParentId;
                            nodeTaxonomy.PbsTaxonomyId = utilityId;
                            context.PbsProductTaxonomy.Update(nodeTaxonomy);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            var utility = new PbsProductTaxonomy
                            {
                                Id = Guid.NewGuid().ToString(),
                                PbsTaxonomyNodeId = parameters.PbsDto.UtilityTaxonomyParentId,
                                PbsProductId = id,
                                PbsTaxonomyId = utilityId
                            };
                            context.PbsProductTaxonomy.Add(utility);
                            await context.SaveChangesAsync();
                        }
                    }

                    else
                    {
                        var update =
                            @"UPDATE dbo.PbsProductTaxonomy SET PbsTaxonomyNodeId = @PbsTaxonomyNodeId WHERE PbsProductId = @Id AND PbsTaxonomyId = '6e54725c-e396-4ce4-88f3-a6e9678a0389';";

                        await using (var dbConnection = new SqlConnection(connectionString))
                        {
                            await dbConnection.ExecuteAsync(update,
                                new { PbsTaxonomyNodeId = parameters.PbsDto.UtilityTaxonomyParentId, Id = id });
                        }
                    }

                if (parameters.PbsDto.LocationTaxonomyParentId != null)
                    if (locationTaxonomy == null)
                    {
                        var nodeTaxonomy = context.PbsProductTaxonomy
                            .Where(p => p.PbsProductId == id)
                            .FirstOrDefault(p => p.PbsTaxonomyNodeId == null);
                        if (nodeTaxonomy != null)
                        {
                            nodeTaxonomy.PbsTaxonomyNodeId = parameters.PbsDto.LocationTaxonomyParentId;
                            nodeTaxonomy.PbsTaxonomyId = locationId;
                            context.PbsProductTaxonomy.Update(nodeTaxonomy);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            var utility = new PbsProductTaxonomy();
                            utility.Id = Guid.NewGuid().ToString();
                            utility.PbsTaxonomyNodeId = parameters.PbsDto.LocationTaxonomyParentId;
                            utility.PbsProductId = id;
                            utility.PbsTaxonomyId = locationId;
                            context.PbsProductTaxonomy.Add(utility);
                            await context.SaveChangesAsync();
                        }
                    }

                    else
                    {
                        var update =
                            @"UPDATE dbo.PbsProductTaxonomy SET PbsTaxonomyNodeId = @PbsTaxonomyNodeId WHERE PbsProductId = @Id AND PbsTaxonomyId = 'ab294299-f251-41a8-94bd-3ae0150df134';";

                        await using (var dbConnection = new SqlConnection(connectionString))
                        {
                            await dbConnection.ExecuteAsync(update,
                                new { PbsTaxonomyNodeId = parameters.PbsDto.LocationTaxonomyParentId, Id = id });
                        }
                    }
            }
        }
    }

    private string CreatePbsQualityResponsibility(PbsParameters parameters)
    {
        var options = new DbContextOptions<PbsDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            parameters.ProjectSequenceId, parameters.TenantProvider);
        using var context = new PbsDbContext(options, connectionString, parameters.TenantProvider);
        var pbsQualityResponsibility = new PbsQualityResponsibility();
        if (parameters.PbsDto.QualityApproverId != null || parameters.PbsDto.QualityProducerId != null ||
            parameters.PbsDto.QualityReviewerId != null)
        {
            pbsQualityResponsibility.QualityApproverId = parameters.PbsDto.QualityApproverId;
            pbsQualityResponsibility.QualityProducerId = parameters.PbsDto.QualityProducerId;
            pbsQualityResponsibility.QualityReviewerId = parameters.PbsDto.QualityReviewerId;
            if (parameters.PbsDto.pbsQualityResponsibilityId == null)
            {
                pbsQualityResponsibility.Id = Guid.NewGuid().ToString();
                context.PbsQualityResponsibility.Add(pbsQualityResponsibility);
                context.SaveChanges();
            }
            else
            {
                pbsQualityResponsibility.Id = parameters.PbsDto.pbsQualityResponsibilityId;
                context.PbsQualityResponsibility.Update(pbsQualityResponsibility);
                context.SaveChanges();
            }
        }

        return pbsQualityResponsibility.Id;
    }

    public async Task<int> AddPbsTreeIndex(string Id, string Connection)
    {
        var query = @"SELECT MAX(TreeIndex) FROM PbsTreeIndex;";

        var maxIndex = -1;
        var insert =
            @"INSERT INTO dbo.PbsTreeIndex ( Id ,PbsProductId ,TreeIndex ) VALUES ( @Id ,@PbsProductId ,@TreeIndex );";
        await using var dbconnection = new SqlConnection(Connection);
        var data = dbconnection.Query<string>(query).FirstOrDefault();

        if (data != null)
        {
            maxIndex = data.ToInt();
        }

        var param = new
        {
            Id = Guid.NewGuid(),
            PbsProductId = Id,
            TreeIndex = maxIndex + 1
        };
        await dbconnection.ExecuteAsync(insert, param);

        return maxIndex;
    }

    public async Task<PbsRelationsDto> GetPbsRelations(PbsParameters parameters)
    {
        var connectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            parameters.ProjectSequenceId, parameters.TenantProvider);
        await using var connection = new SqlConnection(connectionString);

        var pbsRelations = new PbsRelationsDto();

        var bor = connection
            .Query<string>(
                @"SELECT Bor.Id FROM dbo.Bor LEFT OUTER JOIN PbsProduct pp ON Bor.PbsProductId = pp.Id WHERE pp.ProductId = @Id",
                new { parameters.Id }).FirstOrDefault();

        if (bor != null)
        {
            pbsRelations.IsBorDependent = true;
        }

        var cuConnectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            null, parameters.TenantProvider);
        await using var cuConnection = new SqlConnection(cuConnectionString);
        var pbsAssignedLabour = cuConnection
            .Query<string>(@"SELECT Id FROM dbo.PbsAssignedLabour WHERE PbsProduct = @Id", new { parameters.Id })
            .FirstOrDefault();

        if (pbsAssignedLabour != null)
        {
            pbsRelations.IsMidTermDependent = true;
        }

        return pbsRelations;
    }

    public async Task<PbsRelationsDto> GetCpcRelations(PbsParameters parameters)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(parameters.ContractingUnitSequenceId,
            parameters.ProjectSequenceId, parameters.TenantProvider);
        await using var cuConnection = new SqlConnection(cuConnectionString);
        var pbsAssignedLabour = cuConnection
            .Query<string>(
                @"SELECT * FROM dbo.PbsLabour WHERE PbsProductId = @PbsId AND CoperateProductCatalogId = @CpcID",
                new { parameters.CpcRelationsDto.PbsId, parameters.CpcRelationsDto.CpcId }).FirstOrDefault();

        var pbsRelations = new PbsRelationsDto();

        if (pbsAssignedLabour != null)
        {
            pbsRelations.IsMidTermDependent = true;
        }

        return pbsRelations;
    }

    public async Task<PbsCbcResources> AddPbsCbcResource(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        await using var connection = new SqlConnection(connectionString);

        const string insertSql =
            "INSERT INTO dbo.PbsCbcResources ( Id ,PbsId ,LotId ,ArticleNo ,Quantity,ConsumedQuantity,InvoicedQuantity ) VALUES ( @Id ,@PbsId ,@LotId ,@ArticleNo ,@Quantity,'0','0' )";

        const string updateSql = "UPDATE PbsCbcResources SET Quantity = @Quantity Where Id = @Id";

        if (pbsParameters.PbsCbcResourcesDto.Id == null)
        {
            pbsParameters.PbsCbcResourcesDto.Id = Guid.NewGuid().ToString();
            await connection.ExecuteAsync(insertSql, pbsParameters.PbsCbcResourcesDto);
        }
        else
        {
            await connection.ExecuteAsync(updateSql, pbsParameters.PbsCbcResourcesDto);
        }

        return pbsParameters.PbsCbcResourcesDto;
    }

    public async Task<List<string>> DeletePbsCbcResource(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        await using var connection = new SqlConnection(connectionString);

        const string sql = "Delete From PbsCbcResources Where Id IN @Ids";
        await connection.ExecuteAsync(sql, new { Ids = pbsParameters.IdList });


        return pbsParameters.IdList;
    }

    public async Task<List<GetPbsCbcResourcesDto>> GetPbsCbcResourcesById(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        const string sql = "GetPbsCbcResourcesById";
        // var sql = @"CREATE PROCEDURE GetPbsCbcResourcesById @Id nvarchar(450)
        //             AS
        //     SELECT pcr.id,pcr.ArticleNo,pcr.LotId,pcr.Quantity,pcr.PbsId,Concat(pcpd.ArticleNo,' - ',pcpd.Title) AS Title,pcpd.Unit FROM PbsCbcResources pcr LEFT OUTER JOIN ContractorTotalValuesPublished ctvp ON pcr.LotId = ctvp.LotId LEFT OUTER JOIN PublishedContractorsPdfData pcpd ON pcr.ArticleNo = pcpd.ArticleNo AND ctvp.CompanyId = pcpd.CompanyId 
        // WHERE pcr.PbsId = @Id AND ctvp.IsWinner = 1";

        var result = await connection.QueryAsync<GetPbsCbcResourcesDto>(sql, commandType: CommandType.StoredProcedure,
            param: new { Id = pbsParameters.Id });

        if (pbsParameters.Title != null)
        {
            pbsParameters.Title = pbsParameters.Title.Replace("'", "''");

            result = result.Where(x => x.Title.Contains(pbsParameters.Title));
        }
        return result.DistinctBy(x => x.Id).ToList();
    }

    public async Task<List<PmolDeliverableResults>> GetPmolDeliverablesByPbsId(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var results = new List<PmolDeliverableResults>();
        var GetPmolDeliverablesByPbsIdQuery =
            @"SELECT PMolExtraWorkFiles.Link,p.ProjectMoleculeId AS sequenceId,p.Title FROM PMolExtraWorkFiles LEFT OUTER JOIN PMolExtraWork pew ON PMolExtraWorkFiles.PmolExtraWorkId = pew.Id LEFT OUTER JOIN PMol p ON pew.PmolId = p.Id WHERE p.ProductId = @Id
                                                UNION
                                                SELECT pjp.Link,p.ProjectMoleculeId AS SequenceId,p.Title FROM PMolJournalPicture pjp LEFT OUTER JOIN PMolJournal pj ON pjp.PmolJournalId = pj.Id LEFT OUTER JOIN PMol p ON pj.PmolId = p.Id WHERE p.ProductId = @Id";

        var data = connection.Query<PmolDeliverableDto>(GetPmolDeliverablesByPbsIdQuery, new { Id = pbsParameters.Id });

        foreach (var item in data.GroupBy(x => x.SequenceId))
        {
            var pmolData = new PmolDeliverableResults()
            {
                Title = item.FirstOrDefault()?.Title,
                DocLinks = item.Select(x => x.Link).ToList()
            };

            results.Add(pmolData);
        }

        return results;
    }

    public async Task<PbsLotDataDto> GetPbsLotIdById(PbsParameters pbsParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        return connection
            .Query<PbsLotDataDto>(" SELECT PbsLotId FROM PbsProduct WHERE ProductId = @Id",
                new { Id = pbsParameters.Id }).FirstOrDefault();
    }

}