using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Data.WH;
using UPrinceV4.Web.Repositories.CPC;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PO;
using UPrinceV4.Web.Util;
using TimeZone = UPrinceV4.Web.Data.TimeZone;

namespace UPrinceV4.Web.Repositories.PO;

public class PORepository : IPORepository

{
    public async Task<IEnumerable<POListDto>> GetPOList(POParameter pOParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pOParameter.ContractingUnitSequenceId,
            pOParameter.ProjectSequenceId, pOParameter.TenantProvider);
        StringBuilder sb;

        var query = @"SELECT
                                      POType.Name  AS Type
                                     ,POType.TypeId
                                     ,POStatus.Name  AS Status
                                     ,POHeader.Title AS Title
                                     ,POHeader.Id
                                     ,POHeader.SequenceId
                                     ,POHeader.ProjectSequenceCode
                                     ,POHeader.ModifiedDate AS LastModifiedDate
                                     ,POHeader.CustomerCompanyId
                                     ,POHeader.SuplierCompanyId
                                    ,POHeader.NoOfMaterials
                                    ,POHeader.NoOfTools
                                    ,POHeader.NoOfConsumables
                                    ,POHeader.NoOfLabours
                                    ,POHeader.ModifiedBy
                                    ,POHeader.TotalAmount
                                    ,POHeader.PORequestType As RequestTypeId
                                    ,PORequestType.Name AS RequestType

                                    FROM dbo.POHeader
                                    LEFT OUTER JOIN dbo.POType
                                      ON POHeader.POTypeId = POType.TypeId
                                    LEFT OUTER JOIN dbo.POStatus
                                      ON POHeader.POStatusId = POStatus.StatusId 
                                    LEFT OUTER JOIN dbo.PORequestType
                                      ON POHeader.PORequestType = PORequestType.RequestTypeId
                                    WHERE POStatus.LanguageCode = @lang
                                    AND POType.LanguageCode = @lang
                                    AND PORequestType.LanguageCode = @lang ";

        var query2 = @"SELECT
                                      POType.Name  AS Type
                                     ,POType.TypeId
                                     ,POStatus.Name  AS Status
                                     ,POHeader.Title AS Title
                                     ,POHeader.Id
                                     ,POHeader.SequenceId
                                     ,POHeader.ProjectSequenceCode
                                     ,POHeader.ModifiedDate AS LastModifiedDate
                                     ,POHeader.CustomerCompanyId
                                     ,POHeader.SuplierCompanyId
                                    ,POHeader.NoOfMaterials
                                    ,POHeader.NoOfTools
                                    ,POHeader.NoOfConsumables
                                    ,POHeader.NoOfLabours
                                    ,POHeader.ModifiedBy
                                    ,POHeader.TotalAmount
                                    ,POHeader.PORequestType As RequestTypeId
                                    ,PORequestType.Name AS RequestType
                                    FROM dbo.POHeader
                                    LEFT OUTER JOIN dbo.POType
                                      ON POHeader.POTypeId = POType.TypeId
                                    LEFT OUTER JOIN dbo.POStatus
                                      ON POHeader.POStatusId = POStatus.StatusId 
                                    LEFT OUTER JOIN dbo.PORequestType
                                      ON POHeader.PORequestType = PORequestType.RequestTypeId
                                    WHERE POStatus.LanguageCode = @lang
                                    AND POType.LanguageCode = @lang
                                    AND PORequestType.LanguageCode = @lang ";

        if (pOParameter.ProjectSequenceId != null)
            sb = new StringBuilder(query);
        else
            sb = new StringBuilder(query2);


        if (pOParameter.Filter.Title != null)
        {
            pOParameter.Filter.Title = pOParameter.Filter.Title.Replace("'", "''");

            var words = pOParameter.Filter.Title.Split(" ");
            foreach (var element in words) sb.Append(" AND POHeader.Title LIKE '%" + element + "%'");
            //sb.Append(" AND POHeader.Title like '%" + pOParameter.Filter.Title + "%' ");
        }

        //if (pOParameter.Filter.ProjectSequenceCode != null)
        //{
        //        sb.Append(" AND POHeader.ProjectSequenceCode LIKE '%" + pOParameter.Filter.ProjectSequenceCode + "%'");

        //    //sb.Append(" AND POHeader.Title like '%" + pOParameter.Filter.Title + "%' ");
        //}

        if (pOParameter.Filter.TotalAmount != null)
            pOParameter.Filter.TotalAmount = pOParameter.Filter.TotalAmount.Replace("'", "''");
            sb.Append(" AND POHeader.TotalAmount like '%" + pOParameter.Filter.TotalAmount + "%' ");

        if (pOParameter.Filter.StatusId != null)
        {
            if (pOParameter.Filter.StatusId == "7143ff01-d173-4a20-8c17-cacdfecdb84c")
                sb.Append(
                    " AND POStatus.StatusId IN ('7143ff01-d173-4a20-8c17-cacdfecdb84c','7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback', '7143ff01-d173-4a20-8c17-cacdfecdb84c-accept') ");
            else
                sb.Append(" AND POStatus.StatusId = '" + pOParameter.Filter.StatusId + "' ");
        }


        if (pOParameter.Filter.LastModifiedDate != null)
        {
            var gmt = FindGmtDatetime(pOParameter);
            sb.Append("  AND POHeader.ModifiedDate BETWEEN '" + gmt + "' AND '" + gmt.AddHours(24) + "' ");
        }

        if (pOParameter.Filter.Resource != null)
        {
            if (pOParameter.Filter.Resource == "c46c3a26-39a5-42cc-n7k1-89655304eh6")
                sb.Append("  AND POHeader.NoOfMaterials > 0 ");

            if (pOParameter.Filter.Resource == "c46c3a26-39a5-42cc-m06g-89655304eh6")
                sb.Append("  AND POHeader.NoOfConsumables > 0 ");

            if (pOParameter.Filter.Resource == "c46c3a26-39a5-42cc-b07s-89655304eh6")
                sb.Append("  AND POHeader.NoOfLabours > 0 ");

            if (pOParameter.Filter.Resource == "c46c3a26-39a5-42cc-n9wn-89655304eh6")
                sb.Append("  AND POHeader.NoOfTools > 0 ");
        }

        if (pOParameter.Filter.RequestTypeId != null)
            sb.Append(" AND PORequestType.RequestTypeId = '" + pOParameter.Filter.RequestTypeId + "' ");
        else
            sb.Append(" AND PORequestType.RequestTypeId != 'f4d6ba08-3937-44ca-a0a1-7cf33c03e290' ");

        //if (pOParameter.Filter.StatusId != null)
        //{
        //    sb.Append(" AND PMol.TypeId = '" + pOParameter.Filter.StatusId + "' ");
        //}

        if (pOParameter.Filter.Sorter.Attribute == null)
        {
            if (pOParameter.Filter.RequestTypeId == "f4d6ba08-3937-44ca-a0a1-7cf33c03e290")
                sb.Append(" ORDER BY CAST(SUBSTRING(POHeader.SequenceId,5,20) AS INT) DESC");
            else
                sb.Append(" ORDER BY CAST(SUBSTRING(POHeader.SequenceId,4,20) AS INT) DESC");
        }


        if (pOParameter.Filter.Sorter.Attribute != null)
            switch (pOParameter.Filter.Sorter.Attribute.ToLower())
            {
                //sb.Append("ORDER BY CAST(SUBSTRING(POHeader.SequenceId,4,20) AS INT)" + pOParameter.Filter.Sorter.Order);
                case "title" when pOParameter.Filter.RequestTypeId == "f4d6ba08-3937-44ca-a0a1-7cf33c03e290":
                    sb.Append(" ORDER BY CAST(SUBSTRING(POHeader.SequenceId,5,20) AS INT) " +
                              pOParameter.Filter.Sorter.Order);
                    break;
                case "title":
                    sb.Append(" ORDER BY CAST(SUBSTRING(POHeader.SequenceId,4,20) AS INT) " +
                              pOParameter.Filter.Sorter.Order);
                    break;
                case "type":
                case "status":
                    sb.Append("ORDER BY POStatus.Name " + pOParameter.Filter.Sorter.Order);
                    break;
                case "totalamount":
                    sb.Append("ORDER BY POHeader.TotalAmount " + pOParameter.Filter.Sorter.Order);
                    break;
                case "lastmodifieddate":
                    sb.Append("ORDER BY POHeader.CreatedDate " + pOParameter.Filter.Sorter.Order);
                    break;
                case "resource":
                    sb.Append("ORDER BY POHeader.Title " + pOParameter.Filter.Sorter.Order);
                    break;
                case "projectsequencecode":
                    sb.Append("ORDER BY POHeader.projectSequenceCode " + pOParameter.Filter.Sorter.Order);
                    break;
                case "requesttype":
                    sb.Append("ORDER BY PORequestType.Name " + pOParameter.Filter.Sorter.Order);
                    break;
            }

