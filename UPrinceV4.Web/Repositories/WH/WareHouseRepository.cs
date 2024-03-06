using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.WH;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.WH;

public class WareHouseRepository : IWareHouseRepository
{
    public async Task<string> CreateHeader(WHParameter WHParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(WHParameter.ContractingUnitSequenceId,
            WHParameter.ProjectSequenceId, WHParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, WHParameter.TenantProvider);
        string WHSequenceId;

        if (WHParameter.WHDto.Id != null)
        {
            var query1 = @"SELECT [Id] ,[SequenceId] FROM [dbo].[WHHeader] where Id = @Id";

            var parameter = new { WHParameter.WHDto.Id };
            WHHeader data;
            await using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<WHHeader>(query1, parameter).FirstOrDefault();
            }

            try
            {
                if (data == null)
                {
                    var idGenerator = new IdGenerator();
                    WHSequenceId = idGenerator.GenerateId(applicationDbContext, "WH-", "WHSequence");
                }
                else
                {
                    WHSequenceId = data.SequenceId;
                }

                var query =
                    @"MERGE INTO dbo.WHHeader t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE SET [CPCIdVehicle] = @CPCIdVehicle, [Name] = @Name,[Title] = @Title,[StatusId] = @StatusId,[TypeId] = @TypeId,[Address] = @Address,[OpeningHoursFrom] = @OpeningHoursFrom,[OpeningHoursTo] = @OpeningHoursTo ,[ManagerId] = @ManagerId,[Area] = @Area, [ModifiedBy] = @ModifiedBy,[ModifiedDate] = @ModifiedDate WHEN NOT MATCHED THEN INSERT ([Id],[CPCIdVehicle],[SequenceId],[Name],[Title],[StatusId],[TypeId],[Address],[OpeningHoursFrom],[OpeningHoursTo],[ManagerId],[Area],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate]) VALUES (@Id, @CPCIdVehicle,@SequenceId,@Name,@Title,@StatusId,@TypeId,@Address,@OpeningHoursFrom,@OpeningHoursTo,@ManagerId,@Area,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate);";
                var parameters = new
                {
                    WHParameter.WHDto.Id,
                    SequenceId = WHSequenceId,
                    WHParameter.WHDto.Name,
                    Title = WHSequenceId + " " + WHParameter.WHDto.Name,
                    WHParameter.WHDto.StatusId,
                    WHParameter.WHDto.TypeId,
                    Address = WHParameter.WHDto.LocationId,
                    WHParameter.WHDto.OpeningHoursFrom,
                    WHParameter.WHDto.OpeningHoursTo,
                    WHParameter.WHDto.ManagerId,
                    WHParameter.WHDto.Area,
                    CreatedBy = WHParameter.UserId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = WHParameter.UserId,
                    ModifiedDate = DateTime.UtcNow,
                    WHParameter.WHDto.CPCIdVehicle
                };

                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(query, parameters);
                }

                var taxonomyquery =
                    @"MERGE INTO dbo.WHTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.WareHouseId = @WareHouseId AND t1.WHTaxonomyLevelId = 'd60aad0b-2e84-482b-ad25-618d80d49477') WHEN MATCHED THEN UPDATE  SET Title = @Title WHEN NOT MATCHED THEN INSERT ( Id ,WareHouseId ,WHTaxonomyLevelId ,Title ) VALUES ( @Id,@WareHouseId,@WHTaxonomyLevelId,@Title);";
                var taxonomyparameters = new
                {
                    Id = Guid.NewGuid().ToString(),
                    WareHouseId = WHParameter.WHDto.Id,
                    Title = WHParameter.WHDto.Name,
                    WHTaxonomyLevelId = "d60aad0b-2e84-482b-ad25-618d80d49477"
                };

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(taxonomyquery, taxonomyparameters);
                }

