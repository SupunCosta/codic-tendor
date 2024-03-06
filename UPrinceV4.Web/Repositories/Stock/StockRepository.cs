using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Data.WH;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.Stock;

public class StockRepository : IStockRepository
{
    public async Task<string> CreateHeader(StockParameter StockParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(StockParameter.ContractingUnitSequenceId,
            StockParameter.ProjectSequenceId, StockParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, StockParameter.TenantProvider);

        if (StockParameter.StockDto.Id != null)
        {
            const string query1 = @"SELECT [Id] ,[SequenceId] FROM [dbo].[StockHeader] Where Id = @Id";

            var parameter = new { StockParameter.StockDto.Id };
            StockHeader data;
            await using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<StockHeader>(query1, parameter).FirstOrDefault();
            }


            string stockSequenceId;
            if (data == null)
            {
                var idGenerator = new IdGenerator();
                stockSequenceId = idGenerator.GenerateId(applicationDbContext, "Stock-", "StockSequence");

                const string query =
                    @"INSERT INTO [dbo].[StockHeader]([Id],[SequenceId],[Name],[Title],[CPCId],[TypeId],[StatusId],[AvailableQuantity],[MOUId],[AveragePrice],[QuantityToBeDelivered],[WareHouseTaxonomyId],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[ActiveTypeId])VALUES(@Id,@SequenceId,@Name,@Title,@CPCId,@TypeId,@StatusId,@AvailableQuantity,@MOUId,@AveragePrice,@QuantityToBeDelivered,@WareHouseTaxonomyId,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@ActiveTypeId)";

                var parameters = new
                {
                    StockParameter.StockDto.Id,
                    SequenceId = stockSequenceId,
                    StockParameter.StockDto.Name,
                    Title = stockSequenceId + " " + StockParameter.StockDto.Name,
                    CPCId = StockParameter.StockDto.CpcId,
                    TypeId = StockParameter.StockDto.ResourceType,
                    ActiveTypeId = StockParameter.StockDto.Type,
                    StatusId = StockParameter.StockDto.Status,
                    StockParameter.StockDto.AvailableQuantity,
                    MOUId = StockParameter.StockDto.MouId,
                    StockParameter.StockDto.AveragePrice,
                    StockParameter.StockDto.QuantityToBeDelivered,
                    StockParameter.StockDto.WareHouseTaxonomyId,
                    CreatedBy = StockParameter.UserId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = StockParameter.UserId,
                    ModifiedDate = DateTime.UtcNow
                };

                await using var connection = new SqlConnection(connectionString);
                await connection.ExecuteAsync(query, parameters);
            }
            else
            {
                stockSequenceId = data.SequenceId;
                const string query =
                    @"UPDATE [dbo].[StockHeader] SET [Name] = @Name,[Title] = @Title,[CPCId] = @CPCId,[TypeId] = @TypeId,[StatusId] = @StatusId,[AvailableQuantity] = @AvailableQuantity,[MOUId] = @MOUId,[AveragePrice] = @AveragePrice,[QuantityToBeDelivered] = @QuantityToBeDelivered,[WareHouseTaxonomyId] = @WareHouseTaxonomyId, [ModifiedBy] = @ModifiedBy,[ModifiedDate] = @ModifiedDate,[ActiveTypeId] = @ActiveTypeId WHERE Id = @Id";
                var parameters = new
                {
                    StockParameter.StockDto.Id,
                    SequenceId = stockSequenceId,
                    StockParameter.StockDto.Name,
                    Title = stockSequenceId + " " + StockParameter.StockDto.Name,
                    CPCId = StockParameter.StockDto.CpcId,
                    TypeId = StockParameter.StockDto.ResourceType,
                    ActiveTypeId = StockParameter.StockDto.Type,
                    StatusId = StockParameter.StockDto.Status,
                    StockParameter.StockDto.AvailableQuantity,
                    MOUId = StockParameter.StockDto.MouId,
                    StockParameter.StockDto.AveragePrice,
                    StockParameter.StockDto.QuantityToBeDelivered,
                    StockParameter.StockDto.WareHouseTaxonomyId,
                    ModifiedBy = StockParameter.UserId,
                    ModifiedDate = DateTime.UtcNow
                };

                await using var connection = new SqlConnection(connectionString);
                await connection.ExecuteAsync(query, parameters);
            }

            return stockSequenceId;
        }

