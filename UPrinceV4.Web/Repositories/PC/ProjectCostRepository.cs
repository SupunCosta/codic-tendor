using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.PC;
using UPrinceV4.Web.Repositories.Interfaces.PC;
using UPrinceV4.Web.Repositories.Interfaces.PS;
using UPrinceV4.Web.Repositories.PS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PC;

public class ProjectCostRepository : IProjectCostRepository
{
    public async Task<IEnumerable<ProjectCostFilterDto>> Filter(
        ProjectCostRepositoryParameter projectCostRepositoryParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                projectCostRepositoryParameter.ContractingUnitSequenceId,
                projectCostRepositoryParameter.ProjectSequenceId, projectCostRepositoryParameter.TenantProvider);

            await using var dbConnection = new SqlConnection(connectionString);

            var filter = projectCostRepositoryParameter.ProjectCostFilter;
            var sql =
                @"SELECT ProjectCost.Id ,ProjectCost.BorTitle ,ProjectCost.Date ,ProjectCost.ProductTitle ,ProjectCost.ResourceTypeId ,ProjectCost.PmolTitle ,ProjectCost.ResourceNumber ,ProjectCost.ConsumedQuantity ,ProjectCost.CostMou ,ProjectCost.TotalCost ,ProjectCost.ResourceTitle ,CpcBasicUnitOfMeasureLocalizedData.Label AS Mou ,CpcResourceTypeLocalizedData.Label AS ResourceType ,ProjectCost.Mou AS MouId ,ProjectCost.IsPlannedResource ,PbsProductItemTypeLocalizedData.Label AS ProductType FROM dbo.ProjectCost LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId = ProjectCost.Mou LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData ON ProjectCost.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId INNER JOIN dbo.PbsProduct ON ProjectCost.ProductId = PbsProduct.Id INNER JOIN dbo.PbsProductItemTypeLocalizedData ON PbsProductItemTypeLocalizedData.PbsProductItemTypeId = PbsProduct.PbsProductItemTypeId WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR ProjectCost.Mou IS NULL) AND (CpcResourceTypeLocalizedData.LanguageCode = @lang or ProjectCost.ResourceTypeId IS NULL) AND PbsProductItemTypeLocalizedData.LanguageCode = @lang and isUsed = 0";

            var sb = new StringBuilder(sql);

            if (filter.BorTitle != null)
            {
                filter.BorTitle = filter.BorTitle.Replace("'", "''");
                sb.Append(" AND ProjectCost.BorTitle LIKE '%" + filter.BorTitle + "%' ");
            }

            if (filter.ProductTitle != null)
            {
                filter.ProductTitle = filter.ProductTitle.Replace("'", "''");
                sb.Append(" AND ProjectCost.ProductTitle LIKE '%" + filter.ProductTitle + "%' ");

            }

            if (filter.StrDate != null && filter.EndDate != null)
                //sb.Append(" AND ProjectCost.Date ='" + filter.Date + "'");
                //WHERE OrderDate BETWEEN #01/07/1996# AND #31/07/1996#
                sb.Append(" AND Date BETWEEN '" + filter.StrDate + "' AND '" + filter.EndDate + "'");

            if (filter.ResourceTypeId != null)
                sb.Append(" AND ProjectCost.ResourceTypeId = '" + filter.ResourceTypeId + "' ");

            if (filter.ResourceTitle != null)
                sb.Append(" AND ProjectCost.ResourceTitle LIKE '%" + filter.ResourceTitle + "%' ");

            if (filter.productType != null)
                sb.Append(" AND PbsProduct.PbsProductItemTypeId = '" + filter.productType + "' ");

            if (filter.ProjectCostSortingModel.Attribute == null)
            {
                //sb.Append(" ORDER BY cast((select SUBSTRING(ProjectDefinition.SequenceCode, PATINDEX('%[0-9]%', ProjectDefinition.SequenceCode), LEN(ProjectDefinition.SequenceCode))) as int) desc ");
            }

            // if (filter.ProjectCostSortingModel != null)
            // {
            //     if (filter.ProjectCostSortingModel.Attribute != null &&
            //         filter.ProjectCostSortingModel.Order.ToLower().Equals("asc"))
            //     {
            //         if (filter.ProjectCostSortingModel.Attribute == "productType")
            //             sb.Append(" ORDER BY PbsProduct.PbsProductItemTypeId ASC");
            //         else
            //             sb.Append(" ORDER BY " + filter.ProjectCostSortingModel.Attribute + " ASC");
            //     }
            //
            //     if (filter.ProjectCostSortingModel.Attribute != null &&
            //         filter.ProjectCostSortingModel.Order.ToLower().Equals("desc"))
            //     {
            //         if (filter.ProjectCostSortingModel.Attribute == "productType")
            //             sb.Append(" ORDER BY PbsProduct.PbsProductItemTypeId DESC");
            //         else
            //             sb.Append(" ORDER BY " + filter.ProjectCostSortingModel.Attribute + " DESC");
            //     }
            // }

           
            List<ProjectCostFilterDto> result = null;
            var parameters = new { lang = projectCostRepositoryParameter.Lang };
            
