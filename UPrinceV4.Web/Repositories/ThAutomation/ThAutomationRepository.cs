using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AzureMapsToolkit;
using AzureMapsToolkit.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.ThAutomation;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.CPC;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.ThAutomation;
using UPrinceV4.Web.Util;
using Product = UPrinceV4.Web.Data.ThAutomation.Product;

namespace UPrinceV4.Web.Repositories.ThAutomation;

public class ThAutomationRepository : IThAutomationRepository
{
    public async Task<List<GetThProductWithTrucks>> GetProductsWithTrucks(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        List<GetThProductWithTrucks> result;
        result = connection.Query<GetThProductWithTrucks>(
            "Select * from PbsProduct Where StartDate = @date AND ParentId IS NULL ",
            new { date = ThAutomationParameter.ThProductWithTrucksDto.Date }).ToList();

        foreach (var item in result)
        {
            var trucks = new List<GetThProductWithTrucksDto>();

            var obj = new GetThProductWithTrucksDto
            {
                Id = "22222",
                Date = DateTime.UtcNow,
                ProductId = item.Id
            };
            trucks.Add(obj);

            item.PbsDynamicAttributes = connection
                .Query<PbsDynamicAttributesDto>(
                    "Select * from PbsDynamicAttributes Where ProductId = @ProductId ORDER BY [Key] DESC",
                    new { ProductId = item.Id }).AsEnumerable().ToList();
            var truckList =
                connection.Query<GetThProductWithTrucksDto>(
                    "Select ThProductWithTrucks.*,CorporateProductCatalog.ResourceTitle As Title from ThProductWithTrucks left outer join CorporateProductCatalog on ThProductWithTrucks.CpcId = CorporateProductCatalog.Id Where ProductId = @ProductId AND Date = @date",
                    new { ProductId = item.Id, date = ThAutomationParameter.ThProductWithTrucksDto.Date }).ToList();

            foreach (var tr in truckList)
            {
                var trOrder = connection
                    .Query<string>("Select TruckOrder From ThTrucksSchedule Where ProductTruckId = @ProductTruckId",
                        new { ProductTruckId = tr.Id }).FirstOrDefault();

                if (trOrder != null) tr.TruckOrder = trOrder.ToInt();
            }

            trucks.AddRange(truckList);

            item.Trucks = trucks.OrderBy(x => x.TruckOrder).ToList();
        }


        return result;
    }

