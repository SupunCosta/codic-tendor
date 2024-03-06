using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PC;
using UPrinceV4.Web.Repositories.Interfaces.PS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PS;

public class PriceListRepository : IPriceListRepository
{
    public async Task<string> CreateResourceItemPriceList(PriceListParameter parameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
        {
            string id = null;
            if (parameter.ResourceItemPriceListCreateDto.Id == null)
            {
                var priceList = new ResourceItemPriceList
                {
                    Id = Guid.NewGuid().ToString(),
                    Coefficient = parameter.ResourceItemPriceListCreateDto.Coefficient,
                    CpcId = parameter.ResourceItemPriceListCreateDto.CpcId,
                    ResourceTypeId = parameter.ResourceItemPriceListCreateDto.ResourceTypeId,
                    FixedPrice = parameter.ResourceItemPriceListCreateDto.FixedPrice
                };
                context.ResourceItemPriceList.Add(priceList);
                await context.SaveChangesAsync();
                id = priceList.Id;
            }
            else
            {
                var priceList =
                    context.ResourceItemPriceList.FirstOrDefault(p =>
                        p.Id == parameter.ResourceItemPriceListCreateDto.Id);
                if (priceList != null)
                {
                    priceList.Coefficient = parameter.ResourceItemPriceListCreateDto.Coefficient;
                    priceList.CpcId = parameter.ResourceItemPriceListCreateDto.CpcId;
                    priceList.FixedPrice = parameter.ResourceItemPriceListCreateDto.FixedPrice;
                    context.ResourceItemPriceList.Update(priceList);
                    context.SaveChanges();
                    id = priceList.Id;
                }
            }

            return id;
        }
    }

    public async Task<string> CreateResourceTypePriceList(PriceListParameter parameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
        {
            var priceList =
                context.ResourceTypePriceList.FirstOrDefault(p => p.IsCurrent == true);
            if (priceList != null)
            {
                priceList.IsCurrent = false;
                context.ResourceTypePriceList.Update(priceList);
                await context.SaveChangesAsync();
            }

            var newPriceList = new ResourceTypePriceList
            {
                Id = Guid.NewGuid().ToString(),
                ConsumableCoefficient = parameter.ResourceTypePriceListCreateDto.ConsumableCoefficient,
                LabourCoefficient = parameter.ResourceTypePriceListCreateDto.LabourCoefficient,
                MaterialCoefficient = parameter.ResourceTypePriceListCreateDto.MaterialCoefficient,
                ToolCoefficient = parameter.ResourceTypePriceListCreateDto.ToolCoefficient,
                ServiceCoefficient = parameter.ResourceTypePriceListCreateDto.ServiceCoefficient,
                IsCurrent = true
            };
            context.ResourceTypePriceList.Add(newPriceList);
            context.SaveChanges();

            return newPriceList.Id;
        }
    }

    public async Task<string> DeleteResourceItemPriceList(PriceListParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        foreach (var id in parameter.idList)
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Execute(" update ResourceItemPriceList set IsDeleted = 1 where Id = @id",
                    new { id });

                
            }