                var q = sb.ToString();
                result = dbConnection.Query<ProjectCostFilterDto>(q, parameters).ToList();
                //result = dbConnection.Query<ProjectCostFilterDto>(q).ToList();
                
            

            IPriceListRepository _iPriceListRepository = new PriceListRepository();
            var parameter = new PriceListParameter
            {
                ContractingUnitSequenceId = projectCostRepositoryParameter.ContractingUnitSequenceId,
                ProjectSequenceId = projectCostRepositoryParameter.ProjectSequenceId,
                TenantProvider = projectCostRepositoryParameter.TenantProvider
            };

            var priceList = await _iPriceListRepository.ReadResourceTypePriceList(parameter);

            var materialPriceList = await _iPriceListRepository.ReadMaterialPriceList(parameter);

            var labourPriceList = await _iPriceListRepository.ReadLabourPriceList(parameter);

            var consumablePriceList = await _iPriceListRepository.ReadConsumablePriceList(parameter);

            var toolsPriceList = await _iPriceListRepository.ReadToolPriceList(parameter);


            //foreach(ResourceItemPriceListReadDto priceListValue in priceList){ 
            //    if()
            //}
            foreach (var mProjectCostFilterDto in result)
            {
                if (mProjectCostFilterDto.ResourceTypeId == "c46c3a26-39a5-42cc-n7k1-89655304eh6") //mertial 
                {
                    if (materialPriceList.Any())
                    {
                        var isMeteria = materialPriceList.FirstOrDefault(m =>
                            m.ResourceNumber == mProjectCostFilterDto.ResourceNumber);
                        if (isMeteria != null)
                        {
                            if (isMeteria.FixedPrice != null)
                                mProjectCostFilterDto.SpMou = isMeteria.FixedPrice;
                            else
                                mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * isMeteria.Coefficient;
                        }
                        else
                        {
                            mProjectCostFilterDto.SpMou =
                                mProjectCostFilterDto.CostMou * priceList.MaterialCoefficient;
                        }
                    }
                    else
                    {
                        mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * priceList.MaterialCoefficient;
                    }
                }

                if (mProjectCostFilterDto.ResourceTypeId == "c46c3a26-39a5-42cc-m06g-89655304eh6") // Consumable
                {
                    //mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * priceList.ConsumableCoefficient;

                    if (consumablePriceList.Any())
                    {
                        var isConsumable = consumablePriceList.FirstOrDefault(m =>
                            m.ResourceNumber == mProjectCostFilterDto.ResourceNumber);
                        if (isConsumable != null)
                        {
                            if (isConsumable.FixedPrice != null)
                                mProjectCostFilterDto.SpMou = isConsumable.FixedPrice;
                            else
                                mProjectCostFilterDto.SpMou =
                                    mProjectCostFilterDto.CostMou * isConsumable.Coefficient;
                        }
                        else
                        {
                            mProjectCostFilterDto.SpMou =
                                mProjectCostFilterDto.CostMou * priceList.ConsumableCoefficient;
                        }
                    }
                    else
                    {
                        mProjectCostFilterDto.SpMou =
                            mProjectCostFilterDto.CostMou * priceList.ConsumableCoefficient;
                    }
                }

                if (mProjectCostFilterDto.ResourceTypeId == "c46c3a26-39a5-42cc-n9wn-89655304eh6") // tools 
                {
                    // mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * priceList.ToolCoefficient;

                    if (toolsPriceList.Any())
                    {
                        var isTools = toolsPriceList.FirstOrDefault(m =>
                            m.ResourceNumber == mProjectCostFilterDto.ResourceNumber);
                        if (isTools != null)
                        {
                            if (isTools.FixedPrice != null)
                                mProjectCostFilterDto.SpMou = isTools.FixedPrice;
                            else
                                mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * isTools.Coefficient;
                        }
                        else
                        {
                            mProjectCostFilterDto.SpMou =
                                mProjectCostFilterDto.CostMou * priceList.ToolCoefficient;
                        }
                    }
                    else
                    {
                        mProjectCostFilterDto.SpMou =
                            mProjectCostFilterDto.CostMou * priceList.ToolCoefficient;
                    }
                }

                if (mProjectCostFilterDto.ResourceTypeId == "c46c3a26-39a5-42cc-b07s-89655304eh6") // labour
                {
                    if (labourPriceList.Any())
                    {
                        var isLabour = labourPriceList.FirstOrDefault(m =>
                            m.ResourceNumber == mProjectCostFilterDto.ResourceNumber);
                        if (isLabour != null)
                        {
                            if (isLabour.FixedPrice != null)
                                mProjectCostFilterDto.SpMou = isLabour.FixedPrice;
                            else
                                mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * isLabour.Coefficient;
                        }
                        else
                        {
                            mProjectCostFilterDto.SpMou =
                                mProjectCostFilterDto.CostMou * priceList.LabourCoefficient;
                        }
                    }
                    else
                    {
                        mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * priceList.LabourCoefficient;
                    }
                }

                if (mProjectCostFilterDto.ResourceTypeId == null) // service
                    mProjectCostFilterDto.SpMou = mProjectCostFilterDto.CostMou * priceList.ServiceCoefficient;
            }

