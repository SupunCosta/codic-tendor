using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.INV;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Repositories.Interfaces.INV;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.INV;

public class InvoiceRepository : IInvoiceRepository
{
    public Task<string> CreateInvoice(InvoiceParameter parameter)
    {
        throw new NotImplementedException();
    }

    public async Task<string> CUApproveInvoice(InvoiceParameter parameter)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId, null,
            parameter.TenantProvider);

        var psUpdateQuery = @"update Invoice set InvoiceStatusId = '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'
                                    where Id = @id";

        var param = new { id = parameter.InvoiceId };

        using (var connection = new SqlConnection(cuConnectionString))
        {
            await connection.QueryAsync(psUpdateQuery, param);
            connection.Close();
        }

        var readInvoice = @"SELECT
                                          *
                                        FROM dbo.Invoice
                                        WHERE Invoice.Id = @id";

        var readPs = @"SELECT * FROM PsHeader WHERE Id = @Id";
        Invoice mInvoice;
        PsHeader mPsHeader;
        PsHeader ProjectPsHeader;

        await using (var connection = new SqlConnection(cuConnectionString))
        {
            mInvoice = connection.Query<Invoice>(readInvoice, param).SingleOrDefault();
            connection.Close();
        }

        await using (var connection = new SqlConnection(cuConnectionString))
        {
            mPsHeader = connection.Query<PsHeader>(readPs, new { Id = mInvoice.PsId }).SingleOrDefault();
            connection.Close();
        }

        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            mInvoice.ProjectSequenceCode, parameter.TenantProvider);


        await using (var connection = new SqlConnection(connectionString))
        {
            ProjectPsHeader = connection.Query<PsHeader>("SELECT * FROM PsHeader WHERE ProgressStatementId = @Id ",
                new { Id = mPsHeader.ProgressStatementId }).SingleOrDefault();
            connection.Close();
        }

        var insertQ = @"
                            INSERT INTO dbo.Invoice
                            (
                              Id
                             ,InvoiceId
                             ,Name
                             ,Title
                             ,InvoiceStatusId
                             ,WorkPeriodFrom
                             ,WorkPeriodTo
                             ,PurchaseOrderNumber
                             ,TotalIncludingTax
                             ,TotalExcludingTax
                             ,PsId
                             ,Date
                             ,ProjectSequenceCode
                             ,ProductTitle
                            )
                            VALUES
                            (
                              @Id
                             ,@InvoiceId
                             ,@Name
                             ,@Title
                             ,@InvoiceStatusId
                             ,@WorkPeriodFrom
                             ,@WorkPeriodTo
                             ,@PurchaseOrderNumber
                             ,@TotalIncludingTax
                             ,@TotalExcludingTax
                             ,@PsId
                             ,GETDATE()
                             ,@ProjectSequenceCode
                             ,@ProductTitle
                            );";

        var paramInserrt = new
        {
            id = mInvoice.Id,
            mInvoice.InvoiceId,
            mInvoice.Name,
            mInvoice.Title,
            InvoiceStatusId = "4010e768-3e06-4702-b337-ee367a82addb",
            mInvoice.WorkPeriodFrom,
            mInvoice.WorkPeriodTo,
            mInvoice.PurchaseOrderNumber,
            mInvoice.TotalIncludingTax,
            mInvoice.TotalExcludingTax,
            PsId = ProjectPsHeader.Id,
            mInvoice.ProjectSequenceCode,
            mInvoice.ProductTitle
        };

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.QueryAsync(insertQ, paramInserrt);
            connection.Close();
        }

        var readBorMeterial = @"SELECT
                                          ProjectCost.BorId
                                         ,PsResource.CpcResourceTypeId
                                         ,PsResource.SoldQuantity
                                         ,PsResource.CpcResourceNumber
                                         ,CorporateProductCatalog.Id AS CPCId
                                        FROM dbo.PsResource
                                        INNER JOIN dbo.ProjectCost
                                          ON PsResource.ProjectCostId = ProjectCost.Id
                                        INNER JOIN dbo.CorporateProductCatalog
                                          ON PsResource.CpcResourceNumber = CorporateProductCatalog.ResourceNumber
                                        WHERE PsResource.PsId = @PsId
                                        AND PsResource.CpcResourceTypeId = @CpcResourceTypeId AND ProjectCost.IsPlannedResource = 1";

        var updateInvoiced = @"UPDATE dbo.BorMaterial 
                                    SET
                                      Invoiced = dbo.BorMaterial.Invoiced + @SoldQuantity
                                    WHERE
                                      BorProductId = @BorProductId AND dbo.BorMaterial.CorporateProductCatalogId = @CorporateProductCatalogId";
        var updateInvoicedTools = @"UPDATE dbo.BorTools 
                                    SET
                                      Invoiced = dbo.BorTools.Invoiced + @SoldQuantity
                                    WHERE
                                      BorProductId = @BorProductId AND dbo.BorTools.CorporateProductCatalogId = @CorporateProductCatalogId";

        var updateInvoicedConsumables = @"UPDATE dbo.BorConsumables
                                    SET
                                      Invoiced = dbo.BorConsumables.Invoiced + @SoldQuantity
                                    WHERE
                                      BorProductId = @BorProductId AND dbo.BorConsumables.CorporateProductCatalogId = @CorporateProductCatalogId";

        var updateInvoicedLabours = @"UPDATE dbo.BorLabours 
                                    SET
                                      Invoiced = dbo.BorLabours.Invoiced + @SoldQuantity
                                    WHERE
                                      BorProductId = @BorProductId AND dbo.BorLabours.CorporateProductCatalogId = @CorporateProductCatalogId";

        var paramPsmerial = new
            { PsId = ProjectPsHeader.Id, CpcResourceTypeId = "c46c3a26-39a5-42cc-n7k1-89655304eh6" };

        var paramPsTools = new
            { PsId = ProjectPsHeader.Id, CpcResourceTypeId = "c46c3a26-39a5-42cc-n9wn-89655304eh6" };

        var paramPsConsumables = new
            { PsId = ProjectPsHeader.Id, CpcResourceTypeId = "c46c3a26-39a5-42cc-m06g-89655304eh6" };

        var paramPsBorLabours = new
            { PsId = ProjectPsHeader.Id, CpcResourceTypeId = "c46c3a26-39a5-42cc-b07s-89655304eh6" };


        await using (var connection = new SqlConnection(connectionString))
        {
            var list = await connection.QueryAsync<InvoicedDto>(readBorMeterial, paramPsmerial);
            list.ToList().ForEach(invoicedDto =>
                {
                    var paramPsmerialIn = new
                    {
                        BorProductId = invoicedDto.BorId, CorporateProductCatalogId = invoicedDto.CPCId,
                        invoicedDto.SoldQuantity
                    };
                    connection.Query(updateInvoiced, paramPsmerialIn);
                }
            );
            var listTools = await connection.QueryAsync<InvoicedDto>(readBorMeterial, paramPsTools);
            listTools.ToList().ForEach(invoicedDto =>
                {
                    var paramPsmerialIn = new
                    {
                        BorProductId = invoicedDto.BorId, CorporateProductCatalogId = invoicedDto.CPCId,
                        invoicedDto.SoldQuantity
                    };
                    connection.Query(updateInvoicedTools, paramPsmerialIn);
                }
            );

            var listConsumables = await connection.QueryAsync<InvoicedDto>(readBorMeterial, paramPsConsumables);
            listConsumables.ToList().ForEach(invoicedDto =>
                {
                    var paramPsmerialIn = new
                    {
                        BorProductId = invoicedDto.BorId, CorporateProductCatalogId = invoicedDto.CPCId,
                        invoicedDto.SoldQuantity
                    };
                    connection.Query(updateInvoicedConsumables, paramPsmerialIn);
                }
            );

            var listLabours = await connection.QueryAsync<InvoicedDto>(readBorMeterial, paramPsBorLabours);
            listLabours.ToList().ForEach(invoicedDto =>
                {
                    var paramPsmerialIn = new
                    {
                        BorProductId = invoicedDto.BorId, CorporateProductCatalogId = invoicedDto.CPCId,
                        invoicedDto.SoldQuantity
                    };
                    connection.Query(updateInvoicedLabours, paramPsmerialIn);
                }
            );

            //connection.Close();
        }


        return "ok";
    }

    public async Task<PsDropdownData> GetDropdownData(InvoiceParameter parameter)
    {
        var lang = parameter.Lang;

        var dropdownData = new PsDropdownData();
        using (IDbConnection dbConnection =
               new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString))
        {
            var query =
                @"select StatusId as [Key], Name as Text  from InvoiceStatus where LanguageCode = @lang
                                    ORDER BY Name;
                                    select ProjectDefinition.Id, ProjectTime.EndDate, CabPerson.FullName AS ContactPersonName,
                                    CabEmail.EmailAddress AS ContactPersonEmail , CabCompany.Name AS Customer
                                    from ProjectDefinition
                                    left outer join  ProjectTime on ProjectDefinition.Id=ProjectTime.ProjectId
                                    left outer join  ProjectTeam on ProjectDefinition.Id = ProjectTeam.ProjectId
                                    left outer join  ProjectTeamRole on ProjectTeam.Id = ProjectTeamRole.ProjectTeamId
                                    left outer join  CabPerson on ProjectTeamRole.CabPersonId = CabPerson.Id
                                    left outer join CabPersonCompany on CabPerson.Id = CabPersonCompany.PersonId
                                    left outer join CabCompany on CabCompany.Id = CabPersonCompany.CompanyId
                                    left outer join CabEmail on CabPersonCompany.EmailId = CabEmail.Id
                                    where ProjectDefinition.SequenceCode = @id
                                    AND RoleId = '907b7af0-b132-4951-a2dc-6ab82d4cd40d';
                                    select EndDate 
									from ProjectDefinition left outer join  ProjectTime on ProjectDefinition.Id=ProjectTime.ProjectId
									where SequenceCode = @id
                                    ";
            var param = new { lang, id = parameter.ProjectSequenceId };


            using (var multi = dbConnection.QueryMultiple(query, param))
            {
                dropdownData.Status = multi.Read<PmolDropdownDto>();
                var project = multi.Read<projectForPsDto>().FirstOrDefault();
                var date = multi.Read<DateTime?>().FirstOrDefault();

                if (date != null) dropdownData.ProjectCompletionDate = date.ToString();

                if (project != null)
                {
                    var customer = new PsCustomerReadDto();
                    customer.ContactPersonName = project.ContactPersonName;
                    customer.ContactPersonEmail = project.ContactPersonEmail;
                    customer.CustomerName = project.Customer;
                    dropdownData.Customer = customer;
                }
            }
        }

        return dropdownData;
    }

    public async Task<IEnumerable<InvoiceFilterDto>> InvoiceFilter(InvoiceParameter parameter)
    {
        var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);

        var query = @"SELECT
                                      Invoice.Id
                                     ,Invoice.Name
                                     ,Invoice.Title
                                     ,Invoice.InvoiceId
                                     ,Invoice.WorkPeriodFrom
                                     ,Invoice.WorkPeriodTo
                                     ,Invoice.PurchaseOrderNumber
                                     ,Invoice.TotalExcludingTax
                                     ,Invoice.TotalIncludingTax
                                     ,Invoice.Date
                                     ,InvoiceStatus.Name AS InvoiceStatus
                                     ,InvoiceStatus.StatusId AS InvoiceStatusId
                                     ,Invoice.ProductTitle
                                     ,Invoice.ProjectSequenceCode
                                     ,PsHeader.ProgressStatementId AS PsId
                                    FROM dbo.Invoice
                                    LEFT OUTER JOIN dbo.InvoiceStatus
                                      ON Invoice.InvoiceStatusId = InvoiceStatus.StatusId
                                    INNER JOIN dbo.PsHeader
                                      ON Invoice.PsId = PsHeader.Id
                                    WHERE (InvoiceStatus.LanguageCode = @lang
                                    OR Invoice.InvoiceStatusId IS NULL)";

        var sb = new StringBuilder(query);

        if (parameter.filter.Title != null)
        {
            parameter.filter.Title = parameter.filter.Title.Replace("'", "''");
            sb.Append(" AND Invoice.Title like '%" + parameter.filter.Title + "%' ");

        }
            
        if (parameter.filter.Date != null)
        {
            var d = (DateTime)parameter.filter.Date;
            sb.Append(" AND CAST(Invoice.Date as date) = '" + d + " '");
        }

        if (parameter.filter.InvoiceStatusId != null)
            sb.Append(" AND Invoice.InvoiceStatusId = '" + parameter.filter.InvoiceStatusId + "' ");

        if (parameter.filter.TotalExcludingTax != null)
            sb.Append(" AND Invoice.TotalExcludingTax = " + parameter.filter.TotalExcludingTax + " ");

        if (parameter.filter.TotalIncludingTax != null)
            sb.Append(" AND Invoice.TotalIncludingTax = " + parameter.filter.TotalIncludingTax + " ");

        if (parameter.filter.Sorter.Attribute == null) sb.Append(" order by Invoice.InvoiceId desc");

        if (parameter.filter.Sorter.Attribute != null)
        {
            if (parameter.filter.Sorter.Attribute.ToLower().Equals("projectsequencecode"))
                sb.Append("order by Invoice.ProjectSequenceCode " + parameter.filter.Sorter.Order);

            if (parameter.filter.Sorter.Attribute.ToLower().Equals("title"))
                sb.Append("order by Invoice.Title " + parameter.filter.Sorter.Order);

            if (parameter.filter.Sorter.Attribute.ToLower().Equals("invoicestatus"))
                sb.Append("order by InvoiceStatus.Name  " + parameter.filter.Sorter.Order);

            if (parameter.filter.Sorter.Attribute.ToLower().Equals("date"))
                sb.Append("order by Invoice.Date " + parameter.filter.Sorter.Order);

            if (parameter.filter.Sorter.Attribute.ToLower().Equals("totalincludingtax"))
                sb.Append("order by Invoice.TotalIncludingTax " + parameter.filter.Sorter.Order);

            if (parameter.filter.Sorter.Attribute.ToLower().Equals("totalexcludingtax"))
                sb.Append("order by Invoice.TotalExcludingTax " + parameter.filter.Sorter.Order);
        }

        var parameters = new { lang = parameter.Lang };
        IEnumerable<InvoiceFilterDto> result;
        IEnumerable<ProjectDefinitionMobDto> project;
        using (var connection = new SqlConnection(connectionString))
        {
            result = connection.Query<InvoiceFilterDto>(sb.ToString(), parameters);
        }

        var projectParameters = new { sqCode = result.Select(ps => ps.ProjectSequenceCode).Distinct() };
        using (var dbConnection =
               new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString))
        {
            // dbConnection.OpenAsync();
            project = dbConnection.Query<ProjectDefinitionMobDto>(@"SELECT
  ProjectDefinition.SequenceCode as SequenceCode
 , ProjectDefinition.Title
 , ProjectDefinition.ContractingUnitId
FROM dbo.ProjectDefinition
WHERE ProjectDefinition.SequenceCode IN @sqCode
", projectParameters);
            
        }

        foreach (var Ps in result)
            Ps.ProjectSequenceCode = project
                .Where(p => p.SequenceCode != null && p.SequenceCode == Ps.ProjectSequenceCode).FirstOrDefault()
                ?.Title;

        if (parameter.filter != null)
            if (parameter.filter.ProjectSequenceCode != null)
                result = result.Where(x => x.ProjectSequenceCode != null && x.ProjectSequenceCode.ToLower()
                    .Contains(parameter.filter.ProjectSequenceCode.ToLower()));
        // sb.Append(" AND PsHeader.ProjectSequenceCode LIKE '%" + filter.ProjectSequenceCode + "%'");

        return result;
    }

    public async Task<Invoice> ReadInVoiceById(InvoiceParameter parameter)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
            parameter.ProjectSequenceId, parameter.TenantProvider);

        var param = new { id = parameter.InvoiceId };

        var readImvoiceHeader = @"SELECT
                                          Invoice.InvoiceId
                                         ,Invoice.Id
                                         ,Invoice.Name
                                         ,Invoice.Title
                                         ,Invoice.InvoiceStatusId
                                         ,Invoice.WorkPeriodFrom
                                         ,Invoice.WorkPeriodTo
                                         ,Invoice.PurchaseOrderNumber
                                         ,Invoice.TotalIncludingTax
                                         ,Invoice.TotalExcludingTax
                                         ,Invoice.Date
                                         ,Invoice.ProjectSequenceCode
                                         ,Invoice.ProductTitle
                                         ,PsHeader.ProgressStatementId AS PsId
                                        FROM dbo.Invoice
                                        INNER JOIN dbo.PsHeader
                                          ON Invoice.PsId = PsHeader.Id WHERE InvoiceId= @id";
        Invoice mInvoice;

        using (var connection = new SqlConnection(cuConnectionString))
        {
            mInvoice = connection.Query<Invoice>(readImvoiceHeader, param).SingleOrDefault();
            connection.Close();
        }

        return mInvoice;
    }

    public async Task<string> CUInReviewInvoice(InvoiceParameter parameter)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId, null,
            parameter.TenantProvider);

        var psUpdateQuery = @"update Invoice set InvoiceStatusId = '7143ff01-d173-4a20-8c17-cacdfecdb84c'
                                    where Id = @id";

        var param = new { id = parameter.InvoiceId };

        using var connection = new SqlConnection(cuConnectionString);
        await connection.QueryAsync(psUpdateQuery, param);
        connection.Close();
        return "ok";
    }
}