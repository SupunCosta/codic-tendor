using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Repositories.WF;

public class WorkFlowRepository : IWorkFlowRepository
{
    public async Task<string> CreateHeader(WFParameter wfParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(wfParameter.ContractingUnitSequenceId,
            wfParameter.ProjectSequenceId, wfParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, wfParameter.TenantProvider);

        if (wfParameter.WFDto.Id == null) throw new Exception("ID not set");
        var query1 = @"SELECT [Id] ,[SequenceId] FROM [dbo].[WFHeader] Where Id = @Id";

        var parameter = new { wfParameter.WFDto.Id };
        WFHeader data;
        await using (var connection = new SqlConnection(connectionString))
        {
            data = connection.Query<WFHeader>(query1, parameter).FirstOrDefault();
        }


        string wfSequenceId;
        if (data == null)
        {
            var idGenerator = new IdGenerator();
            wfSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");

            var query = @"INSERT INTO dbo.WFHeader
                                        (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutedDateAndTime,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Comment)
                                         VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutedDateAndTime,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Comment)";
            var parameters = new
            {
                wfParameter.WFDto.Id,
                SequenceId = wfSequenceId,
                wfParameter.WFDto.Name,
                Title = wfSequenceId + " " + wfParameter.WFDto.Name,
                wfParameter.WFDto.TypeId,
                wfParameter.WFDto.ResourceType,
                CreatedBy = wfParameter.UserId,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = wfParameter.UserId,
                ModifiedDate = DateTime.UtcNow,
                wfParameter.WFDto.Source,
                wfParameter.WFDto.Destination,
                IsFinish = wfParameter.WFDto.IsFinished,
                wfParameter.WFDto.RequesterId,
                wfParameter.WFDto.ExecutorId,
                wfParameter.WFDto.RequiredDateAndTime,
                wfParameter.WFDto.ExecutedDateAndTime,
                wfParameter.WFDto.EffortEstimate,
                wfParameter.WFDto.EffortCompleted,
                wfParameter.WFDto.Status,
                wfParameter.WFDto.Comment
            };

            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, parameters);
        }
        else
        {
            wfSequenceId = data.SequenceId;
            var query = @"UPDATE dbo.WFHeader
                               SET 
                            Name = @Name
                            ,Title = @Title
                            ,TypeId = @TypeId
                            ,ResourceType = @ResourceType
                            ,ModifiedBy = @ModifiedBy
                            ,ModifiedDate = @ModifiedDate
                            ,Destination = @Destination
                            ,IsFinish = @IsFinish
                            ,Source = @Source
                            ,EffortCompleted = @EffortCompleted
                            ,EffortEstimate = @EffortEstimate
                            ,ExecutedDateAndTime = @ExecutedDateAndTime
                            ,ExecutorId = @ExecutorId
                            ,RequesterId = @RequesterId
                            ,RequiredDateAndTime = @RequiredDateAndTime
                            ,StatusId = @Status
                            ,Comment =@Comment
                              WHERE Id = @Id";
            var parameters = new
            {
                wfParameter.WFDto.Id,
                SequenceId = wfSequenceId,
                wfParameter.WFDto.Name,
                Title = wfSequenceId + " " + wfParameter.WFDto.Name,
                wfParameter.WFDto.TypeId,
                wfParameter.WFDto.ResourceType,
                ModifiedBy = wfParameter.UserId,
                ModifiedDate = DateTime.UtcNow,
                wfParameter.WFDto.Source,
                wfParameter.WFDto.Destination,
                IsFinish = wfParameter.WFDto.IsFinished,
                wfParameter.WFDto.RequesterId,
                wfParameter.WFDto.ExecutorId,
                wfParameter.WFDto.RequiredDateAndTime,
                wfParameter.WFDto.ExecutedDateAndTime,
                wfParameter.WFDto.EffortEstimate,
                wfParameter.WFDto.EffortCompleted,
                wfParameter.WFDto.Status,
                wfParameter.WFDto.Comment
            };

            var deleteTask = @"DELETE FROM dbo.WFTask WHERE WorkFlowId = @Id";
            var deleteDocument = @"DELETE FROM [dbo].[WFDocument] WHERE WFHeaderId = @Id";

            await using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, parameters);
            await connection.ExecuteAsync(deleteTask, parameters);
            await connection.ExecuteAsync(deleteDocument, parameters);
        }

        var documentquery =
            @"INSERT INTO dbo.WFDocument (Id,Link,WFHeaderId) VALUES (@Id,@Link,@WFHeaderId)";

        if (wfParameter.WFDto.files != null)
            foreach (var mdoc in wfParameter.WFDto.files)
                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(documentquery,
                        new
                        {
                            Id = Guid.NewGuid().ToString(), Link = mdoc, WFHeaderId = wfParameter.WFDto.Id
                        });
                }

        var taskquery =
            @"INSERT INTO dbo.WFTask (Id,WorkFlowId,Source,CPCItemId,Quantity,MOUId,PickedQuantity,Destination,Comment,StockAvailability) VALUES (@Id,@WorkFlowId,@Source,@CPCItemId,@Quantity,@MOUId,@PickedQuantity,@Destination,@Comment,@StockAvailability)";

        if (wfParameter.WFDto.Tasks != null)
            foreach (var mWfTasksDto in wfParameter.WFDto.Tasks)
            {
                var taskparameter = new
                {
                    mWfTasksDto.Id,
                    WorkFlowId = wfParameter.WFDto.Id,
                    mWfTasksDto.Source,
                    mWfTasksDto.CPCItemId,
                    mWfTasksDto.Quantity,
                    MOUId = mWfTasksDto.Mou,
                    mWfTasksDto.PickedQuantity,
                    mWfTasksDto.Destination,
                    mWfTasksDto.Comment,
                    mWfTasksDto.StockAvailability
                };

                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(taskquery, taskparameter);
                }
            }

        return wfSequenceId;
    }

    public async Task<WFDropDownData> GetWFDropdown(WFParameter WFParameter)
    {
        var query = @"select TypeId as [Key], Name as Text  from WFType where LanguageCode = @lang order by Name;
                             SELECT StatusId AS [Key],Name AS Text FROM WFActivityStatus WHERE LanguageCode = @Lang ORDER BY Name;
                             select TypeId as [Key], Name as Text  from StockType where LanguageCode = @lang order by Name;";

        var mWfDropDownData = new WFDropDownData();
        var parameters = new { lang = WFParameter.Lang };
        await using var connection = new SqlConnection(WFParameter.TenantProvider.GetTenant().ConnectionString);
        var muilti = await connection.QueryMultipleAsync(query, parameters);
        mWfDropDownData.Types = muilti.Read<WFTypeDto>();
        mWfDropDownData.ActivityStatus = muilti.Read<WFActivityStatusDto>();
        mWfDropDownData.ResourceType = muilti.Read<StockTypeDto>();

        return mWfDropDownData;
    }

    public Task<IEnumerable<WFShortCutPaneDto>> GetShortcutpaneData(WFParameter WFParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<WFListDto>> GetWFList(WFParameter WFParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId,
            WFParameter.ProjectSequenceId, WFParameter.TenantProvider);

        var query = @"SELECT
                              WFHeader.*
                             ,WFType.Name AS WFTypeName
							 ,WFActivityStatus.Name AS WFStatusName
                             ,CpcResourceTypeLocalizedData.Label CpcName
                            FROM dbo.WFHeader
                            LEFT OUTER JOIN dbo.WFType
                              ON WFHeader.TypeId = WFType.TypeId
                            LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
							ON WFHeader.ResourceType = CpcResourceTypeLocalizedData.CpcResourceTypeId
							LEFT OUTER JOIN dbo.WFActivityStatus
                              ON WFHeader.StatusId = WFActivityStatus.StatusId
                              WHERE(WFType.LanguageCode = @lang OR WFHeader.TypeId IS NULL)
							  AND (WFActivityStatus.LanguageCode = @lang OR WFHeader.StatusId IS NULL)
                              AND(CpcResourceTypeLocalizedData.LanguageCode = @lang OR WFHeader.ResourceType IS NULL)";

        var sb = new StringBuilder(query);

        if (WFParameter.Filter.Title != null)
        {
            WFParameter.Filter.Title = WFParameter.Filter.Title.Replace("'", "''");

            var words = WFParameter.Filter.Title.Split(" ");
            foreach (var element in words) sb.Append(" AND WFHeader.Title LIKE '%" + element + "%'");
        }

        if (WFParameter.Filter.TargetDateTime != null)
        {
            var timeZone = new TimeZone
            {
                date = (DateTime)WFParameter.Filter.TargetDateTime,
                offset = WFParameter.Filter.Offset
            };
            var formatter = new TimeZoneFormatter();
            WFParameter.Filter.TargetDateTime = formatter.getUtcTimeFromLocalTime(timeZone);


            var finalOffset = FormatOffset(timeZone);
            var date = timeZone.date - timeZone.date.TimeOfDay;
            date = finalOffset switch
            {
                > 0 => date.AddHours(finalOffset * -1),
                < 0 => date.AddHours(finalOffset),
                _ => date
            };

            sb.Append(" AND WFHeader.RequiredDateAndTime  BETWEEN '" + date + "' AND '" + date.AddHours(24) + "' ");
        }

        if (WFParameter.Filter.ExecutedDateTime != null)
        {
            var timeZone = new TimeZone
            {
                date = (DateTime)WFParameter.Filter.ExecutedDateTime,
                offset = WFParameter.Filter.Offset
            };
            var formatter = new TimeZoneFormatter();
            WFParameter.Filter.ExecutedDateTime = formatter.getUtcTimeFromLocalTime(timeZone);


            var finalOffset = FormatOffset(timeZone);
            var date = timeZone.date - timeZone.date.TimeOfDay;
            date = finalOffset switch
            {
                > 0 => date.AddHours(finalOffset * -1),
                < 0 => date.AddHours(finalOffset),
                _ => date
            };

            sb.Append(" AND WFHeader.ExecutedDateAndTime  BETWEEN '" + date + "' AND '" + date.AddHours(23.99) +
                      "' ");
        }

        if (WFParameter.Filter.Type != null) sb.Append(" AND WFType.TypeId = '" + WFParameter.Filter.Type + "' ");

        if (WFParameter.Filter.Status != null)
            sb.Append(" AND WFActivityStatus.StatusId = '" + WFParameter.Filter.Status + "' ");

        if (WFParameter.Filter.Sorter.Attribute == null) sb.Append(" ORDER BY WFHeader.Title desc");

        if (WFParameter.Filter.Sorter.Attribute != null)
            switch (WFParameter.Filter.Sorter.Attribute.ToLower())
            {
                case "title":
                    sb.Append(" ORDER BY WFHeader.Title " + WFParameter.Filter.Sorter.Order);
                    break;
                case "wftypename":
                    sb.Append(" ORDER BY WFType.Name " + WFParameter.Filter.Sorter.Order);
                    break;
                case "wfstatusname":
                    sb.Append(" ORDER BY WFActivityStatus.Name " + WFParameter.Filter.Sorter.Order);
                    break;
                case "requireddateandtime":
                    sb.Append(" ORDER BY WFHeader.RequiredDateAndTime " + WFParameter.Filter.Sorter.Order);
                    break;
                case "executeddateandtime":
                    sb.Append(" ORDER BY WFHeader.ExecutedDateAndTime " + WFParameter.Filter.Sorter.Order);
                    break;
            }

        var parameters = new { lang = WFParameter.Lang };
        IEnumerable<WFListDto> data;
        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            data = await connection.QueryAsync<WFListDto>(sb.ToString(), parameters);

            
        }

        List<ApplicationUser> applicationUserList;
        List<CabPearsonDto> cabList;

        await using (var connection =
                     new SqlConnection(WFParameter.TenantProvider.GetTenant().ConnectionString))
        {
            applicationUserList = connection.Query<ApplicationUser>("SELECT * FROM dbo.ApplicationUser").ToList();
            cabList = connection.Query<CabPearsonDto>(
                    "SELECT CabPerson.FullName AS FullName,CabPersonCompany.Id AS PersonCompanyId  FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany ON CabPerson.Id = CabPersonCompany.PersonId WHERE CabPerson.IsDeleted = 0 ")
                .ToList();
        }

        foreach (var mWFListDto in data)
        {
            var ModifiedBy = applicationUserList.FirstOrDefault(c => c.OId == mWFListDto.ModifiedBy);

            if (ModifiedBy != null)
                mWFListDto.ModifiedBy = ModifiedBy.FirstName + " " + ModifiedBy.LastName;
            else
                mWFListDto.ModifiedBy = null;

            var createdBy = applicationUserList.FirstOrDefault(c => c.OId == mWFListDto.CreatedBy);
            if (createdBy != null) mWFListDto.CreatedBy = createdBy.FirstName + " " + createdBy.LastName;

            if (mWFListDto.RequesterId != null)
            {
                var requester = cabList.FirstOrDefault(c => c.PersonCompanyId == mWFListDto.RequesterId);

                mWFListDto.Requester = requester?.FullName;
            }

            if (mWFListDto.ExecutorId != null)
            {
                var executor = cabList.FirstOrDefault(c => c.PersonCompanyId == mWFListDto.ExecutorId);

                mWFListDto.Executor = executor?.FullName;
            }
        }

        if (WFParameter.Filter.Executer != null)
            data = data?.Where(x =>
                x.Executor != null &&
                x.ExecutorId.Contains(WFParameter.Filter.Executer, StringComparison.OrdinalIgnoreCase));

        if (WFParameter.Filter.Requester != null)
            data = data?.Where(x =>
                x.Requester != null &&
                x.RequesterId.Contains(WFParameter.Filter.Requester, StringComparison.OrdinalIgnoreCase));

        if (WFParameter.Filter.Sorter.Attribute != null)
        {
            if (WFParameter.Filter.Sorter.Attribute.ToLower().Equals("executor"))
            {
                if (WFParameter.Filter.Sorter.Order.ToLower().Equals("asc"))
                    data = from std in data
                        orderby std.Executor
                        select std;

                else
                    data = (from std in data
                        orderby std.Executor descending
                        select std).ToList();
            }

            if (!WFParameter.Filter.Sorter.Attribute.ToLower().Equals("requester")) return data;
            if (WFParameter.Filter.Sorter.Order.ToLower().Equals("asc"))
                data = from std in data
                    orderby std.Executor
                    select std;

            else
                data = (from std in data
                    orderby std.Executor descending
                    select std).ToList();
        }

        return data;
    }

    public async Task<WFHeaderDto> GetWFById(WFParameter WFParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId,
                WFParameter.ProjectSequenceId, WFParameter.TenantProvider);

            var historyQuery =
                @"SELECT [CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM [dbo].[WFHeader] where [SequenceId] =@SequenceId ";

            var parameter = new { SequenceId = WFParameter.Id };
            WHHistoryDto historyLog;

            var query = @"SELECT [WFHeader].[Id],
                                    [WFHeader].[SequenceId]
                                    ,[WFHeader].[Name]
                                    ,[WFHeader].[Title]
								    ,[Destination]
								    ,[IsFinish]
								    ,[Source]
								    ,[EffortCompleted]
								    ,[EffortEstimate]
								    ,[ExecutedDateAndTime]
								    ,[ExecutorId]
								    ,[RequesterId]
								    ,[RequiredDateAndTime] AS RequestedDateAndTime
                                    ,Project
                                    ,BorId
                                    ,Comment
                                    ,StockId
                                    ,WFType.TypeId AS [Key]
                                    ,WFType.Name AS [Text]
                                    ,CpcResourceTypeLocalizedData.CpcResourceTypeId AS [Key] 
                                    ,CpcResourceTypeLocalizedData.Label AS [Text]
									,WFActivityStatus.StatusId AS [Key]
									,WFActivityStatus.Name AS [Text]
                                    FROM [dbo].[WFHeader] LEFT OUTER JOIN dbo.WFType ON WFHeader.TypeId = WFType.TypeId 
                                    LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData ON WFHeader.ResourceType = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                    LEFT OUTER JOIN dbo.WFActivityStatus ON WFHeader.StatusId = WFActivityStatus.StatusId
									WHERE (WFType.LanguageCode = @lang OR WFHeader.TypeId IS NULL) 
                                    AND (CpcResourceTypeLocalizedData.LanguageCode = @lang OR WFHeader.ResourceType IS NULL)
									AND (WFActivityStatus.LanguageCode = @lang OR WFHeader.StatusId IS NULL)
                                    AND WFHeader.SequenceId = @Id";

            var parameters = new { lang = WFParameter.Lang, WFParameter.Id };

            WFHeaderDto mWFHeaderDto = null;
            using (var connection = new SqlConnection(connectionString))
            {
                historyLog = connection.Query<WHHistoryDto>(historyQuery, parameter).FirstOrDefault();


                mWFHeaderDto = connection
                    .Query<WFHeaderDto, WFTypeDto, CpcForProductDto, WFActivityStatusDto, WFHeaderDto>(
                        query,
                        (wfHeader, wfTypeDto, cpcDto, wfStatusDto) =>
                        {
                            wfHeader.CpcType = cpcDto;
                            wfHeader.Type = wfTypeDto;
                            wfHeader.Status = wfStatusDto;
                            return wfHeader;
                        }, parameters,
                        splitOn: "Key, Key,Key").FirstOrDefault();

                
            }

            var ModifiedByUserQuery =
                @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

            var CabPearsonQuery =
                @"SELECT CabPerson.FullName AS FullName,CabPersonCompany.Id AS PersonCompanyId  FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany ON CabPerson.Id = CabPersonCompany.PersonId WHERE CabPerson.IsDeleted = 0 AND CabPersonCompany.Id = @oid ";


            using (var connection =
                   new SqlConnection(WFParameter.TenantProvider.GetTenant().ConnectionString))
            {
                var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
                historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                    .FirstOrDefault();

                var CreatByParam = new { oid = historyLog.CreatedBy };
                historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();

                var RequesterParam = new { oid = mWFHeaderDto.RequesterId };
                mWFHeaderDto.Requester = connection.Query<string>(CabPearsonQuery, RequesterParam).FirstOrDefault();

                var ExecutorParam = new { oid = mWFHeaderDto.ExecutorId };
                mWFHeaderDto.Executor = connection.Query<string>(CabPearsonQuery, ExecutorParam).FirstOrDefault();
            }

            mWFHeaderDto.History = historyLog;

            var headerFiles = @"SELECT Link FROM dbo.WFDocument WHERE WFHeaderId =@Id";

            using (var connection = new SqlConnection(connectionString))
            {
                mWFHeaderDto.Files = connection.Query<string>(headerFiles, new { mWFHeaderDto.Id }).ToList();

                
            }

            var tasks = @"SELECT
                              WFTask.Id
                             ,WFTask.WorkFlowId
                             ,WFTask.Source
                             ,WFTask.Quantity
                             ,WFTask.PickedQuantity
                             ,WFTask.Destination
                             ,WFTask.Comment
                             ,WFTask.StockAvailability
							 ,WFTask.MOUId AS Mou
                             ,WFTask.CPCItemId
                             ,WFTask.UnitPrice
                             ,CorporateProductCatalog.Id AS [Key]
                             ,CorporateProductCatalog.Title AS Text
                             
                            FROM dbo.WFTask
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON WFTask.CPCItemId = CorporateProductCatalog.Id
                            WHERE WFTask.WorkFlowId = @Id";
            var taskparameters = new { mWFHeaderDto.Id, lang = WFParameter.Lang };
            using (var connection = new SqlConnection(connectionString))
            {
                mWFHeaderDto.Tasks = connection.Query<WFTasksDto, CorporateProductCatalogWFDto, WFTasksDto>(
                    tasks,
                    (wftask, cpc) =>
                    {
                        wftask.CorporateProductCatalog = cpc;
                        return wftask;
                    }, taskparameters,
                    splitOn: "Key").ToList();

                
            }

            if (mWFHeaderDto.StockId == null)
            {
                var connectionStringBor = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId,
                    mWFHeaderDto.Project, WFParameter.TenantProvider);

                using (var connection = new SqlConnection(connectionStringBor))
                {
                    mWFHeaderDto.BorTitle = connection
                        .Query<string>(@"SELECT Title FROM dbo.Bor WHERE Id = @Id;", new { Id = mWFHeaderDto.BorId })
                        .FirstOrDefault();
                }
            }

            IEnumerable<StockHeader> stocks;
            StockHeader selectStock;
            using (var connection = new SqlConnection(connectionString))
            {
                stocks = connection.Query<StockHeader>("SELECT * FROM dbo.StockHeader  WHERE CPCId IN @Ids",
                    new { Ids = mWFHeaderDto.Tasks.Select(t => t.CPCItemId).ToList() });
            }

            foreach (var wf in mWFHeaderDto.Tasks)
                if (wf.CPCItemId != null)
                {
                    selectStock = stocks.Where(s =>
                            s.CPCId == wf.CPCItemId && s.ActiveTypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1")
                        .FirstOrDefault();
                    if (selectStock != null)
                        wf.Stock = selectStock;
                    else
                        wf.Stock = stocks.Where(s => s.CPCId == wf.CPCItemId).FirstOrDefault();
                }

            return mWFHeaderDto;
        }


        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CUApprove(WFParameter WFParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, WFParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId, null,
            WFParameter.TenantProvider);
        //string connectionprojectString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId, null, WFParameter.TenantProvider);
        string WFSequenceId = null;

        IEnumerable<WFTasksDto> taskData = null;
        IEnumerable<WFDocumentDto> documentdata;
        string StockSequenceId;
        string date = null;
        //Boolean isfromcu = false;


        try
        {
            var approvequery =
                @"UPDATE dbo.WFHeader SET  StatusId = '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da' WHERE SequenceId = @SequenceId"; //Approved
            var handedOverquery =
                @"UPDATE dbo.WFHeader SET  StatusId = '4010e768-3e06-4702-b337-ee367a82addb' WHERE SequenceId = @SequenceId"; //Handed Over

            var query = @"SELECT * FROM dbo.WFHeader where SequenceId = @SequenceId";
            var parameter = new { SequenceId = WFParameter.Id };
            WFHeader wfData;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(approvequery, parameter);
                await connection.ExecuteAsync(handedOverquery, parameter);
                wfData = connection.Query<WFHeader>(query, parameter).FirstOrDefault();
            }

            if (wfData.TypeId == "4010e768-fety-4702-bnew-ee367a82addb")
            {
                var parm = new { WorkFlowId = wfData.Id };

                var taskselect = @"SELECT * FROM [dbo].[WFTask] WHERE WorkFlowId = @WorkFlowId ";

                var stockHistoryInsert =
                    @"INSERT INTO dbo.StockHistoryLog (Id,WorkFlowId,DateTime,ActivityTypeId,Quantity,MOUId,WareHouseWorker,StockHeaderId,Price) VALUES (@Id,@WorkFlowId,@DateTime,@ActivityTypeId,@Quantity,@MOUId,@WareHouseWorker,@StockHeaderId,@Price)";

                using (var connection = new SqlConnection(connectionString))
                {
                    var wf = connection.Query<WFTasksDto>(taskselect, parm).FirstOrDefault();

                    var cpc = connection
                        .Query<CorporateProductCatalogDto>(
                            "SELECT * FROM dbo.CorporateProductCatalog  WHERE Id = @Id",
                            new { Id = wf.CPCItemId }).FirstOrDefault();
                    await connection.ExecuteAsync(stockHistoryInsert, new
                    {
                        Id = Guid.NewGuid().ToString(),
                        WorkFlowId = wfData.Id,
                        DateTime = DateTime.UtcNow,
                        ActivityTypeId = "7bcb4e8d-8e8c-inst-81sc-6b91c89fc3da",
                        Quantity = wf.PickedQuantity,
                        wf.MOUId,
                        WareHouseWorker = wfData.ExecutorId,
                        StockHeaderId = wfData.StockId,
                        Price = cpc.InventoryPrice
                    });
                }
            }

            else
            {
                if (wfData.IsFromCU & (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb")) //good reception
                {
                    var parm = new { WorkFlowId = wfData.Id };

                    var taskselect = @"SELECT * FROM [dbo].[WFTask] WHERE WorkFlowId = @WorkFlowId ";

                    var stockHistoryInsert =
                        @"INSERT INTO dbo.StockHistoryLog (Id,WorkFlowId,DateTime,ActivityTypeId,Quantity,MOUId,WareHouseWorker,StockHeaderId,Price) VALUES (@Id,@WorkFlowId,@DateTime,@ActivityTypeId,@Quantity,@MOUId,@WareHouseWorker,@StockHeaderId,@Price)";

                    await using (var connection = new SqlConnection(connectionString))
                    {
                        var wfTask = connection.Query<WFTasksDto>(taskselect, parm).ToList();

                        foreach (var wf in wfTask)
                        {
                            var cpc = connection
                                .Query<CorporateProductCatalogDto>(
                                    "SELECT * FROM dbo.CorporateProductCatalog  WHERE Id = @Id",
                                    new { Id = wf.CPCItemId }).FirstOrDefault();
                            var stockId = connection.Query<string>("SELECT Id FROM dbo.StockHeader WHERE CPCId = @Id;",
                                new { Id = wf.CPCItemId }).FirstOrDefault();

                            if (stockId == null)
                            {
                                var idGenerator1 = new IdGenerator();
                                StockSequenceId =
                                    idGenerator1.GenerateId(applicationDbContext, "Stock-", "StockSequence");

                                var StockInsert =
                                    @"INSERT INTO [dbo].[StockHeader]([Id],[SequenceId],[Name],[Title],[CPCId],[AvailableQuantity],[MOUId],[AveragePrice],[CreatedBy],[CreatedDate],[TypeId])VALUES(@Id,@SequenceId,@Name,@Title,@CPCId,@AvailableQuantity,@MOUId,@AveragePrice,@CreatedBy,@CreatedDate,@TypeId)";

                                var parameters1 = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SequenceId = StockSequenceId,
                                    Name = cpc.ResourceTitle,
                                    Title = StockSequenceId + " " + cpc.ResourceTitle,
                                    CPCId = wf.CPCItemId,
                                    AvailableQuantity = "0",
                                    wf.MOUId,
                                    AveragePrice = cpc.InventoryPrice.ToString(),
                                    //WareHouseTaxonomyId = StockParameter.StockDto.WareHouseTaxonomyId,
                                    CreatedBy = "",
                                    CreatedDate = DateTime.UtcNow,
                                    TypeId = wfData.ResourceType
                                };


                                await connection.ExecuteAsync(StockInsert, parameters1);

                                stockId = parameters1.Id;
                            }

                            await connection.ExecuteAsync(stockHistoryInsert, new
                            {
                                Id = Guid.NewGuid().ToString(),
                                WorkFlowId = wfData.Id,
                                DateTime = DateTime.UtcNow,
                                ActivityTypeId = "7bcb4e8d-8e8c-inst-8170-6b91c89fc3da",
                                Quantity = wf.PickedQuantity,
                                wf.MOUId,
                                WareHouseWorker = wfData.ExecutorId,
                                StockHeaderId = stockId,
                                Price = cpc.InventoryPrice
                            });
                        }
                    }

                    WFSequenceId = await CreateWorkFlowForPoFromCu(WFParameter, wfData);
                }

                else
                {
                    if (wfData != null)
                    {
                        var idGenerator = new IdGenerator();
                        WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");
                        var Id = Guid.NewGuid().ToString();

                        var createquery = @"INSERT INTO dbo.WFHeader
                                            (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Destination,IsFinish,Source,StatusId,BorId,Project,RequiredDateAndTime,PoId,IsFromCU,ExecutedDateAndTime)
                                             VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Destination,@IsFinish,@Source,@Status,@BorId,@Project,@RequiredDateAndTime,@PoId,@IsFromCU,@ExecutedDateAndTime)";
                        var parameters = new
                        {
                            Id,
                            SequenceId = WFSequenceId,
                            wfData.Name,
                            Title = WFSequenceId + " " + wfData.Name,
                            TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
                            wfData.ResourceType,
                            CreatedBy = WFParameter.UserId,
                            CreatedDate = DateTime.UtcNow,
                            ModifiedBy = WFParameter.UserId,
                            ModifiedDate = DateTime.UtcNow,
                            wfData.Source,
                            wfData.Destination,
                            IsFinish = false,
                            wfData.Project,
                            RequiredDateAndTime = DateTime.UtcNow,
                            ExecutedDateAndTime = date,
                            Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
                            BorID = wfData.BorId,
                            wfData.PoId,
                            wfData.IsFromCU
                        };

                        var connectionprojectString = ConnectionString.MapConnectionString(
                            WFParameter.ContractingUnitSequenceId, wfData.Project, WFParameter.TenantProvider);

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            await connection.ExecuteAsync(createquery, parameters);
                        }

                        var jsonProduct = JsonConvert.SerializeObject(parameters, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });

                        await WFHistoryLog(WFParameter, connectionString, "WFCuApprove", jsonProduct, WFParameter.Id,
                            WFSequenceId);


                        var taskselect = @"SELECT * FROM [dbo].[WFTask] WHERE WorkFlowId = @WorkFlowId ";
                        var documentselect = @"SELECT Link FROM dbo.WFDocument WHERE WFHeaderId = @WorkFlowId";

                        var parm = new { WorkFlowId = wfData.Id };


                        using (var connection = new SqlConnection(connectionString))
                        {
                            taskData = connection.Query<WFTasksDto>(taskselect, parm);
                            documentdata = connection.Query<WFDocumentDto>(documentselect, parm);
                        }

                        var taskquery =
                            @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,UnitPrice) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@MOUId,@PickedQuantity,@UnitPrice)";

                        foreach (var a in taskData)
                        {
                            StockHeaderDto projectStocks;
                            using (var connection = new SqlConnection(connectionprojectString))
                            {
                                projectStocks = connection
                                    .Query<StockHeaderDto>("SELECT * FROM dbo.StockHeader  WHERE CPCId = @Id",
                                        new { Id = a.CPCItemId }).FirstOrDefault();
                            }

                            if (projectStocks != null)
                            {
                                var taskparameters = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    WorkFlowId = Id,
                                    // Source = a.Source,
                                    a.CPCItemId,
                                    a.Quantity,
                                    a.MOUId,
                                    a.PickedQuantity,
                                    a.UnitPrice
                                    //Destination = a.Destination,
                                    //StockAvailability = projectStocks.AvailableQuantity
                                };

                                using (var connection = new SqlConnection(connectionprojectString))
                                {
                                    await connection.ExecuteAsync(taskquery, taskparameters);
                                }
                            }
                            else
                            {
                                //StockHeaderDto cuStocks;
                                //using (var connection = new SqlConnection(connectionString))
                                //{
                                //    cuStocks = connection.Query<StockHeaderDto>("SELECT * FROM dbo.StockHeader  WHERE CPCId = @Id", new { Id = a.CPCItemId }).FirstOrDefault();
                                //}

                                CorporateProductCatalogDto cpc;

                                var idGenerator1 = new IdGenerator();
                                StockSequenceId =
                                    idGenerator1.GenerateId(applicationDbContext, "Stock-", "StockSequence");

                                var projectStockInsert =
                                    @"INSERT INTO [dbo].[StockHeader]([Id],[SequenceId],[Name],[Title],[CPCId],[AvailableQuantity],[MOUId],[AveragePrice],[CreatedBy],[CreatedDate],[TypeId])VALUES(@Id,@SequenceId,@Name,@Title,@CPCId,@AvailableQuantity,@MOUId,@AveragePrice,@CreatedBy,@CreatedDate,@TypeId)";
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    cpc = connection
                                        .Query<CorporateProductCatalogDto>(
                                            "SELECT * FROM dbo.CorporateProductCatalog  WHERE Id = @Id",
                                            new { Id = a.CPCItemId }).FirstOrDefault();
                                }

                                var parameters1 = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SequenceId = StockSequenceId,
                                    Name = cpc.ResourceTitle,
                                    Title = StockSequenceId + " " + cpc.ResourceTitle,
                                    CPCId = a.CPCItemId,
                                    AvailableQuantity = "0",
                                    a.MOUId,
                                    AveragePrice = cpc.InventoryPrice.ToString(),
                                    //WareHouseTaxonomyId = StockParameter.StockDto.WareHouseTaxonomyId,
                                    CreatedBy = "",
                                    CreatedDate = DateTime.UtcNow,
                                    TypeId = wfData.ResourceType
                                };

                                using (var connection = new SqlConnection(connectionprojectString))
                                {
                                    await connection.ExecuteAsync(projectStockInsert, parameters1);
                                }


                                var taskparameters = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    WorkFlowId = Id,
                                    // Source = a.Source,
                                    a.CPCItemId,
                                    a.Quantity,
                                    a.MOUId,
                                    a.PickedQuantity,
                                    a.UnitPrice
                                    //Destination = a.Destination,
                                    //StockAvailability = projectStocks.AvailableQuantity
                                };


                                using (var connection = new SqlConnection(connectionprojectString))
                                {
                                    await connection.ExecuteAsync(taskquery, taskparameters);
                                }
                            }
                        }

                        if (documentdata != null)
                        {
                            var documentquery =
                                @"INSERT INTO dbo.WFDocument (Id,Link,WFHeaderId) VALUES (@Id,@Link,@WFHeaderId)";

                            foreach (var mdoc in documentdata)
                                using (var connection = new SqlConnection(connectionprojectString))
                                {
                                    await connection.ExecuteAsync(documentquery,
                                        new { Id = Guid.NewGuid().ToString(), mdoc.Link, WFHeaderId = Id });
                                }
                        }
                    }

                    if (taskData != null)
                    {
                        IEnumerable<StockHeader> stocks;
                        StockHeader stock = null;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            stocks = connection.Query<StockHeader>("SELECT * FROM dbo.StockHeader  WHERE CPCId IN @Ids",
                                new { Ids = taskData.Select(t => t.CPCItemId).ToList() });
                        }

                        CorporateProductCatalogDto cpc;
                        foreach (var wf in taskData)
                            if (wf.CPCItemId != null)
                            {
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    cpc = connection
                                        .Query<CorporateProductCatalogDto>(
                                            "SELECT * FROM dbo.CorporateProductCatalog  WHERE Id = @Id",
                                            new { Id = wf.CPCItemId }).FirstOrDefault();
                                }

                                var selectStock = stocks.Where(s =>
                                        s.CPCId == wf.CPCItemId &&
                                        s.ActiveTypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1")
                                    .FirstOrDefault();
                                if (selectStock != null)
                                    stock = selectStock;
                                else
                                    stock = stocks.Where(s => s.CPCId == wf.CPCItemId).FirstOrDefault();


                                if (stock != null)
                                {
                                    string typeId = null;
                                    string unitPrice = null;
                                    var stockHistoryInsert =
                                        @"INSERT INTO dbo.StockHistoryLog (Id,WorkFlowId,DateTime,ActivityTypeId,Quantity,MOUId,WareHouseWorker,StockHeaderId,Price) VALUES (@Id,@WorkFlowId,@DateTime,@ActivityTypeId,@Quantity,@MOUId,@WareHouseWorker,@StockHeaderId,@Price)";

                                    if (wfData.TypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1") //good picking
                                    {
                                        typeId = "7bcb4e8d-8e8c-outs-8170-6b91c89fc3da"; // out to the stock
                                        unitPrice = cpc.InventoryPrice.ToString();
                                    }
                                    else if (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb") //good reception)
                                    {
                                        typeId = "7bcb4e8d-8e8c-inst-8170-6b91c89fc3da"; // in to the stock 
                                        unitPrice = cpc.InventoryPrice.ToString();
                                    }

                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        await connection.ExecuteAsync(stockHistoryInsert, new
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            WorkFlowId = wfData.Id,
                                            DateTime = DateTime.UtcNow,
                                            ActivityTypeId = typeId,
                                            Quantity = wf.PickedQuantity,
                                            wf.MOUId,
                                            WareHouseWorker = wfData.ExecutorId,
                                            StockHeaderId = stock.Id,
                                            Price = unitPrice
                                        });
                                    }

                                    var stockUpdate =
                                        @"UPDATE dbo.StockHeader SET  AvailableQuantity = @AvailableQuantity WHERE Id =@Id";

                                    if (wfData.TypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1") //good picking
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(stockUpdate, new
                                            {
                                                stock.Id,
                                                AvailableQuantity =
                                                    (float.Parse(stock.AvailableQuantity) -
                                                     float.Parse(wf.PickedQuantity)).ToString()
                                            });
                                        }
                                    else if (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb") //good reception
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(stockUpdate, new
                                            {
                                                stock.Id,
                                                AvailableQuantity =
                                                    (float.Parse(stock.AvailableQuantity) +
                                                     float.Parse(wf.PickedQuantity)).ToString()
                                            });
                                        }
                                }
                            }
                    }

                    var connectionProjectString =
                        ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId, wfData.Project,
                            WFParameter.TenantProvider);

                    if (wfData.BorId != null)
                    {
                        var insterTPmol =
                            @"INSERT INTO dbo.PMol ( Id ,ProjectMoleculeId,Name ,IsDeleted ,TypeId ,StatusId ,Title ,BorId ,LocationId ,IsFinished  ) VALUES ( @Id ,@ProjectMoleculeId ,@Name ,0 ,@TypeId ,@StatusId ,@Title ,@BorId ,@LocationId ,0 );";

                        ProjectDefinition projectDefinition;
                        using (var connection =
                               new SqlConnection(WFParameter.TenantProvider.GetTenant().ConnectionString))
                        {
                            projectDefinition = connection
                                .Query<ProjectDefinition>(
                                    "SELECT * FROM dbo.ProjectDefinition WHERE SequenceCode = @SequenceCode",
                                    new { SequenceCode = wfData.Project }).FirstOrDefault();
                        }

                        using (var connection = new SqlConnection(connectionProjectString))
                        {
                            var idGenerator = new IdGenerator();
                            var ProjectMoleculeId1 =
                                idGenerator.GenerateId(applicationDbContext, "PMOL-", "PmolSequenceCode");
                            var ProjectMoleculeId2 =
                                idGenerator.GenerateId(applicationDbContext, "PMOL-", "PmolSequenceCode");
                            var ProjectMoleculeId3 =
                                idGenerator.GenerateId(applicationDbContext, "PMOL-", "PmolSequenceCode");

                            var Id = Guid.NewGuid().ToString();
                            var Id2 = Guid.NewGuid().ToString();
                            var IdTravel = Guid.NewGuid().ToString();
                            var bor = connection
                                .Query<Bor>("SELECT * FROM dbo.Bor WHERE Id = @Id", new { Id = wfData.BorId })
                                .FirstOrDefault();
                            var paramPmol = new
                            {
                                Id,
                                ProjectMoleculeId = ProjectMoleculeId1,
                                BorId = bor.Id,
                                TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
                                StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                Name = wfData.Title,
                                Title = ProjectMoleculeId1 + " - " + wfData.Title + wfData.RequiredDateAndTime,
                                projectDefinition.LocationId
                            };

                            var paramUnload = new
                            {
                                Id = Id2,
                                ProjectMoleculeId = ProjectMoleculeId2,
                                BorId = bor.Id,
                                TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
                                StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                Name = wfData.Title,
                                Title = ProjectMoleculeId2 + " - " + wfData.Title + wfData.RequiredDateAndTime,
                                projectDefinition
                                    .LocationId
                            };

                            var paramPmolTravel = new
                            {
                                Id = IdTravel,
                                ProjectMoleculeId = ProjectMoleculeId3,
                                BorId = bor.Id,
                                TypeId = "3f8ce-f268-4ce3-9f12-fa6b3adad2cf9d1",
                                StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                Name = wfData.Title,
                                Title = ProjectMoleculeId3 + " - " + wfData.Title + wfData.RequiredDateAndTime,
                                projectDefinition
                                    .LocationId
                            };
                            await connection.ExecuteAsync(insterTPmol, paramPmol);
                            await connection.ExecuteAsync(insterTPmol, paramUnload);
                            await connection.ExecuteAsync(insterTPmol, paramPmolTravel);
                            if (taskData != null)
                            {
                                if (wfData.ResourceType == "c46c3a26-39a5-42cc-n7k1-89655304eh6")
                                {
                                    //sb.Append("  AND POHeader.NoOfMaterials > 0 ");
                                    var pmolResource =
                                        @"INSERT INTO dbo.PMolPlannedWorkMaterial ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity ) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0, 0.0)";

                                    foreach (var wf in taskData)
                                        if (wf.CPCItemId != null)
                                        {
                                            var paramR = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR);

                                            var paramR2 = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id2, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR2);
                                        }
                                }

                                if (wfData.ResourceType == "c46c3a26-39a5-42cc-m06g-89655304eh6")
                                {
                                    // sb.Append("  AND POHeader.NoOfConsumables > 0 ");
                                    var pmolResource =
                                        @"INSERT INTO dbo.PMolPlannedWorkConsumable ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0, 0.0 )";

                                    foreach (var wf in taskData)
                                        if (wf.CPCItemId != null)
                                        {
                                            var paramR = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR);

                                            var paramR2 = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id2, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR2);
                                        }
                                }

                                if (wfData.ResourceType == "c46c3a26-39a5-42cc-b07s-89655304eh6")
                                {
                                    // sb.Append("  AND POHeader.NoOfLabours > 0 ");

                                    var pmolResource =
                                        @"INSERT INTO dbo.PMolPlannedWorkLabour ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity ) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0, 0.0 )";

                                    foreach (var wf in taskData)
                                        if (wf.CPCItemId != null)
                                        {
                                            var paramR = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR);

                                            var paramR2 = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id2, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR2);
                                        }
                                }

                                if (wfData.ResourceType == "c46c3a26-39a5-42cc-n9wn-89655304eh6")
                                {
                                    var pmolResource =
                                        @"INSERT INTO dbo.PMolPlannedWorkTools ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0,0.0 )";

                                    foreach (var wf in taskData)
                                        if (wf.CPCItemId != null)
                                        {
                                            var paramR = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR);

                                            var paramR2 = new
                                            {
                                                CoperateProductCatalogId = wf.CPCItemId, RequiredQuantity = wf.Quantity,
                                                PmolId = Id2, Type = "Planned", CpcBasicUnitofMeasureId = wf.MOUId
                                            };
                                            await connection.ExecuteAsync(pmolResource, paramR2);
                                        }


                                    // sb.Append("  AND POHeader.NoOfTools > 0 ");
                                }
                            }
                        }
                    }
                }
            }

            return WFSequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> ProjectApprove(WFParameter WFParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, WFParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId,
            WFParameter.ProjectSequenceId, WFParameter.TenantProvider);
        //string connectionprojectString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId, null, WFParameter.TenantProvider);
        string WFSequenceId = null;

        IEnumerable<WFTasksDto> taskData = null;
        IEnumerable<WFDocumentDto> documentdata;

        try
        {
            var approvequery =
                @"UPDATE dbo.WFHeader SET StatusId = '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da' WHERE SequenceId = @SequenceId";
            var query = @"SELECT * FROM dbo.WFHeader where SequenceId = @SequenceId";
            var parameter = new { SequenceId = WFParameter.Id };
            WFHeader wfData;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(approvequery, parameter);
                wfData = connection.Query<WFHeader>(query, parameter).FirstOrDefault();
            }

            if (wfData != null)
            {
                var taskselect = @"SELECT * FROM [dbo].[WFTask] WHERE WorkFlowId = @WorkFlowId ";
                var documentselect = @"SELECT Link FROM dbo.WFDocument WHERE WFHeaderId = @WorkFlowId";

                var parm = new { WorkFlowId = wfData.Id };


                using (var connection = new SqlConnection(connectionString))
                {
                    taskData = connection.Query<WFTasksDto>(taskselect, parm);
                    documentdata = connection.Query<WFDocumentDto>(documentselect, parm);
                }
            }

            if (taskData != null)
            {
                IEnumerable<StockHeader> stocks;
                using (var connection = new SqlConnection(connectionString))
                {
                    stocks = connection.Query<StockHeader>("SELECT * FROM dbo.StockHeader  WHERE CPCId IN @Ids",
                        new { Ids = taskData.Select(t => t.CPCItemId).ToList() });
                }

                CorporateProductCatalogDto cpc;
                foreach (var wf in taskData)
                {
                    if (wf.CPCItemId != null)
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            cpc = connection
                                .Query<CorporateProductCatalogDto>(
                                    "SELECT * FROM dbo.CorporateProductCatalog  WHERE Id = @Id",
                                    new { Id = wf.CPCItemId }).FirstOrDefault();
                        }

                        var stock = stocks.Where(s => s.CPCId == wf.CPCItemId).FirstOrDefault();

                        if (stock != null)
                        {
                            string typeId = null;
                            string unitPrice = null;

                            var stockHistoryInsert =
                                @"INSERT INTO dbo.StockHistoryLog (Id,WorkFlowId,DateTime,ActivityTypeId,Quantity,MOUId,WareHouseWorker,StockHeaderId,Price) VALUES (@Id,@WorkFlowId,@DateTime,@ActivityTypeId,@Quantity,@MOUId,@WareHouseWorker,@StockHeaderId,@Price)";

                            if (wfData.TypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1") //good picking
                            {
                                typeId = "7bcb4e8d-8e8c-outs-8170-6b91c89fc3da"; // out to the stock
                                unitPrice = cpc.InventoryPrice.ToString();
                            }
                            else if (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb") //good reception)
                            {
                                typeId = "7bcb4e8d-8e8c-inst-8170-6b91c89fc3da"; // in to the stock 
                                unitPrice = cpc.InventoryPrice.ToString();
                            }

                            using (var connection = new SqlConnection(connectionString))
                            {
                                await connection.ExecuteAsync(stockHistoryInsert, new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    WorkFlowId = wfData.Id,
                                    DateTime = DateTime.UtcNow,
                                    ActivityTypeId = typeId,
                                    Quantity = wf.PickedQuantity,
                                    wf.MOUId,
                                    WareHouseWorker = wfData.ExecutorId,
                                    StockHeaderId = stock.Id,
                                    Price = unitPrice
                                });
                            }

                            var stockUpdate =
                                @"UPDATE dbo.StockHeader SET  AvailableQuantity = @AvailableQuantity WHERE Id =@Id";

                            if (wfData.TypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1") //good picking
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(stockUpdate, new
                                    {
                                        stock.Id,
                                        AvailableQuantity =
                                            (float.Parse(stock.AvailableQuantity) - float.Parse(wf.PickedQuantity))
                                            .ToString()
                                    });
                                }
                            else if (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb") //good reception
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(stockUpdate, new
                                    {
                                        stock.Id,
                                        AvailableQuantity =
                                            (float.Parse(stock.AvailableQuantity) + float.Parse(wf.PickedQuantity))
                                            .ToString()
                                    });
                                }
                        }
                    }

                    if (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb" &&
                        wfData.ResourceType == "c46c3a26-39a5-42cc-n7k1-89655304eh6") //materials
                    {
                        var BorResourceUpdate =
                            "UPDATE dbo.BorMaterial SET  Warf = @Warf, ActualDeliveryDate = @ActualDeliveryDate WHERE BorProductId = @BorId AND CorporateProductCatalogId = @CorporateProductCatalogId";

                        var parameters = new
                        {
                            Warf = wf.PickedQuantity,
                            wfData.BorId,
                            CorporateProductCatalogId = wf.CPCItemId,
                            ActualDeliveryDate = DateTime.UtcNow
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(BorResourceUpdate, parameters);
                        }
                    }

                    if (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb" &&
                        wfData.ResourceType == "c46c3a26-39a5-42cc-n9wn-89655304eh6") //tools
                    {
                        var BorResourceUpdate =
                            "UPDATE dbo.BorTools SET  Warf = @Warf, ActualDeliveryDate = @ActualDeliveryDate WHERE BorProductId = @BorId AND CorporateProductCatalogId = @CorporateProductCatalogId";

                        var parameters = new
                        {
                            Warf = wf.PickedQuantity,
                            wfData.BorId,
                            CorporateProductCatalogId = wf.CPCItemId,
                            ActualDeliveryDate = DateTime.UtcNow
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(BorResourceUpdate, parameters);
                        }
                    }

                    if (wfData.TypeId == "4010e768-3e06-4702-b337-ee367a82addb" &&
                        wfData.ResourceType == "c46c3a26-39a5-42cc-m06g-89655304eh6") //consumable
                    {
                        var BorResourceUpdate =
                            "UPDATE dbo.BorConsumable SET  Warf = @Warf, ActualDeliveryDate = @ActualDeliveryDate WHERE BorProductId = @BorId AND CorporateProductCatalogId = @CorporateProductCatalogId";

                        var parameters = new
                        {
                            Warf = wf.PickedQuantity,
                            wfData.BorId,
                            CorporateProductCatalogId = wf.CPCItemId,
                            ActualDeliveryDate = DateTime.UtcNow
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(BorResourceUpdate, parameters);
                        }
                    }
                }
            }


            return WFSequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> WFHistoryLog(WFParameter wfParameter, string connectionString, string action,
        string historyLog, string PoId, string WfId)
    {
        try
        {
            await using var connection = new SqlConnection(connectionString);

            var sql =
                @"INSERT INTO dbo.WFHistoryLogFroPO ( Id ,HistoryLog ,ChangedByUserId ,Action ,ChangedTime,PoId,WfId) VALUES ( @Id ,@HistoryLog ,@ChangedByUserId ,@Action ,@ChangedTime,@PoId,@WfId);";

            var param = new
            {
                Id = Guid.NewGuid().ToString(),
                HistoryLog = historyLog,
                Action = action,
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = wfParameter.UserId,
                PoId,
                WfId
            };

            await connection.ExecuteAsync(sql, param);

            return "ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public double FormatOffset(TimeZone timeZone)
    {
        var offsetwithdot = timeZone.offset.Insert(timeZone.offset.Length - 2, ".");


        var minutes1 = offsetwithdot.Substring(4, 2);
        var minutes2 = Convert.ToDouble(minutes1) / 60;
        var min = minutes2.ToString(CultureInfo.InvariantCulture);
        var aStringBuilder = new StringBuilder(offsetwithdot);
        aStringBuilder.Remove(4, 2);
        var theString = aStringBuilder.ToString();

        string finalStringOffset;
        if (min == "0")
        {
            finalStringOffset = theString + min;
        }
        else
        {
            var aStringBuilder2 = new StringBuilder(min);
            aStringBuilder2.Remove(0, 2);
            var theString2 = aStringBuilder2.ToString();
            finalStringOffset = theString + theString2;
        }

        var finalOffset = Convert.ToDouble(finalStringOffset);
        return finalOffset;
    }

    public DateTime FindGmtDatetime(WFParameter wFParameter)
    {
        var timeZone = new TimeZone();
        timeZone.offset = wFParameter.Filter.Offset;
        if (wFParameter.Filter.Date == null)
        {
            timeZone.date = (DateTime)wFParameter.Filter.DateTime;
        }
        else
        {
            var days = Convert.ToDouble(wFParameter.Filter.Date);
            var d = wFParameter.Filter.LocalDate;
            timeZone.date = d.AddDays(days);
        }

        var finalOffset = FormatOffset(timeZone);
        var date = timeZone.date - timeZone.date.TimeOfDay;
        if (finalOffset > 0)
            date = date.AddHours(finalOffset * -1);
        else if (finalOffset < 0) date = date.AddHours(finalOffset);

        return date;
    }

    public async Task<string> CreateWorkFlowForPoFromCu(WFParameter WFParameter, WFHeader wfData)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, WFParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId, null,
            WFParameter.TenantProvider);
        //string connectionprojectString = ConnectionString.MapConnectionString(WFParameter.ContractingUnitSequenceId, null, WFParameter.TenantProvider);
        string WFSequenceId = null;
        string date = null;
        IEnumerable<WFTasksDto> taskData = null;
        IEnumerable<WFDocumentDto> documentdata;
        var idGenerator = new IdGenerator();
        WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");
        var Id = Guid.NewGuid().ToString();

        var createquery = @"INSERT INTO dbo.WFHeader
                                        (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Destination,IsFinish,Source,StatusId,BorId,Project,RequiredDateAndTime,PoId,IsFromCU)
                                         VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Destination,@IsFinish,@Source,@Status,@BorId,@Project,@RequiredDateAndTime,@PoId,@IsFromCU)";
        var parameters = new
        {
            Id,
            SequenceId = WFSequenceId,
            wfData.Name,
            Title = WFSequenceId + " " + wfData.Name,
            TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
            wfData.ResourceType,
            CreatedBy = WFParameter.UserId,
            CreatedDate = DateTime.UtcNow,
            ModifiedBy = WFParameter.UserId,
            ModifiedDate = DateTime.UtcNow,
            wfData.Source,
            wfData.Destination,
            IsFinish = false,
            wfData.Project,
            RequiredDateAndTime = DateTime.UtcNow,
            ExecutedDateAndTime = date,
            Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
            BorID = wfData.BorId,
            wfData.PoId,
            wfData.IsFromCU
        };


        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(createquery, parameters);
        }

        var jsonProduct = JsonConvert.SerializeObject(parameters, Formatting.Indented,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

        await WFHistoryLog(WFParameter, connectionString, "WFCuApprove(IsCu)", jsonProduct, wfData.Id, WFSequenceId);

        var taskselect = @"SELECT * FROM [dbo].[WFTask] WHERE WorkFlowId = @WorkFlowId ";
        var documentselect = @"SELECT Link FROM dbo.WFDocument WHERE WFHeaderId = @WorkFlowId";

        var parm = new { WorkFlowId = wfData.Id };


        await using (var connection = new SqlConnection(connectionString))
        {
            taskData = connection.Query<WFTasksDto>(taskselect, parm);
            documentdata = connection.Query<WFDocumentDto>(documentselect, parm);
        }

        var taskquery =
            @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,UnitPrice) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@MOUId,@PickedQuantity,@UnitPrice)";

        foreach (var a in taskData)
        {
            var taskparameters = new
            {
                Id = Guid.NewGuid().ToString(),
                WorkFlowId = Id,
                a.CPCItemId,
                a.Quantity,
                a.MOUId,
                a.PickedQuantity,
                a.UnitPrice
            };

            using (var connection = new SqlConnection(connectionString))
            {
                var projectStocks = connection
                    .Query<StockHeaderDto>("SELECT * FROM dbo.StockHeader  WHERE CPCId = @Id",
                        new { Id = a.CPCItemId }).FirstOrDefault();

                var pickedQuantity = a.PickedQuantity.ToFloat() + projectStocks.AvailableQuantity.ToFloat();

                await connection.ExecuteAsync(
                    "UPDATE dbo.StockHeader SET AvailableQuantity = @pickedQuantity WHERE Id = @Id",
                    new { pickedQuantity, projectStocks.Id });
            }

            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(taskquery, taskparameters);
            }
        }

        if (documentdata != null)
        {
            var documentquery =
                @"INSERT INTO dbo.WFDocument (Id,Link,WFHeaderId) VALUES (@Id,@Link,@WFHeaderId)";

            foreach (var mdoc in documentdata)
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(documentquery,
                        new { Id = Guid.NewGuid().ToString(), mdoc.Link, WFHeaderId = Id });
                }
        }

        return WFSequenceId;
    }
}