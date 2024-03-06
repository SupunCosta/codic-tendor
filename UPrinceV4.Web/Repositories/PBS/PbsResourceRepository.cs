using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using log4net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class PbsResourceRepository : IPbsResourceRepository
{
    public async Task<string> CreatePbsConsumable(PbsResourceParameters pbsParameters)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
        using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
        {
            var consumable = new ConsumableForPbs();

            consumable.PbsProductId = pbsParameters.PbsConsumableCreateDto.PbsProductId;
            consumable.CoperateProductCatalogId = pbsParameters.PbsConsumableCreateDto.CoperateProductCatalogId;
            consumable.Quantity = pbsParameters.PbsConsumableCreateDto.Quantity;
            if (pbsParameters.PbsConsumableCreateDto.ResourceId == null)
            {
                consumable.Id = Guid.NewGuid().ToString();
                context.PbsConsumable.Add(consumable);
                await context.SaveChangesAsync();
            }
            else
            {
                consumable.Id = pbsParameters.PbsConsumableCreateDto.ResourceId;
                context.PbsConsumable.Update(consumable);
                await context.SaveChangesAsync();
            }

            if (pbsParameters.PbsConsumableCreateDto.Environment != "local" &&
                pbsParameters.PbsConsumableCreateDto.ResourceId == null)
            {
                var parameters = new CpcParameters
                {
                    Lang = pbsParameters.Lang,
                    Id = pbsParameters.PbsConsumableCreateDto.CoperateProductCatalogId
                };
                pbsParameters.cpcParameter = parameters;
                pbsParameters.Lang = pbsParameters.Lang;
                pbsParameters.cpcParameter.Oid = pbsParameters.UserId;
                await CopyCpc(pbsParameters, pbsParameters.PbsConsumableCreateDto.CoperateProductCatalogId,
                    connectionString, pbsParameters.PbsConsumableCreateDto.Environment);
            }

            if (pbsParameters.PbsConsumableCreateDto.Environment != null)
                if (pbsParameters.PbsConsumableCreateDto.Environment != "local" &&
                    pbsParameters.PbsConsumableCreateDto.ResourceId != null)
                {
                    var parameters = new CpcParameters
                    {
                        Lang = pbsParameters.Lang,
                        Id = pbsParameters.PbsConsumableCreateDto.CoperateProductCatalogId
                    };
                    pbsParameters.cpcParameter = parameters;
                    pbsParameters.Lang = pbsParameters.Lang;
                    pbsParameters.cpcParameter.Oid = pbsParameters.UserId;
                    await CopyCpc(pbsParameters, pbsParameters.PbsConsumableCreateDto.CoperateProductCatalogId,
                        connectionString, pbsParameters.PbsConsumableCreateDto.Environment);
                }

            return consumable.Id;
        }
    }

    public async Task<string> CreatePbsTool(PbsResourceParameters pbsParameters)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            {
                var tool = new ToolsForPbs
                {
                    PbsProductId = pbsParameters.PbsToolCreateDto.PbsProductId,
                    CoperateProductCatalogId = pbsParameters.PbsToolCreateDto.CoperateProductCatalogId,
                    Quantity = pbsParameters.PbsToolCreateDto.Quantity
                };

                if (pbsParameters.PbsToolCreateDto.ResourceId == null)
                {
                    tool.Id = Guid.NewGuid().ToString();
                    context.PbsTools.Add(tool);
                    await context.SaveChangesAsync();
                }
                else
                {
                    tool.Id = pbsParameters.PbsToolCreateDto.ResourceId;
                    context.PbsTools.Update(tool);
                    await context.SaveChangesAsync();
                }

                if (pbsParameters.PbsToolCreateDto.Environment != "local" &&
                    pbsParameters.PbsToolCreateDto.ResourceId == null)
                {
                    var parameters = new CpcParameters();
                    parameters.Lang = pbsParameters.Lang;
                    parameters.Id = pbsParameters.PbsToolCreateDto.CoperateProductCatalogId;
                    pbsParameters.cpcParameter = parameters;
                    pbsParameters.Lang = pbsParameters.Lang;
                    pbsParameters.cpcParameter.Oid = pbsParameters.UserId;

                    await CopyCpc(pbsParameters, pbsParameters.PbsToolCreateDto.CoperateProductCatalogId,
                        connectionString, pbsParameters.PbsToolCreateDto.Environment);
                }

                if (pbsParameters.PbsToolCreateDto.Environment != null)
                    if (pbsParameters.PbsToolCreateDto.Environment != "local" &&
                        pbsParameters.PbsToolCreateDto.ResourceId != null)
                    {
                        var parameters = new CpcParameters();
                        parameters.Lang = pbsParameters.Lang;
                        parameters.Id = pbsParameters.PbsToolCreateDto.CoperateProductCatalogId;
                        pbsParameters.cpcParameter = parameters;
                        pbsParameters.Lang = pbsParameters.Lang;
                        pbsParameters.cpcParameter.Oid = pbsParameters.UserId;

                        await CopyCpc(pbsParameters, pbsParameters.PbsToolCreateDto.CoperateProductCatalogId,
                            connectionString, pbsParameters.PbsToolCreateDto.Environment);
                    }


                return tool.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreatePbsMaterial(PbsResourceParameters pbsParameters)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            {
                var material = new MaterialForPbs();


                material.PbsProductId = pbsParameters.PbsMaterialCreateDto.PbsProductId;
                material.CoperateProductCatalogId = pbsParameters.PbsMaterialCreateDto.CoperateProductCatalogId;
                material.Quantity = pbsParameters.PbsMaterialCreateDto.Quantity;

                if (pbsParameters.PbsMaterialCreateDto.ResourceId == null)
                {
                    material.Id = Guid.NewGuid().ToString();
                    context.PbsMaterial.Add(material);
                    context.SaveChanges();
                }
                else
                {
                    material.Id = pbsParameters.PbsMaterialCreateDto.ResourceId;
                    context.PbsMaterial.Update(material);
                    context.SaveChanges();
                }

                if (pbsParameters.PbsMaterialCreateDto.Environment != "local" &&
                    pbsParameters.PbsMaterialCreateDto.ResourceId == null)
                {
                    var parameters = new CpcParameters();
                    parameters.Lang = pbsParameters.Lang;
                    parameters.Id = pbsParameters.PbsMaterialCreateDto.CoperateProductCatalogId;
                    pbsParameters.cpcParameter = parameters;
                    pbsParameters.Lang = pbsParameters.Lang;
                    pbsParameters.cpcParameter.Oid = pbsParameters.UserId;

                    await CopyCpc(pbsParameters, pbsParameters.PbsMaterialCreateDto.CoperateProductCatalogId,
                        connectionString, pbsParameters.PbsMaterialCreateDto.Environment);
                }

                if (pbsParameters.PbsMaterialCreateDto.Environment != null)
                    if (pbsParameters.PbsMaterialCreateDto.Environment != "local" &&
                        pbsParameters.PbsMaterialCreateDto.ResourceId != null)
                    {
                        var parameters = new CpcParameters();
                        parameters.Lang = pbsParameters.Lang;
                        parameters.Id = pbsParameters.PbsMaterialCreateDto.CoperateProductCatalogId;
                        pbsParameters.cpcParameter = parameters;
                        pbsParameters.Lang = pbsParameters.Lang;
                        pbsParameters.cpcParameter.Oid = pbsParameters.UserId;

                        await CopyCpc(pbsParameters, pbsParameters.PbsMaterialCreateDto.CoperateProductCatalogId,
                            connectionString, pbsParameters.PbsMaterialCreateDto.Environment);
                    }

                return material.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreatePbsLabour(PbsResourceParameters pbsParameters)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            await using var connection = new SqlConnection(connectionString);

            using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            {
                var Labour = new LabourForPbs();

                Labour.PbsProductId = pbsParameters.PbsLabourCreateDto.PbsProductId;
                Labour.CoperateProductCatalogId = pbsParameters.PbsLabourCreateDto.CoperateProductCatalogId;
                Labour.Quantity = pbsParameters.PbsLabourCreateDto.Quantity;

                if (pbsParameters.PbsLabourCreateDto.ResourceId == null)
                {
                    Labour.Id = Guid.NewGuid().ToString();
                    context.PbsLabour.Add(Labour);
                    context.SaveChanges();
                }
                else
                {
                    Labour.Id = pbsParameters.PbsLabourCreateDto.ResourceId;
                    context.PbsLabour.Update(Labour);
                    context.SaveChanges();
                }

                if (pbsParameters.PbsLabourCreateDto.Environment != "local" &&
                    pbsParameters.PbsLabourCreateDto.ResourceId == null)
                {
                    //CpcParameters parameters = new CpcParameters();
                    //pbsParameters.cpcParameter = parameters;

                    var parameters = new CpcParameters();
                    parameters.Lang = pbsParameters.Lang;
                    parameters.Id = pbsParameters.PbsLabourCreateDto.CoperateProductCatalogId;
                    pbsParameters.cpcParameter = parameters;
                    pbsParameters.Lang = pbsParameters.Lang;
                    pbsParameters.cpcParameter.Oid = pbsParameters.UserId;

                    await CopyCpc(pbsParameters, pbsParameters.PbsLabourCreateDto.CoperateProductCatalogId,
                        connectionString, pbsParameters.PbsLabourCreateDto.Environment);
                }

                if (pbsParameters.PbsLabourCreateDto.Environment != null)
                    if (pbsParameters.PbsLabourCreateDto.Environment != "local" &&
                        pbsParameters.PbsLabourCreateDto.ResourceId != null)
                    {
                        var parameters = new CpcParameters();
                        parameters.Lang = pbsParameters.Lang;
                        parameters.Id = pbsParameters.PbsLabourCreateDto.CoperateProductCatalogId;
                        pbsParameters.cpcParameter = parameters;
                        pbsParameters.Lang = pbsParameters.Lang;
                        pbsParameters.cpcParameter.Oid = pbsParameters.UserId;

                        await CopyCpc(pbsParameters, pbsParameters.PbsLabourCreateDto.CoperateProductCatalogId,
                            connectionString, pbsParameters.PbsLabourCreateDto.Environment);
                    }

                pbsParameters.PbsProductId = connection
                    .Query<string>("Select ProductId  From PbsProduct  WHERE Id = @Id ",
                        new { Id = pbsParameters.PbsLabourCreateDto.PbsProductId }).FirstOrDefault();
                await PbsParentDateAdjust(pbsParameters);

                var mPbsAssignedLabourDto = new PbsAssignedLabourDto()
                {
                    PbsProduct = pbsParameters.PbsProductId,
                    Project = pbsParameters.ProjectSequenceId
                };
                pbsParameters.PbsAssignedLabour = mPbsAssignedLabourDto;

                await PbsLabourAssignReCalculate(pbsParameters);
                return Labour.Id;
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    public async Task DeletePbsLabour(PbsResourceParameters pbsParameters)
    {
        try
        {
            //var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            //using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            //{
            //    foreach (string id in pbsParameters.IdList)
            //    {
            //        LabourForPbs labour = (from a in context.PbsLabour
            //                               where a.Id == id
            //                               select a).Single();
            //        context.PbsLabour.Remove(labour);
            //        context.SaveChanges();
            //    }
            //}
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM dbo.PbsLabour WHERE Id IN @Ids";
                await connection.ExecuteAsync(query, new { Ids = pbsParameters.IdList });
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task DeletePbsMaterial(PbsResourceParameters pbsParameters)
    {
        try
        {
            //var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            //using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            //{
            //    foreach (string id in pbsParameters.IdList)
            //    {
            //        MaterialForPbs material = (from a in context.PbsMaterial
            //                                   where a.Id == id
            //                                   select a).Single();
            //        context.PbsMaterial.Remove(material);
            //        context.SaveChanges();
            //    }
            //    string query = "DELETE FROM dbo.PbsMaterial WHERE Id IN @Ids";


            //}

            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM dbo.PbsMaterial WHERE Id IN @Ids";
                await connection.ExecuteAsync(query, new { Ids = pbsParameters.IdList });
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task DeletePbsTool(PbsResourceParameters pbsParameters)
    {
        try
        {
            //var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            //using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            //{
            //    foreach (string id in pbsParameters.IdList)
            //    {
            //        ToolsForPbs tool = (from a in context.PbsTools
            //                            where a.Id == id
            //                            select a).Single();
            //        context.PbsTools.Remove(tool);
            //        context.SaveChanges();
            //    }
            //}

            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM dbo.PbsTools WHERE Id IN @Ids";
                await connection.ExecuteAsync(query, new { Ids = pbsParameters.IdList });
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task DeletePbsConsumable(PbsResourceParameters pbsParameters)
    {
        try
        {
            //var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            //using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            //{
            //    foreach (string id in pbsParameters.IdList)
            //    {
            //        ConsumableForPbs consumable = (from a in context.PbsConsumable
            //                                       where a.Id == id
            //                                       select a).Single();
            //        context.PbsConsumable.Remove(consumable);
            //        context.SaveChanges();
            //    }
            //}

            using (var connection = new SqlConnection(connectionString))
            {
                var query = "DELETE FROM dbo.PbsConsumable WHERE Id IN @Ids";
                await connection.ExecuteAsync(query, new { Ids = pbsParameters.IdList });
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<CpcForProductDto>> GetMaterial(PbsResourceParameters pbsParameters)
    {
        try
        {
            var query = @"
                              	 SELECT CorporateProductCatalog.Id AS [Key]
								,CorporateProductCatalog.Title AS Text
                                FROM dbo.CorporateProductCatalog
								WHERE CorporateProductCatalog.ResourceTypeId = 'c46c3a26-39a5-42cc-n7k1-89655304eh6'
								AND CorporateProductCatalog.Title like '%" + pbsParameters.Name?.Replace("'", "''") +
                        "%' ";

            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);


            using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<CpcForProductDto>(query);
                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<CpcForProductDto>> GetLabour(PbsResourceParameters pbsParameters)
    {
        try
        {
            var query = @"
                              	 SELECT CorporateProductCatalog.Id AS [Key]
								,CorporateProductCatalog.Title AS Text
                                FROM dbo.CorporateProductCatalog
								WHERE CorporateProductCatalog.ResourceTypeId = 'c46c3a26-39a5-42cc-b07s-89655304eh6'
								AND CorporateProductCatalog.Title like '%" + pbsParameters.Name?.Replace("'", "''") +
                        "%' ";

            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

            using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<CpcForProductDto>(query);
                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<CpcForProductDto>> GetTool(PbsResourceParameters pbsParameters)
    {
        try
        {
            var query = @"
                              	 SELECT CorporateProductCatalog.Id AS [Key]
								,CorporateProductCatalog.Title AS Text
                                FROM dbo.CorporateProductCatalog
								WHERE CorporateProductCatalog.ResourceTypeId = 'c46c3a26-39a5-42cc-n9wn-89655304eh6'
								AND CorporateProductCatalog.Title like '%" + pbsParameters.Name?.Replace("'", "''") +
                        "%' ";

            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

            using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<CpcForProductDto>(query);
                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<CpcForProductDto>> GetConsumable(PbsResourceParameters pbsParameters)
    {
        try
        {
            var query = @"
                              	 SELECT CorporateProductCatalog.Id AS [Key]
								,CorporateProductCatalog.Title AS Text
                                FROM dbo.CorporateProductCatalog
								WHERE CorporateProductCatalog.ResourceTypeId = 'c46c3a26-39a5-42cc-m06g-89655304eh6'
								AND CorporateProductCatalog.Title like '%" + pbsParameters.Name?.Replace("'", "''") +
                        "%' ";

            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<CpcForProductDto>(query);
                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PbsResourceReadDto>> GetMaterialByProductId(
        PbsResourceParameters pbsResourceParameters)
    {
        var _log4net = LogManager.GetLogger(typeof(PbsResourceRepository));
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                pbsResourceParameters.ContractingUnitSequenceId, pbsResourceParameters.ProjectSequenceId,
                pbsResourceParameters.TenantProvider);
            IEnumerable<MaterialForPbs> pbsMaterial;


            await using var connection = new SqlConnection(connectionString);

            pbsMaterial = await connection
                .QueryAsync<MaterialForPbs, CorporateProductCatalog, CpcBasicUnitOfMeasure, CpcResourceFamily,
                    CpcResourceType, MaterialForPbs>("GetMaterialByProductId",
                    (materialForPbs, corporateProductCatalog, cpcBasicUnitOfMeasure, cpcResourceFamily,
                        cpcResourceType) =>
                    {
                        materialForPbs.CorporateProductCatalog = corporateProductCatalog;
                        if (cpcResourceFamily != null)
                        {
                            corporateProductCatalog.ResourceFamily = cpcResourceFamily;
                        }

                        if (cpcBasicUnitOfMeasure != null)
                        {
                            corporateProductCatalog.CpcBasicUnitOfMeasure = cpcBasicUnitOfMeasure;
                        }

                        if (cpcResourceType != null)
                        {
                            corporateProductCatalog.ResourceType = cpcResourceType;
                        }

                        return materialForPbs;
                    }, splitOn: "Id,Id", param: new { PbsProductId = pbsResourceParameters.PbsProductId },
                    commandType: CommandType.StoredProcedure);
            // string stringFromJson = JsonToStringConverter.getStringFromJson(pbsMaterial);
            // _log4net.Error("Create " + stringFromJson);
            var pbsResourceReadDtos = new List<PbsResourceReadDto>();
            pbsResourceParameters.IdList = new List<string>()
            {
                pbsResourceParameters.PbsProductId
            };
            var cons = GetPbsResourceConsolidatedQuantity(pbsResourceParameters, "PbsMaterial").Result
                .FirstOrDefault(x => x.PbsId == pbsResourceParameters.PbsProductId);
            foreach (var meterial in pbsMaterial)
            {
                var readDto = new PbsResourceReadDto
                {
                    Quantity = meterial.Quantity,
                    ResourceId = meterial.Id
                };

                if (meterial.CorporateProductCatalog != null)
                {
                    readDto.CpcTitle = meterial.CorporateProductCatalog.Title;
                    readDto.CpcKey = meterial.CorporateProductCatalog.Key;
                    readDto.CpcText = meterial.CorporateProductCatalog.Text;
                    readDto.ResourceTypeId = meterial.CorporateProductCatalog.ResourceTypeId;

                    if (meterial.CorporateProductCatalog.ResourceFamily != null)
                    {
                        if (meterial.CorporateProductCatalog.ResourceFamily.Title != null)
                            readDto.ResourceFamilyTitle = meterial.CorporateProductCatalog.ResourceFamily.Title;
                        else
                            readDto.ResourceFamilyTitle = "-";
                    }
                    else
                    {
                        readDto.ResourceFamilyTitle = "-";
                    }

                    if (meterial.CorporateProductCatalog.CpcBasicUnitOfMeasure != null)
                    {
                        if (meterial.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name != null)
                            readDto.Unit = meterial.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name;
                        else
                            readDto.Unit = "-";
                    }
                    else
                    {
                        readDto.Unit = "-";
                    }

                    readDto.ConsolidatedQuantity = cons?.res
                        .Where(v => v.CpcKey == meterial.CorporateProductCatalog.Key).Sum(x => x.ConsolidatedQuantity);
                }

                pbsResourceReadDtos.Add(readDto);
            }

            return pbsResourceReadDtos;
        }
        catch (Exception e)
        {
            string stringFromJson = JsonToStringConverter.getStringFromJson(pbsResourceParameters);
            _log4net.Info("Exception thrown" + stringFromJson, e);
            _log4net.Error(e.ToString());
            _log4net.Error("Create " + stringFromJson);
            //_log4net.Error(pbsResourceParameters.ToString());
            throw new Exception(stringFromJson);
        }
    }

    public async Task<IEnumerable<PbsResourceReadDto>> GetToolByProductId(
        PbsResourceParameters pbsResourceParameters)
    {
        var _log4net = LogManager.GetLogger(typeof(PbsResourceRepository));
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                pbsResourceParameters.ContractingUnitSequenceId, pbsResourceParameters.ProjectSequenceId,
                pbsResourceParameters.TenantProvider);

            IEnumerable<ToolsForPbs> pbsTools;
            await using var connection = new SqlConnection(connectionString);

            pbsTools = await connection
                .QueryAsync<ToolsForPbs, CorporateProductCatalog, CpcBasicUnitOfMeasure, CpcResourceFamily,
                    CpcResourceType, ToolsForPbs>("GetToolByProductId",
                    (toolsForPbs, corporateProductCatalog, cpcBasicUnitOfMeasure, cpcResourceFamily,
                        cpcResourceType) =>
                    {
                        toolsForPbs.CorporateProductCatalog = corporateProductCatalog;
                        if (cpcResourceFamily != null)
                        {
                            corporateProductCatalog.ResourceFamily = cpcResourceFamily;
                        }

                        if (cpcBasicUnitOfMeasure != null)
                        {
                            corporateProductCatalog.CpcBasicUnitOfMeasure = cpcBasicUnitOfMeasure;
                        }

                        if (cpcResourceType != null)
                        {
                            corporateProductCatalog.ResourceType = cpcResourceType;
                        }

                        return toolsForPbs;
                    }, splitOn: "Id,Id", param: new { PbsProductId = pbsResourceParameters.PbsProductId },
                    commandType: CommandType.StoredProcedure);
            // string stringFromJson = JsonToStringConverter.getStringFromJson(pbsTools);
            // _log4net.Error("Create " + stringFromJson);
            var pbsResourceReadDtos = new List<PbsResourceReadDto>();

            pbsResourceParameters.IdList = new List<string>()
            {
                pbsResourceParameters.PbsProductId
            };
            var cons = GetPbsResourceConsolidatedQuantity(pbsResourceParameters, "PbsTools").Result
                .FirstOrDefault(x => x.PbsId == pbsResourceParameters.PbsProductId);

            foreach (var tool in pbsTools)
            {
                var readDto = new PbsResourceReadDto
                {
                    Quantity = tool.Quantity,
                    ResourceId = tool.Id
                };

                if (tool.CorporateProductCatalog != null)
                {
                    readDto.CpcTitle = tool.CorporateProductCatalog.Title;
                    readDto.CpcKey = tool.CorporateProductCatalog.Key;
                    readDto.CpcText = tool.CorporateProductCatalog.Text;
                    readDto.ResourceTypeId = tool.CorporateProductCatalog.ResourceTypeId;

                    if (tool.CorporateProductCatalog.ResourceFamily != null)
                    {
                        if (tool.CorporateProductCatalog.ResourceFamily.Title != null)
                            readDto.ResourceFamilyTitle = tool.CorporateProductCatalog.ResourceFamily.Title;
                        else
                            readDto.ResourceFamilyTitle = "-";
                    }
                    else
                    {
                        readDto.ResourceFamilyTitle = "-";
                    }

                    if (tool.CorporateProductCatalog.CpcBasicUnitOfMeasure != null)
                    {
                        if (tool.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name != null)
                            readDto.Unit = tool.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name;
                        else
                            readDto.Unit = "-";
                    }
                    else
                    {
                        readDto.Unit = "-";
                    }

                    readDto.ConsolidatedQuantity = cons?.res.Where(v => v.CpcKey == tool.CorporateProductCatalog.Key)
                        .Sum(x => x.ConsolidatedQuantity);
                }

                pbsResourceReadDtos.Add(readDto);
            }

            return pbsResourceReadDtos;
        }
        catch (Exception e)
        {
            string stringFromJson = JsonToStringConverter.getStringFromJson(pbsResourceParameters);
            _log4net.Info("Exception thrown" + stringFromJson, e);
            _log4net.Error(e.ToString());
            _log4net.Error("Create " + stringFromJson);
            //_log4net.Error(pbsResourceParameters.ToString());
            throw new Exception(stringFromJson);
        }
    }

    public async Task<IEnumerable<PbsResourceReadDto>> GetLabourByProductId(
        PbsResourceParameters pbsResourceParameters)
    {
        var _log4net = LogManager.GetLogger(typeof(PbsResourceRepository));

        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                pbsResourceParameters.ContractingUnitSequenceId, pbsResourceParameters.ProjectSequenceId,
                pbsResourceParameters.TenantProvider);
            IEnumerable<LabourForPbs> pbsLabour;


            await using var connection = new SqlConnection(connectionString);

            pbsLabour = await connection
                .QueryAsync<LabourForPbs, CorporateProductCatalog, CpcBasicUnitOfMeasure, CpcResourceFamily,
                    CpcResourceType,
                    LabourForPbs>("GetLabourByProductId",
                    (labourForPbs, corporateProductCatalog, cpcBasicUnitOfMeasure, cpcResourceFamily,
                        cpcResourceType) =>
                    {
                        labourForPbs.CorporateProductCatalog = corporateProductCatalog;
                        if (cpcResourceFamily != null)
                        {
                            corporateProductCatalog.ResourceFamily = cpcResourceFamily;
                        }

                        if (cpcBasicUnitOfMeasure != null)
                        {
                            corporateProductCatalog.CpcBasicUnitOfMeasure = cpcBasicUnitOfMeasure;
                        }

                        if (cpcResourceType != null)
                        {
                            corporateProductCatalog.ResourceType = cpcResourceType;
                        }


                        return labourForPbs;
                    }, splitOn: "Id,Id", param: new { PbsProductId = pbsResourceParameters.PbsProductId },
                    commandType: CommandType.StoredProcedure);


            var pbsResourceReadDtos = new List<PbsResourceReadDto>();
            // string stringFromJson = JsonToStringConverter.getStringFromJson(pbsLabour);
            // _log4net.Error("Create " + stringFromJson);
            pbsResourceParameters.IdList = new List<string>()
            {
                pbsResourceParameters.PbsProductId
            };
            var cons = GetPbsResourceConsolidatedQuantity(pbsResourceParameters, "PbsLabour").Result
                .FirstOrDefault(x => x.PbsId == pbsResourceParameters.PbsProductId);
            foreach (var labour in pbsLabour)
            {
                var readDto = new PbsResourceReadDto
                {
                    Quantity = labour.Quantity,
                    ResourceId = labour.Id
                };

                if (labour.CorporateProductCatalog != null)
                {
                    readDto.CpcTitle = labour.CorporateProductCatalog.Title;
                    readDto.CpcKey = labour.CorporateProductCatalog.Key;
                    readDto.CpcText = labour.CorporateProductCatalog.Text;
                    readDto.ResourceTypeId = labour.CorporateProductCatalog.ResourceTypeId;

                    if (labour.CorporateProductCatalog.ResourceFamily != null)
                    {
                        if (labour.CorporateProductCatalog.ResourceFamily.Title != null)
                            readDto.ResourceFamilyTitle = labour.CorporateProductCatalog.ResourceFamily.Title;
                        else
                            readDto.ResourceFamilyTitle = "-";
                    }
                    else
                    {
                        readDto.ResourceFamilyTitle = "-";
                    }

                    if (labour.CorporateProductCatalog.CpcBasicUnitOfMeasure != null)
                    {
                        if (labour.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name != null)
                            readDto.Unit = labour.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name;
                        else
                            readDto.Unit = "-";
                    }
                    else
                    {
                        readDto.Unit = "-";
                    }


                    readDto.ConsolidatedQuantity = cons?.res.Where(v => v.CpcKey == labour.CorporateProductCatalog.Key)
                        .Sum(x => x.ConsolidatedQuantity);
                }

                pbsResourceReadDtos.Add(readDto);
            }

            return pbsResourceReadDtos;
        }
        catch (Exception e)
        {
            string stringFromJson = JsonToStringConverter.getStringFromJson(pbsResourceParameters);
            _log4net.Info("Exception thrown" + stringFromJson, e);
            _log4net.Error(e.ToString());
            _log4net.Error("Create " + stringFromJson);
            //_log4net.Error(pbsResourceParameters.ToString());
            throw new Exception(stringFromJson);
        }
    }

    public async Task<IEnumerable<PbsResourceReadDto>> GetConsumableByProductId(
        PbsResourceParameters pbsResourceParameters)
    {
        var _log4net = LogManager.GetLogger(typeof(PbsResourceRepository));
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                pbsResourceParameters.ContractingUnitSequenceId, pbsResourceParameters.ProjectSequenceId,
                pbsResourceParameters.TenantProvider);
            IEnumerable<ConsumableForPbs> pbsConsumable;


            await using var connection = new SqlConnection(connectionString);

            pbsConsumable = await connection
                .QueryAsync<ConsumableForPbs, CorporateProductCatalog, CpcBasicUnitOfMeasure, CpcResourceFamily,
                    CpcResourceType, ConsumableForPbs>("GetConsumableByProductId",
                    (consumableForPbs, corporateProductCatalog, cpcBasicUnitOfMeasure, cpcResourceFamily,
                        cpcResourceType) =>
                    {
                        consumableForPbs.CorporateProductCatalog = corporateProductCatalog;
                        if (cpcResourceFamily != null)
                        {
                            corporateProductCatalog.ResourceFamily = cpcResourceFamily;
                        }

                        if (cpcBasicUnitOfMeasure != null)
                        {
                            corporateProductCatalog.CpcBasicUnitOfMeasure = cpcBasicUnitOfMeasure;
                        }

                        if (cpcResourceType != null)
                        {
                            corporateProductCatalog.ResourceType = cpcResourceType;
                        }

                        return consumableForPbs;
                    }, splitOn: "Id,Id", param: new { PbsProductId = pbsResourceParameters.PbsProductId },
                    commandType: CommandType.StoredProcedure);
            // string stringFromJson = JsonToStringConverter.getStringFromJson(pbsConsumable);
            // _log4net.Error("Create " + stringFromJson);
            var pbsResourceReadDtos = new List<PbsResourceReadDto>();
            pbsResourceParameters.IdList = new List<string>()
            {
                pbsResourceParameters.PbsProductId
            };
            var cons = GetPbsResourceConsolidatedQuantity(pbsResourceParameters, "PbsConsumable").Result
                .FirstOrDefault(x => x.PbsId == pbsResourceParameters.PbsProductId);

            foreach (var consumable in pbsConsumable)
            {
                var readDto = new PbsResourceReadDto
                {
                    Quantity = consumable.Quantity,
                    ResourceId = consumable.Id
                };

                if (consumable.CorporateProductCatalog != null)
                {
                    readDto.CpcTitle = consumable.CorporateProductCatalog.Title;
                    readDto.CpcKey = consumable.CorporateProductCatalog.Key;
                    readDto.CpcText = consumable.CorporateProductCatalog.Text;
                    readDto.ResourceTypeId = consumable.CorporateProductCatalog.ResourceTypeId;
                    if (consumable.CorporateProductCatalog.ResourceFamily != null)
                    {
                        if (consumable.CorporateProductCatalog.ResourceFamily.Title != null)
                            readDto.ResourceFamilyTitle = consumable.CorporateProductCatalog.ResourceFamily.Title;
                        else
                            readDto.ResourceFamilyTitle = "-";
                    }
                    else
                    {
                        readDto.ResourceFamilyTitle = "-";
                    }

                    if (consumable.CorporateProductCatalog.CpcBasicUnitOfMeasure != null)
                    {
                        if (consumable.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name != null)
                            readDto.Unit = consumable.CorporateProductCatalog.CpcBasicUnitOfMeasure.Name;
                        else
                            readDto.Unit = "-";
                    }
                    else
                    {
                        readDto.Unit = "-";
                    }


                    readDto.ConsolidatedQuantity = cons?.res
                        .Where(v => v.CpcKey == consumable.CorporateProductCatalog.Key)
                        .Sum(x => x.ConsolidatedQuantity);
                }

                pbsResourceReadDtos.Add(readDto);
            }

            return pbsResourceReadDtos;
        }
        catch (Exception e)
        {
            string stringFromJson = JsonToStringConverter.getStringFromJson(pbsResourceParameters);
            _log4net.Info("Exception thrown" + stringFromJson, e);
            _log4net.Error(e.ToString());
            _log4net.Error("Create " + stringFromJson);
            //_log4net.Error(pbsResourceParameters.ToString());
            throw new Exception(stringFromJson);
        }
    }

    public async Task<string> CreatePbsService(PbsResourceParameters pbsParameters)
    {
        try
        {
            //var options = new DbContextOptions<ShanukaDbContext>();
            // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
                pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);
            PbsServiceGetByIdDto data;
            ServiceDocuments docData;
            string newId = null;
            if (pbsParameters.PbsServiceCreateDto.ProductId != null)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    data = connection
                        .Query<PbsServiceGetByIdDto>("SELECT * FROM PbsService WHERE ProductId =@ProductId",
                            new { pbsParameters.PbsServiceCreateDto.ProductId }).FirstOrDefault();
                }

                if (data == null)
                {
                    var insertQuery =
                        @"INSERT INTO PbsService ([Id], [ProductId], [Quantity], [MouId], [UnitPrice], [TotalPrice], [Comments]) VALUES (@Id, @ProductId, @Quantity, @MouId, @UnitPrice, @TotalPrice, @Comments)";
                    newId = Guid.NewGuid().ToString();
                    var parameters = new
                    {
                        Id = newId,
                        pbsParameters.PbsServiceCreateDto.ProductId,
                        pbsParameters.PbsServiceCreateDto.Quantity,
                        MouId = pbsParameters.PbsServiceCreateDto.Mou,
                        pbsParameters.PbsServiceCreateDto.UnitPrice,
                        pbsParameters.PbsServiceCreateDto.TotalPrice,
                        pbsParameters.PbsServiceCreateDto.Comments
                    };

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Execute(insertQuery, parameters);
                    }

                    if (pbsParameters.PbsServiceCreateDto.Documents != null)
                        if (pbsParameters.PbsServiceCreateDto.Documents.FirstOrDefault() != null)
                            foreach (var doc in pbsParameters.PbsServiceCreateDto.Documents)
                            {
                                var docInsert =
                                    @"INSERT INTO ServiceDocuments ([Id], [Link], [ProductId], [ServiceId]) VALUES (@Id, @Link, @ProductId, @ServiceId)";

                                var param = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Link = doc,
                                    pbsParameters.PbsServiceCreateDto.ProductId,
                                    ServiceId = newId
                                };

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(docInsert, param);
                                }
                            }
                }
                else
                {
                    var updateQuery =
                        @"UPDATE PbsService SET  Quantity = @Quantity, MouId = @MouId, UnitPrice = @UnitPrice, TotalPrice = @TotalPrice, Comments = @Comments WHERE Id = @Id";

                    var parameters = new
                    {
                        data.Id,
                        pbsParameters.PbsServiceCreateDto.Quantity,
                        MouId = pbsParameters.PbsServiceCreateDto.Mou,
                        pbsParameters.PbsServiceCreateDto.UnitPrice,
                        pbsParameters.PbsServiceCreateDto.TotalPrice,
                        pbsParameters.PbsServiceCreateDto.Comments
                    };

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Execute(updateQuery, parameters);
                        docData = connection
                            .Query<ServiceDocuments>("SELECT * FROM ServiceDocuments WHERE ProductId =@ProductId",
                                new { pbsParameters.PbsServiceCreateDto.ProductId }).FirstOrDefault();
                    }

                    if (pbsParameters.PbsServiceCreateDto.Documents != null)
                        if (pbsParameters.PbsServiceCreateDto.Documents.FirstOrDefault() != null)
                        {
                            if (docData == null)
                                foreach (var doc in pbsParameters.PbsServiceCreateDto.Documents)
                                {
                                    var docInsert =
                                        @"INSERT INTO ServiceDocuments ([Id], [Link], [ProductId], [ServiceId]) VALUES (@Id, @Link, @ProductId, @ServiceId)";

                                    var param = new
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Link = doc,
                                        pbsParameters.PbsServiceCreateDto.ProductId,
                                        ServiceId = data.Id
                                    };

                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        connection.Execute(docInsert, param);
                                    }
                                }
                            else
                                foreach (var doc in pbsParameters.PbsServiceCreateDto.Documents)
                                {
                                    var docInsert =
                                        @"INSERT INTO ServiceDocuments ([Id], [Link], [ProductId], [ServiceId]) VALUES (@Id, @Link, @ProductId, @ServiceId)";

                                    var param = new
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Link = doc,
                                        pbsParameters.PbsServiceCreateDto.ProductId,
                                        ServiceId = data.Id
                                    };

                                    string docLink;

                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        docLink = connection
                                            .Query<string>("SELECT Id FROM ServiceDocuments WHERE Link =@doc",
                                                new { doc }).FirstOrDefault();
                                    }

                                    if (docLink == null)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            connection.Execute(docInsert, param);
                                        }
                                }
                        }
                }
            }

            return newId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> PbsAssignedLabourDelete(PbsResourceParameters pbsParameters)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            null, pbsParameters.TenantProvider);

        await using var connection = new SqlConnection(cuConnectionString);

        await connection.ExecuteAsync(
            @"DELETE FROM dbo.PbsAssignedLabour WHERE Project = @Project AND CpcId = @CpcId AND CabPersonId = @CabPersonId",
            pbsParameters.PbsAssignedLabourDelete);

        return pbsParameters.PbsAssignedLabourDelete.CabPersonId;
    }

    public async Task<IEnumerable<PbsServiceGetByIdDto>> ReadServiceByPbsProduct(
        PbsResourceParameters pbsResourceParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(
            pbsResourceParameters.ContractingUnitSequenceId, pbsResourceParameters.ProjectSequenceId,
            pbsResourceParameters.TenantProvider);
        try
        {
            IEnumerable<PbsServiceGetByIdDto> data;
            List<string> doc;

            using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<PbsServiceGetByIdDto>(
                    "SELECT * ,MouId AS Mou FROM PbsService WHERE ProductId =@ProductId",
                    new { ProductId = pbsResourceParameters.PbsProductId });
            }

            if (data.FirstOrDefault() != null)
            {
                var documents = @"SELECT Link FROM dbo.ServiceDocuments WHERE ProductId =@Id";
                //var getMou = @"SELECT
                //              CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId AS [Key]
                //             ,CpcBasicUnitOfMeasureLocalizedData.Label AS [Text]
                //            FROM dbo.PbsService
                //            INNER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
                //              ON PbsService.MouId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                //              WHERE CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang AND PbsService.ProductId = @Id";

                using (var connection = new SqlConnection(connectionString))
                {
                    doc = connection.Query<string>(documents, new { Id = pbsResourceParameters.PbsProductId })
                        .ToList();
                    data.FirstOrDefault().ProductTitle = connection
                        .Query<string>("SELECT Title FROM PbsProduct WHERE Id = @Id",
                            new { Id = pbsResourceParameters.PbsProductId }).FirstOrDefault();
                }

                using (var connection =
                       new SqlConnection(pbsResourceParameters.TenantProvider.GetTenant().ConnectionString))
                {
                    data.FirstOrDefault().ProjectTitle = connection
                        .Query<string>("SELECT Title FROM ProjectDefinition WHERE SequenceCode = @SequenceCode",
                            new { SequenceCode = pbsResourceParameters.ProjectSequenceId }).FirstOrDefault();
                }

                if (doc.FirstOrDefault() != null) data.FirstOrDefault().Documents = doc;
            }

            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<List<DatabasesEx>> PbsResourcesEnvironment(PbsResourceParameters pbsResourceParameters)
    {
        // var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0009\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0020\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"P0055\"},{\"DatabaseName\":\"P0056\"},{\"DatabaseName\":\"P0057\"},{\"DatabaseName\":\"P0058\"},{\"DatabaseName\":\"P0059\"},{\"DatabaseName\":\"P0060\"},{\"DatabaseName\":\"P0061\"},{\"DatabaseName\":\"P0062\"},{\"DatabaseName\":\"P0063\"},{\"DatabaseName\":\"P0064\"},{\"DatabaseName\":\"P0065\"},{\"DatabaseName\":\"P0066\"},{\"DatabaseName\":\"P0067\"},{\"DatabaseName\":\"P0068\"},{\"DatabaseName\":\"P0069\"},{\"DatabaseName\":\"P0070\"},{\"DatabaseName\":\"P0071\"},{\"DatabaseName\":\"P0072\"},{\"DatabaseName\":\"P0073\"},{\"DatabaseName\":\"P0074\"},{\"DatabaseName\":\"P0075\"},{\"DatabaseName\":\"P0076\"},{\"DatabaseName\":\"P0077\"},{\"DatabaseName\":\"P0078\"},{\"DatabaseName\":\"P0079\"},{\"DatabaseName\":\"P0080\"},{\"DatabaseName\":\"P0081\"},{\"DatabaseName\":\"P0082\"},{\"DatabaseName\":\"P0083\"},{\"DatabaseName\":\"P0084\"},{\"DatabaseName\":\"P0085\"},{\"DatabaseName\":\"P0086\"},{\"DatabaseName\":\"P0087\"},{\"DatabaseName\":\"P0088\"},{\"DatabaseName\":\"P0089\"},{\"DatabaseName\":\"P0090\"},{\"DatabaseName\":\"P0091\"},{\"DatabaseName\":\"P0092\"},{\"DatabaseName\":\"P0093\"},{\"DatabaseName\":\"P0094\"},{\"DatabaseName\":\"P0095\"},{\"DatabaseName\":\"P0096\"},{\"DatabaseName\":\"P0097\"},{\"DatabaseName\":\"P0098\"},{\"DatabaseName\":\"P0099\"},{\"DatabaseName\":\"P0100\"},{\"DatabaseName\":\"P0101\"},{\"DatabaseName\":\"P0102\"},{\"DatabaseName\":\"P0103\"},{\"DatabaseName\":\"P0104\"},{\"DatabaseName\":\"P0105\"},{\"DatabaseName\":\"P0106\"},{\"DatabaseName\":\"P0107\"},{\"DatabaseName\":\"UPrinceV4Einstein\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"}]";
        //var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"COM0053\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"},{\"DatabaseName\":\"UPrinceV4UAT\"}]";

        var result = new List<Databases>();
        string env = null;
        //uprincev4uatdb
        //uprincev4einstein
        // var env = "uprincev4einstein";
        var data = new List<PbsResourcesEnvDto>();
        var exceptionLst = new List<DatabasesEx>();
        if (pbsResourceParameters.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
        {
            env = "uprincev4uatdb";
        }
        else if (pbsResourceParameters.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
        {
            env = "uprincev4einstein";
            ;
        }

        using (var connection = new SqlConnection("Server=tcp:" + env +
                                                  ".database.windows.net,1433;Initial Catalog=master;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        {
            result = connection.Query<Databases>(
                    @"select [name] as DatabaseName from sys.databases WHERE name NOT IN('master', 'MsalTokenCacheDatabase', 'UPrinceV4EinsteinCatelog', 'UPrinceV4UATCatelog', 'COM0001', 'UPrinceV4Einstein', 'UPrinceV4UAT') order by name")
                .ToList();
        }

        var PbsSql = "SELECT * FROM PbsProduct";
        foreach (var project in result)
            try
            {
                var connectionString = "Server=tcp:" + env + ".database.windows.net,1433;Initial Catalog=" +
                                       project.DatabaseName +
                                       ";Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


                using (var connection = new SqlConnection(connectionString))
                {
                    //    var pbsData = connection.Query<PbsDto>(PbsSql);


                    //    foreach (var k in pbsData)
                    //    {

                    var pbsMaterials = connection.Query<PbsResourcesEnvDto>("SELECT * FROM PbsMaterial");
                    var pbsTools = connection.Query<PbsResourcesEnvDto>("SELECT * FROM PbsTools");
                    var PbsConsumable = connection.Query<PbsResourcesEnvDto>("SELECT * FROM PbsConsumable");
                    var PbsLabour = connection.Query<PbsResourcesEnvDto>("SELECT * FROM PbsLabour");


                    foreach (var m in pbsMaterials)
                    {
                        var projectCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, project.DatabaseName,
                            pbsResourceParameters.TenantProvider);
                        var CuCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, null,
                            pbsResourceParameters.TenantProvider);
                        var orgCon =
                            ConnectionString.MapConnectionString(null, null, pbsResourceParameters.TenantProvider);

                        using (var newConnection = new SqlConnection(projectCon))
                        {
                            var cpc = newConnection
                                .Query<CorporateProductCatalog>(
                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                            if (cpc != null)
                            {
                                //consumable.CorporateProductCatalog = cpc;
                                //m.CPCTitle = cpc.Title;
                                //m.ProjectSequenceCode = project.DatabaseName;
                                //m.Environment = "local";

                                //data.Add(m);
                            }
                            else
                            {
                                using (var newCuConnection = new SqlConnection(CuCon))
                                {
                                    var cuCpc = newCuConnection
                                        .Query<CorporateProductCatalog>(
                                            "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                            new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                    if (cuCpc != null)
                                        //consumable.CorporateProductCatalog = cpc;
                                        //    m.CPCTitle = cuCpc.Title;
                                        //    m.ProjectSequenceCode = project.DatabaseName;
                                        //    m.Environment = "cu";

                                        //data.Add(m);

                                        await CopyCpcFromCuToProject(pbsResourceParameters, cuCpc.Id, projectCon,
                                            "cu");
                                    else
                                        using (var newOrgConnection = new SqlConnection(CuCon))
                                        {
                                            var orgCpc = newOrgConnection
                                                .Query<CorporateProductCatalog>(
                                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                            if (orgCpc != null)
                                            {
                                                //consumable.CorporateProductCatalog = cpc;
                                                //m.CPCTitle = orgCpc.Title;
                                                //m.ProjectSequenceCode = project.DatabaseName;
                                                //m.Environment = "org";

                                                //data.Add(m);
                                            }
                                        }
                                }
                            }
                        }
                    }

                    foreach (var m in pbsTools)
                    {
                        var projectCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, project.DatabaseName,
                            pbsResourceParameters.TenantProvider);
                        var CuCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, null,
                            pbsResourceParameters.TenantProvider);
                        var orgCon =
                            ConnectionString.MapConnectionString(null, null, pbsResourceParameters.TenantProvider);

                        using (var newConnection = new SqlConnection(projectCon))
                        {
                            var cpc = newConnection
                                .Query<CorporateProductCatalog>(
                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                            if (cpc != null)
                            {
                                //consumable.CorporateProductCatalog = cpc;
                                //m.CPCTitle = cpc.Title;
                                //m.ProjectSequenceCode = project.DatabaseName;
                                //m.Environment = "local";

                                //data.Add(m);
                            }
                            else
                            {
                                using (var newCuConnection = new SqlConnection(CuCon))
                                {
                                    var cuCpc = newCuConnection
                                        .Query<CorporateProductCatalog>(
                                            "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                            new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                    if (cuCpc != null)
                                        //consumable.CorporateProductCatalog = cpc;
                                        //m.CPCTitle = cuCpc.Title;
                                        //m.ProjectSequenceCode = project.DatabaseName;
                                        //m.Environment = "cu";

                                        //data.Add(m);

                                        await CopyCpcFromCuToProject(pbsResourceParameters, cuCpc.Id, projectCon,
                                            "cu");
                                    else
                                        using (var newOrgConnection = new SqlConnection(CuCon))
                                        {
                                            var orgCpc = newOrgConnection
                                                .Query<CorporateProductCatalog>(
                                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                            if (orgCpc != null)
                                            {
                                                //consumable.CorporateProductCatalog = cpc;
                                                //m.CPCTitle = orgCpc.Title;
                                                //m.ProjectSequenceCode = project.DatabaseName;
                                                //m.Environment = "org";

                                                //data.Add(m);
                                            }
                                        }
                                }
                            }
                        }
                    }

                    foreach (var m in PbsConsumable)
                    {
                        var projectCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, project.DatabaseName,
                            pbsResourceParameters.TenantProvider);
                        var CuCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, null,
                            pbsResourceParameters.TenantProvider);
                        var orgCon =
                            ConnectionString.MapConnectionString(null, null, pbsResourceParameters.TenantProvider);

                        using (var newConnection = new SqlConnection(projectCon))
                        {
                            var cpc = newConnection
                                .Query<CorporateProductCatalog>(
                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                            if (cpc != null)
                            {
                                //consumable.CorporateProductCatalog = cpc;
                                //m.CPCTitle = cpc.Title;
                                //m.ProjectSequenceCode = project.DatabaseName;
                                //m.Environment = "local";

                                //data.Add(m);
                            }
                            else
                            {
                                using (var newCuConnection = new SqlConnection(CuCon))
                                {
                                    var cuCpc = newCuConnection
                                        .Query<CorporateProductCatalog>(
                                            "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                            new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                    if (cuCpc != null)
                                        //consumable.CorporateProductCatalog = cpc;
                                        //m.CPCTitle = cuCpc.Title;
                                        //m.ProjectSequenceCode = project.DatabaseName;
                                        //m.Environment = "cu";

                                        //data.Add(m);

                                        await CopyCpcFromCuToProject(pbsResourceParameters, cuCpc.Id, projectCon,
                                            "cu");
                                    else
                                        using (var newOrgConnection = new SqlConnection(CuCon))
                                        {
                                            var orgCpc = newOrgConnection
                                                .Query<CorporateProductCatalog>(
                                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                            if (orgCpc != null)
                                            {
                                                //consumable.CorporateProductCatalog = cpc;
                                                //m.CPCTitle = orgCpc.Title;
                                                //m.ProjectSequenceCode = project.DatabaseName;
                                                //m.Environment = "org";

                                                //data.Add(m);
                                            }
                                        }
                                }
                            }
                        }
                    }

                    foreach (var m in PbsLabour)
                    {
                        var projectCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, project.DatabaseName,
                            pbsResourceParameters.TenantProvider);
                        var CuCon = ConnectionString.MapConnectionString(
                            pbsResourceParameters.ContractingUnitSequenceId, null,
                            pbsResourceParameters.TenantProvider);
                        var orgCon =
                            ConnectionString.MapConnectionString(null, null, pbsResourceParameters.TenantProvider);

                        using (var newConnection = new SqlConnection(projectCon))
                        {
                            var cpc = newConnection
                                .Query<CorporateProductCatalog>(
                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                            if (cpc != null)
                            {
                                //consumable.CorporateProductCatalog = cpc;
                                //m.CPCTitle = cpc.Title;
                                //m.ProjectSequenceCode = project.DatabaseName;
                                //m.Environment = "local";

                                //data.Add(m);
                            }
                            else
                            {
                                using (var newCuConnection = new SqlConnection(CuCon))
                                {
                                    var cuCpc = newCuConnection
                                        .Query<CorporateProductCatalog>(
                                            "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                            new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                    if (cuCpc != null)
                                        //consumable.CorporateProductCatalog = cpc;
                                        //m.CPCTitle = cuCpc.Title;
                                        //m.ProjectSequenceCode = project.DatabaseName;
                                        //m.Environment = "cu";

                                        //data.Add(m);

                                        await CopyCpcFromCuToProject(pbsResourceParameters, cuCpc.Id, projectCon,
                                            "cu");
                                    else
                                        using (var newOrgConnection = new SqlConnection(CuCon))
                                        {
                                            var orgCpc = newOrgConnection
                                                .Query<CorporateProductCatalog>(
                                                    "SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                                                    new { Id = m.CoperateProductCatalogId }).FirstOrDefault();

                                            if (orgCpc != null)
                                            {
                                                //consumable.CorporateProductCatalog = cpc;
                                                //m.CPCTitle = orgCpc.Title;
                                                //m.ProjectSequenceCode = project.DatabaseName;
                                                //m.Environment = "org";

                                                //data.Add(m);
                                            }
                                        }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var mDatabasesEx = new DatabasesEx();
                mDatabasesEx.DatabaseName = project.DatabaseName;
                mDatabasesEx.Exception = ex;
                exceptionLst.Add(mDatabasesEx);
            }


        return exceptionLst;
    }

    public async Task<string> CopyCpcFromCuToProject(PbsResourceParameters pbsParameters,
        string coperateProductCatalogId, string connectionString, string environment)
    {
        // var options = new DbContextOptions<ShanukaDbContext>();
        // bool isCpcExist;
        var parameter = new CpcParameters();
        parameter.Lang = pbsParameters.Lang;
        parameter.ContextAccessor = pbsParameters.ContextAccessor;
        parameter.TenantProvider = pbsParameters.TenantProvider;
        if (environment == "cu") parameter.ContractingUnitSequenceId = pbsParameters.ContractingUnitSequenceId;

        var CpcConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        CorporateProductCatalog corporateProductCatalog;
        IEnumerable<CpcResourceNickname> nickName;
        IEnumerable<CpcImage> cpcImage;
        IEnumerable<CpcVendor> vendor;

        using (var connection = new SqlConnection(CpcConnectionString))
        {
            corporateProductCatalog = connection
                .Query<CorporateProductCatalog>("SELECT * FROM CorporateProductCatalog WHERE Id = @Id",
                    new { Id = coperateProductCatalogId }).FirstOrDefault();
            nickName = connection.Query<CpcResourceNickname>(
                "SELECT * FROM CpcResourceNickname WHERE CoperateProductCatalogId = @coperateProductCatalogId",
                new { coperateProductCatalogId });
            cpcImage = connection.Query<CpcImage>(
                "SELECT * FROM CpcImage WHERE CoperateProductCatalogId = @coperateProductCatalogId",
                new { coperateProductCatalogId });
            vendor = connection.Query<CpcVendor>(
                "SELECT * FROM CpcVendor WHERE CoperateProductCatalogId = @coperateProductCatalogId",
                new { coperateProductCatalogId });
        }

        //using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
        //{
        //    isCpcExist = context.CorporateProductCatalog.Any(c => c.ResourceNumber == corporateProductCatalog.ResourceNumber);
        //}
        //if (isCpcExist == false)
        //{
        if (corporateProductCatalog != null)
        {
            var sql =
                "INSERT INTO dbo.CorporateProductCatalog (Id, ResourceTitle, ResourceTypeId, ResourceFamilyId, CpcBasicUnitOfMeasureId, CpcMaterialId, CpcPressureClassId, InventoryPrice, CpcUnitOfSizeMeasureId, Size, WallThickness, MinOrderQuantity, MaxOrderQuantity, Weight, Status, ResourceNumber, IsDeleted, CpcBrandId, Title) VALUES " +
                "(@Id, @ResourceTitle, @ResourceTypeId, @ResourceFamilyId, @CpcBasicUnitOfMeasureId, @CpcMaterialId, @CpcPressureClassId, @InventoryPrice, @CpcUnitOfSizeMeasureId, @Size, @WallThickness, @MinOrderQuantity, @MaxOrderQuantity, @Weight, @Status, @ResourceNumber, @IsDeleted, @CpcBrandId, @Title)";

            var param = new
            {
                corporateProductCatalog.Id,
                corporateProductCatalog.ResourceTitle,
                corporateProductCatalog.ResourceTypeId,
                corporateProductCatalog.ResourceFamilyId,
                corporateProductCatalog.CpcBasicUnitOfMeasureId,
                corporateProductCatalog.CpcMaterialId,
                corporateProductCatalog.CpcPressureClassId,
                corporateProductCatalog.InventoryPrice,
                corporateProductCatalog.CpcUnitOfSizeMeasureId,
                corporateProductCatalog.Size,
                corporateProductCatalog.WallThickness,
                corporateProductCatalog.MinOrderQuantity,
                corporateProductCatalog.MaxOrderQuantity,
                corporateProductCatalog.Weight,
                corporateProductCatalog.Status,
                corporateProductCatalog.ResourceNumber,
                corporateProductCatalog.IsDeleted,
                corporateProductCatalog.CpcBrandId,
                corporateProductCatalog.Title
            };

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, param);
            }

            if (nickName.FirstOrDefault() != null)
                foreach (var nn in nickName)
                {
                    var insertQuery =
                        "INSERT INTO dbo.CpcResourceNickname (Id, NickName, CoperateProductCatalogId, IsDeleted, Language, LocaleCode)" +
                        "VALUES (@Id, @NickName, @CoperateProductCatalogId, @IsDeleted, @Language, @LocaleCode)";

                    var nnParam = new
                    {
                        nn.Id,
                        nn.NickName,
                        nn.CoperateProductCatalogId,
                        nn.IsDeleted,
                        nn.Language,
                        nn.LocaleCode
                    };

                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.ExecuteAsync(insertQuery, nnParam);
                    }
                }

            if (cpcImage.FirstOrDefault() != null)
                foreach (var images in cpcImage)
                {
                    var insertQuery =
                        "INSERT INTO dbo.CpcImage ( Id, CoperateProductCatalogId, Image, IsDeleted )" +
                        "VALUES (@Id, @CoperateProductCatalogId, @Image, @IsDeleted)";

                    var imgParam = new
                    {
                        images.Id,
                        images.CoperateProductCatalogId,
                        images.Image,
                        images.IsDeleted
                    };

                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.ExecuteAsync(insertQuery, imgParam);
                    }
                }

            if (vendor.FirstOrDefault() != null)
                foreach (var vv in vendor)
                {
                    var insertQuery =
                        "INSERT INTO dbo.CpcVendor ( Id, ResourceNumber , ResourceTitle , PurchasingUnit , ResourcePrice , ResourceLeadTime , MinOrderQuantity , MaxOrderQuantity , RoundingValue , CoperateProductCatalogId, IsDeleted, PreferredParty, CompanyId ) " +
                        "VALUES (@Id, @ResourceNumber , @ResourceTitle , @PurchasingUnit , @ResourcePrice , @ResourceLeadTime , @MinOrderQuantity , @MaxOrderQuantity , @RoundingValue , @CoperateProductCatalogId, @IsDeleted, @PreferredParty, @CompanyId)";

                    var vendorParam = new
                    {
                        vv.Id,
                        vv.ResourceNumber,
                        vv.ResourceTitle,
                        vv.PurchasingUnit,
                        vv.ResourcePrice,
                        vv.ResourceLeadTime,
                        vv.MinOrderQuantity,
                        vv.MaxOrderQuantity,
                        vv.RoundingValue,
                        vv.CoperateProductCatalogId,
                        vv.IsDeleted,
                        vv.PreferredParty,
                        vv.CompanyId
                    };

                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.ExecuteAsync(insertQuery, vendorParam);
                    }
                }
        }


        // }
        return corporateProductCatalog.CpcBasicUnitOfMeasureId;
    }

    public async Task<ReadAllPbsResourceReadDto> ReadPbsResourcesByPbsProduct(PbsResourceParameters pbsParameters)
    {
        try
        {
            var result = new ReadAllPbsResourceReadDto();

            result.Materials = GetMaterialByProductId(pbsParameters).Result.ToList();
            result.Consumables = GetConsumableByProductId(pbsParameters).Result.ToList();
            result.Tools = GetToolByProductId(pbsParameters).Result.ToList();
            result.Labours = GetLabourByProductId(pbsParameters).Result.ToList();


            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private async Task<string> CopyCpc(PbsResourceParameters pbsParameters, string coperateProductCatalogId,
        string connectionString, string environment)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            bool isCpcExist;
            var parameter = new CpcParameters
            {
                Lang = pbsParameters.Lang,
                ContextAccessor = pbsParameters.ContextAccessor,
                TenantProvider = pbsParameters.TenantProvider
            };
            if (environment == "cu") parameter.ContractingUnitSequenceId = pbsParameters.ContractingUnitSequenceId;

            var CpcConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            CorporateProductCatalog corporateProductCatalog;
            using (var context = new ShanukaDbContext(options, CpcConnectionString, pbsParameters.TenantProvider))
            {
                corporateProductCatalog = context.CorporateProductCatalog
                    .FirstOrDefault(c => c.Id == coperateProductCatalogId);
            }

            using (var context = new ShanukaDbContext(options, connectionString, pbsParameters.TenantProvider))
            {
                isCpcExist =
                    context.CorporateProductCatalog.Any(c =>
                        c.ResourceNumber == corporateProductCatalog.ResourceNumber);
            }

            if (isCpcExist == false)
            {
                parameter.Id = corporateProductCatalog.ResourceNumber;

                var cpc =
                    await pbsParameters.ICoporateProductCatalogRepository.GetCorporateProductCatalogById(parameter);

                var cpcCreateDto = new CoperateProductCatalogCreateDto();
                if (cpc.CpcBasicUnitOfMeasure != null)
                    cpcCreateDto.CpcBasicUnitOfMeasureId = cpc.CpcBasicUnitOfMeasure.Key;

                if (cpc.CpcBrand != null) cpcCreateDto.CpcBrandId = cpc.CpcBrand.Key;

                if (cpc.CpcPressureClass != null) cpcCreateDto.CpcPressureClassId = cpc.CpcPressureClass.Key;

                if (cpc.ResourceFamily != null) cpcCreateDto.ResourceFamilyId = cpc.ResourceFamily.Key;

                if (cpc.CpcUnitOfSizeMeasure != null)
                    cpcCreateDto.CpcUnitOfSizeMeasureId = cpc.CpcUnitOfSizeMeasure.Key;

                cpcCreateDto.CpcMaterialId = cpc.CpcMaterialId;
                cpcCreateDto.Id = cpc.Id;
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
                parameter.Oid = pbsParameters.cpcParameter.Oid;

                parameter.CpcDto = cpcCreateDto;
                parameter.isCopy = true;
                parameter.ProjectSequenceId = pbsParameters.ProjectSequenceId;
                parameter.ContractingUnitSequenceId = pbsParameters.ContractingUnitSequenceId;
                await pbsParameters.ICoporateProductCatalogRepository.CreateCoporateProductCatalog(parameter,
                    pbsParameters.ContextAccessor);
            }

            return "";
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    public async Task<string> PbsLabourAssign(PbsResourceParameters pbsParameters)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            null, pbsParameters.TenantProvider);
        var pConnectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.PbsAssignedLabour.Project, pbsParameters.TenantProvider);
        try
        {
            await using var dbConnection = new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString);

            var project = dbConnection
                .Query<ProjectDefinition>(
                    @"SELECT ProjectDefinition.SequenceCode , cp.FullName AS ProjectManagerId,ProjectDefinition.Title FROM dbo.ProjectDefinition LEFT OUTER JOIN CabPerson cp ON ProjectDefinition.ProjectManagerId = cp.Id ")
                .ToList();

            await using var pConnection = new SqlConnection(pConnectionString);

            var pbs = pConnection
                .Query<PbsProduct>(@"SELECT * FROM PbsProduct WHERE ProductId = @Id",
                    new { Id = pbsParameters.PbsAssignedLabour.PbsProduct })
                .FirstOrDefault();

            var pbsLabour = pConnection
                .Query<LabourForPbs>(
                    @"SELECT * FROM dbo.PbsLabour WHERE PbsProductId = @Id AND CoperateProductCatalogId = @CpcId",
                    new { pbs?.Id, pbsParameters.PbsAssignedLabour.CpcId })
                .FirstOrDefault();

            var totalDays = (int)(pbs.EndDate.Value - pbs.StartDate.Value).TotalDays;

            var weekDays = 0;

            var days = new List<DateTime>();
            for (var i = 0; i <= totalDays; i++)
            {
                var currentDate = pbs.StartDate.Value.AddDays(i);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    days.Add(currentDate);
                    weekDays++;
                }
            }

            var perDay = pbsLabour?.Quantity / weekDays;

            if (perDay is > 8 or null)
            {
                perDay = 8;
            }

            const string sql =
                @"INSERT INTO dbo.PbsAssignedLabour ( Id ,CabPersonId ,IsDeleted ,CpcId ,EndDate ,PbsProduct ,Project ,StartDate,AssignedHoursPerDay,Week,ProjectManager,DayOfWeek,Date) VALUES ( @Id ,@CabPersonId ,@IsDeleted ,@CpcId ,@EndDate ,@PbsProduct ,@Project ,@StartDate,@AssignedHoursPerDay,@Week,@ProjectManager,@DayOfWeek,@Date);";

            await using (var connection = new SqlConnection(cuConnectionString))
            {
                foreach (var i in pbsParameters.PbsAssignedLabour.CabPersonId)
                {
                    foreach (var n in days)
                    {
                        var week = GetIso8601WeekOfYear(n);
                        var param = new
                        {
                            Id = Guid.NewGuid(),
                            pbsParameters.PbsAssignedLabour.PbsProduct,
                            CabPersonId = i,
                            IsDeleted = false,
                            pbsParameters.PbsAssignedLabour.CpcId,
                            pbsParameters.PbsAssignedLabour.Project,
                            pbs.StartDate,
                            Enddate = pbs.EndDate,
                            AssignedHoursPerDay = perDay,
                            Week = week,
                            ProjectManager = project
                                .Where(e => e.SequenceCode == pbsParameters.PbsAssignedLabour.Project)
                                .Select(e => e.ProjectManagerId),
                            DayOfWeek = n.DayOfWeek.ToString(),
                            Date = n
                        };

                        await connection.ExecuteAsync(sql, param);
                    }
                }
            }

            var _borParameter = new BorParameter
            {
                ContractingUnitSequenceId = pbsParameters.PbsAssignedLabour.Cu,
                ProjectSequenceId = pbsParameters.PbsAssignedLabour.Project,
                Lang = pbsParameters.Lang,
                ContextAccessor = pbsParameters.ContextAccessor,
                TenantProvider = pbsParameters.TenantProvider,
                IBorResourceRepository = pbsParameters._iBorResourceRepositoryRepository,
                ICoporateProductCatalogRepository = pbsParameters.ICoporateProductCatalogRepository,
                //_CpcParameters.Oid = userId;
                CpcParameters = new CpcParameters()
            };

            var borDto = new BorDto
            {
                Id = Guid.NewGuid().ToString(),
                BorStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                BorTypeId = "88282458-0b40-poa3-b0f9-c2e40344c888",
                Name = pbs.Title,
                StartDate = pbs.StartDate,
                EndDate = pbs.EndDate,
                BorResources = new BorResource(),
                IsTh = false,
                Product = new BorProductDto
                {
                    Id = pbs.Id,
                    ProductId = pbs.ProductId
                },
                WeekPlan = true,
                CId = pbsParameters.PbsAssignedLabour.CpcId
            };

            _borParameter.BorDto = borDto;

            //var borItemId = await pbsParameters.BorRepository.CreateBor(_borParameter);

            var vpParameter = new VPParameter()
            {
                ContractingUnitSequenceId = pbsParameters.PbsAssignedLabour.Cu,
                ProjectSequenceId = pbsParameters.PbsAssignedLabour.Project,
                Lang = pbsParameters.Lang,
                ContextAccessor = pbsParameters.ContextAccessor,
                TenantProvider = pbsParameters.TenantProvider,
                UserId = pbsParameters.UserId,
                CreatePr = new CreatePr()
                {
                    BorId = borDto.Id
                },
                BorRepository = pbsParameters.BorRepository,
                ProjectDefinitionRepository = pbsParameters.ProjectDefinitionRepository,
                Configuration = pbsParameters.Configuration,
                uPrinceCustomerContext = pbsParameters.uPrinceCustomerContext
            };

            await pbsParameters.IVPRepository.CretePrFromBor(vpParameter);

            await PbsLabourAssignReCalculate(pbsParameters);
            return pbsParameters.PbsAssignedLabour.Id;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> PbsLabourAssignReCalculate(PbsResourceParameters pbsParameters)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            null, pbsParameters.TenantProvider);
        var pConnectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.PbsAssignedLabour.Project, pbsParameters.TenantProvider);

        try
        {
            await using var dbConnection = new SqlConnection(pbsParameters.TenantProvider.GetTenant().ConnectionString);

            var project = dbConnection
                .Query<ProjectDefinition>(
                    @"SELECT ProjectDefinition.SequenceCode , cp.FullName AS ProjectManagerId,ProjectDefinition.Title FROM dbo.ProjectDefinition LEFT OUTER JOIN CabPerson cp ON ProjectDefinition.ProjectManagerId = cp.Id ")
                .ToList();
            
            await using var pConnection = new SqlConnection(pConnectionString);
            
            var pbs = pConnection
                .Query<PbsProduct>(@"SELECT * FROM PbsProduct WHERE ProductId = @Id",
                    new { Id = pbsParameters.PbsAssignedLabour.PbsProduct })
                .FirstOrDefault();

            if (pbs?.StartDate != null)
            {
                var pbsLabour = pConnection
                    .Query<LabourForPbs>(
                        @"SELECT * FROM dbo.PbsLabour WHERE PbsProductId = @Id ",
                        new { pbs?.Id })
                    .ToList();

                var totalDays = (int)(pbs.EndDate.Value - pbs.StartDate.Value).TotalDays;

                await using var cuConnection = new SqlConnection(cuConnectionString);

                var pbsAssignedLabour = cuConnection
                    .Query<PbsAssignedLabour>(
                        @"SELECT * FROM dbo.PbsAssignedLabour WHERE PbsProduct = @PbsProduct",
                        pbsParameters.PbsAssignedLabour).ToList();


                var pbsAssignedLabourByCpc = pbsAssignedLabour.GroupBy(e => e.CpcId).ToList();

                foreach (var cpc in pbsAssignedLabourByCpc)
                {
                    var weekDays = 0;

                    var days = new List<DateTime>();
                    for (var i = 0; i <= totalDays; i++)
                    {
                        var currentDate = pbs.StartDate.Value.AddDays(i);
                        if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            days.Add(currentDate);
                            weekDays++;
                        }
                    }

                    var person = cpc.GroupBy(e => e.CabPersonId);
                    
                    var quantity = pbsLabour?.Where(e => e.CoperateProductCatalogId == cpc.Key).Select(e => e.Quantity)
                        .Sum();
                    var perDay = quantity / (weekDays * person.Count());

                    if (perDay is > 8 or null)
                    {
                        perDay = 8;
                    }

                    await cuConnection.ExecuteAsync(
                        @"DELETE FROM dbo.PbsAssignedLabour WHERE CpcId = @CpcId AND PbsProduct = @PbsProduct",
                        new { CpcId = cpc.Key, cpc.First().PbsProduct });

                    const string sql =
                        @"INSERT INTO dbo.PbsAssignedLabour ( Id ,CabPersonId ,IsDeleted ,CpcId ,EndDate ,PbsProduct ,Project ,StartDate,AssignedHoursPerDay,Week,ProjectManager,DayOfWeek,Date) VALUES ( @Id ,@CabPersonId ,@IsDeleted ,@CpcId ,@EndDate ,@PbsProduct ,@Project ,@StartDate,@AssignedHoursPerDay,@Week,@ProjectManager,@DayOfWeek,@Date);";

                    var pbsAssignedLabourByCpcByLabour = cpc.GroupBy(e => e.CabPersonId).ToList();

                    foreach (var i in pbsAssignedLabourByCpcByLabour)
                    {
                        foreach (var n in days)
                        {
                            var week = GetIso8601WeekOfYear(n);
                            var param = new
                            {
                                Id = Guid.NewGuid(),
                                i.First().PbsProduct,
                                i.First().CabPersonId,
                                IsDeleted = false,
                                i.First().CpcId,
                                i.First().Project,
                                pbs.StartDate,
                                Enddate = pbs.EndDate,
                                AssignedHoursPerDay = perDay,
                                Week = week,
                                ProjectManager = project
                                    .Where(e => e.SequenceCode == pbsParameters.PbsAssignedLabour.Project)
                                    .Select(e => e.ProjectManagerId),
                                DayOfWeek = n.DayOfWeek.ToString(),
                                Date = n
                            };

                            await cuConnection.ExecuteAsync(sql, param);
                        }
                    }
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return "Ok";
    }

    private static int GetIso8601WeekOfYear(DateTime time)
    {
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }

        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday);
    }

    public async Task<List<ConsolidateReadDto>> GetPbsResourceConsolidatedQuantity(PbsResourceParameters pbsParameters,
        string table)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsParameters.ContractingUnitSequenceId,
            pbsParameters.ProjectSequenceId, pbsParameters.TenantProvider);

       
            await using var connection = new SqlConnection(connectionString);


            if (pbsParameters.IdList == null)
            {
                throw new Exception("Idlist null");
            }

            var resultList = new List<ConsolidateReadDto>();
            foreach (var pbs in pbsParameters.IdList)
            {
                try
                {
                var consRes = new ConsolidateReadDto
                {
                    PbsId = pbs,
                    res = new List<PbsResourceReadDto>()
                };
                var childPbs = connection.Query<PbsProduct>("GetPbsResourceConsolidatedQuantity",
                    new { Id = pbs }, commandType: CommandType.StoredProcedure).ToList();

                var mChildPbs = connection
                    .Query<string>(@"SELECT ParentId FROM dbo.PbsProduct WHERE ParentId IN @ProductId",
                        new { ProductId = childPbs.Select(e => e.ProductId).ToList() }).ToList();

                childPbs = childPbs.Where(e => mChildPbs.All(p => p != e.ProductId)).ToList();

                var labourRes = connection.Query<MaterialForPbs>(
                    "Select * from " + table + " where PbsProductId = @PbsProductId",
                    new
                    {
                        PbsProductId = pbs
                    });
                var childLabours = connection.Query<MaterialForPbs>(
                        @"Select * from " + table +
                        " where CoperateProductCatalogId in @CoperateProductCatalogId AND PbsProductId In @PbsProductIds",
                        new
                        {
                            CoperateProductCatalogId = labourRes.Where(s=>s.CoperateProductCatalogId != null ).Select(e => e.CoperateProductCatalogId),
                            PbsProductIds = childPbs.Select(e => e.Id).ToList()
                        })
                    .ToList();
                foreach (var res in labourRes)
                {
                    var readDto = new PbsResourceReadDto();


                    readDto.ConsolidatedQuantity = childLabours
                        ?.Where(s => s.CoperateProductCatalogId == res.CoperateProductCatalogId).Sum(x => x.Quantity);
                    readDto.CpcKey = res.CoperateProductCatalogId;
                    readDto.PbsId = res.PbsProductId;
                    readDto.Quantity = res.Quantity;

                    consRes.res.Add(readDto);
                }

                consRes.TotalConsolidatedQuantity = consRes?.res.Sum(x => x.ConsolidatedQuantity);
                if (consRes.res.All(x => x.ConsolidatedQuantity == 0))
                {
                    consRes.TotalConsolidatedQuantity +=
                        consRes?.res.Where(x => x.ConsolidatedQuantity == 0).Sum(c => c.Quantity);
                }

                resultList.Add(consRes);
                }
                catch (Exception e)
                {
                    throw new Exception(e + " - " + pbs);
                }
            }

            return resultList;
        
    }

    public async Task PbsParentDateAdjust(PbsResourceParameters pbsResourceParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(pbsResourceParameters.ContractingUnitSequenceId,
            pbsResourceParameters.ProjectSequenceId, pbsResourceParameters.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var query =
            @"UPDATE dbo.PbsProduct SET EndDate = @EndDate ,StartDate = @StartDate WHERE ProductId = @SequenceId";

        var parentPbs = connection.Query<PbsTreeStructure>(@"with name_tree as
                                                                        (SELECT
                                                                          Id ,Title ,StartDate ,EndDate ,ProductId AS PbsSequenceId ,ParentId
                                                                        FROM dbo.PbsProduct
                                                                        WHERE PbsProduct.ProductId = @SequenceId
                                                                          UNION ALL
                                                                          SELECT c.Id, c.Title ,c.StartDate ,c.EndDate ,c.ProductId AS PbsSequenceId ,c.ParentId
                                                                          FROM dbo.PbsProduct c
                                                                          JOIN name_tree p on p.ParentId = c.ProductId)
                                                                          select *
                                                                          from name_tree",
            new { SequenceId = pbsResourceParameters.PbsProductId });

        var pbsId = parentPbs.FirstOrDefault(x => x.PbsSequenceId == pbsResourceParameters.PbsProductId)?.Id;
        var labourItems = connection.Query<MaterialForPbs>("Select * from PbsLabour");
        var childLabourItems = labourItems.Where(x => x.PbsProductId == pbsId).ToList();

        if (parentPbs != null)
        {
            foreach (var item in parentPbs)
            {
                var childPbs = connection.Query<PbsTreeStructure>(@"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM PbsProduct
                                                                            WHERE Id = @Id                                                               
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM PbsProduct t
                                                                            INNER JOIN ret r
                                                                            ON t.ParentId = r.ProductId WHERE r.IsDeleted = 0
                                                                            )
                                                                            SELECT
                                                                            ret.Id ,Title ,StartDate ,EndDate ,ProductId AS PbsSequenceId ,ParentId, t4.TreeIndex
                                                                            FROM ret LEFT OUTER JOIN PbsTreeIndex t4 ON ret.ProductId = t4.PbsProductId
                                                                            WHERE ret.Id != @Id AND ret.IsDeleted = 0",
                    new { item.Id }).ToList();

                if (childPbs.Any())
                {
                    var maxEndDate = childPbs.Max(x => x.EndDate);
                    var minStartDate = childPbs.Min(x => x.StartDate);

                    if (maxEndDate == null)
                        maxEndDate = item.EndDate;
                    else if (maxEndDate < item.EndDate) maxEndDate = item.EndDate;
                    if (minStartDate == null)
                        minStartDate = item.StartDate;
                    else if (minStartDate > item.StartDate) minStartDate = item.StartDate;

                    var parm2 = new
                    {
                        StartDate = minStartDate,
                        EndDate = maxEndDate,
                        SequenceId = item.PbsSequenceId
                    };
                    await connection.ExecuteAsync(query, parm2);
                }

                if (childLabourItems.Any())
                {
                    if (item.PbsSequenceId != pbsResourceParameters.PbsProductId)
                    {
                        var parentLabourItems = labourItems.Where(x => x.PbsProductId == item.Id).ToList();

                        var notExistCpc = childLabourItems.ExceptBy(
                            parentLabourItems.Select(x => x.CoperateProductCatalogId), e => e.CoperateProductCatalogId);

                        var laborQuery =
                            "Insert Into PbsLabour values (@Id,@PbsProductId,@CoperateProductCatalogId,@Quantity)";

                        foreach (var cpc in notExistCpc)
                        {
                            var labourParam = new MaterialForPbs()
                            {
                                Id = Guid.NewGuid().ToString(),
                                PbsProductId = item.Id,
                                Quantity = 0,
                                CoperateProductCatalogId = cpc.CoperateProductCatalogId
                            };

                            await connection.ExecuteAsync(laborQuery, labourParam);
                        }
                    }
                }
            }
        }
    }
}