        var parameters = new { lang = pOParameter.Lang };
        IEnumerable<POListDto> data;
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            data = await connection.QueryAsync<POListDto>(sb.ToString(), parameters);
            
        }

        List<CabCompanyDto> Companies;
        using (var connection =
               new SqlConnection(pOParameter.TenantProvider.GetTenant().ConnectionString))
        {
            Companies = connection
                .Query<CabCompanyDto>("SELECT CabCompany.Name, CabCompany.Id AS[Key] FROM dbo.CabCompany").ToList();
        }

        // var sqlCabPersopn = @"SELECT CabPerson.* FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Oid = 'e3ab6b9e-83b1-4471-9c62-32352670a2ba'";

        List<ApplicationUser> ApplicationUserList;
        using (var connection =
               new SqlConnection(pOParameter.TenantProvider.GetTenant().ConnectionString))
        {
            ApplicationUserList = connection.Query<ApplicationUser>("SELECT * FROM dbo.ApplicationUser").ToList();
        }

        foreach (var mPOListDto in data)
        {
            var customer = Companies.Where(c => c.Key == mPOListDto.CustomerCompanyId).FirstOrDefault();
            if (customer != null) mPOListDto.Customer = customer.Name;

            var suplier = Companies.Where(c => c.Key == mPOListDto.SuplierCompanyId).FirstOrDefault();
            if (suplier != null) mPOListDto.Supplier = suplier.Name;

            var ModifiedBy = ApplicationUserList.Where(c => c.OId == mPOListDto.ModifiedBy).FirstOrDefault();

            if (ModifiedBy != null)
                mPOListDto.ModifiedBy = ModifiedBy.FirstName + " " + ModifiedBy.LastName;
            else
                mPOListDto.ModifiedBy = null;
        }

        if (pOParameter.Filter.Customer != null)
            data = data.Where(x =>
                x.Customer != null &&
                x.Customer.Contains(pOParameter.Filter.Customer, StringComparison.OrdinalIgnoreCase));

        if (pOParameter.Filter.ModifiedBy != null)
            data = data.Where(x =>
                x.ModifiedBy != null && x.ModifiedBy.Contains(pOParameter.Filter.ModifiedBy,
                    StringComparison.OrdinalIgnoreCase));

        if (pOParameter.Filter.Supplier != null)
            data = data.Where(x => x.Supplier != null && x.Supplier.Contains(pOParameter.Filter.Supplier.ToLower(),
                StringComparison.OrdinalIgnoreCase));


        if (pOParameter.Filter.Sorter.Attribute != null)
        {
            if (pOParameter.Filter.Sorter.Attribute.ToLower().Equals("supplier"))
            {
                //sb.Append("ORDER BY POHeader.Title " + pOParameter.Filter.Sorter.Order);
                if (pOParameter.Filter.Sorter.Order.ToLower().Equals("asc"))
                    data = from std in data
                        orderby std.Supplier
                        select std;
                else
                    //data = data.OrderByDescending(x => x.Supplier).ToList();
                    data = (from std in data
                        orderby std.Supplier descending
                        select std).ToList();
            }

            if (pOParameter.Filter.Sorter.Attribute.ToLower().Equals("customer"))
            {
                if (pOParameter.Filter.Sorter.Order.ToLower().Equals("asc"))
                    data = (from std in data
                        orderby std.Customer
                        select std).ToList();
                else
                    //ata = data.OrderByDescending(x => x.Customer).ToList();
                    data = (from std in data
                        orderby std.Customer descending
                        select std).ToList();
            }

            if (pOParameter.Filter.Sorter.Attribute.ToLower().Equals("modifiedby"))
            {
                //sb.Append("ORDER BY POHeader.Title " + pOParameter.Filter.Sorter.Order);

                if (pOParameter.Filter.Sorter.Order.ToLower().Equals("asc"))
                    data = (from std in data
                        orderby std.ModifiedBy
                        select std).ToList();
                // data = data.OrderBy(x => x.Customer).ToList();
                else
                    //ata = data.OrderByDescending(x => x.Customer).ToList();
                    data = (from std in data
                        orderby std.ModifiedBy descending
                        select std).ToList();
            }
        }

        var projectParameters = new
            { sqCode = data.Where(v => v.ProjectSequenceCode != null).Select(ps => ps.ProjectSequenceCode).Distinct() };
        IEnumerable<ProjectDefinitionMobDto> project;
        using (var dbConnection =
               new SqlConnection(pOParameter.TenantProvider.GetTenant().ConnectionString))
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

        foreach (var Ps in data)
            Ps.ProjectSequenceCode = project
                .Where(p => p.SequenceCode != null && p.SequenceCode == Ps.ProjectSequenceCode).FirstOrDefault()
                ?.Title;

        if (pOParameter.Filter.ProjectSequenceCode != null)
        {
            pOParameter.Filter.ProjectSequenceCode = pOParameter.Filter.ProjectSequenceCode.Replace("'", "''");
            data = data.Where(X => X.ProjectSequenceCode != null && X.ProjectSequenceCode.ToLower()
                .Contains(pOParameter.Filter.ProjectSequenceCode
                    .ToLower()));
        }
        return data;
    }

    public async Task<IEnumerable<POShortcutpaneDataDto>> GetShortcutpaneData(POParameter POParameter)
    {
        try
        {
            var query =
                @"SELECT POStatus.Id ,POStatus.Name ,POStatus.StatusId AS Value FROM dbo.POStatus WHERE POStatus.LanguageCode = @lang AND POStatus.StatusId NOT IN ('7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback', '7143ff01-d173-4a20-8c17-cacdfecdb84c-accept') ORDER BY POStatus.DisplayOrder";
            var queryCPCType =
                @"SELECT CpcResourceTypeLocalizedData.Label AS Name, CpcResourceTypeLocalizedData.CpcResourceTypeId AS Value, CpcResourceTypeLocalizedData.Id, 'Resource' as Type FROM dbo.CpcResourceTypeLocalizedData WHERE CpcResourceTypeLocalizedData.LanguageCode = @lang ORDER BY CASE WHEN CpcResourceTypeLocalizedData.Label = 'Materials' THEN 1 WHEN CpcResourceTypeLocalizedData.Label = 'Tools' THEN 2 WHEN CpcResourceTypeLocalizedData.Label = 'Consumables' THEN 3 WHEN CpcResourceTypeLocalizedData.Label = 'Human Resources' THEN 4 ELSE (ROW_NUMBER() OVER (ORDER BY CpcResourceTypeLocalizedData.Label)+4) END ASC";

            var poType =
                @"SELECT Id,Name,RequestTypeId AS Value, 'RequestType' As Type FROM PORequestType WHERE PORequestType.LanguageCode = @lang order BY DisplayOrder";

            var parameters = new { lang = POParameter.Lang };
            List<POShortcutpaneDataDto> data;
            using (var connection = new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<POShortcutpaneDataDto>(query, parameters).ToList();
                data.AddRange(connection.Query<POShortcutpaneDataDto>(queryCPCType, parameters).ToList());
                data.AddRange(connection.Query<POShortcutpaneDataDto>(poType, parameters).ToList());
            }

            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task<string> CreateHeader(POParameter POParameter, bool isCopy, bool toCu)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);
        string POSequenceId = null;

        if (isCopy)
            if (toCu)
                connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
                    POParameter.TenantProvider);

        if (POParameter.PoDto.IsClone) POParameter.ProjectSequenceId = POParameter.PoDto.ProjectSequenceCode;


        if (POParameter.PoDto.Id != null)
        {
            var query1 = @"SELECT [Id] ,[SequenceId] FROM [dbo].[POHeader] where Id = @Id";

            var parameter = new { POParameter.PoDto.Id };
            POHeader data;
            using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<POHeader>(query1, parameter).FirstOrDefault();
            }

            string requestType;
            if (POParameter.PoDto.DeliveryLocation == null)
                requestType = POParameter.PoDto.PoRequestTypeId;
            else
                requestType = "343482458-0spr-poa3-b0f9-c2e40344clll";

            try
            {
                POHeaderDto createdPO = null;

                if (data == null)
                {
                    if (!isCopy)
                    {
                        var idGenerator = new IdGenerator();

                        if (POParameter.PoDto.PoRequestTypeId == "343482458-0spr-poa3-b0f9-c2e40344clll")
                            POSequenceId = idGenerator.GenerateId(applicationDbContext, "PO-", "POSequence");
                        if (POParameter.PoDto.PoRequestTypeId == "94282458-0b40-poa3-b0f9-c2e40344c8f1")
                        {
                            POSequenceId = idGenerator.GenerateId(applicationDbContext, "PO-", "POSequence");
                            POSequenceId = Regex.Replace(POSequenceId, "PO", "PR");
                        }

                        if (POParameter.PoDto.PoRequestTypeId == "4010e768-3e06-po02-b337-ee367a82addb")
                        {
                            POSequenceId = idGenerator.GenerateId(applicationDbContext, "PO-", "POSequence");
                            POSequenceId = Regex.Replace(POSequenceId, "PO", "RR");
                        }

                        if (POParameter.PoDto.PoRequestTypeId == "lll82458-0b40-poa3-b0f9-c2e40344clll")
                        {
                            POSequenceId = idGenerator.GenerateId(applicationDbContext, "PO-", "POSequence");
                            POSequenceId = Regex.Replace(POSequenceId, "PO", "CR");
                        }
                    }
                    else
                    {
                        POSequenceId = POParameter.PoDto.SequenceId;
                    }

                    var query =
                        @"INSERT INTO [dbo].[POHeader] ([Id],[Title],[Name],[POTypeId],[POStatusId],[Comments],[ProjectSequenceCode],[IsDeleted],[SequenceId],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[CustomerCompanyId],[CustomerId],[CustomerReference],[SuplierCompanyId],[SupplierCabPersonCompanyId],[SupplierReference], [NoOfMaterials] ,[NoOfTools] ,[NoOfConsumables] ,[NoOfLabours], [DeliveryDate],[LocationId], [TotalAmount], [IsClone] ,[IsCu] ,[DeliveryRequest] ,[TaxonomyId],PORequestType,RequestedDate,ExpectedDate,AvailableProbability,RequestedProbability ) VALUES (@Id ,@Title ,@Name ,@POTypeId ,@POStatusId ,@Comments ,@ProjectSequenceCode ,@IsDeleted ,@SequenceId ,@CreatedBy ,@CreatedDate ,@ModifiedBy ,@ModifiedDate ,@CustomerCompanyId ,@CustomerId ,@CustomerReference ,@SuplierCompanyId ,@SupplierCabPersonCompanyId ,@SupplierReference ,@NoOfMaterials ,@NoOfTools ,@NoOfConsumables ,@NoOfLabours, @DeliveryDate,@LocationId, @TotalAmount, @IsClone , @IsCu ,@DeliveryLocation ,@WarehouseTaxonomyId,@PORequestType,@RequestedDate,@ExpectedDate, @AvailableProbability, @RequestedProbability )";

                    var parameters = new
                    {
                        POParameter.PoDto.Id,
                        Title = POSequenceId + " " + POParameter.PoDto.Name,
                        POParameter.PoDto.Name,
                        POParameter.PoDto.POTypeId,
                        POParameter.PoDto.POStatusId,
                        POParameter.PoDto.Comments,
                        ProjectSequenceCode = POParameter.ProjectSequenceId,
                        ISDeleted = false,
                        SequenceId = POSequenceId,
                        CreatedBy = POParameter.UserId,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedBy = POParameter.UserId,
                        ModifiedDate = DateTime.UtcNow,
                        CustomerCompanyId = POParameter.PoDto.CustomerOrganisationId,
                        CustomerId = POParameter.PoDto.CustomerContactId,
                        POParameter.PoDto.CustomerReference,
                        SuplierCompanyId = POParameter.PoDto.SupplierOrganisationId,
                        SupplierCabPersonCompanyId = POParameter.PoDto.SupplierContactId,
                        POParameter.PoDto.SupplierReference,
                        NoOfMaterials = POParameter.PoDto.POResources.materials.Count,
                        NoOfTools = POParameter.PoDto.POResources.tools.Count,
                        NoOfConsumables = POParameter.PoDto.POResources.consumable.Count,
                        NoOfLabours = POParameter.PoDto.POResources.labour.Count,
                        POParameter.PoDto.DeliveryDate,
                        POParameter.PoDto.LocationId,
                        POParameter.PoDto.TotalAmount,
                        POParameter.PoDto.IsClone,
                        POParameter.PoDto.IsCu,
                        POParameter.PoDto.DeliveryLocation,
                        POParameter.PoDto.WarehouseTaxonomyId,
                        PORequestType = requestType,
                        POParameter.PoDto.RequestedDate,
                        POParameter.PoDto.ExpectedDate,
                        POParameter.PoDto.AvailableProbability,
                        POParameter.PoDto.RequestedProbability
                    };

                    if (POParameter.PoDto.IsDeliveryPlan)
                    {
                        var query2 =
                            "SELECT SequenceId FROM [dbo].[POHeader] WHERE ProjectSequenceCode = @ProjectSequenceCode ";


                        using (var connection = new SqlConnection(connectionString))
                        {
                            createdPO = connection.Query<POHeaderDto>(query2,
                                new { POParameter.PoDto.ProjectSequenceCode }).FirstOrDefault();
                        }

                        if (createdPO == null)
                            using (var connection = new SqlConnection(connectionString))
                            {
                                await connection.ExecuteAsync(query, parameters);
                            }
                    }
                    else
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(query, parameters);
                        }
                    }
                }

                else
                {
                    POSequenceId = data.SequenceId;
                    var query =
                        @"UPDATE[dbo].[POHeader] SET [Title] = @Title ,[Name] = @Name ,[POTypeId] = @POTypeId ,[POStatusId] = @POStatusId ,[Comments] = @Comments ,[IsDeleted] = @ISDeleted ,[ModifiedBy] = @ModifiedBy ,[ModifiedDate] = @ModifiedDate ,[CustomerCompanyId] = @CustomerCompanyId ,[CustomerId] = @CustomerId ,[CustomerReference] = @CustomerReference ,[SuplierCompanyId] = @SuplierCompanyId ,[SupplierCabPersonCompanyId] = @SupplierCabPersonCompanyId ,[SupplierReference] = @SupplierReference , [NoOfMaterials] = @NoOfMaterials ,[NoOfTools] = @NoOfTools ,[NoOfConsumables] = @NoOfConsumables ,[NoOfLabours] = @NoOfLabours, [DeliveryDate] = @DeliveryDate, [LocationId] = @LocationId, [TotalAmount] = @TotalAmount, [DeliveryRequest] = @DeliveryLocation, [TaxonomyId] = @WarehouseTaxonomyId, PORequestType = @PORequestType, RequestedDate = @RequestedDate, ExpectedDate = @ExpectedDate, AvailableProbability = @AvailableProbability, RequestedProbability = @RequestedProbability  WHERE Id = @Id";

                    var parameters = new
                    {
                        POParameter.PoDto.Id,
                        Title = data.SequenceId + " " + POParameter.PoDto.Name,
                        POParameter.PoDto.Name,
                        POParameter.PoDto.POTypeId,
                        POParameter.PoDto.POStatusId,
                        POParameter.PoDto.Comments,
                        ISDeleted = false,
                        ModifiedBy = POParameter.UserId,
                        ModifiedDate = DateTime.UtcNow,
                        CustomerCompanyId = POParameter.PoDto.CustomerOrganisationId,
                        CustomerId = POParameter.PoDto.CustomerContactId,
                        POParameter.PoDto.CustomerReference,
                        SuplierCompanyId = POParameter.PoDto.SupplierOrganisationId,
                        SupplierCabPersonCompanyId = POParameter.PoDto.SupplierContactId,
                        POParameter.PoDto.SupplierReference,
                        NoOfMaterials = POParameter.PoDto.POResources.materials.Count,
                        NoOfTools = POParameter.PoDto.POResources.tools.Count,
                        NoOfConsumables = POParameter.PoDto.POResources.consumable.Count,
                        NoOfLabours = POParameter.PoDto.POResources.labour.Count,
                        POParameter.PoDto.DeliveryDate,
                        POParameter.PoDto.LocationId,
                        POParameter.PoDto.TotalAmount,
                        POParameter.PoDto.DeliveryLocation,
                        POParameter.PoDto.WarehouseTaxonomyId,
                        PORequestType = requestType,
                        POParameter.PoDto.RequestedDate,
                        POParameter.PoDto.ExpectedDate,
                        POParameter.PoDto.AvailableProbability,
                        POParameter.PoDto.RequestedProbability
                    };


                    var deleteQueary = @"DELETE FROM dbo.POResources WHERE PurchesOrderId = @Id;";
                    var deleteResource = @"DELETE FROM dbo.POResourcesDocument WHERE POHeaderId = @Id;";
                    var pOResourcesDocumentC = @"DELETE FROM dbo.POResourcesDocumentC WHERE POHeaderId = @Id;";

                    var deleteResourceHeader = @"DELETE FROM dbo.PODocument WHERE POHeaderId = @Id;";
                    var deleteQuearyPOProduct = @"DELETE FROM dbo.POProduct WHERE POHeaderId = @Id;";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.ExecuteAsync(query, parameters);
                        await connection.ExecuteAsync(deleteQueary, parameters);
                        await connection.ExecuteAsync(deleteResource, parameters);
                        await connection.ExecuteAsync(deleteResourceHeader, parameters);
                        await connection.ExecuteAsync(deleteQuearyPOProduct, parameters);
                        await connection.ExecuteAsync(pOResourcesDocumentC, parameters);
                    }
                }

                if (createdPO == null)
                {
                    var reseourceInsertHeader =
                        @"INSERT INTO dbo.PODocument(Id ,Link ,POHeaderId ) VALUES (@Id,@Link,@POHeaderId);";
                    if (POParameter.PoDto.files != null)
                        foreach (var mdoc in POParameter.PoDto.files)
                            using (var connection = new SqlConnection(connectionString))
                            {
                                await connection.ExecuteAsync(reseourceInsertHeader,
                                    new
                                    {
                                        Id = Guid.NewGuid().ToString(), Link = mdoc,
                                        POHeaderId = POParameter.PoDto.Id
                                    });
                            }

                    if (POParameter.PoDto.POTypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1" ||
                        POParameter.PoDto.POTypeId == "94282458-0b40-capa-b0f9-c2e40344c8f1")
                    {
                        var resoursequery =
                            @"INSERT INTO dbo.POResources ( Id ,PurchesOrderId ,PComments ,PInvoiced ,BorId ,PTotalPrice ,PPurchased ,PStartDate ,PStopDate ,PUnitPrice ,Date ,PFullTimeEmployee ,BorTitle ,PCrossReference ,PConsumed ,CorporateProductCatalogId ,CDeliveryRequested ,PDeliveryRequested ,PNumberOfDate ,PQuantity ,ResourceNumber ,warf ,CComments ,CCrossReference ,CFullTimeEmployee ,CNumberOfDate ,CPurchased ,CQuantity ,CStartDate ,CStopDate ,CTotalPrice ,CUnitPrice ,ResourcesType ,Cdevices ,Pdevices ,ProjectTitle ,PbsTitle ,CTitle ,PTitle ,CMou ,PMou ,CCPCId ,PCPCId, ResourceFamily, BorResourceId, IsUsed, HasChanged, IsStock, RequestedDeliveryDate, ExpectedDeliveryDate, PbsProductId) VALUES ( @Id ,@PurchesOrderId ,@PComments ,@PInvoiced ,@BorId ,@PTotalPrice ,@PPurchased ,@PStartDate ,@PStopDate ,@PUnitPrice ,@Date ,@PFullTimeEmployee ,@BorTitle ,@PCrossReference ,@PConsumed ,@CorporateProductCatalogId ,@CDeliveryRequested ,@PDeliveryRequested ,@PNumberOfDate ,@PQuantity ,@ResourceNumber ,@warf ,@CComments ,@CCrossReference ,@CFullTimeEmployee ,@CNumberOfDate ,@CPurchased ,@CQuantity ,@CStartDate ,@CStopDate ,@CTotalPrice ,@CUnitPrice ,@ResourcesType ,@Cdevices ,@Pdevices ,@ProjectTitle,@PbsTitle,@CTitle,@PTitle,@CMou,@PMou,@CCPCId,@PCPCId, @ResourceFamily, @BorResourceId, @IsUsed, @HasChanged, @IsStock, @RequestedDeliveryDate, @ExpectedDeliveryDate, @PbsProductId);";

                        var updatePoResource =
                            @"UPDATE dbo.POResources SET [IsUsed] = @IsUsed,[UsedPoId] = @UsedPoId where Id =@Id";
                        var reseourceInsert =
                            @"INSERT INTO dbo.POResourcesDocument ( Id ,Link ,POResourcesId, POHeaderId ) VALUES ( @Id ,@Link ,@POResourcesId, @POHeaderId );";

                        var POResourcesDocumentCInsert =
                            @"INSERT INTO dbo.POResourcesDocumentC ( Id ,Link ,POResourcesId, POHeaderId ) VALUES ( @Id ,@Link ,@POResourcesId, @POHeaderId );";


                        //if (POParameter.PoDto.POResources.tools.FirstOrDefault() != null)
                        //{
                        //    POResourcesAddDto mPOResourcesTool = POParameter.PoDto.POResources.tools.FirstOrDefault();
                        //    if (mPOResourcesTool.PurchesOrderId != POParameter.PoDto.Id)
                        //    {
                        //        using (var connection = new SqlConnection(connectionString))
                        //        {
                        //            await connection.ExecuteAsync("UPDATE dbo.POResources SET [IsUsed] = @IsUsed where UsedPoId =@Id", new { Id = POParameter.PoDto.Id, IsUsed = false });
                        //        }
                        //    }


                        //}

                        //if (POParameter.PoDto.POResources.materials.FirstOrDefault() != null)
                        //{
                        //    POResourcesAddDto mPOResources = POParameter.PoDto.POResources.materials.FirstOrDefault();
                        //    if (mPOResources.PurchesOrderId != POParameter.PoDto.Id)
                        //    {
                        //        using (var connection = new SqlConnection(connectionString))
                        //        {
                        //            await connection.ExecuteAsync("UPDATE dbo.POResources SET [IsUsed] = @IsUsed where UsedPoId =@Id", new { Id = POParameter.PoDto.Id, IsUsed = false });
                        //        }
                        //    }
                        //}

                        //if (POParameter.PoDto.POResources.consumable.FirstOrDefault() != null)
                        //{
                        //    POResourcesAddDto mPOResources = POParameter.PoDto.POResources.consumable.FirstOrDefault();
                        //    if (mPOResources.PurchesOrderId != POParameter.PoDto.Id)
                        //    {
                        //        using (var connection = new SqlConnection(connectionString))
                        //        {
                        //            await connection.ExecuteAsync("UPDATE dbo.POResources SET [IsUsed] = @IsUsed where UsedPoId =@Id", new { Id = POParameter.PoDto.Id, IsUsed = false });
                        //        }
                        //    }
                        //}

                        //if (POParameter.PoDto.POResources.labour.FirstOrDefault() != null)
                        //{
                        //    POResourcesAddDto mPOResources = POParameter.PoDto.POResources.labour.FirstOrDefault();
                        //    if (mPOResources.PurchesOrderId != POParameter.PoDto.Id)
                        //    {
                        //        using (var connection = new SqlConnection(connectionString))
                        //        {
                        //            await connection.ExecuteAsync("UPDATE dbo.POResources SET [IsUsed] = @IsUsed where UsedPoId =@Id", new { Id = POParameter.PoDto.Id, IsUsed = false });
                        //        }
                        //    }
                        //}

                        if (POParameter.PoDto.POResources.tools != null)
                            foreach (var mPOResourcesTool in POParameter.PoDto.POResources.tools)
                            {
                                var resourcceId = Guid.NewGuid().ToString();
                                var reparameters = new
                                {
                                    Id = resourcceId,
                                    PurchesOrderId = POParameter.PoDto.Id,
                                    mPOResourcesTool.PComments,
                                    mPOResourcesTool.PInvoiced,
                                    mPOResourcesTool.BorId,
                                    mPOResourcesTool.PTotalPrice,
                                    mPOResourcesTool.PPurchased,
                                    mPOResourcesTool.PStartDate,
                                    mPOResourcesTool.PStopDate,
                                    mPOResourcesTool.PUnitPrice,
                                    mPOResourcesTool.Date,
                                    mPOResourcesTool.PFullTimeEmployee,
                                    mPOResourcesTool.BorTitle,
                                    mPOResourcesTool.PCrossReference,
                                    mPOResourcesTool.PConsumed,
                                    mPOResourcesTool.CorporateProductCatalogId,
                                    mPOResourcesTool.CDeliveryRequested,
                                    mPOResourcesTool.PDeliveryRequested,
                                    mPOResourcesTool.PNumberOfDate,
                                    mPOResourcesTool.PQuantity,
                                    mPOResourcesTool.ResourceNumber,
                                    mPOResourcesTool.warf,
                                    mPOResourcesTool.CComments,
                                    mPOResourcesTool.CCrossReference,
                                    mPOResourcesTool.CFullTimeEmployee,
                                    mPOResourcesTool.CNumberOfDate,
                                    mPOResourcesTool.CPurchased,
                                    mPOResourcesTool.CQuantity,
                                    mPOResourcesTool.CStartDate,
                                    mPOResourcesTool.CStopDate,
                                    mPOResourcesTool.CTotalPrice,
                                    mPOResourcesTool.CUnitPrice,
                                    ResourcesType = "c46c3a26-39a5-42cc-n9wn-89655304eh6",
                                    mPOResourcesTool.Cdevices,
                                    mPOResourcesTool.Pdevices,
                                    mPOResourcesTool.ProjectTitle,
                                    mPOResourcesTool.PbsTitle,
                                    mPOResourcesTool.CMou,
                                    mPOResourcesTool.PMou,
                                    mPOResourcesTool.CCPCId,
                                    mPOResourcesTool.PCPCId,
                                    mPOResourcesTool.CTitle,
                                    mPOResourcesTool.PTitle,
                                    mPOResourcesTool.ResourceFamily,
                                    mPOResourcesTool.BorResourceId,
                                    mPOResourcesTool.IsUsed,
                                    mPOResourcesTool.HasChanged,
                                    mPOResourcesTool.IsStock,
                                    mPOResourcesTool.RequestedDeliveryDate,
                                    mPOResourcesTool.ExpectedDeliveryDate,
                                    mPOResourcesTool.PbsProductId
                                };

                                var usedParam = new
                                {
                                    mPOResourcesTool.Id,
                                    mPOResourcesTool.IsUsed,
                                    UsedPoId = POParameter.PoDto.Id
                                };

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(resoursequery, reparameters);
                                    await connection.ExecuteAsync(updatePoResource, usedParam);
                                }

                                if (mPOResourcesTool.PDocuments != null)
                                    foreach (var mdoc in mPOResourcesTool.PDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(reseourceInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }

                                if (mPOResourcesTool.CDocuments != null)
                                    foreach (var mdoc in mPOResourcesTool.CDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(POResourcesDocumentCInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }
                            }

                        if (POParameter.PoDto.POResources.materials != null)
                            foreach (var mPOResourcesTool in POParameter.PoDto.POResources.materials)
                            {
                                var resourcceId = Guid.NewGuid().ToString();

                                var reparameters = new
                                {
                                    Id = resourcceId,
                                    PurchesOrderId = POParameter.PoDto.Id,
                                    mPOResourcesTool.PComments,
                                    mPOResourcesTool.PInvoiced,
                                    mPOResourcesTool.BorId,
                                    mPOResourcesTool.PTotalPrice,
                                    mPOResourcesTool.PPurchased,
                                    mPOResourcesTool.PStartDate,
                                    mPOResourcesTool.PStopDate,
                                    mPOResourcesTool.PUnitPrice,
                                    mPOResourcesTool.Date,
                                    mPOResourcesTool.PFullTimeEmployee,
                                    mPOResourcesTool.BorTitle,
                                    mPOResourcesTool.PCrossReference,
                                    mPOResourcesTool.PConsumed,
                                    mPOResourcesTool.CorporateProductCatalogId,
                                    mPOResourcesTool.CDeliveryRequested,
                                    mPOResourcesTool.PDeliveryRequested,
                                    mPOResourcesTool.PNumberOfDate,
                                    mPOResourcesTool.PQuantity,
                                    mPOResourcesTool.ResourceNumber,
                                    mPOResourcesTool.warf,
                                    mPOResourcesTool.CComments,
                                    mPOResourcesTool.CCrossReference,
                                    mPOResourcesTool.CFullTimeEmployee,
                                    mPOResourcesTool.CNumberOfDate,
                                    mPOResourcesTool.CPurchased,
                                    mPOResourcesTool.CQuantity,
                                    mPOResourcesTool.CStartDate,
                                    mPOResourcesTool.CStopDate,
                                    mPOResourcesTool.CTotalPrice,
                                    mPOResourcesTool.CUnitPrice,
                                    ResourcesType = "c46c3a26-39a5-42cc-n7k1-89655304eh6",
                                    mPOResourcesTool.Cdevices,
                                    mPOResourcesTool.Pdevices,
                                    mPOResourcesTool.ProjectTitle,
                                    mPOResourcesTool.PbsTitle,
                                    mPOResourcesTool.CMou,
                                    mPOResourcesTool.PMou,
                                    mPOResourcesTool.CCPCId,
                                    mPOResourcesTool.PCPCId,
                                    mPOResourcesTool.CTitle,
                                    mPOResourcesTool.PTitle,
                                    mPOResourcesTool.ResourceFamily,
                                    mPOResourcesTool.BorResourceId,
                                    mPOResourcesTool.IsUsed,
                                    mPOResourcesTool.HasChanged,
                                    mPOResourcesTool.IsStock,
                                    mPOResourcesTool.RequestedDeliveryDate,
                                    mPOResourcesTool.ExpectedDeliveryDate,
                                    mPOResourcesTool.PbsProductId
                                };


                                var usedParam = new
                                {
                                    mPOResourcesTool.Id,
                                    mPOResourcesTool.IsUsed,
                                    UsedPoId = POParameter.PoDto.Id
                                };

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(resoursequery, reparameters);
                                    await connection.ExecuteAsync(updatePoResource, usedParam);
                                }

                                if (mPOResourcesTool.PDocuments != null)
                                    foreach (var mdoc in mPOResourcesTool.PDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(reseourceInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }

                                if (mPOResourcesTool.CDocuments != null)
                                    foreach (var mdoc in mPOResourcesTool.CDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(POResourcesDocumentCInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }
                            }

                        if (POParameter.PoDto.POResources.consumable != null)
                            foreach (var mPOResourcesTool in POParameter.PoDto.POResources.consumable)
                            {
                                var resourcceId = Guid.NewGuid().ToString();
                                var reparameters = new
                                {
                                    Id = resourcceId,
                                    PurchesOrderId = POParameter.PoDto.Id,
                                    mPOResourcesTool.PComments,
                                    mPOResourcesTool.PInvoiced,
                                    mPOResourcesTool.BorId,
                                    mPOResourcesTool.PTotalPrice,
                                    mPOResourcesTool.PPurchased,
                                    mPOResourcesTool.PStartDate,
                                    mPOResourcesTool.PStopDate,
                                    mPOResourcesTool.PUnitPrice,
                                    mPOResourcesTool.Date,
                                    mPOResourcesTool.PFullTimeEmployee,
                                    mPOResourcesTool.BorTitle,
                                    mPOResourcesTool.PCrossReference,
                                    mPOResourcesTool.PConsumed,
                                    mPOResourcesTool.CorporateProductCatalogId,
                                    mPOResourcesTool.CDeliveryRequested,
                                    mPOResourcesTool.PDeliveryRequested,
                                    mPOResourcesTool.PNumberOfDate,
                                    mPOResourcesTool.PQuantity,
                                    mPOResourcesTool.ResourceNumber,
                                    mPOResourcesTool.warf,
                                    mPOResourcesTool.CComments,
                                    mPOResourcesTool.CCrossReference,
                                    mPOResourcesTool.CFullTimeEmployee,
                                    mPOResourcesTool.CNumberOfDate,
                                    mPOResourcesTool.CPurchased,
                                    mPOResourcesTool.CQuantity,
                                    mPOResourcesTool.CStartDate,
                                    mPOResourcesTool.CStopDate,
                                    mPOResourcesTool.CTotalPrice,
                                    mPOResourcesTool.CUnitPrice,
                                    ResourcesType = "c46c3a26-39a5-42cc-m06g-89655304eh6",
                                    mPOResourcesTool.Cdevices,
                                    mPOResourcesTool.Pdevices,
                                    mPOResourcesTool.ProjectTitle,
                                    mPOResourcesTool.PbsTitle,
                                    mPOResourcesTool.CMou,
                                    mPOResourcesTool.PMou,
                                    mPOResourcesTool.CCPCId,
                                    mPOResourcesTool.PCPCId,
                                    mPOResourcesTool.CTitle,
                                    mPOResourcesTool.PTitle,
                                    mPOResourcesTool.ResourceFamily,
                                    mPOResourcesTool.BorResourceId,
                                    mPOResourcesTool.IsUsed,
                                    mPOResourcesTool.HasChanged,
                                    mPOResourcesTool.IsStock,
                                    mPOResourcesTool.RequestedDeliveryDate,
                                    mPOResourcesTool.ExpectedDeliveryDate,
                                    mPOResourcesTool.PbsProductId
                                };

                                var usedParam = new
                                {
                                    mPOResourcesTool.Id,
                                    mPOResourcesTool.IsUsed,
                                    UsedPoId = POParameter.PoDto.Id
                                };

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(resoursequery, reparameters);
                                    await connection.ExecuteAsync(updatePoResource, usedParam);
                                }

                                if (mPOResourcesTool.PDocuments != null)
                                    foreach (var mdoc in mPOResourcesTool.PDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(reseourceInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }

                                if (mPOResourcesTool.CDocuments != null)
                                    foreach (var mdoc in mPOResourcesTool.CDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(POResourcesDocumentCInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }
                            }

                        if (POParameter.PoDto.POResources.labour != null)
                            foreach (var mPOResourcesLabour in POParameter.PoDto.POResources.labour)
                            {
                                var resourcceId = Guid.NewGuid().ToString();
                                var reparameters = new
                                {
                                    Id = resourcceId,
                                    PurchesOrderId = POParameter.PoDto.Id,
                                    mPOResourcesLabour.PComments,
                                    mPOResourcesLabour.PInvoiced,
                                    mPOResourcesLabour.BorId,
                                    mPOResourcesLabour.PTotalPrice,
                                    mPOResourcesLabour.PPurchased,
                                    mPOResourcesLabour.PStartDate,
                                    mPOResourcesLabour.PStopDate,
                                    mPOResourcesLabour.PUnitPrice,
                                    mPOResourcesLabour.Date,
                                    mPOResourcesLabour.PFullTimeEmployee,
                                    mPOResourcesLabour.BorTitle,
                                    mPOResourcesLabour.PCrossReference,
                                    mPOResourcesLabour.PConsumed,
                                    mPOResourcesLabour.CorporateProductCatalogId,
                                    mPOResourcesLabour.CDeliveryRequested,
                                    mPOResourcesLabour.PDeliveryRequested,
                                    mPOResourcesLabour.PNumberOfDate,
                                    mPOResourcesLabour.PQuantity,
                                    mPOResourcesLabour.ResourceNumber,
                                    mPOResourcesLabour.warf,
                                    mPOResourcesLabour.CComments,
                                    mPOResourcesLabour.CCrossReference,
                                    mPOResourcesLabour.CFullTimeEmployee,
                                    mPOResourcesLabour.CNumberOfDate,
                                    mPOResourcesLabour.CPurchased,
                                    mPOResourcesLabour.CQuantity,
                                    mPOResourcesLabour.CStartDate,
                                    mPOResourcesLabour.CStopDate,
                                    mPOResourcesLabour.CTotalPrice,
                                    mPOResourcesLabour.CUnitPrice,
                                    ResourcesType = "c46c3a26-39a5-42cc-b07s-89655304eh6",
                                    mPOResourcesLabour.Cdevices,
                                    mPOResourcesLabour.Pdevices,
                                    mPOResourcesLabour.ProjectTitle,
                                    mPOResourcesLabour.PbsTitle,
                                    mPOResourcesLabour.CMou,
                                    mPOResourcesLabour.PMou,
                                    mPOResourcesLabour.CCPCId,
                                    mPOResourcesLabour.PCPCId,
                                    mPOResourcesLabour.CTitle,
                                    mPOResourcesLabour.PTitle,
                                    mPOResourcesLabour.ResourceFamily,
                                    mPOResourcesLabour.BorResourceId,
                                    mPOResourcesLabour.IsUsed,
                                    mPOResourcesLabour.HasChanged,
                                    mPOResourcesLabour.IsStock,
                                    mPOResourcesLabour.RequestedDeliveryDate,
                                    mPOResourcesLabour.ExpectedDeliveryDate,
                                    mPOResourcesLabour.PbsProductId
                                };
                                var usedParam = new
                                {
                                    mPOResourcesLabour.Id,
                                    mPOResourcesLabour.IsUsed,
                                    UsedPoId = POParameter.PoDto.Id
                                };

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(resoursequery, reparameters);
                                    await connection.ExecuteAsync(updatePoResource, usedParam);
                                }

                                if (mPOResourcesLabour.PDocuments != null)
                                    foreach (var mdoc in mPOResourcesLabour.PDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(reseourceInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }

                                if (mPOResourcesLabour.CDocuments != null)
                                    foreach (var mdoc in mPOResourcesLabour.CDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(POResourcesDocumentCInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }
                            }

                        if (POParameter.PoDto.POResources.tools.FirstOrDefault() == null &&
                            POParameter.PoDto.POResources.consumable.FirstOrDefault() == null &&
                            POParameter.PoDto.POResources.materials.FirstOrDefault() == null &&
                            POParameter.PoDto.POResources.labour.FirstOrDefault() == null)
                            using (var connection = new SqlConnection(connectionString))
                            {
                                await connection.ExecuteAsync(
                                    "UPDATE dbo.POResources SET [IsUsed] = @IsUsed where UsedPoId =@Id",
                                    new { POParameter.PoDto.Id, IsUsed = false });
                            }
                    }
                    else //products
                    {
                        var resoursequery =
                            @"INSERT INTO dbo.POProduct ( Id ,HeaderTitle ,[Key] ,Name ,PbsProductItemType ,PbsProductItemTypeId ,PbsProductStatus ,PbsProductStatusId ,PbsToleranceState ,PbsToleranceStateId ,ProductId ,Title ,PComment ,CComment ,PQuantity ,CQuantity ,Mou ,PUnitPrice ,CUnitPrice ,PTotalPrice ,CTotalPrice ,CCrossReference ,PCrossReference ,ProjectTitle ,POHeaderId,RequestedDeliveryDate,ExpectedDeliveryDate,IsPo ) VALUES ( @Id ,@HeaderTitle ,@Key ,@Name ,@PbsProductItemType ,@PbsProductItemTypeId ,@PbsProductStatus ,@PbsProductStatusId ,@PbsToleranceState ,@PbsToleranceStateId ,@ProductId ,@Title ,@PComment ,@CComment ,@PQuantity ,@CQuantity ,@Mou ,@PUnitPrice ,@CUnitPrice ,@PTotalPrice ,@CTotalPrice ,@CCrossReference ,@PCrossReference ,@ProjectTitle ,@POHeaderId,@RequestedDeliveryDate,@ExpectedDeliveryDate,@IsPo );";
                        var reseourceInsert =
                            @"INSERT INTO dbo.POResourcesDocument ( Id ,Link ,POResourcesId, POHeaderId ) VALUES ( @Id ,@Link ,@POResourcesId, @POHeaderId );";
                        var POResourcesDocumentCInsert =
                            @"INSERT INTO dbo.POResourcesDocumentC ( Id ,Link ,POResourcesId, POHeaderId ) VALUES ( @Id ,@Link ,@POResourcesId, @POHeaderId );";
                        var updatePoProduct =
                            @"UPDATE dbo.POProduct SET [IsUsed] = @IsUsed,[UsedPoId] = @UsedPoId where Id =@Id";


                        //if (POParameter.PoDto.ExternalProduct.FirstOrDefault() != null)
                        //{

                        //    POProduct mPOProduct = POParameter.PoDto.ExternalProduct.FirstOrDefault();

                        //    if (mPOProduct.POHeaderId != POParameter.PoDto.Id)
                        //    {
                        //        using (var connection = new SqlConnection(connectionString))
                        //        {
                        //            await connection.ExecuteAsync("UPDATE dbo.POProduct SET [IsUsed] = @IsUsed where UsedPoId =@Id", new { Id = POParameter.PoDto.Id, IsUsed = false });
                        //        }
                        //    }

                        //}
                        if (POParameter.PoDto.ExternalProduct != null)
                            foreach (var mPOProduct in POParameter.PoDto.ExternalProduct)
                            {
                                var resourcceId = Guid.NewGuid().ToString();
                                var reparameters = new
                                {
                                    Id = resourcceId,
                                    mPOProduct.HeaderTitle,
                                    mPOProduct.Key,
                                    mPOProduct.Name,
                                    mPOProduct.PbsProductItemType,
                                    mPOProduct.PbsProductItemTypeId,
                                    mPOProduct.PbsProductStatus,
                                    mPOProduct.PbsProductStatusId,
                                    mPOProduct.PbsToleranceState,
                                    mPOProduct.PbsToleranceStateId,
                                    mPOProduct.ProductId,
                                    mPOProduct.Title,
                                    mPOProduct.PComment,
                                    mPOProduct.CComment,
                                    mPOProduct.PQuantity,
                                    mPOProduct.CQuantity,
                                    mPOProduct.Mou,
                                    mPOProduct.PUnitPrice,
                                    mPOProduct.CUnitPrice,
                                    mPOProduct.PTotalPrice,
                                    mPOProduct.CTotalPrice,
                                    mPOProduct.CCrossReference,
                                    mPOProduct.PCrossReference,
                                    mPOProduct.ProjectTitle,
                                    POHeaderId = POParameter.PoDto.Id,
                                    mPOProduct.IsUsed,
                                    mPOProduct.RequestedDeliveryDate,
                                    mPOProduct.ExpectedDeliveryDate,
                                    IsPO = mPOProduct.IsPo
                                };

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(resoursequery, reparameters);
                                    await connection.ExecuteAsync(
                                        "UPDATE dbo.POProduct SET [IsUsed] = @IsUsed,[UsedPoId] = @UsedPoId where [Key] =@Id AND IsPO = 1",
                                        new
                                        {
                                            mPOProduct.Id,
                                            mPOProduct.IsUsed,
                                            UsedPoId = POParameter.PoDto.Id
                                        });
                                }

                                if (mPOProduct.PDocuments != null)
                                    foreach (var mdoc in mPOProduct.PDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(reseourceInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }

                                if (mPOProduct.CDocuments != null)
                                    foreach (var mdoc in mPOProduct.CDocuments)
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            await connection.ExecuteAsync(POResourcesDocumentCInsert,
                                                new
                                                {
                                                    Id = Guid.NewGuid().ToString(), Link = mdoc,
                                                    POResourcesId = resourcceId, POHeaderId = POParameter.PoDto.Id
                                                });
                                        }
                            }

                        if (POParameter.PoDto.ExternalProduct != null)
                            if (POParameter.PoDto.ExternalProduct.FirstOrDefault() == null)
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(
                                        "UPDATE dbo.POProduct SET [IsUsed] = @IsUsed where UsedPoId =@Id",
                                        new { POParameter.PoDto.Id, IsUsed = false });
                                }
                    }

                    if (POParameter.PoDto.IsClone)
                        if (toCu)
                        {
                            connectionString = ConnectionString.MapConnectionString(
                                POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                            var clonetrue = @"UPDATE [dbo].[POHeader] SET IsClone = 1 WHERE POHeader.Id = @Id";
                            var cloneparameter = new { POParameter.PoDto.Id };
                            using (var connection = new SqlConnection(connectionString))
                            {
                                await connection.ExecuteAsync(clonetrue, cloneparameter);
                            }
                        }

                    return POSequenceId;
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        throw new Exception("ID not set");
    }

    public async Task<PODropdownData> GetPbsDropdown(POParameter opParameter, CancellationToken cancellationToken)
    {
        var pbsDropdownData = new PODropdownData();

        try
        {
            var query =
                @"SELECT POType.TypeId AS [key] ,POType.Name AS [Text] FROM dbo.POType WHERE POType.LanguageCode = @lang ORDER BY POType.DisplayOrder; SELECT POStatus.Name AS [Text] ,POStatus.StatusId AS [Key] FROM dbo.POStatus WHERE POStatus.LanguageCode = @lang ORDER BY POStatus.DisplayOrder;SELECT PORequestType.RequestTypeId AS [key] ,PORequestType.Name AS [Text] FROM dbo.PORequestType WHERE PORequestType.LanguageCode = @lang ORDER BY PORequestType.DisplayOrder";
            var parameters = new { lang = opParameter.Lang };
            using (var dbConnection =
                   new SqlConnection(opParameter.TenantProvider.GetTenant().ConnectionString))
            {
                await dbConnection.OpenAsync();
                using (var multi = await dbConnection.QueryMultipleAsync(query, parameters))
                {
                    pbsDropdownData.Types = multi.Read<POTypeDto>().ToList();
                    pbsDropdownData.States = multi.Read<POStatusDto>().ToList();
                    pbsDropdownData.RequestTypes = multi.Read<PORequestTypeDto>().ToList();
                }
            }
        }

        catch (OperationCanceledException e)
        {
            throw e;
        }


        return pbsDropdownData;
    }

    public async Task<POHeaderDto> GetPOById(POParameter POParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
                POParameter.ProjectSequenceId, POParameter.TenantProvider);

            var historyQuery =
                @"SELECT [CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM [dbo].[POHeader] where [SequenceId] =@SequenceId ";

            var parameter = new { SequenceId = POParameter.Id };
            POHistoryDto historyLog;
            using (var connection = new SqlConnection(connectionString))
            {
                historyLog = connection.Query<POHistoryDto>(historyQuery, parameter).FirstOrDefault();
            }

            var query = @"SELECT
                                      POHeader.Title AS Title
                                     ,POHeader.IsCu
                                     ,POHeader.Id
                                     ,POHeader.SequenceId
                                     ,POHeader.Comments
                                     ,POHeader.Name
                                     ,POHeader.ProjectSequenceCode
                                     ,POHeader.LocationId
                                     ,POHeader.CustomerCompanyId AS CustomerOrganisationId
                                     ,POHeader.CustomerId AS CustomerContactId
                                     ,POHeader.SuplierCompanyId AS SupplierOrganisationId
                                     ,POHeader.SupplierCabPersonCompanyId AS SupplierContactId
                                     ,POHeader.SupplierReference AS SupplierReference 
	                                 ,POHeader.CustomerReference AS CustomerReference, POHeader.TotalAmount, POHeader.DeliveryDate
                                     ,POHeader.DeliveryRequest As DeliveryLocation
                                     ,POHeader.TaxonomyId As WarehouseTaxonomyId
                                     ,POHeader.AvailableProbability
                                     ,POHeader.RequestedProbability
                                     ,POHeader.RequestedDate
                                     ,POHeader.ExpectedDate
                                     ,POStatus.StatusId  AS [Key]
                                     ,POStatus.Name AS [Text]
                                     ,POType.TypeId AS [Key]
                                     ,POType.Name AS [Text]
									 ,PORequestType.RequestTypeId AS [Key]
									 ,PORequestType.Name AS [Text]
                                     
                                    FROM dbo.POHeader
                                    LEFT OUTER JOIN dbo.POType
                                      ON POHeader.POTypeId = POType.TypeId
                                    LEFT OUTER JOIN dbo.POStatus
                                      ON POHeader.POStatusId = POStatus.StatusId
									LEFT OUTER JOIN dbo.PORequestType
                                      ON POHeader.PORequestType = PORequestType.RequestTypeId
                                    WHERE POStatus.LanguageCode = @lang
                                    AND POType.LanguageCode = @lang
                                    AND POHeader.SequenceId = @Id ";


            var parameters = new { lang = POParameter.Lang, POParameter.Id };

            POHeaderDto mPOHeaderDto = null;
            using (var connection = new SqlConnection(connectionString))
            {
                mPOHeaderDto = connection.Query<POHeaderDto, POStatusDto, POTypeDto, PORequestTypeDto, POHeaderDto>(
                    query,
                    (pOHeader, pOStatusDto, poTypeDto, poRequestTypeDto) =>
                    {
                        pOHeader.Status = pOStatusDto;
                        pOHeader.Type = poTypeDto;
                        pOHeader.RequestType = poRequestTypeDto;
                        return pOHeader;
                    }, parameters,
                    splitOn: "Key, Key").FirstOrDefault();


                var mPOResourcesDto = new POResourcesDto();
                var pOResourcesAddDtoDictionaryM = new Dictionary<string, POResourcesAddDto>();
                var sqlResorce =
                    @"SELECT POResources.* FROM dbo.POResources WHERE POResources.PurchesOrderId = @PurchesOrderId AND POResources.ResourcesType = @ResourcesType";
                mPOResourcesDto.materials = connection.Query<POResourcesAddDto>
                    (sqlResorce,
                        new
                        {
                            PurchesOrderId = mPOHeaderDto.Id, ResourcesType = "c46c3a26-39a5-42cc-n7k1-89655304eh6"
                        })
                    .ToList();

                var pOResourcesAddDtoDictionaryC = new Dictionary<string, POResourcesAddDto>();

                mPOResourcesDto.consumable = connection.Query<POResourcesAddDto>
                (sqlResorce,
                    new { PurchesOrderId = mPOHeaderDto.Id, ResourcesType = "c46c3a26-39a5-42cc-m06g-89655304eh6" }
                ).ToList();

                mPOResourcesDto.labour = connection.Query<POResourcesAddDto>
                (sqlResorce,
                    new { PurchesOrderId = mPOHeaderDto.Id, ResourcesType = "c46c3a26-39a5-42cc-b07s-89655304eh6" }
                ).ToList();

                mPOResourcesDto.tools = connection.Query<POResourcesAddDto>
                (sqlResorce,
                    new { PurchesOrderId = mPOHeaderDto.Id, ResourcesType = "c46c3a26-39a5-42cc-n9wn-89655304eh6" }
                ).ToList();
                var pResourceLinl = @"SELECT Link FROM POResourcesDocument WHERE POResourcesId = @POResourcesId";
                var cResourceLinl = @"SELECT Link FROM POResourcesDocumentC WHERE POResourcesId = @POResourcesId";
                foreach (var r in mPOResourcesDto.consumable)
                {
                    r.PDocuments = connection.Query<string>(pResourceLinl, new { POResourcesId = r.Id }).ToList();
                    r.CDocuments = connection.Query<string>(cResourceLinl, new { POResourcesId = r.Id }).ToList();
                }

                foreach (var r in mPOResourcesDto.tools)
                {
                    r.PDocuments = connection.Query<string>(pResourceLinl, new { POResourcesId = r.Id }).ToList();
                    r.CDocuments = connection.Query<string>(cResourceLinl, new { POResourcesId = r.Id }).ToList();
                }

                foreach (var r in mPOResourcesDto.materials)
                {
                    r.PDocuments = connection.Query<string>(pResourceLinl, new { POResourcesId = r.Id }).ToList();
                    r.CDocuments = connection.Query<string>(cResourceLinl, new { POResourcesId = r.Id }).ToList();
                }

                foreach (var r in mPOResourcesDto.labour)
                {
                    r.PDocuments = connection.Query<string>(pResourceLinl, new { POResourcesId = r.Id }).ToList();
                    r.CDocuments = connection.Query<string>(cResourceLinl, new { POResourcesId = r.Id }).ToList();
                }

                mPOHeaderDto.POResources = mPOResourcesDto;
                var headerFiles = @"SELECT Link FROM dbo.PODocument WHERE POHeaderId =@Id";

                mPOHeaderDto.Files = connection.Query<string>(headerFiles, new { mPOHeaderDto.Id }).ToList();

                mPOHeaderDto.ExternalProduct = connection
                    .Query<POProduct>("SELECT * FROM dbo.POProduct WHERE POHeaderId =@Id",
                        new { mPOHeaderDto.Id }).ToList();
                //var pResourceLinl = @"SELECT Link FROM POResourcesDocument WHERE POResourcesId = @POResourcesId";
                //var cResourceLinl = @"SELECT Link FROM POResourcesDocumentC WHERE POResourcesId = @POResourcesId";
                foreach (var r in mPOHeaderDto.ExternalProduct)
                {
                    r.PDocuments = connection.Query<string>(pResourceLinl, new { POResourcesId = r.Id }).ToList();
                    r.CDocuments = connection.Query<string>(cResourceLinl, new { POResourcesId = r.Id }).ToList();
                }

                var borResources = "SELECT DISTINCT BorId FROM POResources WHERE PurchesOrderId = @PoId ";

                var borIds = connection.Query<string>(borResources, new { PoId = mPOHeaderDto.Id }).ToList();

                if (mPOHeaderDto.RequestType.key == "94282458-0b40-poa3-b0f9-c2e40344c8f1")
                {
                    var selectConsumable =
                        @"SELECT BorConsumable.Id FROM dbo.BorConsumable WHERE BorConsumable.BorProductId IN @BorProductId";
                    var selectLabour =
                        @"SELECT BorLabour.Id FROM dbo.BorLabour WHERE BorLabour.BorProductId IN @BorProductId";
                    var selectMaterials =
                        @"SELECT BorMaterial.Id FROM dbo.BorMaterial WHERE BorMaterial.BorProductId IN @BorProductId";
                    var selectTools =
                        @"SELECT BorTools.Id FROM dbo.BorTools  WHERE BorTools.BorProductId IN @BorProductId";

                    IEnumerable<PbsResourcesForBorDto> consumableCount;
                    IEnumerable<PbsResourcesForBorDto> materialCount;
                    IEnumerable<PbsResourcesForBorDto> toolsCount;
                    IEnumerable<PbsResourcesForBorDto> labourCount;

                    var parm = new { BorProductId = borIds };

                    consumableCount = connection.Query<PbsResourcesForBorDto>(selectConsumable, parm);
                    materialCount = connection.Query<PbsResourcesForBorDto>(selectMaterials, parm);
                    toolsCount = connection.Query<PbsResourcesForBorDto>(selectTools, parm);
                    labourCount = connection.Query<PbsResourcesForBorDto>(selectLabour, parm);

                    mPOHeaderDto.ConsumableCount = consumableCount.Count().ToString();
                    mPOHeaderDto.MaterialCount = materialCount.Count().ToString();
                    mPOHeaderDto.ToolsCount = toolsCount.Count().ToString();
                    mPOHeaderDto.LabourCount = labourCount.Count().ToString();
                }
            }

            var ModifiedByUserQuery =
                @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

            var GetCustomerQuery =
                @"SELECT CabEmail.EmailAddress AS CustomerEmail ,CabMobilePhone.MobilePhoneNumber AS CustomerMobile ,CabPerson.FullName AS CustomerContact ,CabCompany.VatId ,CabVat.Vat ,CabAddress.* ,Country.* FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id LEFT OUTER JOIN dbo.CabMobilePhone ON CabPersonCompany.MobilePhoneId = CabMobilePhone.Id LEFT OUTER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id INNER JOIN dbo.CabCompany ON CabPersonCompany.CompanyId = CabCompany.Id LEFT OUTER JOIN dbo.CabVat ON CabCompany.VatId = CabVat.Id LEFT OUTER JOIN dbo.CabAddress ON CabCompany.AddressId = CabAddress.Id LEFT OUTER JOIN dbo.Country ON CabAddress.CountryId = Country.Id WHERE CabPersonCompany.Id = @CompanyId";


            using (var connection =
                   new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
            {
                var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
                historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                    .FirstOrDefault();

                var CreatByParam = new { oid = historyLog.CreatedBy };
                historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();

                mPOHeaderDto.CustomerOrganisation = connection
                    .Query<string>("SELECT [Name] FROM dbo.CabCompany WHERE Id=@CompanyId;",
                        new { CompanyId = mPOHeaderDto.CustomerOrganisationId }).FirstOrDefault();

                mPOHeaderDto.SupplierOrganisation = connection
                    .Query<string>("SELECT [Name] FROM dbo.CabCompany WHERE Id=@CompanyId;",
                        new { CompanyId = mPOHeaderDto.SupplierOrganisationId }).FirstOrDefault();
                //GetCustomerQuery, 
                mPOHeaderDto.CustomerContact = connection
                    .Query<POInvolvePartiesDto, CabAddress, Country, POInvolvePartiesDto>(
                        GetCustomerQuery,
                        (POInvolvePartiesDto, CabAddress, Country) =>
                        {
                            POInvolvePartiesDto.CabAddress = CabAddress;
                            POInvolvePartiesDto.CabAddress.Country = Country;

                            return POInvolvePartiesDto;
                        },
                        new { CompanyId = mPOHeaderDto.CustomerContactId }, splitOn: "Vat, Id").FirstOrDefault();
                //SupplierContactId
                mPOHeaderDto.SupplierContact = connection
                    .Query<POInvolvePartiesDto, CabAddress, Country, POInvolvePartiesDto>(
                        GetCustomerQuery,
                        (POInvolvePartiesDto, CabAddress, Country) =>
                        {
                            POInvolvePartiesDto.CabAddress = CabAddress;
                            POInvolvePartiesDto.CabAddress.Country = Country;
                            return POInvolvePartiesDto;
                        },
                        new { CompanyId = mPOHeaderDto.SupplierContactId }, splitOn: "Vat, Id").FirstOrDefault();
            }

            mPOHeaderDto.History = historyLog;
            if (mPOHeaderDto.LocationId != null)
            {
                POParameter.LocationId = mPOHeaderDto.LocationId;
                mPOHeaderDto.MapLocation = await ReadLocation(POParameter);
            }

            // mPOHeaderDto.IsLinkExpired = mPOHeaderDto.Status.Key is "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da" or "4010e768-3e06-4702-b337-ee367a82addb"; //approved or handed over

            mPOHeaderDto.IsLinkExpired = mPOHeaderDto.DeliveryDate?.Date < DateTime.UtcNow.Date;

            if (mPOHeaderDto.RequestType.key == "f4d6ba08-3937-44ca-a0a1-7cf33c03e290")
            {
                await using var dbconnection =
                    new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString);

                var cpcId = POParameter.Configuration.GetValue<string>("RFQCpc");

                var personQuery =
                    @"SELECT  CabPersonCompany.JobRole FROM dbo.CabPersonCompany WHERE  CabPersonCompany.Id = @Id";

                var role = dbconnection.Query<string>(personQuery, new { Id = mPOHeaderDto.SupplierContactId })
                    .FirstOrDefault();
                mPOHeaderDto.SupplierCity = dbconnection
                    .Query<string>(
                        "SELECT CabAddress.City FROM dbo.CabCompany LEFT OUTER JOIN CabAddress  ON CabCompany.AddressId = CabAddress.Id WHERE CabCompany.Id = @Id",
                        new { Id = mPOHeaderDto.SupplierOrganisationId }).FirstOrDefault();

                using (var connection = new SqlConnection(connectionString))
                {
                    var vendorData = connection
                        .Query<CpcVendor>(
                            "SELECT * FROM CpcVendor WHERE CompanyId = @CompanyId AND CoperateProductCatalogId = @cpcId AND IsDeleted = 0",
                            new { CompanyId = mPOHeaderDto.SupplierOrganisationId, cpcId }).FirstOrDefault();

                    mPOHeaderDto.UnitPrice = vendorData?.ResourcePrice.ToString();
                    mPOHeaderDto.ResourceLeadTime = vendorData?.ResourceLeadTime;
                    mPOHeaderDto.CustomerJobRole = role;
                }
            }

            return mPOHeaderDto;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task<string> ProjectSend(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);

        var query = @"UPDATE dbo.POHeader 
                                                    SET
                                                      POStatusId = @POStatusId
                                                     ,ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


        var parameters = new
        {
            POParameter.PoDto.SequenceId,
            ApprovedBy = POParameter.UserId,
            POStatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c",
            ModifiedBy = POParameter.UserId
        };

        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(query, parameters);
        }

        if (POParameter.PoDto.POResources.tools != null)
        {
            var borUpdate =
                @"UPDATE dbo.BorTools SET [RequestedDeliveryDate] =@RequestedDeliveryDate WHERE BorProductId =@BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";
            foreach (var param in POParameter.PoDto.POResources.tools.Select(mPOResourcesTool => new
                     {
                         BorProductId = mPOResourcesTool.BorId,
                         CorporateProductCatalogId = mPOResourcesTool.PCPCId,
                         RequestedDeliveryDate = POParameter.PoDto.DeliveryDate
                     }))
                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(borUpdate, param);
                }
        }

        if (POParameter.PoDto.POResources.consumable != null)
        {
            var borUpdate =
                @"UPDATE dbo.BorConsumable SET [RequestedDeliveryDate] =@RequestedDeliveryDate WHERE BorProductId =@BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";
            foreach (var param in POParameter.PoDto.POResources.consumable.Select(mPOResourcesconsumable => new
                     {
                         BorProductId = mPOResourcesconsumable.BorId,
                         CorporateProductCatalogId = mPOResourcesconsumable.PCPCId,
                         RequestedDeliveryDate = POParameter.PoDto.DeliveryDate
                     }))
                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(borUpdate, param);
                }
        }

        if (POParameter.PoDto.POResources.materials != null)
        {
            var borUpdate =
                @"UPDATE dbo.BorMaterial SET [RequestedDeliveryDate] =@RequestedDeliveryDate WHERE BorProductId =@BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";
            foreach (var param in POParameter.PoDto.POResources.materials.Select(mPOResourcesmaterials => new
                     {
                         BorProductId = mPOResourcesmaterials.BorId,
                         CorporateProductCatalogId = mPOResourcesmaterials.PCPCId,
                         RequestedDeliveryDate = POParameter.PoDto.DeliveryDate
                     }))
                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(borUpdate, param);
                }
        }

        if (POParameter.PoDto.POResources.labour != null)
        {
            var borUpdate =
                @"UPDATE dbo.BorLabour SET [RequestedDeliveryDate] =@RequestedDeliveryDate WHERE BorProductId =@BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";
            foreach (var param in POParameter.PoDto.POResources.labour.Select(mPOResourceslabour => new
                     {
                         BorProductId = mPOResourceslabour.BorId,
                         CorporateProductCatalogId = mPOResourceslabour.PCPCId,
                         RequestedDeliveryDate = POParameter.PoDto.DeliveryDate
                     }))
                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(borUpdate, param);
                }
        }


        //POParameter.ProjectSequenceId = null;
        POParameter.PoDto.POStatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c";
        await CreateHeader(POParameter, true, true);
        return POParameter.PoDto.SequenceId;
    }

    public async Task<string> CUSend(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);

        try
        {
            var jsonProduct = JsonConvert.SerializeObject(POParameter.PoDto, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            await POHistoryLog(POParameter, connectionString, "CuSend_start", jsonProduct);

            var query = @"UPDATE dbo.POHeader 
                                                    SET
                                                      POStatusId = @POStatusId
                                                     ,ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


            var parameters = new
            {
                POParameter.PoDto.SequenceId,
                ApprovedBy = POParameter.UserId,
                POStatusId = "4010e768-3e06-4702-b337-ee367a82addb",
                ModifiedBy = POParameter.UserId
            };


            await CUSendUpdateVendor(POParameter, connectionString);

            await CUSendCreateWorkFlow(POParameter, connectionString, "CUSend");

            await POHistoryLog(POParameter, connectionString, "CuSend", jsonProduct);

            await using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(query, parameters);


            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CUApprove(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);
        var connectionprojectString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.PoDto.ProjectSequenceCode, POParameter.TenantProvider);

        try
        {
            var jsonProduct = JsonConvert.SerializeObject(POParameter.PoDto, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            await POHistoryLog(POParameter, connectionString, "CUApprove_start", jsonProduct);

            var query = @"UPDATE dbo.POHeader 
                                                    SET
                                                      POStatusId = @POStatusId
                                                     ,ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


            var parameters = new
            {
                POParameter.PoDto.SequenceId,
                ApprovedBy = POParameter.UserId,
                POStatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
                ModifiedBy = POParameter.UserId
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, parameters);
            }

            POParameter.ProjectSequenceId = POParameter.PoDto.ProjectSequenceCode;
            POParameter.PoDto.POStatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da";
            await CreateHeader(POParameter, true, false);
            await CreateWorkFlow(POParameter, connectionString, "CUApprove");

            //HistoryLog
            await POHistoryLog(POParameter, connectionString, "CUApprove", jsonProduct);

            using (var connection = new SqlConnection(connectionprojectString))
            {
                connection.Execute(query, parameters);
            }

            if (POParameter.PoDto.POResources.tools != null)
                foreach (var mPOResourcesTool in POParameter.PoDto.POResources.tools)
                {
                    //string borApprove = @"UPDATE dbo.Bor SET BorStatusId = @BorStatusId WHERE Id= @Id";
                    //var param2 = new
                    //{
                    //    Id = mPOResourcesTool.BorId,
                    //    BorStatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da"
                    //};
                    //using (var connection = new SqlConnection(connectionprojectString))
                    //{
                    //    connection.Execute(borApprove, param2);
                    //}
                    if (mPOResourcesTool.CorporateProductCatalogId != null)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";

                        //var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesTool.CorporateProductCatalogId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesTool.CorporateProductCatalogId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionString, "project");
                    }

                    if (mPOResourcesTool.HasChanged)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";


                        // var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesTool.CCPCId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesTool.CCPCId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionprojectString, "cu");


                        var borUpdate =
                            @"UPDATE dbo.BorTools SET Purchased = @Purchased, DeliveryRequested = @DeliveryRequested WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        var paramBor = new
                        {
                            BorProductId = mPOResourcesTool.BorId,
                            CorporateProductCatalogId = mPOResourcesTool.PCPCId,
                            Purchased = 0,
                            DeliveryRequested = 0
                        };

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(borUpdate, paramBor);
                        }

                        var borInsert =
                            @"INSERT INTO dbo.BorTools ( Id ,BorProductId ,Date, Required, Purchased, DeliveryRequested, Warf, Consumed, Invoiced ,CorporateProductCatalogId, ExpectedDeliveryDate) VALUES ( @Id ,@BorProductId ,@Date, @Required, @Purchased, @DeliveryRequested, @Warf, @Consumed, @Invoiced, @CorporateProductCatalogId, @ExpectedDeliveryDate );";

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            await connection.ExecuteAsync(borInsert, new
                            {
                                Id = Guid.NewGuid().ToString(),
                                BorProductId = mPOResourcesTool.BorId,
                                Date = DateTime.UtcNow,
                                Required = 0,
                                Purchased = mPOResourcesTool.CQuantity,
                                DeliveryRequested = mPOResourcesTool.CQuantity,
                                Warf = 0,
                                Consumed = 0,
                                Invoiced = 0,
                                CorporateProductCatalogId = mPOResourcesTool.CCPCId,
                                mPOResourcesTool.ExpectedDeliveryDate
                            });
                        }

                        var pmolQuery =
                            @"SELECT [Id]  FROM [dbo].[PMol] where StatusId = @StatusId AND BorId = @BorId";

                        var parameter4 = new
                        {
                            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                            mPOResourcesTool.BorId
                        };
                        IEnumerable<string> pmolId;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            pmolId = connection.Query<string>(pmolQuery, parameter4);
                        }

                        var pmolUpdate =
                            @"UPDATE dbo.PMolPlannedWorkTools SET CoperateProductCatalogId = @CCPCId WHERE PmolId= @Id AND CoperateProductCatalogId=@CoperateProductCatalogId";
                        foreach (var PMolPlannedWorkTools in pmolId)
                        {
                            var param4 = new
                            {
                                Id = PMolPlannedWorkTools,
                                mPOResourcesTool.CCPCId,
                                CoperateProductCatalogId = mPOResourcesTool.CorporateProductCatalogId
                            };
                            using (var connection = new SqlConnection(connectionprojectString))
                            {
                                connection.Execute(pmolUpdate, param4);
                            }
                        }
                    }
                    else
                    {
                        var expectDate =
                            @"UPDATE dbo.BorTools SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(expectDate, new
                            {
                                BorProductId = mPOResourcesTool.BorId,
                                CorporateProductCatalogId = mPOResourcesTool.CCPCId,
                                mPOResourcesTool.ExpectedDeliveryDate
                            });
                        }
                    }

                    var cpcInsert =
                        @"UPDATE dbo.CorporateProductCatalog SET InventoryPrice = @InventoryPrice WHERE Id= @Id";

                    var param = new
                    {
                        Id = mPOResourcesTool.CCPCId,
                        InventoryPrice = mPOResourcesTool.CUnitPrice
                    };

                    using (var connection = new SqlConnection(connectionprojectString))
                    {
                        connection.Execute(cpcInsert, param);
                    }
                    //using (var connection = new SqlConnection(connectionString))
                    //{
                    //    connection.Execute(cpcInsert, param);
                    //}
                }

            if (POParameter.PoDto.POResources.labour != null)
                foreach (var mPOResourcesLabour in POParameter.PoDto.POResources.labour)
                {
                    //string borApprove = @"UPDATE dbo.Bor SET BorStatusId = @BorStatusId WHERE Id= @Id";
                    //var param2 = new
                    //{
                    //    Id = mPOResourcesLabour.BorId,
                    //    BorStatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da"
                    //};
                    //using (var connection = new SqlConnection(connectionprojectString))
                    //{
                    //    connection.Execute(borApprove, param2);
                    //}
                    if (mPOResourcesLabour.CorporateProductCatalogId != null)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";

                        //var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesLabour.CorporateProductCatalogId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesLabour.CorporateProductCatalogId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionString, "project");
                    }

                    if (mPOResourcesLabour.HasChanged)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";


                        // var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesLabour.CCPCId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesLabour.CCPCId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionprojectString, "cu");

                        var borUpdate =
                            @"UPDATE dbo.BorLabour SET Purchased = @Purchased, DeliveryRequested = @DeliveryRequested WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        var paramBor = new
                        {
                            BorProductId = mPOResourcesLabour.BorId,
                            CorporateProductCatalogId = mPOResourcesLabour.PCPCId,
                            Purchased = 0,
                            DeliveryRequested = 0
                        };

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(borUpdate, paramBor);
                        }

                        var borInsert =
                            @"INSERT INTO dbo.BorLabour ( Id ,BorProductId ,Date, Required, Purchased, DeliveryRequested, Warf, Consumed, Invoiced ,CorporateProductCatalogId, ExpectedDeliveryDate) VALUES ( @Id ,@BorProductId ,@Date, @Required, @Purchased, @DeliveryRequested, @Warf, @Consumed, @Invoiced, @CorporateProductCatalogId, @ExpectedDeliveryDate );";


                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            await connection.ExecuteAsync(borInsert, new
                            {
                                Id = Guid.NewGuid().ToString(),
                                BorProductId = mPOResourcesLabour.BorId,
                                Date = DateTime.UtcNow,
                                Required = 0,
                                Purchased = mPOResourcesLabour.CQuantity,
                                DeliveryRequested = mPOResourcesLabour.CQuantity,
                                Warf = 0,
                                Consumed = 0,
                                Invoiced = 0,
                                CorporateProductCatalogId = mPOResourcesLabour.CCPCId,
                                mPOResourcesLabour.ExpectedDeliveryDate
                            });
                        }

                        var pmolQuery =
                            @"SELECT [Id]  FROM [dbo].[PMol] where StatusId = @StatusId AND BorId = @BorId";

                        var parameter4 = new
                        {
                            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                            mPOResourcesLabour.BorId
                        };
                        IEnumerable<string> pmolId;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            pmolId = connection.Query<string>(pmolQuery, parameter4);
                        }

                        var pmolUpdate =
                            @"UPDATE dbo.PMolPlannedWorkLabour SET CoperateProductCatalogId = @CCPCId WHERE PmolId= @Id AND CoperateProductCatalogId=@CoperateProductCatalogId";
                        foreach (var PMolPlannedWorkLabour in pmolId)
                        {
                            var param4 = new
                            {
                                Id = PMolPlannedWorkLabour,
                                mPOResourcesLabour.CCPCId,
                                CoperateProductCatalogId = mPOResourcesLabour.CorporateProductCatalogId
                            };
                            using (var connection = new SqlConnection(connectionprojectString))
                            {
                                connection.Execute(pmolUpdate, param4);
                            }
                        }
                    }
                    else
                    {
                        var expectDate =
                            @"UPDATE dbo.BorLabour SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(expectDate, new
                            {
                                BorProductId = mPOResourcesLabour.BorId,
                                CorporateProductCatalogId = mPOResourcesLabour.CCPCId,
                                mPOResourcesLabour.ExpectedDeliveryDate
                            });
                        }
                    }

                    var cpcInsert =
                        @"UPDATE dbo.CorporateProductCatalog SET InventoryPrice = @InventoryPrice WHERE Id= @Id";

                    var param = new
                    {
                        Id = mPOResourcesLabour.CCPCId,
                        InventoryPrice = mPOResourcesLabour.CUnitPrice
                    };

                    using (var connection = new SqlConnection(connectionprojectString))
                    {
                        connection.Execute(cpcInsert, param);
                    }
                    //using (var connection = new SqlConnection(connectionString))
                    //{
                    //    connection.Execute(cpcInsert, param);
                    //}
                }

            if (POParameter.PoDto.POResources.consumable != null)
                foreach (var mPOResourcesConsumable in POParameter.PoDto.POResources.consumable)
                {
                    //string borApprove = @"UPDATE dbo.Bor SET BorStatusId = @BorStatusId WHERE Id= @Id";
                    //var param2 = new
                    //{
                    //    Id = mPOResourcesConsumable.BorId,
                    //    BorStatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da"
                    //};
                    //using (var connection = new SqlConnection(connectionprojectString))
                    //{
                    //    connection.Execute(borApprove, param2);
                    //}
                    if (mPOResourcesConsumable.CorporateProductCatalogId != null)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";

                        //var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesConsumable.CorporateProductCatalogId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesConsumable.CorporateProductCatalogId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionString, "project");
                    }

                    if (mPOResourcesConsumable.HasChanged)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";


                        // var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesConsumable.CCPCId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesConsumable.CCPCId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionprojectString, "cu");

                        var borUpdate =
                            @"UPDATE dbo.BorConsumable SET Purchased = @Purchased, DeliveryRequested = @DeliveryRequested  WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        var paramBor = new
                        {
                            BorProductId = mPOResourcesConsumable.BorId,
                            CorporateProductCatalogId = mPOResourcesConsumable.PCPCId,
                            Purchased = 0,
                            DeliveryRequested = 0
                        };

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(borUpdate, paramBor);
                        }

                        var borInsert =
                            @"INSERT INTO dbo.BorConsumable ( Id ,BorProductId ,Date, Required, Purchased, DeliveryRequested, Warf, Consumed, Invoiced ,CorporateProductCatalogId, ExpectedDeliveryDate) VALUES ( @Id ,@BorProductId ,@Date, @Required, @Purchased, @DeliveryRequested, @Warf, @Consumed, @Invoiced, @CorporateProductCatalogId, @ExpectedDeliveryDate );";


                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            await connection.ExecuteAsync(borInsert, new
                            {
                                Id = Guid.NewGuid().ToString(),
                                BorProductId = mPOResourcesConsumable.BorId,
                                Date = DateTime.UtcNow,
                                Required = 0,
                                Purchased = mPOResourcesConsumable.CQuantity,
                                DeliveryRequested = mPOResourcesConsumable.CQuantity,
                                Warf = 0,
                                Consumed = 0,
                                Invoiced = 0,
                                CorporateProductCatalogId = mPOResourcesConsumable.CCPCId,
                                mPOResourcesConsumable.ExpectedDeliveryDate
                            });
                        }

                        var pmolQuery =
                            @"SELECT [Id]  FROM [dbo].[PMol] where StatusId = @StatusId AND BorId = @BorId";

                        var parameter4 = new
                        {
                            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", mPOResourcesConsumable.BorId
                        };
                        IEnumerable<string> pmolId;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            pmolId = connection.Query<string>(pmolQuery, parameter4);
                        }

                        var pmolUpdate =
                            @"UPDATE dbo.PMolPlannedWorkConsumable SET CoperateProductCatalogId = @CCPCId WHERE PmolId= @Id AND CoperateProductCatalogId=@CoperateProductCatalogId";
                        foreach (var PMolPlannedWorkConsumable in pmolId)
                        {
                            var param4 = new
                            {
                                Id = PMolPlannedWorkConsumable,
                                mPOResourcesConsumable.CCPCId,
                                CoperateProductCatalogId = mPOResourcesConsumable.CorporateProductCatalogId
                            };
                            using (var connection = new SqlConnection(connectionprojectString))
                            {
                                connection.Execute(pmolUpdate, param4);
                            }
                        }
                    }
                    else
                    {
                        var expectDate =
                            @"UPDATE dbo.BorConsumable SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(expectDate, new
                            {
                                BorProductId = mPOResourcesConsumable.BorId,
                                CorporateProductCatalogId = mPOResourcesConsumable.CCPCId,
                                mPOResourcesConsumable.ExpectedDeliveryDate
                            });
                        }
                    }


                    var cpcInsert =
                        @"UPDATE dbo.CorporateProductCatalog SET InventoryPrice = @InventoryPrice WHERE Id= @Id";

                    var param = new
                    {
                        Id = mPOResourcesConsumable.CCPCId,
                        InventoryPrice = mPOResourcesConsumable.CUnitPrice
                    };

                    using (var connection = new SqlConnection(connectionprojectString))
                    {
                        connection.Execute(cpcInsert, param);
                    }
                    //using (var connection = new SqlConnection(connectionString))
                    //{
                    //    connection.Execute(cpcInsert, param);
                    //}
                }

            if (POParameter.PoDto.POResources.materials != null)
                foreach (var mPOResourcesMaterials in POParameter.PoDto.POResources.materials)
                {
                    //string borApprove = @"UPDATE dbo.Bor SET BorStatusId = @BorStatusId WHERE Id= @Id";
                    //var param2 = new
                    //{
                    //    Id = mPOResourcesMaterials.BorId,
                    //    BorStatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da"
                    //};
                    //using (var connection = new SqlConnection(connectionprojectString))
                    //{
                    //    connection.Execute(borApprove, param2);
                    //}
                    if (mPOResourcesMaterials.CorporateProductCatalogId != null)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";

                        //var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesMaterials.CorporateProductCatalogId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesMaterials.CorporateProductCatalogId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionString, "project");
                    }

                    if (mPOResourcesMaterials.HasChanged)
                    {
                        var query3 =
                            @"SELECT [ResourceNumber]  FROM [dbo].[CorporateProductCatalog] where Id = @Id";


                        // var connectionString2 = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null, POParameter.TenantProvider);
                        var parameter3 = new { Id = mPOResourcesMaterials.CCPCId };
                        POResourcesAddDto data3;
                        using (var connection = new SqlConnection(connectionString))
                        {
                            data3 = connection.Query<POResourcesAddDto>(query3, parameter3).FirstOrDefault();
                        }

                        var parameters4 = new CpcParameters();
                        parameters4.Lang = POParameter.Lang;
                        parameters4.Id = mPOResourcesMaterials.CCPCId;
                        POParameter.CpcParameters = parameters4;
                        POParameter.Lang = POParameter.Lang;
                        await CopyCpc(POParameter, data3.ResourceNumber, connectionprojectString, "cu");

                        var borUpdate =
                            @"UPDATE dbo.BorMaterial SET Purchased = @Purchased, DeliveryRequested = @DeliveryRequested WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        var paramBor = new
                        {
                            BorProductId = mPOResourcesMaterials.BorId,
                            CorporateProductCatalogId = mPOResourcesMaterials.PCPCId,
                            Purchased = 0,
                            DeliveryRequested = 0
                        };

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(borUpdate, paramBor);
                        }

                        var borInsert =
                            @"INSERT INTO dbo.BorMaterial ( Id ,BorProductId ,Date, Required, Purchased, DeliveryRequested, Warf, Consumed, Invoiced ,CorporateProductCatalogId, ExpectedDeliveryDate) VALUES ( @Id ,@BorProductId ,@Date, @Required, @Purchased, @DeliveryRequested, @Warf, @Consumed, @Invoiced, @CorporateProductCatalogId, @ExpectedDeliveryDate );";


                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            await connection.ExecuteAsync(borInsert, new
                            {
                                Id = Guid.NewGuid().ToString(),
                                BorProductId = mPOResourcesMaterials.BorId,
                                Date = DateTime.UtcNow,
                                Required = 0,
                                Purchased = mPOResourcesMaterials.CQuantity,
                                DeliveryRequested = mPOResourcesMaterials.CQuantity,
                                Warf = 0,
                                Consumed = 0,
                                Invoiced = 0,
                                CorporateProductCatalogId = mPOResourcesMaterials.CCPCId,
                                mPOResourcesMaterials.ExpectedDeliveryDate
                            });
                        }

                        var pmolQuery =
                            @"SELECT [Id]  FROM [dbo].[PMol] where StatusId = @StatusId AND BorId = @BorId";

                        var parameter4 = new
                        {
                            StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477", mPOResourcesMaterials.BorId
                        };
                        IEnumerable<string> pmolId;
                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            pmolId = connection.Query<string>(pmolQuery, parameter4);
                        }

                        var pmolUpdate =
                            @"UPDATE dbo.PMolPlannedWorkMaterial SET CoperateProductCatalogId = @CCPCId WHERE PmolId= @Id AND CoperateProductCatalogId=@CoperateProductCatalogId";
                        foreach (var PMolPlannedWorkMaterials in pmolId)
                        {
                            var param4 = new
                            {
                                Id = PMolPlannedWorkMaterials,
                                mPOResourcesMaterials.CCPCId,
                                CoperateProductCatalogId = mPOResourcesMaterials.CorporateProductCatalogId
                            };
                            using (var connection = new SqlConnection(connectionprojectString))
                            {
                                connection.Execute(pmolUpdate, param4);
                            }
                        }
                    }
                    else
                    {
                        var expectDate =
                            @"UPDATE dbo.BorMaterial SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

                        using (var connection = new SqlConnection(connectionprojectString))
                        {
                            connection.Execute(expectDate, new
                            {
                                BorProductId = mPOResourcesMaterials.BorId,
                                CorporateProductCatalogId = mPOResourcesMaterials.CCPCId,
                                mPOResourcesMaterials.ExpectedDeliveryDate
                            });
                        }
                    }

                    var cpcInsert =
                        @"UPDATE dbo.CorporateProductCatalog SET InventoryPrice = @InventoryPrice WHERE Id= @Id";

                    var param = new
                    {
                        Id = mPOResourcesMaterials.CCPCId,
                        InventoryPrice = mPOResourcesMaterials.CUnitPrice
                    };

                    using (var connection = new SqlConnection(connectionprojectString))
                    {
                        connection.Execute(cpcInsert, param);
                    }
                    //using (var connection = new SqlConnection(connectionString))
                    //{
                    //    connection.Execute(cpcInsert, param);
                    //}
                }

            await UpdateBor(POParameter);

            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CULevelApprove(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);
        var connectionprojectString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.PoDto.ProjectSequenceCode, POParameter.TenantProvider);

        try
        {
            var query = @"UPDATE dbo.POHeader 
                                                    SET
                                                      POStatusId = @POStatusId
                                                     ,ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


            var parameters = new
            {
                POParameter.PoDto.SequenceId,
                ApprovedBy = POParameter.UserId,
                POStatusId = "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da",
                ModifiedBy = POParameter.UserId
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, parameters);
            }

            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task<string> CUFeedback(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);
        var connectionprojectString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.PoDto.ProjectSequenceCode, POParameter.TenantProvider);

        try
        {
            var query = @"UPDATE dbo.POHeader 
                                                    SET
                                                      POStatusId = @POStatusId
                                                     ,ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


            var parameters = new
            {
                POParameter.PoDto.SequenceId,
                ApprovedBy = POParameter.UserId,
                POStatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback",
                ModifiedBy = POParameter.UserId
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, parameters);
            }

            POParameter.ProjectSequenceId = POParameter.PoDto.ProjectSequenceCode;
            POParameter.PoDto.POStatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-feedback";
            await CreateHeader(POParameter, true, false);

            using (var connection = new SqlConnection(connectionprojectString))
            {
                connection.Execute(query, parameters);
            }

            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> PMApprove(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);

        try
        {
            var query = @"UPDATE dbo.POHeader 
                                                    SET
                                                      POStatusId = @POStatusId
                                                     ,ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


            var parameters = new
            {
                POParameter.PoDto.SequenceId,
                ApprovedBy = POParameter.UserId,
                POStatusId = "4010e768-3e06-4702-b337-ee367a82addb",
                ModifiedBy = POParameter.UserId
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, parameters);
            }

            POParameter.ProjectSequenceId = null;
            POParameter.PoDto.POStatusId = "4010e768-3e06-4702-b337-ee367a82addb";
            await CreateHeader(POParameter, true, true);
            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> PMAccept(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);

        try
        {
            var query = @"UPDATE dbo.POHeader 
                                                    SET
                                                      POStatusId = @POStatusId
                                                     ,ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


            var parameters = new
            {
                POParameter.PoDto.SequenceId,
                ApprovedBy = POParameter.UserId,
                POStatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-accept",
                ModifiedBy = POParameter.UserId
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, parameters);
            }

            POParameter.ProjectSequenceId = null;
            POParameter.PoDto.POStatusId = "7143ff01-d173-4a20-8c17-cacdfecdb84c-accept";
            await CreateHeader(POParameter, true, true);
            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateLocation(POParameter POParameter)
    {
        var projectLocation = POParameter.MapLocation;
        var options = new DbContextOptions<ShanukaDbContext>();
        //string connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId, pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var context = new ShanukaDbContext(options, POParameter.TenantProvider.GetTenant().ConnectionString,
            POParameter.TenantProvider);

        if (projectLocation != null)
        {
            //if (projectLocation.Id == null)
            //{
            projectLocation.Id = Guid.NewGuid().ToString();
            //}

            if (projectLocation.Position != null)
            {
                var position = projectLocation.Position;
                position.Id = Guid.NewGuid().ToString();
                context.Position.Add(position);
                context.SaveChanges();
            }

            if (projectLocation.Viewport != null)
            {
                var viewPort = projectLocation.Viewport;
                if (viewPort.TopLeftPoint != null)
                {
                    var tlp = viewPort.TopLeftPoint;
                    tlp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(tlp);
                    context.SaveChanges();
                }

                if (viewPort.BtmRightPoint != null)
                {
                    var brp = viewPort.BtmRightPoint;
                    brp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(brp);
                    context.SaveChanges();
                }

                viewPort.Id = Guid.NewGuid().ToString();
                context.BoundingPoint.Add(viewPort);
                context.SaveChanges();
            }

            if (projectLocation.BoundingBox != null)
            {
                var boundingBox = projectLocation.BoundingBox;
                if (boundingBox.TopLeftPoint != null)
                {
                    var tlp = boundingBox.TopLeftPoint;
                    tlp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(tlp);
                    context.SaveChanges();
                }

                if (boundingBox.BtmRightPoint != null)
                {
                    var brp = boundingBox.BtmRightPoint;
                    brp.Id = Guid.NewGuid().ToString();
                    context.Position.Add(brp);
                    context.SaveChanges();
                }

                boundingBox.Id = Guid.NewGuid().ToString();
                context.BoundingPoint.Add(boundingBox);
                context.SaveChanges();
            }

            if (projectLocation.Address != null)
            {
                var address = projectLocation.Address;
                address.Id = Guid.NewGuid().ToString();
                context.Address.Add(address);
                context.SaveChanges();
            }

            if (projectLocation.DataSources != null)
            {
                var dataSource = projectLocation.DataSources;
                if (dataSource.Geometry != null)
                {
                    var geometry = dataSource.Geometry;
                    //if (geometry.Id == null)
                    //{
                    geometry.Id = Guid.NewGuid().ToString();
                    //}
                    context.Geometry.Add(geometry);
                    context.SaveChanges();
                }

                dataSource.Id = Guid.NewGuid().ToString();
                context.DataSources.Add(dataSource);
                context.SaveChanges();
            }

            context.MapLocation.Add(projectLocation);
            context.SaveChanges();
            return projectLocation.Id;
        }

        return null;
    }

    public async Task<IEnumerable<POBORResources>> POResourceFilter(POParameter POParameter)
    {
        var pOResourcesAddDtoDictionaryR = new Dictionary<string, POResourcesAddDto>();
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);
        var resource = new List<POBORResources>();


        using (var connection = new SqlConnection(connectionString))
        {
            var resourceX = connection.Query<POResourcesAddDto, POHeaderDto, POResourcesAddDto>
            (@"SELECT POResources.*, dbo.CpcResourceTypeLocalizedData.Label AS ResourcesTypeName,WFHeader.StatusId AS WFStatus,POHeader.* FROM dbo.POResources INNER JOIN dbo.POHeader ON POResources.PurchesOrderId = POHeader.Id LEFT OUTER JOIN CpcResourceTypeLocalizedData ON CpcResourceTypeLocalizedData.CpcResourceTypeId = POResources.ResourcesType LEFT OUTER JOIN WFHeader ON WFHeader.Id = POResources.WorkFlowId WHERE POResources.PurchesOrderId = @PurchesOrderId AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND CpcResourceTypeLocalizedData.CpcResourceTypeId != 'c46c3a26-39a5-42cc-b07s-89655304eh6' ",
                (pOResources, pOHeaderDto) =>
                {
                    pOResources.POHeaderDto = pOHeaderDto;
                    return pOResources;
                }, new { PurchesOrderId = POParameter.POResourceFilterDto.POHeaderId, lang = POParameter.Lang },
                splitOn: "ID").GroupBy(g => g.BorId).ToList();

            foreach (var group in resourceX)
            {
                //Console.WriteLine("Users starting with " + group.Key + ":");
                var mPOBORResources = new POBORResources();
                mPOBORResources.BorTitle = group.ToList().First().BorTitle;
                mPOBORResources.BorId = group.ToList().First().BorId;

                mPOBORResources.resources = group.OrderBy(o => o.ResourcesType).ToList();

                foreach (var res in mPOBORResources.resources)
                {
                    var stockAvailability = connection
                        .Query<string>(@"SELECT AvailableQuantity FROM dbo.StockHeader WHERE CPCId = @CPCId;",
                            new { CPCId = res.CCPCId }).FirstOrDefault();

                    if (stockAvailability != null) res.StockAvailability = stockAvailability;
                }

                resource.Add(mPOBORResources);
            }
            //resource = resourceX.GroupBy(re=>re.BorTitle);
        }

        return resource;
    }

    public async Task<IEnumerable<POBORResources>> CUPOResourceFilter(POParameter POParameter)
    {
        var pOResourcesAddDtoDictionaryR = new Dictionary<string, POResourcesAddDto>();
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);
        var resource = new List<POBORResources>();


        using (var connection = new SqlConnection(connectionString))
        {
            var resourceX = connection.Query<POResourcesAddDto, POHeaderDto, POResourcesAddDto>
            (@"SELECT POResources.*, dbo.CpcResourceTypeLocalizedData.Label AS ResourcesTypeName,WFHeader.StatusId AS WFStatus,POHeader.*,POHeader.DeliveryRequest As DeliveryLocation  FROM dbo.POResources INNER JOIN dbo.POHeader ON POResources.PurchesOrderId = POHeader.Id LEFT OUTER JOIN CpcResourceTypeLocalizedData ON CpcResourceTypeLocalizedData.CpcResourceTypeId = POResources.ResourcesType LEFT OUTER JOIN WFHeader ON WFHeader.Id = POResources.WorkFlowId WHERE POResources.PurchesOrderId = @PurchesOrderId AND CpcResourceTypeLocalizedData.LanguageCode = @lang AND CpcResourceTypeLocalizedData.CpcResourceTypeId != 'c46c3a26-39a5-42cc-b07s-89655304eh6'",
                (pOResources, pOHeaderDto) =>
                {
                    pOResources.POHeaderDto = pOHeaderDto;
                    return pOResources;
                }, new { PurchesOrderId = POParameter.POResourceFilterDto.POHeaderId, lang = POParameter.Lang },
                splitOn: "ID").GroupBy(g => g.BorId).ToList();

            foreach (var group in resourceX)
            {
                //Console.WriteLine("Users starting with " + group.Key + ":");
                var mPOBORResources = new POBORResources();
                mPOBORResources.BorTitle = group.ToList().First().BorTitle;
                mPOBORResources.BorId = group.ToList().First().BorId;

                mPOBORResources.resources = group.OrderBy(o => o.ResourcesType).ToList();

                foreach (var res in mPOBORResources.resources)
                {
                    var stockAvailability = connection
                        .Query<string>(@"SELECT AvailableQuantity FROM dbo.StockHeader WHERE CPCId = @CPCId;",
                            new { CPCId = res.CCPCId }).FirstOrDefault();

                    if (stockAvailability != null) res.StockAvailability = stockAvailability;

                    if (res.POHeaderDto.DeliveryLocation != "1")
                    {
                        string wfstatus;

                        using (var proconnection = new SqlConnection(
                                   ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
                                       res.POHeaderDto.ProjectSequenceCode, POParameter.TenantProvider)))
                        {
                            wfstatus = proconnection
                                .Query<string>(
                                    @"SELECT StatusId AS WFStatus FROM dbo.WFHeader WHERE Id = @WorkFlowId ;",
                                    new { res.WorkFlowId }).FirstOrDefault();
                        }

                        res.WFStatus = wfstatus;
                    }
                }

                resource.Add(mPOBORResources);
            }
            //resource = resourceX.GroupBy(re=>re.BorTitle);
        }

        return resource;
    }

    public async Task<MapLocation> ReadLocation(POParameter POParameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            //string connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId, pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
            var context = new ShanukaDbContext(options, POParameter.TenantProvider.GetTenant().ConnectionString,
                POParameter.TenantProvider);


            var MapLocation = context.MapLocation.Where(L => L.Id == POParameter.LocationId).Include(m => m.Address)
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

    public async Task<IEnumerable<POResourceStockDto>> POResourceStockUpdate(POParameter POParameter)
    {
        POHistoryDto username;
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);

        var query = @"UPDATE dbo.POHeader 
                                                    SET                                                     
                                                      ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


        var parameters = new
        {
            POParameter.POResourceStockUpdate.SequenceId,
            ApprovedBy = POParameter.UserId,
            ModifiedBy = POParameter.UserId
        };

        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        using (var connection =
               new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var ModifiedByParameter = new { oid = POParameter.UserId };
            username = connection.Query<POHistoryDto>(ModifiedByUserQuery, ModifiedByParameter).FirstOrDefault();
        }

        using (var connection = new SqlConnection(connectionString))
        {
            foreach (var POResourceStockDto in POParameter.POResourceStockUpdate.POResourceStockDtoList)
            {
                var SelectUprice = @"SELECT * FROM dbo.POResources WHERE POResources.Id = @Id";
                var parm = new { Id = POResourceStockDto.POResourceId };

                POResources uprice;
                uprice = connection.Query<POResources>(SelectUprice, parm).FirstOrDefault();
                string comment;
                WFTask wfTask;
                string WFSequenceId;
                string PoName;
                string date = null;
                IEnumerable<WFTask> taskCount;

                PoName = connection.Query<string>(@"SELECT Name FROM dbo.POHeader WHERE SequenceId =@SequenceId",
                    new { POParameter.POResourceStockUpdate.SequenceId }).FirstOrDefault();
                if (uprice.ExpectedDeliveryDate != POResourceStockDto.ExpectedDeliveryDate &&
                    POResourceStockDto.IsStock)
                    if (uprice.WorkFlowId != null)
                    {
                        taskCount = connection.Query<WFTask>("SELECT * FROM WFTask WHERE WorkFlowId =@WorkFlowId",
                            new { uprice.WorkFlowId });

                        if (taskCount.Count() > 1)
                        {
                            var wfQuery = @"SELECT WFTask.* FROM dbo.WFHeader INNER JOIN dbo.WFTask
                                               ON WFHeader.Id = WFTask.WorkFlowId WHERE WFHeader.StatusId NOT IN('7bcb4e8d-8e8c-487d-8170-6b91c89fc3da','7143ff01-d173-4a20-8c17-cacdfecdb84c','4010e768-3e06-4702-b337-ee367a82addb')
                                               AND WFTask.WorkFlowId = @WorkFlowId AND WFTask.CPCItemId = @CPCItemId";

                            wfTask = connection.Query<WFTask>(wfQuery,
                                new { uprice.WorkFlowId, CPCItemId = uprice.CCPCId }).FirstOrDefault();

                            if (wfTask != null)
                            {
                                connection.Execute("DELETE FROM WFTask WHERE Id =@Id", new { wfTask.Id });

                                var Id = Guid.NewGuid().ToString();
                                var idGenerator = new IdGenerator();
                                WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");
                                var wfquery = @"INSERT INTO dbo.WFHeader
                                            (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Project,BorId,PoId,ExecutedDateAndTime)
                                             VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Project,@BorId,@PoId,@ExecutedDateAndTime)";
                                var wfparameters = new
                                {
                                    Id,
                                    SequenceId = WFSequenceId,
                                    Name = PoName + " " + uprice.BorTitle,
                                    Title = WFSequenceId + " " + PoName + " " + uprice.BorTitle,
                                    TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1", //good picking
                                    ResourceType = uprice.ResourcesType,
                                    CreatedBy = POParameter.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    ModifiedBy = POParameter.UserId,
                                    ModifiedDate = DateTime.UtcNow,
                                    Source = "",
                                    Destination = "",
                                    IsFinish = false,
                                    RequesterId = "",
                                    ExecutorId = "",
                                    RequiredDateAndTime = DateTime.UtcNow,
                                    ExecutedDateAndTime = date,
                                    EffortEstimate = 0,
                                    EffortCompleted = 0,
                                    Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                    Project = POParameter.POResourceStockUpdate.ProjectSequenceId,
                                    uprice.BorId,
                                    PoId = uprice.PurchesOrderId
                                };

                                connection.Execute(wfquery, wfparameters);

                                connection.Execute(@"UPDATE POResources SET WorkFlowId =@WorkFlowId WHERE Id =@Id",
                                    new { WorkFlowId = Id, Id = POResourceStockDto.POResourceId });


                                var taskId = Guid.NewGuid().ToString();
                                var taskparameters = new
                                {
                                    Id = taskId,
                                    WorkFlowId = Id,
                                    wfTask.CPCItemId,
                                    wfTask.Quantity,
                                    wfTask.PickedQuantity,
                                    Mou = wfTask.MOUId,
                                    wfTask.StockAvailability
                                };

                                var taskquery =
                                    @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,StockAvailability) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@StockAvailability)";

                                await connection.ExecuteAsync(taskquery, taskparameters);
                            }
                        }
                    }

                if (uprice.CUnitPrice != POResourceStockDto.CUnitPrice)
                {
                    comment = uprice.CComments + "</br></br>" + DateTime.UtcNow + " - " + username.ModifiedBy +
                              "</br>" + "Unit Price: " + uprice.CUnitPrice + " : " + POResourceStockDto.CUnitPrice;

                    var sqlUpdate =
                        @"UPDATE dbo.POResources SET IsStock = @IsStock, ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate, CUnitPrice = @CUnitPrice, CTotalPrice = @CTotalPrice, CComments = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            IsStock = POResourceStockDto.IsStock ? 1 : 0, Id = POResourceStockDto.POResourceId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate,
                            POResourceStockDto.CUnitPrice, CComments = comment,
                            POResourceStockDto.CTotalPrice
                        });
                }
                else
                {
                    comment = uprice.CComments;
                    var sqlUpdate =
                        @"UPDATE dbo.POResources SET IsStock = @IsStock, ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate, CUnitPrice = @CUnitPrice, CTotalPrice = @CTotalPrice, CComments = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            IsStock = POResourceStockDto.IsStock ? 1 : 0, Id = POResourceStockDto.POResourceId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate,
                            POResourceStockDto.CUnitPrice, CComments = comment,
                            POResourceStockDto.CTotalPrice
                        });
                }
            }

            connection.Execute(query, parameters);
        }


        var connectionStringProject = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.POResourceStockUpdate.ProjectSequenceId, POParameter.TenantProvider);

        using (var connection = new SqlConnection(connectionStringProject))
        {
            connection.Execute(query, parameters);
        }

        return POParameter.POResourceStockUpdate.POResourceStockDtoList;
    }

    public async Task<IEnumerable<POResourceStockDto>> CUPOResourceStockUpdate(POParameter POParameter)
    {
        POHistoryDto username;
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);
        var connectionStringProject = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.POResourceStockUpdate.ProjectSequenceId, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);

        var query = @"UPDATE dbo.POHeader 
                                                    SET                                                     
                                                      ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


        var parameters = new
        {
            POParameter.POResourceStockUpdate.SequenceId,
            ApprovedBy = POParameter.UserId,
            ModifiedBy = POParameter.UserId
        };

        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        using (var connection =
               new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var ModifiedByParameter = new { oid = POParameter.UserId };
            username = connection.Query<POHistoryDto>(ModifiedByUserQuery, ModifiedByParameter).FirstOrDefault();
        }


        foreach (var POResourceStockDto in POParameter.POResourceStockUpdate.POResourceStockDtoList)
        {
            POResources uprice;
            string comment;
            WFTask wfTask;
            string WFSequenceId;
            POHeader Po;
            string date = null;
            IEnumerable<WFTask> taskCount;

            using (var connection = new SqlConnection(connectionString))
            {
                var SelectUprice = @"SELECT * FROM dbo.POResources WHERE POResources.Id = @Id";
                var parm = new { Id = POResourceStockDto.POResourceId };


                uprice = connection.Query<POResources>(SelectUprice, parm).FirstOrDefault();
                Po = connection.Query<POHeader>(@"SELECT * FROM dbo.POHeader WHERE SequenceId =@SequenceId",
                    new { POParameter.POResourceStockUpdate.SequenceId }).FirstOrDefault();
            }

            if (Po.DeliveryRequest == "1")
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    if (uprice.ExpectedDeliveryDate != POResourceStockDto.ExpectedDeliveryDate)
                        if (uprice.WorkFlowId != null)
                        {
                            taskCount = connection.Query<WFTask>(
                                "SELECT * FROM WFTask WHERE WorkFlowId =@WorkFlowId",
                                new { uprice.WorkFlowId });

                            if (taskCount.Count() > 1)
                            {
                                var wfQuery = @"SELECT WFTask.* FROM dbo.WFHeader INNER JOIN dbo.WFTask
                                               ON WFHeader.Id = WFTask.WorkFlowId WHERE WFHeader.StatusId NOT IN('7bcb4e8d-8e8c-487d-8170-6b91c89fc3da','7143ff01-d173-4a20-8c17-cacdfecdb84c','4010e768-3e06-4702-b337-ee367a82addb')
                                               AND WFTask.WorkFlowId = @WorkFlowId AND WFTask.CPCItemId = @CPCItemId";

                                wfTask = connection.Query<WFTask>(wfQuery,
                                        new { uprice.WorkFlowId, CPCItemId = uprice.CCPCId })
                                    .FirstOrDefault();

                                if (wfTask != null)
                                {
                                    connection.Execute("DELETE FROM WFTask WHERE Id =@Id", new { wfTask.Id });

                                    var Id = Guid.NewGuid().ToString();
                                    var idGenerator = new IdGenerator();
                                    WFSequenceId =
                                        idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");

                                    var wfquery = @"INSERT INTO dbo.WFHeader
                                            (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Project,BorId,PoId,ExecutedDateAndTime)
                                             VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Project,@BorId,@PoId,@ExecutedDateAndTime)";
                                    var wfparameters = new
                                    {
                                        Id,
                                        SequenceId = WFSequenceId,
                                        Name = Po.Name + " " + uprice.BorTitle,
                                        Title = WFSequenceId + " " + Po.Name + " " + uprice.BorTitle,
                                        TypeId = "4010e768-3e06-4702-b337-ee367a82addb", //good reception
                                        ResourceType = uprice.ResourcesType,
                                        CreatedBy = POParameter.UserId,
                                        CreatedDate = DateTime.UtcNow,
                                        ModifiedBy = POParameter.UserId,
                                        ModifiedDate = DateTime.UtcNow,
                                        Source = "",
                                        Destination = "",
                                        IsFinish = false,
                                        RequesterId = "",
                                        ExecutorId = "",
                                        RequiredDateAndTime = DateTime.UtcNow,
                                        ExecutedDateAndTime = date,
                                        EffortEstimate = 0,
                                        EffortCompleted = 0,
                                        Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                        Project = POParameter.POResourceStockUpdate.ProjectSequenceId,
                                        uprice.BorId,
                                        PoId = uprice.PurchesOrderId
                                    };

                                    connection.Execute(wfquery, wfparameters);

                                    connection.Execute(
                                        @"UPDATE POResources SET WorkFlowId =@WorkFlowId WHERE Id =@Id",
                                        new { WorkFlowId = Id, Id = POResourceStockDto.POResourceId });


                                    var taskId = Guid.NewGuid().ToString();
                                    var taskparameters = new
                                    {
                                        Id = taskId,
                                        WorkFlowId = Id,
                                        wfTask.CPCItemId,
                                        wfTask.Quantity,
                                        wfTask.PickedQuantity,
                                        Mou = wfTask.MOUId,
                                        wfTask.StockAvailability
                                    };

                                    var taskquery =
                                        @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,StockAvailability) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@StockAvailability)";

                                    await connection.ExecuteAsync(taskquery, taskparameters);
                                }
                            }
                        }
                }
            }
            else
            {
                if (uprice.ExpectedDeliveryDate != POResourceStockDto.ExpectedDeliveryDate)
                    if (uprice.WorkFlowId != null)
                    {
                        using (var connection = new SqlConnection(connectionStringProject))
                        {
                            taskCount = connection.Query<WFTask>(
                                "SELECT * FROM WFTask WHERE WorkFlowId =@WorkFlowId",
                                new { uprice.WorkFlowId });
                        }

                        if (taskCount.Count() > 1)
                        {
                            var wfQuery = @"SELECT WFTask.* FROM dbo.WFHeader INNER JOIN dbo.WFTask
                                               ON WFHeader.Id = WFTask.WorkFlowId WHERE WFHeader.StatusId NOT IN('7bcb4e8d-8e8c-487d-8170-6b91c89fc3da','7143ff01-d173-4a20-8c17-cacdfecdb84c','4010e768-3e06-4702-b337-ee367a82addb')
                                               AND WFTask.WorkFlowId = @WorkFlowId AND WFTask.CPCItemId = @CPCItemId";
                            using (var connection = new SqlConnection(connectionStringProject))
                            {
                                wfTask = connection.Query<WFTask>(wfQuery,
                                        new { uprice.WorkFlowId, CPCItemId = uprice.CCPCId })
                                    .FirstOrDefault();
                            }

                            if (wfTask != null)
                            {
                                using (var connection = new SqlConnection(connectionStringProject))
                                {
                                    connection.Execute("DELETE FROM WFTask WHERE Id =@Id", new { wfTask.Id });
                                }

                                var Id = Guid.NewGuid().ToString();
                                var idGenerator = new IdGenerator();
                                WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");

                                var wfquery = @"INSERT INTO dbo.WFHeader
                                            (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Project,BorId,PoId,ExecutedDateAndTime)
                                             VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Project,@BorId,@PoId,@ExecutedDateAndTime)";
                                var wfparameters = new
                                {
                                    Id,
                                    SequenceId = WFSequenceId,
                                    Name = Po.Name + " " + uprice.BorTitle,
                                    Title = WFSequenceId + " " + Po.Name + " " + uprice.BorTitle,
                                    TypeId = "4010e768-3e06-4702-b337-ee367a82addb", //good reception
                                    ResourceType = uprice.ResourcesType,
                                    CreatedBy = POParameter.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    ModifiedBy = POParameter.UserId,
                                    ModifiedDate = DateTime.UtcNow,
                                    Source = "",
                                    Destination = "",
                                    IsFinish = false,
                                    RequesterId = "",
                                    ExecutorId = "",
                                    RequiredDateAndTime = DateTime.UtcNow,
                                    ExecutedDateAndTime = date,
                                    EffortEstimate = 0,
                                    EffortCompleted = 0,
                                    Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                    Project = POParameter.POResourceStockUpdate.ProjectSequenceId,
                                    uprice.BorId,
                                    PoId = uprice.PurchesOrderId
                                };

                                using (var connection = new SqlConnection(connectionStringProject))
                                {
                                    connection.Execute(wfquery, wfparameters);
                                }

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(
                                        @"UPDATE POResources SET WorkFlowId =@WorkFlowId WHERE Id =@Id",
                                        new { WorkFlowId = Id, Id = POResourceStockDto.POResourceId });
                                }

                                var taskId = Guid.NewGuid().ToString();
                                var taskparameters = new
                                {
                                    Id = taskId,
                                    WorkFlowId = Id,
                                    wfTask.CPCItemId,
                                    wfTask.Quantity,
                                    wfTask.PickedQuantity,
                                    Mou = wfTask.MOUId,
                                    wfTask.StockAvailability
                                };

                                var taskquery =
                                    @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,StockAvailability) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@StockAvailability)";

                                using (var connection = new SqlConnection(connectionStringProject))
                                {
                                    await connection.ExecuteAsync(taskquery, taskparameters);
                                }
                            }
                        }
                    }
            }


            using (var connection = new SqlConnection(connectionString))
            {
                if (uprice.CUnitPrice != POResourceStockDto.CUnitPrice)
                {
                    comment = uprice.CComments + "</br></br>" + DateTime.UtcNow + " - " + username.ModifiedBy +
                              "</br>" + "Unit Price: " + uprice.CUnitPrice + " : " + POResourceStockDto.CUnitPrice;

                    var sqlUpdate =
                        @"UPDATE dbo.POResources SET ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate, CUnitPrice = @CUnitPrice, CComments = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            Id = POResourceStockDto.POResourceId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate,
                            POResourceStockDto.CUnitPrice, CComments = comment
                        });
                }
                else
                {
                    comment = uprice.CComments;
                    var sqlUpdate =
                        @"UPDATE dbo.POResources SET ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate, CUnitPrice = @CUnitPrice, CComments = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            Id = POResourceStockDto.POResourceId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate,
                            POResourceStockDto.CUnitPrice, CComments = comment
                        });
                }

                connection.Execute(query, parameters);
            }
        }


        using (var connection = new SqlConnection(connectionStringProject))
        {
            connection.Execute(query, parameters);
        }

        return POParameter.POResourceStockUpdate.POResourceStockDtoList;
    }

    public async Task<IEnumerable<POProductIsPoDto>> POProductIsPoUpdate(POParameter POParameter)
    {
        POHistoryDto username;
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);
        var connectionStringProject = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.POProductIsPoUpdate.ProjectSequenceId, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);

        var query = @"UPDATE dbo.POHeader 
                                                    SET                                                    
                                                      ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


        var parameters = new
        {
            POParameter.POProductIsPoUpdate.SequenceId,
            ApprovedBy = POParameter.UserId,
            ModifiedBy = POParameter.UserId
        };

        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        using (var connection =
               new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var ModifiedByParameter = new { oid = POParameter.UserId };
            username = connection.Query<POHistoryDto>(ModifiedByUserQuery, ModifiedByParameter).FirstOrDefault();
        }

        using (var connection = new SqlConnection(connectionString))
        {
            foreach (var POResourceStockDto in POParameter.POProductIsPoUpdate.POProductIsPoDtoList)
            {
                var SelectUprice = @"SELECT * FROM dbo.POProduct WHERE POProduct.Id = @Id";
                var parm = new { Id = POResourceStockDto.POProductId };

                POProduct uprice;
                uprice = connection.Query<POProduct>(SelectUprice, parm).FirstOrDefault();
                string comment;

                if (uprice.CTotalPrice != POResourceStockDto.CTotalPrice)
                {
                    comment = uprice.CComment + "</br></br>" + DateTime.UtcNow + " - " + username.ModifiedBy +
                              "</br>" + "Total Price: " + uprice.CTotalPrice + " : " +
                              POResourceStockDto.CTotalPrice;

                    var sqlUpdate =
                        @"UPDATE dbo.POProduct SET IsPo = @IsPo, ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate, CTotalPrice = @CTotalPrice , CComment = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            IsPo = POResourceStockDto.IsPo ? 1 : 0, Id = POResourceStockDto.POProductId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate, CComments = comment,
                            POResourceStockDto.CTotalPrice
                        });
                }
                else
                {
                    comment = uprice.CComment;
                    var sqlUpdate =
                        @"UPDATE dbo.POProduct SET IsPo = @IsPo, ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate,CTotalPrice = @CTotalPrice,  CComment = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            IsPo = POResourceStockDto.IsPo ? 1 : 0, Id = POResourceStockDto.POProductId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate, CComments = comment,
                            POResourceStockDto.CTotalPrice
                        });
                }
            }

            connection.Execute(query, parameters);
        }


        using (var connection = new SqlConnection(connectionStringProject))
        {
            connection.Execute(query, parameters);
        }

        return POParameter.POProductIsPoUpdate.POProductIsPoDtoList;
    }

    public async Task<IEnumerable<POProductIsPoDto>> CUPOProductIsPoUpdate(POParameter POParameter)
    {
        POHistoryDto username;
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.ProjectSequenceId, POParameter.TenantProvider);
        var connectionStringProject = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.POProductIsPoUpdate.ProjectSequenceId, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);

        var query = @"UPDATE dbo.POHeader 
                                                    SET                                                    
                                                      ModifiedBy = @ModifiedBy
                                                     ,ModifiedDate = GETDATE()
                                                     ,ApprovedBy = @ApprovedBy
                                                    WHERE
                                                      SequenceId = @SequenceId
                                                    ;";


        var parameters = new
        {
            POParameter.POProductIsPoUpdate.SequenceId,
            ApprovedBy = POParameter.UserId,
            ModifiedBy = POParameter.UserId
        };

        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        using (var connection =
               new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var ModifiedByParameter = new { oid = POParameter.UserId };
            username = connection.Query<POHistoryDto>(ModifiedByUserQuery, ModifiedByParameter).FirstOrDefault();
        }

        using (var connection = new SqlConnection(connectionString))
        {
            foreach (var POResourceStockDto in POParameter.POProductIsPoUpdate.POProductIsPoDtoList)
            {
                var SelectUprice = @"SELECT * FROM dbo.POProduct WHERE POProduct.Id = @Id";
                var parm = new { Id = POResourceStockDto.POProductId };

                POProduct uprice;
                uprice = connection.Query<POProduct>(SelectUprice, parm).FirstOrDefault();
                string comment;

                if (uprice.CTotalPrice != POResourceStockDto.CTotalPrice)
                {
                    comment = uprice.CComment + "</br></br>" + DateTime.UtcNow + " - " + username.ModifiedBy +
                              "</br>" + "Total Price: " + uprice.CTotalPrice + " : " +
                              POResourceStockDto.CTotalPrice;
                    var sqlUpdate =
                        @"UPDATE dbo.POProduct SET ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate, CTotalPrice = @CTotalPrice , CComment = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            Id = POResourceStockDto.POProductId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate, CComments = comment,
                            POResourceStockDto.CTotalPrice
                        });
                    var sqlUpdateProjectPo =
                        @"UPDATE dbo.POProduct SET ExpectedDeliveryDate = @ExpectedDeliveryDate  WHERE ProductId = @Id";
                    connection.Execute(sqlUpdateProjectPo,
                        new
                        {
                            Id = uprice.ProductId, POResourceStockDto.ExpectedDeliveryDate
                        });
                }
                else
                {
                    comment = uprice.CComment;
                    var sqlUpdate =
                        @"UPDATE dbo.POProduct SET ExpectedDeliveryDate = @ExpectedDeliveryDate, RequestedDeliveryDate = @RequestedDeliveryDate,CTotalPrice = @CTotalPrice,  CComment = @CComments  WHERE Id = @Id";
                    connection.Execute(sqlUpdate,
                        new
                        {
                            Id = POResourceStockDto.POProductId,
                            POResourceStockDto.ExpectedDeliveryDate,
                            POResourceStockDto.RequestedDeliveryDate, CComments = comment,
                            POResourceStockDto.CTotalPrice
                        });
                    var sqlUpdateProjectPo =
                        @"UPDATE dbo.POProduct SET ExpectedDeliveryDate = @ExpectedDeliveryDate  WHERE ProductId = @Id";
                    connection.Execute(sqlUpdateProjectPo,
                        new
                        {
                            Id = uprice.ProductId, POResourceStockDto.ExpectedDeliveryDate
                        });
                }
            }

            connection.Execute(query, parameters);
        }

        using (var connection = new SqlConnection(connectionStringProject))
        {
            connection.Execute(query, parameters);
        }

        return POParameter.POProductIsPoUpdate.POProductIsPoDtoList;
    }

    public async Task<IEnumerable<ProjectDefinitionMobDto>> POProjectList(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);

        var query =
            @"SELECT DISTINCT POHeader.ProjectSequenceCode FROM dbo.POHeader WHERE ProjectSequenceCode IS NOT NULL";


        List<string> projectSequenceList;
        using (var connection = new SqlConnection(connectionString))
        {
            projectSequenceList = connection.Query<string>(query).ToList();
        }

        //string connectionStringProject = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, POParameter.POProductIsPoUpdate.ProjectSequenceId, POParameter.TenantProvider);
        IEnumerable<ProjectDefinitionMobDto> result;
        using (var connection = new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
        {
            result = connection.Query<ProjectDefinitionMobDto>(
                @"SELECT ProjectDefinition.SequenceCode, ProjectDefinition.Title FROM dbo.ProjectDefinition WHERE ProjectDefinition.SequenceCode IN @Ids AND IsDeleted = 0",
                new { Ids = projectSequenceList });
        }


        return result;
    }

    public async Task<IEnumerable<POResourcesExcelDto>> POBorResourceFilter(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);
        var pOResourcesAddDtoDictionaryR = new Dictionary<string, POResourcesExcelDto>();
        var sql =
            @"SELECT POResources.*,CpcResourceTypeLocalizedData.Label AS ResourceType,CpcResourceTypeLocalizedData.CpcResourceTypeId AS ResourceTypeId ,POResources.PTitle AS ProjectCPCTitle ,POResources.CTitle AS CTitle, POResources.PTitle AS PTitle, POResourcesDocument.Id AS POResourcesDocumentId ,POResourcesDocument.Link ,POHeader.* FROM dbo.POResources LEFT OUTER JOIN dbo.POResourcesDocument ON POResources.Id = POResourcesDocument.POResourcesId INNER JOIN dbo.POHeader ON POResources.PurchesOrderId = POHeader.Id INNER JOIN dbo.CpcResourceTypeLocalizedData ON POResources.ResourcesType = CpcResourceTypeLocalizedData.CpcResourceTypeId WHERE CpcResourceTypeLocalizedData.LanguageCode = @lang AND POHeader.ProjectSequenceCode = @ProjectSequenceCode AND POResources.IsStock = 0 AND (POResources.IsUsed = 0 OR POResources.IsUsed IS NULL)";
        var sb = new StringBuilder(sql);

        if (POParameter.POBorResourceFilter.BorTitle != null)
        {
            var words = POParameter.POBorResourceFilter.BorTitle.Split(" ");
            foreach (var element in words) sb.Append(" AND POResources.BorTitle LIKE '%" + element + "%'");
        }

        if (POParameter.POBorResourceFilter.ResourceFamily != null)
        {
            var words = POParameter.POBorResourceFilter.ResourceFamily.Split(" ");
            foreach (var element in words) sb.Append(" AND POResources.ResourceFamily LIKE '%" + element + "%'");
        }

        if (POParameter.POBorResourceFilter.PbsTitle != null)
        {
            var words = POParameter.POBorResourceFilter.PbsTitle.Split(" ");
            foreach (var element in words) sb.Append(" AND POResources.PbsTitle LIKE '%" + element + "%'");
        }

        if (POParameter.POBorResourceFilter.PbsTitle != null)
        {
            var words = POParameter.POBorResourceFilter.PbsTitle.Split(" ");
            foreach (var element in words) sb.Append(" AND POResources.PbsTitle LIKE '%" + element + "%'");
        }

        if (POParameter.POBorResourceFilter.ResourceTitle != null)
        {
            var words = POParameter.POBorResourceFilter.ResourceTitle.Split(" ");
            foreach (var element in words) sb.Append(" AND POResources.CTitle LIKE '%" + element + "%'");
        }

        if (POParameter.POBorResourceFilter.ResourceTitle != null)
        {
            var words = POParameter.POBorResourceFilter.ResourceTitle.Split(" ");
            foreach (var element in words) sb.Append(" AND POResources.CTitle LIKE '%" + element + "%'");
        }

        if (POParameter.POBorResourceFilter.ResourceTypeId != null)
            sb.Append(" AND POResources.ResourcesType = '" + POParameter.POBorResourceFilter.ResourceTypeId + "' ");

        if (POParameter.POBorResourceFilter.ProjectSequenceCode != null)
            sb.Append(" AND POHeader.ProjectSequenceCode LIKE '%" +
                      POParameter.POBorResourceFilter.ProjectSequenceCode + "%'");

        if (POParameter.POBorResourceFilter.ProjectSequenceCode != null)
            sb.Append(" AND POHeader.ProjectSequenceCode LIKE '%" +
                      POParameter.POBorResourceFilter.ProjectSequenceCode + "%'");

        if (POParameter.POBorResourceFilter.Sorter.Attribute == null) sb.Append(" ORDER BY POHeader.Title desc");


        if (POParameter.POBorResourceFilter.Sorter.Attribute != null)
        {
            if (POParameter.POBorResourceFilter.Sorter.Attribute.ToLower().Equals("bortitle"))
                sb.Append("ORDER BY POResources.BorTitle " + POParameter.POBorResourceFilter.Sorter.Order);

            if (POParameter.POBorResourceFilter.Sorter.Attribute.ToLower().Equals("pbstitle"))
                sb.Append("ORDER BY POResources.PbsTitle " + POParameter.POBorResourceFilter.Sorter.Order);

            if (POParameter.POBorResourceFilter.Sorter.Attribute.ToLower().Equals("projectcpctitle"))
                sb.Append("ORDER BY POResources.PTitle " + POParameter.POBorResourceFilter.Sorter.Order);

            if (POParameter.POBorResourceFilter.Sorter.Attribute.ToLower().Equals("resourcetype"))
                sb.Append("ORDER BY CpcResourceTypeLocalizedData.Label " +
                          POParameter.POBorResourceFilter.Sorter.Order);

            if (POParameter.POBorResourceFilter.Sorter.Attribute.ToLower().Equals("resourcefamily"))
                sb.Append("ORDER BY POResources.ResourceFamily " + POParameter.POBorResourceFilter.Sorter.Order);
        }


        using (var connection = new SqlConnection(connectionString))
        {
            var resourceX = connection
                .Query<POResourcesExcelDto, POResourcesDocument, POHeaderDto, POResourcesExcelDto>
                (sb.ToString(),
                    (pOResources, pOResourcesDocument, pOHeaderDto) =>
                    {
                        POResourcesExcelDto orderEntry;

                        if (!pOResourcesAddDtoDictionaryR.TryGetValue(pOResources.Id, out orderEntry))
                        {
                            orderEntry = pOResources;
                            orderEntry.PDocuments = new List<string>();
                            pOResourcesAddDtoDictionaryR.Add(orderEntry.Id, orderEntry);
                        }


                        orderEntry.POHeaderDto = pOHeaderDto;

                        if (orderEntry.ResourceTypeId == "c46c3a26-39a5-42cc-b07s-89655304eh6")
                            orderEntry.ResourceName = "Human Resources";
                        else if (orderEntry.ResourceTypeId == "c46c3a26-39a5-42cc-m06g-89655304eh6")
                            orderEntry.ResourceName = "Consumables";
                        else if (orderEntry.ResourceTypeId == "c46c3a26-39a5-42cc-n7k1-89655304eh6")
                            orderEntry.ResourceName = "Materials";
                        else if (orderEntry.ResourceTypeId == "c46c3a26-39a5-42cc-n9wn-89655304eh6")
                            orderEntry.ResourceName = "Tools";

                        orderEntry.PDocuments.Add(pOResourcesDocument.Link);
                        if (orderEntry.PDocuments.Count > 0)
                        {
                            if (orderEntry.PDocuments.First() == null)
                            {
                                orderEntry.PDocuments = null;
                                return orderEntry;
                            }

                            return orderEntry;
                        }

                        orderEntry.PDocuments = null;
                        return orderEntry;
                    },
                    new { lang = POParameter.Lang, ProjectSequenceCode = POParameter.ProjectSequenceId },
                    splitOn: "POResourcesDocumentId,ID");
            return resourceX;
        }
    }

    public async Task<IEnumerable<POProductDto>> POPBSResourceFilter(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);
        var pOResourcesAddDtoDictionaryR = new Dictionary<string, POResourcesExcelDto>();
        var sql =
            @"SELECT POProduct.* ,POHeader.* FROM dbo.POProduct INNER JOIN dbo.POHeader ON POProduct.POHeaderId = POHeader.Id WHERE POHeader.ProjectSequenceCode  = @ProjectSequenceCode AND POProduct.IsPo = 1 AND (POProduct.IsUsed = 0 OR POProduct.IsUsed IS NULL)";
        var sb = new StringBuilder(sql);


        if (POParameter.POPbsResourceFilter.Title != null)
        {
            var words = POParameter.POPbsResourceFilter.Title.Split(" ");
            foreach (var element in words) sb.Append(" AND POProduct.Title LIKE '%" + element + "%'");

            //sb.Append(" AND POProduct.Title LIKE '%" + POParameter.POBorResourceFilter.PbsTitle + "%'");
        }

        if (POParameter.POPbsResourceFilter.PbsProductItemTypeId != null)
            sb.Append("AND POProduct.PbsProductItemTypeId='" +
                      POParameter.POPbsResourceFilter.PbsProductItemTypeId + "' ");

        if (POParameter.POPbsResourceFilter.PbsProductStatusId != null)
            sb.Append("AND POProduct.PbsProductStatusId='" + POParameter.POPbsResourceFilter.PbsProductStatusId +
                      "' ");

        if (POParameter.POPbsResourceFilter.PbsToleranceStateId != null)
            sb.Append("AND POProduct.PbsToleranceStateId='" + POParameter.POPbsResourceFilter.PbsToleranceStateId +
                      "' ");


        if (POParameter.POPbsResourceFilter.Sorter.Attribute == null) sb.Append(" ORDER BY POProduct.Title desc");


        if (POParameter.POPbsResourceFilter.Sorter.Attribute != null)
        {
            if (POParameter.POPbsResourceFilter.Sorter.Attribute.ToLower().Equals("title"))
                sb.Append("ORDER BY POProduct.Title " + POParameter.POPbsResourceFilter.Sorter.Order);

            if (POParameter.POPbsResourceFilter.Sorter.Attribute.ToLower().Equals("pbsproductitemtype"))
                sb.Append("ORDER BY POProduct.PbsProductItemType " + POParameter.POPbsResourceFilter.Sorter.Order);

            if (POParameter.POPbsResourceFilter.Sorter.Attribute.ToLower().Equals("pbsproductstatus"))
                sb.Append("ORDER BY POProduct.PbsProductStatus " + POParameter.POPbsResourceFilter.Sorter.Order);

            if (POParameter.POPbsResourceFilter.Sorter.Attribute.ToLower().Equals("pbstolerancestate"))
                sb.Append("ORDER BY POProduct.PbsToleranceState " + POParameter.POPbsResourceFilter.Sorter.Order);
        }


        using (var connection = new SqlConnection(connectionString))
        {
            var resourceX = connection.Query<POProductDto, POHeader, POProductDto>
            (sb.ToString(),
                (pOResources, pOHeaderDto) =>
                {
                    pOResources.POHeader = pOHeaderDto;
                    return pOResources;
                },
                new
                {
                    lang = POParameter.Lang, POParameter.POPbsResourceFilter.ProjectSequenceCode
                }, splitOn: "POResourcesDocumentId,ID");
            return resourceX;
        }
    }


    public async Task<string> CUSendCreateWorkFlow(POParameter POParameter, string connectionString, string action)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);
        string WFSequenceId;
        string date = null;
        var projectConnectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.PoDto.ProjectSequenceCode, POParameter.TenantProvider);
        try
        {
            var BorQuery =
                @"Select distinct BorId from POResources where PurchesOrderId = (select Id from dbo.POHeader WHERE SequenceId = @Id)";
            var parameter = new { Id = POParameter.PoDto.SequenceId };
            IEnumerable<POResourceForWorkFlow> borData;
            using (var connection = new SqlConnection(connectionString))
            {
                borData = connection.Query<POResourceForWorkFlow>(BorQuery, parameter);
            }

            foreach (var b in borData)
            {
                var ResourceQuery = @"SELECT
                                                      POResources.Id AS POResourcesId
                                                     ,POResources.ResourcesType
                                                     ,POResources.BorTitle
                                                     ,POResources.CComments
                                                     ,POResources.CQuantity
                                                     ,POResources.CTotalPrice
                                                     ,POResources.CCPCId
                                                     ,POResources.CMou
                                                     ,POResources.BorId
                                                     ,POResources.CUnitPrice
                                                     ,POResources.ExpectedDeliveryDate
                                                     ,POHeader.ProjectSequenceCode
													 ,POHeader.DeliveryRequest
												     ,POHeader.Name AS POName
                                                     ,POHeader.Title AS POTitle
                                                    FROM dbo.POResources
                                                    LEFT OUTER JOIN dbo.POHeader
												    ON POResources.PurchesOrderId = POHeader.Id 
                                                    WHERE POResources.BorId = @BorId
                                                    AND POResources.PurchesOrderId = (select Id from dbo.POHeader WHERE SequenceId = @Id)";

                var parameter1 = new { Id = POParameter.PoDto.SequenceId, b.BorId };
                IEnumerable<POResourceForWorkFlow> resoudceData;
                using (var connection = new SqlConnection(connectionString))
                {
                    resoudceData = connection.Query<POResourceForWorkFlow>(ResourceQuery, parameter1);
                }


                var groupresources = resoudceData.GroupBy(r => r.ResourcesType);

                foreach (var r in groupresources)
                    if (r.Key != "c46c3a26-39a5-42cc-b07s-89655304eh6")
                    {
                        var groupdate = r.GroupBy(a => a.ExpectedDeliveryDate);
                        foreach (var a in groupdate)
                        {
                            string dbconnection;
                            //await PoCpcAverage(POParameter);
                            await CpcUnitPriceUpdate(POParameter);
                            if (r.First().DeliveryRequest == "1")
                            {
                                dbconnection = connectionString;

                                var wfId = Guid.NewGuid().ToString();
                                var idGenerator = new IdGenerator();
                                WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");

                                var wfquery = @"INSERT INTO dbo.WFHeader
                                            (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Project,BorId,IsFromCU,ExecutedDateAndTime,PoId)
                                             VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Project,@BorId,@IsFromCU,@ExecutedDateAndTime,@PoId)";

                                var wfparameters = new
                                {
                                    Id = wfId,
                                    SequenceId = WFSequenceId,
                                    // Name = a.First().POName + " " + a.First().BorTitle,
                                    // Title =
                                    //     WFSequenceId + " " + a.First().POName + " " + a.First().BorTitle + a.Key,
                                    Name = a.First().POTitle,
                                    Title =
                                        WFSequenceId + " " + a.First().POTitle,
                                    TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
                                    ResourceType = a.First().ResourcesType,
                                    CreatedBy = POParameter.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    Source = "",
                                    Destination = POParameter.PoDto.WarehouseTaxonomyId,
                                    IsFinish = false,
                                    RequesterId = "",
                                    ExecutorId = "",
                                    RequiredDateAndTime = DateTime.UtcNow,
                                    ExecutedDateAndTime = date,
                                    EffortEstimate = 0,
                                    EffortCompleted = 0,
                                    Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                    Project = a.First().ProjectSequenceCode,
                                    b.BorId,
                                    IsFromCU = true,
                                    PoId = POParameter.PoDto.Id
                                };

                                using (var connection = new SqlConnection(dbconnection))
                                {
                                    await connection.ExecuteAsync(wfquery, wfparameters);
                                }

                                var jsonProduct = JsonConvert.SerializeObject(wfparameters, Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                await WFHistoryLog(POParameter, connectionString, action, jsonProduct);

                                var selecttaxonomy = @"with name_tree as
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
                                                                  where WHTaxonomyLevelId is not null";

                                var parametertaxonomy = new { Id = POParameter.PoDto.WarehouseTaxonomyId };
                                IEnumerable<WHTaxonomyListDto> taxonomyData;
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    taxonomyData =
                                        connection.Query<WHTaxonomyListDto>(selecttaxonomy, parametertaxonomy);
                                }


                                foreach (var item in a)
                                {
                                    var resourceupdate =
                                        @"UPDATE dbo.POResources SET WorkFlowId = @WorkFlowId WHERE POResources.Id = @POResourcesId";

                                    var resourcesparm = new { item.POResourcesId, WorkFlowId = wfId };

                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        await connection.ExecuteAsync(resourceupdate, resourcesparm);
                                    }

                                    var stockquery = @"SELECT * FROM dbo.StockHeader WHERE CPCId = @CPCId";

                                    var parameterstock = new { CPCId = item.CCPCId };
                                    IEnumerable<StockListDto> stockData;
                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        stockData = connection.Query<StockListDto>(stockquery, parameterstock);
                                    }

                                    string destination = null;
                                    foreach (var Stock in stockData)
                                    {
                                        var selectstocktaxonomy = @"with name_tree as
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
                                                                  where WHTaxonomyLevelId is not null";

                                        var parameterstocktaxonomy = new { Id = Stock.WareHouseTaxonomyId };
                                        IEnumerable<WHTaxonomyListDto> stocktaxonomyData;
                                        using (var connection = new SqlConnection(connectionString))
                                        {
                                            stocktaxonomyData =
                                                connection.Query<WHTaxonomyListDto>(selectstocktaxonomy,
                                                    parameterstocktaxonomy);
                                        }

                                        var list1 = new List<string>();
                                        var list2 = new List<string>();

                                        foreach (var s in stocktaxonomyData) list1.Add(s.Title);

                                        foreach (var t in taxonomyData) list2.Add(t.Title);

                                        foreach (var c in taxonomyData.Select((value, index) => new { value, index }))
                                            if (list1[list1.Count - 1 - c.index] ==
                                                list2[list2.Count - 1 - c.index])
                                            {
                                                destination = Stock.WareHouseTaxonomyId;
                                            }
                                            else
                                            {
                                                destination = null;
                                                break;
                                            }
                                    }

                                    var taskId = Guid.NewGuid().ToString();
                                    var taskparameters = new
                                    {
                                        Id = taskId,
                                        WorkFlowId = wfId,
                                        CPCItemId = item.CCPCId,
                                        Quantity = item.CQuantity,
                                        PickedQuantity = 0,
                                        Mou = item.CMou,
                                        StockAvailability = 0,
                                        UnitPrice = item.CUnitPrice,
                                        Destination = destination
                                    };

                                    var taskquery =
                                        @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,Destination,StockAvailability,UnitPrice) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@Destination,@StockAvailability,@UnitPrice)";


                                    using (var connection = new SqlConnection(dbconnection))
                                    {
                                        await connection.ExecuteAsync(taskquery, taskparameters);
                                    }
                                }
                            }
                            else
                            {
                                dbconnection = ConnectionString.MapConnectionString(
                                    POParameter.ContractingUnitSequenceId, POParameter.PoDto.ProjectSequenceCode,
                                    POParameter.TenantProvider);

                                var wfId = Guid.NewGuid().ToString();
                                var idGenerator = new IdGenerator();
                                WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");

                                var wfquery = @"INSERT INTO dbo.WFHeader
                                                (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Project,BorId,IsFromCU,ExecutedDateAndTime,PoId)
                                                 VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Project,@BorId,@IsFromCU,@ExecutedDateAndTime,@PoId)";
                                var wfparameters = new
                                {
                                    Id = wfId,
                                    SequenceId = WFSequenceId,
                                    // Name = a.First().POName + " " + a.First().BorTitle,
                                    // Title =
                                    //     WFSequenceId + " " + a.First().POName + " " + a.First().BorTitle + a.Key,
                                    Name = a.First().POTitle,
                                    Title =
                                        WFSequenceId + " " + a.First().POTitle,
                                    TypeId = "4010e768-3e06-4702-b337-ee367a82addb",
                                    ResourceType = a.First().ResourcesType,
                                    CreatedBy = POParameter.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    ModifiedBy = POParameter.UserId,
                                    ModifiedDate = DateTime.UtcNow,
                                    Source = "",
                                    Destination = POParameter.PoDto.WarehouseTaxonomyId,
                                    IsFinish = false,
                                    RequesterId = "",
                                    ExecutorId = "",
                                    RequiredDateAndTime = DateTime.UtcNow,
                                    ExecutedDateAndTime = date,
                                    EffortEstimate = 0,
                                    EffortCompleted = 0,
                                    Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                    Project = a.First().ProjectSequenceCode,
                                    b.BorId,
                                    IsFromCU = true,
                                    PoId = POParameter.PoDto.Id
                                };

                                using (var connection = new SqlConnection(dbconnection))
                                {
                                    await connection.ExecuteAsync(wfquery, wfparameters);
                                }

                                var jsonProduct = JsonConvert.SerializeObject(wfparameters, Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                    });

                                await WFHistoryLog(POParameter, connectionString, action, jsonProduct);

                                var selecttaxonomy = @"with name_tree as
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
                                                                  where WHTaxonomyLevelId is not null";

                                var parametertaxonomy = new { Id = POParameter.PoDto.WarehouseTaxonomyId };
                                IEnumerable<WHTaxonomyListDto> taxonomyData;
                                using (var connection = new SqlConnection(dbconnection))
                                {
                                    taxonomyData =
                                        connection.Query<WHTaxonomyListDto>(selecttaxonomy, parametertaxonomy);
                                }

                                var list2 = new List<string>();
                                foreach (var t in taxonomyData) list2.Add(t.Title);

                                foreach (var item in a)
                                {
                                    var resourceupdate =
                                        @"UPDATE dbo.POResources SET WorkFlowId = @WorkFlowId WHERE POResources.Id = @POResourcesId";

                                    var resourcesparm = new { item.POResourcesId, WorkFlowId = wfId };

                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        await connection.ExecuteAsync(resourceupdate, resourcesparm);
                                    }

                                    if (POParameter.PoDto.WarehouseTaxonomyId != null)
                                    {
                                        var stockquery = @"SELECT * FROM dbo.StockHeader WHERE CPCId = @CPCId";

                                        var parameterstock = new { CPCId = item.CCPCId };
                                        IEnumerable<StockListDto> stockData;
                                        using (var connection = new SqlConnection(dbconnection))
                                        {
                                            stockData = connection.Query<StockListDto>(stockquery, parameterstock);
                                        }

                                        var isset = false;
                                        string destination = null;
                                        foreach (var Stock in stockData)
                                        {
                                            var selectstocktaxonomy = @"with name_tree as
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
                                                                  where WHTaxonomyLevelId is not null";

                                            var parameterstocktaxonomy = new { Id = Stock.WareHouseTaxonomyId };
                                            IEnumerable<WHTaxonomyListDto> stocktaxonomyData;
                                            using (var connection = new SqlConnection(dbconnection))
                                            {
                                                stocktaxonomyData =
                                                    connection.Query<WHTaxonomyListDto>(selectstocktaxonomy,
                                                        parameterstocktaxonomy);
                                            }

                                            var list1 = new List<string>();


                                            foreach (var s in stocktaxonomyData) list1.Add(s.Title);


                                            foreach (var c in taxonomyData.Select((value, index) =>
                                                         new { value, index }))
                                            {
                                                if (isset) break;

                                                if (list1[list1.Count - 1 - c.index] ==
                                                    list2[list2.Count - 1 - c.index])
                                                {
                                                    destination = Stock.WareHouseTaxonomyId;
                                                    if (list2.Count == c.index) isset = true;
                                                }
                                                else
                                                {
                                                    destination = null;
                                                    break;
                                                }
                                            }
                                        }

                                        var taskId = Guid.NewGuid().ToString();
                                        var taskparameters = new
                                        {
                                            Id = taskId,
                                            WorkFlowId = wfId,
                                            CPCItemId = item.CCPCId,
                                            Quantity = item.CQuantity,
                                            PickedQuantity = 0,
                                            Mou = item.CMou,
                                            StockAvailability = 0,
                                            UnitPrice = item.CUnitPrice,
                                            Destination = destination
                                        };

                                        var taskquery =
                                            @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,Destination,StockAvailability,UnitPrice) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@Destination,@StockAvailability,@UnitPrice)";


                                        using (var connection = new SqlConnection(dbconnection))
                                        {
                                            await connection.ExecuteAsync(taskquery, taskparameters);
                                        }
                                    }
                                    else
                                    {
                                        var taskId = Guid.NewGuid().ToString();
                                        var taskparameters = new
                                        {
                                            Id = taskId,
                                            WorkFlowId = wfId,
                                            CPCItemId = item.CCPCId,
                                            Quantity = item.CQuantity,
                                            PickedQuantity = 0,
                                            Mou = item.CMou,
                                            StockAvailability = 0,
                                            UnitPrice = item.CUnitPrice
                                        };

                                        var taskquery =
                                            @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,StockAvailability,UnitPrice) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@StockAvailability,@UnitPrice)";


                                        using (var connection = new SqlConnection(dbconnection))
                                        {
                                            await connection.ExecuteAsync(taskquery, taskparameters);
                                        }
                                    }
                                }


                                var insterTPmol =
                                    @"INSERT INTO dbo.PMol ( Id ,ProjectMoleculeId,Name ,IsDeleted ,TypeId ,StatusId ,Title ,BorId ,LocationId ,IsFinished  ) VALUES ( @Id ,@ProjectMoleculeId ,@Name ,0 ,@TypeId ,@StatusId ,@Title ,@BorId ,@LocationId ,0 );";

                                ProjectDefinition projectDefinition;
                                using (var connection =
                                       new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
                                {
                                    projectDefinition = connection
                                        .Query<ProjectDefinition>(
                                            "SELECT * FROM dbo.ProjectDefinition WHERE SequenceCode = @SequenceCode",
                                            new { SequenceCode = POParameter.PoDto.ProjectSequenceCode })
                                        .FirstOrDefault();
                                }

                                using (var connection = new SqlConnection(projectConnectionString))
                                {
                                    var ProjectMoleculeId1 = idGenerator.GenerateId(applicationDbContext, "PMOL-",
                                        "PmolSequenceCode");
                                    var ProjectMoleculeId2 = idGenerator.GenerateId(applicationDbContext, "PMOL-",
                                        "PmolSequenceCode");
                                    var ProjectMoleculeId3 = idGenerator.GenerateId(applicationDbContext, "PMOL-",
                                        "PmolSequenceCode");

                                    var Id = Guid.NewGuid().ToString();
                                    var Id2 = Guid.NewGuid().ToString();
                                    var IdTravel = Guid.NewGuid().ToString();
                                    var bor = connection.Query<Bor>("SELECT * FROM dbo.Bor WHERE Id = @Id",
                                        new { Id = b.BorId }).FirstOrDefault();
                                    var paramPmol = new
                                    {
                                        Id,
                                        ProjectMoleculeId = ProjectMoleculeId1,
                                        BorId = bor.Id,
                                        TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
                                        StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                        Name = a.First().POName + " " + a.First().BorTitle + a.Key,
                                        Title = ProjectMoleculeId1 + " - " + WFSequenceId + " " + r.First().POName +
                                                " " + r.First().BorTitle,
                                        projectDefinition.LocationId
                                    };

                                    var paramUnload = new
                                    {
                                        Id = Id2,
                                        ProjectMoleculeId = ProjectMoleculeId2,
                                        BorId = bor.Id,
                                        TypeId = "848e5e-622d-4783-95e6-4092004eb5eaff",
                                        StatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                        Name = a.First().POName + " " + a.First().BorTitle + a.Key,
                                        Title = ProjectMoleculeId2 + " - " + WFSequenceId + " " + r.First().POName +
                                                " " + r.First().BorTitle,
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
                                        Name = a.First().POName + " " + a.First().BorTitle + a.Key,
                                        Title = ProjectMoleculeId3 + " - " + WFSequenceId + " " + r.First().POName +
                                                " " + r.First().BorTitle,
                                        projectDefinition
                                            .LocationId
                                    };
                                    await connection.ExecuteAsync(insterTPmol, paramPmol);
                                    await connection.ExecuteAsync(insterTPmol, paramUnload);
                                    await connection.ExecuteAsync(insterTPmol, paramPmolTravel);
                                    if (r.Key != null)
                                    {
                                        if (r.Key == "c46c3a26-39a5-42cc-n7k1-89655304eh6")
                                        {
                                            //sb.Append("  AND POHeader.NoOfMaterials > 0 ");
                                            var pmolResource =
                                                @"INSERT INTO dbo.PMolPlannedWorkMaterial ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity ) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0, 0.0)";

                                            foreach (var item in a)
                                                if (item.CCPCId != null)
                                                {
                                                    var paramR = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
                                                    };
                                                    await connection.ExecuteAsync(pmolResource, paramR);

                                                    var paramR2 = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id2,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
                                                    };
                                                    await connection.ExecuteAsync(pmolResource, paramR2);
                                                }
                                        }

                                        if (r.Key == "c46c3a26-39a5-42cc-m06g-89655304eh6")
                                        {
                                            // sb.Append("  AND POHeader.NoOfConsumables > 0 ");
                                            var pmolResource =
                                                @"INSERT INTO dbo.PMolPlannedWorkConsumable ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0, 0.0 )";

                                            foreach (var item in a)
                                                if (item.CCPCId != null)
                                                {
                                                    var paramR = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
                                                    };
                                                    await connection.ExecuteAsync(pmolResource, paramR);

                                                    var paramR2 = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id2,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
                                                    };
                                                    await connection.ExecuteAsync(pmolResource, paramR2);
                                                }
                                        }

                                        if (r.Key == "c46c3a26-39a5-42cc-b07s-89655304eh6")
                                        {
                                            // sb.Append("  AND POHeader.NoOfLabours > 0 ");
                                            var pmolResource =
                                                @"INSERT INTO dbo.PMolPlannedWorkLabour ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity ) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0, 0.0 )";

                                            foreach (var item in a)
                                                if (item.CCPCId != null)
                                                {
                                                    var paramR = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
                                                    };
                                                    await connection.ExecuteAsync(pmolResource, paramR);

                                                    var paramR2 = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id2,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
                                                    };
                                                    await connection.ExecuteAsync(pmolResource, paramR2);
                                                }
                                        }

                                        if (r.Key == "c46c3a26-39a5-42cc-n9wn-89655304eh6")
                                        {
                                            var pmolResource =
                                                @"INSERT INTO dbo.PMolPlannedWorkTools ( Id ,CoperateProductCatalogId ,RequiredQuantity ,CpcBasicUnitofMeasureId ,PmolId ,Type ,IsDeleted, ConsumedQuantity) VALUES ( NEWID() ,@CoperateProductCatalogId ,@RequiredQuantity ,@CpcBasicUnitofMeasureId ,@PmolId ,@Type,0,0.0 )";

                                            foreach (var item in a)
                                                if (item.CCPCId != null)
                                                {
                                                    var paramR = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
                                                    };
                                                    await connection.ExecuteAsync(pmolResource, paramR);

                                                    var paramR2 = new
                                                    {
                                                        CoperateProductCatalogId = item.CCPCId,
                                                        RequiredQuantity = item.CQuantity, PmolId = Id2,
                                                        Type = "Planned", CpcBasicUnitofMeasureId = item.CMou
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
            }


            return POParameter.PoDto.SequenceId;
        }

        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CUSendUpdateVendor(POParameter POParameter, string connectionString)
    {
        try
        {
            if (POParameter.PoDto.SupplierOrganisationId != null)
            {
                CpcVendor vendorList;
                var vendorquery =
                    @"SELECT * FROM CpcVendor WHERE CompanyId = @CompanyId AND CoperateProductCatalogId = @CoperateProductCatalogId";
                var poResourceUpdate =
                    @"UPDATE POResources SET CCrossReference = @CCrossReference WHERE PurchesOrderId = @PurchesOrderId AND CCPCId = @CCPCId";
                var cpcQuery = @"SELECT * FROM CorporateProductCatalog WHERE Id = @Id";
                CorporateProductCatalogDto cpc;
                string companyName = null;
                using (var connection = new SqlConnection(POParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    companyName = connection.Query<string>(@"SELECT Name FROM CabCompany WHERE Id = @Id",
                        new { Id = POParameter.PoDto.SupplierOrganisationId }).FirstOrDefault();
                }

                if (POParameter.PoDto.POResources.tools != null)
                    foreach (var mPOResourcesTool in POParameter.PoDto.POResources.tools)
                    {
                        var param = new
                        {
                            CompanyId = POParameter.PoDto.SupplierOrganisationId,
                            CoperateProductCatalogId = mPOResourcesTool.CCPCId
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            vendorList = connection.Query<CpcVendor>(vendorquery, param).FirstOrDefault();
                            cpc = connection
                                .Query<CorporateProductCatalogDto>(cpcQuery, new { Id = mPOResourcesTool.CCPCId })
                                .FirstOrDefault();
                        }

                        if (vendorList != null)
                        {
                            if (!mPOResourcesTool.CUnitPrice.Equals(vendorList.ResourcePrice))
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(
                                        @"UPDATE CpcVendor SET ResourcePrice = @ResourcePrice WHERE Id = @Id",
                                        new { vendorList.Id, ResourcePrice = mPOResourcesTool.CUnitPrice });
                                    connection.Execute(poResourceUpdate,
                                        new
                                        {
                                            PurchesOrderId = POParameter.PoDto.Id,
                                            mPOResourcesTool.CCPCId,
                                            CCrossReference = companyName + "_" + mPOResourcesTool.CTitle
                                        });
                                }
                        }
                        else
                        {
                            var vendorInsert =
                                @"INSERT INTO CpcVendor (Id, CompanyId, ResourceNumber, ResourceTitle, ResourcePrice, PurchasingUnit, CoperateProductCatalogId, PreferredParty) VALUES (@Id, @CompanyId, @ResourceNumber, @ResourceTitle, @ResourcePrice, @PurchasingUnit, @CoperateProductCatalogId, @PreferredParty)";

                            var parameters = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                CompanyId = POParameter.PoDto.SupplierOrganisationId,
                                ResourceNumber = companyName + "_" + mPOResourcesTool.CTitle,
                                ResourceTitle = mPOResourcesTool.CTitle,
                                ResourcePrice = mPOResourcesTool.CUnitPrice,
                                PurchasingUnit = mPOResourcesTool.CMou,
                                CoperateProductCatalogId = mPOResourcesTool.CCPCId,
                                PreferredParty = false
                            };

                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Execute(vendorInsert, parameters);
                                connection.Execute(poResourceUpdate,
                                    new
                                    {
                                        PurchesOrderId = POParameter.PoDto.Id,
                                        mPOResourcesTool.CCPCId,
                                        CCrossReference = companyName + "_" + mPOResourcesTool.CTitle
                                    });
                            }
                        }
                    }

                if (POParameter.PoDto.POResources.consumable != null)
                    foreach (var mPOResourcesconsumable in POParameter.PoDto.POResources.consumable)
                    {
                        var param = new
                        {
                            CompanyId = POParameter.PoDto.SupplierOrganisationId,
                            CoperateProductCatalogId = mPOResourcesconsumable.CCPCId
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            vendorList = connection.Query<CpcVendor>(vendorquery, param).FirstOrDefault();
                            cpc = connection
                                .Query<CorporateProductCatalogDto>(cpcQuery,
                                    new { Id = mPOResourcesconsumable.CCPCId }).FirstOrDefault();
                        }

                        if (vendorList != null)
                        {
                            if (!mPOResourcesconsumable.CUnitPrice.Equals(vendorList.ResourcePrice))
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(
                                        @"UPDATE CpcVendor SET ResourcePrice = @ResourcePrice WHERE Id = @Id",
                                        new
                                        {
                                            vendorList.Id, ResourcePrice = mPOResourcesconsumable.CUnitPrice
                                        });
                                    connection.Execute(poResourceUpdate,
                                        new
                                        {
                                            PurchesOrderId = POParameter.PoDto.Id,
                                            mPOResourcesconsumable.CCPCId,
                                            CCrossReference = companyName + "_" + mPOResourcesconsumable.CTitle
                                        });
                                }
                        }
                        else
                        {
                            var vendorInsert =
                                @"INSERT INTO CpcVendor (Id, CompanyId, ResourceNumber, ResourceTitle, ResourcePrice, PurchasingUnit, CoperateProductCatalogId, PreferredParty) VALUES (@Id, @CompanyId, @ResourceNumber, @ResourceTitle, @ResourcePrice, @PurchasingUnit, @CoperateProductCatalogId, @PreferredParty)";

                            var parameters = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                CompanyId = POParameter.PoDto.SupplierOrganisationId,
                                ResourceNumber = companyName + "_" + mPOResourcesconsumable.CTitle,
                                ResourceTitle = mPOResourcesconsumable.CTitle,
                                ResourcePrice = mPOResourcesconsumable.CUnitPrice,
                                PurchasingUnit = mPOResourcesconsumable.CMou,
                                CoperateProductCatalogId = mPOResourcesconsumable.CCPCId,
                                PreferredParty = false
                            };

                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Execute(vendorInsert, parameters);
                                connection.Execute(poResourceUpdate,
                                    new
                                    {
                                        PurchesOrderId = POParameter.PoDto.Id,
                                        mPOResourcesconsumable.CCPCId,
                                        CCrossReference = companyName + "_" + mPOResourcesconsumable.CTitle
                                    });
                            }
                        }
                    }

                if (POParameter.PoDto.POResources.materials != null)
                    foreach (var mPOResourcesmaterials in POParameter.PoDto.POResources.materials)
                    {
                        var param = new
                        {
                            CompanyId = POParameter.PoDto.SupplierOrganisationId,
                            CoperateProductCatalogId = mPOResourcesmaterials.CCPCId
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            vendorList = connection.Query<CpcVendor>(vendorquery, param).FirstOrDefault();
                            cpc = connection
                                .Query<CorporateProductCatalogDto>(cpcQuery,
                                    new { Id = mPOResourcesmaterials.CCPCId }).FirstOrDefault();
                        }

                        if (vendorList != null)
                        {
                            if (!mPOResourcesmaterials.CUnitPrice.Equals(vendorList.ResourcePrice))
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(
                                        @"UPDATE CpcVendor SET ResourcePrice = @ResourcePrice WHERE Id = @Id",
                                        new { vendorList.Id, ResourcePrice = mPOResourcesmaterials.CUnitPrice });
                                    connection.Execute(poResourceUpdate,
                                        new
                                        {
                                            PurchesOrderId = POParameter.PoDto.Id,
                                            mPOResourcesmaterials.CCPCId,
                                            CCrossReference = companyName + "_" + mPOResourcesmaterials.CTitle
                                        });
                                }
                        }
                        else
                        {
                            var vendorInsert =
                                @"INSERT INTO CpcVendor (Id, CompanyId, ResourceNumber, ResourceTitle, ResourcePrice, PurchasingUnit, CoperateProductCatalogId, PreferredParty) VALUES (@Id, @CompanyId, @ResourceNumber, @ResourceTitle, @ResourcePrice, @PurchasingUnit, @CoperateProductCatalogId, @PreferredParty)";

                            var parameters = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                CompanyId = POParameter.PoDto.SupplierOrganisationId,
                                ResourceNumber = companyName + "_" + mPOResourcesmaterials.CTitle,
                                ResourceTitle = mPOResourcesmaterials.CTitle,
                                ResourcePrice = mPOResourcesmaterials.CUnitPrice,
                                PurchasingUnit = mPOResourcesmaterials.CMou,
                                CoperateProductCatalogId = mPOResourcesmaterials.CCPCId,
                                PreferredParty = false
                            };

                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Execute(vendorInsert, parameters);
                                connection.Execute(poResourceUpdate,
                                    new
                                    {
                                        PurchesOrderId = POParameter.PoDto.Id,
                                        mPOResourcesmaterials.CCPCId,
                                        CCrossReference = companyName + "_" + mPOResourcesmaterials.CTitle
                                    });
                            }
                        }
                    }

                if (POParameter.PoDto.POResources.labour != null)
                    foreach (var mPOResourceslabour in POParameter.PoDto.POResources.labour)
                    {
                        var param = new
                        {
                            CompanyId = POParameter.PoDto.SupplierOrganisationId,
                            CoperateProductCatalogId = mPOResourceslabour.CCPCId
                        };

                        using (var connection = new SqlConnection(connectionString))
                        {
                            vendorList = connection.Query<CpcVendor>(vendorquery, param).FirstOrDefault();
                            cpc = connection
                                .Query<CorporateProductCatalogDto>(cpcQuery, new { Id = mPOResourceslabour.CCPCId })
                                .FirstOrDefault();
                        }

                        if (vendorList != null)
                        {
                            if (!mPOResourceslabour.CUnitPrice.Equals(vendorList.ResourcePrice))
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    connection.Execute(
                                        @"UPDATE CpcVendor SET ResourcePrice = @ResourcePrice WHERE Id = @Id",
                                        new { vendorList.Id, ResourcePrice = mPOResourceslabour.CUnitPrice });
                                    connection.Execute(poResourceUpdate,
                                        new
                                        {
                                            PurchesOrderId = POParameter.PoDto.Id,
                                            mPOResourceslabour.CCPCId,
                                            CCrossReference = companyName + "_" + mPOResourceslabour.CTitle
                                        });
                                }
                        }
                        else
                        {
                            var vendorInsert =
                                @"INSERT INTO CpcVendor (Id, CompanyId, ResourceNumber, ResourceTitle, ResourcePrice, PurchasingUnit, CoperateProductCatalogId, PreferredParty) VALUES (@Id, @CompanyId, @ResourceNumber, @ResourceTitle, @ResourcePrice, @PurchasingUnit, @CoperateProductCatalogId, @PreferredParty)";

                            var parameters = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                CompanyId = POParameter.PoDto.SupplierOrganisationId,
                                ResourceNumber = companyName + "_" + mPOResourceslabour.CTitle,
                                ResourceTitle = mPOResourceslabour.CTitle,
                                ResourcePrice = mPOResourceslabour.CUnitPrice,
                                PurchasingUnit = mPOResourceslabour.CMou,
                                CoperateProductCatalogId = mPOResourceslabour.CCPCId,
                                PreferredParty = false
                            };

                            using (var connection = new SqlConnection(connectionString))
                            {
                                connection.Execute(vendorInsert, parameters);
                                connection.Execute(poResourceUpdate,
                                    new
                                    {
                                        PurchesOrderId = POParameter.PoDto.Id,
                                        mPOResourceslabour.CCPCId,
                                        CCrossReference = companyName + "_" + mPOResourceslabour.CTitle
                                    });
                            }
                        }
                    }
            }

            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<POResourceStockDto>> POBorUpdate(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.POResourceStockUpdate.ProjectSequenceId, POParameter.TenantProvider);

        using (var connection = new SqlConnection(connectionString))
        {
            foreach (var POResourceStockDto in POParameter.POResourceStockUpdate.POResourceStockDtoList)
                if (POResourceStockDto.IsStock)
                {
                    if (POResourceStockDto.ResourceType == "c46c3a26-39a5-42cc-n7k1-89655304eh6") //material
                    {
                        var query =
                            @"UPDATE dbo.BorMaterial SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId = @BorProductId and CorporateProductCatalogId = @CorporateProductCatalogId";
                        var parameter = new
                        {
                            POResourceStockDto.ExpectedDeliveryDate,
                            BorProductId = POResourceStockDto.BorId,
                            CorporateProductCatalogId = POResourceStockDto.CpcId
                        };

                        await connection.ExecuteAsync(query, parameter);
                    }

                    if (POResourceStockDto.ResourceType == "c46c3a26-39a5-42cc-n9wn-89655304eh6") //tool
                    {
                        var query =
                            @"UPDATE dbo.BorTools SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId = @BorProductId and CorporateProductCatalogId = @CorporateProductCatalogId";
                        var parameter = new
                        {
                            POResourceStockDto.ExpectedDeliveryDate,
                            BorProductId = POResourceStockDto.BorId,
                            CorporateProductCatalogId = POResourceStockDto.CpcId
                        };

                        await connection.ExecuteAsync(query, parameter);
                    }

                    if (POResourceStockDto.ResourceType == "c46c3a26-39a5-42cc-m06g-89655304eh6") //consumable
                    {
                        var query =
                            @"UPDATE dbo.BorConsumable SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId = @BorProductId and CorporateProductCatalogId = @CorporateProductCatalogId";
                        var parameter = new
                        {
                            POResourceStockDto.ExpectedDeliveryDate,
                            BorProductId = POResourceStockDto.BorId,
                            CorporateProductCatalogId = POResourceStockDto.CpcId
                        };

                        await connection.ExecuteAsync(query, parameter);
                    }
                }
        }

        return POParameter.POResourceStockUpdate.POResourceStockDtoList;
    }

    public async Task<IEnumerable<POResourceStockDto>> CUPOBorUpdate(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.POResourceStockUpdate.ProjectSequenceId, POParameter.TenantProvider);

        using (var connection = new SqlConnection(connectionString))
        {
            foreach (var POResourceStockDto in POParameter.POResourceStockUpdate.POResourceStockDtoList)
            {
                if (POResourceStockDto.ResourceType == "c46c3a26-39a5-42cc-n7k1-89655304eh6") //material
                {
                    var query =
                        @"UPDATE dbo.BorMaterial SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId = @BorProductId and CorporateProductCatalogId = @CorporateProductCatalogId";
                    var parameter = new
                    {
                        POResourceStockDto.ExpectedDeliveryDate,
                        BorProductId = POResourceStockDto.BorId,
                        CorporateProductCatalogId = POResourceStockDto.CpcId
                    };

                    var parameter2 = new
                    {
                        POResourceStockDto.ExpectedDeliveryDate,
                        BorProductId = POResourceStockDto.BorId,
                        CorporateProductCatalogId = POResourceStockDto.CcpcId
                    };

                    await connection.ExecuteAsync(query, parameter);
                    await connection.ExecuteAsync(query, parameter2);
                }

                if (POResourceStockDto.ResourceType == "c46c3a26-39a5-42cc-n9wn-89655304eh6") //tool
                {
                    var query =
                        @"UPDATE dbo.BorTools SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId = @BorProductId and CorporateProductCatalogId = @CorporateProductCatalogId";
                    var parameter = new
                    {
                        POResourceStockDto.ExpectedDeliveryDate,
                        BorProductId = POResourceStockDto.BorId,
                        CorporateProductCatalogId = POResourceStockDto.CpcId
                    };

                    var parameter2 = new
                    {
                        POResourceStockDto.ExpectedDeliveryDate,
                        BorProductId = POResourceStockDto.BorId,
                        CorporateProductCatalogId = POResourceStockDto.CcpcId
                    };

                    await connection.ExecuteAsync(query, parameter);
                    await connection.ExecuteAsync(query, parameter2);
                }

                if (POResourceStockDto.ResourceType == "c46c3a26-39a5-42cc-m06g-89655304eh6") //consumable
                {
                    var query =
                        @"UPDATE dbo.BorConsumable SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorProductId = @BorProductId and CorporateProductCatalogId = @CorporateProductCatalogId";
                    var parameter = new
                    {
                        POResourceStockDto.ExpectedDeliveryDate,
                        BorProductId = POResourceStockDto.BorId,
                        CorporateProductCatalogId = POResourceStockDto.CpcId
                    };

                    var parameter2 = new
                    {
                        POResourceStockDto.ExpectedDeliveryDate,
                        BorProductId = POResourceStockDto.BorId,
                        CorporateProductCatalogId = POResourceStockDto.CcpcId
                    };

                    await connection.ExecuteAsync(query, parameter);
                    await connection.ExecuteAsync(query, parameter2);
                }

                var poresourceupdate =
                    @"UPDATE dbo.POResources SET ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE POResources.BorId = @BorProductId and POResources.CCPCId = @CorporateProductCatalogId";

                var parameter1 = new
                {
                    POResourceStockDto.ExpectedDeliveryDate,
                    BorProductId = POResourceStockDto.BorId,
                    CorporateProductCatalogId = POResourceStockDto.CcpcId
                };

                using (var cuconnection =
                       new SqlConnection(ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
                           null, POParameter.TenantProvider)))
                {
                    await cuconnection.ExecuteAsync(poresourceupdate, parameter1);
                }

                var updateQuery =
                    @"Update POResources Set ExpectedDeliveryDate = @ExpectedDeliveryDate WHERE BorId = @BorId AND CCPCId = @CCPCId";

                await connection.ExecuteAsync(updateQuery,
                    new
                    {
                        POResourceStockDto.BorId,
                        POResourceStockDto.ExpectedDeliveryDate,
                        CCPCId = POResourceStockDto.CcpcId
                    });
            }
        }

        return POParameter.POResourceStockUpdate.POResourceStockDtoList;
    }

    public async Task<string> CreateWorkFlow(POParameter POParameter, string connectionString, string action)
    {
        //string connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, POParameter.ProjectSequenceId, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);
        string WFSequenceId;
        string date = null;

        try
        {
            var BorQuery =
                @"Select distinct BorId from POResources where PurchesOrderId = (select Id from dbo.POHeader WHERE SequenceId = @Id)";

            var parameter = new { Id = POParameter.PoDto.SequenceId };
            IEnumerable<POResourceForWorkFlow> borData;
            using (var connection = new SqlConnection(connectionString))
            {
                borData = connection.Query<POResourceForWorkFlow>(BorQuery, parameter);
            }

            foreach (var b in borData)
            {
                var ResourceQuery = @"SELECT
                                                  POResources.Id as POResourcesId
                                                 ,POResources.ResourcesType
                                                 ,POResources.BorTitle
                                                 ,POResources.CComments
                                                 ,POResources.CQuantity
                                                 ,POResources.CTotalPrice
                                                 ,POResources.CCPCId
                                                 ,POResources.CMou
                                                 ,POResources.BorId
                                                 ,POResources.IsStock
                                                 ,POHeader.ProjectSequenceCode
												 ,POHeader.Name AS POName
                                                 ,POHeader.Id AS PoId
                                                 ,POHeader.Title AS POTitle
                                                 ,POResources.ExpectedDeliveryDate
                                                FROM dbo.POResources
                                                LEFT OUTER JOIN dbo.POHeader
												ON POResources.PurchesOrderId = POHeader.Id 
                                                WHERE POResources.BorId = @BorId
                                                AND POResources.IsStock = 1
                                                AND POResources.PurchesOrderId = (select Id from dbo.POHeader WHERE SequenceId = @Id)";

                var parameter1 = new { Id = POParameter.PoDto.SequenceId, b.BorId };
                IEnumerable<POResourceForWorkFlow> resoudceData;
                using (var connection = new SqlConnection(connectionString))
                {
                    resoudceData = connection.Query<POResourceForWorkFlow>(ResourceQuery, parameter1);
                }

                var groupresources = resoudceData.GroupBy(r => r.ResourcesType);

                foreach (var r in groupresources)
                    if (r.Key != "c46c3a26-39a5-42cc-b07s-89655304eh6")
                    {
                        var groupdate = r.GroupBy(a => a.ExpectedDeliveryDate);
                        foreach (var a in groupdate)
                        {
                            var Id = Guid.NewGuid().ToString();
                            var idGenerator = new IdGenerator();
                            WFSequenceId = idGenerator.GenerateId(applicationDbContext, "WF-", "WFSequence");

                            var wfquery = @"INSERT INTO dbo.WFHeader
                                            (Id,SequenceId,Name,Title,TypeId,ResourceType,CreatedBy,CreatedDate,Destination,IsFinish,Source,EffortCompleted,EffortEstimate,ExecutorId,RequesterId,RequiredDateAndTime,StatusId,Project,BorId,PoId,ExecutedDateAndTime)
                                             VALUES (@Id,@SequenceId,@Name,@Title,@TypeId,@ResourceType,@CreatedBy,@CreatedDate,@Destination,@IsFinish,@Source,@EffortCompleted,@EffortEstimate,@ExecutorId,@RequesterId,@RequiredDateAndTime,@Status,@Project,@BorId,@PoId,@ExecutedDateAndTime)";
                            var wfparameters = new
                            {
                                Id,
                                SequenceId = WFSequenceId,
                                Name = a.First().POTitle,
                                Title = WFSequenceId + " " + a.First().POTitle,
                                TypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
                                ResourceType = a.First().ResourcesType,
                                CreatedBy = POParameter.UserId,
                                CreatedDate = DateTime.UtcNow,
                                ModifiedBy = POParameter.UserId,
                                ModifiedDate = DateTime.UtcNow,
                                Source = "",
                                Destination = "",
                                IsFinish = false,
                                RequesterId = "",
                                ExecutorId = "",
                                RequiredDateAndTime = DateTime.UtcNow,
                                ExecutedDateAndTime = date,
                                EffortEstimate = 0,
                                EffortCompleted = 0,
                                Status = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                Project = a.First().ProjectSequenceCode,
                                b.BorId,
                                a.First().PoId
                            };

                            using (var connection = new SqlConnection(connectionString))
                            {
                                await connection.ExecuteAsync(wfquery, wfparameters);
                            }

                            var jsonProduct = JsonConvert.SerializeObject(wfparameters, Formatting.Indented,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });

                            await WFHistoryLog(POParameter, connectionString, action, jsonProduct);

                            foreach (var item in a)
                            {
                                var resourceupdate =
                                    @"UPDATE dbo.POResources SET WorkFlowId = @WorkFlowId WHERE POResources.Id = @POResourcesId";

                                var resourcesparm = new { item.POResourcesId, WorkFlowId = Id };

                                using (var connection = new SqlConnection(connectionString))
                                {
                                    await connection.ExecuteAsync(resourceupdate, resourcesparm);
                                }

                                var getStock =
                                    @"Select CPCId,AvailableQuantity,MOUId,AveragePrice,QuantityToBeDelivered,ActiveTypeId from dbo.StockHeader where CPCId = @CPCId ";

                                var stockParam = new { CPCId = item.CCPCId };

                                IEnumerable<StockHeaderDto> stockData;
                                StockHeaderDto selectedStock;
                                string stockMou;
                                string stockQuantity;
                                using (var connection = new SqlConnection(connectionString))
                                {
                                    stockData = connection.Query<StockHeaderDto>(getStock, stockParam);
                                }

                                if (stockData.FirstOrDefault() != null)
                                {
                                    selectedStock = stockData.Where(p =>
                                        p.ActiveTypeId == "94282458-0b40-40a3-b0f9-c2e40344c8f1").FirstOrDefault();

                                    if (selectedStock != null)
                                    {
                                        stockMou = selectedStock.MOUId;
                                        stockQuantity = selectedStock.AvailableQuantity;
                                    }
                                    else
                                    {
                                        stockMou = stockData.FirstOrDefault().MOUId;
                                        stockQuantity = stockData.FirstOrDefault().AvailableQuantity;
                                    }

                                    var taskId = Guid.NewGuid().ToString();
                                    var taskparameters = new
                                    {
                                        Id = taskId,
                                        WorkFlowId = Id,
                                        CPCItemId = item.CCPCId,
                                        Quantity = item.CQuantity,
                                        PickedQuantity = 0,
                                        Mou = stockMou,
                                        StockAvailability = stockQuantity
                                    };

                                    var taskquery =
                                        @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,StockAvailability) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@StockAvailability)";


                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        await connection.ExecuteAsync(taskquery, taskparameters);
                                    }
                                }
                                else
                                {
                                    var taskId = Guid.NewGuid().ToString();
                                    var taskparameters = new
                                    {
                                        Id = taskId,
                                        WorkFlowId = Id,
                                        CPCItemId = item.CCPCId,
                                        Quantity = item.CQuantity,
                                        PickedQuantity = 0,
                                        Mou = "-",
                                        StockAvailability = 0
                                    };

                                    var taskquery =
                                        @"INSERT INTO dbo.WFTask (Id,WorkFlowId,CPCItemId,Quantity,MOUId,PickedQuantity,StockAvailability) VALUES (@Id,@WorkFlowId,@CPCItemId,@Quantity,@Mou,@PickedQuantity,@StockAvailability)";


                                    using (var connection = new SqlConnection(connectionString))
                                    {
                                        await connection.ExecuteAsync(taskquery, taskparameters);
                                    }
                                }
                            }
                        }
                    }
            }

            return POParameter.PoDto.SequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> ConvertToPo(POParameter POParameter)
    {
        var connectionProjectString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.PoDto.ProjectSequenceCode, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);
        string POSequenceId;
        string BorSequenceId;
        string borId;


        try
        {
            var idGenerator = new IdGenerator();
            POSequenceId = idGenerator.GenerateId(applicationDbContext, "PO-", "POSequence");
            BorSequenceId = idGenerator.GenerateId(applicationDbContext, "BOR-", "BorSequenceCode");
            borId = Guid.NewGuid().ToString();
            string date = null;

            var poInsertquery =
                @"INSERT INTO [dbo].[POHeader] ([Id],[Title],[Name],[POTypeId],[POStatusId],[Comments],[ProjectSequenceCode],[IsDeleted],[SequenceId],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[CustomerCompanyId],[CustomerId],[CustomerReference],[SuplierCompanyId],[SupplierCabPersonCompanyId],[SupplierReference], [NoOfMaterials] ,[NoOfTools] ,[NoOfConsumables] ,[NoOfLabours], [DeliveryDate],[LocationId], [TotalAmount], [IsClone] ,[IsCu] ,[DeliveryRequest] ,[TaxonomyId],PORequestType,RequestedDate,ExpectedDate,AvailableProbability,RequestedProbability ) VALUES (@Id ,@Title ,@Name ,@POTypeId ,@POStatusId ,@Comments ,@ProjectSequenceCode ,@IsDeleted ,@SequenceId ,@CreatedBy ,@CreatedDate ,@ModifiedBy ,@ModifiedDate ,@CustomerCompanyId ,@CustomerId ,@CustomerReference ,@SuplierCompanyId ,@SupplierCabPersonCompanyId ,@SupplierReference ,@NoOfMaterials ,@NoOfTools ,@NoOfConsumables ,@NoOfLabours, @DeliveryDate,@LocationId, @TotalAmount, @IsClone , @IsCu ,@DeliveryLocation ,@WarehouseTaxonomyId,@PORequestType,@RequestedDate,@ExpectedDate,@AvailableProbability,@RequestedProbability )";
            var borInsertQuery =
                @"insert into Bor (Id, ItemId, Name, IsDeleted, BorStatusId, Title, BorTypeId, ProjectSequenceCode) values(@Id, @ItemId, @Name, @IsDeleted, @BorStatusId, @Title, @BorTypeId, @ProjectSequenceCode);";
            var updatePo = @"UPDATE POHeader SET POStatusId = @POStatusId WHERE SequenceId = @SequenceId ";

            var parameters = new
            {
                POParameter.PoDto.Id,
                Title = POSequenceId + " " + POParameter.PoDto.Name,
                POParameter.PoDto.Name,
                POTypeId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
                POStatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
                POParameter.PoDto.Comments,
                POParameter.PoDto.ProjectSequenceCode,
                ISDeleted = false,
                SequenceId = POSequenceId,
                CreatedBy = POParameter.UserId,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = POParameter.UserId,
                ModifiedDate = DateTime.UtcNow,
                CustomerCompanyId = POParameter.PoDto.CustomerOrganisationId,
                CustomerId = POParameter.PoDto.CustomerContactId,
                POParameter.PoDto.CustomerReference,
                SuplierCompanyId = POParameter.PoDto.SupplierOrganisationId,
                SupplierCabPersonCompanyId = POParameter.PoDto.SupplierContactId,
                POParameter.PoDto.SupplierReference,
                NoOfMaterials = POParameter.PoDto.POResources.materials.Count,
                NoOfTools = POParameter.PoDto.POResources.tools.Count,
                NoOfConsumables = POParameter.PoDto.POResources.consumable.Count,
                NoOfLabours = POParameter.PoDto.POResources.labour.Count,
                POParameter.PoDto.DeliveryDate,
                POParameter.PoDto.LocationId,
                POParameter.PoDto.TotalAmount,
                POParameter.PoDto.IsClone,
                POParameter.PoDto.IsCu,
                POParameter.PoDto.DeliveryLocation,
                POParameter.PoDto.WarehouseTaxonomyId,
                PORequestType = "94282458-0b40-poa3-b0f9-c2e40344c8f1",
                POParameter.PoDto.RequestedDate,
                ExpectedDate = date,
                POParameter.PoDto.AvailableProbability,
                POParameter.PoDto.RequestedProbability
            };

            var borParameters = new
            {
                Id = borId,
                ItemId = BorSequenceId,
                Title = BorSequenceId + " " + POParameter.PoDto.Name,
                POParameter.PoDto.Name,
                POStatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
                IsDeleted = false,
                BorStatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1",
                BorTypeId = "88282458-0b40-poa3-b0f9-c2e40344c888",
                POParameter.PoDto.ProjectSequenceCode
            };


            using (var connection = new SqlConnection(connectionProjectString))
            {
                await connection.ExecuteAsync(updatePo,
                    new
                    {
                        POStatusId = "4010e768-3e06-4702-b337-ee367a82addb", POParameter.PoDto.SequenceId
                    });
                await connection.ExecuteAsync(poInsertquery, parameters);
                await connection.ExecuteAsync(borInsertQuery, borParameters);
            }

            var reseourceInsertHeader =
                @"INSERT INTO dbo.PODocument(Id ,Link ,POHeaderId ) VALUES (@Id,@Link,@POHeaderId);";
            if (POParameter.PoDto.files != null)
                foreach (var mdoc in POParameter.PoDto.files)
                    using (var connection = new SqlConnection(connectionProjectString))
                    {
                        await connection.ExecuteAsync(reseourceInsertHeader,
                            new { Id = Guid.NewGuid().ToString(), Link = mdoc, POHeaderId = POParameter.PoDto.Id });
                    }


            if (POParameter.PoDto.POTypeId == "94282458-0b40-capa-b0f9-c2e40344c8f1")
            {
                var resoursequery =
                    @"INSERT INTO dbo.POResources ( Id ,PurchesOrderId ,PComments ,PInvoiced ,BorId ,PTotalPrice ,PPurchased ,PStartDate ,PStopDate ,PUnitPrice ,Date ,PFullTimeEmployee ,BorTitle ,PCrossReference ,PConsumed ,CorporateProductCatalogId ,CDeliveryRequested ,PDeliveryRequested ,PNumberOfDate ,PQuantity ,ResourceNumber ,warf ,CComments ,CCrossReference ,CFullTimeEmployee ,CNumberOfDate ,CPurchased ,CQuantity ,CStartDate ,CStopDate ,CTotalPrice ,CUnitPrice ,ResourcesType ,Cdevices ,Pdevices ,ProjectTitle ,PbsTitle ,CTitle ,PTitle ,CMou ,PMou ,CCPCId ,PCPCId, ResourceFamily, BorResourceId, IsUsed, HasChanged, IsStock, RequestedDeliveryDate, ExpectedDeliveryDate,PbsProductId) VALUES ( @Id ,@PurchesOrderId ,@PComments ,@PInvoiced ,@BorId ,@PTotalPrice ,@PPurchased ,@PStartDate ,@PStopDate ,@PUnitPrice ,@Date ,@PFullTimeEmployee ,@BorTitle ,@PCrossReference ,@PConsumed ,@CorporateProductCatalogId ,@CDeliveryRequested ,@PDeliveryRequested ,@PNumberOfDate ,@PQuantity ,@ResourceNumber ,@warf ,@CComments ,@CCrossReference ,@CFullTimeEmployee ,@CNumberOfDate ,@CPurchased ,@CQuantity ,@CStartDate ,@CStopDate ,@CTotalPrice ,@CUnitPrice ,@ResourcesType ,@Cdevices ,@Pdevices ,@ProjectTitle,@PbsTitle,@CTitle,@PTitle,@CMou,@PMou,@CCPCId,@PCPCId, @ResourceFamily, @BorResourceId, @IsUsed, @HasChanged, @IsStock, @RequestedDeliveryDate, @ExpectedDeliveryDate, @PbsProductId );";

                if (POParameter.PoDto.POResources.tools.FirstOrDefault() != null)
                    foreach (var mPOResourcesTool in POParameter.PoDto.POResources.tools)
                    {
                        var borToolsQuery =
                            @"INSERT INTO BorTools (Id, BorProductId, Date, Required, Purchased, DeliveryRequested, Warf, Consumed, Invoiced, CorporateProductCatalogId, TotalRequired, Returned) VALUES (@Id, @BorProductId, @Date, @Required, @Purchased, @DeliveryRequested, @Warf, @Consumed, @Invoiced, @CorporateProductCatalogId, @TotalRequired, @Returned)";
                        var updateBor = @"UPDATE Bor SET PbsProductId = @PbsProductId WHERE Id = @Id";

                        var resourcceId = Guid.NewGuid().ToString();
                        var reparameters = new
                        {
                            Id = resourcceId,
                            PurchesOrderId = POParameter.PoDto.Id,
                            mPOResourcesTool.PComments,
                            mPOResourcesTool.PInvoiced,
                            BorId = borId,
                            mPOResourcesTool.PTotalPrice,
                            mPOResourcesTool.PPurchased,
                            mPOResourcesTool.PStartDate,
                            mPOResourcesTool.PStopDate,
                            mPOResourcesTool.PUnitPrice,
                            mPOResourcesTool.Date,
                            mPOResourcesTool.PFullTimeEmployee,
                            BorTitle = BorSequenceId + " " + POParameter.PoDto.Name,
                            mPOResourcesTool.PCrossReference,
                            mPOResourcesTool.PConsumed,
                            mPOResourcesTool.CorporateProductCatalogId,
                            mPOResourcesTool.CDeliveryRequested,
                            mPOResourcesTool.PDeliveryRequested,
                            mPOResourcesTool.PNumberOfDate,
                            mPOResourcesTool.PQuantity,
                            mPOResourcesTool.ResourceNumber,
                            mPOResourcesTool.warf,
                            mPOResourcesTool.CComments,
                            mPOResourcesTool.CCrossReference,
                            mPOResourcesTool.CFullTimeEmployee,
                            mPOResourcesTool.CNumberOfDate,
                            mPOResourcesTool.CPurchased,
                            mPOResourcesTool.CQuantity,
                            mPOResourcesTool.CStartDate,
                            mPOResourcesTool.CStopDate,
                            mPOResourcesTool.CTotalPrice,
                            mPOResourcesTool.CUnitPrice,
                            ResourcesType = "c46c3a26-39a5-42cc-n9wn-89655304eh6",
                            mPOResourcesTool.Cdevices,
                            mPOResourcesTool.Pdevices,
                            mPOResourcesTool.ProjectTitle,
                            mPOResourcesTool.PbsTitle,
                            mPOResourcesTool.CMou,
                            mPOResourcesTool.PMou,
                            mPOResourcesTool.CCPCId,
                            mPOResourcesTool.PCPCId,
                            mPOResourcesTool.CTitle,
                            mPOResourcesTool.PTitle,
                            mPOResourcesTool.ResourceFamily,
                            mPOResourcesTool.BorResourceId,
                            mPOResourcesTool.IsUsed,
                            mPOResourcesTool.HasChanged,
                            mPOResourcesTool.IsStock,
                            mPOResourcesTool.RequestedDeliveryDate,
                            mPOResourcesTool.ExpectedDeliveryDate,
                            mPOResourcesTool.PbsProductId
                        };


                        var resParam = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            BorProductId = borId,
                            Date = DateTime.UtcNow,
                            Required = mPOResourcesTool.CQuantity,
                            Purchased = 0,
                            DeliveryRequested = 0,
                            Warf = 0,
                            Consumed = 0,
                            Invoiced = 0,
                            CorporateProductCatalogId = mPOResourcesTool.CCPCId,
                            TotalRequired = 0,
                            Returned = 0
                        };

                        using (var connection = new SqlConnection(connectionProjectString))
                        {
                            await connection.ExecuteAsync(resoursequery, reparameters);
                            await connection.ExecuteAsync(borToolsQuery, resParam);
                            await connection.ExecuteAsync(updateBor,
                                new { mPOResourcesTool.PbsProductId, Id = borId });
                        }
                    }

                if (POParameter.PoDto.POResources.labour.FirstOrDefault() != null)
                    foreach (var mPOResourceslabour in POParameter.PoDto.POResources.labour)
                    {
                        var borLabourQuery =
                            @"INSERT INTO BorLabour (Id, BorProductId, Date, Required, Purchased, DeliveryRequested, Warf, Consumed, Invoiced, CorporateProductCatalogId, TotalRequired, Returned) VALUES (@Id, @BorProductId, @Date, @Required, @Purchased, @DeliveryRequested, @Warf, @Consumed, @Invoiced, @CorporateProductCatalogId, @TotalRequired, @Returned)";
                        var updateBor = @"UPDATE Bor SET PbsProductId = @PbsProductId WHERE Id = @Id";

                        var resourcceId = Guid.NewGuid().ToString();
                        var reparameters = new
                        {
                            Id = resourcceId,
                            PurchesOrderId = POParameter.PoDto.Id,
                            mPOResourceslabour.PComments,
                            mPOResourceslabour.PInvoiced,
                            BorId = borId,
                            mPOResourceslabour.PTotalPrice,
                            mPOResourceslabour.PPurchased,
                            mPOResourceslabour.PStartDate,
                            mPOResourceslabour.PStopDate,
                            mPOResourceslabour.PUnitPrice,
                            mPOResourceslabour.Date,
                            mPOResourceslabour.PFullTimeEmployee,
                            BorTitle = BorSequenceId + " " + POParameter.PoDto.Name,
                            mPOResourceslabour.PCrossReference,
                            mPOResourceslabour.PConsumed,
                            mPOResourceslabour.CorporateProductCatalogId,
                            mPOResourceslabour.CDeliveryRequested,
                            mPOResourceslabour.PDeliveryRequested,
                            mPOResourceslabour.PNumberOfDate,
                            mPOResourceslabour.PQuantity,
                            mPOResourceslabour.ResourceNumber,
                            mPOResourceslabour.warf,
                            mPOResourceslabour.CComments,
                            mPOResourceslabour.CCrossReference,
                            mPOResourceslabour.CFullTimeEmployee,
                            mPOResourceslabour.CNumberOfDate,
                            mPOResourceslabour.CPurchased,
                            mPOResourceslabour.CQuantity,
                            mPOResourceslabour.CStartDate,
                            mPOResourceslabour.CStopDate,
                            mPOResourceslabour.CTotalPrice,
                            mPOResourceslabour.CUnitPrice,
                            ResourcesType = "c46c3a26-39a5-42cc-b07s-89655304eh6",
                            mPOResourceslabour.Cdevices,
                            mPOResourceslabour.Pdevices,
                            mPOResourceslabour.ProjectTitle,
                            mPOResourceslabour.PbsTitle,
                            mPOResourceslabour.CMou,
                            mPOResourceslabour.PMou,
                            mPOResourceslabour.CCPCId,
                            mPOResourceslabour.PCPCId,
                            mPOResourceslabour.CTitle,
                            mPOResourceslabour.PTitle,
                            mPOResourceslabour.ResourceFamily,
                            mPOResourceslabour.BorResourceId,
                            mPOResourceslabour.IsUsed,
                            mPOResourceslabour.HasChanged,
                            mPOResourceslabour.IsStock,
                            mPOResourceslabour.RequestedDeliveryDate,
                            mPOResourceslabour.ExpectedDeliveryDate,
                            mPOResourceslabour.PbsProductId
                        };


                        var resParam = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            BorProductId = borId,
                            Date = DateTime.UtcNow,
                            Required = mPOResourceslabour.CQuantity,
                            Purchased = 0,
                            DeliveryRequested = 0,
                            Warf = 0,
                            Consumed = 0,
                            Invoiced = 0,
                            CorporateProductCatalogId = mPOResourceslabour.CCPCId,
                            TotalRequired = 0,
                            Returned = 0
                        };

                        using (var connection = new SqlConnection(connectionProjectString))
                        {
                            await connection.ExecuteAsync(resoursequery, reparameters);
                            await connection.ExecuteAsync(borLabourQuery, resParam);
                            await connection.ExecuteAsync(updateBor,
                                new { mPOResourceslabour.PbsProductId, Id = borId });
                        }
                    }

                //string borCreate = 
            }

            return POSequenceId;
        }

        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> AddPoLabourTeam(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);

        POLabourTeam data;


        try
        {
            var sql = @"SELECT * FROM POLabourTeam WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<POLabourTeam>(sql, new { POParameter.POLabourTeam.Id }).FirstOrDefault();
            }

            if (data == null)
            {
                var insertSql =
                    "INSERT INTO POLabourTeam VALUES (@Id, @POId, @PersonId, @CPCId,@StartDate,@EndDate)";

                var param = new
                {
                    POParameter.POLabourTeam.Id,
                    POParameter.POLabourTeam.POId,
                    POParameter.POLabourTeam.PersonId,
                    POParameter.POLabourTeam.CPCId,
                    POParameter.POLabourTeam.StartDate,
                    POParameter.POLabourTeam.EndDate
                };

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(insertSql, param);
                }
            }

            return POParameter.POLabourTeam.Id;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<GetPOLabourTeam>> GetPoLabourTeam(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);

        IEnumerable<GetPOLabourTeam> data;


        try
        {
            var sql = @"SELECT
                                  POLabourTeam.*
                                 ,POHeader.Title AS POTitle
                                FROM dbo.POLabourTeam
                                LEFT OUTER JOIN dbo.POHeader
                                  ON POLabourTeam.POId = POHeader.Id";

            using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<GetPOLabourTeam>(sql);
            }


            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> AddPoToolPool(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);
        var projectconnectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
            POParameter.POToolPool.Project, POParameter.TenantProvider);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, POParameter.TenantProvider);

        POToolPool data;


        try
        {
            var sql = @"SELECT * FROM POToolPool WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<POToolPool>(sql, new { POParameter.POToolPool.Id }).FirstOrDefault();
            }

            if (data == null)
            {
                var insertSql =
                    "INSERT INTO POToolPool VALUES (@Id, @POId, @WareHouseTaxonomyId, @RequestedCPCId, @ResourceTypeId,@StartDate,@EndDate, @AssignedCPCId)";

                var param = new
                {
                    POParameter.POToolPool.Id,
                    POParameter.POToolPool.POId,
                    POParameter.POToolPool.WareHouseTaxonomyId,
                    POParameter.POToolPool.RequestedCPCId,
                    POParameter.POToolPool.ResourceTypeId,
                    POParameter.POToolPool.StartDate,
                    POParameter.POToolPool.EndDate,
                    POParameter.POToolPool.AssignedCPCId
                };

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(insertSql, param);
                }
            }


            var checkCpcs =
                @"SELECT CorporateProductCatalog.Id FROM dbo.CorporateProductCatalog WHERE CorporateProductCatalog.Id = @Id";

            CorporateProductCatalog checkcpc = null;

            using (var connection = new SqlConnection(projectconnectionString))
            {
                checkcpc = connection
                    .Query<CorporateProductCatalog>(checkCpcs, new { Id = POParameter.POToolPool.AssignedCPCId })
                    .FirstOrDefault();
            }

            if (checkcpc == null)
            {
                var getcpcId =
                    @"SELECT * FROM dbo.CorporateProductCatalog where CorporateProductCatalog.Id = @Id";

                CorporateProductCatalog getcpc = null;

                using (var connection = new SqlConnection(connectionString))
                {
                    getcpc = connection
                        .Query<CorporateProductCatalog>(getcpcId, new { Id = POParameter.POToolPool.AssignedCPCId })
                        .FirstOrDefault();
                }

                var insertcpc =
                    @"INSERT INTO dbo.CorporateProductCatalog ( Id,ResourceTitle,ResourceTypeId,ResourceFamilyId,CpcBasicUnitOfMeasureId,CpcMaterialId,CpcPressureClassId,InventoryPrice,CpcUnitOfSizeMeasureId,Size,WallThickness,MinOrderQuantity,MaxOrderQuantity,Weight,Status,ResourceNumber,IsDeleted,CpcBrandId,Title) VALUES (@Id,@ResourceTitle,@ResourceTypeId,@ResourceFamilyId,@CpcBasicUnitOfMeasureId,@CpcMaterialId,@CpcPressureClassId,@InventoryPrice,@CpcUnitOfSizeMeasureId,@Size,@WallThickness,@MinOrderQuantity,@MaxOrderQuantity,@Weight,@Status,@ResourceNumber,@IsDeleted ,@CpcBrandId,@Title)";

                var parameters3 = new
                {
                    getcpc.Id,
                    getcpc.ResourceTitle,
                    getcpc.ResourceTypeId,
                    getcpc.ResourceFamilyId,
                    getcpc.CpcBasicUnitOfMeasureId,
                    getcpc.CpcMaterialId,
                    getcpc.CpcPressureClassId,
                    getcpc.InventoryPrice,
                    getcpc.CpcUnitOfSizeMeasureId,
                    getcpc.Size,
                    getcpc.WallThickness,
                    getcpc.MinOrderQuantity,
                    getcpc.MaxOrderQuantity,
                    getcpc.Weight,
                    getcpc.Status,
                    getcpc.ResourceNumber,
                    IsDeleted = false,
                    getcpc.CpcBrandId,
                    getcpc.Title
                };

                using (var connection = new SqlConnection(projectconnectionString))
                {
                    await connection.ExecuteAsync(insertcpc, parameters3);
                }
            }

            return POParameter.POToolPool.Id;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> POHistoryLog(POParameter poParameter, string connectionString, string action,
        string historyLog)
    {
        try
        {
            await using var connection = new SqlConnection(connectionString);

            var sql =
                @"INSERT INTO dbo.POHistoryLog ( Id ,HistoryLog ,ChangedByUserId ,Action ,ChangedTime,PoId) VALUES ( @Id ,@HistoryLog ,@ChangedByUserId ,@Action ,@ChangedTime,@PoId);";

            var param = new
            {
                Id = Guid.NewGuid().ToString(),
                HistoryLog = historyLog,
                Action = action,
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = poParameter.UserId,
                PoId = poParameter.PoDto.SequenceId
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

    public async Task<string> WFHistoryLog(POParameter poParameter, string connectionString, string action,
        string historyLog)
    {
        try
        {
            await using var connection = new SqlConnection(connectionString);

            var sql =
                @"INSERT INTO dbo.WFHistoryLogFroPO ( Id ,HistoryLog ,ChangedByUserId ,Action ,ChangedTime,PoId) VALUES ( @Id ,@HistoryLog ,@ChangedByUserId ,@Action ,@ChangedTime,@PoId);";

            var param = new
            {
                Id = Guid.NewGuid().ToString(),
                HistoryLog = historyLog,
                Action = action,
                ChangedTime = DateTime.UtcNow,
                ChangedByUserId = poParameter.UserId,
                PoId = poParameter.PoDto.SequenceId
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

    public DateTime FindGmtDatetime(POParameter pOParameter)
    {
        var timeZone = new TimeZone();
        timeZone.offset = pOParameter.Filter.Offset;
        if (pOParameter.Filter.Date == null)
        {
            timeZone.date = (DateTime)pOParameter.Filter.LastModifiedDate;
        }
        else
        {
            var days = Convert.ToDouble(pOParameter.Filter.Date);
            var d = pOParameter.Filter.LocalDate;
            timeZone.date = d.AddDays(days);
        }

        var finalOffset = FormatOffset(timeZone);
        var date = timeZone.date - timeZone.date.TimeOfDay;
        if (finalOffset > 0)
            date = date.AddHours(finalOffset * -1);
        else if (finalOffset < 0) date = date.AddHours(finalOffset);

        return date;
    }

    private async Task<string> CopyCpc(POParameter poParameter, string resourceNumber, string connectionString,
        string Environment)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        bool isCpcExist;
        var parameter = new CpcParameters();
        parameter.Lang = poParameter.Lang;

        using (var context = new ShanukaDbContext(options, connectionString, poParameter.TenantProvider))
        {
            isCpcExist = context.CorporateProductCatalog.Any(c => c.ResourceNumber == resourceNumber);
        }

        if (isCpcExist == false)
        {
            parameter.Id = resourceNumber;
            if (Environment == "cu") parameter.ContractingUnitSequenceId = poParameter.ContractingUnitSequenceId;

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
            cpcCreateDto.Id = poParameter.CpcParameters.Id;
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
            if (Environment == "cu") parameter.ProjectSequenceId = poParameter.PoDto.ProjectSequenceCode;

            if (Environment == "project") parameter.ProjectSequenceId = null;

            parameter.ContractingUnitSequenceId = poParameter.ContractingUnitSequenceId;

            await _ICoporateProductCatalogRepository.CreateCoporateProductCatalog(parameter,
                poParameter.ContextAccessor);
        }

        return "";
    }


    public async Task<string> UpdateBor(POParameter POParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId,
                POParameter.ProjectSequenceId, POParameter.TenantProvider);
            var queryMaterial =
                @"UPDATE BorMaterial SET BorMaterial.Purchased = BorMaterial.Purchased  + @CQuantity, BorMaterial.DeliveryRequested = BorMaterial.DeliveryRequested  + @CQuantity WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

            var queryConsumable =
                @"UPDATE BorConsumable SET BorConsumable.Purchased = BorConsumable.Purchased  + @CQuantity, BorConsumable.DeliveryRequested = BorConsumable.DeliveryRequested  + @CQuantity WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

            var queryLabour =
                @"UPDATE BorLabour SET BorLabour.Purchased = BorLabour.Purchased  + @CQuantity, BorLabour.DeliveryRequested = BorLabour.DeliveryRequested  + @CQuantity WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";

            var queryTools =
                @"UPDATE BorTools SET BorTools.Purchased = BorTools.Purchased  + @CQuantity, BorTools.DeliveryRequested = BorTools.DeliveryRequested  + @CQuantity WHERE BorProductId  = @BorProductId AND CorporateProductCatalogId =@CorporateProductCatalogId";


            using (var connection = new SqlConnection(connectionString))
            {
                foreach (var Material in POParameter.PoDto.POResources.materials)
                    if (!Material.HasChanged)
                    {
                        var param = new
                        {
                            Material.CQuantity, BorProductId = Material.BorId, Material.CorporateProductCatalogId
                        };
                        await connection.QueryAsync(queryMaterial, param);
                    }

                foreach (var Labour in POParameter.PoDto.POResources.labour)
                    if (!Labour.HasChanged)
                    {
                        var param = new
                        {
                            Labour.CQuantity, BorProductId = Labour.BorId, Labour.CorporateProductCatalogId
                        };
                        await connection.QueryAsync(queryLabour, param);
                    }

                foreach (var Tools in POParameter.PoDto.POResources.tools)
                    if (!Tools.HasChanged)
                    {
                        var param = new
                        {
                            Tools.CQuantity, BorProductId = Tools.BorId, Tools.CorporateProductCatalogId
                        };
                        await connection.QueryAsync(queryTools, param);
                    }

                foreach (var Consumable in POParameter.PoDto.POResources.consumable)
                    if (!Consumable.HasChanged)
                    {
                        var param = new
                        {
                            Consumable.CQuantity, BorProductId = Consumable.BorId, Consumable.CorporateProductCatalogId
                        };
                        await connection.QueryAsync(queryConsumable, param);
                    }
                
            }

            return "Ok";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public Task<string> UploadPODocument(POParameter POParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<string> PoCpcAverage(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);

        try
        {
            var data =
                @"SELECT POResources.CCPCId AS CpcId,POResources.CUnitPrice AS Price,POResources.PurchesOrderId AS PoId FROM POResources WHERE POResources.PurchesOrderId = (SELECT POHeader.Id FROM POHeader WHERE POHeader.SequenceId = @SequenceId)";

            var parameter = new { POParameter.PoDto.SequenceId };
            IEnumerable<POCpcAverageDto> cpcData;
            using (var connection = new SqlConnection(connectionString))
            {
                cpcData = connection.Query<POCpcAverageDto>(data, parameter);
            }

            if (cpcData != null)
                foreach (var r in cpcData)
                {
                    var query =
                        @"INSERT INTO dbo.POCpcAverage (Id,CpcId,PoId,CreatedDate,ModifiedDate,Price) VALUES (@Id,@CpcId,@PoId,@CreatedDate,@ModifiedDate,@Price)";

                    var parameter1 = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        r.CpcId,
                        r.PoId,
                        r.Price,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };

                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.ExecuteAsync(query, parameter1);
                    }

                    var selectprice =
                        @"SELECT TOP (3) Price FROM dbo.POCpcAverage where POCpcAverage.CpcId = @CpcId Order by CreatedDate desc";
                    IEnumerable<POCpcAverageDto> cpcPrice;
                    using (var connection = new SqlConnection(connectionString))
                    {
                        cpcPrice = connection.Query<POCpcAverageDto>(selectprice, parameter1);
                    }

                    if (cpcPrice != null)
                    {
                        double count = 0;
                        double total = 0;
                        double avg = 0;
                        string strAvg = null;

                        foreach (var i in cpcPrice)
                        {
                            count++;
                            total = total + double.Parse(i.Price);
                            avg = total / count;
                        }

                        avg = Math.Round(avg, 2);
                        strAvg = avg.ToString();

                        var updatePrice =
                            @"Update dbo.CorporateProductCatalog set CorporateProductCatalog.InventoryPrice = @strAvg where Id = @CpcId";

                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(updatePrice, new { r.CpcId, strAvg });
                        }
                    }
                }


            return POParameter.Id;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    
    public async Task<string> CpcUnitPriceUpdate(POParameter POParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(POParameter.ContractingUnitSequenceId, null,
            POParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);
            
        var data =
            @"SELECT POResources.CCPCId AS CpcId,POResources.CUnitPrice AS Price,POResources.PurchesOrderId AS PoId FROM POResources WHERE POResources.PurchesOrderId = (SELECT POHeader.Id FROM POHeader WHERE POHeader.SequenceId = @SequenceId)";

        var parameter = new { POParameter.PoDto.SequenceId };
        IEnumerable<POCpcAverageDto> cpcData;
       
            cpcData = connection.Query<POCpcAverageDto>(data, parameter);

            if (cpcData != null)
            foreach (var r in cpcData)
            {

                var cpcUnitPrice = connection
                    .Query<string>("Select InventoryPrice From CorporateProductCatalog Where Id = @Id",
                        new { Id = r.CpcId }).FirstOrDefault();

                if (cpcUnitPrice != null)
                {
                    if (cpcUnitPrice.ToDouble() < r.Price.ToDouble())
                    {
                        var updatePrice =
                            @"Update dbo.CorporateProductCatalog set CorporateProductCatalog.InventoryPrice = @strAvg where Id = @CpcId";

                
                        await connection.ExecuteAsync(updatePrice, new { r.CpcId, strAvg = r.Price });
                    }
                }
                
            }


        return POParameter.Id;
    }
}