                return WHSequenceId;
            }


            catch (Exception e)
            {
                throw e;
            }
        }

        throw new Exception("ID not set");
    }

    public async Task<string> CreateWHTaxonomy(WHParameter whParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(whParameter.ContractingUnitSequenceId,
            whParameter.ProjectSequenceId, whParameter.TenantProvider);

        if (whParameter.WHTaxonomyDto.Id == null) throw new Exception("ID not set");
        var query =
            @"MERGE INTO dbo.WHTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title,StartDate = @StartDate, EndDate = @EndDate WHEN NOT MATCHED THEN INSERT (Id,WareHouseId,ParentId,WHTaxonomyLevelId,Title,StartDate,EndDate ) VALUES (@Id,@WareHouseId,@ParentId,@WHTaxonomyLevelId,@Title,@StartDate,@EndDate );";
        var parameters = new
        {
            whParameter.WHTaxonomyDto.Id,
            whParameter.WHTaxonomyDto.Title,
            whParameter.WHTaxonomyDto.WareHouseId,
            whParameter.WHTaxonomyDto.ParentId,
            whParameter.WHTaxonomyDto.WHTaxonomyLevelId,
            whParameter.WHTaxonomyDto.StartDate,
            whParameter.WHTaxonomyDto.EndDate
        };

        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(query, parameters);
        }

        return whParameter.WHTaxonomyDto.Id;
    }

    public async Task<WHDropDownData> GetWHDropdown(WHParameter WHParameter)
    {
        const string query = @"select StatusId as [Key], Name as Text  from WHStatus where LanguageCode = @lang
                              ORDER BY DisplayOrder;select TypeId as [Key], Name as Text  from WHType where LanguageCode = @lang order by Name ;
                              SELECT * FROM dbo.WHTaxonomyLevel WHERE LanguageCode = @lang ORDER BY DisplayOrder";
        var mWhDropDownData = new WHDropDownData();
        var parameters = new { lang = WHParameter.Lang };
        await using var connection = new SqlConnection(WHParameter.TenantProvider.GetTenant().ConnectionString);
        var muilti = await connection.QueryMultipleAsync(query, parameters);
        mWhDropDownData.Status = muilti.Read<WHStatusDto>();
        mWhDropDownData.Types = muilti.Read<WHTypeDto>();
        mWhDropDownData.WHTaxonomyLevel = muilti.Read<WHTaxonomyLevel>();

        return mWhDropDownData;
    }

    public Task<IEnumerable<WHShortCutPaneDto>> GetShortcutpaneData(WHParameter WHParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<WHListDto>> GetWHList(WHParameter whParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(whParameter.ContractingUnitSequenceId,
            whParameter.ProjectSequenceId, whParameter.TenantProvider);
        var query = @"SELECT
                              WHHeader.Id AS Id
                             ,WHHeader.SequenceId AS SequenceId
                             ,WHHeader.Name AS Name
                             ,WHHeader.Title AS Title
                             ,WHHeader.StatusId AS StatusId
                             ,WHHeader.TypeId AS TypeId
                             ,WHHeader.Address AS Address
                             ,WHHeader.OpeningHoursFrom AS OpeningHoursFrom
                             ,WHHeader.OpeningHoursTo AS OpeningHoursTo
                             ,WHHeader.ManagerId AS ManagerId
                             ,WHHeader.Area AS Area
                             ,WHType.Name AS Type
                             ,WHStatus.Name AS Status
                            FROM dbo.WHHeader
                            LEFT OUTER JOIN dbo.WHType
                              ON WHHeader.TypeId = WHType.TypeId
                            LEFT OUTER JOIN dbo.WHStatus
                              ON WHHeader.StatusId = WHStatus.StatusId
                            WHERE ( WHType.LanguageCode = @lang
                            OR WHHeader.TypeId IS NULL)
                            AND ( WHStatus.LanguageCode = @lang
                            OR WHHeader.StatusId IS NULL)";

        var sb = new StringBuilder(query);

        if (whParameter.Filter.Title != null)
        {
            whParameter.Filter.Title = whParameter.Filter.Title.Replace("'", "''");

            var words = whParameter.Filter.Title.Split(" ");
            foreach (var element in words) sb.Append(" AND WHHeader.Title LIKE '%" + element + "%'");
        }

        if (whParameter.Filter.SequenceId != null)
        {
            whParameter.Filter.SequenceId = whParameter.Filter.SequenceId.Replace("'", "''");
            sb.Append(" AND WHHeader.SequenceId LIKE '%" + whParameter.Filter.SequenceId + "%'");
        }
        
        if (whParameter.Filter.Status != null)
            sb.Append(" AND WHStatus.StatusId = '" + whParameter.Filter.Status + "' ");

        if (whParameter.Filter.Type != null) sb.Append(" AND WHType.TypeId = '" + whParameter.Filter.Type + "' ");

        if (whParameter.Filter.Sorter.Attribute == null) sb.Append(" ORDER BY WHHeader.Title desc");

        if (whParameter.Filter.Sorter.Attribute != null)
            switch (whParameter.Filter.Sorter.Attribute.ToLower())
            {
                case "title":
                    sb.Append(" ORDER BY WHHeader.Title " + whParameter.Filter.Sorter.Order);
                    break;
                case "type":
                    sb.Append(" ORDER BY WHType.Name " + whParameter.Filter.Sorter.Order);
                    break;
                case "status":
                    sb.Append(" ORDER BY WHStatus.Name " + whParameter.Filter.Sorter.Order);
                    break;
                case "sequenceid":
                    sb.Append(" ORDER BY WHHeader.SequenceId " + whParameter.Filter.Sorter.Order);
                    break;
            }

        var parameters = new { lang = whParameter.Lang };
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        var data = await connection.QueryAsync<WHListDto>(sb.ToString(), parameters);
        
        return data;
    }

    public async Task<WHHeaderDto> GetWHById(WHParameter WHParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(WHParameter.ContractingUnitSequenceId,
                WHParameter.ProjectSequenceId, WHParameter.TenantProvider);

            var historyQuery =
                @"SELECT [CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM [dbo].[WHHeader] where [SequenceId] =@SequenceId ";
            var parameter = new { SequenceId = WHParameter.Id };
            WHHistoryDto historyLog;

            var query = @"SELECT
                                  WHHeader.Id AS Id
                                 ,WHHeader.SequenceId AS SequenceId
                                 ,WHHeader.Name AS Name
                                 ,WHHeader.Title AS Title
                                 ,WHHeader.Address AS LocationId
                                 ,WHHeader.OpeningHoursFrom AS OpeningHoursFrom
                                 ,WHHeader.OpeningHoursTo AS OpeningHoursTo
                                 ,WHHeader.ManagerId AS ManagerId
                                 ,WHHeader.Area AS Area
                                 ,WHTaxonomy.Id AS WareHouseTaxonomyId
                                 ,WHType.TypeId AS [Key]
                                 ,WHType.Name AS [Text]
                                 ,WHStatus.StatusId AS [Key]
                                 ,WHStatus.Name AS [Text] 
                                 ,CorporateProductCatalog.Id AS [Value]
                                 ,CorporateProductCatalog.Title AS [Label]
                                FROM dbo.WHHeader
                                LEFT OUTER JOIN dbo.WHType
                                  ON WHHeader.TypeId = WHType.TypeId
                                LEFT OUTER JOIN dbo.WHStatus
                                  ON WHHeader.StatusId = WHStatus.StatusId
                                LEFT OUTER JOIN dbo.WHTaxonomy
                                  ON WHHeader.Id =.WHTaxonomy.WareHouseId
                                LEFT OUTER JOIN dbo.CorporateProductCatalog
                                  ON WHHeader.CPCIdVehicle = CorporateProductCatalog.Id
                                WHERE ( WHType.LanguageCode = @lang
                                OR WHHeader.TypeId IS NULL)
                                AND ( WHStatus.LanguageCode = @lang
                                OR WHHeader.StatusId IS NULL) AND WHHeader.SequenceId = @Id ";

            var parameters = new { lang = WHParameter.Lang, WHParameter.Id };

            WHHeaderDto mWHHeaderDto = null;
            await using (var connection = new SqlConnection(connectionString))
            {
                historyLog = connection.Query<WHHistoryDto>(historyQuery, parameter).FirstOrDefault();

                mWHHeaderDto = connection.Query<WHHeaderDto, WHTypeDto, WHStatusDto, CpcWareHousetDto, WHHeaderDto>(
                    query,
                    (whHeader, whTypeDto, whStatusDto, cpcWareHousetDto) =>
                    {
                        whHeader.Status = whStatusDto;
                        whHeader.Type = whTypeDto;
                        whHeader.VehicleNumber = cpcWareHousetDto;
                        return whHeader;
                    }, parameters,
                    splitOn: "Key, Key, Value").FirstOrDefault();

                
            }

            var ModifiedByUserQuery =
                @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

            var CabPearsonQuery =
                @"SELECT CabPerson.FullName AS FullName,CabPerson.Id AS PersonCompanyId  FROM dbo.CabPerson WHERE CabPerson.IsDeleted = 0 AND CabPerson.Id = @oid ";


            await using (var connection =
                         new SqlConnection(WHParameter.TenantProvider.GetTenant().ConnectionString))
            {
                var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
                historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                    .FirstOrDefault();

                var CreatByParam = new { oid = historyLog.CreatedBy };
                historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();

                var MangerParam = new { oid = mWHHeaderDto.ManagerId };
                mWHHeaderDto.ManagerName = connection.Query<string>(CabPearsonQuery, MangerParam).FirstOrDefault();
            }

            mWHHeaderDto.History = historyLog;

            if (mWHHeaderDto.LocationId == null) return mWHHeaderDto;
            WHParameter.LocationId = mWHHeaderDto.LocationId;
            mWHHeaderDto.MapLocation = await ReadLocation(WHParameter);

            return mWHHeaderDto;
        }


        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<MapLocation> ReadLocation(WHParameter WHParameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            //string connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId, pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
            var context = new ShanukaDbContext(options, WHParameter.TenantProvider.GetTenant().ConnectionString,
                WHParameter.TenantProvider);


            var MapLocation = context.MapLocation.Where(L => L.Id == WHParameter.LocationId).Include(m => m.Address)
                .Include(m => m.Position)
                .Include(m => m.Viewport).ThenInclude(v => v.BtmRightPoint)
                .Include(m => m.Viewport).ThenInclude(v => v.TopLeftPoint)
                .Include(b => b.BoundingBox).ThenInclude(v => v.BtmRightPoint)
                .Include(b => b.BoundingBox).ThenInclude(v => v.TopLeftPoint)
                .Include(d => d.DataSources).ThenInclude(d => d.Geometry).ToList().FirstOrDefault();

            return MapLocation;

            return null;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<WHTaxonomyListDto>> GetWHTaxonomyList(WHParameter WHParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(WHParameter.ContractingUnitSequenceId,
            WHParameter.ProjectSequenceId, WHParameter.TenantProvider);


        var query =
            @"SELECT WHTaxonomy.Id,WHTaxonomy.WareHouseId,WHTaxonomy.ParentId,WHTaxonomy.WHTaxonomyLevelId,WHTaxonomy.Title,WHTaxonomy.StartDate,WHTaxonomy.EndDate,WHTaxonomyLevel.DisplayOrder,WHTaxonomyLevel.IsChildren FROM dbo.WHTaxonomy LEFT OUTER JOIN dbo.WHTaxonomyLevel ON WHTaxonomy.WHTaxonomyLevelId = WHTaxonomyLevel.LevelId WHERE (WHTaxonomyLevel.LanguageCode = @lang OR WHTaxonomy.WHTaxonomyLevelId IS NULL)";

        var sb = new StringBuilder(query);

        if (WHParameter.WHTaxonomyFilter.WareHouseId != null)
            sb.Append(" AND WHTaxonomy.WareHouseId = '" + WHParameter.WHTaxonomyFilter.WareHouseId + "'");

        var parameters = new { lang = WHParameter.Lang };
        IEnumerable<WHTaxonomyListDto> data;
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            data = await connection.QueryAsync<WHTaxonomyListDto>(sb.ToString(), parameters);

            
        }

        return data;
    }

    public async Task<List<WHRockCpcList>> GetWHRockCpcList(WHParameter WHParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(WHParameter.ContractingUnitSequenceId,
            WHParameter.ProjectSequenceId, WHParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);


        var query = @"SELECT
                      WHRockCpc.*
                     ,WHTaxonomy.Title AS Location
                     ,WHTaxonomy.Id As LocationId
                     ,CorporateProductCatalog.ResourceTitle AS ResourceItem
                    FROM dbo.WHRockCpc
                    LEFT OUTER JOIN dbo.StockHeader
                      ON WHRockCpc.CpcId = StockHeader.CPCId
                    LEFT OUTER JOIN dbo.WHTaxonomy
                      ON StockHeader.WareHouseTaxonomyId = WHTaxonomy.Id
                    LEFT OUTER JOIN dbo.CorporateProductCatalog
                      ON WHRockCpc.CpcId = CorporateProductCatalog.Id WHERE WHRockCpc.UserId = @UserId AND Date = @Date ";

        var sb = new StringBuilder(query);

        if (WHParameter.WHRockCpcFilter.type != null && !WHParameter.WHRockCpcFilter.text.IsNullOrEmpty())
        {
            WHParameter.WHRockCpcFilter.text = WHParameter.WHRockCpcFilter.text.Replace("'", "''");

            switch (WHParameter.WHRockCpcFilter.type.ToLower())
            {
                case "resourceitem":
                    sb.Append(" AND CorporateProductCatalog.ResourceTitle LIKE '%" + WHParameter.WHRockCpcFilter.text +
                              "%'");
                    break;
                case "location":
                    sb.Append(" AND WHTaxonomy.Title LIKE '%" + WHParameter.WHRockCpcFilter.text + "%'");
                    break;
                case "quantity":
                    sb.Append(" AND WHRockCpc.Quantity LIKE '%" + WHParameter.WHRockCpcFilter.text + "%'");
                    break;
                case "all":
                    sb.Append(" AND WHRockCpc.Quantity LIKE '%" + WHParameter.WHRockCpcFilter.text +
                              "%' OR CorporateProductCatalog.ResourceTitle LIKE '%" + WHParameter.WHRockCpcFilter.text +
                              "%' OR WHTaxonomy.Title LIKE '%" + WHParameter.WHRockCpcFilter.text + "%'");
                    break;
            }
        }

        if (WHParameter.WHRockCpcFilter.Sorter.Order != null &&
            !WHParameter.WHRockCpcFilter.Sorter.Order.IsNullOrEmpty())
            switch (WHParameter.WHRockCpcFilter.type.ToLower())
            {
                case "resourceitem":
                    sb.Append(" ORDER BY CorporateProductCatalog.ResourceTitle " +
                              WHParameter.WHRockCpcFilter.Sorter.Order);
                    break;
                case "location":
                    sb.Append(" ORDER BY WHTaxonomy.Title " + WHParameter.WHRockCpcFilter.Sorter.Order);
                    break;
                case "quantity":
                    sb.Append(" ORDER BY CAST(SUBSTRING(WHRockCpc.Quantity,0,20) AS INT) " +
                              WHParameter.WHRockCpcFilter.Sorter.Order);
                    break;
                case "all":
                    sb.Append(" ORDER BY DateTime " + WHParameter.WHRockCpcFilter.Sorter.Order);
                    break;
            }
        else
            sb.Append(" ORDER BY DateTime desc ");

        var parameters = new { lang = WHParameter.Lang, WHParameter.UserId, DateTime.UtcNow.Date };
        List<WHRockCpcList> data;

        data = connection.Query<WHRockCpcList>(sb.ToString(), parameters).ToList();

        return data;
    }

    public async Task<StockHeader> GetWHRockCpcById(WHParameter WHParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(WHParameter.ContractingUnitSequenceId,
                WHParameter.ProjectSequenceId, WHParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            var select = @"SELECT
                              StockHeader.Id
                             ,StockHeader.SequenceId
                             ,StockHeader.Name
                             ,StockHeader.Title
                             ,StockHeader.TypeId
                             ,StockHeader.StatusId
                             ,StockHeader.AvailableQuantity
                             ,StockHeader.MOUId
                             ,StockHeader.AveragePrice
                             ,StockHeader.QuantityToBeDelivered
                             ,StockHeader.WareHouseTaxonomyId
                             ,StockHeader.CreatedBy
                             ,StockHeader.CreatedDate
                             ,StockHeader.ModifiedBy
                             ,StockHeader.ModifiedDate
                             ,StockHeader.ActiveTypeId
                             ,CorporateProductCatalog.ResourceTitle AS CPCId
                            FROM dbo.StockHeader LEFT OUTER JOIN CorporateProductCatalog ON StockHeader.CPCId = CorporateProductCatalog.Id WHERE StockHeader.CPCId = @Id";

            var stock = connection.Query<StockHeader>(select, new { WHParameter.Id }).FirstOrDefault();

            if (stock != null)
            {
                var taxonomy = @"with name_tree as
                                (SELECT
                            WHTaxonomy.Id
                                ,WHTaxonomy.WareHouseId
                                ,WHTaxonomy.WHTaxonomyLevelId
                                ,ParentId
                                ,Title
                                FROM dbo.WHTaxonomy
                                WHERE WHTaxonomy.Id = @Id
                            UNION ALL
                            SELECT c.Id, c.WareHouseId,c.WHTaxonomyLevelId ,c.ParentId,c.Title
                                FROM dbo.WHTaxonomy c
                            JOIN name_tree p on p.ParentId = c.Id)
                            select WHTaxonomyLevelId, WareHouseId, Id,ParentId,Title
                            from name_tree
                            where WHTaxonomyLevelId is not NULL";

                var mtaxonomy = connection.Query<WHTaxonomy>(taxonomy, new { Id = stock.WareHouseTaxonomyId }).ToList();

                string WareHouseTaxonomyId = null;
                foreach (var i in mtaxonomy)
                    if (WareHouseTaxonomyId == null)
                        WareHouseTaxonomyId = i.Title;
                    else
                        WareHouseTaxonomyId = i.Title + " > " + WareHouseTaxonomyId;

                stock.WareHouseTaxonomyId = WareHouseTaxonomyId;
            }

            return stock;
        }

        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> SaveWHRockCpc(WHParameter WHParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(WHParameter.ContractingUnitSequenceId,
                WHParameter.ProjectSequenceId, WHParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            var id = connection.Query<string>("SELECT Id FROM dbo.WHRockCpc WHERE StockId = @StockId;",
                new { WHParameter.WHRockCpcDto.StockId }).FirstOrDefault();

            var param = new
            {
                Id = Guid.NewGuid(),
                WHParameter.WHRockCpcDto.CpcId,
                WHParameter.WHRockCpcDto.Quantity,
                WHParameter.WHRockCpcDto.Comment,
                WHParameter.WHRockCpcDto.CommentUrl,
                WHParameter.WHRockCpcDto.Correct,
                WHParameter.WHRockCpcDto.StockId,
                DateTime.UtcNow.Date,
                DateTime = DateTime.UtcNow,
                WHParameter.UserId
            };

            var updateStock = @"UPDATE dbo.StockHeader 
                                    SET
                                      AvailableQuantity = @Quantity
                                     ,ModifiedBy = @UserId
                                     ,ModifiedDate = @DateTime
                                    WHERE
                                      CPCId = @CpcId;";

            var insert =
                @"INSERT INTO dbo.WHRockCpc ( Id ,CpcId ,Quantity ,Comment ,CommentUrl ,Correct ,StockId ,Date ,UserId,DateTime ) VALUES ( @Id ,@CpcId ,@Quantity ,@Comment ,@CommentUrl ,@Correct ,@StockId ,@Date ,@UserId,@DateTime );";

            await connection.ExecuteAsync(insert, param);

            await connection.ExecuteAsync(updateStock, param);

            var wfid = await CreateWF(WHParameter);

            var image = connection
                .Query<WHRockCpcImage>("SELECT Id ,StockId ,Url FROM dbo.WHRockCpcImage WHERE StockId = @Id",
                    new { Id = WHParameter.WHRockCpcDto.StockId }).ToList();

            foreach (var i in image)
            {
                var wfdoc = @"INSERT INTO dbo.WFDocument ( Id ,Link ,WFHeaderId ) VALUES ( @Id ,@Link ,@WFHeaderId );";

                await connection.ExecuteAsync(wfdoc, new { Id = Guid.NewGuid(), Link = i.Url, WFHeaderId = wfid });
            }


            // else
            // {
            //     string update = @"UPDATE dbo.WHRockCpc 
            //                         SET Quantity = @Quantity
            //                           ,Comment = @Comment
            //                          ,Correct = @Correct
            //                          ,Date = @Date
            //                          ,DateTime =@DateTime
            //                         WHERE
            //                           StockId = @StockId
            //                         ;";
            //     
            //     await connection.ExecuteAsync(update, param);
            //
            //     await connection.ExecuteAsync(updateStock, param);
            //
            //     string wfid = connection.Query<string>("SELECT Id FROM dbo.WFHeader WHERE StockId = @StockId", param)
            //         .FirstOrDefault();
            //     
            //     await connection.ExecuteAsync("UPDATE dbo.WFHeader SET Comment = @Comment WHERE Id = @wfid", new{wfid,WHParameter.WHRockCpcDto.Comment});
            //     
            //     await connection.ExecuteAsync("UPDATE dbo.WFTask SET Quantity = @Quantity WHERE WorkFlowId = @wfid AND CPCItemId = @CpcId", new{wfid,WHParameter.WHRockCpcDto.CpcId});
            //     
            //     await connection.ExecuteAsync("DELETE FROM dbo.WFDocument WHERE WFHeaderId = @wfid", new{wfid});
            //     
            //     
            //     
            //     var image = connection
            //         .Query<WHRockCpcImage>("SELECT Id ,StockId ,Url FROM dbo.WHRockCpcImage WHERE StockId = @Id",
            //             new {Id = WHParameter.WHRockCpcDto.StockId }).ToList();
            //      
            //     
            //     foreach (var i in image)
            //     {
            //         string wfdoc = @"INSERT INTO dbo.WFDocument ( Id ,Link ,WFHeaderId ) VALUES ( @Id ,@Link ,@WFHeaderId );";
            //     
            //         await connection.ExecuteAsync(wfdoc, new {Id = Guid.NewGuid(),Link = i.Url,WFHeaderId = wfid});
            //     }
            //
            // }

            return param.Id.ToString();
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> UploadImageForMobile(WHParameter WHParameter)
    {
       // var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(WHParameter.ContractingUnitSequenceId,
            WHParameter.ProjectSequenceId, WHParameter.TenantProvider);
       // var context = new ShanukaDbContext(options, connectionString, WHParameter.TenantProvider);
        await using var connection = new SqlConnection(connectionString);
        var client = new FileClient();

        string stockId = WHParameter.formData["StockId"];
        string Id = null;
        foreach (var file in WHParameter.formData.Files)
        {
            var url = client.PersistPhotoInNewFolder(file?.FileName, WHParameter.TenantProvider, file,
                "UploadExtraworkFiles");

            var whImage =
                @"INSERT INTO dbo.WHRockCpcImage ( Id ,StockId ,Url,Date ) VALUES ( @Id ,@stockId ,@Url,@date );";

            await connection.ExecuteAsync(whImage, new { Id = Guid.NewGuid(), stockId, url, date = DateTime.UtcNow });
        }

        return Id;
    }

    public async Task<string> CreateWF(WHParameter whParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, whParameter.TenantProvider);

        var connectionString = ConnectionString.MapConnectionString(whParameter.ContractingUnitSequenceId,
            whParameter.ProjectSequenceId, whParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var wfId = Guid.NewGuid().ToString();

        var idGenerator = new IdGenerator();
        var WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");

        var strock = connection.Query<StockHeader>("SELECT * FROM dbo.StockHeader WHERE Id = @Id",
            new { Id = whParameter.WHRockCpcDto.StockId }).FirstOrDefault();

        var wfquery = @"INSERT INTO dbo.WFHeader
                                            (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Project,BorId,Comment,IsFromCU,PoId,StockId)
                                             VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Project,@BorId,@Comment,@IsFromCU,@PoId,@StockId)";

        var wfparameters = new
        {
            Id = wfId,
            SequenceId = WFSequenceId,
            Name = strock.Title,
            Title = WFSequenceId + " " + strock.Title,
            TypeId = "4010e768-fety-4702-bnew-ee367a82addb",
            ResourceType = strock.TypeId,
            CreatedBy = whParameter.UserId,
            CreatedDate = DateTime.UtcNow,
            Source = "",
            Destination = strock.WareHouseTaxonomyId,
            IsFinish = false,
            RequesterId = "",
            ExecutorId = "",
            RequiredDateAndTime = DateTime.UtcNow,
            ExecutedDateAndTime = "",
            EffortEstimate = 0,
            EffortCompleted = 0,
            Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
            Project = "",
            BorId = "",
            IsFromCU = false,
            PoId = "",
            StockId = strock.Id,
            whParameter.WHRockCpcDto.Comment
        };

        var taskId = Guid.NewGuid().ToString();

        var taskparameters = new
        {
            Id = taskId,
            WorkFlowId = wfId,
            CPCItemId = strock.CPCId,
            Quantity = strock.AvailableQuantity,
            PickedQuantity = 0,
            Mou = strock.MOUId,
            StockAvailability = 0,
            UnitPrice = strock.AveragePrice,
            Destination = strock.WareHouseTaxonomyId
        };

        var taskquery =
            @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,Destination,StockAvailability,UnitPrice) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@Destination,@StockAvailability,@UnitPrice)";


        await connection.ExecuteAsync(wfquery, wfparameters);
        await connection.ExecuteAsync(taskquery, taskparameters);

        return wfId;
    }
}