        return "Ok";
    }

    public async Task<IEnumerable<ResourceItemPriceListReadDto>> ReadConsumablePriceList(
        PriceListParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);

        var query = @"
                              select ResourceItemPriceList.Id, ResourceItemPriceList.CpcId,ResourceItemPriceList.ResourceTypeId, ResourceItemPriceList.Coefficient,
                              ResourceItemPriceList.FixedPrice ,CorporateProductCatalog.Title AS Title ,CorporateProductCatalog.ResourceNumber
                              From ResourceItemPriceList 
                              Inner join CorporateProductCatalog on ResourceItemPriceList.CpcId = CorporateProductCatalog.Id
                              where ResourceItemPriceList.ResourceTypeId = 'c46c3a26-39a5-42cc-m06g-89655304eh6'
                              AND ResourceItemPriceList.IsDeleted = 0
                             ";

        using (var dbConnection = new SqlConnection(connectionString))
        {
            var result =
                dbConnection.Query<ResourceItemPriceListReadDto>(query);
            

            return result;
        }
    }

    public async Task<IEnumerable<ResourceItemPriceListReadDto>> ReadLabourPriceList(PriceListParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);

        var query = @"
                              select ResourceItemPriceList.Id, ResourceItemPriceList.CpcId,ResourceItemPriceList.ResourceTypeId, ResourceItemPriceList.Coefficient,
                              ResourceItemPriceList.FixedPrice ,CorporateProductCatalog.Title AS Title ,CorporateProductCatalog.ResourceNumber
                              From ResourceItemPriceList 
                              Inner join CorporateProductCatalog on ResourceItemPriceList.CpcId = CorporateProductCatalog.Id
                              where ResourceItemPriceList.ResourceTypeId = 'c46c3a26-39a5-42cc-b07s-89655304eh6'
                              AND ResourceItemPriceList.IsDeleted = 0
                             ";

        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var result =
                dbConnection.Query<ResourceItemPriceListReadDto>(query);
            

            return result;
        }
    }

    public async Task<IEnumerable<ResourceItemPriceListReadDto>> ReadMaterialPriceList(PriceListParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);

        var query = @"
                              select ResourceItemPriceList.Id, ResourceItemPriceList.CpcId,ResourceItemPriceList.ResourceTypeId, ResourceItemPriceList.Coefficient,
                              ResourceItemPriceList.FixedPrice ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber
                              From ResourceItemPriceList 
                              Inner join CorporateProductCatalog on ResourceItemPriceList.CpcId = CorporateProductCatalog.Id
                              where ResourceItemPriceList.ResourceTypeId = 'c46c3a26-39a5-42cc-n7k1-89655304eh6'
                              AND ResourceItemPriceList.IsDeleted = 0
                             ";

        using (var dbConnection = new SqlConnection(connectionString))
        {
            var result =
                dbConnection.Query<ResourceItemPriceListReadDto>(query);
            

            return result;
        }
    }

    public async Task<ResourceTypePriceList> ReadResourceTypePriceList(PriceListParameter parameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        ResourceTypePriceList priceList;
        ProjectDefinition mProjectDefinition;

        await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
        {
            priceList = context.ResourceTypePriceList.FirstOrDefault(p => p.IsCurrent == true);
        }

        if (priceList == null)
        {
            var cuConnectionString =
                ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId, null,
                    parameter.TenantProvider);
            await using var context = new ShanukaDbContext(options, cuConnectionString, parameter.TenantProvider);
            priceList = context.ResourceTypePriceList.FirstOrDefault(p => p.IsCurrent == true);
            //  priceList.Tax = context.Tax.Where(T => T.Id == context.ProjectDefinition.Where(P => P.SequenceCode == parameter.ProjectSequenceId).FirstOrDefault().Id).FirstOrDefault(p=> p.IsDefault == true);
            //priceList.Tax = context.Tax.Where(T => T.Id == mProjectDefinition.VATId).FirstOrDefault(t => t.IsDefault == true);
        }

        if (priceList != null)
        {
            await using (var contextProject = new ShanukaDbContext(options,
                             parameter.TenantProvider.GetTenant().ConnectionString, parameter.TenantProvider))
            {
                mProjectDefinition =
                    contextProject.ProjectDefinition.FirstOrDefault(p => p.SequenceCode == parameter.ProjectSequenceId);
                if (mProjectDefinition.VATId != null)
                    priceList.Tax = contextProject.Tax
                        .FirstOrDefault(T => T.Id == mProjectDefinition.VATId);
                else
                    priceList.Tax = contextProject.Tax.FirstOrDefault(T => T.IsDefault == true);
            }
        }
        
        return priceList;
    }

    public async Task<IEnumerable<ResourceItemPriceListReadDto>> ReadToolPriceList(PriceListParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);
        var query =
            @"select ResourceItemPriceList.Id, ResourceItemPriceList.CpcId,ResourceItemPriceList.ResourceTypeId, ResourceItemPriceList.Coefficient, ResourceItemPriceList.FixedPrice ,CorporateProductCatalog.Title AS Title ,CorporateProductCatalog.ResourceNumber From ResourceItemPriceList Inner join CorporateProductCatalog on ResourceItemPriceList.CpcId = CorporateProductCatalog.Id where ResourceItemPriceList.ResourceTypeId = 'c46c3a26-39a5-42cc-n9wn-89655304eh6' AND ResourceItemPriceList.IsDeleted = 0";
        await using var dbConnection = new SqlConnection(connectionString);
        var result =
            dbConnection.Query<ResourceItemPriceListReadDto>(query);
        
        return result;
    }
}