        throw new Exception("ID not set");
    }

    public async Task<StockDropDownData> GetStockDropdown(StockParameter StockParameter)
    {
        const string query =
            @"select StatusId as [Key], Name as Text  from StockStatus where LanguageCode = @lang ORDER BY DisplayOrder;
                             select TypeId as [Key], Name as Text  from StockType where LanguageCode = @lang order by Name;
                             SELECT StockActiveType.TypeId AS [Key],StockActiveType.Name AS Text FROM dbo.StockActiveType where LanguageCode = @lang ORDER BY DisplayOrder";
        var mStockDropDownData = new StockDropDownData();
        var parameters = new { lang = StockParameter.Lang };
        await using var connection = new SqlConnection(StockParameter.TenantProvider.GetTenant().ConnectionString);
        var muilti = await connection.QueryMultipleAsync(query, parameters);
        mStockDropDownData.Status = muilti.Read<StockStatusDto>();
        mStockDropDownData.ResourceTypes = muilti.Read<StockTypeDto>();
        mStockDropDownData.Types = muilti.Read<StockActiveTypeDto>();

        return mStockDropDownData;
    }

    public async Task<StockShortCutPaneCommon> GetShortcutpaneData(StockParameter StockParameter)
    {
        const string query = @"select
                              StockShortCutPane.TypeId AS Id
                             ,StockShortCutPane.DisplayOrder
                             ,StockShortCutPane.LanguageCode
                             ,StockShortCutPane.Name from StockShortCutPane
                            where LanguageCode = @lang
                            order by DisplayOrder;
                              SELECT
                              WHShortCutPane.TypeId AS Id
                             ,WHShortCutPane.DisplayOrder
                             ,WHShortCutPane.LanguageCode
                             ,WHShortCutPane.Name
                            FROM dbo.WHShortCutPane
                            WHERE WHShortCutPane.LanguageCode = @lang
                            ORDER BY WHShortCutPane.DisplayOrder;
                              SELECT
                              WFShortCutPane.TypeId AS Id
                             ,WFShortCutPane.DisplayOrder
                             ,WFShortCutPane.LanguageCode
                             ,WFShortCutPane.Name
                            FROM dbo.WFShortCutPane
                            WHERE WFShortCutPane.LanguageCode = @lang
                            ORDER BY WFShortCutPane.DisplayOrder";

        var stockShortCutPaneCommon = new StockShortCutPaneCommon();

        var parameters = new { lang = StockParameter.Lang };
        await using var connection = new SqlConnection(StockParameter.TenantProvider.GetTenant().ConnectionString);
        var data = connection.QueryMultiple(query, parameters);
        stockShortCutPaneCommon.Stock = data.Read<StockShortCutPane>();
        stockShortCutPaneCommon.Warehouse = data.Read<WHShortCutPane>();
        stockShortCutPaneCommon.WorkFlow = data.Read<WFShortCutPane>();
        return stockShortCutPaneCommon;
    }

    public async Task<IEnumerable<StockListDto>> GetStockList(StockParameter stockParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(stockParameter.ContractingUnitSequenceId,
            stockParameter.ProjectSequenceId, stockParameter.TenantProvider);

        const string query = @"SELECT 
                             StockHeader.Id
                            ,StockHeader.SequenceId
                            ,StockHeader.Name
                            ,StockHeader.Title
                            ,StockHeader.AvailableQuantity AS Count
                            ,StockHeader.MOUId
                            ,StockHeader.AveragePrice
                            ,StockHeader.QuantityToBeDelivered
                            ,StockHeader.WareHouseTaxonomyId
                            ,WHTaxonomy.Title AS WareHouse
                            ,StockType.Name AS ResourceType 
                            ,StockType.TypeId AS ResourceTypeId
							,StockActiveType.Name AS Type 
                            ,StockActiveType.TypeId AS TypeId
                            ,StockStatus.Name AS Status 
                            ,StockStatus.StatusId AS StatusId
                            ,CorporateProductCatalog.Id AS CpcId
                            ,CorporateProductCatalog.Title AS CpcResourceTitle
                            FROM dbo.StockHeader 
                            LEFT OUTER JOIN dbo.StockType ON StockHeader.TypeId = StockType.TypeId 
                            LEFT OUTER JOIN dbo.StockActiveType ON StockHeader.ActiveTypeId = StockActiveType.TypeId 
                            LEFT OUTER JOIN dbo.StockStatus ON StockHeader.StatusId = StockStatus.StatusId 
                            LEFT OUTER JOIN dbo.CorporateProductCatalog ON StockHeader.CPCId = CorporateProductCatalog.Id
                            LEFT OUTER JOIN dbo.WHTaxonomy ON StockHeader.WareHouseTaxonomyId = WHTaxonomy.Id
                            WHERE (StockType.LanguageCode = @lang OR StockHeader.TypeId IS NULL) 
                            AND (StockActiveType.LanguageCode = @lang OR StockHeader.ActiveTypeId IS NULL) 
                            AND (StockStatus.LanguageCode = @lang OR StockHeader.StatusId IS NULL)";
        var sb = new StringBuilder(query);

        if (stockParameter.Filter.Title != null)
        {
            stockParameter.Filter.Title = stockParameter.Filter.Title.Replace("'", "''");

            var words = stockParameter.Filter.Title.Split(" ");
            foreach (var element in words) sb.Append(" AND StockHeader.Title LIKE '%" + element + "%'");
        }

        if (stockParameter.Filter.WharehouseTaxonomy != null)
        {
            stockParameter.Filter.WharehouseTaxonomy = stockParameter.Filter.WharehouseTaxonomy.Replace("'", "''");
            sb.Append(" AND StockHeader.WareHouseTaxonomyId LIKE '%" + stockParameter.Filter.WharehouseTaxonomy + "%'");

        }
            
        if (stockParameter.Filter.Status != null)
            sb.Append(" AND StockStatus.StatusId = '" + stockParameter.Filter.Status + "' ");

        if (stockParameter.Filter.Name != null)
            sb.Append(" AND StockStatus.Name = '" + stockParameter.Filter.Name + "' ");

        if (stockParameter.Filter.Type != null)
            sb.Append(" AND StockActiveType.TypeId = '" + stockParameter.Filter.Type + "' ");

        if (stockParameter.Filter.ResourceType != null)
            sb.Append(" AND StockType.TypeId = '" + stockParameter.Filter.ResourceType + "' ");

        if (stockParameter.Filter.WareHouse != null)
        {
            stockParameter.Filter.WareHouse = stockParameter.Filter.WareHouse.Replace("'", "''");
            sb.Append(" AND WHTaxonomy.Title LIKE '%" + stockParameter.Filter.WareHouse + "%' ");

        }
            
        if (stockParameter.Filter.Sorter.Attribute == null)
            sb.Append(" ORDER BY CAST(SUBSTRING(StockHeader.SequenceId,7,20) AS INT) desc");

        if (stockParameter.Filter.Sorter.Attribute != null)
            switch (stockParameter.Filter.Sorter.Attribute.ToLower())
            {
                case "title":
                    sb.Append("ORDER BY CAST(SUBSTRING(StockHeader.SequenceId,7,20) AS INT) " +
                              stockParameter.Filter.Sorter.Order);
                    break;
                case "type":
                    sb.Append(" ORDER BY Type " + stockParameter.Filter.Sorter.Order);
                    break;
                case "status":
                    sb.Append(" ORDER BY StockStatus.Name " + stockParameter.Filter.Sorter.Order);
                    break;
                case "warehouse":
                    sb.Append(" ORDER BY WHTaxonomy.Title " + stockParameter.Filter.Sorter.Order);
                    break;
                case "count":
                    sb.Append(" ORDER BY  CAST(SUBSTRING(StockHeader.AvailableQuantity,0,20) AS FLOAT) " +
                              stockParameter.Filter.Sorter.Order);
                    break;
            }

        var parameters = new { lang = stockParameter.Lang };
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        var data = await connection.QueryAsync<StockListDto>(sb.ToString(), parameters);
        

        return data;
    }

    public async Task<StockHeaderDto> GetStockById(StockParameter stockParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(stockParameter.ContractingUnitSequenceId,
            stockParameter.ProjectSequenceId, stockParameter.TenantProvider);

        const string historyQuery =
            @"SELECT [CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM [dbo].[StockHeader] where [SequenceId] =@SequenceId ";

        var parameter = new { SequenceId = stockParameter.Id };
        StockHistoryDto historyLog;

        const string query = @"SELECT 
                               StockHeader.Id
                              ,StockHeader.SequenceId
                              ,StockHeader.Name
                              ,StockHeader.Title
                              ,StockHeader.AvailableQuantity
                              ,StockHeader.MOUId
                              ,CorporateProductCatalog.InventoryPrice AS AveragePrice
                              ,StockHeader.QuantityToBeDelivered
                              ,StockHeader.WareHouseTaxonomyId AS WarehouseTaxonomyId
                                ,StockType.TypeId AS [key] 
                                ,StockType.Name AS [Text] 
                                ,StockActiveType.TypeId AS [Key]
                                ,StockActiveType.Name AS [Text]
                                ,StockStatus.StatusId [Key]
                                ,StockStatus.Name AS [Text]
                                ,CorporateProductCatalog.Id AS [Value] 
                                ,CorporateProductCatalog.Title AS [Label]                              
                                FROM dbo.StockHeader 
                                LEFT OUTER JOIN dbo.StockType ON StockHeader.TypeId = StockType.TypeId 
								LEFT OUTER JOIN dbo.StockActiveType ON StockHeader.ActiveTypeId = StockActiveType.TypeId 
                                LEFT OUTER JOIN dbo.StockStatus ON StockHeader.StatusId = StockStatus.StatusId 
								LEFT OUTER JOIN dbo.CorporateProductCatalog ON StockHeader.CPCId = CorporateProductCatalog.Id
                                WHERE (StockType.LanguageCode = @lang OR StockHeader.TypeId IS NULL)
								AND (StockActiveType.LanguageCode = @lang OR StockHeader.ActiveTypeId IS NULL) 
                                AND (StockStatus.LanguageCode = @lang OR StockHeader.StatusId IS NULL) 
                                AND StockHeader.SequenceId= @Id";

        var parameters = new { lang = stockParameter.Lang, stockParameter.Id };

        StockHeaderDto mStockHeaderDto = null;
        await using (var connection = new SqlConnection(connectionString))
        {
            historyLog = connection.Query<StockHistoryDto>(historyQuery, parameter).FirstOrDefault();

            mStockHeaderDto = connection
                .Query<StockHeaderDto, StockTypeDto, StockActiveTypeDto, StockStatusDto, CpcDto,
                    StockHeaderDto>(
                    query,
                    (stockHeader, stockResocurceTypeDto, stockTypeDto, stockStatusDto, cpcDto) =>
                    {
                        stockHeader.Status = stockStatusDto;
                        stockHeader.ResourceType = stockResocurceTypeDto;
                        stockHeader.Type = stockTypeDto;
                        stockHeader.CpcType = cpcDto;
                        return stockHeader;
                    }, parameters,
                    splitOn: "Key, Key, Key, Value").FirstOrDefault();

            
        }

        const string ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        const string GetActivityHistory = @"SELECT
                                              StockHistoryLog.Id
                                             ,StockHistoryLog.WorkFlowId
                                             ,StockHistoryLog.DateTime AS Date
                                             ,StockHistoryLog.Quantity
                                             ,StockHistoryLog.Price
                                             ,StockHistoryLog.MOUId AS Mou
                                             ,StockActivityType.ActivityName AS Type
                                             ,WFHeader.Title
                                             ,StockHistoryLog.WareHouseWorker
                                            FROM dbo.StockHistoryLog
                                            LEFT OUTER JOIN dbo.StockActivityType
                                              ON StockHistoryLog.ActivityTypeId = StockActivityType.ActivityTypeId
                                            LEFT OUTER JOIN dbo.WFHeader
                                              ON StockHistoryLog.WorkFlowId = WFHeader.Id
                                            WHERE (StockActivityType.LanguageCode = @lang
                                            OR StockHistoryLog.ActivityTypeId IS NULL)
                                            AND StockHistoryLog.StockHeaderId = @Id Order by Date Desc";


        IEnumerable<StockActivityHistoryDto> stockHistoryLog = null;

        await using (var connection =
                     new SqlConnection(stockParameter.TenantProvider.GetTenant().ConnectionString))
        {
            if (historyLog != null)
            {
                var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
                historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                    .FirstOrDefault();

                var CreatByParam = new { oid = historyLog.CreatedBy };
                historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();
            }
        }

        await using (var connection = new SqlConnection(connectionString))
        {
            if (mStockHeaderDto != null)
            {
                var StockHistoryParam = new { lang = stockParameter.Lang, mStockHeaderDto.Id };
                await connection.OpenAsync();
                stockHistoryLog =
                    await connection.QueryAsync<StockActivityHistoryDto>(GetActivityHistory, StockHistoryParam);
            }
        }

        var GetWarehouseWorker =
            @"select CabPerson.FullName from CabPersonCompany left outer join CabPerson on CabPerson.Id = CabPersonCompany.PersonId where CabPersonCompany.Id = @Id";

        if (stockHistoryLog != null)
            foreach (var sHistory in stockHistoryLog)
                if (sHistory.WareHouseWorker != null)
                {
                    await using var connection =
                        new SqlConnection(stockParameter.TenantProvider.GetTenant().ConnectionString);
                    var Param1 = new { Id = sHistory.WareHouseWorker };
                    sHistory.WareHouseWorker = connection.Query<string>(GetWarehouseWorker, Param1)
                        .FirstOrDefault();
                }

        if (mStockHeaderDto != null)
        {
            mStockHeaderDto.History = historyLog;
            mStockHeaderDto.StockHistory = stockHistoryLog;
        }

        return mStockHeaderDto;
    }
}