            var cbcSql =
                @"SELECT PbsCbcResources.Id,pp.Title AS ProductTitle,'cbc3a26-cbc-cbc-cbc-89655304cbc' AS ResourceTypeId,PbsCbcResources.ArticleNo AS ResourceNumber ,(CONVERT(FLOAT,PbsCbcResources.ConsumedQuantity)-CONVERT(FLOAT,PbsCbcResources.InvoicedQuantity)) AS ConsumedQuantity,CONCAT(pcpd.ArticleNo,' - ',pcpd.Title) AS ResourceTitle,'CBC' AS ResourceType,PbsProductItemTypeLocalizedData.Label AS ProductType,pcpd.Unit AS Mou  ,pcpd.Unit AS MouId FROM PbsCbcResources LEFT OUTER JOIN PbsProduct pp ON PbsCbcResources.PbsId = pp.Id LEFT OUTER JOIN ContractorTotalValuesPublished ctvp ON PbsCbcResources.LotId = ctvp.LotId LEFT OUTER JOIN PublishedContractorsPdfData pcpd ON PbsCbcResources.ArticleNo = pcpd.ArticleNo AND ctvp.CompanyId = pcpd.CompanyId AND ctvp.LotId = pcpd.LotId LEFT OUTER JOIN PbsProductItemTypeLocalizedData ON PbsProductItemTypeLocalizedData.PbsProductItemTypeId = pp.PbsProductItemTypeId WHERE (CONVERT(FLOAT,PbsCbcResources.ConsumedQuantity)-CONVERT(FLOAT,PbsCbcResources.InvoicedQuantity)) > 0 AND PbsCbcResources.IsIgnore = 0 AND PbsProductItemTypeLocalizedData.LanguageCode = 'en' AND ctvp.IsWinner = 1";
            var cbcSb = new StringBuilder(cbcSql);
            
            if (filter.ProductTitle != null)
            {
                filter.ProductTitle = filter.ProductTitle.Replace("'", "''");
                cbcSb.Append(" AND ProductTitle LIKE '%" + filter.ProductTitle + "%' ");

            }
            
            if (filter.ResourceTypeId != null)
                cbcSb.Append(" AND ResourceTypeId = '" + filter.ResourceTypeId + "' ");

            if (filter.ResourceTitle != null)
                cbcSb.Append(" AND ResourceTitle LIKE '%" + filter.ResourceTitle + "%' ");

            if (filter.productType != null)
                cbcSb.Append(" AND PbsProductItemTypeId = '" + filter.productType + "' ");

            var cbcList = dbConnection.Query<ProjectCostFilterDto>(cbcSb.ToString(),parameters);

            result.AddRange(cbcList);
            
            if (filter.ProjectCostSortingModel != null)
            {
                if (filter.ProjectCostSortingModel.Attribute != null &&
                    filter.ProjectCostSortingModel.Order.ToLower().Equals("asc"))
                {
                    if (filter.ProjectCostSortingModel.Attribute == "productType")
                    {
                        result = result.OrderBy(x => x.ProductType).ToList();
                    }
                    else
                    {
                        var propertyInfo = typeof(ProjectCostFilterDto).GetProperty(filter.ProjectCostSortingModel.Attribute.ToLower(),BindingFlags.IgnoreCase| BindingFlags.Public | BindingFlags.Instance);    
                        result = result.OrderBy(x => propertyInfo?.GetValue(x, null)).ToList();
                    }
                }

                if (filter.ProjectCostSortingModel.Attribute != null &&
                    filter.ProjectCostSortingModel.Order.ToLower().Equals("desc"))
                {
                    if (filter.ProjectCostSortingModel.Attribute == "productType")
                    {
                        result = result.OrderByDescending(x => x.ProductType).ToList();
                    }
                    else
                    {
                        var propertyInfo = typeof(ProjectCostFilterDto).GetProperty(filter.ProjectCostSortingModel.Attribute,BindingFlags.IgnoreCase| BindingFlags.Public | BindingFlags.Instance);    
                        result = result.OrderByDescending(x => propertyInfo?.GetValue(x, null)).ToList();
                    }
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<List<string>> IgnoreProjectCostCbcResources(
        ProjectCostRepositoryParameter projectCostRepositoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            projectCostRepositoryParameter.ContractingUnitSequenceId,
            projectCostRepositoryParameter.ProjectSequenceId, projectCostRepositoryParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync("Update PbsCbcResources Set IsIgnore = 1 Where Id In @Ids",
            new { Ids = projectCostRepositoryParameter.IdList });

        return projectCostRepositoryParameter.IdList;

    }

}