    public async Task<List<GetThTrucksSchedule>> GetTruckAssignData(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        List<GetThTrucksSchedule> result;
        var mixerResult = new List<GetThTrucksSchedule>();

        var pbsDynamicAttributes = connection
            .Query<GetPbsDynamicAttributes>(
                "Select PbsDynamicAttributes.*,PbsProduct.StartDate from PbsDynamicAttributes Left outer join PbsProduct on PbsDynamicAttributes.ProductId = PbsProduct.Id  Where PbsDynamicAttributes.ProductId = @ProductId",
                new { ProductId = ThAutomationParameter.TruckAssignDto.Id }).ToList();

        var projectStartTime = DateTime.Parse(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").StartDate
            .ToShortDateString());
        var projectEndTime = DateTime.Parse(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").StartDate
            .ToShortDateString());
        var projectSTime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectStartTime = projectStartTime.AddHours(projectSTime.Hour)
            .AddMinutes(projectSTime.Minute);
        var projectETime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "EndTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectEndTime = projectEndTime.AddHours(projectETime.Hour).AddMinutes(projectSTime.Minute);


        result = connection.Query<GetThTrucksSchedule>(
            @"SELECT ThTrucksSchedule.*,ThProductWithTrucks.CpcId, cpc.ResourceTitle AS Truck,cpc.Size FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id
        LEFT OUTER JOIN CorporateProductCatalog cpc ON ThProductWithTrucks.CpcId = cpc.Id WHERE ThProductWithTrucks.ProductId = @Id",
            new { ThAutomationParameter.TruckAssignDto.Id }).ToList();

        // foreach (var item in result)
        // {
        //     item.STime = item.StartTime?.ToShortTimeString();
        //     item.ETime = item.EndTime?.ToShortTimeString();
        //
        //     if (item.Type == "2")
        //     {
        //         var mixer = new GetThTrucksSchedule()
        //         {
        //             Id = Guid.NewGuid().ToString(),
        //             ProductTruckId = "22222",
        //             Title = "Loading",
        //             StartTime = item.StartTime,
        //             EndTime = item.EndTime,
        //             STime = item.STime,
        //             ETime = item.ETime,
        //             Type = "12"
        //         };
        //         mixerResult.Add(mixer);
        //     }
        //     
        // }

        if (!ThAutomationParameter.TruckAssignDto.IsPmol)
        {
            var pbsTruckData = result.GroupBy(x => x.ProductTruckId);

            foreach (var pbsTrucks in pbsTruckData)
            foreach (var turn in pbsTrucks.GroupBy(c => c.TurnNumber))
            {
                var truckAssign = new GetThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = pbsTrucks.Key,
                    Title = "Turn",
                    StartTime = turn.FirstOrDefault(x => x.Type == "0")?.StartTime,
                    EndTime = turn.FirstOrDefault(x => x.Type == "3")?.EndTime,
                    STime = turn.FirstOrDefault(x => x.Type == "0")?.StartTime?.ToShortTimeString(),
                    ETime = turn.FirstOrDefault(x => x.Type == "3")?.EndTime?.ToShortTimeString(),
                    Type = "10",
                    TurnNumber = turn.Key,
                    CpcId = turn.FirstOrDefault()?.CpcId,
                    Truck = turn.FirstOrDefault()?.Truck,
                    Size = turn.FirstOrDefault()?.Size,
                    LoadingNumber = turn.FirstOrDefault()?.LoadingNumber,
                    TruckOrder = turn.FirstOrDefault()?.TruckOrder
                };
                mixerResult.Add(truckAssign);
            }

            var freeTimes = result.Where(c => c.Type == "2").OrderBy(x => x.StartTime).ToList();
            var totalFreeTimes = new List<GetThTrucksSchedule>();

            if (freeTimes.Any())
            {
                var currentTime = projectStartTime;

                foreach (var times in freeTimes)
                {
                    times.STime = times.StartTime?.ToShortTimeString();
                    times.ETime = times.EndTime?.ToShortTimeString();
                    times.Type = "12";
                    times.ProductTruckId = "22222";

                    if (currentTime < times.StartTime)
                    {
                        var tt = new GetThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = "22222",
                            Title = "Not Unloading",
                            StartTime = currentTime,
                            EndTime = times.StartTime,
                            STime = currentTime.ToShortTimeString(),
                            ETime = times.StartTime?.ToShortTimeString(),
                            Type = "14"
                        };
                        totalFreeTimes.Add(tt);
                    }

                    currentTime = times.EndTime.Value;
                }

                if (currentTime < projectEndTime)
                {
                    var tt = new GetThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = "22222",
                        Title = "Not Unloading",
                        StartTime = currentTime,
                        EndTime = projectEndTime,
                        STime = currentTime.ToShortTimeString(),
                        ETime = projectEndTime.ToShortTimeString(),
                        Type = "14"
                    };
                    totalFreeTimes.Add(tt);
                }
            }
            else
            {
                var tt = new GetThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = "22222",
                    Title = "Not Unloading",
                    StartTime = projectStartTime,
                    EndTime = projectEndTime,
                    STime = projectStartTime.ToShortTimeString(),
                    ETime = projectEndTime.ToShortTimeString(),
                    Type = "14"
                };
                totalFreeTimes.Add(tt);
            }

            mixerResult.AddRange(freeTimes);

            mixerResult.AddRange(totalFreeTimes);

            var mixerTotal = new GetThTrucksSchedule
            {
                Id = Guid.NewGuid().ToString(),
                ProductTruckId = "22222",
                Title = "Total",
                StartTime = projectStartTime.AddMinutes(-60),
                EndTime = projectStartTime.AddMinutes(-10),
                STime = projectStartTime.AddMinutes(-60).ToShortTimeString(),
                ETime = projectStartTime.AddMinutes(-10).ToShortTimeString(),
                Type = "13",
                Size = freeTimes.Sum(v => v.Size),
                Capacity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Capacity").Value
            };
            mixerResult.Add(mixerTotal);

            //result.AddRange(mixerResult);
        }
        else
        {
            foreach (var item in result)
            {
                item.STime = item.StartTime?.ToShortTimeString();
                item.ETime = item.EndTime?.ToShortTimeString();

                if (item.Type == "2")
                {
                    var mixer = new GetThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = "22222",
                        Title = "Unloading",
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        STime = item.StartTime?.ToShortTimeString(),
                        ETime = item.EndTime?.ToShortTimeString(),
                        Type = "12"
                    };
                    mixerResult.Add(mixer);
                }
            }

            var freeTimes = mixerResult.OrderBy(x => x.StartTime).ToList();
            var totalFreeTimes = new List<GetThTrucksSchedule>();

            if (freeTimes.Any())
            {
                var currentTime = projectStartTime;

                foreach (var times in freeTimes)
                {
                    if (currentTime < times.StartTime)
                    {
                        var tt = new GetThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = "22222",
                            Title = "Not Unloading",
                            StartTime = currentTime,
                            EndTime = times.StartTime,
                            STime = currentTime.ToShortTimeString(),
                            ETime = times.StartTime?.ToShortTimeString(),
                            Type = "14"
                        };
                        totalFreeTimes.Add(tt);
                    }

                    currentTime = times.EndTime.Value;
                }

                if (currentTime < projectEndTime)
                {
                    var tt = new GetThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = "22222",
                        Title = "Not Unloading",
                        StartTime = currentTime,
                        EndTime = projectEndTime,
                        STime = currentTime.ToShortTimeString(),
                        ETime = projectEndTime.ToShortTimeString(),
                        Type = "14"
                    };
                    totalFreeTimes.Add(tt);
                }
            }
            else
            {
                var tt = new GetThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = "22222",
                    Title = "Not Unloading",
                    StartTime = projectStartTime,
                    EndTime = projectEndTime,
                    STime = projectStartTime.ToShortTimeString(),
                    ETime = projectEndTime.ToShortTimeString(),
                    Type = "14"
                };
                totalFreeTimes.Add(tt);
            }

            mixerResult.AddRange(totalFreeTimes);

            mixerResult.AddRange(result);

            var mixerTotal = new GetThTrucksSchedule
            {
                Id = Guid.NewGuid().ToString(),
                ProductTruckId = "22222",
                Title = "Total",
                StartTime = projectStartTime.AddMinutes(-60),
                EndTime = projectStartTime.AddMinutes(-10),
                STime = projectStartTime.AddMinutes(-60).ToShortTimeString(),
                ETime = projectStartTime.AddMinutes(-10).ToShortTimeString(),
                Type = "13",
                Size = result.Where(x => x.Type == "2").Sum(v => v.Size),
                Capacity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Capacity")?.Value
            };
            mixerResult.Add(mixerTotal);
        }

        return mixerResult.OrderBy(x => x.TruckOrder).ToList();
    }

    public async Task<List<ThTruckWithProductData>> GetTruckWithProduct(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var ThTruckWithProductData = new List<ThTruckWithProductData>();

        var cpc = connection
            .Query<ThTruckWithProductData>(
                "SELECT Id,Title AS TruckTitle,Size AS TruckCapacity FROM dbo.CorporateProductCatalog WHERE ResourceFamilyId = '2210e768-human-kknk-truck-ee367a82ad17' ORDER BY TruckTitle ")
            .ToList();

        var thProductWithTrucks = connection
            .Query<ThProductWithTrucks>("SELECT * FROM ThProductWithTrucks WHERE Date = @Date",
                new { ThAutomationParameter.ThTruckWithProductDto.Date }).ToList();

        var product = connection.Query<Product>("SELECT Id,Title AS ProductTitle FROM dbo.PbsProduct").ToList();

        var thTrucksSchedule = connection.Query<ThTrucksSchedule>("SELECT * FROM dbo.ThTrucksSchedule").ToList();

        foreach (var i in cpc)
        {
            var productForTruck = thProductWithTrucks.Where(e => e.CpcId == i.Id).ToList();

            var mProduct = new List<Product>();

            foreach (var n in productForTruck)
            {
                var mthTrucksSchedule = thTrucksSchedule.Where(e => e.ProductTruckId == n.Id).ToList();

                var nxProduct = product.FirstOrDefault(e => e.Id == n.ProductId);

                foreach (var r in mthTrucksSchedule.GroupBy(c => c.TurnNumber))
                {
                    var nProduct = new Product();
                    nProduct.Id = nxProduct.Id;
                    nProduct.ProductTitle = nxProduct.ProductTitle;
                    nProduct.StartTime = r.FirstOrDefault(x => x.Type == "0")?.StartTime;
                    nProduct.EndTime = r.FirstOrDefault(x => x.Type == "3")?.EndTime;

                    mProduct.Add(nProduct);
                }
            }

            i.Product = mProduct;
            ThTruckWithProductData.Add(i);
        }

        return ThTruckWithProductData;
    }


    public async Task<GetThProductWithTrucksDto> AddTrucksToProduct(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        await using var dbconnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, ThAutomationParameter.TenantProvider);
        var idGenerator = new IdGenerator();

        var pbsDynamicAttributes = connection
            .Query<PbsDynamicAttributes>("Select * from PbsDynamicAttributes Where ProductId = @ProductId",
                new { ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).ToList();

        var projectStartTime = DateTime.Parse(ThAutomationParameter.GetThProductWithTrucksDto.Date.ToShortDateString());
        var projectEndTime = DateTime.Parse(ThAutomationParameter.GetThProductWithTrucksDto.Date.ToShortDateString());
        var projectSTime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectStartTime = projectStartTime.AddHours(projectSTime.Hour)
            .AddMinutes(projectSTime.Minute);
        var projectETime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "EndTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectEndTime = projectEndTime.AddHours(projectETime.Hour).AddMinutes(projectSTime.Minute);
        var capacity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Capacity")?.Value;
        var velocity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Velocity")?.Value;


        var isExist = connection
            .Query<ThProductWithTrucks>(
                "Select * From ThProductWithTrucks Where CpcId = @CpcId AND ProductId = @ProductId",
                ThAutomationParameter.GetThProductWithTrucksDto);


        if (!isExist.Any())
        {
            var insertQuery =
                @"INSERT INTO dbo.ThProductWithTrucks (Id, CpcId, ProductId, Date) VALUES (@Id, @CpcId, @ProductId, @Date)";

            var query =
                @"INSERT INTO dbo.ThTrucksSchedule ( Id ,ProductTruckId ,Title ,EndTime ,StartTime ,Type,TurnNumber,LoadingNumber,TruckOrder ) VALUES ( @Id ,@ProductTruckId ,@Title ,@EndTime ,@StartTime ,@Type,@TurnNumber,@LoadingNumber,@TruckOrder )";

            await connection.ExecuteAsync(insertQuery, ThAutomationParameter.GetThProductWithTrucksDto);


            var truckResult = connection.Query<GetThTrucksSchedule>(
                @"SELECT * FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id WHERE ThProductWithTrucks.ProductId = @Id",
                new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).ToList();

            var cpcTitle = connection
                .Query<string>("Select Title From CorporateProductCatalog Where Id = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

            if (ThAutomationParameter.GetThProductWithTrucksDto.Type == "14")
            {
                var truckSize1 = connection.Query<float>("Select Size From CorporateProductCatalog Where Id = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                var unloadingTime1 = Math.Round(truckSize1 / velocity.ToDouble() * 60);

                var concreteCentralVelocity1 = connection.Query<float>(
                        "Select Velocity From CPCVelocity Where CPCId = @Id",
                        new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                    .FirstOrDefault();

                var lastLoading = truckResult.Where(x => x.Type == "2" && x.TurnNumber == 1)
                    .MaxBy(c => c.TruckOrder);
                var maxTruckNumber = lastLoading != null ? lastLoading.TruckOrder : 0;

                var maxLoadingNumber = truckResult.FirstOrDefault() != null
                    ? truckResult.MaxBy(x => x.LoadingNumber)!.LoadingNumber
                    : 0;

                var loadingTime1 = Math.Round(truckSize1 / concreteCentralVelocity1 * 60);


                var tt = DateTime.ParseExact(ThAutomationParameter.GetThProductWithTrucksDto.STime, "HH:mm",
                    CultureInfo.InvariantCulture);

                var unloadTime = ThAutomationParameter.GetThProductWithTrucksDto.Date.AddHours(tt.Hour)
                    .AddMinutes(tt.Minute);

                var truckUnloadParam1 = new ThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                    Title = "Unloading",
                    StartTime = unloadTime,
                    EndTime = unloadTime.AddMinutes(unloadingTime1),
                    Type = "2",
                    TurnNumber = 1,
                    TruckOrder = maxTruckNumber + 1,
                    LoadingNumber = maxLoadingNumber + 1
                };

                var projectSequenceId1 = connection
                    .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).FirstOrDefault();


                var mapLocation1 = dbconnection
                    .Query<Position>(
                        "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                        new { SequenceCode = projectSequenceId1 }).FirstOrDefault();


                if (mapLocation1 != null)
                {
                    // var duration = ThAutomationParameter._iShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                    //     50.93654767100221, 3.1299880230856334, ThAutomationParameter.TenantProvider,ThAutomationParameter.Configuration,true);
                    //
                    // if (await duration != 0)
                    // {
                    var dur = 20;
                    var truckTravel1Param = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                        Title = "First Travel",
                        // StartTime = projectStartTime.AddMinutes(-await duration),
                        StartTime = truckUnloadParam1.StartTime?.AddMinutes(-dur),
                        EndTime = truckUnloadParam1.StartTime,
                        Type = "1",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };


                    var truckTravel2Param = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                        Title = "Second Travel",
                        StartTime = truckUnloadParam1.EndTime,
                        // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                        EndTime = truckUnloadParam1.EndTime?.AddMinutes(dur),
                        Type = "3",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };

                    //var endTimeTruck = truckTravel2Param.EndTime;

                    var truckLoadingParam = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                        Title = "Loading",
                        StartTime = truckTravel1Param.StartTime?.AddMinutes(-loadingTime1),
                        EndTime = truckTravel1Param.StartTime,
                        Type = "0",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };

                    await connection.ExecuteAsync(query, truckUnloadParam1);
                    await connection.ExecuteAsync(query, truckTravel1Param);
                    await connection.ExecuteAsync(query, truckTravel2Param);
                    await connection.ExecuteAsync(query, truckLoadingParam);

                    var dynamicDto = new ThPbsCreateDto
                    {
                        Capacity = truckSize1,
                        Velocity = velocity,
                        StartDate = truckLoadingParam.StartTime,
                        EndDate = truckTravel2Param.EndTime,
                        CpcTitle = cpcTitle,
                        TurnNumber = 1
                    };

                    ThAutomationParameter.ThPbsCreateDto = dynamicDto;

                    await ThPbsCreate(ThAutomationParameter);

                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel1Param);
                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel2Param);
                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckLoadingParam);
                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckUnloadParam1);
                }
            }
            else
            {
                if (!truckResult.Any())
                {
                    var truckSize = connection.Query<float>(
                        "Select Size From CorporateProductCatalog Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                    var unloadingTime = Math.Round(truckSize / velocity.ToDouble() * 60);

                    var concreteCentralVelocity = connection.Query<float>(
                            "Select Velocity From CPCVelocity Where CPCId = @Id",
                            new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                        .FirstOrDefault();

                    var loadingTime = Math.Round(truckSize / concreteCentralVelocity * 60);

                    var truckUnloadParam = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                        Title = "Unloading",
                        StartTime = projectStartTime,
                        EndTime = projectStartTime.AddMinutes(unloadingTime),
                        Type = "2",
                        TurnNumber = 1,
                        TruckOrder = 1,
                        LoadingNumber = 1
                    };

                    var projectSequenceId = connection
                        .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                            new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId })
                        .FirstOrDefault();


                    var mapLocation = dbconnection
                        .Query<Position>(
                            "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                            new { SequenceCode = projectSequenceId }).FirstOrDefault();
                    if (mapLocation != null)
                    {
                        // var duration = ThAutomationParameter._iShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                        //     50.93654767100221, 3.1299880230856334, ThAutomationParameter.TenantProvider,ThAutomationParameter.Configuration,true);
                        //
                        // if (await duration != 0)
                        // {
                        var dur = 20;
                        var truckTravel1Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "First Travel",
                            // StartTime = projectStartTime.AddMinutes(-await duration),
                            StartTime = projectStartTime.AddMinutes(-dur),
                            EndTime = projectStartTime,
                            Type = "1",
                            TurnNumber = 1,
                            TruckOrder = 1,
                            LoadingNumber = 1
                        };

                        var truckTravel2Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "Second Travel",
                            StartTime = truckUnloadParam.EndTime,
                            // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                            EndTime = truckUnloadParam.EndTime?.AddMinutes(dur),
                            Type = "3",
                            TurnNumber = 1,
                            TruckOrder = 1,
                            LoadingNumber = 1
                        };

                        var endTimeTruck = truckTravel2Param.EndTime;

                        var truckLoadingParam = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "Loading",
                            StartTime = truckTravel1Param.StartTime?.AddMinutes(-loadingTime),
                            EndTime = truckTravel1Param.StartTime,
                            Type = "0",
                            TurnNumber = 1,
                            TruckOrder = 1,
                            LoadingNumber = 1
                        };

                        await connection.ExecuteAsync(query, truckUnloadParam);
                        await connection.ExecuteAsync(query, truckTravel1Param);
                        await connection.ExecuteAsync(query, truckTravel2Param);
                        await connection.ExecuteAsync(query, truckLoadingParam);


                        var dynamicDto = new ThPbsCreateDto
                        {
                            Capacity = truckSize,
                            Velocity = velocity,
                            StartDate = truckLoadingParam.StartTime,
                            EndDate = truckTravel2Param.EndTime,
                            CpcTitle = cpcTitle,
                            TurnNumber = 1
                        };

                        ThAutomationParameter.ThPbsCreateDto = dynamicDto;

                        await ThPbsCreate(ThAutomationParameter);

                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel1Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel2Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckLoadingParam);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckUnloadParam);


                        var truckTotalTime =
                            (truckTravel2Param.EndTime - truckLoadingParam.StartTime).Value.TotalMinutes;
                        var remainingTime = (projectEndTime - truckTravel2Param.EndTime).Value.TotalMinutes;

                        var remainingTurns = Math.Floor(remainingTime / truckTotalTime);

                        for (var i = 0; i < remainingTurns; i++)
                        {
                            var Loading = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Loading",
                                StartTime = endTimeTruck,
                                EndTime = endTimeTruck.Value.AddMinutes(loadingTime),
                                Type = "0",
                                TurnNumber = i + 2,
                                TruckOrder = 1,
                                LoadingNumber = i + 2
                            };

                            var travel1 = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "First Travel",
                                StartTime = Loading.EndTime,
                                // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                                EndTime = Loading.EndTime.Value.AddMinutes(dur),
                                Type = "1",
                                TurnNumber = i + 2,
                                TruckOrder = 1,
                                LoadingNumber = i + 2
                            };

                            var unload = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Unloading",
                                StartTime = travel1.EndTime,
                                EndTime = travel1.EndTime.Value.AddMinutes(unloadingTime),
                                Type = "2",
                                TurnNumber = i + 2,
                                TruckOrder = 1,
                                LoadingNumber = i + 2
                            };

                            var travel2 = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Second Travel",
                                StartTime = unload.EndTime,
                                // EndTime = unload.EndTime?.AddMinutes(await duration),
                                EndTime = unload.EndTime?.AddMinutes(dur),
                                Type = "3",
                                TurnNumber = i + 2,
                                TruckOrder = 1,
                                LoadingNumber = i + 2
                            };

                            endTimeTruck = travel2.EndTime;

                            await connection.ExecuteAsync(query, Loading);
                            await connection.ExecuteAsync(query, travel1);
                            await connection.ExecuteAsync(query, travel2);
                            await connection.ExecuteAsync(query, unload);

                            var dynamicDto1 = new ThPbsCreateDto
                            {
                                Capacity = truckSize,
                                Velocity = velocity,
                                StartDate = Loading.StartTime,
                                EndDate = travel2.EndTime,
                                CpcTitle = cpcTitle,
                                TurnNumber = i + 2
                            };

                            ThAutomationParameter.ThPbsCreateDto = dynamicDto1;

                            await ThPbsCreate(ThAutomationParameter);

                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel1);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel2);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, Loading);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, unload);
                        }

                        //}
                    }
                }
                else
                {
                    var lastLoading = truckResult.Where(x => x.Type == "2" && x.TurnNumber == 1)
                        .MaxBy(c => c.TruckOrder);
                    var maxTruckNumber = lastLoading.TruckOrder;
                    var maxLoadingNumber = truckResult.MaxBy(x => x.LoadingNumber)!.LoadingNumber;

                    var truckSize = connection.Query<float>(
                        "Select Size From CorporateProductCatalog Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                    var unloadingTime = Math.Round(truckSize / velocity.ToDouble() * 60);

                    var concreteCentralVelocity = connection.Query<float>(
                            "Select Velocity From CPCVelocity Where CPCId = @Id",
                            new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                        .FirstOrDefault();

                    var loadingTime = Math.Round(truckSize / concreteCentralVelocity * 60);

                    var truckUnloadParam = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                        Title = "Unloading",
                        StartTime = lastLoading.EndTime,
                        EndTime = lastLoading.EndTime?.AddMinutes(unloadingTime),
                        Type = "2",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };

                    var projectSequenceId = connection
                        .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                            new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId })
                        .FirstOrDefault();


                    var mapLocation = dbconnection
                        .Query<Position>(
                            "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                            new { SequenceCode = projectSequenceId }).FirstOrDefault();
                    if (mapLocation != null)
                    {
                        // var duration = ThAutomationParameter._iShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                        //     50.93654767100221, 3.1299880230856334, ThAutomationParameter.TenantProvider,ThAutomationParameter.Configuration,true);
                        //
                        // if (await duration != 0)
                        // {
                        const int dur = 20;
                        var truckTravel1Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "First Travel",
                            // StartTime = projectStartTime.AddMinutes(-await duration),
                            StartTime = lastLoading.EndTime?.AddMinutes(-dur),
                            EndTime = lastLoading.EndTime,
                            Type = "1",
                            TurnNumber = 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        var truckTravel2Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "Second Travel",
                            StartTime = truckUnloadParam.EndTime,
                            // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                            EndTime = truckUnloadParam.EndTime?.AddMinutes(dur),
                            Type = "3",
                            TurnNumber = 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        var endTimeTruck = truckTravel2Param.EndTime;

                        var truckLoadingParam = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "Loading",
                            StartTime = truckTravel1Param.StartTime?.AddMinutes(-loadingTime),
                            EndTime = truckTravel1Param.StartTime,
                            Type = "0",
                            TurnNumber = 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        await connection.ExecuteAsync(query, truckUnloadParam);
                        await connection.ExecuteAsync(query, truckTravel1Param);
                        await connection.ExecuteAsync(query, truckTravel2Param);
                        await connection.ExecuteAsync(query, truckLoadingParam);


                        var dynamicDto = new ThPbsCreateDto
                        {
                            Capacity = truckSize,
                            Velocity = velocity,
                            StartDate = truckLoadingParam.StartTime,
                            EndDate = truckTravel2Param.EndTime,
                            CpcTitle = cpcTitle,
                            TurnNumber = 1
                        };

                        ThAutomationParameter.ThPbsCreateDto = dynamicDto;

                        await ThPbsCreate(ThAutomationParameter);

                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel1Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel2Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckLoadingParam);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckUnloadParam);

                        var truckTotalTime =
                            (truckTravel2Param.EndTime - truckLoadingParam.StartTime).Value.TotalMinutes;
                        var remainingTime = (projectEndTime - truckTravel2Param.EndTime).Value.TotalMinutes;

                        var remainingTurns = Math.Floor(remainingTime / truckTotalTime);

                        for (var i = 0; i < remainingTurns; i++)
                        {
                            // var lastTruckTurn2 = truckResult.FirstOrDefault(x => x.TruckOrder == maxTruckNumber && x.TurnNumber == i + 2 && x.Type == "2");
                            //
                            // if (lastTruckTurn2 != null)
                            // {
                            //
                            //     var unload = new ThTrucksSchedule()
                            //     {
                            //         Id = Guid.NewGuid().ToString(),
                            //         ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            //         Title = "Unloading",
                            //         StartTime = lastTruckTurn2.EndTime,
                            //         EndTime = lastTruckTurn2.EndTime.Value.AddMinutes(unloadingTime),
                            //         Type = "2",
                            //         TurnNumber = i + 2,
                            //         TruckOrder = maxTruckNumber + 1,
                            //         LoadingNumber = maxLoadingNumber + 2 + i
                            //
                            //     };
                            //     
                            //     var travel2 = new ThTrucksSchedule()
                            //     {
                            //         Id = Guid.NewGuid().ToString(),
                            //         ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            //         Title = "Second Travel",
                            //         StartTime = unload.EndTime,
                            //         // EndTime = unload.EndTime?.AddMinutes(await duration),
                            //         EndTime = unload.EndTime?.AddMinutes(dur),
                            //         Type = "3",
                            //         TurnNumber = i + 2,
                            //         TruckOrder = maxTruckNumber + 1,
                            //         LoadingNumber = maxLoadingNumber + 2 + i
                            //
                            //     };
                            //
                            //     var travel1 = new ThTrucksSchedule()
                            //     {
                            //         Id = Guid.NewGuid().ToString(),
                            //         ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            //         Title = "First Travel",
                            //         StartTime = unload.StartTime.Value.AddMinutes(-dur),
                            //         // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                            //         EndTime = unload.StartTime,
                            //         Type = "1",
                            //         TurnNumber = i + 2,
                            //         TruckOrder = maxTruckNumber + 1,
                            //         LoadingNumber = maxLoadingNumber + 2 + i
                            //
                            //     };
                            //
                            //     var Loading = new ThTrucksSchedule()
                            //     {
                            //         Id = Guid.NewGuid().ToString(),
                            //         ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            //         Title = "Loading",
                            //         StartTime = travel1.StartTime.Value.AddMinutes(-loadingTime),
                            //         EndTime = travel1.StartTime,
                            //         Type = "0",
                            //         TurnNumber = i + 2,
                            //         TruckOrder = maxTruckNumber + 1,
                            //         LoadingNumber = maxLoadingNumber + 2 + i
                            //
                            //     };
                            //
                            //     await connection.ExecuteAsync(query, Loading);
                            //     await connection.ExecuteAsync(query, travel1);
                            //     await connection.ExecuteAsync(query, travel2);
                            //     await connection.ExecuteAsync(query, unload);
                            //
                            // }
                            // else
                            // {

                            var Loading = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Loading",
                                StartTime = endTimeTruck,
                                EndTime = endTimeTruck.Value.AddMinutes(loadingTime),
                                Type = "0",
                                TurnNumber = i + 2,
                                TruckOrder = maxTruckNumber + 1,
                                LoadingNumber = maxLoadingNumber + 2 + i
                            };

                            var travel1 = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "First Travel",
                                StartTime = Loading.EndTime,
                                // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                                EndTime = Loading.EndTime.Value.AddMinutes(dur),
                                Type = "1",
                                TurnNumber = i + 2,
                                TruckOrder = maxTruckNumber + 1,
                                LoadingNumber = maxLoadingNumber + 2 + i
                            };

                            var unload = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Unloading",
                                StartTime = travel1.EndTime,
                                EndTime = travel1.EndTime.Value.AddMinutes(unloadingTime),
                                Type = "2",
                                TurnNumber = i + 2,
                                TruckOrder = maxTruckNumber + 1,
                                LoadingNumber = maxLoadingNumber + 2 + i
                            };

                            var travel2 = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Second Travel",
                                StartTime = unload.EndTime,
                                // EndTime = unload.EndTime?.AddMinutes(await duration),
                                EndTime = unload.EndTime?.AddMinutes(dur),
                                Type = "3",
                                TurnNumber = i + 2,
                                TruckOrder = maxTruckNumber + 1,
                                LoadingNumber = maxLoadingNumber + 2 + i
                            };

                            endTimeTruck = travel2.EndTime;

                            await connection.ExecuteAsync(query, Loading);
                            await connection.ExecuteAsync(query, travel1);
                            await connection.ExecuteAsync(query, travel2);
                            await connection.ExecuteAsync(query, unload);

                            var dynamicDto1 = new ThPbsCreateDto
                            {
                                Capacity = truckSize,
                                Velocity = velocity,
                                StartDate = Loading.StartTime,
                                EndDate = travel2.EndTime,
                                CpcTitle = cpcTitle,
                                TurnNumber = i + 2
                            };

                            ThAutomationParameter.ThPbsCreateDto = dynamicDto1;

                            await ThPbsCreate(ThAutomationParameter);

                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel1);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel2);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, Loading);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, unload);
                        }

                        //}

                        //}
                    }
                }
            }
        }
        else
        {
            if (ThAutomationParameter.GetThProductWithTrucksDto.Type == "14")
            {
                var query =
                    @"INSERT INTO dbo.ThTrucksSchedule ( Id ,ProductTruckId ,Title ,EndTime ,StartTime ,Type,TurnNumber,LoadingNumber,TruckOrder ) VALUES ( @Id ,@ProductTruckId ,@Title ,@EndTime ,@StartTime ,@Type,@TurnNumber,@LoadingNumber,@TruckOrder )";

                var truckResult = connection.Query<GetThTrucksSchedule>(
                    @"SELECT * FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id WHERE ThProductWithTrucks.ProductId = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).ToList();

                var cpcTitle = connection
                    .Query<string>("Select Title From CorporateProductCatalog Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();
                var truckSize1 = connection.Query<float>("Select Size From CorporateProductCatalog Where Id = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                var unloadingTime1 = Math.Round(truckSize1 / velocity.ToDouble() * 60);

                var concreteCentralVelocity1 = connection.Query<float>(
                        "Select Velocity From CPCVelocity Where CPCId = @Id",
                        new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                    .FirstOrDefault();

                var lastLoading = truckResult.Where(x => x.Type == "2" && x.TurnNumber == 1)
                    .MaxBy(c => c.TruckOrder);
                var maxTruckNumber = lastLoading != null ? lastLoading.TruckOrder : 0;

                var maxLoadingNumber = truckResult.FirstOrDefault() != null
                    ? truckResult.MaxBy(x => x.LoadingNumber)!.LoadingNumber
                    : 0;

                var loadingTime1 = Math.Round(truckSize1 / concreteCentralVelocity1 * 60);


                var tt = DateTime.ParseExact(ThAutomationParameter.GetThProductWithTrucksDto.STime, "HH:mm",
                    CultureInfo.InvariantCulture);

                var unloadTime = ThAutomationParameter.GetThProductWithTrucksDto.Date.AddHours(tt.Hour)
                    .AddMinutes(tt.Minute);

                var truckUnloadParam1 = new ThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = isExist.FirstOrDefault().Id,
                    Title = "Unloading",
                    StartTime = unloadTime,
                    EndTime = unloadTime.AddMinutes(unloadingTime1),
                    Type = "2",
                    TurnNumber = 1,
                    TruckOrder = maxTruckNumber + 1,
                    LoadingNumber = maxLoadingNumber + 1
                };

                var projectSequenceId1 = connection
                    .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).FirstOrDefault();


                var mapLocation1 = dbconnection
                    .Query<Position>(
                        "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                        new { SequenceCode = projectSequenceId1 }).FirstOrDefault();
                if (mapLocation1 != null)
                {
                    // var duration = ThAutomationParameter._iShiftRepository.CalculateDistance(mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                    //     50.93654767100221, 3.1299880230856334, ThAutomationParameter.TenantProvider,ThAutomationParameter.Configuration,true);
                    //
                    // if (await duration != 0)
                    // {
                    var dur = 20;
                    var truckTravel1Param = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = isExist.FirstOrDefault().Id,
                        Title = "First Travel",
                        // StartTime = projectStartTime.AddMinutes(-await duration),
                        StartTime = truckUnloadParam1.StartTime?.AddMinutes(-dur),
                        EndTime = truckUnloadParam1.StartTime,
                        Type = "1",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };

                    var truckTravel2Param = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = isExist.FirstOrDefault().Id,
                        Title = "Second Travel",
                        StartTime = truckUnloadParam1.EndTime,
                        // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                        EndTime = truckUnloadParam1.EndTime?.AddMinutes(dur),
                        Type = "3",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };

                    //var endTimeTruck = truckTravel2Param.EndTime;

                    var truckLoadingParam = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = isExist.FirstOrDefault().Id,
                        Title = "Loading",
                        StartTime = truckTravel1Param.StartTime?.AddMinutes(-loadingTime1),
                        EndTime = truckTravel1Param.StartTime,
                        Type = "0",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };

                    await connection.ExecuteAsync(query, truckUnloadParam1);
                    await connection.ExecuteAsync(query, truckTravel1Param);
                    await connection.ExecuteAsync(query, truckTravel2Param);
                    await connection.ExecuteAsync(query, truckLoadingParam);

                    var dynamicDto = new ThPbsCreateDto
                    {
                        Capacity = truckSize1,
                        Velocity = velocity,
                        StartDate = truckLoadingParam.StartTime,
                        EndDate = truckTravel2Param.EndTime,
                        CpcTitle = cpcTitle,
                        TurnNumber = 1
                    };

                    ThAutomationParameter.ThPbsCreateDto = dynamicDto;

                    await ThPbsCreate(ThAutomationParameter);
                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel1Param);
                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel2Param);
                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckLoadingParam);
                    ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckUnloadParam1);
                }
            }
        }

        return ThAutomationParameter.GetThProductWithTrucksDto;
    }

    public async Task<string> RemoveTruckFromDay(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var productlist = connection
            .Query<string>("SELECT Id FROM dbo.ThProductWithTrucks WHERE CpcId = @Id AND Date = @Date",
                new
                {
                    ThAutomationParameter.RemoveTruckFromDayDto.Id, ThAutomationParameter.RemoveTruckFromDayDto.Date
                }).ToList();

        await connection.ExecuteAsync("DELETE FROM dbo.ThProductWithTrucks WHERE CpcId = @Id AND Date = @Date",
            new { ThAutomationParameter.RemoveTruckFromDayDto.Id, ThAutomationParameter.RemoveTruckFromDayDto.Date });

        foreach (var i in productlist)
            await connection.ExecuteAsync("DELETE FROM dbo.ThTrucksSchedule WHERE ProductTruckId = @Id;",
                new { Id = i });

        return ThAutomationParameter.RemoveTruckFromDayDto.Id;
    }

    public async Task<string> RemoveTruckFromDayForMyEnv(ThAutomationParameter ThAutomationParameter)
    {
        await using var dbConnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

        var projects = dbConnection.Query<ProjectDefinition>(
            "SELECT ProjectDefinition.* FROM dbo.ThCustomerOrganizations INNER  JOIN ProjectDefinition ON ThCustomerOrganizations.ProjectId = ProjectDefinition.Id AND ProjectDefinition.IsDeleted = 0");

        foreach (var project in projects.DistinctBy(x => x.SequenceCode))
        {
            var connectionString = ConnectionString.MapConnectionString("COM-0001",
                project.SequenceCode, ThAutomationParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            var productlist = connection
                .Query<string>("SELECT Id FROM dbo.ThProductWithTrucks WHERE CpcId = @Id AND Date = @Date",
                    new
                    {
                        ThAutomationParameter.RemoveTruckFromDayDto.Id, ThAutomationParameter.RemoveTruckFromDayDto.Date
                    }).ToList();

            await connection.ExecuteAsync("DELETE FROM dbo.ThProductWithTrucks WHERE CpcId = @Id AND Date = @Date",
                new
                {
                    ThAutomationParameter.RemoveTruckFromDayDto.Id, ThAutomationParameter.RemoveTruckFromDayDto.Date
                });

            foreach (var i in productlist)
                await connection.ExecuteAsync("DELETE FROM dbo.ThTrucksSchedule WHERE ProductTruckId = @Id;",
                    new { Id = i });
        }


        return ThAutomationParameter.RemoveTruckFromDayDto.Id;
    }


    public async Task<double> CalculateDistance(ThAutomationParameter ThAutomationParameter)
    {
        //string _address = "https://atlas.microsoft.com/route/directions/batch/sync/json?api-version=1.0&subscription-key=O0JczvPLEAonCkCAXASeOnKdiMpj_7qr5G2IcoGqKa4";
        //string _address = "https://atlas.microsoft.com/route/directions/batch/sync/json?api-version=1.0&subscription-key=1vju_5glNJUztg8XATTMHJjjaNdnlIuomHT5qp-lEh0";
        // var client = new HttpClient();
        // client.BaseAddress = new Uri(iTenantProvider.GetTenant().MapUrl);
        // // client.DefaultRequestHeaders.Accept.Add(
        // //     new MediaTypeWithQualityHeaderValue("application/json"));
        // client.DefaultRequestHeaders.Add("Accept", "application/json");
        // //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        //
        // AzureMap map = new AzureMap();
        // List<Query> batchItemList = new List<Query>();
        // Query query = new Query();
        // query.query = "?query=" + previousLatitude + "," + previousLongitude + ":" + latitude + "," + longitude +
        //               "&travelMode=car&routeType=fastest";
        // //query.query = "?query=7.4406723,80.4589001:6.8270834,79.880269&travelMode=car&routeType=fastest";
        // batchItemList.Add(query);
        // map.batchItems = batchItemList;
        // HttpResponseMessage response = await client.PostAsJsonAsync(iTenantProvider.GetTenant().MapUrl, map);
        // //response.EnsureSuccessStatusCode();
        // var result = await response.Content.ReadAsStringAsync();
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
        // if (myDeserializedClass.batchItems.FirstOrDefault().statusCode == 200)
        // {
        //     return myDeserializedClass.batchItems.FirstOrDefault().response.routes.FirstOrDefault().summary
        //         .travelTimeInSeconds;
        // }
        // else
        // {
        //     return 0;
        // }

        var am = new AzureMapsServices(ThAutomationParameter.Configuration.GetValue<string>("AzureMapKey"));
        var client = new HttpClient();

        var origin = ThAutomationParameter.CaculateDistance.Latitude + "," +
                     ThAutomationParameter.CaculateDistance.Longitude;
        var destination = ThAutomationParameter.CaculateDistance.PreviousLatitude + "," +
                          ThAutomationParameter.CaculateDistance.PreviousLongitude;


        var resultUrl =
            $"https://atlas.microsoft.com/route/directions/json?subscription-key=LJgehqCoZFQyHnvKf4gDl1AVWh4ihTa9ZAgCGH5YY5Y&api-version=1.0&query={origin}:{destination}&routeType=fastest&TravelMode=car&computeTravelTimeFor=all";
        var response = await client.GetAsync(resultUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var directionResults = JsonConvert.DeserializeObject<RouteDirectionsResponse>(content);

        if (directionResults != null)
        {
            var hh = directionResults.Routes?.FirstOrDefault()?.Summary.TravelTimeInSeconds;
            // var hh = matrixResponse.Result.Matrix.FirstOrDefault().FirstOrDefault().Response.RouteSummary
            //     .TravelTimeInSeconds;

            //var hh = 530;
            var jj = (double)hh/ 60;
            var kk = (double)Math.Round((decimal)jj);
            return kk;
        }

        return 0;

        // https://atlas.microsoft.com/route/directions/batch/sync/json?api-version=1.0&subscription-key=
        // var client = new HttpClient();
        // //client.DefaultRequestHeaders.Add("LJgehqCoZFQyHnvKf4gDl1AVWh4ihTa9ZAgCGH5YY5Y", ThAutomationParameter.Configuration.GetValue<String>("AzureMapKey"));
        //
        // var origin = ThAutomationParameter.CaculateDistance.Longitude +"," + ThAutomationParameter.CaculateDistance.Latitude;
        // var destination = ThAutomationParameter.CaculateDistance.PreviousLongitude +"," + ThAutomationParameter.CaculateDistance.PreviousLatitude;
        //
        // var response = await client.GetAsync($"https://atlas.microsoft.com/route/directions/json?subscription-key=LJgehqCoZFQyHnvKf4gDl1AVWh4ihTa9ZAgCGH5YY5Y&api-version=1.0&query={origin}:{destination}&routeType=fastest&computeTravelTimeFor=all");
        // response.EnsureSuccessStatusCode();
        //
        // var content = await response.Content.ReadAsStringAsync();

        return 0;
    }

    public async Task<RemoveThProductDto> RemoveTHProduct(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            ThAutomationParameter.RemoveThProductDto.ProjectSequenceCode, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(
            "Delete FROM dbo.ThTrucksSchedule WHERE ProductTruckId = @ProductTruckId AND TurnNumber = @TurnNumber",
            ThAutomationParameter.RemoveThProductDto);

        var pbsTh = connection.Query<ThTrucksSchedule>(
            "Select * FROM dbo.ThTrucksSchedule WHERE ProductTruckId = @ProductTruckId",
            ThAutomationParameter.RemoveThProductDto);

        if (!pbsTh.Any())
            await connection.ExecuteAsync("Delete From ThProductWithTrucks Where Id = @ProductTruckId",
                ThAutomationParameter.RemoveThProductDto);


        return ThAutomationParameter.RemoveThProductDto;
    }

    public async Task<string> ThFileUpload(ThAutomationParameter ThAutomationParameter)
    {
        //var connectionString = ConnectionString.MapConnectionString("COM-0001",
        //    null, ThAutomationParameter.TenantProvider);

        await using var connection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);
        
        // await using var connection = new SqlConnection(connectionString);

        //await connection.ExecuteAsync("Delete FROM dbo.ThFileUpload WHERE ProductId = @ZipProduct AND ProjectId = @ZipProject", ThAutomationParameter.RemoveThProductDto);

        var fileName = ThAutomationParameter.Zip.Files.FirstOrDefault()?.FileName.Replace(" ","");

        var existFileName = connection.Query<string>(
            "Select FileName FROM dbo.WeTransfer WHERE Customer = @ZipProduct AND ProgressStatement = @ZipProject",
            new
            {
                ZipProduct = ThAutomationParameter.WeTransfer.Customer,
                ZipProject = ThAutomationParameter.WeTransfer.ProgressStatement
            }).FirstOrDefault();
        string url = null;
        string shortUrl = null;

        if (existFileName != null)
        {
            var client = new FileClient();
            url = client.PersistThZipUpload(fileName,
                ThAutomationParameter.TenantProvider
                , ThAutomationParameter.Zip.Files.FirstOrDefault(), ThAutomationParameter.WeTransfer.ProgressStatement,
                ThAutomationParameter.WeTransfer.Customer, existFileName);

            var shortId = ShortUrlIdCreate(url);
            shortUrl =  ThAutomationParameter.TenantProvider.GetTenant().Host + "/doc/" + shortId;

            

            await connection.ExecuteAsync(
                "Update dbo.WeTransfer Set Link = @url, FileName = @fileName , ShortUrlId = @ShortUrlId,ShortUrl = @ShortUrl WHERE Customer = @ZipProduct AND ProgressStatement = @ZipProject",
                new
                {
                    ZipProduct = ThAutomationParameter.WeTransfer.Customer,
                    ZipProject = ThAutomationParameter.WeTransfer.ProgressStatement,
                    url,
                    fileName = fileName,
                    ShortUrlId = shortId,
                    ShortUrl = shortUrl
                    
                });
        }
        else
        {
            var client = new FileClient();
            url = client.PersistThZipUpload(fileName,
                ThAutomationParameter.TenantProvider
                , ThAutomationParameter.Zip.Files.FirstOrDefault(), ThAutomationParameter.WeTransfer.ProgressStatement,
                ThAutomationParameter.WeTransfer.Customer, existFileName);

            var shortId = ShortUrlIdCreate(url);
            shortUrl =  ThAutomationParameter.TenantProvider.GetTenant().Host + "/doc/" + shortId;
            
            await connection.ExecuteAsync(
                "Insert Into dbo.WeTransfer (Id,Customer,ProgressStatement,Link,FileName,ShortUrlId,ShortUrl) Values (@Id,@ZipProduct, @ZipProject, @url, @fileName,@ShortUrlId,@ShortUrl)",
                new
                {
                    Id = Guid.NewGuid().ToString(), ZipProduct = ThAutomationParameter.WeTransfer.Customer,
                    ZipProject = ThAutomationParameter.WeTransfer.ProgressStatement,
                    url,
                    fileName = fileName,
                    ShortUrlId = shortId,
                    ShortUrl = shortUrl
                });
        }

        return shortUrl;
        //return url;
    }

    public async Task<List<GetThProductWithTrucks>> GetProductsWithTrucksForMyEnv(
        ThAutomationParameter ThAutomationParameter)
    {
        // var connectionString = ConnectionString.MapConnectionString("COM-0001",
        //     null, ThAutomationParameter.TenantProvider);

        await using var dbConnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

        var projects = dbConnection.Query<ProjectDefinition>(
            "SELECT ProjectDefinition.* FROM dbo.ThCustomerOrganizations INNER  JOIN ProjectDefinition ON ThCustomerOrganizations.ProjectId = ProjectDefinition.Id AND ProjectDefinition.IsDeleted = 0");
        var pbsData = new List<GetThProductWithTrucks>();

        // foreach (var project in projects.DistinctBy(x => x.SequenceCode))
        // {
        Parallel.ForEach(projects.DistinctBy(x => x.SequenceCode), project =>
        {
            using var connection = new SqlConnection(project.ProjectConnectionString);
            
            List<GetThProductWithTrucks> result;
            result = connection.Query<GetThProductWithTrucks>(
                "Select * from PbsProduct Where StartDate = @date AND ParentId IS NULL ",
                new { date = ThAutomationParameter.ThProductWithTrucksDto.Date }).ToList();
            connection.Close();

            // foreach (var item in result)
            //{
                Parallel.ForEach(result, item =>
                {
                    using var connection1 = new SqlConnection(project.ProjectConnectionString);
                    
                        var trucks = new List<GetThProductWithTrucksDto>();

                        var obj = new GetThProductWithTrucksDto
                        {
                            Id = "22222",
                            Date = ThAutomationParameter.ThProductWithTrucksDto.Date,
                            ProductId = item.Id
                        };
                        trucks.Add(obj);

                        var mPbsDynamicAttributes = connection1
                            .Query<PbsDynamicAttributesDto>(
                                "Select * from PbsDynamicAttributes Where ProductId = @ProductId ORDER BY [Key] DESC",
                                new { ProductId = item.Id }).AsEnumerable().ToList();

                        item.PbsDynamicAttributes = mPbsDynamicAttributes;
                        item.Capacity = new string(mPbsDynamicAttributes.FirstOrDefault(e => e.Key == "Capacity")
                            ?.Value.ToCharArray()
                            .Where(char.IsDigit)
                            .ToArray()).ToInt();
                        var truckList =
                            connection1.Query<GetThProductWithTrucksDto>(
                                    "Select ThProductWithTrucks.*,CorporateProductCatalog.ResourceTitle As Title , ThProductWithTrucks.IsTruck from ThProductWithTrucks left outer join CorporateProductCatalog on ThProductWithTrucks.CpcId = CorporateProductCatalog.Id Where ProductId = @ProductId AND Date = @date",
                                    new
                                    {
                                        ProductId = item.Id, date = ThAutomationParameter.ThProductWithTrucksDto.Date
                                    })
                                .ToList();
                        connection1.Close();

                        // foreach (var tr in truckList)
                        // {
                        Parallel.ForEach(truckList, tr =>
                        {
                            using var connection2 = new SqlConnection(project.ProjectConnectionString);

                            var trOrder = connection2
                                .Query<string>(
                                    "Select TruckOrder From ThTrucksSchedule Where ProductTruckId = @ProductTruckId",
                                    new { ProductTruckId = tr.Id }).FirstOrDefault();

                            if (trOrder != null) tr.TruckOrder = trOrder.ToInt();
                            connection2.Close();
                        });

                        trucks.AddRange(truckList);

                        item.Trucks = trucks.OrderBy(x => x.TruckOrder).ToList();
                        item.ProjectSequenceCode = project.SequenceCode;
                   
                });

            pbsData.AddRange(result);
        });

        var getThProductWithTrucksList = pbsData.OrderBy(e => e.Title).ToList().OrderByDescending(e => e.Capacity).ToList();
       // var thProductWithTrucksList = getThProductWithTrucksList.OrderByDescending(e => e.Capacity).ToList();

        return getThProductWithTrucksList;
    }

    public async Task<GetThProductWithTrucksDto> AddTrucksToProductMyEnv(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode, ThAutomationParameter.TenantProvider);

        var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        await using var dbconnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);
        await using var cuConnection = new SqlConnection(cuConnectionString);


        var pbsDynamicAttributes = connection
            .Query<PbsDynamicAttributes>("Select * from PbsDynamicAttributes Where ProductId = @ProductId",
                new { ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).ToList();

        var projectStartTime = DateTime.Parse(ThAutomationParameter.GetThProductWithTrucksDto.Date.ToShortDateString());
        var projectEndTime = DateTime.Parse(ThAutomationParameter.GetThProductWithTrucksDto.Date.ToShortDateString());
        var projectSTime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectStartTime = projectStartTime.AddHours(projectSTime.Hour)
            .AddMinutes(projectSTime.Minute);
        var projectETime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "EndTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectEndTime = projectEndTime.AddHours(projectETime.Hour).AddMinutes(projectSTime.Minute);
        var capacity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Capacity")?.Value;
        var velocity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Velocity")?.Value;


        var isExist = connection
            .Query<ThProductWithTrucks>(
                "Select * From ThProductWithTrucks Where CpcId = @CpcId AND ProductId = @ProductId",
                ThAutomationParameter.GetThProductWithTrucksDto);

        await CopyCpcCuToProject(ThAutomationParameter, ThAutomationParameter.GetThProductWithTrucksDto.CpcId,
            connectionString, "cu");

        if (!isExist.Any())
        {
            var insertQuery =
                @"INSERT INTO dbo.ThProductWithTrucks (Id, CpcId, ProductId, Date,IsTruck) VALUES (@Id, @CpcId, @ProductId, @Date, @IsTruck)";

            var query =
                @"INSERT INTO dbo.ThTrucksSchedule ( Id ,ProductTruckId ,Title ,EndTime ,StartTime ,Type,TurnNumber,LoadingNumber,TruckOrder ) VALUES ( @Id ,@ProductTruckId ,@Title ,@EndTime ,@StartTime ,@Type,@TurnNumber,@LoadingNumber,@TruckOrder )";

            ThAutomationParameter.GetThProductWithTrucksDto.IsTruck = true;
            await connection.ExecuteAsync(insertQuery, ThAutomationParameter.GetThProductWithTrucksDto);


            var truckResult = connection.Query<GetThTrucksSchedule>(
                @"SELECT * FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id WHERE ThProductWithTrucks.ProductId = @Id AND ThProductWithTrucks.IsTruck = 1",
                new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).ToList();

            var cpcTitle = cuConnection
                .Query<string>("Select Title From CorporateProductCatalog Where Id = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

            if (ThAutomationParameter.GetThProductWithTrucksDto.Type == "14")
            {
                var truckSize1 = cuConnection.Query<float>("Select Size From CorporateProductCatalog Where Id = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                var unloadingTime1 = Math.Round(truckSize1 / velocity.ToDouble() * 60);

                var concreteCentralVelocity1 = cuConnection.Query<float>(
                        "Select Velocity From CPCVelocity Where CPCId = @Id",
                        new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                    .FirstOrDefault();

                
                var maxTruck = truckResult.Where(x => x.Type == "2")
                    .MaxBy(c => c.TruckOrder);
                var maxTruckNumber = maxTruck != null ? maxTruck.TruckOrder : 0;

                var lastLoading = truckResult.Where(x => x.Type == "2" && x.TruckOrder == maxTruckNumber)
                    .MinBy(c => c.TurnNumber);

                var maxLoadingNumber = truckResult.FirstOrDefault() != null
                    ? truckResult.MaxBy(x => x.LoadingNumber)!.LoadingNumber
                    : 0;
                
                // var turnNumber  = truckResult.FirstOrDefault() != null
                //     ? truckResult.MaxBy(x => x.TurnNumber)!.TurnNumber
                //     : 0;
                var turnNumber = 1;

                var loadingTime1 = Math.Round(truckSize1 / concreteCentralVelocity1 * 60);


                var tt = DateTime.ParseExact(ThAutomationParameter.GetThProductWithTrucksDto.STime, "HH:mm",
                    CultureInfo.InvariantCulture);

                var unloadTime = ThAutomationParameter.GetThProductWithTrucksDto.Date.AddHours(tt.Hour)
                    .AddMinutes(tt.Minute);

                var truckUnloadParam1 = new ThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                    Title = "Unloading",
                    StartTime = unloadTime,
                    EndTime = unloadTime.AddMinutes(unloadingTime1),
                    Type = "2",
                    TurnNumber = turnNumber + 1,
                    TruckOrder = maxTruckNumber + 1,
                    LoadingNumber = maxLoadingNumber + 1
                };

                // var projectSequenceId1 = connection
                //     .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                //         new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).FirstOrDefault();

                var projectSequenceId1 = ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode;

                var mapLocation1 = dbconnection
                    .Query<Position>(
                        "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                        new { SequenceCode = projectSequenceId1 }).FirstOrDefault();
                if (mapLocation1 != null)
                {

                    var orgLat = ThAutomationParameter.Configuration.GetValue<string>("HQLat");
                    var orgLong = ThAutomationParameter.Configuration.GetValue<string>("HQLong");

                    var dur = ThAutomationParameter._iShiftRepository.CalculateDistance(
                        mapLocation1.Lat.ToDouble(), mapLocation1.Lon.ToDouble(),
                        orgLat.ToDouble(), orgLong.ToDouble(), ThAutomationParameter.TenantProvider,
                        ThAutomationParameter.Configuration, true);

                    if (await dur != 0)
                    {
                        //var dur = 20;
                        var truckTravel1Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "First Travel", 
                            //StartTime = projectStartTime.AddMinutes(-await duration),
                            StartTime = truckUnloadParam1.StartTime?.AddMinutes(-await dur),
                            EndTime = truckUnloadParam1.StartTime,
                            Type = "1",
                            TurnNumber = turnNumber + 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        var truckTravel2Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "Second Travel",
                            StartTime = truckUnloadParam1.EndTime,
                            // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                            EndTime = truckUnloadParam1.EndTime?.AddMinutes(await dur),
                            Type = "3",
                            TurnNumber = turnNumber + 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        //var endTimeTruck = truckTravel2Param.EndTime;

                        var truckLoadingParam = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "Loading",
                            StartTime = truckTravel1Param.StartTime?.AddMinutes(-loadingTime1),
                            EndTime = truckTravel1Param.StartTime,
                            Type = "0",
                            TurnNumber = turnNumber + 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        await connection.ExecuteAsync(query, truckUnloadParam1);
                        await connection.ExecuteAsync(query, truckTravel1Param);
                        await connection.ExecuteAsync(query, truckTravel2Param);
                        await connection.ExecuteAsync(query, truckLoadingParam);

                        var dynamicDto = new ThPbsCreateDto
                        {
                            Capacity = truckSize1,
                            Velocity = velocity,
                            StartDate = truckLoadingParam.StartTime,
                            EndDate = truckTravel2Param.EndTime,
                            CpcTitle = cpcTitle,
                            TurnNumber = 1
                        };

                        ThAutomationParameter.ThPbsCreateDto = dynamicDto;
                        ThAutomationParameter.ProjectSequenceId =
                            ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode;

                        var pbsId = await ThPbsCreate(ThAutomationParameter);

                        ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId;
                        
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel1Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel2Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckLoadingParam);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckUnloadParam1);
                    }
                }
            }
            else
            {
                if (!truckResult.Any())
                {
                    var truckSize = cuConnection.Query<float>(
                        "Select Size From CorporateProductCatalog Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                    var unloadingTime = Math.Round(truckSize / velocity.ToDouble() * 60);

                    var concreteCentralVelocity = cuConnection.Query<float>(
                            "Select Velocity From CPCVelocity Where CPCId = @Id",
                            new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                        .FirstOrDefault();

                    var loadingTime = Math.Round(truckSize / concreteCentralVelocity * 60);

                    var truckUnloadParam = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                        Title = "Unloading",
                        StartTime = projectStartTime,
                        EndTime = projectStartTime.AddMinutes(unloadingTime),
                        Type = "2",
                        TurnNumber = 1,
                        TruckOrder = 1,
                        LoadingNumber = 1
                    };

                    // var projectSequenceId = connection
                    //     .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                    //         new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId })
                    //     .FirstOrDefault();

                    var projectSequenceId = ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode;


                    var mapLocation = dbconnection
                        .Query<Position>(
                            "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                            new { SequenceCode = projectSequenceId }).FirstOrDefault();
                    if (mapLocation != null)
                    {
                        var orgLat = ThAutomationParameter.Configuration.GetValue<string>("HQLat");
                        var orgLong = ThAutomationParameter.Configuration.GetValue<string>("HQLong");

                        var dur = ThAutomationParameter._iShiftRepository.CalculateDistance(
                            mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                            orgLat.ToDouble(), orgLong.ToDouble(), ThAutomationParameter.TenantProvider,
                            ThAutomationParameter.Configuration, true);

                        if (await dur != 0)
                        {
                            var truckTravel1Param = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "First Travel",
                                // StartTime = projectStartTime.AddMinutes(-await duration),
                                StartTime = projectStartTime.AddMinutes(-await dur),
                                EndTime = projectStartTime,
                                Type = "1",
                                TurnNumber = 1,
                                TruckOrder = 1,
                                LoadingNumber = 1
                            };

                            var truckTravel2Param = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Second Travel",
                                StartTime = truckUnloadParam.EndTime,
                                // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                                EndTime = truckUnloadParam.EndTime?.AddMinutes(await dur),
                                Type = "3",
                                TurnNumber = 1,
                                TruckOrder = 1,
                                LoadingNumber = 1
                            };

                            var endTimeTruck = truckTravel2Param.EndTime;

                            var truckLoadingParam = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Loading",
                                StartTime = truckTravel1Param.StartTime?.AddMinutes(-loadingTime),
                                EndTime = truckTravel1Param.StartTime,
                                Type = "0",
                                TurnNumber = 1,
                                TruckOrder = 1,
                                LoadingNumber = 1
                            };

                            await connection.ExecuteAsync(query, truckUnloadParam);
                            await connection.ExecuteAsync(query, truckTravel1Param);
                            await connection.ExecuteAsync(query, truckTravel2Param);
                            await connection.ExecuteAsync(query, truckLoadingParam);


                            var dynamicDto = new ThPbsCreateDto
                            {
                                Capacity = truckSize,
                                Velocity = velocity,
                                StartDate = truckLoadingParam.StartTime,
                                EndDate = truckTravel2Param.EndTime,
                                CpcTitle = cpcTitle,
                                TurnNumber = 1
                            };

                            ThAutomationParameter.ThPbsCreateDto = dynamicDto;

                           var pbsId =  await ThPbsCreate(ThAutomationParameter);
                           
                           ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId;
                           
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel1Param);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel2Param);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckLoadingParam);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckUnloadParam);


                            var truckTotalTime =
                                (truckTravel2Param.EndTime - truckLoadingParam.StartTime).Value.TotalMinutes;
                            var remainingTime = (projectEndTime - truckTravel2Param.EndTime).Value.TotalMinutes;

                            var remainingTurns = Math.Floor(remainingTime / truckTotalTime);


                            for (var i = 0; i < remainingTurns; i++)
                            {
                                var Loading = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "Loading",
                                    StartTime = endTimeTruck,
                                    EndTime = endTimeTruck.Value.AddMinutes(loadingTime),
                                    Type = "0",
                                    TurnNumber = i + 2,
                                    TruckOrder = 1,
                                    LoadingNumber = i + 2
                                };

                                var travel1 = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "First Travel",
                                    StartTime = Loading.EndTime,
                                    // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                                    EndTime = Loading.EndTime.Value.AddMinutes(await dur),
                                    Type = "1",
                                    TurnNumber = i + 2,
                                    TruckOrder = 1,
                                    LoadingNumber = i + 2
                                };

                                var unload = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "Unloading",
                                    StartTime = travel1.EndTime,
                                    EndTime = travel1.EndTime.Value.AddMinutes(unloadingTime),
                                    Type = "2",
                                    TurnNumber = i + 2,
                                    TruckOrder = 1,
                                    LoadingNumber = i + 2
                                };

                                var travel2 = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "Second Travel",
                                    StartTime = unload.EndTime,
                                    // EndTime = unload.EndTime?.AddMinutes(await duration),
                                    EndTime = unload.EndTime?.AddMinutes(await dur),
                                    Type = "3",
                                    TurnNumber = i + 2,
                                    TruckOrder = 1,
                                    LoadingNumber = i + 2
                                };

                                endTimeTruck = travel2.EndTime;

                                await connection.ExecuteAsync(query, Loading);
                                await connection.ExecuteAsync(query, travel1);
                                await connection.ExecuteAsync(query, travel2);
                                await connection.ExecuteAsync(query, unload);

                                var dynamicDto1 = new ThPbsCreateDto
                                {
                                    Capacity = truckSize,
                                    Velocity = velocity,
                                    StartDate = Loading.StartTime,
                                    EndDate = travel2.EndTime,
                                    CpcTitle = cpcTitle,
                                    TurnNumber = i + 2
                                };

                                ThAutomationParameter.ThPbsCreateDto = dynamicDto1;

                              var pbsId1 =  await ThPbsCreate(ThAutomationParameter);
                              ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId1;


                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, Loading);
                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel1);
                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel2);
                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, unload);

                                var truckRemainTime = (projectEndTime - travel2.EndTime).Value.TotalMinutes;
                                var truckTtimesx = truckTotalTime - await dur;
                                if (truckRemainTime < truckTotalTime)
                                {
                                    if (truckTtimesx <= truckRemainTime)
                                    {


                                        var Loadingx = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Loading",
                                            StartTime = travel2.EndTime,
                                            EndTime = travel2.EndTime.Value.AddMinutes(loadingTime),
                                            Type = "0",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        var travel1x = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "First Travel",
                                            StartTime = Loadingx.EndTime,
                                            // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                                            EndTime = Loadingx.EndTime.Value.AddMinutes(await dur),
                                            Type = "1",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        var unloadx = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Unloading",
                                            StartTime = travel1x.EndTime,
                                            EndTime = travel1x.EndTime.Value.AddMinutes(unloadingTime),
                                            Type = "2",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        var travel2x = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Second Travel",
                                            StartTime = unloadx.EndTime,
                                            // EndTime = unload.EndTime?.AddMinutes(await duration),
                                            EndTime = unloadx.EndTime?.AddMinutes(await dur),
                                            Type = "3",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        endTimeTruck = travel2.EndTime;

                                        await connection.ExecuteAsync(query, Loadingx);
                                        await connection.ExecuteAsync(query, travel1x);
                                        await connection.ExecuteAsync(query, travel2x);
                                        await connection.ExecuteAsync(query, unloadx);

                                        var dynamicDto1x = new ThPbsCreateDto
                                        {
                                            Capacity = truckSize,
                                            Velocity = velocity,
                                            StartDate = Loadingx.StartTime,
                                            EndDate = travel2x.EndTime,
                                            CpcTitle = cpcTitle,
                                            TurnNumber = travel2.TurnNumber + 1
                                        };

                                        ThAutomationParameter.ThPbsCreateDto = dynamicDto1x;

                                       var pbsId2 = await ThPbsCreate(ThAutomationParameter);
                                       ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId2;
                                       
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, Loadingx);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel1x);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel2x);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, unloadx);
                                    }
                                }
                            }

                            if (remainingTurns == 0)
                            {
                                var truckRemainTime = (projectEndTime - truckTravel2Param.EndTime).Value.TotalMinutes;
                                var truckTtimesx = truckTotalTime - await dur;
                                if (truckRemainTime < truckTotalTime)
                                {
                                    if (truckTtimesx <= truckRemainTime)
                                    {


                                        var Loadingx = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Loading",
                                            StartTime = truckTravel2Param.EndTime,
                                            EndTime = truckTravel2Param.EndTime.Value.AddMinutes(loadingTime),
                                            Type = "0",
                                            TurnNumber = truckTravel2Param.TurnNumber + 1,
                                            TruckOrder = truckTravel2Param.TruckOrder,
                                            LoadingNumber = truckTravel2Param.TurnNumber + 1
                                        };

                                        var travel1x = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "First Travel",
                                            StartTime = Loadingx.EndTime,
                                            // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                                            EndTime = Loadingx.EndTime.Value.AddMinutes(await dur),
                                            Type = "1",
                                            TurnNumber = truckTravel2Param.TurnNumber + 1,
                                            TruckOrder = truckTravel2Param.TruckOrder,
                                            LoadingNumber = truckTravel2Param.TurnNumber + 1
                                        };

                                        var unloadx = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Unloading",
                                            StartTime = travel1x.EndTime,
                                            EndTime = travel1x.EndTime.Value.AddMinutes(unloadingTime),
                                            Type = "2",
                                            TurnNumber = truckTravel2Param.TurnNumber + 1,
                                            TruckOrder = truckTravel2Param.TruckOrder,
                                            LoadingNumber = truckTravel2Param.TurnNumber + 1
                                        };

                                        var travel2x = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Second Travel",
                                            StartTime = unloadx.EndTime,
                                            // EndTime = unload.EndTime?.AddMinutes(await duration),
                                            EndTime = unloadx.EndTime?.AddMinutes(await dur),
                                            Type = "3",
                                            TurnNumber = truckTravel2Param.TurnNumber + 1,
                                            TruckOrder = truckTravel2Param.TruckOrder,
                                            LoadingNumber = truckTravel2Param.TurnNumber + 1
                                        };

                                        endTimeTruck = truckTravel2Param.EndTime;

                                        await connection.ExecuteAsync(query, Loadingx);
                                        await connection.ExecuteAsync(query, travel1x);
                                        await connection.ExecuteAsync(query, travel2x);
                                        await connection.ExecuteAsync(query, unloadx);

                                        var dynamicDto1x = new ThPbsCreateDto
                                        {
                                            Capacity = truckSize,
                                            Velocity = velocity,
                                            StartDate = Loadingx.StartTime,
                                            EndDate = travel2x.EndTime,
                                            CpcTitle = cpcTitle,
                                            TurnNumber = truckTravel2Param.TurnNumber + 1
                                        };

                                        ThAutomationParameter.ThPbsCreateDto = dynamicDto1x;

                                       var pbsId2 =  await ThPbsCreate(ThAutomationParameter);
                                       ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId2;


                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, Loadingx);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel1x);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel2x);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, unloadx);
                                    }
                                }
                            }
                        }

                        //}
                    }
                }
                else
                {
                    var maxTruck = truckResult.Where(x => x.Type == "2")
                        .MaxBy(c => c.TruckOrder);
                    var maxTruckNumber = maxTruck.TruckOrder;

                    var lastLoading = truckResult.Where(x => x.Type == "2" && x.TruckOrder == maxTruckNumber)
                        .MinBy(c => c.TurnNumber);
                    var maxLoadingNumber = truckResult.MaxBy(x => x.LoadingNumber)!.LoadingNumber;

                    var truckSize = cuConnection.Query<float>(
                        "Select Size From CorporateProductCatalog Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                    var unloadingTime = Math.Round(truckSize / velocity.ToDouble() * 60);

                    var concreteCentralVelocity = cuConnection.Query<float>(
                            "Select Velocity From CPCVelocity Where CPCId = @Id",
                            new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                        .FirstOrDefault();

                    var loadingTime = Math.Round(truckSize / concreteCentralVelocity * 60);

                    var truckUnloadParam = new ThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                        Title = "Unloading",
                        StartTime = lastLoading.EndTime,
                        EndTime = lastLoading.EndTime?.AddMinutes(unloadingTime),
                        Type = "2",
                        TurnNumber = 1,
                        TruckOrder = maxTruckNumber + 1,
                        LoadingNumber = maxLoadingNumber + 1
                    };

                    // var projectSequenceId = connection
                    //     .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                    //         new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId })
                    //     .FirstOrDefault();

                    var projectSequenceId = ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode;


                    var mapLocation = dbconnection
                        .Query<Position>(
                            "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                            new { SequenceCode = projectSequenceId }).FirstOrDefault();
                    if (mapLocation != null)
                    {
                        var orgLat = ThAutomationParameter.Configuration.GetValue<string>("HQLat");
                        var orgLong = ThAutomationParameter.Configuration.GetValue<string>("HQLong");

                        var dur = ThAutomationParameter._iShiftRepository.CalculateDistance(
                            mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                            orgLat.ToDouble(), orgLong.ToDouble(), ThAutomationParameter.TenantProvider,
                            ThAutomationParameter.Configuration, true);
                        
                        if (await dur != 0)
                        {
                            var truckTravel1Param = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "First Travel",
                                // StartTime = projectStartTime.AddMinutes(-await duration),
                                StartTime = lastLoading.EndTime?.AddMinutes(-await dur),
                                EndTime = lastLoading.EndTime,
                                Type = "1",
                                TurnNumber = 1,
                                TruckOrder = maxTruckNumber + 1,
                                LoadingNumber = maxLoadingNumber + 1
                            };

                            var truckTravel2Param = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Second Travel",
                                StartTime = truckUnloadParam.EndTime,
                                // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                                EndTime = truckUnloadParam.EndTime?.AddMinutes(await dur),
                                Type = "3",
                                TurnNumber = 1,
                                TruckOrder = maxTruckNumber + 1,
                                LoadingNumber = maxLoadingNumber + 1
                            };

                            var endTimeTruck = truckTravel2Param.EndTime;

                            var truckLoadingParam = new ThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                Title = "Loading",
                                StartTime = truckTravel1Param.StartTime?.AddMinutes(-loadingTime),
                                EndTime = truckTravel1Param.StartTime,
                                Type = "0",
                                TurnNumber = 1,
                                TruckOrder = maxTruckNumber + 1,
                                LoadingNumber = maxLoadingNumber + 1
                            };

                            await connection.ExecuteAsync(query, truckUnloadParam);
                            await connection.ExecuteAsync(query, truckTravel1Param);
                            await connection.ExecuteAsync(query, truckTravel2Param);
                            await connection.ExecuteAsync(query, truckLoadingParam);


                            var dynamicDto = new ThPbsCreateDto
                            {
                                Capacity = truckSize,
                                Velocity = velocity,
                                StartDate = truckLoadingParam.StartTime,
                                EndDate = truckTravel2Param.EndTime,
                                CpcTitle = cpcTitle,
                                TurnNumber = 1
                            };

                            ThAutomationParameter.ThPbsCreateDto = dynamicDto;

                            var pbsId = await ThPbsCreate(ThAutomationParameter);
                            ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId;


                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel1Param);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckTravel2Param);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckLoadingParam);
                            ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, truckUnloadParam);

                            var truckTotalTime =
                                (truckTravel2Param.EndTime - truckLoadingParam.StartTime).Value.TotalMinutes;
                            var remainingTime = (projectEndTime - truckTravel2Param.EndTime).Value.TotalMinutes;

                            var remainingTurns = Math.Floor(remainingTime / truckTotalTime);

                            for (var i = 0; i < remainingTurns; i++)
                            {
                                
                                var Loading = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "Loading",
                                    StartTime = endTimeTruck,
                                    EndTime = endTimeTruck.Value.AddMinutes(loadingTime),
                                    Type = "0",
                                    TurnNumber = i + 2,
                                    TruckOrder = maxTruckNumber + 1,
                                    LoadingNumber = maxLoadingNumber + 2 + i
                                };

                                var travel1 = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "First Travel",
                                    StartTime = Loading.EndTime,
                                    // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                                    EndTime = Loading.EndTime.Value.AddMinutes(await dur),
                                    Type = "1",
                                    TurnNumber = i + 2,
                                    TruckOrder = maxTruckNumber + 1,
                                    LoadingNumber = maxLoadingNumber + 2 + i
                                };

                                var unload = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "Unloading",
                                    StartTime = travel1.EndTime,
                                    EndTime = travel1.EndTime.Value.AddMinutes(unloadingTime),
                                    Type = "2",
                                    TurnNumber = i + 2,
                                    TruckOrder = maxTruckNumber + 1,
                                    LoadingNumber = maxLoadingNumber + 2 + i
                                };

                                var travel2 = new ThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                    Title = "Second Travel",
                                    StartTime = unload.EndTime,
                                    // EndTime = unload.EndTime?.AddMinutes(await duration),
                                    EndTime = unload.EndTime?.AddMinutes(await dur),
                                    Type = "3",
                                    TurnNumber = i + 2,
                                    TruckOrder = maxTruckNumber + 1,
                                    LoadingNumber = maxLoadingNumber + 2 + i
                                };

                                endTimeTruck = travel2.EndTime;

                                await connection.ExecuteAsync(query, Loading);
                                await connection.ExecuteAsync(query, travel1);
                                await connection.ExecuteAsync(query, travel2);
                                await connection.ExecuteAsync(query, unload);

                                var dynamicDto1 = new ThPbsCreateDto
                                {
                                    Capacity = truckSize,
                                    Velocity = velocity,
                                    StartDate = Loading.StartTime,
                                    EndDate = travel2.EndTime,
                                    CpcTitle = cpcTitle,
                                    TurnNumber = i + 2
                                };

                                ThAutomationParameter.ThPbsCreateDto = dynamicDto1;

                               var pbsId1 =  await ThPbsCreate(ThAutomationParameter);
                               ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId1;


                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, Loading);
                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel1);
                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel2);
                                ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, unload);
                            
                            
                                var truckRemainTime = (projectEndTime - travel2.EndTime).Value.TotalMinutes;
                                var truckTtimesx = truckTotalTime - await dur;
                                if (truckRemainTime < truckTotalTime)
                                {
                                    if (truckTtimesx <= truckRemainTime)
                                    {


                                        var Loadingx = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Loading",
                                            StartTime = travel2.EndTime,
                                            EndTime = travel2.EndTime.Value.AddMinutes(loadingTime),
                                            Type = "0",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        var travel1x = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "First Travel",
                                            StartTime = Loadingx.EndTime,
                                            // EndTime = Loading.EndTime.Value.AddMinutes(await duration),
                                            EndTime = Loadingx.EndTime.Value.AddMinutes(await dur),
                                            Type = "1",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        var unloadx = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Unloading",
                                            StartTime = travel1x.EndTime,
                                            EndTime = travel1x.EndTime.Value.AddMinutes(unloadingTime),
                                            Type = "2",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        var travel2x = new ThTrucksSchedule
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                                            Title = "Second Travel",
                                            StartTime = unloadx.EndTime,
                                            // EndTime = unload.EndTime?.AddMinutes(await duration),
                                            EndTime = unloadx.EndTime?.AddMinutes(await dur),
                                            Type = "3",
                                            TurnNumber = travel2.TurnNumber + 1,
                                            TruckOrder = travel2.TruckOrder,
                                            LoadingNumber = travel2.TurnNumber + 1
                                        };

                                        endTimeTruck = travel2.EndTime;

                                        await connection.ExecuteAsync(query, Loadingx);
                                        await connection.ExecuteAsync(query, travel1x);
                                        await connection.ExecuteAsync(query, travel2x);
                                        await connection.ExecuteAsync(query, unloadx);

                                        var dynamicDto1x = new ThPbsCreateDto
                                        {
                                            Capacity = truckSize,
                                            Velocity = velocity,
                                            StartDate = Loadingx.StartTime,
                                            EndDate = travel2x.EndTime,
                                            CpcTitle = cpcTitle,
                                            TurnNumber = travel2.TurnNumber + 1
                                        };

                                        ThAutomationParameter.ThPbsCreateDto = dynamicDto1x;

                                       var pbsId2 =  await ThPbsCreate(ThAutomationParameter);
                                       ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId2;
                                       
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, Loadingx);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel1x);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, travel2x);
                                        ThPmolCreate(ThAutomationParameter, projectSequenceId, cpcTitle, unloadx);
                                    }
                                }
                            }

                            //}

                        }
                        
                    }
                }
            }
        }
        else
        {
            if (ThAutomationParameter.GetThProductWithTrucksDto.Type == "14")
            {
                var query =
                    @"INSERT INTO dbo.ThTrucksSchedule ( Id ,ProductTruckId ,Title ,EndTime ,StartTime ,Type,TurnNumber,LoadingNumber,TruckOrder ) VALUES ( @Id ,@ProductTruckId ,@Title ,@EndTime ,@StartTime ,@Type,@TurnNumber,@LoadingNumber,@TruckOrder )";

                var truckResult = connection.Query<GetThTrucksSchedule>(
                    @"SELECT * FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id WHERE ThProductWithTrucks.ProductId = @Id AND ThProductWithTrucks.IsTruck = 1",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).ToList();

                var cpcTitle = cuConnection
                    .Query<string>("Select Title From CorporateProductCatalog Where Id = @Id",
                        new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();
                var truckSize1 = cuConnection.Query<float>("Select Size From CorporateProductCatalog Where Id = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();

                var unloadingTime1 = Math.Round(truckSize1 / velocity.ToDouble() * 60);

                var concreteCentralVelocity1 = cuConnection.Query<float>(
                        "Select Velocity From CPCVelocity Where CPCId = @Id",
                        new { Id = ThAutomationParameter.Configuration.GetValue<string>("CpcConcreteCentral") })
                    .FirstOrDefault();

               
                var maxTruck = truckResult.Where(x => x.Type == "2")
                    .MaxBy(c => c.TruckOrder);
                var maxTruckNumber = maxTruck != null ? maxTruck.TruckOrder : 0;

                var lastLoading = truckResult.Where(x => x.Type == "2" && x.TruckOrder == maxTruckNumber)
                    .MinBy(c => c.TurnNumber);
                
                var maxLoadingNumber = truckResult.FirstOrDefault() != null
                    ? truckResult.MaxBy(x => x.LoadingNumber)!.LoadingNumber
                    : 0;

                // var turnNumber  = truckResult.FirstOrDefault() != null
                //     ? truckResult.MaxBy(x => x.TurnNumber)!.TurnNumber
                //     : 0;
                var turn = truckResult.Where(x => x.ProductTruckId == isExist.FirstOrDefault().Id && x.Type == "3")
                    .MaxBy(c => c.TurnNumber);
                var turnNumber = turn?.TurnNumber;
                
                var loadingTime1 = Math.Round(truckSize1 / concreteCentralVelocity1 * 60);


                // var tt = DateTime.ParseExact(ThAutomationParameter.GetThProductWithTrucksDto.STime, "HH:mm",
                //     CultureInfo.InvariantCulture);
                //
                // var unloadTime = ThAutomationParameter.GetThProductWithTrucksDto.Date.AddHours(tt.Hour)
                //     .AddMinutes(tt.Minute);

                

                // var projectSequenceId1 = connection
                //     .Query<string>("Select ProjectSequenceCode From PbsProduct Where Id = @Id",
                //         new { Id = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).FirstOrDefault();
                //

                var projectSequenceId1 = ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode;


                var mapLocation1 = dbconnection
                    .Query<Position>(
                        "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                        new { SequenceCode = projectSequenceId1 }).FirstOrDefault();
                if (mapLocation1 != null)
                {
                    var orgLat = ThAutomationParameter.Configuration.GetValue<string>("HQLat");
                    var orgLong = ThAutomationParameter.Configuration.GetValue<string>("HQLong");
                    
                    var dur = ThAutomationParameter._iShiftRepository.CalculateDistance(
                        mapLocation1.Lat.ToDouble(), mapLocation1.Lon.ToDouble(),
                        orgLat.ToDouble(), orgLong.ToDouble(), ThAutomationParameter.TenantProvider,
                        ThAutomationParameter.Configuration, true);

                    if (await dur != 0)
                    {
                        var truckLoadingParam = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = isExist.FirstOrDefault().Id,
                            Title = "Loading",
                            StartTime = turn?.EndTime,
                            EndTime = turn?.EndTime?.AddMinutes(loadingTime1),
                            Type = "0",
                            TurnNumber = turnNumber + 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };
                        var truckTravel1Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = isExist.FirstOrDefault().Id,
                            Title = "First Travel",
                            // StartTime = projectStartTime.AddMinutes(-await duration),
                            StartTime = truckLoadingParam.EndTime,
                            EndTime = truckLoadingParam.StartTime?.AddMinutes(await dur),
                            Type = "1",
                            TurnNumber = turnNumber + 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        var truckUnloadParam1 = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = isExist.FirstOrDefault().Id,
                            Title = "Unloading",
                            StartTime = truckTravel1Param?.EndTime,
                            EndTime = truckTravel1Param?.EndTime?.AddMinutes(unloadingTime1),
                            Type = "2",
                            TurnNumber = turnNumber + 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };
                        var truckTravel2Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = isExist.FirstOrDefault().Id,
                            Title = "Second Travel",
                            StartTime = truckUnloadParam1.EndTime,
                            // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                            EndTime = truckUnloadParam1.EndTime?.AddMinutes(await dur),
                            Type = "3",
                            TurnNumber = turnNumber + 1,
                            TruckOrder = maxTruckNumber + 1,
                            LoadingNumber = maxLoadingNumber + 1
                        };

                        //var endTimeTruck = truckTravel2Param.EndTime;

                        

                        await connection.ExecuteAsync(query, truckUnloadParam1);
                        await connection.ExecuteAsync(query, truckTravel1Param);
                        await connection.ExecuteAsync(query, truckTravel2Param);
                        await connection.ExecuteAsync(query, truckLoadingParam);

                        var dynamicDto = new ThPbsCreateDto
                        {
                            Capacity = truckSize1,
                            Velocity = velocity,
                            StartDate = truckLoadingParam.StartTime,
                            EndDate = truckTravel2Param.EndTime,
                            CpcTitle = cpcTitle,
                            TurnNumber = 1
                        };

                        ThAutomationParameter.ThPbsCreateDto = dynamicDto;

                      var pbsId =  await ThPbsCreate(ThAutomationParameter);
                      ThAutomationParameter.GetThProductWithTrucksDto.ProductId = pbsId;


                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel1Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel2Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckLoadingParam);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckUnloadParam1);
                    }
                }
            }
        }

        return ThAutomationParameter.GetThProductWithTrucksDto;
    }

    public async Task<List<GetThTrucksSchedule>> GetTruckAssignDataForMyEnv(ThAutomationParameter ThAutomationParameter)
    {

        var connectionString = ConnectionString.MapConnectionString("COM-0001",
                ThAutomationParameter.TruckAssignDto.ProjectSequenceCode, ThAutomationParameter.TenantProvider);

            var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
                null, ThAutomationParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            await using var cuConnection = new SqlConnection(cuConnectionString);
            
            await using var dbconnection =
                new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

            List<GetThTrucksSchedule> result;
            var mixerResult = new List<GetThTrucksSchedule>();

            var pbsDynamicAttributes = connection
                .Query<GetPbsDynamicAttributes>(
                    "Select PbsDynamicAttributes.*,PbsProduct.StartDate from PbsDynamicAttributes Left outer join PbsProduct on PbsDynamicAttributes.ProductId = PbsProduct.Id  Where PbsDynamicAttributes.ProductId = @ProductId",
                    new { ProductId = ThAutomationParameter.TruckAssignDto.Id }).ToList();

            var projectStartTime = DateTime.Parse(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime")
                .StartDate
                .ToShortDateString());
            var projectEndTime = DateTime.Parse(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").StartDate
                .ToShortDateString());
            var projectSTime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").Value,
                "HH:mm", CultureInfo.InvariantCulture);
            projectStartTime = projectStartTime.AddHours(projectSTime.Hour)
                .AddMinutes(projectSTime.Minute);
            var projectETime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "EndTime").Value,
                "HH:mm", CultureInfo.InvariantCulture);
            projectEndTime = projectEndTime.AddHours(projectETime.Hour).AddMinutes(projectETime.Minute);

            var cpcList = cuConnection.Query<CorporateProductCatalog>("Select * From CorporateProductCatalog").ToList();

            result = connection.Query<GetThTrucksSchedule>(
                @"SELECT ThTrucksSchedule.*,ThProductWithTrucks.CpcId,ThProductWithTrucks.IsTruck FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id
                WHERE ThProductWithTrucks.ProductId = @Id AND ThProductWithTrucks.IsTruck = 1 ",
                new { ThAutomationParameter.TruckAssignDto.Id }).ToList();
            
            var pumpData = connection.Query<GetThTrucksSchedule>(
                @"SELECT ThTrucksSchedule.*,ThProductWithTrucks.CpcId,ThProductWithTrucks.IsTruck FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id
                WHERE ThProductWithTrucks.ProductId = @Id AND ThProductWithTrucks.IsTruck = 0",
                new { ThAutomationParameter.TruckAssignDto.Id }).ToList();


            if (!ThAutomationParameter.TruckAssignDto.IsPmol)
            {
                var allTruckdata = new List<GetThTrucksSchedule>();

                var pbsTruckData = result.GroupBy(x => x.ProductTruckId);

                //foreach (var pbsTrucks in pbsTruckData)
                Parallel.ForEach(pbsTruckData, pbsTrucks =>
                {

                
                // foreach (var turn in pbsTrucks.GroupBy(c => c.TurnNumber))
                // {
                Parallel.ForEach(pbsTrucks.GroupBy(c => c.TurnNumber), turn =>
                {
                     using var cuConnection1 = new SqlConnection(cuConnectionString);

                    var truckAssign = new GetThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = pbsTrucks.Key,
                        Title = "Turn",
                        StartTime = turn.FirstOrDefault(x => x.Type == "0")?.StartTime,
                        EndTime = turn.FirstOrDefault(x => x.Type == "3")?.EndTime,
                        STime = turn.FirstOrDefault(x => x.Type == "0")?.StartTime?.ToShortTimeString(),
                        ETime = turn.FirstOrDefault(x => x.Type == "3")?.EndTime?.ToShortTimeString(),
                        Type = "10",
                        TurnNumber = turn.Key,
                        CpcId = turn.FirstOrDefault()?.CpcId,
                        Truck = cpcList.FirstOrDefault(x => x.Id == turn.FirstOrDefault()?.CpcId).ResourceTitle,
                        Size = cpcList.FirstOrDefault(x => x.Id == turn.FirstOrDefault()?.CpcId).Size,
                        LoadingNumber = turn.FirstOrDefault()?.LoadingNumber,
                        TruckOrder = turn.FirstOrDefault()?.TruckOrder
                    };

                    var timeSlots = cuConnection1
                        .Query<TimeSlots>(
                            @"SELECT ThTruckAvailability.STime, ThTruckAvailability.ETime FROM dbo.ThTruckAvailability LEFT OUTER JOIN StockHeader ON ThTruckAvailability.StockId = StockHeader.Id WHERE StockHeader.CPCId = @CPCId AND Date = @Date AND ThTruckAvailability.Availability = 1 ORDER BY SortingOrder ASC",
                            new { CPCId = truckAssign.CpcId, Date = truckAssign.StartTime?.Date })
                        .ToList();


                    if (timeSlots.Any())
                    {
                        var arry = new List<int>();


                        // foreach (var timeSlot in timeSlots)
                        // {
                        Parallel.ForEach(timeSlots, timeSlot =>
                        {
                            var itemSTime = DateTime.ParseExact(timeSlot.STime, "HH:mm", CultureInfo.InvariantCulture);
                            var itemETime = DateTime.ParseExact(timeSlot.ETime, "HH:mm", CultureInfo.InvariantCulture);

                            var time1 = itemSTime.TimeOfDay;
                            var time2 = itemETime.TimeOfDay;

                            var time3 = truckAssign.StartTime?.TimeOfDay;
                            var time4 = truckAssign.EndTime?.TimeOfDay;

                            if (!(time3 >= time1 && time4 <= time2))
                            {

                                arry.Add(1);

                            }
                            else
                            {
                                arry.Add(0);

                            }


                        });

                        truckAssign.IsError = arry.All(x => x != 0);
                        truckAssign.ErrorMessage = ThAutomationParameter.Lang == "en"
                            ? "Time Slot Not Available"
                            : "Time Slot Not Available(nl)";
                    }

                     using var dbconnection1 =
                        new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);
                     
                    var projects = dbconnection1.Query<ProjectDefinition>(
                        "SELECT ProjectDefinition.* FROM dbo.ThCustomerOrganizations INNER  JOIN ProjectDefinition ON ThCustomerOrganizations.ProjectId = ProjectDefinition.Id AND ProjectDefinition.IsDeleted = 0");


                    Parallel.ForEach(projects.DistinctBy(x => x.SequenceCode), project =>
                    {

                        var projectconnectionString = ConnectionString.MapConnectionString("COM-0001",
                            project.SequenceCode, ThAutomationParameter.TenantProvider);

                        using var projectconnection = new SqlConnection(projectconnectionString);

                        var truckProject = projectconnection.Query<GetThTrucksSchedule>(
                                @"SELECT ThTrucksSchedule.*,ThProductWithTrucks.CpcId,ThProductWithTrucks.IsTruck FROM ThTrucksSchedule LEFT OUTER JOIN ThProductWithTrucks ON ThTrucksSchedule.ProductTruckId = ThProductWithTrucks.Id
                                     WHERE ThProductWithTrucks.Date = @Date  AND ThProductWithTrucks.ProductId != @Id",
                                new
 {
                                    Date = truckAssign.StartTime?.Date, Id = ThAutomationParameter.TruckAssignDto.Id
                                })
                            .ToList();

                        allTruckdata.AddRange(truckProject);

                    });

                    // foreach (var data in allTruckdata.Where(v => v.CpcId == truckAssign.CpcId)
                    //              .GroupBy(x => x.ProductTruckId))
                    // {
                    Parallel.ForEach(
                        allTruckdata.Where(v => v.CpcId == truckAssign.CpcId).GroupBy(x => x.ProductTruckId), data =>
                        {
                            // foreach (var k in data.GroupBy(v => v.TurnNumber))
                            // {
                            Parallel.ForEach(data.GroupBy(v => v.TurnNumber), k =>
                            {
                                var truckdata = new GetThTrucksSchedule
                                {
                                    StartTime = k.FirstOrDefault(x => x.Type == "0")?.StartTime,
                                    EndTime = k.FirstOrDefault(x => x.Type == "3")?.EndTime

                                };

                                // if(truckAssign.StartTime >  truckdata.StartTime && truckAssign.StartTime < truckdata.EndTime)
                                // {
                                //     truckAssign.IsError = true;
                                //     truckAssign.ErrorMessage = ThAutomationParameter.Lang == "en" ? "Time Slot Conflicts" : "Time Slot Conflicts(nl)";
                                //
                                //     
                                // }else if(truckAssign.EndTime >  truckdata.StartTime && truckAssign.EndTime < truckdata.EndTime)
                                // {
                                //     truckAssign.IsError = true;
                                //     truckAssign.ErrorMessage = ThAutomationParameter.Lang == "en" ? "Time Slot Conflicts" : "Time Slot Conflicts(nl)";
                                //
                                // }
                                if (truckAssign.StartTime < truckdata.EndTime &&
                                    truckAssign.EndTime > truckdata.StartTime)
                                {
                                    truckAssign.IsError = true;
                                    truckAssign.ErrorMessage = ThAutomationParameter.Lang == "en"
                                        ? "Time Slot Conflicts"
                                        : "Time Slot Conflicts(nl)";


                                }


                            });
                        });


                    mixerResult.Add(truckAssign);
                });

                });

                var freeTimes = result.Where(c => c.Type == "2").OrderBy(x => x.StartTime).ToList();
                var totalFreeTimes = new List<GetThTrucksSchedule>();

                if (freeTimes.Any())
                {
                    var currentTime = projectStartTime;

                    // foreach (var times in freeTimes)
                    // {
                    Parallel.ForEach(freeTimes, times =>
                    {
                        times.STime = times.StartTime?.ToShortTimeString();
                        times.ETime = times.EndTime?.ToShortTimeString();
                        times.Type = "12";
                        times.ProductTruckId = "22222";

                        if (times.EndTime <= projectEndTime)
                        {


                            if (currentTime < times.StartTime)
                            {
                                var tt = new GetThTrucksSchedule
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductTruckId = "22222",
                                    Title = "Not Unloading",
                                    StartTime = currentTime,
                                    EndTime = times.StartTime,
                                    STime = currentTime.ToShortTimeString(),
                                    ETime = times.StartTime?.ToShortTimeString(),
                                    Type = "14"
                                };
                                totalFreeTimes.Add(tt);
                            }

                            currentTime = times.EndTime.Value;
                        }
                        else
                        {
                            if (times.StartTime < projectEndTime)
                            {
                                times.ETime = projectEndTime.ToShortTimeString();
                                times.EndTime = projectEndTime;
                            }


                            if (currentTime < times.StartTime)
                            {
                                if (times.StartTime < projectEndTime)
                                {
                                    var tt = new GetThTrucksSchedule
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProductTruckId = "22222",
                                        Title = "Not Unloading",
                                        StartTime = currentTime,
                                        EndTime = times.StartTime,
                                        STime = currentTime.ToShortTimeString(),
                                        ETime = times.StartTime?.ToShortTimeString(),
                                        Type = "14"
                                    };
                                    totalFreeTimes.Add(tt);
                                }
                                else
                                {
                                    var tt = new GetThTrucksSchedule
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProductTruckId = "22222",
                                        Title = "Not Unloading",
                                        StartTime = currentTime,
                                        EndTime = projectEndTime,
                                        STime = currentTime.ToShortTimeString(),
                                        ETime = projectEndTime.ToShortTimeString(),
                                        Type = "14"
                                    };
                                    totalFreeTimes.Add(tt);
                                }

                            }

                            currentTime = projectEndTime;
                        }
                    });

                    if (currentTime < projectEndTime)
                    {
                        var tt = new GetThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = "22222",
                            Title = "Not Unloading",
                            StartTime = currentTime,
                            EndTime = projectEndTime,
                            STime = currentTime.ToShortTimeString(),
                            ETime = projectEndTime.ToShortTimeString(),
                            Type = "14"
                        };
                        totalFreeTimes.Add(tt);
                    }
                    
                    
                }
                else
                {
                    var tt = new GetThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = "22222",
                        Title = "Not Unloading",
                        StartTime = projectStartTime,
                        EndTime = projectEndTime,
                        STime = projectStartTime.ToShortTimeString(),
                        ETime = projectEndTime.ToShortTimeString(),
                        Type = "14"
                    };
                    totalFreeTimes.Add(tt);
                }

                mixerResult.AddRange(freeTimes);

                mixerResult.AddRange(totalFreeTimes);

                var mixerTotal = new GetThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = "22222",
                    Title = "Total",
                    StartTime = projectStartTime.AddMinutes(-60),
                    EndTime = projectStartTime.AddMinutes(-10),
                    STime = projectStartTime.AddMinutes(-60).ToShortTimeString(),
                    ETime = projectStartTime.AddMinutes(-10).ToShortTimeString(),
                    Type = "13",
                    Size = mixerResult.Sum(v => v.Size),
                    Capacity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Capacity").Value
                };
                mixerResult.Add(mixerTotal);


                if (pumpData.Any())
                {
                    
                    var pumpAssign = new GetThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = pumpData.FirstOrDefault()?.ProductTruckId,
                        Title = "Turn",
                        StartTime = pumpData.FirstOrDefault(x => x.Type == "1")?.StartTime,
                        EndTime = pumpData.FirstOrDefault(x => x.Type == "3")?.EndTime,
                        STime = pumpData.FirstOrDefault(x => x.Type == "1")?.StartTime?.ToShortTimeString(),
                        ETime = pumpData.FirstOrDefault(x => x.Type == "3")?.EndTime?.ToShortTimeString(),
                        Type = "15",
                        TurnNumber = pumpData.FirstOrDefault()?.TurnNumber,
                        CpcId = pumpData.FirstOrDefault()?.CpcId,
                        Truck = cpcList.FirstOrDefault(x => x.Id == pumpData.FirstOrDefault()?.CpcId)?.ResourceTitle,
                        Size = cpcList.FirstOrDefault(x => x.Id == pumpData.FirstOrDefault()?.CpcId)?.Size,
                        LoadingNumber = pumpData.FirstOrDefault()?.LoadingNumber,
                        TruckOrder = pumpData.FirstOrDefault()?.TruckOrder
                    };
                    
                     var timeSlots = cuConnection
                        .Query<TimeSlots>(
                            @"SELECT ThTruckAvailability.STime, ThTruckAvailability.ETime FROM dbo.ThTruckAvailability LEFT OUTER JOIN StockHeader ON ThTruckAvailability.StockId = StockHeader.Id WHERE StockHeader.CPCId = @CPCId AND Date = @Date AND ThTruckAvailability.Availability = 1 ORDER BY SortingOrder ASC", new{CPCId = pumpAssign.CpcId,Date = pumpAssign.StartTime?.Date})
                        .ToList();
                    

                    if (timeSlots.Any())
                    {
                        var arry = new List<int>();


                        // foreach (var timeSlot in timeSlots)
                        // {
                        Parallel.ForEach(timeSlots, timeSlot =>
                        {
                            var itemSTime = DateTime.ParseExact(timeSlot.STime, "HH:mm", CultureInfo.InvariantCulture);
                            var itemETime = DateTime.ParseExact(timeSlot.ETime, "HH:mm", CultureInfo.InvariantCulture);

                            var time1 = itemSTime.TimeOfDay;
                            var time2 = itemETime.TimeOfDay;

                            var time3 = pumpAssign.StartTime?.TimeOfDay;
                            var time4 = pumpAssign.EndTime?.TimeOfDay;

                            if (!(time3 >= time1 && time4 <= time2))
                            {

                                arry.Add(1);

                            }
                            else
                            {
                                arry.Add(0);

                            }


                        });

                        pumpAssign.IsError = arry.All(x => x != 0);
                        pumpAssign.ErrorMessage = ThAutomationParameter.Lang == "en" ? "Time Slot Not Available" : "Time Slot Not Available(nl)";
                    }

                    // foreach (var data in allTruckdata.Where(v => v.CpcId == pumpAssign.CpcId)
                    //              .GroupBy(x => x.ProductTruckId))
                    // {
                    Parallel.ForEach(allTruckdata.Where(v => v.CpcId == pumpAssign.CpcId)
                        .GroupBy(x => x.ProductTruckId), data =>
                    {


                        // foreach (var k in data.GroupBy(v => v.TurnNumber))
                        // {
                        Parallel.ForEach(data.GroupBy(v => v.TurnNumber), k =>
                        {

                            var truckdata = new GetThTrucksSchedule
                            {
                                StartTime = k.FirstOrDefault(x => x.Type == "1")?.StartTime,
                                EndTime = k.FirstOrDefault(x => x.Type == "3")?.EndTime

                            };

                            // if (pumpAssign.StartTime > truckdata.StartTime && pumpAssign.StartTime < truckdata.EndTime)
                            // {
                            //     pumpAssign.IsError = true;
                            //     pumpAssign.ErrorMessage = ThAutomationParameter.Lang == "en"
                            //         ? "Time Slot Conflicts"
                            //         : "Time Slot Conflicts(nl)";
                            //
                            //
                            // }
                            // else if (pumpAssign.EndTime > truckdata.StartTime && pumpAssign.EndTime < truckdata.EndTime)
                            // {
                            //     pumpAssign.IsError = true;
                            //     pumpAssign.ErrorMessage = ThAutomationParameter.Lang == "en"
                            //         ? "Time Slot Conflicts"
                            //         : "Time Slot Conflicts(nl)";
                            //
                            // }

                            if (pumpAssign.StartTime < truckdata.EndTime && pumpAssign.EndTime > truckdata.StartTime)
                            {
                                pumpAssign.IsError = true;
                                pumpAssign.ErrorMessage = ThAutomationParameter.Lang == "en"
                                    ? "Time Slot Conflicts"
                                    : "Time Slot Conflicts(nl)";


                            }



                        });
                    });

                    mixerResult.Add(pumpAssign);
                }

                //result.AddRange(mixerResult);
            }
            else
            {
                // foreach (var item in result)
                // {
                Parallel.ForEach(result, item =>
                {
                    item.STime = item.StartTime?.ToShortTimeString();
                    item.ETime = item.EndTime?.ToShortTimeString();
                    item.Truck = cpcList.FirstOrDefault(x => x.Id == item.CpcId).ResourceTitle;
                    item.Size = cpcList.FirstOrDefault(x => x.Id == item.CpcId).Size;
                    if (item.Type == "2")
                    {
                        var mixer = new GetThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = "22222",
                            Title = "Unloading",
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            STime = item.StartTime?.ToShortTimeString(),
                            ETime = item.EndTime?.ToShortTimeString(),
                            Type = "12"
                        };
                        mixerResult.Add(mixer);
                    }
                });

                var freeTimes = mixerResult.OrderBy(x => x.StartTime).ToList();
                var totalFreeTimes = new List<GetThTrucksSchedule>();

                if (freeTimes.Any())
                {
                    var currentTime = projectStartTime;

                    // foreach (var times in freeTimes.Where(x => x.EndTime <= projectEndTime))
                    // {
                    Parallel.ForEach(freeTimes.Where(x => x.EndTime <= projectEndTime), times =>
                    {
                        if (currentTime < times.StartTime)
                        {
                            var tt = new GetThTrucksSchedule
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductTruckId = "22222",
                                Title = "Not Unloading",
                                StartTime = currentTime,
                                EndTime = times.StartTime,
                                STime = currentTime.ToShortTimeString(),
                                ETime = times.StartTime?.ToShortTimeString(),
                                Type = "14"
                            };
                            totalFreeTimes.Add(tt);
                        }

                        currentTime = times.EndTime.Value;
                    });

                    var exceed = freeTimes.FirstOrDefault(x => x.EndTime > projectEndTime);
                    if (exceed != null)
                    {
                        if (exceed.StartTime < projectEndTime)
                        {
                            exceed.EndTime = projectEndTime;
                            exceed.ETime = projectEndTime.ToShortTimeString();
                        }
                       
                        
                    }
                    if (currentTime < projectEndTime)
                    {
                        var tt = new GetThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = "22222",
                            Title = "Not Unloading",
                            StartTime = currentTime,
                            EndTime = projectEndTime,
                            STime = currentTime.ToShortTimeString(),
                            ETime = projectEndTime.ToShortTimeString(),
                            Type = "14"
                        };
                        totalFreeTimes.Add(tt);
                    }
                    currentTime = projectEndTime;

                    
                }
                else
                {
                    var tt = new GetThTrucksSchedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductTruckId = "22222",
                        Title = "Not Unloading",
                        StartTime = projectStartTime,
                        EndTime = projectEndTime,
                        STime = projectStartTime.ToShortTimeString(),
                        ETime = projectEndTime.ToShortTimeString(),
                        Type = "14"
                    };
                    totalFreeTimes.Add(tt);
                }

                mixerResult.AddRange(totalFreeTimes);

                mixerResult.AddRange(result);

                var mixerTotal = new GetThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = "22222",
                    Title = "Total",
                    StartTime = projectStartTime.AddMinutes(-60),
                    EndTime = projectStartTime.AddMinutes(-10),
                    STime = projectStartTime.AddMinutes(-60).ToShortTimeString(),
                    ETime = projectStartTime.AddMinutes(-10).ToShortTimeString(),
                    Type = "13",
                    Size = result.Where(x => x.Type == "2").Sum(v => v.Size),
                    Capacity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Capacity")?.Value
                };
                mixerResult.Add(mixerTotal);
                
                // foreach (var item in pumpData)
                // {
                Parallel.ForEach(pumpData, item =>
                {
                    item.STime = item.StartTime?.ToShortTimeString();
                    item.ETime = item.EndTime?.ToShortTimeString();
                    item.Truck = cpcList.FirstOrDefault(x => x.Id == item.CpcId)?.ResourceTitle;
                    item.Size = cpcList.FirstOrDefault(x => x.Id == item.CpcId)?.Size;

                });
                
                mixerResult.AddRange(pumpData);

            }

            return mixerResult.OrderBy(x => x.TruckOrder).ToList();
       
    }


    public async Task<List<ThTruckWithProductData>> GetTruckWithProductForMyEnv(
        ThAutomationParameter ThAutomationParameter)
    {
        var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var dbConnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var ThTruckWithProductData = new List<ThTruckWithProductData>();

        // var cpc = cuConnection
        //     .Query<ThTruckWithProductData>(
        //         "SELECT Id,Title AS TruckTitle,Size AS TruckCapacity FROM dbo.CorporateProductCatalog WHERE ResourceFamilyId = '2210e768-human-kknk-truck-ee367a82ad17' ORDER BY TruckTitle ")
        //     .ToList();

        var cpc = cuConnection.Query<ThTruckWithProductData>(@"SELECT
                                                              ThTruckAvailability.SortingOrder
                                                             ,CorporateProductCatalog.Id
                                                             ,CorporateProductCatalog.Size AS TruckCapacity
                                                             ,CorporateProductCatalog.Title AS TruckTitle
                                                            FROM dbo.StockHeader
                                                            LEFT OUTER JOIN dbo.ThTruckAvailability
                                                              ON StockHeader.Id = ThTruckAvailability.StockId
                                                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                                                              ON CorporateProductCatalog.Id = StockHeader.CPCId WHERE ThTruckAvailability.Date = @Date AND CorporateProductCatalog.ResourceFamilyId = '2210e768-human-kknk-truck-ee367a82ad17'",
            new { ThAutomationParameter.ThTruckWithProductDto.Date }).ToList().DistinctBy(c => c.Id);


        var projects = dbConnection.Query<ProjectDefinition>(
            "SELECT ProjectDefinition.* FROM dbo.ThCustomerOrganizations INNER  JOIN ProjectDefinition ON ThCustomerOrganizations.ProjectId = ProjectDefinition.Id AND ProjectDefinition.IsDeleted = 0");


        foreach (var i in cpc)
        {
            var mProduct = new List<Product>();

            foreach (var project in projects.DistinctBy(x => x.SequenceCode))
            {
                await using var connection = new SqlConnection(project.ProjectConnectionString);

                var thProductWithTrucks = connection
                    .Query<ThProductWithTrucks>("SELECT * FROM ThProductWithTrucks WHERE Date = @Date AND IsTruck = 1",
                        new { ThAutomationParameter.ThTruckWithProductDto.Date }).ToList();

                var product = connection.Query<Product>("SELECT Id,Title AS ProductTitle FROM dbo.PbsProduct").ToList();

                var thTrucksSchedule =
                    connection.Query<ThTrucksSchedule>("SELECT * FROM dbo.ThTrucksSchedule").ToList();

                var productForTruck = thProductWithTrucks.Where(e => e.CpcId == i.Id).ToList();


                foreach (var n in productForTruck)
                {
                    var mthTrucksSchedule = thTrucksSchedule.Where(e => e.ProductTruckId == n.Id).ToList();

                    var nxProduct = product.FirstOrDefault(e => e.Id == n.ProductId);

                    foreach (var r in mthTrucksSchedule.GroupBy(c => c.TurnNumber))
                    {
                        var nProduct = new Product();
                        nProduct.Id = nxProduct.Id;
                        nProduct.ProductTitle = nxProduct.ProductTitle;
                        nProduct.StartTime = r.FirstOrDefault(x => x.Type == "0")?.StartTime;
                        nProduct.EndTime = r.FirstOrDefault(x => x.Type == "3")?.EndTime;

                        mProduct.Add(nProduct);
                    }
                }
            }

            var TimeSlots = cuConnection
                .Query<TimeSlots>(
                    @"SELECT ThTruckAvailability.STime, ThTruckAvailability.ETime FROM dbo.ThTruckAvailability LEFT OUTER JOIN StockHeader ON ThTruckAvailability.StockId = StockHeader.Id WHERE StockHeader.CPCId = @CPCId AND Date = @Date AND ThTruckAvailability.Availability = 1 ORDER BY SortingOrder ASC", new{CPCId = i.Id,Date = ThAutomationParameter.ThTruckWithProductDto.Date})
                .ToList();

            if (TimeSlots.Any())
            {
                TimeSlots = TimeSlots.OrderBy(x => x.STime.Replace(":", "").ToDouble()).ToList();

                i.TimeSlots = TimeSlots;
            }
            

            i.Product = mProduct;

            ThTruckWithProductData.Add(i);
        }


        return ThTruckWithProductData.OrderBy(x => x.SortingOrder).ToList();
    }
    
    public async Task<List<ThTruckWithProductData>> GetPumpsWithProductForMyEnv(
        ThAutomationParameter ThAutomationParameter)
    {
        var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var dbConnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var ThTruckWithProductData = new List<ThTruckWithProductData>();

        // var cpc = cuConnection
        //     .Query<ThTruckWithProductData>(
        //         "SELECT Id,Title AS TruckTitle,Size AS TruckCapacity FROM dbo.CorporateProductCatalog WHERE ResourceFamilyId = 'hjkl2c90-94f6-pump-8410-921a43c2lkjl' ORDER BY TruckTitle ")
        //     .ToList();

        var cpc = cuConnection.Query<ThTruckWithProductData>(@"SELECT
                                                              ThTruckAvailability.SortingOrder
                                                             ,CorporateProductCatalog.Id
                                                             ,CorporateProductCatalog.Size AS TruckCapacity
                                                             ,CorporateProductCatalog.Title AS TruckTitle
                                                            FROM dbo.StockHeader
                                                            LEFT OUTER JOIN dbo.ThTruckAvailability
                                                              ON StockHeader.Id = ThTruckAvailability.StockId
                                                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                                                              ON CorporateProductCatalog.Id = StockHeader.CPCId WHERE ThTruckAvailability.Date = @Date AND CorporateProductCatalog.ResourceFamilyId = 'hjkl2c90-94f6-pump-8410-921a43c2lkjl'",
            new { ThAutomationParameter.ThTruckWithProductDto.Date }).ToList().DistinctBy(c => c.Id);

        var projects = dbConnection.Query<ProjectDefinition>(
            "SELECT ProjectDefinition.* FROM dbo.ThCustomerOrganizations INNER  JOIN ProjectDefinition ON ThCustomerOrganizations.ProjectId = ProjectDefinition.Id AND ProjectDefinition.IsDeleted = 0");


        foreach (var i in cpc)
        {
            var mProduct = new List<Product>();

            foreach (var project in projects.DistinctBy(x => x.SequenceCode))
            {
                await using var connection = new SqlConnection(project.ProjectConnectionString);

                var thProductWithTrucks = connection
                    .Query<ThProductWithTrucks>("SELECT * FROM ThProductWithTrucks WHERE Date = @Date AND IsTruck = 0",
                        new { ThAutomationParameter.ThTruckWithProductDto.Date }).ToList();

                var product = connection.Query<Product>("SELECT Id,Title AS ProductTitle FROM dbo.PbsProduct").ToList();

                var thTrucksSchedule =
                    connection.Query<ThTrucksSchedule>("SELECT * FROM dbo.ThTrucksSchedule").ToList();

                var productForTruck = thProductWithTrucks.Where(e => e.CpcId == i.Id).ToList();


                foreach (var n in productForTruck)
                {
                    var mthTrucksSchedule = thTrucksSchedule.Where(e => e.ProductTruckId == n.Id).ToList();

                    var nxProduct = product.FirstOrDefault(e => e.Id == n.ProductId);

                    foreach (var r in mthTrucksSchedule.GroupBy(c => c.TurnNumber))
                    {
                        var nProduct = new Product();
                        nProduct.Id = nxProduct.Id;
                        nProduct.ProductTitle = nxProduct.ProductTitle;
                        nProduct.StartTime = r.FirstOrDefault(x => x.Type == "1")?.StartTime;
                        nProduct.EndTime = r.FirstOrDefault(x => x.Type == "3")?.EndTime;

                        mProduct.Add(nProduct);
                    }
                }
            }
            var TimeSlots = cuConnection
                .Query<TimeSlots>(
                    @"SELECT ThTruckAvailability.STime, ThTruckAvailability.ETime FROM dbo.ThTruckAvailability LEFT OUTER JOIN StockHeader ON ThTruckAvailability.StockId = StockHeader.Id WHERE StockHeader.CPCId = @CPCId AND Date = @Date AND ThTruckAvailability.Availability = 1 ORDER BY SortingOrder ASC", new{CPCId = i.Id,Date = ThAutomationParameter.ThTruckWithProductDto.Date})
                .ToList();

            if (TimeSlots.Any())
            {
                TimeSlots = TimeSlots.OrderBy(x => x.STime.Replace(":", "").ToDouble()).ToList();

                i.TimeSlots = TimeSlots;
            }
            
            i.Product = mProduct;

            ThTruckWithProductData.Add(i);
        }
        

        return ThTruckWithProductData.OrderBy(x => x.SortingOrder).ToList();

    }
    public async Task<GetThTrucksSchedule> UpdateThProduct(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            ThAutomationParameter.UpdateThProduct.ProjectSequenceCode, ThAutomationParameter.TenantProvider);

        await using var dbConnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

        await using var connection = new SqlConnection(connectionString);

        var updateQuery =
            "Update ThTrucksSchedule Set StartTime = @StartTime , EndTime = @EndTime Where Id = @Id ";

        var truckdata = connection
            .Query<GetThTrucksSchedule>(
                "Select * from ThTrucksSchedule Where ProductTruckId = @ProductTruckId AND TurnNumber = @TurnNumber",
                ThAutomationParameter.UpdateThProduct).ToList();


        var loading = truckdata.FirstOrDefault(x => x.Type == "0");
        var loadingtime = loading?.EndTime - loading?.StartTime;
        var loadingEndTime =
            ThAutomationParameter.UpdateThProduct.StartTime?.AddMinutes(loadingtime.Value.TotalMinutes);

        await connection.ExecuteAsync(updateQuery,
            new { ThAutomationParameter.UpdateThProduct.StartTime, EndTime = loadingEndTime, loading?.Id });

        var travel1 = truckdata.FirstOrDefault(x => x.Type == "1");
        var travelTme = travel1?.EndTime - travel1?.StartTime;
        var travelEndTime =
            loadingEndTime?.AddMinutes(travelTme.Value.TotalMinutes);

        await connection.ExecuteAsync(updateQuery,
            new { StartTime = loadingEndTime, EndTime = travelEndTime, travel1?.Id });

        var unloading = truckdata.FirstOrDefault(x => x.Type == "2");
        var unloadingTme = unloading?.EndTime - unloading?.StartTime;
        var unloadingEndTime =
            travelEndTime?.AddMinutes(unloadingTme.Value.TotalMinutes);

        await connection.ExecuteAsync(updateQuery,
            new { StartTime = travelEndTime, EndTime = unloadingEndTime, unloading?.Id });

        var travel2 = truckdata.FirstOrDefault(x => x.Type == "3");

        await connection.ExecuteAsync(updateQuery,
            new
            {
                StartTime = unloadingEndTime, EndTime = unloadingEndTime?.AddMinutes(travelTme.Value.TotalMinutes),
                travel2?.Id
            });

        return ThAutomationParameter.UpdateThProduct;
    }

    private async void ThPmolCreate(ThAutomationParameter ThAutomationParameter, string projectSequenceId1,
        string cpcTitle, ThTrucksSchedule truckParam)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        await using var dbconnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, ThAutomationParameter.TenantProvider);
        var idGenerator = new IdGenerator();

        var pmolQuery =
            @"INSERT INTO PMol (Id, ProjectMoleculeId, Name, ExecutionDate, IsDeleted, TypeId, StatusId, Title, BorId, LocationId, IsFinished, IsBreak, PmolType, ProductId, ProjectSequenceCode, ExecutionEndTime, ExecutionStartTime) VALUES
                                (@Id, @ProjectMoleculeId, @Name, @ExecutionDate, @IsDeleted, @TypeId, @StatusId, @Title, @BorId, @LocationId, @IsFinished, @IsBreak, @PmolType, @ProductId, @ProjectSequenceCode, @ExecutionEndTime, @ExecutionStartTime)";


        var locationId = dbconnection
            .Query<string>("Select LocationId From ProjectDefinition Where SequenceCode = @SequenceCode",
                new { SequenceCode = projectSequenceId1 }).FirstOrDefault();
        var borId = connection.Query<string>("Select Id From Bor Where PbsProductId = @PbsProductId",
            new { PbsProductId = ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).FirstOrDefault();

        var projectMoleculeId1 = idGenerator.GenerateId(applicationDbContext, "PMOL-", "PmolSequenceCode");
        var pmolName1 = cpcTitle + " " + truckParam.Title + " Turn " + truckParam.TurnNumber;
        var pmolParam1 = new
        {
            Id = Guid.NewGuid().ToString(),
            ProjectMoleculeId = projectMoleculeId1,
            Name = pmolName1,
            ExecutionDate = ThAutomationParameter.GetThProductWithTrucksDto.Date,
            IsDeleted = false,
            TypeId = "5bb656-f708-4a0d-9973-3d834ffe757d01",
            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            Title = projectMoleculeId1 + " - " + pmolName1,
            BorId = borId,
            LocationId = locationId,
            IsFinished = false,
            IsBreak = false,
            PmolType = "regular",
            ThAutomationParameter.GetThProductWithTrucksDto.ProductId,
            ProjectSequenceCode = projectSequenceId1,
            ExecutionEndTime = truckParam.EndTime?.ToString("HH:mm"),
            ExecutionStartTime = truckParam.StartTime?.ToString("HH:mm")
        };

        await connection.ExecuteAsync(pmolQuery, pmolParam1);
    }

    private static async Task<string> ShortUrl(string url)
    {
        var baseURL = "http://tinyurl.com/";
        var ssUrl = "api-create.php?url=" + url;


        var client = new HttpClient();
        client.BaseAddress = new Uri(baseURL);
        var response = await client.GetAsync(ssUrl);

        var jsonString = await response.Content.ReadAsStringAsync();


        return jsonString;
    }
    
    private static string ShortUrlIdCreate(string url)
    {
        using SHA256 sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(url));
        var hash = Convert.ToBase64String(bytes);

        // Remove special characters from the hash
        hash = hash.Replace("/", "").Replace("+", "").Replace("=", "");

        // Use the first 8 characters of the hash as the short URL
        var shortUrl = hash.Substring(0, 8);

        return shortUrl;
    }

    private  async Task<string> ThPbsCreate(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var pbsParameters = new PbsParameters
        {
            ProjectSequenceId = ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode,
            ContractingUnitSequenceId = "COM-0001",
            Lang = ThAutomationParameter.Lang,
            ContextAccessor = ThAutomationParameter.ContextAccessor,
            TenantProvider = ThAutomationParameter.TenantProvider,
            ChangedUser = new ApplicationUser
            {
                OId = ThAutomationParameter.UserId
            }
        };

        var dtoNew = new PbsProductCreateDto
        {
            Id = Guid.NewGuid().ToString(),
            ParentId = ThAutomationParameter.GetThProductWithTrucksDto.ProductId,
            Name = ThAutomationParameter.ThPbsCreateDto.CpcTitle + " Turn - " + 1,
            PbsType = "regular",
            PbsProductItemTypeId = "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
            Scope = "0",
            PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
            PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
            Contract = "ThAutomation",
            StartDate = ThAutomationParameter.ThPbsCreateDto.StartDate,
            EndDate = ThAutomationParameter.ThPbsCreateDto.EndDate
        };
        pbsParameters.PbsDto = dtoNew;

        await ThAutomationParameter.iPbsRepository.CreatePbs(pbsParameters).ConfigureAwait(false);

        var dynamicAttribute1 = new PbsDynamicAttributes
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = dtoNew.Id,
            Key = "Velocity",
            Value = ThAutomationParameter.ThPbsCreateDto.Velocity
        };
        var dynamicAttribute2 = new PbsDynamicAttributes
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = dtoNew.Id,
            Key = "CPC",
            Value = ThAutomationParameter.GetThProductWithTrucksDto.CpcId
        };
        var dynamicAttribute3 = new PbsDynamicAttributes
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = dtoNew.Id,
            Key = "Capacity",
            Value = ThAutomationParameter.ThPbsCreateDto.Capacity.ToString()
        };
        var dynamicAttribute4 = new PbsDynamicAttributes
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = dtoNew.Id,
            Key = "StartTime",
            Value = ThAutomationParameter.ThPbsCreateDto.StartDate?.ToString("HH:mm")
        };
        var dynamicAttribute5 = new PbsDynamicAttributes
        {
            Id = Guid.NewGuid().ToString(),
            ProductId = dtoNew.Id,
            Key = "EndTime",
            Value = ThAutomationParameter.ThPbsCreateDto.EndDate?.ToString("HH:mm")
        };

        var dynamicInsert =
            "INSERT INTO dbo.PbsDynamicAttributes (Id, ProductId, [Key], Value) VALUES (@Id, @ProductId, @Key, @Value)";

        await connection.ExecuteAsync(dynamicInsert, dynamicAttribute1);
        await connection.ExecuteAsync(dynamicInsert, dynamicAttribute2);
        await connection.ExecuteAsync(dynamicInsert, dynamicAttribute3);
        await connection.ExecuteAsync(dynamicInsert, dynamicAttribute4);
        await connection.ExecuteAsync(dynamicInsert, dynamicAttribute5);

        return dtoNew.Id;
    }

    private static async Task<string> CopyCpcCuToProject(ThAutomationParameter poParameter, string cpcId,
        string connectionString, string Environment)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        bool isCpcExist;
        var parameter = new CpcParameters();
        parameter.Lang = poParameter.Lang;

        using (var context = new ShanukaDbContext(options, connectionString, poParameter.TenantProvider))
        {
            isCpcExist = context.CorporateProductCatalog.Any(c => c.Id == cpcId);
        }

        if (isCpcExist == false)
        {
            parameter.Id = cpcId;
            if (Environment == "cu") parameter.ContractingUnitSequenceId = "COM-0001";

            if (Environment == "project")
            {
                parameter.ContractingUnitSequenceId = poParameter.ContractingUnitSequenceId;
                parameter.ProjectSequenceId = poParameter.ProjectSequenceId;
            }


            parameter.ContextAccessor = poParameter.ContextAccessor;
            parameter.TenantProvider = poParameter.TenantProvider;
            var _ICoporateProductCatalogRepository = new CoporateProductCatalogRepository();

            var cpc = await _ICoporateProductCatalogRepository.GetCorporateProductCatalogById(parameter);

            var cpcCreateDto = new CoperateProductCatalogCreateDto();
            if (cpc.CpcBasicUnitOfMeasure != null) cpcCreateDto.CpcBasicUnitOfMeasureId = cpc.CpcBasicUnitOfMeasure.Key;

            if (cpc.CpcBrand != null) cpcCreateDto.CpcBrandId = cpc.CpcBrand.Key;

            if (cpc.CpcPressureClass != null) cpcCreateDto.CpcPressureClassId = cpc.CpcPressureClass.Key;

            if (cpc.ResourceFamily != null) cpcCreateDto.ResourceFamilyId = cpc.ResourceFamily.Key;

            if (cpc.CpcUnitOfSizeMeasure != null) cpcCreateDto.CpcUnitOfSizeMeasureId = cpc.CpcUnitOfSizeMeasure.Key;

            cpcCreateDto.CpcMaterialId = cpc.CpcMaterialId;
            cpcCreateDto.Id = poParameter.GetThProductWithTrucksDto.CpcId;
            cpcCreateDto.CpcId = cpc.Id;
            cpcCreateDto.InventoryPrice = cpc.InventoryPrice;
            cpcCreateDto.MaxOrderQuantity = cpc.MaxOrderQuantity;
            cpcCreateDto.MinOrderQuantity = cpc.MinOrderQuantity;

            cpcCreateDto.ResourceNumber = cpc.ResourceNumber;
            cpcCreateDto.ResourceTitle = cpc.ResourceTitle;
            cpcCreateDto.ResourceTypeId = cpc.ResourceType.key;
            cpcCreateDto.Size = cpc.Size;
            cpcCreateDto.Status = cpc.Status;
            cpcCreateDto.WallThickness = cpc.WallThickness;

            var resouceList = new List<CpcResourceNicknameCreateDto>();
            foreach (var resource in cpc.CpcResourceNickname)
            {
                var dto = new CpcResourceNicknameCreateDto();
                dto.CoperateProductCatalogId = resource.CoperateProductCatalogId;
                dto.Id = null;
                dto.Language = resource.Language;
                dto.LocaleCode = resource.LocaleCode;
                dto.NickName = resource.NickName;
                resouceList.Add(dto);
            }

            cpcCreateDto.CpcResourceNickname = resouceList;

            var imageList = new List<CpcImageCreateDto>();
            foreach (var image in cpc.CpcImage)
            {
                var dto = new CpcImageCreateDto();
                dto.Id = null;
                dto.Image = image.Image;
                imageList.Add(dto);
            }

            cpcCreateDto.CpcImage = imageList;

            var vendorList = new List<CpcVendorCreateDto>();
            foreach (var vendor in cpc.CpcVendor)
            {
                var dto = new CpcVendorCreateDto();
                dto.CompanyId = vendor.CompanyId;
                dto.CompanyName = vendor.Company.Name;
                dto.CoperateProductCatalogId = vendor.CoperateProductCatalogId;
                dto.Id = null;
                dto.MaxOrderQuantity = vendor.MaxOrderQuantity;
                dto.MinOrderQuantity = vendor.MinOrderQuantity;
                dto.PreferredParty = vendor.PreferredParty;
                dto.PurchasingUnit = vendor.PurchasingUnit;
                dto.ResourceLeadTime = vendor.ResourceLeadTime;
                dto.ResourceNumber = vendor.ResourceNumber;
                dto.ResourcePrice = vendor.ResourcePrice;
                dto.ResourceTitle = vendor.ResourceTitle;
                dto.RoundingValue = vendor.RoundingValue;
                vendorList.Add(dto);
            }

            cpcCreateDto.CpcVendor = vendorList;

            parameter.CpcDto = cpcCreateDto;
            parameter.isCopy = true;
            if (Environment == "cu")
                parameter.ProjectSequenceId = poParameter.GetThProductWithTrucksDto.ProjectSequenceCode;

            if (Environment == "project") parameter.ProjectSequenceId = null;

            parameter.ContractingUnitSequenceId = "COM-0001";

            await _ICoporateProductCatalogRepository.CreateCoporateProductCatalog(parameter,
                poParameter.ContextAccessor);
        }

        return "";

    }

    public async Task<CpcResourceFamilyLocalizedData> CreateVehicleResourceFamily(ThAutomationParameter ThAutomationParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(ThAutomationParameter.ContractingUnitSequenceId,
                null, ThAutomationParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            await using var dbconnection = new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

            var id = connection
                .Query<CpcResourceFamilyLocalizedData>(
                    "SELECT Id  FROM dbo.CpcResourceFamilyLocalizedData WHERE CpcResourceFamilyId = @CpcResourceFamilyId AND LanguageCode = @LanguageCode",
                    new { ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId , LanguageCode = ThAutomationParameter.cpcResourceFamily.LanguageCode }).FirstOrDefault();

            //var project = dbconnection.Query<ProjectDefinition>("SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0").ToList();

            
                if (id == null)
                {
                    const string insert = @"INSERT INTO dbo.CpcResourceFamilyLocalizedData ( Id ,Label ,LanguageCode ,CpcResourceFamilyId ,ParentId ) VALUES ( @Id ,@Label ,@LanguageCode ,@CpcResourceFamilyId ,@ParentId );";

                    if (ThAutomationParameter.cpcResourceFamily.LanguageCode == "en")
                    {
                        var langData = connection
                            .Query<CpcResourceFamilyLocalizedData>(
                                "SELECT Id  FROM dbo.CpcResourceFamilyLocalizedData WHERE CpcResourceFamilyId = @CpcResourceFamilyId AND LanguageCode = @LanguageCode",
                                new { ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId , LanguageCode = "nl" }).FirstOrDefault();

                        if (langData != null)
                        {
                            var paramlang = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                ThAutomationParameter.cpcResourceFamily.Label,
                                LanguageCode = ThAutomationParameter.cpcResourceFamily.LanguageCode,
                                ParentId = ThAutomationParameter.cpcResourceFamily.ParentId,
                                CpcResourceFamilyId = ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId
                            };

                            await connection.ExecuteAsync(insert, paramlang);
                        }
                        else
                        {
                            var param = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                ThAutomationParameter.cpcResourceFamily.Label,
                                LanguageCode = ThAutomationParameter.cpcResourceFamily.LanguageCode,
                                ParentId = ThAutomationParameter.cpcResourceFamily.ParentId,
                                CpcResourceFamilyId = ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId
                            };


                            await connection.ExecuteAsync(
                                "INSERT INTO dbo.CpcResourceFamily ( Id ,LocaleCode ,ParentId ,DisplayOrder ,Title ) VALUES ( @CpcResourceFamilyId ,@Label ,@ParentId ,'0',@Label );",
                                param);
                            await connection.ExecuteAsync(insert, param);
                        }
                        
                    }else if (ThAutomationParameter.cpcResourceFamily.LanguageCode == "nl")
                    {
                        var langData = connection
                            .Query<CpcResourceFamilyLocalizedData>(
                                "SELECT Id  FROM dbo.CpcResourceFamilyLocalizedData WHERE CpcResourceFamilyId = @CpcResourceFamilyId AND LanguageCode = @LanguageCode",
                                new { ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId , LanguageCode = "en" }).FirstOrDefault();
                        
                        if (langData != null)
                        {
                            var paramlang = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                ThAutomationParameter.cpcResourceFamily.Label,
                                LanguageCode = ThAutomationParameter.cpcResourceFamily.LanguageCode,
                                ParentId = ThAutomationParameter.cpcResourceFamily.ParentId,
                                CpcResourceFamilyId = ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId
                            };

                            await connection.ExecuteAsync(insert, paramlang);
                        }
                        else
                        {
                            var param = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                ThAutomationParameter.cpcResourceFamily.Label,
                                LanguageCode = ThAutomationParameter.cpcResourceFamily.LanguageCode,
                                ParentId = ThAutomationParameter.cpcResourceFamily.ParentId,
                                CpcResourceFamilyId = ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId
                            };


                            await connection.ExecuteAsync(
                                "INSERT INTO dbo.CpcResourceFamily ( Id ,LocaleCode ,ParentId ,DisplayOrder ,Title ) VALUES ( @CpcResourceFamilyId ,@Label ,@ParentId ,'0',@Label );",
                                param);
                            await connection.ExecuteAsync(insert, param);
                        }
                    }
                   
                    
                    // var param = new
                    // {
                    //     Id = Guid.NewGuid().ToString(),
                    //     ThAutomationParameter.cpcResourceFamily.Label,
                    //     LanguageCode = ThAutomationParameter.cpcResourceFamily.LanguageCode,
                    //     ParentId = ThAutomationParameter.cpcResourceFamily.ParentId,
                    //     CpcResourceFamilyId = ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId
                    // };
                    //
                    //
                    // await connection.ExecuteAsync(
                    //     "INSERT INTO dbo.CpcResourceFamily ( Id ,LocaleCode ,ParentId ,DisplayOrder ,Title ) VALUES ( @CpcResourceFamilyId ,@Label ,@ParentId ,'0',@Label );",
                    //     param);
                    // // await dbconnection.ExecuteAsync(
                    // //     "INSERT INTO dbo.CpcResourceFamily ( Id ,LocaleCode ,ParentId ,DisplayOrder ,Title ) VALUES ( @CpcResourceFamilyId ,@Label ,@ParentId ,'0',@Label );",
                    // //     param);
                    //
                    // await connection.ExecuteAsync(insert, param);
                   // await dbconnection.ExecuteAsync(insert, param);

                    // foreach (var i in project)
                    // {
                    //     await using var pconnection = new SqlConnection(i.ProjectConnectionString);
                    //
                    //     await pconnection.ExecuteAsync(
                    //         "INSERT INTO dbo.CpcResourceFamily ( Id ,LocaleCode ,ParentId ,DisplayOrder ,Title ) VALUES ( @CpcResourceFamilyId ,@Label ,@ParentId ,'0',@Label );",
                    //         param);
                    //     await pconnection.ExecuteAsync(insert, param);
                    //
                    // }


                }
                else
                {
                    var param1 = new
                    {
                        Id = ThAutomationParameter.cpcResourceFamily.Label,
                        LanguageCode = ThAutomationParameter.cpcResourceFamily.LanguageCode,
                        ParentId = ThAutomationParameter.cpcResourceFamily.ParentId,
                        ThAutomationParameter.cpcResourceFamily.CpcResourceFamilyId
                    };

                    const string insert = @"UPDATE dbo.CpcResourceFamilyLocalizedData 
                                SET Label = @Label
                                WHERE
                                  CpcResourceFamilyId = @CpcResourceFamilyId AND LanguageCode = @LanguageCode
                                ;";

                    await connection.ExecuteAsync(insert, param1);
                    //await dbconnection.ExecuteAsync(insert, param1);

                    // foreach (var i in project)
                    // {
                    //     await using var pconnection = new SqlConnection(i.ProjectConnectionString);
                    //
                    //     await pconnection.ExecuteAsync(insert, param1);
                    //
                    // }
                    
                }

            
            return ThAutomationParameter.cpcResourceFamily;

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

   public async Task<GetThProductWithTrucksDto> AddPumpsToProductMyEnv(ThAutomationParameter ThAutomationParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode, ThAutomationParameter.TenantProvider);

        var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
            null, ThAutomationParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
        await using var dbconnection =
            new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);
        await using var cuConnection = new SqlConnection(cuConnectionString);


        var pbsDynamicAttributes = connection
            .Query<PbsDynamicAttributes>("Select * from PbsDynamicAttributes Where ProductId = @ProductId",
                new { ThAutomationParameter.GetThProductWithTrucksDto.ProductId }).ToList();

        var projectStartTime = DateTime.Parse(ThAutomationParameter.GetThProductWithTrucksDto.Date.ToShortDateString());
        var projectEndTime = DateTime.Parse(ThAutomationParameter.GetThProductWithTrucksDto.Date.ToShortDateString());
        var projectSTime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "StartTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectStartTime = projectStartTime.AddHours(projectSTime.Hour)
            .AddMinutes(projectSTime.Minute);
        var projectETime = DateTime.ParseExact(pbsDynamicAttributes.FirstOrDefault(x => x.Key == "EndTime").Value,
            "HH:mm", CultureInfo.InvariantCulture);
        projectEndTime = projectEndTime.AddHours(projectETime.Hour).AddMinutes(projectSTime.Minute);
        var capacity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Capacity")?.Value;
        var velocity = pbsDynamicAttributes.FirstOrDefault(x => x.Key == "Velocity")?.Value;


        var isExist = connection
            .Query<ThProductWithTrucks>(
                "Select * From ThProductWithTrucks Where CpcId = @CpcId AND ProductId = @ProductId AND IsTruck = 0",
                ThAutomationParameter.GetThProductWithTrucksDto);

        await CopyCpcCuToProject(ThAutomationParameter, ThAutomationParameter.GetThProductWithTrucksDto.CpcId,
            connectionString, "cu");

        if (!isExist.Any())
        {
            var insertQuery =
                @"INSERT INTO dbo.ThProductWithTrucks (Id, CpcId, ProductId, Date,IsTruck) VALUES (@Id, @CpcId, @ProductId, @Date,@IsTruck)";

            var query =
                @"INSERT INTO dbo.ThTrucksSchedule ( Id ,ProductTruckId ,Title ,EndTime ,StartTime ,Type,TurnNumber,LoadingNumber,TruckOrder ) VALUES ( @Id ,@ProductTruckId ,@Title ,@EndTime ,@StartTime ,@Type,@TurnNumber,@LoadingNumber,@TruckOrder )";

            ThAutomationParameter.GetThProductWithTrucksDto.IsTruck = false;
            await connection.ExecuteAsync(insertQuery, ThAutomationParameter.GetThProductWithTrucksDto);
            

            var cpcTitle = cuConnection
                .Query<string>("Select Title From CorporateProductCatalog Where Id = @Id",
                    new { Id = ThAutomationParameter.GetThProductWithTrucksDto.CpcId }).FirstOrDefault();
            

                var truckUnloadParam1 = new ThTrucksSchedule
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                    Title = "Unloading",
                    StartTime = projectStartTime,
                    EndTime = projectEndTime,
                    Type = "2",
                    TurnNumber = 1,
                    TruckOrder = 0,
                    LoadingNumber = 0
                };
            

                var projectSequenceId1 = ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode;

                var mapLocation1 = dbconnection
                    .Query<Position>(
                        "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId LEFT OUTER JOIN ProjectDefinition pd ON ml.Id = pd.LocationId WHERE pd.SequenceCode = @SequenceCode",
                        new { SequenceCode = projectSequenceId1 }).FirstOrDefault();
                if (mapLocation1 != null)
                {
                    var orgLat = ThAutomationParameter.Configuration.GetValue<string>("HQLat");
                    var orgLong = ThAutomationParameter.Configuration.GetValue<string>("HQLong");

                    var dur = ThAutomationParameter._iShiftRepository.CalculateDistance(
                        mapLocation1.Lat.ToDouble(), mapLocation1.Lon.ToDouble(),
                        orgLat.ToDouble(), orgLong.ToDouble(), ThAutomationParameter.TenantProvider,
                        ThAutomationParameter.Configuration, true);

                    if (await dur != 0)
                    {
                        var truckTravel1Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "First Travel",
                            // StartTime = projectStartTime.AddMinutes(-await duration),
                            StartTime = truckUnloadParam1.StartTime?.AddMinutes(-await dur),
                            EndTime = truckUnloadParam1.StartTime,
                            Type = "1",
                            TurnNumber = 1,
                            TruckOrder = 0,
                            LoadingNumber = 0
                        };

                        var truckTravel2Param = new ThTrucksSchedule
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductTruckId = ThAutomationParameter.GetThProductWithTrucksDto.Id,
                            Title = "Second Travel",
                            StartTime = truckUnloadParam1.EndTime,
                            // EndTime = truckUnloadParam.EndTime?.AddMinutes(await duration),
                            EndTime = truckUnloadParam1.EndTime?.AddMinutes(await dur),
                            Type = "3",
                            TurnNumber = 1,
                            TruckOrder = 0,
                            LoadingNumber = 0
                        };




                        await connection.ExecuteAsync(query, truckUnloadParam1);
                        await connection.ExecuteAsync(query, truckTravel1Param);
                        await connection.ExecuteAsync(query, truckTravel2Param);

                        var dynamicDto = new ThPbsCreateDto
                        {
                            Capacity = capacity?.ToFloat(),
                            Velocity = velocity,
                            StartDate = truckTravel1Param.StartTime,
                            EndDate = truckTravel2Param.EndTime,
                            CpcTitle = cpcTitle,
                            TurnNumber = 1
                        };

                        ThAutomationParameter.ThPbsCreateDto = dynamicDto;
                        ThAutomationParameter.ProjectSequenceId =
                            ThAutomationParameter.GetThProductWithTrucksDto.ProjectSequenceCode;

                        await ThPbsCreate(ThAutomationParameter);

                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel1Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckTravel2Param);
                        ThPmolCreate(ThAutomationParameter, projectSequenceId1, cpcTitle, truckUnloadParam1);
                    }

                }
        }
        

        return ThAutomationParameter.GetThProductWithTrucksDto;
    }
   
   public async Task<List<GetThResourceFamilies>> GetThResourceFamilies(ThAutomationParameter ThAutomationParameter)
   {

       var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
           null, ThAutomationParameter.TenantProvider);
       
       await using var cuConnection = new SqlConnection(cuConnectionString);

       var data = new List<GetThResourceFamilies>();

       if (ThAutomationParameter.ThResourceFamiliesDto.IsTruck)
       {
           var family = cuConnection.Query<GetThResourceFamilies>(@"
                           SELECT
                           CpcResourceFamilyLocalizedData.CpcResourceFamilyId AS Id
                           ,CpcResourceFamilyLocalizedData.Label AS title
                            FROM dbo.CpcResourceFamilyLocalizedData
                            WHERE CpcResourceFamilyLocalizedData.LanguageCode = @lang
                            AND CpcResourceFamilyLocalizedData.CpcResourceFamilyId = '2210e768-human-kknk-truck-ee367a82ad17'
                              ", new{lang = ThAutomationParameter.Lang}).FirstOrDefault();
           data.Add(family);

           var cpcData = cuConnection
               .Query<GetThResourceFamilies>(
                   @"SELECT Id,Title,ResourceFamilyId As ParentId,Id As CPCId FROM dbo.CorporateProductCatalog WHERE ResourceFamilyId = '2210e768-human-kknk-truck-ee367a82ad17' ")
               .ToList();
           
           data.AddRange(cpcData);
       }
       else
       {
           
               var family = cuConnection.Query<GetThResourceFamilies>(@"
                           SELECT
                           CpcResourceFamilyLocalizedData.CpcResourceFamilyId AS Id
                           ,CpcResourceFamilyLocalizedData.Label AS title
                            FROM dbo.CpcResourceFamilyLocalizedData
                            WHERE CpcResourceFamilyLocalizedData.LanguageCode = @lang
                            AND CpcResourceFamilyLocalizedData.CpcResourceFamilyId = 'hjkl2c90-94f6-pump-8410-921a43c2lkjl'
                              ", new{lang = ThAutomationParameter.Lang}).FirstOrDefault();
               data.Add(family);

               var cpcData = cuConnection
                   .Query<GetThResourceFamilies>(
                       @"SELECT Id,Title,ResourceFamilyId As ParentId,Id As CPCId FROM dbo.CorporateProductCatalog WHERE ResourceFamilyId = 'hjkl2c90-94f6-pump-8410-921a43c2lkjl' ")
                   .ToList();
           
               data.AddRange(cpcData);
           
       }

       return data;
   }

   public async Task<ThTruckAvailabilityDto> AddTruckAvailableTimes(ThAutomationParameter ThAutomationParameter)
   {

       var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
           null, ThAutomationParameter.TenantProvider);

       await using var cuConnection = new SqlConnection(cuConnectionString);

       var timeslot = cuConnection
           .Query<string>("Select Id From ThTruckAvailability Where Date = @Date AND StockId = @StockId",
               ThAutomationParameter.ThTruckAvailabilityDto).ToList();

       await cuConnection.ExecuteAsync("Update ThTruckAvailability Set SortingOrder = @SortingOrder Where Id In @Ids",
           new { Ids = timeslot, SortingOrder = ThAutomationParameter.ThTruckAvailabilityDto.SortingOrder });
       
       var insertQuery =
           @"INSERT INTO dbo.ThTruckAvailability ( Id ,StockId ,ActivityType ,Date ,Availability ,ResourceFamilyId ,STime ,ETime ,SortingOrder ) VALUES ( @Id ,@StockId ,@ActivityType ,@Date ,@Availability ,@ResourceFamilyId ,@STime ,@ETime ,@SortingOrder )";
       
       await cuConnection.ExecuteAsync(insertQuery, ThAutomationParameter.ThTruckAvailabilityDto);

       return ThAutomationParameter.ThTruckAvailabilityDto;
   }

   public async Task<string> ThStockCreate(ThAutomationParameter ThAutomationParameter)
   {
       var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
           null, ThAutomationParameter.TenantProvider);
       
       await using var connection = new SqlConnection(cuConnectionString);

       var stockItem = connection.Query<StockHeader>("SELECT * FROM [dbo].[StockHeader] Where CPCId = @CPCId ",
           ThAutomationParameter.ThStockCreate).FirstOrDefault();

       if (stockItem == null)
       {
           var cpc = connection.Query<CorporateProductCatalog>(
               "Select * From CorporateProductCatalog Where Id = @CPCId ",
               ThAutomationParameter.ThStockCreate).FirstOrDefault();

           if (cpc != null)
           {

               var mou = connection.Query<string>(
                       "SELECT Label FROM CpcBasicUnitOfMeasureLocalizedData WHERE CpcBasicUnitOfMeasureId = @CpcBasicUnitOfMeasureId AND LanguageCode = @lang ",
                       new { CpcBasicUnitOfMeasureId = cpc.CpcBasicUnitOfMeasureId, lang = ThAutomationParameter.Lang })
                   .FirstOrDefault();
               
               var stockParam = new StockParameter()
               {
                   ContractingUnitSequenceId = "COM-0001",
                   TenantProvider = ThAutomationParameter.TenantProvider,
                   Lang = ThAutomationParameter.Lang
               };

               var stockCreate = new StockCreateDto()
               {
                   Id = Guid.NewGuid().ToString(),
                   AveragePrice = "0.00",
                   CpcId = cpc.Id,
                   MouId = mou,
                   Name = cpc.ResourceTitle,
                   ResourceType = cpc.ResourceTypeId,
                   Status = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
                   Type = "94282458-0b40-40a3-b0f9-c2e40344c8f1"
               };
               stockParam.StockDto = stockCreate;

               await ThAutomationParameter.iStockRepository.CreateHeader(stockParam);

               return stockCreate.Id;

           }
           else
           {
               throw new Exception("CPC does not exist");
           }
           
       }
       else
       {
           return stockItem.Id;
       }

   }

   public async Task<ThStockDelete> DeleteThStockAvailableTime(ThAutomationParameter ThAutomationParameter)
   {
       var cuConnectionString = ConnectionString.MapConnectionString("COM-0001",
           null, ThAutomationParameter.TenantProvider);
       
       await using var connection = new SqlConnection(cuConnectionString);

       if (ThAutomationParameter.ThStockDelete.Id != null)
       {
           const string deleteQuery = "Delete From ThTruckAvailability Where Id = @Id ";

           await connection.ExecuteAsync(deleteQuery, ThAutomationParameter.ThStockDelete);
       }

       return ThAutomationParameter.ThStockDelete;
   }

   public async Task<List<ThColorsDto>> GetColourCodes(ThAutomationParameter ThAutomationParameter)
   {
       await using var connection = new SqlConnection(ThAutomationParameter.TenantProvider.GetTenant().ConnectionString);

       var data = connection.Query<ThColorsDto>("Select Code As BgColor, Font  From ThColors").ToList();

       return data;

   }



}