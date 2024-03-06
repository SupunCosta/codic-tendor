using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.BOR;

public class BorResourceRepository : IBorResourceRepository
{
    public async Task<string> CreateBorConsumable(BorParameter borParameter, bool isUpdate)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, borParameter.TenantProvider))
        {
            if (isUpdate == false && borParameter.BorDto.WeekPlan == false)
                if (borParameter.BorDto.Product != null)
                {
                    var query =
                        @"SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsConsumable WHERE PbsProductId = @Id";

                    var parameter = new { borParameter.BorDto.Product.Id };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorConsumable (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";
                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredConsumable (Id,Date,Quantity,BorConsumableId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";
                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                            await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                        }
                    }
                }

            if (borParameter.BorDto.WeekPlan)
                if (borParameter.BorDto.Product != null)
                {
                    var query =
                        @" SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsConsumable WHERE PbsProductId = @Id AND CoperateProductCatalogId = @CpcId";

                    var parameter = new { borParameter.BorDto.Product.Id, CpcId = borParameter.BorDto.CId };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorConsumable (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";

                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredConsumable (Id,Date,Quantity,BorConsumableId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";
                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using var connection = new SqlConnection(connectionString);
                        await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                        await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                    }
                }

            if (borParameter.BorDto.BorResources.Consumable != null && borParameter.BorDto.WeekPlan == false)
                foreach (var dto in borParameter.BorDto.BorResources.Consumable)
                {
                    switch (dto.Type)
                    {
                        case "1" when dto.IsNew:
                        {
                            string id = null;
                            var consumable = context.BorConsumable.FirstOrDefault(b =>
                                b.BorProductId == borParameter.BorDto.Id &&
                                b.CorporateProductCatalogId == dto.Id);
                            if (consumable == null)
                            {
                                var borconsumable = new BorConsumable
                                {
                                    BorProductId = borParameter.BorDto.Id,
                                    CorporateProductCatalogId = dto.Id,
                                    Date = DateTime.UtcNow,
                                    Required = dto.Required,
                                    TotalRequired = dto.Required,
                                    Returned = dto.Returned.ToDouble(),
                                    Id = Guid.NewGuid().ToString()
                                };
                                context.BorConsumable.Add(borconsumable);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                id = borconsumable.Id;
                            }
                            else
                            {
                                id = consumable.Id;
                                consumable.TotalRequired += dto.Required;
                                consumable.Returned = dto.Returned.ToDouble();
                                context.BorConsumable.Update(consumable);
                                await context.SaveChangesAsync().ConfigureAwait(false);
                            }

                            var requiredConsumable = new BorRequiredConsumable();
                            requiredConsumable.BorConsumableId = id;
                            requiredConsumable.CpcId = dto.Id;
                            requiredConsumable.Date = DateTime.UtcNow;
                            requiredConsumable.Quantity = dto.Required;
                            requiredConsumable.Id = Guid.NewGuid().ToString();
                            context.BorRequiredConsumable.Add(requiredConsumable);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                        case "0" when dto.IsNew:
                        {
                            var consumable = context.BorConsumable.FirstOrDefault(b =>
                                b.BorProductId == borParameter.BorDto.Id &&
                                b.CorporateProductCatalogId == dto.Id);
                            if (consumable != null)
                            {
                                consumable.TotalRequired -= dto.Required;
                                consumable.Returned = dto.Returned.ToDouble();
                                context.BorConsumable.Update(consumable);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                var requiredConsumable = new BorRequiredConsumable
                                {
                                    BorConsumableId = consumable.Id,
                                    CpcId = dto.Id,
                                    Date = DateTime.UtcNow,
                                    Quantity = dto.Required * -1,
                                    Id = Guid.NewGuid().ToString()
                                };
                                context.BorRequiredConsumable.Add(requiredConsumable);
                            }

                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                    }

                    if (dto.Environment != "local" && dto.IsNew)
                    {
                        var parameters = new CpcParameters
                        {
                            Lang = borParameter.Lang,
                            Id = dto.Id
                        };
                        borParameter.CpcParameters = parameters;
                        borParameter.Lang = borParameter.Lang;
                        await CopyCpc(borParameter, dto.ResourceNumber, connectionString, dto.Environment)
                            .ConfigureAwait(false);
                    }
                }
        }

        return borParameter.BorDto.Id;
    }

    public async Task<string> CreateBorLabour(BorParameter borParameter, bool isUpdate)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, borParameter.TenantProvider))
        {
            if (isUpdate == false && borParameter.BorDto.WeekPlan == false)
                if (borParameter.BorDto.Product != null && borParameter.BorDto.WeekPlan == false)
                {
                    var query =
                        @" SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsLabour WHERE PbsProductId = @Id";

                    var parameter = new { borParameter.BorDto.Product.Id };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorLabour (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";

                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredLabour (Id,Date,Quantity,BorLabourId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";
                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using var connection = new SqlConnection(connectionString);
                        await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                        await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                    }
                }

            if (borParameter.BorDto.WeekPlan)
                if (borParameter.BorDto.Product != null)
                {
                    var query =
                        @" SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsLabour WHERE PbsProductId = @Id AND CoperateProductCatalogId = @CpcId";

                    var parameter = new { borParameter.BorDto.Product.Id, CpcId = borParameter.BorDto.CId };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorLabour (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";

                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredLabour (Id,Date,Quantity,BorLabourId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";
                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using var connection = new SqlConnection(connectionString);
                        await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                        await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                    }
                }

            if (borParameter.BorDto.BorResources.Labour != null && borParameter.BorDto.WeekPlan == false)
                foreach (var dto in borParameter.BorDto.BorResources.Labour)
                {
                    switch (dto.Type)
                    {
                        case "1" when dto.IsNew:
                        {
                            string id = null;
                            var labour = context.BorLabour.FirstOrDefault(b =>
                                b.BorProductId == borParameter.BorDto.Id &&
                                b.CorporateProductCatalogId == dto.Id);

                            if (labour == null)
                            {
                                var borlabour = new BorLabour
                                {
                                    BorProductId = borParameter.BorDto.Id,
                                    CorporateProductCatalogId = dto.Id,
                                    Date = DateTime.UtcNow,
                                    Required = dto.Required,
                                    TotalRequired = dto.Required,
                                    Returned = dto.Returned.ToDouble(),
                                    Id = Guid.NewGuid().ToString()
                                };
                                context.BorLabour.Add(borlabour);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                id = borlabour.Id;
                            }
                            else
                            {
                                id = labour.Id;
                                labour.TotalRequired += dto.Required;
                                labour.Returned = dto.Returned.ToDouble();
                                context.BorLabour.Update(labour);
                                await context.SaveChangesAsync().ConfigureAwait(false);
                            }

                            var requiredlabour = new BorRequiredLabour
                            {
                                BorLabourId = id,
                                CpcId = dto.Id,
                                Date = DateTime.UtcNow,
                                Quantity = dto.Required,
                                Id = Guid.NewGuid().ToString()
                            };
                            context.BorRequiredLabour.Add(requiredlabour);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                        case "0" when dto.IsNew:
                        {
                            var labour = context.BorLabour.FirstOrDefault(b =>
                                b.BorProductId == borParameter.BorDto.Id &&
                                b.CorporateProductCatalogId == dto.Id);
                            if (labour != null)
                            {
                                labour.TotalRequired -= dto.Required;
                                labour.Returned = dto.Returned.ToDouble();
                                context.BorLabour.Update(labour);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                var requiredlabour = new BorRequiredLabour
                                {
                                    BorLabourId = labour.Id,
                                    CpcId = dto.Id,
                                    Date = DateTime.UtcNow,
                                    Quantity = dto.Required * -1,
                                    Id = Guid.NewGuid().ToString()
                                };
                                context.BorRequiredLabour.Add(requiredlabour);
                            }

                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                    }

                    if (dto.Environment != "local" && dto.IsNew)
                    {
                        var parameters = new CpcParameters
                        {
                            Lang = borParameter.Lang,
                            Id = dto.Id
                        };
                        borParameter.CpcParameters = parameters;
                        borParameter.Lang = borParameter.Lang;
                        await CopyCpc(borParameter, dto.ResourceNumber, connectionString, dto.Environment)
                            .ConfigureAwait(false);
                    }
                }
        }

        return borParameter.BorDto.Id;
    }

    public async Task<string> CreateBorMaterial(BorParameter borParameter, bool isUpdate)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, borParameter.TenantProvider))
        {
            if (isUpdate == false && borParameter.BorDto.WeekPlan == false)
                if (borParameter.BorDto.Product != null)
                {
                    var query =
                        @" SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsMaterial WHERE PbsProductId = @Id";

                    var parameter = new { borParameter.BorDto.Product.Id };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorMaterial (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";

                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredMaterial (Id,Date,Quantity,BorMaterialId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";
                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using var connection = new SqlConnection(connectionString);
                        await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                        await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                    }
                }

            if (borParameter.BorDto.WeekPlan)
                if (borParameter.BorDto.Product != null)
                {
                    var query =
                        @" SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsMaterial WHERE PbsProductId = @Id AND CoperateProductCatalogId = @CpcId";

                    var parameter = new { borParameter.BorDto.Product.Id, CpcId = borParameter.BorDto.CId };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorMaterial (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";

                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredMaterial (Id,Date,Quantity,BorMaterialId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";
                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using var connection = new SqlConnection(connectionString);
                        await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                        await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                    }
                }

            if (borParameter.BorDto.BorResources.Materials != null && borParameter.BorDto.WeekPlan == false)
                foreach (var dto in borParameter.BorDto.BorResources.Materials)
                {
                    switch (dto.Type)
                    {
                        // type 1 add extra
                        case "1" when dto.IsNew:
                        {
                            string id = null;
                            var material = context.BorMaterial.FirstOrDefault(b =>
                                b.BorProductId == borParameter.BorDto.Id &&
                                b.CorporateProductCatalogId == dto.Id);
                            if (material == null)
                            {
                                var bormaterial = new BorMaterial
                                {
                                    BorProductId = borParameter.BorDto.Id,
                                    CorporateProductCatalogId = dto.Id,
                                    Date = DateTime.UtcNow,
                                    Required = dto.Required,
                                    TotalRequired = dto.Required,
                                    Returned = dto.Returned.ToDouble(),
                                    Id = Guid.NewGuid().ToString()
                                };
                                context.BorMaterial.Add(bormaterial);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                id = bormaterial.Id;
                            }
                            else
                            {
                                id = material.Id;
                                material.TotalRequired += dto.Required;
                                material.Returned = dto.Returned.ToDouble();
                                context.BorMaterial.Update(material);
                                await context.SaveChangesAsync().ConfigureAwait(false);
                            }

                            var requiredMaterial = new BorRequiredMaterial
                            {
                                BorMaterialId = id,
                                CpcId = dto.Id,
                                Date = DateTime.UtcNow,
                                Quantity = dto.Required,
                                Id = Guid.NewGuid().ToString()
                            };
                            context.BorRequiredMaterial.Add(requiredMaterial);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                        // type remove
                        case "0" when dto.IsNew:
                        {
                            var material = context.BorMaterial.FirstOrDefault(b =>
                                b.BorProductId == borParameter.BorDto.Id &&
                                b.CorporateProductCatalogId == dto.Id);
                            if (material != null)
                            {
                                material.TotalRequired -= dto.Required;
                                material.Returned = dto.Returned.ToDouble();
                                context.BorMaterial.Update(material);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                var requiredMaterial = new BorRequiredMaterial
                                {
                                    BorMaterialId = material.Id,
                                    CpcId = dto.Id,
                                    Date = DateTime.UtcNow,
                                    Quantity = dto.Required * -1,
                                    Id = Guid.NewGuid().ToString()
                                };
                                context.BorRequiredMaterial.Add(requiredMaterial);
                            }

                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                    }

                    if (dto.Environment != "local" && dto.IsNew)
                    {
                        var parameters = new CpcParameters
                        {
                            Lang = borParameter.Lang,
                            Id = dto.Id
                        };
                        borParameter.CpcParameters = parameters;
                        borParameter.Lang = borParameter.Lang;
                        await CopyCpc(borParameter, dto.ResourceNumber, connectionString, dto.Environment)
                            .ConfigureAwait(false);
                    }
                }
        }

        return borParameter.BorDto.Id;
    }

    public async Task<string> CreateBorTools(BorParameter borParameter, bool isUpdate)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(borParameter.ContractingUnitSequenceId,
            borParameter.ProjectSequenceId, borParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, borParameter.TenantProvider))
        {
            if (isUpdate == false && borParameter.BorDto.WeekPlan == false)
                if (borParameter.BorDto.Product != null)
                {
                    var query =
                        @" SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsTools WHERE PbsProductId = @Id";

                    var parameter = new { borParameter.BorDto.Product.Id };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorTools (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";

                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredTools (Id,Date,Quantity,BorToolsId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";

                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using var connection = new SqlConnection(connectionString);
                        await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                        await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                    }
                }

            if (borParameter.BorDto.WeekPlan)
                if (borParameter.BorDto.Product != null)
                {
                    var query =
                        @" SELECT Id, PbsProductId, CoperateProductCatalogId, Quantity FROM dbo.PbsTools WHERE PbsProductId = @Id AND CoperateProductCatalogId = @CpcId";

                    var parameter = new { borParameter.BorDto.Product.Id, CpcId = borParameter.BorDto.CId };
                    IEnumerable<PbsResourcesForBorDto> data;
                    await using (var dbConnection = new SqlConnection(connectionString))
                    {
                        data = dbConnection.Query<PbsResourcesForBorDto>(query, parameter);
                    }

                    foreach (var m in data)
                    {
                        var createquery =
                            @"INSERT INTO dbo.BorTools (Id,BorProductId,Date,Required,Purchased,DeliveryRequested,Warf,Consumed,Invoiced,CorporateProductCatalogId,TotalRequired) VALUES (@ResourceId,@BorProductId,@Date,@Required,'0','0','0','0','0',@CorporateProductCatalogId,@TotalRequired)";

                        var reqiredquery =
                            @"INSERT INTO dbo.BorRequiredTools (Id,Date,Quantity,BorToolsId,CpcId) VALUES (@ReqiredResourceId,@Date,@Required,@ResourceId,@CorporateProductCatalogId)";
                        var createparameter = new
                        {
                            ResourceId = Guid.NewGuid().ToString(),
                            BorProductId = borParameter.BorDto.Id,
                            Date = DateTime.UtcNow,
                            Required = m.Quantity,
                            CorporateProductCatalogId = m.CoperateProductCatalogId,
                            TotalRequired = m.Quantity,
                            ReqiredResourceId = Guid.NewGuid().ToString()
                        };

                        await using var connection = new SqlConnection(connectionString);
                        await connection.ExecuteAsync(createquery, createparameter).ConfigureAwait(false);
                        await connection.ExecuteAsync(reqiredquery, createparameter).ConfigureAwait(false);
                    }
                }

            if (borParameter.BorDto.BorResources.Tools != null && borParameter.BorDto.WeekPlan == false)
                foreach (var dto in borParameter.BorDto.BorResources.Tools)
                {
                    switch (dto.Type)
                    {
                        case "1" when dto.IsNew:
                        {
                            string id = null;
                            var tool = context.BorTools.FirstOrDefault(b => b.BorProductId == borParameter.BorDto.Id &&
                                                                            b.CorporateProductCatalogId == dto.Id);
                            if (tool == null)
                            {
                                var bortool = new BorTools
                                {
                                    BorProductId = borParameter.BorDto.Id,
                                    CorporateProductCatalogId = dto.Id,
                                    Date = DateTime.UtcNow,
                                    Required = dto.Required,
                                    TotalRequired = dto.Required,
                                    Returned = dto.Returned.ToDouble(),
                                    Id = Guid.NewGuid().ToString()
                                };
                                context.BorTools.Add(bortool);
                                await context.SaveChangesAsync().ConfigureAwait(false);

                                id = bortool.Id;
                            }
                            else
                            {
                                id = tool.Id;
                                tool.TotalRequired += dto.Required;
                                tool.Returned = dto.Returned.ToDouble();
                                context.BorTools.Update(tool);

                                await context.SaveChangesAsync().ConfigureAwait(false);
                            }

                            var requiredtool = new BorRequiredTools
                            {
                                BorToolsId = id,
                                CpcId = dto.Id,
                                Date = DateTime.UtcNow,
                                Quantity = dto.Required,
                                Id = Guid.NewGuid().ToString()
                            };
                            context.BorRequiredTools.Add(requiredtool);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                        case "0" when dto.IsNew:
                        {
                            var tool = context.BorTools.FirstOrDefault(b => b.BorProductId == borParameter.BorDto.Id &&
                                                                            b.CorporateProductCatalogId == dto.Id);
                            tool.TotalRequired -= dto.Required;
                            tool.Returned = dto.Returned.ToDouble();
                            context.BorTools.Update(tool);
                            await context.SaveChangesAsync().ConfigureAwait(false);

                            var requiredtool = new BorRequiredTools
                            {
                                BorToolsId = tool.Id,
                                CpcId = dto.Id,
                                Date = DateTime.UtcNow,
                                Quantity = dto.Required * -1,
                                Id = Guid.NewGuid().ToString()
                            };
                            context.BorRequiredTools.Add(requiredtool);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                            break;
                        }
                    }

                    if (dto.Environment != "local" && dto.IsNew)
                    {
                        var parameters = new CpcParameters
                        {
                            Lang = borParameter.Lang,
                            Id = dto.Id
                        };
                        borParameter.CpcParameters = parameters;
                        borParameter.Lang = borParameter.Lang;
                        await CopyCpc(borParameter, dto.ResourceNumber, connectionString, dto.Environment)
                            .ConfigureAwait(false);
                    }
                }
        }

        return borParameter.BorDto.Id;
    }


    public async Task<string> UpdateBorMaterial(BorParameterResoruce borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);
        var parameters = new
            { borParameterResoruce.borResourceUpdate.BorRequiredId, borParameterResoruce.borResourceUpdate.required };

        var updateBorMaterial =
            @"UPDATE dbo.BorRequiredMaterial SET Quantity = @required WHERE BorMaterialId = @BorRequiredId";
        var updateBorMaterialParent = @"UPDATE dbo.BorMaterial SET Required = @required WHERE Id = @paretntID";
        var parametersParent = new
        {
            paretntID = borParameterResoruce.borResourceUpdate.BorRequiredId,
            borParameterResoruce.borResourceUpdate.required
        };

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.QueryFirstOrDefaultAsync(updateBorMaterial, parameters).ConfigureAwait(false);
            await dbConnection.QueryFirstOrDefaultAsync(updateBorMaterialParent, parametersParent)
                .ConfigureAwait(false);
        }

        return borParameterResoruce.borResourceUpdate.BorRequiredId;
    }

    public async Task<string> UpdateBorTools(BorParameterResoruce borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);
        var parameters = new
            { borParameterResoruce.borResourceUpdate.BorRequiredId, borParameterResoruce.borResourceUpdate.required };

        var updateBorMaterialParent = @"UPDATE dbo.BorTools SET Required = @required WHERE Id = @paretntID";
        var parametersParent = new
        {
            paretntID = borParameterResoruce.borResourceUpdate.BorRequiredId,
            borParameterResoruce.borResourceUpdate.required
        };


        var updateBorMaterial =
            @"UPDATE dbo.BorRequiredTools SET Quantity = @required WHERE BorToolsId = @BorRequiredId";
        string result;
        await using var dbConnection = new SqlConnection(connectionString);
        result = await dbConnection.QueryFirstOrDefaultAsync(updateBorMaterial, parameters).ConfigureAwait(false);
        await dbConnection.QueryFirstOrDefaultAsync(updateBorMaterialParent, parametersParent);

        

        return result;
    }

    public async Task<string> UpdateBorLabour(BorParameterResoruce borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);
        var parameters = new
            { borParameterResoruce.borResourceUpdate.BorRequiredId, borParameterResoruce.borResourceUpdate.required };


        var updateBorMaterialParent = @"UPDATE dbo.BorLabour SET Required = @required WHERE Id = @paretntID";
        var parametersParent = new
        {
            paretntID = borParameterResoruce.borResourceUpdate.BorRequiredId,
            borParameterResoruce.borResourceUpdate.required
        };


        var updateBorMaterial =
            @"UPDATE dbo.BorRequiredLabour SET Quantity = @required WHERE BorLabourId = @BorRequiredId";
        string result;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            result = dbConnection.QueryFirstOrDefault(updateBorMaterial, parameters);
            dbConnection.QueryFirstOrDefault(updateBorMaterialParent, parametersParent);

            
        }

        return borParameterResoruce.borResourceUpdate.BorRequiredId;
    }

    public async Task<string> UpdateBorConsumable(BorParameterResoruce borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);
        var parameters = new
            { borParameterResoruce.borResourceUpdate.BorRequiredId, borParameterResoruce.borResourceUpdate.required };


        var updateBorMaterialParent = @"UPDATE dbo.BorConsumable SET Required = @required WHERE Id = @paretntID";
        var parametersParent = new
        {
            paretntID = borParameterResoruce.borResourceUpdate.BorRequiredId,
            borParameterResoruce.borResourceUpdate.required
        };


        var updateBorMaterial =
            @"UPDATE dbo.BorRequiredConsumable SET Quantity = @required WHERE BorConsumableId = @BorRequiredId";
        var dbConnection = new SqlConnection(connectionString);
        await using (dbConnection.ConfigureAwait(false))
        {
            await dbConnection.QueryFirstOrDefaultAsync(updateBorMaterial, parameters);
            await dbConnection.QueryFirstOrDefaultAsync(updateBorMaterialParent, parametersParent);

            
        }

        return borParameterResoruce.borResourceUpdate.BorRequiredId;
    }


    public async Task<List<string>> DeleteBorMaterial(BorParameterResoruceDelete borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);

        foreach (var id in borParameterResoruce.idList)
        {
            var parameters = new { id };


            var resultDeleteChild = @"DELETE FROM dbo.BorRequiredMaterial WHERE BorMaterialId =@id";

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteChild, parameters).FirstOrDefault();


                
            }

            var resultDeleteParent = @"DELETE FROM dbo.BorMaterial WHERE Id =@id";


            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteParent, parameters).FirstOrDefault();


                
            }
        }

        return borParameterResoruce.idList;
    }


    public async Task<List<string>> DeleteBorLabour(BorParameterResoruceDelete borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);
        //var parameters = new { lang = borParameterResoruce.Lang, id = borParameterResoruce.BorId };

        foreach (var id in borParameterResoruce.idList)
        {
            var parameters = new { id };

            var resultDeleteChild = @"DELETE FROM dbo.BorRequiredLabour WHERE BorLabourId =@id";

            using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteChild, parameters).FirstOrDefault();


                
            }

            var resultDeleteParent = @"DELETE FROM dbo.BorLabour WHERE Id =@id";
            //var parametersParent = new { id = BorMaterialId };


            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteParent, parameters).FirstOrDefault();


                
            }
        }

        return borParameterResoruce.idList;
    }


    public async Task<List<string>> DeleteBorConsumable(BorParameterResoruceDelete borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);
        //var parameters = new { lang = borParameterResoruce.Lang, id = borParameterResoruce.BorId };

        foreach (var id in borParameterResoruce.idList)
        {
            var parameters = new { id };

            var resultDeleteChild = @"DELETE FROM dbo.BorRequiredConsumable WHERE BorConsumableId =@id";

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteChild, parameters).FirstOrDefault();


                
            }

            var resultDeleteParent = @"DELETE FROM dbo.BorConsumable WHERE Id =@id";


            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteParent, parameters).FirstOrDefault();


                
            }
        }

        return borParameterResoruce.idList;
    }

    public async Task<List<string>> DeleteBorTools(BorParameterResoruceDelete borParameterResoruce)
    {
        var connectionString = ConnectionString.MapConnectionString(borParameterResoruce.ContractingUnitSequenceId,
            borParameterResoruce.ProjectSequenceId, borParameterResoruce.TenantProvider);
        //var parameters = new { lang = borParameterResoruce.Lang, id = borParameterResoruce.BorId };

        foreach (var id in borParameterResoruce.idList)
        {
            var parameters = new { id };


            var resultDeleteChild = @"DELETE FROM dbo.BorRequiredTools WHERE BorToolsId =@id";

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteChild, parameters).FirstOrDefault();


                
            }

            var resultDeleteParent = @"DELETE FROM dbo.BorTools WHERE Id =@id";

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Query<string>(resultDeleteParent, parameters).FirstOrDefault();


                
            }
        }

        return borParameterResoruce.idList;
    }

    private async Task<string> CopyCpc(BorParameter borParameter, string resourceNumber, string connectionString,
        string environment)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        bool isCpcExist;
        var parameter = new CpcParameters
        {
            Lang = borParameter.Lang
        };

        await using (var context = new ShanukaDbContext(options, connectionString, borParameter.TenantProvider))
        {
            isCpcExist = context.CorporateProductCatalog.Any(c => c.ResourceNumber == resourceNumber);
        }

        if (isCpcExist == false)
        {
            parameter.Id = resourceNumber;
            switch (environment)
            {
                case "cu":
                case null:
                    parameter.ContractingUnitSequenceId = borParameter.ContractingUnitSequenceId;
                    break;
            }

            parameter.ContextAccessor = borParameter.ContextAccessor;
            parameter.TenantProvider = borParameter.TenantProvider;
            var cpc = await borParameter.ICoporateProductCatalogRepository.GetCorporateProductCatalogById(parameter);

            var cpcCreateDto = new CoperateProductCatalogCreateDto();
            if (cpc.CpcBasicUnitOfMeasure != null) cpcCreateDto.CpcBasicUnitOfMeasureId = cpc.CpcBasicUnitOfMeasure.Key;
            if (cpc.CpcBrand != null) cpcCreateDto.CpcBrandId = cpc.CpcBrand.Key;
            if (cpc.CpcPressureClass != null) cpcCreateDto.CpcPressureClassId = cpc.CpcPressureClass.Key;
            if (cpc.ResourceFamily != null) cpcCreateDto.ResourceFamilyId = cpc.ResourceFamily.Key;
            if (cpc.CpcUnitOfSizeMeasure != null) cpcCreateDto.CpcUnitOfSizeMeasureId = cpc.CpcUnitOfSizeMeasure.Key;
            cpcCreateDto.CpcMaterialId = cpc.CpcMaterialId;
            cpcCreateDto.Id = borParameter.CpcParameters.Id;
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
                var dto = new CpcResourceNicknameCreateDto
                {
                    CoperateProductCatalogId = resource.CoperateProductCatalogId,
                    Id = null,
                    Language = resource.Language,
                    LocaleCode = resource.LocaleCode,
                    NickName = resource.NickName
                };
                resouceList.Add(dto);
            }

            cpcCreateDto.CpcResourceNickname = resouceList;

            var imageList = new List<CpcImageCreateDto>();
            foreach (var image in cpc.CpcImage)
            {
                var dto = new CpcImageCreateDto
                {
                    Id = null,
                    Image = image.Image
                };
                imageList.Add(dto);
            }

            cpcCreateDto.CpcImage = imageList;

            var vendorList = new List<CpcVendorCreateDto>();
            foreach (var vendor in cpc.CpcVendor)
            {
                var dto = new CpcVendorCreateDto
                {
                    CompanyId = vendor.CompanyId,
                    CompanyName = vendor.Company.Name,
                    CoperateProductCatalogId = vendor.CoperateProductCatalogId,
                    Id = null,
                    MaxOrderQuantity = vendor.MaxOrderQuantity,
                    MinOrderQuantity = vendor.MinOrderQuantity,
                    PreferredParty = vendor.PreferredParty,
                    PurchasingUnit = vendor.PurchasingUnit,
                    ResourceLeadTime = vendor.ResourceLeadTime,
                    ResourceNumber = vendor.ResourceNumber,
                    ResourcePrice = vendor.ResourcePrice,
                    ResourceTitle = vendor.ResourceTitle,
                    RoundingValue = vendor.RoundingValue
                };
                vendorList.Add(dto);
            }

            cpcCreateDto.CpcVendor = vendorList;

            parameter.CpcDto = cpcCreateDto;
            parameter.isCopy = true;
            parameter.ProjectSequenceId = borParameter.ProjectSequenceId;
            parameter.ContractingUnitSequenceId = borParameter.ContractingUnitSequenceId;
            await borParameter.ICoporateProductCatalogRepository.CreateCoporateProductCatalog(parameter,
                borParameter.ContextAccessor).ConfigureAwait(false);
        }

        return "";
    }
}