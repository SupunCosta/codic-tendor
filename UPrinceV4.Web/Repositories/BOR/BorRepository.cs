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
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.BOR;

public class BorRepository : IBorRepository
{
    public async Task<string> CreateBor(BorParameter borParameter)
    {
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, borParameter.TenantProvider);

        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        string ItemId = null;
        await using (var context = new ShanukaDbContext(options, connectionString, borParameter.TenantProvider))
        {
            var isUpdate = false;
            var isIdExist = context.Bor.Any(x => x.Id == borParameter.BorDto.Id);

            if (isIdExist == false)
            {
                var idGenerator = new IdGenerator();
                ItemId = idGenerator.GenerateId(applicationDbContext, "BOR-", "BorSequenceCode");

                var sb = new StringBuilder(
                    "insert into Bor values(@Id, @ItemId, @Name, @PbsProductId, @IsDeleted, @LocationParentId, @UtilityParentId, @BorStatusId, @Title, @BorTypeId, @ProjectSequenceCode, @EndDate, @StartDate);");

                await using (var connection = new SqlConnection(connectionString))
                {
                    if (borParameter.BorDto.Product != null)
                    {
                        if (borParameter.BorDto.Product.Id == null &&
                            borParameter.BorDto.Product.ProductId == null &&
                            borParameter.BorDto.BorTypeId != "6610e768-3e06-po02-b337-ee367a82ad66")
                            throw new Exception("Product Id cannot be null");

                        var pbsId = connection
                            .Query<string>("SELECT Id FROM dbo.PbsProduct WHERE ProductId = @ProductId",
                                new { borParameter.BorDto.Product.ProductId }).FirstOrDefault();
                        borParameter.BorDto.Product.Id = pbsId;
                        var affectedRows = connection.ExecuteAsync(sb.ToString(), new
                        {
                            borParameter.BorDto.Id, ItemId, borParameter.BorDto.Name,
                            PbsProductId = borParameter.BorDto.Product.Id, IsDeleted = 0,
                            LocationParentId = borParameter.BorDto.Product.LocationTaxonomyParentId,
                            UtilityParentId = borParameter.BorDto.Product.UtilityTaxonomyParentId,
                            borParameter.BorDto.BorStatusId, Title = ItemId + " - " + borParameter.BorDto.Name,
                            borParameter.BorDto.BorTypeId, ProjectSequenceCode = borParameter.ProjectSequenceId,
                            borParameter.BorDto.StartDate,
                            borParameter.BorDto.EndDate
                        }).Result;
                        connection.Close();
                    }
                    else
                    {
                        string pId = null;
                        var affectedRows = connection.ExecuteAsync(sb.ToString(), new
                        {
                            borParameter.BorDto.Id, ItemId, borParameter.BorDto.Name, PbsProductId = pId,
                            IsDeleted = 0, LocationParentId = pId, UtilityParentId = pId,
                            borParameter.BorDto.BorStatusId, Title = ItemId + " - " + borParameter.BorDto.Name,
                            borParameter.BorDto.BorTypeId, ProjectSequenceCode = borParameter.ProjectSequenceId,
                            borParameter.BorDto.StartDate,
                            borParameter.BorDto.EndDate
                        }).Result;
                        connection.Close();
                    }
                }
            }

            else
            {
                isUpdate = true;
                var sb1 = new StringBuilder("select * from Bor where Id = @Id ");
                await using (var connection = new SqlConnection(connectionString))
                {
                    var bor = connection
                        .QuerySingleOrDefaultAsync<Bor>(sb1.ToString(), new { borParameter.BorDto.Id }).Result;
                    ItemId = bor.ItemId;
                    if (borParameter.BorDto.BorTypeId != "6610e768-3e06-po02-b337-ee367a82ad66")
                    {
                        var sb2 = new StringBuilder(
                            "update Bor set  ItemId=@ItemId, Name=@Name, PbsProductId=@PbsProductId, IsDeleted=@IsDeleted, LocationParentId=@LocationParentId,UtilityParentId=@UtilityParentId  , BorStatusId=@BorStatusId, Title =@Title, StartDate = @StartDate, EndDate = @EndDate");
                        sb2.Append(" where Id = @Id");
                        var affectedRows = connection.ExecuteAsync(sb2.ToString(), new
                        {
                            borParameter.BorDto.Id, bor.ItemId, borParameter.BorDto.Name,
                            PbsProductId = borParameter.BorDto.Product.Id,
                            bor.IsDeleted, LocationParentId = borParameter.BorDto.Product.LocationTaxonomyParentId,
                            UtilityParentId = borParameter.BorDto.Product.UtilityTaxonomyParentId,
                            borParameter.BorDto.BorStatusId, Title = ItemId + " - " + borParameter.BorDto.Name,
                            borParameter.BorDto.StartDate,
                            borParameter.BorDto.EndDate
                        }).Result;
                        connection.Close();
                    }
                    else
                    {
                        var sb2 = new StringBuilder(
                            "update Bor set  ItemId=@ItemId, Name=@Name, IsDeleted=@IsDeleted , BorStatusId=@BorStatusId, Title =@Title, StartDate = @StartDate, EndDate = @EndDate");
                        sb2.Append(" where Id = @Id");
                        var affectedRows = connection.ExecuteAsync(sb2.ToString(), new
                        {
                            borParameter.BorDto.Id, bor.ItemId, borParameter.BorDto.Name, bor.IsDeleted,
                            borParameter.BorDto.BorStatusId, Title = ItemId + " - " + borParameter.BorDto.Name,
                            borParameter.BorDto.StartDate,
                            borParameter.BorDto.EndDate
                        }).Result;
                        connection.Close();
                    }
                }
            }

            await borParameter.IBorResourceRepository.CreateBorConsumable(borParameter, isUpdate).ConfigureAwait(false);
            await borParameter.IBorResourceRepository.CreateBorLabour(borParameter, isUpdate).ConfigureAwait(false);
            await borParameter.IBorResourceRepository.CreateBorMaterial(borParameter, isUpdate).ConfigureAwait(false);
            await borParameter.IBorResourceRepository.CreateBorTools(borParameter, isUpdate).ConfigureAwait(false);

            if (borParameter.BorDto.IsTh != true) await CreateHistory(borParameter);
            return ItemId;
        }
    }

    public async Task<IEnumerable<BorShortcutPaneDto>> GetShortcutPaneData(BorParameter borParameter)
    {
        var sb = new StringBuilder();
        sb.Append(
            "select type.Id as Id, data.Label as Name, 'Resource' as Type from[dbo].[CpcResourceType] type,[dbo].[CpcResourceTypeLocalizedData] data  ");
        sb.Append("where type.Id = data.CpcResourceTypeId and data.LanguageCode ='" + borParameter.Lang + "' ");
        sb.Append("union select '0' as Id, 'Bor' as Name, 'Bor' as Type ");
        sb.Append("union select '1' as Id, 'Bor - Resources' as Name, 'AllResource' as Type");

        // using IDbConnection dbConnection =
        //     new SqlConnection(borParameter.TenantProvider.GetTenant().ConnectionString);
        var result = borParameter.TenantProvider.orgSqlConnection().Query<BorShortcutPaneDto>(sb.ToString());
       // 
        return result;
    }

    public async Task<IEnumerable<PbsProductDto>> GetProduct(BorParameter borParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        StringBuilder sb;
        if (borParameter.Title != null)
        {
            
            borParameter.Title = borParameter.Title.Replace("'", "''");
            sb = new StringBuilder("select ProductId as [Key] , Title AS Text from PbsProduct where Title like '%" +
                                   borParameter.Title + "%' AND PbsProduct.PbsProductItemTypeId NOT IN('48a7dd9c-55ac-4e7c-a2f3-653811c0eb14') and IsDeleted = 0 order by ProductId");
        }
        else
        {
            
            sb = new StringBuilder(
                "select ProductId as [Key] , Title AS Text from PbsProduct where PbsProduct.PbsProductItemTypeId NOT IN('48a7dd9c-55ac-4e7c-a2f3-653811c0eb14') and IsDeleted = 0 order by ProductId");
        }

        await using (var connection = new SqlConnection(connectionString))
        {
            IEnumerable<PbsProductDto> result = connection.Query<PbsProductDto>(sb.ToString()).ToList();
            return result;
        }
    }


    public async Task<IEnumerable<BorListDto>> GetBorList(BorParameter borParameter)
    {
        var query =
            @"SELECT Bor.Id AS Id, Bor.Id AS [Key], Bor.ItemId, Bor.Title AS BorTitle, Bor.Name,Bor.BorTypeId,dbo.PbsProduct.Id AS PbsId, PbsProduct.ProductId
                                ,PbsProduct.Name Product, PbsProduct_1.Name AS LocationParent, PbsProduct_2.Name AS UtilityParent
                                FROM dbo.Bor
                                LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_1 ON Bor.LocationParentId = PbsProduct_1.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_2 ON Bor.UtilityParentId = PbsProduct_2.Id 
                                WHERE Bor.IsDeleted = 0 ";
        var sb = new StringBuilder(query);
        if (borParameter.BorFilter.BorTitle != null)
        {
            borParameter.BorFilter.BorTitle = borParameter.BorFilter.BorTitle.Replace("'", "''");
            sb.Append("AND Bor.Title LIKE '%" + borParameter.BorFilter.BorTitle + "%' ");
        }

        if (borParameter.BorFilter.LocationParent != null)
        {
            borParameter.BorFilter.LocationParent = borParameter.BorFilter.LocationParent.Replace("'", "''");
            sb.Append("AND PbsProduct_1.Name LIKE '%" + borParameter.BorFilter.LocationParent + "%' ");
        }
            
        if (borParameter.BorFilter.UtilityParent != null)
            sb.Append("AND PbsProduct_2.Name LIKE  '%" + borParameter.BorFilter.UtilityParent + "%' ");
        if (borParameter.BorFilter.Product != null)
            sb.Append("AND PbsProduct.Name LIKE  '%" + borParameter.BorFilter.Product + "%' ");
        if (borParameter.BorFilter.Sorter.Attribute == null)
            sb.Append("ORDER BY CAST(SUBSTRING(Bor.ItemId,5,20) AS INT) DESC ");
        if (borParameter.BorFilter.Sorter.Attribute != null)
        {
            if (borParameter.BorFilter.Sorter.Attribute.ToLower().Equals("bortitle"))
                sb.Append("ORDER BY CAST(SUBSTRING(Bor.ItemId,5,20) AS INT) " + borParameter.BorFilter.Sorter.Order);
            if (borParameter.BorFilter.Sorter.Attribute.ToLower().Equals("locationparent"))
                sb.Append("ORDER BY PbsProduct_1.Name " + borParameter.BorFilter.Sorter.Order);
            if (borParameter.BorFilter.Sorter.Attribute.ToLower().Equals("utilityparent"))
                sb.Append("ORDER BY PbsProduct_2.Name " + borParameter.BorFilter.Sorter.Order);
            if (borParameter.BorFilter.Sorter.Attribute.ToLower().Equals("product"))
                sb.Append("ORDER BY PbsProduct.Name " + borParameter.BorFilter.Sorter.Order);
        }

        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var connection = new SqlConnection(connectionString))
        {
            var result = connection.Query<BorListDto>(sb.ToString());
            return result;
        }
    }

    public async Task<IEnumerable<BorListDto>> FilterBorPo(BorParameter borParameter)
    {
        var query =
            @"SELECT Bor.Id AS Id, Bor.Id AS [Key], Bor.ItemId, Bor.Title AS BorTitle, Bor.Name,dbo.PbsProduct.Id AS PbsId, PbsProduct.ProductId
                                ,PbsProduct.Name Product, PbsProduct_1.Name AS LocationParent, PbsProduct_2.Name AS UtilityParent
                                FROM dbo.Bor
                                LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_1 ON Bor.LocationParentId = PbsProduct_1.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_2 ON Bor.UtilityParentId = PbsProduct_2.Id 
                                WHERE Bor.IsDeleted = 0 AND (BorTypeId != '6610e768-3e06-po02-b337-ee367a82ad66' OR BorTypeId IS NULL) AND Bor.Id NOT in (SELECT POResources.BorId FROM dbo.POResources WHERE BorId IS NOT NULL GROUP BY POResources.BorId) ";
        var sb = new StringBuilder(query);
        if (borParameter.BorFilter.BorTitle != null)
        {
            borParameter.BorFilter.BorTitle = borParameter.BorFilter.BorTitle.Replace("'", "''");
            sb.Append("AND Bor.Title LIKE '%" + borParameter.BorFilter.BorTitle + "%' ");
        }

        if (borParameter.BorFilter.LocationParent != null)
        {
            borParameter.BorFilter.LocationParent = borParameter.BorFilter.LocationParent.Replace("'", "''");
            sb.Append("AND PbsProduct_1.Name LIKE '%" + borParameter.BorFilter.LocationParent + "%' ");
        }
           
        if (borParameter.BorFilter.UtilityParent != null)
            sb.Append("AND PbsProduct_2.Name LIKE  '%" + borParameter.BorFilter.UtilityParent + "%' ");
        if (borParameter.BorFilter.Product != null)
            sb.Append("AND PbsProduct.Name LIKE  '%" + borParameter.BorFilter.Product + "%' ");
        if (borParameter.BorFilter.Sorter.Attribute == null) sb.Append("ORDER BY ItemId DESC ");
        if (borParameter.BorFilter.Sorter.Attribute != null)
            switch (borParameter.BorFilter.Sorter.Attribute.ToLower())
            {
                case "bortitle":
                    sb.Append("ORDER BY Bor.ItemId " + borParameter.BorFilter.Sorter.Order);
                    break;
                case "locationparent":
                    sb.Append("ORDER BY PbsProduct_1.Name " + borParameter.BorFilter.Sorter.Order);
                    break;
                case "utilityparent":
                    sb.Append("ORDER BY PbsProduct_2.Name " + borParameter.BorFilter.Sorter.Order);
                    break;
                case "product":
                    sb.Append("ORDER BY PbsProduct.Name " + borParameter.BorFilter.Sorter.Order);
                    break;
            }

        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var connection = new SqlConnection(connectionString))
        {
            var result = connection.Query<BorListDto>(sb.ToString());
            return result;
        }
    }


    public async Task<IEnumerable<BorListDto>> GetBorListByProduct(BorParameter borParameter)
    {
        var query =
            @"SELECT Bor.Id AS Id, Bor.Id AS [Key], Bor.ItemId, Bor.Title AS BorTitle, Bor.Name,dbo.PbsProduct.Id AS PbsId, PbsProduct.ProductId
                                ,PbsProduct.Name Product
                                FROM dbo.Bor
                                LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id
                                WHERE Bor.IsDeleted = 0 
                                AND PbsProduct.ProductId = '" + borParameter.BorFilter.Product + "'";
        var sb = new StringBuilder(query);

        if (borParameter.BorFilter.BorTitle != null)
        {
            borParameter.BorFilter.BorTitle = borParameter.BorFilter.BorTitle.Replace("'", "''");
            sb.Append("AND Bor.Title LIKE '%" + borParameter.BorFilter.BorTitle + "%' ");

        }
            
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);
        return connection.Query<BorListDto>(sb.ToString());
        ;
    }


    public async Task<BorGetByIdDto> GetBorById(BorParameter borParameter)
    {
        var lang = borParameter.Lang;

        IEnumerable<BorGetByIdDto> result;
        IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyLogDto = null;
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            {
                var query =
                    @"SELECT Bor.Id AS Id, Bor.ItemId, Bor.Title AS BorTitle, Bor.BorStatusId, Bor.Name, CONCAT(ItemId , ' - ' , Bor.Name) AS HeaderTitle, Bor.BorTypeId, Bor.StartDate, Bor.EndDate,
                                        PbsProduct.ProductId, PbsProduct.Name, PbsProduct_1.Id AS LocationTaxonomyParentId, PbsProduct_2.Id AS UtilityTaxonomyParentId, PbsProduct.Id
                                        , PbsProduct.Title AS Title, PbsProduct.PbsProductItemTypeId, PbsProductItemTypeLocalizedData.Label as PbsProductItemType
                                        FROM dbo.Bor
                                        LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id
                                        LEFT OUTER JOIN dbo.PbsProduct PbsProduct_1 ON Bor.LocationParentId = PbsProduct_1.Id
                                        LEFT OUTER JOIN dbo.PbsProduct PbsProduct_2 ON Bor.UtilityParentId = PbsProduct_2.Id 
                                        LEFT OUTER JOIN PbsProductItemTypeLocalizedData ON PbsProduct.PbsProductItemTypeId = PbsProductItemTypeLocalizedData.PbsProductItemTypeId
                                        WHERE (PbsProductItemTypeLocalizedData.LanguageCode = @lang OR PbsProduct.PbsProductItemTypeId is null) AND ItemId = @ItemId";
                var paramNew = new { ItemId = borParameter.Id, lang };

                result = dbConnection.Query<BorGetByIdDto, BorGetByIdProductDto,
                    BorGetByIdDto>(query, (bor, product) =>
                {
                    bor.Product = product;
                    return bor;
                }, paramNew, splitOn: "ProductId");
                

                var historyParameter = new { id = result.FirstOrDefault().Id };

                var historyQuery =
                    @"SELECT   BorHistoryLog.ChangedTime AS DateTime  ,BorHistoryLog.ChangedByUserId AS Oid, BorHistoryLog.RevisionNumber AS RevisionNumber 
                                                FROM BorHistoryLog WHERE BorHistoryLog.BorId =@id ORDER BY RevisionNumber";

                await using (var connection = new SqlConnection(connectionString))
                {
                    historyLogDto =
                        connection.Query<ProjectDefinitionHistoryLogDapperDto>(historyQuery, historyParameter);
                    
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
                    using var connection =
                        new SqlConnection(borParameter.TenantProvider.GetTenant().ConnectionString);
                    userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                    log.CreatedByUser = userName.FirstOrDefault();
                    
                }

                if (historyLogDto.Count() >= 2)
                {
                    var historydto = historyLogDto.Last();
                    log.UpdatedDateTime = historydto.DateTime;
                    log.RevisionNumber = historydto.RevisionNumber;

                    var historyUserParameter = new { userId = historydto.Oid };
                    using (var connection =
                           new SqlConnection(borParameter.TenantProvider.GetTenant().ConnectionString))
                    {
                        userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                        log.UpdatedByUser = userName.FirstOrDefault();
                        
                    }
                }

                result.FirstOrDefault().historyLog = log;
            }
        }


        var bor = result.FirstOrDefault();
        bor.BorResources = await GetBorResourcesbyId(borParameter, bor.Id);
        var selectConsumable =
            @"SELECT COUNT(PbsConsumable.Id) FROM PbsConsumable WHERE PbsProductId = @PbsProductId";
        var selectLabour = @"SELECT COUNT(PbsLabour.Id) FROM PbsLabour WHERE PbsProductId = @PbsProductId";
        var selectMaterials =
            @"SELECT count (PbsMaterial.Id) FROM dbo.PbsMaterial WHERE PbsProductId = @PbsProductId";
        var selectTools = @"SELECT count (PbsTools.Id) FROM dbo.PbsTools WHERE PbsProductId = @PbsProductId";


        if (bor.BorTypeId != "6610e768-3e06-po02-b337-ee367a82ad66")
        {
            var parm = new { PbsProductId = result.Select(i => i.Product.Id) };
            await using (var connection = new SqlConnection(connectionString))
            {
                bor.MaterialCount = connection.QueryFirstOrDefault<int>(selectMaterials, parm);
                bor.ConsumableCount = connection.QueryFirstOrDefault<int>(selectConsumable, parm);
                bor.ToolsCount = connection.QueryFirstOrDefault<int>(selectTools, parm);
                bor.LabourCount = connection.QueryFirstOrDefault<int>(selectLabour, parm);
            }
        }

        return bor;
    }

    public async Task<BorResourceListGetByIdsDto> GetBorResourcesbyIds(BorParameter borParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        string borType;
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            borType = dbConnection
                .Query<string>("SELECT BorTypeId FROM Bor where Id in @Ids", new { Ids = borParameter.idList })
                .FirstOrDefault();
        }


        var sql =
            "SELECT BorMaterial.Invoiced ,BorMaterial.Date ,BorMaterial.Required AS Required ,BorMaterial.Purchased ,BorMaterial.DeliveryRequested ,BorMaterial.Consumed ,BorMaterial.Warf ,BorMaterial.BorProductId ,BorMaterial.Id ,BorMaterial.CorporateProductCatalogId ,BorMaterial.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber ,PbsProduct.Title AS PbsTitle ,CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorMaterial LEFT OUTER JOIN dbo.Bor ON BorMaterial.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorMaterial.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorMaterial.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorMaterial.Date;";
        sql +=
            "SELECT BorConsumable.Invoiced ,BorConsumable.Date ,BorConsumable.Required AS Required ,BorConsumable.Purchased ,BorConsumable.DeliveryRequested ,BorConsumable.Consumed ,BorConsumable.Warf ,BorConsumable.BorProductId ,BorConsumable.Id ,BorConsumable.CorporateProductCatalogId ,BorConsumable.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber ,PbsProduct.Title AS PbsTitle ,CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorConsumable LEFT OUTER JOIN dbo.Bor ON BorConsumable.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorConsumable.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorConsumable.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorConsumable.Date;";
        sql +=
            "SELECT BorLabour.Invoiced ,BorLabour.Date ,BorLabour.Required AS Required ,BorLabour.Purchased ,BorLabour.DeliveryRequested ,BorLabour.Consumed ,BorLabour.Warf ,BorLabour.BorProductId ,BorLabour.Id ,BorLabour.CorporateProductCatalogId ,BorLabour.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber ,PbsProduct.Title AS PbsTitle ,CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorLabour LEFT OUTER JOIN dbo.Bor ON BorLabour.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorLabour.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorLabour.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorLabour.Date;";
        sql +=
            "SELECT BorTools.Invoiced ,BorTools.Date ,BorTools.Required AS Required ,BorTools.Purchased ,BorTools.DeliveryRequested ,BorTools.Consumed ,BorTools.Warf ,BorTools.BorProductId ,BorTools.Id ,BorTools.CorporateProductCatalogId ,BorTools.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber ,PbsProduct.Title AS PbsTitle ,CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorTools LEFT OUTER JOIN dbo.Bor ON BorTools.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorTools.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorTools.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorTools.Date";

        var sql2 =
            "SELECT BorMaterial.Invoiced ,BorMaterial.Date ,BorMaterial.Required AS Required ,BorMaterial.Purchased ,BorMaterial.DeliveryRequested ,BorMaterial.Consumed ,BorMaterial.Warf ,BorMaterial.BorProductId ,BorMaterial.Id ,BorMaterial.CorporateProductCatalogId ,BorMaterial.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber ,CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorMaterial LEFT OUTER JOIN dbo.Bor ON BorMaterial.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorMaterial.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorMaterial.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorMaterial.Date;";
        sql2 +=
            "SELECT BorConsumable.Invoiced ,BorConsumable.Date ,BorConsumable.Required AS Required ,BorConsumable.Purchased ,BorConsumable.DeliveryRequested ,BorConsumable.Consumed ,BorConsumable.Warf ,BorConsumable.BorProductId ,BorConsumable.Id ,BorConsumable.CorporateProductCatalogId ,BorConsumable.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber, CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorConsumable LEFT OUTER JOIN dbo.Bor ON BorConsumable.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorConsumable.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorConsumable.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorConsumable.Date;";
        sql2 +=
            "SELECT BorLabour.Invoiced ,BorLabour.Date ,BorLabour.Required AS Required ,BorLabour.Purchased ,BorLabour.DeliveryRequested ,BorLabour.Consumed ,BorLabour.Warf ,BorLabour.BorProductId ,BorLabour.Id ,BorLabour.CorporateProductCatalogId ,BorLabour.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber ,CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorLabour LEFT OUTER JOIN dbo.Bor ON BorLabour.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorLabour.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorLabour.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorLabour.Date;";
        sql2 +=
            "SELECT BorTools.Invoiced ,BorTools.Date ,BorTools.Required AS Required ,BorTools.Purchased ,BorTools.DeliveryRequested ,BorTools.Consumed ,BorTools.Warf ,BorTools.BorProductId ,BorTools.Id ,BorTools.CorporateProductCatalogId ,BorTools.Returned,Bor.Title AS BorTitle ,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU ,CorporateProductCatalog.ResourceNumber AS ResourceNumber ,CorporateProductCatalog.InventoryPrice ,CpcResourceFamilyLocalizedData.Label AS ResourceFamily FROM dbo.BorTools LEFT OUTER JOIN dbo.Bor ON BorTools.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorTools.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId WHERE BorTools.BorProductId IN @Ids AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceFamilyId IS NULL) ORDER BY BorTools.Date";


        var paramss = new { Ids = borParameter.idList, lang = borParameter.Lang };

        var resources = new BorResourceListGetByIdsDto();
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            dbConnection.Open();

            if (borType != "6610e768-3e06-po02-b337-ee367a82ad66")
            {
                using var multi = await dbConnection.QueryMultipleAsync(sql, paramss);
                resources.Materials = multi.Read<BorResourceGetByIdsDto>();
                resources.Consumable = multi.Read<BorResourceGetByIdsDto>();
                resources.Labour = multi.Read<BorResourceGetByIdsDto>();

                resources.Tools = multi.Read<BorResourceGetByIdsDto>();
            }
            else
            {
                using var multi = await dbConnection.QueryMultipleAsync(sql2, paramss);
                resources.Materials = multi.Read<BorResourceGetByIdsDto>();
                resources.Consumable = multi.Read<BorResourceGetByIdsDto>();
                resources.Labour = multi.Read<BorResourceGetByIdsDto>();

                resources.Tools = multi.Read<BorResourceGetByIdsDto>();
            }
        }

        return resources;
    }

    public async Task<IEnumerable<BorResourceListDto>> GetBorResourceList(BorParameter borParameter)
    {
        var lang = borParameter.Lang;

        var query = @"SELECT
                                  *
                                FROM (SELECT
                                    SUM(BorConsumable.TotalRequired) AS Required
                                   ,SUM(BorConsumable.Consumed) AS Consumed
                                   ,SUM(BorConsumable.Purchased) AS Purchased
                                   ,SUM(BorConsumable.DeliveryRequested) AS DeliveryRequested
                                   ,SUM(BorConsumable.Warf) AS Warf
                                   ,SUM(BorConsumable.Invoiced) AS Invoiced
                                   ,SUM(BorConsumable.Returned) AS Returned
                                   ,BorConsumable.CorporateProductCatalogId
                                   ,CorporateProductCatalog.Title AS ResourceTitle
                                   ,CpcResourceTypeLocalizedData.Label AS ResourceType
                                   ,CorporateProductCatalog.ResourceTypeId
                                  FROM dbo.BorConsumable
                                  LEFT OUTER JOIN dbo.CorporateProductCatalog
                                    ON BorConsumable.CorporateProductCatalogId = CorporateProductCatalog.Id
                                  LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                    ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                  WHERE CpcResourceTypeLocalizedData.LanguageCode = @lang
                                  GROUP BY BorConsumable.CorporateProductCatalogId
                                          ,CorporateProductCatalog.Title
                                          ,CpcResourceTypeLocalizedData.Label
                                          ,CorporateProductCatalog.ResourceTypeId
                                  UNION
                                  SELECT
                                    SUM(BorMaterial.TotalRequired) AS Required
                                   ,SUM(BorMaterial.Consumed) AS Consumed
                                   ,SUM(BorMaterial.Purchased) AS Purchased
                                   ,SUM(BorMaterial.DeliveryRequested) AS DeliveryRequested
                                   ,SUM(BorMaterial.Warf) AS Warf
                                   ,SUM(BorMaterial.Invoiced) AS Invoiced
                                   ,SUM(BorMaterial.Returned) AS Returned
                                   ,BorMaterial.CorporateProductCatalogId
                                   ,CorporateProductCatalog.Title AS ResourceTitle
                                   ,CpcResourceTypeLocalizedData.Label AS ResourceType
                                   ,CorporateProductCatalog.ResourceTypeId
                                  FROM dbo.BorMaterial
                                  LEFT OUTER JOIN dbo.CorporateProductCatalog
                                    ON BorMaterial.CorporateProductCatalogId = CorporateProductCatalog.Id
                                  LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                    ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                  WHERE CpcResourceTypeLocalizedData.LanguageCode = @lang
                                  GROUP BY BorMaterial.CorporateProductCatalogId
                                          ,CorporateProductCatalog.Title
                                          ,CpcResourceTypeLocalizedData.Label
                                          ,CorporateProductCatalog.ResourceTypeId
                                  UNION
                                  SELECT
                                    SUM(BorLabour.TotalRequired) AS Required
                                   ,SUM(BorLabour.Consumed) AS Consumed
                                   ,SUM(BorLabour.Purchased) AS Purchased
                                   ,SUM(BorLabour.DeliveryRequested) AS DeliveryRequested
                                   ,SUM(BorLabour.Warf) AS Warf
                                   ,SUM(BorLabour.Invoiced) AS Invoiced
                                   ,SUM(BorLabour.Returned) AS Returned
                                   ,BorLabour.CorporateProductCatalogId
                                   ,CorporateProductCatalog.Title AS ResourceTitle
                                   ,CpcResourceTypeLocalizedData.Label AS ResourceType
                                   ,CorporateProductCatalog.ResourceTypeId
                                  FROM dbo.BorLabour
                                  LEFT OUTER JOIN dbo.CorporateProductCatalog
                                    ON BorLabour.CorporateProductCatalogId = CorporateProductCatalog.Id
                                  LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                    ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                  WHERE CpcResourceTypeLocalizedData.LanguageCode = @lang
                                  GROUP BY BorLabour.CorporateProductCatalogId
                                          ,CorporateProductCatalog.Title
                                          ,CpcResourceTypeLocalizedData.Label
                                          ,CorporateProductCatalog.ResourceTypeId

                                  UNION
                                  SELECT
                                    SUM(BorTools.TotalRequired) AS Required
                                   ,SUM(BorTools.Consumed) AS Consumed
                                   ,SUM(BorTools.Purchased) AS Purchased
                                   ,SUM(BorTools.DeliveryRequested) AS DeliveryRequested
                                   ,SUM(BorTools.Warf) AS Warf
                                   ,SUM(BorTools.Invoiced) AS Invoiced
                                   ,SUM(BorTools.Returned) AS Returned
                                   ,BorTools.CorporateProductCatalogId
                                   ,CorporateProductCatalog.Title AS ResourceTitle
                                   ,CpcResourceTypeLocalizedData.Label AS ResourceType
                                   ,CorporateProductCatalog.ResourceTypeId
                                  FROM dbo.BorTools
                                  LEFT OUTER JOIN dbo.CorporateProductCatalog
                                    ON BorTools.CorporateProductCatalogId = CorporateProductCatalog.Id
                                  LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                    ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                  WHERE CpcResourceTypeLocalizedData.LanguageCode = @lang
                                  GROUP BY BorTools.CorporateProductCatalogId
                                          ,CorporateProductCatalog.Title
                                          ,CpcResourceTypeLocalizedData.Label
                                          ,CorporateProductCatalog.ResourceTypeId) AS BorCpcResources Where 1=1 ";
        var sb = new StringBuilder(query);
        var filter = borParameter.BorResourceFilter;
        if (filter.DeliveryRequested != null)
            sb.Append(" AND DeliveryRequested ='" + filter.DeliveryRequested + "' ");
        if (filter.Invoiced != null) sb.Append(" AND Invoiced ='" + filter.Invoiced + "' ");
        if (filter.Purchased != null) sb.Append(" AND Purchased ='" + filter.Purchased + "' ");
        if (filter.Required != null) sb.Append(" AND Required ='" + filter.Required + "' ");
        if (filter.ResourceTitle != null)
        {
            filter.ResourceTitle = filter.ResourceTitle.Replace("'", "''");
            sb.Append(" AND ResourceTitle like '%" + filter.ResourceTitle + "%' ");
        }
            
        if (filter.ResourceTypeId != null)
            sb.Append("  AND BorCpcResources.ResourceTypeId ='" + filter.ResourceTypeId + "' ");
        if (filter.Warf != null) sb.Append("  AND Warf ='" + filter.Warf + "' ");
        if (filter.Sorter.Attribute != null)
            sb.Append("  ORDER By " + filter.Sorter.Attribute + " " + filter.Sorter.Order);

        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        var parameter = new { lang };
        await using (var connection = new SqlConnection(connectionString))
        {
            var result = connection.Query<BorResourceListDto>(sb.ToString(), parameter);
            return result;
        }
    }

    public async Task<BorDropdownData> GetBorDropdownData(BorParameter borParameter)
    {
        var dropdownData = new BorDropdownData();
        var lang = borParameter.Lang;

        var query = @"select status.PbsProductStatusId as Id, status.Label as Name
                                from PbsProductStatusLocalizedData status where LanguageCode= @lang
                                order by DisplayOrder
                             ";
        var borType = @"select BorType.BorTypeId as [Key], BorType.Name as [Text]
                                from BorType";

        var parameters = new { lang };
        await using (var dbConnection = new SqlConnection(borParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await dbConnection.OpenAsync();
            dropdownData.BorStatus = await dbConnection.QueryAsync<PbsProductStatusDto>(query, parameters);
            dropdownData.BorType = await dbConnection.QueryAsync<BorTypeDto>(borType);
            
        }

        return dropdownData;
    }

    public async Task<string> UpdateBorStatus(BorParameter borParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var connection = new SqlConnection(connectionString))
        {
            var query = @"update Bor set BorStatusId = @statusId where Id = @id";
            var affectedRows = connection.ExecuteAsync(query,
                new
                {
                    id = borParameter.borStatusUpdateDto.BorId, statusId = borParameter.borStatusUpdateDto.StatusId
                }).Result;
            connection.Close();
        }

        return borParameter.borStatusUpdateDto.BorId;
    }

    public async Task<IEnumerable<BorListDto>> FilterReturnBorPo(BorParameter borParameter)
    {
        var query =
            @"SELECT Bor.Id AS Id, Bor.Id AS [Key], Bor.ItemId, Bor.Title AS BorTitle, Bor.Name, Bor.BorTypeId, dbo.PbsProduct.Id AS PbsId, PbsProduct.ProductId
                                ,PbsProduct.Name Product, PbsProduct_1.Name AS LocationParent, PbsProduct_2.Name AS UtilityParent
                                FROM dbo.Bor
                                LEFT OUTER JOIN dbo.PbsProduct ON Bor.PbsProductId = PbsProduct.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_1 ON Bor.LocationParentId = PbsProduct_1.Id
                                LEFT OUTER JOIN dbo.PbsProduct PbsProduct_2 ON Bor.UtilityParentId = PbsProduct_2.Id 
                                WHERE Bor.IsDeleted = 0 AND Bor.BorTypeId = '6610e768-3e06-po02-b337-ee367a82ad66' AND  Bor.Id NOT in (SELECT POResources.BorId FROM dbo.POResources WHERE BorId IS NOT NULL GROUP BY POResources.BorId) ";
        var sb = new StringBuilder(query);
        if (borParameter.BorFilter.BorTitle != null)
        {
            borParameter.BorFilter.BorTitle = borParameter.BorFilter.BorTitle.Replace("'", "''");
            sb.Append("AND Bor.Title LIKE '%" + borParameter.BorFilter.BorTitle + "%' ");
        }

        if (borParameter.BorFilter.LocationParent != null)
        {
            borParameter.BorFilter.LocationParent = borParameter.BorFilter.LocationParent.Replace("'", "''");
            sb.Append("AND PbsProduct_1.Name LIKE '%" + borParameter.BorFilter.LocationParent + "%' ");
        }
            
        if (borParameter.BorFilter.UtilityParent != null)
            sb.Append("AND PbsProduct_2.Name LIKE  '%" + borParameter.BorFilter.UtilityParent + "%' ");
        if (borParameter.BorFilter.Product != null)
            sb.Append("AND PbsProduct.Name LIKE  '%" + borParameter.BorFilter.Product + "%' ");
        if (borParameter.BorFilter.Sorter.Attribute == null) sb.Append("ORDER BY ItemId DESC ");
        if (borParameter.BorFilter.Sorter.Attribute != null)
            switch (borParameter.BorFilter.Sorter.Attribute.ToLower())
            {
                case "bortitle":
                    sb.Append("ORDER BY Bor.ItemId " + borParameter.BorFilter.Sorter.Order);
                    break;
                case "locationparent":
                    sb.Append("ORDER BY PbsProduct_1.Name " + borParameter.BorFilter.Sorter.Order);
                    break;
                case "utilityparent":
                    sb.Append("ORDER BY PbsProduct_2.Name " + borParameter.BorFilter.Sorter.Order);
                    break;
                case "product":
                    sb.Append("ORDER BY PbsProduct.Name " + borParameter.BorFilter.Sorter.Order);
                    break;
            }

        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        using var connection = new SqlConnection(connectionString);
        var result = connection.Query<BorListDto>(sb.ToString());
        return result;
    }

    private async Task CreateHistory(BorParameter borParameter)
    {
        var projectCon = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        var options = new DbContextOptions<ShanukaDbContext>();
        var applicationDbContext = new ShanukaDbContext(options, projectCon, borParameter.TenantProvider);

        var oid = borParameter.ContextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

        var jsonProject = JsonConvert.SerializeObject(borParameter.BorDto, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

        var history = new BorHistoryLog();

        var isAdded = applicationDbContext.BorHistoryLog.Any(x => x.BorId == borParameter.BorDto.Id);
        history.Action = isAdded == false ? HistoryState.ADDED.ToString() : HistoryState.UPDATED.ToString();

        history.ChangedByUserId = oid;
        history.Id = Guid.NewGuid().ToString();
        history.HistoryLog = jsonProject;
        history.ChangedTime = DateTime.UtcNow;
        history.BorId = borParameter.BorDto.Id;

        applicationDbContext.BorHistoryLog.Add(history);
        applicationDbContext.SaveChanges();
    }

    public async Task<BorResourceListGetByIdDto> GetBorResourcesbyId(BorParameter borParameter, string id)
    {
        var sql =
            "SELECT BorMaterial.Invoiced ,BorMaterial.Date ,BorMaterial.Required AS Required ,BorMaterial.Purchased,BorMaterial.DeliveryRequested ,BorMaterial.Consumed ,BorMaterial.Warf,BorMaterial.BorProductId,BorMaterial.Id ,BorMaterial.CorporateProductCatalogId ,BorMaterial.ActualDeliveryDate ,BorMaterial.ExpectedDeliveryDate ,BorMaterial.RequestedDeliveryDate ,BorMaterial.Returned As Returned ,Bor.Title AS BorTitle,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU,CorporateProductCatalog.ResourceNumber  AS ResourceNumber FROM dbo.BorMaterial LEFT OUTER JOIN dbo.Bor ON BorMaterial.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorMaterial.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId WHERE BorMaterial.BorProductId = @Id AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) ORDER BY BorMaterial.Date;";
        sql +=
            "SELECT BorConsumable.Invoiced ,BorConsumable.Date ,BorConsumable.Required AS Required ,BorConsumable.Purchased,BorConsumable.DeliveryRequested ,BorConsumable.Consumed ,BorConsumable.Warf,BorConsumable.BorProductId,BorConsumable.Id ,BorConsumable.CorporateProductCatalogId ,BorConsumable.ActualDeliveryDate ,BorConsumable.ExpectedDeliveryDate ,BorConsumable.RequestedDeliveryDate ,BorConsumable.Returned As Returned ,Bor.Title AS BorTitle,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU,CorporateProductCatalog.ResourceNumber  AS ResourceNumber FROM dbo.BorConsumable LEFT OUTER JOIN dbo.Bor ON BorConsumable.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorConsumable.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId WHERE BorConsumable.BorProductId = @Id AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) ORDER BY BorConsumable.Date;";
        sql +=
            "SELECT BorLabour.Invoiced ,BorLabour.Date ,BorLabour.Required AS Required ,BorLabour.Purchased,BorLabour.DeliveryRequested ,BorLabour.Consumed ,BorLabour.Warf,BorLabour.BorProductId,BorLabour.Id ,BorLabour.CorporateProductCatalogId,BorLabour.ActualDeliveryDate ,BorLabour.ExpectedDeliveryDate ,BorLabour.RequestedDeliveryDate ,BorLabour.Returned As Returned,Bor.Title AS BorTitle,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU,CorporateProductCatalog.ResourceNumber  AS ResourceNumber FROM dbo.BorLabour LEFT OUTER JOIN dbo.Bor ON BorLabour.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorLabour.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId WHERE BorLabour.BorProductId = @Id AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) ORDER BY BorLabour.Date;";
        sql +=
            "SELECT BorTools.Invoiced ,BorTools.Date ,BorTools.Required AS Required ,BorTools.Purchased,BorTools.DeliveryRequested ,BorTools.Consumed ,BorTools.Warf,BorTools.BorProductId,BorTools.Id ,BorTools.CorporateProductCatalogId,BorTools.ActualDeliveryDate ,BorTools.ExpectedDeliveryDate ,BorTools.RequestedDeliveryDate ,BorTools.Returned As Returned ,Bor.Title AS BorTitle,Bor.Id AS BorId ,CorporateProductCatalog.Title AS Title ,CpcBasicUnitOfMeasureLocalizedData.Label AS MOU,CorporateProductCatalog.ResourceNumber  AS ResourceNumber FROM dbo.BorTools LEFT OUTER JOIN dbo.Bor ON BorTools.BorProductId = Bor.Id LEFT OUTER JOIN dbo.CorporateProductCatalog ON BorTools.CorporateProductCatalogId = CorporateProductCatalog.Id LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId WHERE BorTools.BorProductId = @id AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL) ORDER BY BorTools.Date";

        var paramss = new { Id = id, lang = borParameter.Lang };

        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        var resources = new BorResourceListGetByIdDto();
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
            dbConnection.Open();

            using (var multi = await dbConnection.QueryMultipleAsync(sql, paramss))
            {
                resources.Materials = multi.Read<BorResourceGetByIdDto>();
                resources.Consumable = multi.Read<BorResourceGetByIdDto>();
                resources.Labour = multi.Read<BorResourceGetByIdDto>();
                resources.Tools = multi.Read<BorResourceGetByIdDto>();
            }
        }

        await AddCpcTitle(resources, borParameter);
        return resources;
    }

    public async Task<BorResourceListGetByIdDto> AddCpcTitle(BorResourceListGetByIdDto resources,
        BorParameter borParameter)
    {
        var projectCon = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        var CuCon = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId, null,
            borParameter.TenantProvider);
        var orgCon = ConnectionString.MapConnectionString(null, null, borParameter.TenantProvider);

        foreach (var dto in resources.Consumable)
        {
            var options = new DbContextOptions<ShanukaDbContext>();

            await using (var context = new ShanukaDbContext(options, projectCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog.Where(c => c.Id == dto.CorporateProductCatalogId)
                    .FirstOrDefault();
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            await using (var context = new ShanukaDbContext(options, CuCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog.Where(c => c.Id == dto.CorporateProductCatalogId)
                    .FirstOrDefault();
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            await using (var context = new ShanukaDbContext(options, orgCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }
        }

        foreach (var dto in resources.Labour)
        {
            var options = new DbContextOptions<ShanukaDbContext>();

            using (var context = new ShanukaDbContext(options, projectCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            using (var context = new ShanukaDbContext(options, CuCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            using (var context = new ShanukaDbContext(options, orgCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }
        }

        foreach (var dto in resources.Materials)
        {
            var options = new DbContextOptions<ShanukaDbContext>();

            using (var context = new ShanukaDbContext(options, projectCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            using (var context = new ShanukaDbContext(options, CuCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            using (var context = new ShanukaDbContext(options, orgCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }
        }

        foreach (var dto in resources.Tools)
        {
            var options = new DbContextOptions<ShanukaDbContext>();

            await using (var context = new ShanukaDbContext(options, projectCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            await using (var context = new ShanukaDbContext(options, CuCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }

            await using (var context = new ShanukaDbContext(options, orgCon, borParameter.TenantProvider))
            {
                var cpc = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == dto.CorporateProductCatalogId);
                if (cpc != null)
                {
                    dto.Title = cpc.ResourceNumber + " - " + cpc.ResourceTitle;
                    dto.ResourceNumber = cpc.ResourceNumber;
                }
            }
        }

        return resources;
    }
}