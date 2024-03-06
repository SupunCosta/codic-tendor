using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.CAB;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.CPC;

public class CoporateProductCatalogRepository : ICoporateProductCatalogRepository
{
    public async Task<CorporateProductCatalogDto> CreateCoporateProductCatalog(CpcParameters cpcParameters,
        IHttpContextAccessor contextAccessor)
    {
        try
        {
            
            // var acc = contextAccessor;
            // var options = new DbContextOptions<ApplicationDbContext>();
            // var applicationDbContext =
            //     new ApplicationDbContext(options, cpcParameters.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
                cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
            await using var connection = new SqlConnection(connectionString);


            var cpcId = await CreateCpc(cpcParameters.Context, cpcParameters.CpcDto, cpcParameters,
                connectionString);
            if (cpcParameters.CpcDto.CpcImage != null)
                CreateCpcImage(cpcParameters.CpcDto.CpcImage, connectionString, cpcParameters.CpcDto.Id);

            if (cpcParameters.CpcDto.CpcResourceNickname != null)
                CreateNickName(cpcParameters.CpcDto.CpcResourceNickname, connectionString, cpcParameters.CpcDto.Id);

            if (cpcParameters.CpcDto.CpcVendor != null)
                CreateVendor(cpcParameters.CpcDto.CpcVendor, connectionString, cpcParameters.CpcDto.Id);

            if (cpcParameters.CpcDto.Status == 1)
                await CreateHistory(cpcParameters.CpcDto, contextAccessor, cpcParameters.CpcDto.Id, cpcParameters);

            if (cpcParameters.CpcDto.ResourceId != null)
            {
                var gdCpc = connection
                    .Query<CpcVehicleTrackingNo>("Select * from CpcVehicleTrackingNo Where CpcId = @CpcId",
                        new { CpcId = cpcId }).FirstOrDefault();

                var param = new CpcVehicleTrackingNo
                {
                    Id = Guid.NewGuid().ToString(),
                    CpcId = cpcParameters.CpcDto.Id,
                    ResourceId = cpcParameters.CpcDto.ResourceId,
                    TrackingNo = cpcParameters.CpcDto.TrackingNo
                };
                if (gdCpc != null)
                    await connection.ExecuteAsync(
                        "Update CpcVehicleTrackingNo Set ResourceId = @ResourceId, TrackingNo = @TrackingNo Where CpcId = @CpcId",
                        param);
                else
                    await connection.ExecuteAsync(
                        "INSERT INTO dbo.CpcVehicleTrackingNo ( Id ,CpcId ,ResourceId ,TrackingNo ) VALUES ( @Id ,@CpcId ,@ResourceId ,@TrackingNo )",
                        param);
            }


            if (cpcId != null)
            {
            
                if (cpcParameters.CpcDto.Id != null)
                {
                    var cpcVelocity = connection
                        .Query<CPCVelocity>("Select * from CPCVelocity Where CPCId = @CPCId",
                            new { CPCId = cpcParameters.CpcDto.Id }).FirstOrDefault();

                    var paramVelocity = new CPCVelocity
                    {
                        Id = Guid.NewGuid().ToString(),
                        CPCId = cpcParameters.CpcDto.Id,
                        Velocity = cpcParameters.CpcDto.Velocity
                    };

                    if (cpcVelocity != null)
                        await connection.ExecuteAsync(
                            "Update CPCVelocity Set Velocity = @Velocity Where CPCId = @CPCId",
                            paramVelocity);
                    else
                        await connection.ExecuteAsync(
                            "INSERT INTO dbo.CPCVelocity ( Id ,CPCId ,Velocity  ) VALUES ( @Id ,@CPCId ,@Velocity )",
                            paramVelocity);
                }

                if (cpcParameters.CpcDto.Id != null)
                {
                    var cpcVelocity = connection
                        .Query<CPCVelocity>("Select * from CpcSerialNumber Where CPCId = @CPCId",
                            new { CPCId = cpcParameters.CpcDto.Id }).FirstOrDefault();

                    var paramVelocity = new CpcSerialNumber
                    {
                        Id = Guid.NewGuid().ToString(),
                        CPCId = cpcParameters.CpcDto.Id,
                        SerialNumber = cpcParameters.CpcDto.SerialNumber
                    };

                    if (cpcVelocity != null)
                        await connection.ExecuteAsync(
                            "Update CpcSerialNumber Set SerialNumber = @SerialNumber Where CPCId = @CPCId",
                            paramVelocity);
                    else
                        await connection.ExecuteAsync(
                            "INSERT INTO dbo.CpcSerialNumber ( Id ,CPCId ,SerialNumber  ) VALUES ( @Id ,@CPCId ,@SerialNumber )",
                            paramVelocity);
                }
            }

            cpcParameters.Id = cpcId;

            var cpcDto = await GetCorporateProductCatalogById(cpcParameters);
            return cpcDto;
        
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    public async Task<CpcDropdown> GetCpcDropdown(CpcParameters cpcParameters)
    {
        var cpcDropdownData = new CpcDropdown
        {
            CpcResourceFamily = getCpcResouceFamily(cpcParameters).Result,
            CpcResourceType = getCpcResouceType(cpcParameters).Result,
            CpcBasicUnitOfMeasure = getCpcBasicUnitOfMeasure(cpcParameters).Result,
            CpcMaterial = getCpcMaterial(cpcParameters).Result,
            CpcPressureClass = getCpcPressureClass(cpcParameters).Result,
            CpcUnitOfSizeMeasure = getCpcUnitOfSizeMeasure(cpcParameters).Result,
            CpcBrand = getCpcBrand(cpcParameters).Result
        };
        return cpcDropdownData;
    }


    public async Task<IEnumerable<CpcResourceType>> getCpcResouceType(CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;
        if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Contains("en")) lang = "en";

        var sql =
            "SELECT CpcResourceTypeLocalizedData.CpcResourceTypeId AS Id ,CpcResourceTypeLocalizedData.Label AS Name FROM dbo.CpcResourceTypeLocalizedData WHERE CpcResourceTypeLocalizedData.LanguageCode = '" +
            lang + "' order by CpcResourceTypeLocalizedData.Label asc";

        using (var dbConnection =
               new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        {
            var result = await dbConnection.QueryAsync<CpcResourceType>(sql);
            return result;
        }
    }

    public async Task<string> UploadImage(CpcParameters cpcParameters)
    {
        var client = new FileClient();
        var url = client.PersistPhoto(cpcParameters.Image.Files.FirstOrDefault()?.FileName,
            cpcParameters.TenantProvider, cpcParameters.Image.Files.FirstOrDefault());
        return url;
    }

    public async Task DeleteVendor(CpcParameters cpcParameters)
    {
        CommonDelete(cpcParameters, "CpcVendor");
    }

    public async Task DeleteImage(CpcParameters cpcParameters)
    {
        CommonDelete(cpcParameters, "CpcImage");
    }

    public async Task DeleteNickName(CpcParameters cpcParameters)
    {
        CommonDelete(cpcParameters, "CpcResourceNickname");
    }

    public async Task<CorporateProductCatalogDto> GetCorporateProductCatalogById(CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;

        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
        var sql = @"SELECT
                 CorporateProductCatalog.Id AS Id
                 ,CorporateProductCatalog.ResourceTitle AS ResourceTitle
                 ,CorporateProductCatalog.InventoryPrice AS InventoryPrice
                 ,CorporateProductCatalog.Size AS Size
                 ,CorporateProductCatalog.WallThickness AS WallThickness
                 ,CorporateProductCatalog.MinOrderQuantity AS MinOrderQuantity
                 ,CorporateProductCatalog.MaxOrderQuantity AS MaxOrderQuantity
                 ,CorporateProductCatalog.Weight AS Weight
                 ,CorporateProductCatalog.Status AS Status
                 ,CpcMaterialLocalizedData.CpcMaterialId AS CpcMaterialId
                 ,CpcMaterialLocalizedData.Label AS CpcMaterialName
                 ,CorporateProductCatalog.ResourceNumber
                 , CorporateProductCatalog.Title AS Title
                  ,CorporateProductCatalog.Title AS [Text]
                  , CorporateProductCatalog.Title AS HeaderTitle
                ,CpcResourceTypeLocalizedData.CpcResourceTypeId AS [Key]
                ,CpcResourceTypeLocalizedData.Label AS [Text]
                   ,CpcResourceFamilyLocalizedData.CpcResourceFamilyId AS [Key]
                  ,CpcResourceFamilyLocalizedData.Label AS [Text]
                   ,CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId AS [Key]
                 ,CpcBasicUnitOfMeasureLocalizedData.Label AS [Text]
                   ,CpcPressureClass.Id AS [Key]
                   ,CpcPressureClass.Name AS [Text]
                   ,CpcUnitOfSizeMeasure.Id AS [Key]
                   ,CpcUnitOfSizeMeasure.Name AS [Text]
				   ,CpcBrand.CpcBrandId AS [Key]
                   ,CpcBrand.Name AS [Text]
                FROM dbo.CorporateProductCatalog
                LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                  ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                  ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId
                LEFT OUTER JOIN dbo.CpcMaterialLocalizedData
                  ON CorporateProductCatalog.CpcMaterialId = CpcMaterialLocalizedData.CpcMaterialId
                LEFT OUTER JOIN dbo.CpcPressureClass
                  ON CorporateProductCatalog.CpcPressureClassId = CpcPressureClass.Id
                LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
                  ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                LEFT OUTER JOIN dbo.CpcUnitOfSizeMeasure
                  ON CorporateProductCatalog.CpcUnitOfSizeMeasureId = CpcUnitOfSizeMeasure.Id
                LEFT OUTER JOIN CpcBrand on CorporateProductCatalog.CpcBrandId = CpcBrand.CpcBrandId
                WHERE (CpcResourceTypeLocalizedData.LanguageCode = @lang
                OR CpcResourceTypeLocalizedData.CpcResourceTypeId IS NULL)
                AND (CpcResourceFamilyLocalizedData.LanguageCode = @lang
                OR CorporateProductCatalog.ResourceFamilyId IS NULL)
                AND (CpcMaterialLocalizedData.LanguageCode = @lang
                OR CorporateProductCatalog.CpcMaterialId IS NULL)
                AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
                OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)
				 AND (CpcBrand.LanguageCode =@lang
                OR CorporateProductCatalog.CpcBrandId IS NULL)
                AND (CorporateProductCatalog.ResourceNumber = @id OR CorporateProductCatalog.Id =@id)
                AND CorporateProductCatalog.IsDeleted = 0";

        var parameters = new { lang, id = cpcParameters.Id };
        var parameters2 = new { lang, id = "" };
        IEnumerable<CorporateProductCatalogDto> cpcs = null;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            await dbConnection.OpenAsync();
            cpcs = await dbConnection
                .QueryAsync<CorporateProductCatalogDto, CpcResourceTypeDto, CpcResourceFamilyDtoDapper,
                    CpcBasicUnitOfMeasureDto, CpcPressureClassDto, CpcUnitOfSizeMeasureDto, CpcBrandDto,
                    CorporateProductCatalogDto>
                (sql, (corporateProductCatalog, cpcResourceType, cpcResourceFamily, cpcBasicUnitOfMeasure,
                    cpcPressureClass, cpcUnitOfSizeMeasure, cCpcBrand) =>
                {
                    corporateProductCatalog.ResourceType = cpcResourceType;
                    corporateProductCatalog.ResourceFamily = cpcResourceFamily;
                    corporateProductCatalog.CpcBasicUnitOfMeasure = cpcBasicUnitOfMeasure;
                    corporateProductCatalog.CpcPressureClass = cpcPressureClass;
                    corporateProductCatalog.CpcUnitOfSizeMeasure = cpcUnitOfSizeMeasure;
                    corporateProductCatalog.CpcBrand = cCpcBrand;
                    return corporateProductCatalog;
                }, splitOn: "Key,Key,Key,Key,Key", param: parameters);
        }

        var cpc = cpcs.FirstOrDefault();
        if (cpc != null)
        {
            var cpcMaterialDto = new CpcMaterialDto
            {
                Key = cpc.CpcMaterialId,
                Text = cpc.CpcMaterialName
            };
            cpc.CpcMaterial = cpcMaterialDto;

            var sql2 =
                "SELECT CpcImage.Id AS Id ,CpcImage.Image AS Image FROM dbo.CpcImage  WHERE IsDeleted = 0 AND CpcImage.CoperateProductCatalogId = '" +
                cpc.Id + "'";

            var sql3 =
                "SELECT  CpcResourceNickname.Id ,CpcResourceNickname.NickName ,CpcResourceNickname.Language ,CpcResourceNickname.LocaleCode" +
                " ,CpcResourceNickname.CoperateProductCatalogId FROM dbo.CpcResourceNickname WHERE IsDeleted = 0 AND " +
                "CpcResourceNickname.CoperateProductCatalogId = '" + cpc.Id + "'";

            var sql4 =
                "SELECT CpcVendor.Id ,CpcVendor.ResourceNumber ,CpcVendor.ResourceTitle ,CpcVendor.PurchasingUnit ,CpcVendor.ResourcePrice, CpcVendor.CompanyId  ,CpcVendor.ResourceLeadTime" +
                " ,CpcVendor.MinOrderQuantity ,CpcVendor.MaxOrderQuantity ,CpcVendor.RoundingValue ,CpcVendor.PreferredParty ,CabCompany.Id, CabCompany.Name  " +
                "FROM dbo.CpcVendor LEFT OUTER JOIN dbo.CabCompany   ON CpcVendor.CompanyId = CabCompany.Id WHERE CpcVendor.IsDeleted = 0 AND CpcVendor.CoperateProductCatalogId = '" +
                cpc.Id + "'";

            //string sql5 = "SELECT   CpcHistoryLog.ChangedTime AS DateTime ,CONCAT(AllUsers.FirstName,' ',AllUsers.LastName) AS [User]  ,CpcHistoryLog.RevisionNumber AS RevisionNumber" +
            //              " FROM dbo.CpcHistoryLog LEFT OUTER JOIN dbo.AllUsers   ON CpcHistoryLog.ChangedByUserId = AllUsers.Id WHERE " +
            //              "CpcHistoryLog.CoperateProductCatalogId ='" + cpc.Id + "' ORDER BY RevisionNumber";

            var sql5 =
                "SELECT   CpcHistoryLog.ChangedTime AS DateTime ,CpcHistoryLog.ChangedByUserId AS Oid  ,CpcHistoryLog.RevisionNumber AS RevisionNumber" +
                " FROM dbo.CpcHistoryLog WHERE CpcHistoryLog.CoperateProductCatalogId ='" + cpc.Id +
                "' ORDER BY RevisionNumber";

            IEnumerable<CpcImageCreateDto> images = null;
            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                images = await connection.QueryAsync<CpcImageCreateDto>(sql2);
            }

            cpc.CpcImage = images.ToList();

            IEnumerable<CpcResourceNicknameCreateDto> nickNames = null;
            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                nickNames = await connection.QueryAsync<CpcResourceNicknameCreateDto>(sql3);
            }

            cpc.CpcResourceNickname = nickNames.ToList();

            IEnumerable<CpcVendor> venders = null;
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                await dbConnection.OpenAsync();
                venders = await dbConnection
                    .QueryAsync<CpcVendor, CompanyDto,
                        CpcVendor>
                    (sql4, (cpcVendor, cabCompany) =>
                    {
                        cpcVendor.Company = cabCompany;
                        return cpcVendor;
                    }, splitOn: "Id", param: parameters);
            }

            var companyRepository = new CompanyRepository();
            var companyRepositoryParameter = new CompanyRepositoryParameter
            {
                ApplicationDbContext = cpcParameters.Context,
                TenantProvider = cpcParameters.TenantProvider
            };
            foreach (var vender in venders)
            {
                companyRepositoryParameter.CompanyId = vender.CompanyId;
                var companyDto = companyRepository.GetCompanyById(companyRepositoryParameter);
                vender.Company = companyDto.Result;
            }

            IEnumerable<ProjectDefinitionHistoryLogDapperDto> historyList = null;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                historyList = await connection.QueryAsync<ProjectDefinitionHistoryLogDapperDto>(sql5);
            }

            var historyLogDto = new ProjectDefinitionHistoryLogDto();
            var historyUserQuery =
                @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [User] FROM ApplicationUser WHERE ApplicationUser.Oid =@userId";
            IEnumerable<string> userName = null;
            var historyLst = historyList.ToList();
            if (historyLst.Any())
            {
                historyLogDto.CreatedDateTime = historyLst.FirstOrDefault().DateTime;
                historyLogDto.RevisionNumber = historyLst.FirstOrDefault().RevisionNumber;
                var historyUserParameter = new { userId = historyLst.FirstOrDefault().Oid };
                using (var connection =
                       new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
                {
                    userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                    historyLogDto.CreatedByUser = userName.FirstOrDefault();
                }
            }

            if (historyLst.Count >= 2)
            {
                historyLogDto.UpdatedDateTime = historyLst.LastOrDefault().DateTime;
                historyLogDto.RevisionNumber = historyLst.LastOrDefault().RevisionNumber;
                var historyUserParameter = new { userId = historyLst.LastOrDefault().Oid };
                using (var connection =
                       new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
                {
                    userName = connection.Query<string>(historyUserQuery, historyUserParameter);
                    historyLogDto.UpdatedByUser = userName.FirstOrDefault();
                }
            }

            cpc.CpcVendor = venders.ToList();
            cpc.CpcHistoryLog = historyLogDto;
            cpc.HeaderTitle = GenerateHeaderTitle(cpc);

            using (var connection = new SqlConnection(connectionString))
            {
                cpc.GdVehicle = connection
                    .Query<CpcVehicleTrackingNo>("Select * From CpcVehicleTrackingNo Where CpcId = @CpcId",
                        new { CpcId = cpc.Id }).FirstOrDefault();
                var velocity = connection
                    .Query<string>("Select Velocity From CPCVelocity Where CPCId = @CPCId", new { CPCId = cpc.Id })
                    .FirstOrDefault();

                if (velocity != null)
                    cpc.Velocity = velocity.ToDouble();
                else
                    cpc.Velocity = 0;
                
                cpc.SerialNumber = connection
                    .Query<string>("Select SerialNumber From CpcSerialNumber Where CPCId = @CPCId", new { CPCId = cpc.Id })
                    .FirstOrDefault();    
            }

            return cpc;
        }

        return null;
    }

    public async Task<IEnumerable<CoperateProductCatalogFilterNewDto>> GetCorporateProductCatalog(
        CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;

        //string sql = SELECT * FROM CpcFilter;
        //CREATE VIEW CpcFilter AS 
        var sql =
            "SELECT CorporateProductCatalog.Id, CorporateProductCatalog.ResourceTitle ,CorporateProductCatalog.Status AS Status ,CorporateProductCatalog.ResourceNumber AS ResourceNumber " +
            " ,CpcResourceTypeLocalizedData.Label AS ResourceType  , CorporateProductCatalog.Title AS Title" +
            "  , CONCAT(CorporateProductCatalog.ResourceNumber, ' - ',  CorporateProductCatalog.ResourceTitle) AS HeaderTitle  FROM dbo.CpcResourceTypeLocalizedData " +
            " ,dbo.CorporateProductCatalog  INNER JOIN dbo.CpcResourceType ON CorporateProductCatalog.ResourceTypeId = CpcResourceType.Id " +
            " WHERE CorporateProductCatalog.IsDeleted = 0 AND CpcResourceType.Id = CorporateProductCatalog.ResourceTypeId AND CpcResourceTypeLocalizedData.CpcResourceTypeId = CpcResourceType.Id " +
            " AND CpcResourceTypeLocalizedData.LanguageCode = @lang";

        var sql2 =
            "SELECT CorporateProductCatalog.Id, CorporateProductCatalog.ResourceTitle ,CorporateProductCatalog.Status AS Status ,CorporateProductCatalog.ResourceNumber AS ResourceNumber " +
            " ,CpcResourceTypeLocalizedData.Label AS ResourceType  , CorporateProductCatalog.Title AS Title" +
            "  , CONCAT(CorporateProductCatalog.ResourceNumber, ' - ',  CorporateProductCatalog.ResourceTitle) AS HeaderTitle  FROM dbo.CpcResourceTypeLocalizedData " +
            " ,dbo.CorporateProductCatalog  INNER JOIN dbo.CpcResourceType ON CorporateProductCatalog.ResourceTypeId = CpcResourceType.Id " +
            " WHERE CorporateProductCatalog.IsDeleted = 0 AND (CpcResourceType.Id = 'c46c3a26-39a5-42cc-n7k1-89655304eh6' OR CpcResourceType.Id = 'c46c3a26-39a5-42cc-n9wn-89655304eh6' OR CpcResourceType.Id = 'c46c3a26-39a5-42cc-m06g-89655304eh6') AND CpcResourceTypeLocalizedData.CpcResourceTypeId = CpcResourceType.Id " +
            " AND CpcResourceTypeLocalizedData.LanguageCode = @lang";


        if (!cpcParameters.filter.isStock)
        {
            var sb = new StringBuilder(sql);

            if (cpcParameters.filter != null)
            {
                if (cpcParameters.filter.Title != null)
                {
                    var words = cpcParameters.filter.Title.Split(" ");
                    foreach (var element in words)
                        sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
                    //sb.Append(" AND CorporateProductCatalog.Title  LIKE '%" + cpcParameters.filter.Title + "%'");
                }

                if (cpcParameters.filter.ResourceTypeId != null)
                    sb.Append(" AND CpcResourceTypeLocalizedData.CpcResourceTypeId = '" +
                              cpcParameters.filter.ResourceTypeId + "'");

                if (cpcParameters.filter.Status != null)
                    sb.Append(" AND CorporateProductCatalog.Status =" + cpcParameters.filter.Status);

                if (cpcParameters.filter.Sorter != null)
                {
                    if (cpcParameters.filter.Sorter.Attribute != null &&
                        cpcParameters.filter.Sorter.Order.ToLower().Equals("asc"))
                        sb.Append(" ORDER BY " + cpcParameters.filter.Sorter.Attribute + " ASC");

                    if (cpcParameters.filter.Sorter.Attribute != null &&
                        cpcParameters.filter.Sorter.Order.ToLower().Equals("desc"))
                        sb.Append(" ORDER BY " + cpcParameters.filter.Sorter.Attribute + " DESC");
                }
            }

            var connectionString = ConnectionString.MapConnectionString(
                cpcParameters.ContractingUnitSequenceId, cpcParameters.ProjectSequenceId,
                cpcParameters.TenantProvider);
            using var dbConnection = new SqlConnection(connectionString);
            await dbConnection.OpenAsync();
            var result = await dbConnection.QueryAsync<CoperateProductCatalogFilterNewDto>(sb.ToString(), new { lang });
            return result;
        }
        else
        {
            var sb = new StringBuilder(sql2);

            if (cpcParameters.filter != null)
            {
                if (cpcParameters.filter.Title != null)
                {
                    var words = cpcParameters.filter.Title.Split(" ");
                    foreach (var element in words)
                        sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
                    //sb.Append(" AND CorporateProductCatalog.Title  LIKE '%" + cpcParameters.filter.Title + "%'");
                }

                if (cpcParameters.filter.ResourceTypeId != null)
                    sb.Append(" AND CpcResourceTypeLocalizedData.CpcResourceTypeId = '" +
                              cpcParameters.filter.ResourceTypeId + "'");

                if (cpcParameters.filter.Status != null)
                    sb.Append(" AND CorporateProductCatalog.Status =" + cpcParameters.filter.Status);

                if (cpcParameters.filter.Sorter != null)
                {
                    if (cpcParameters.filter.Sorter.Attribute != null &&
                        cpcParameters.filter.Sorter.Order.ToLower().Equals("asc"))
                        sb.Append(" ORDER BY " + cpcParameters.filter.Sorter.Attribute + " ASC");

                    if (cpcParameters.filter.Sorter.Attribute != null &&
                        cpcParameters.filter.Sorter.Order.ToLower().Equals("desc"))
                        sb.Append(" ORDER BY " + cpcParameters.filter.Sorter.Attribute + " DESC");
                }
            }

            var connectionString = ConnectionString.MapConnectionString(
                cpcParameters.ContractingUnitSequenceId, cpcParameters.ProjectSequenceId,
                cpcParameters.TenantProvider);
            await using var dbConnection = new SqlConnection(connectionString);
            await dbConnection.OpenAsync();
            var result = await dbConnection.QueryAsync<CoperateProductCatalogFilterNewDto>(sb.ToString(), new { lang });
            
            return result;
        }
    }

    public async Task<IEnumerable<CpcResourceType>> GetShortcutPaneData(CpcParameters cpcParameters)
    {
        return getCpcResouceType(cpcParameters).Result;
    }

    public async Task DeleteCpc(CpcParameters cpcParameters)
    {
        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
        foreach (var id in cpcParameters.IdList)
            await using (var dbConnection = new SqlConnection(connectionString))
            {
                await dbConnection.ExecuteAsync(
                    "update CorporateProductCatalog set IsDeleted = @val where ResourceNumber = @id",
                    new { val = true, id });
                
            }
    }

    public async Task<IEnumerable<CpcExporter>> GetCpcToExport(CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;
        if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Contains("en")) lang = "en";

        var sql = @"SELECT
                                CpcResourceFamilyLocalizedData.Label AS ResourceFamily
                                ,CpcBasicUnitOfMeasureLocalizedData.Label AS CpcBasicUnitOfMeasure
                                ,CorporateProductCatalog.Id
                                ,CorporateProductCatalog.ResourceTitle AS Title
                                ,CorporateProductCatalog.InventoryPrice AS InventoryPrice
                                ,CorporateProductCatalog.Size AS Size
                                ,CorporateProductCatalog.WallThickness AS WallThickness
                                ,CorporateProductCatalog.MinOrderQuantity AS MinOrderQuantity
                                ,CorporateProductCatalog.MaxOrderQuantity AS MaxOrderQuantity
                                ,CorporateProductCatalog.Weight AS Weight
                                ,CpcUnitOfSizeMeasure.Name AS CpcUnitOfSizeMeasure
                                ,CpcPressureClass.Name AS CpcPressureClass
                                ,CpcResourceTypeLocalizedData.Label AS ResourceType
                                FROM dbo.CorporateProductCatalog
                                LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                LEFT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                                ON CorporateProductCatalog.ResourceFamilyId = CpcResourceFamilyLocalizedData.CpcResourceFamilyId
                                LEFT OUTER JOIN dbo.CpcMaterialLocalizedData
                                ON CorporateProductCatalog.CpcMaterialId = CpcMaterialLocalizedData.CpcMaterialId
                                LEFT OUTER JOIN dbo.CpcPressureClass
                                ON CorporateProductCatalog.CpcPressureClassId = CpcPressureClass.Id
                                LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
                                ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                                LEFT OUTER JOIN dbo.CpcUnitOfSizeMeasure
                                ON CorporateProductCatalog.CpcUnitOfSizeMeasureId = CpcUnitOfSizeMeasure.Id
                                WHERE (CpcResourceTypeLocalizedData.LanguageCode = 'en'
                                OR CpcResourceTypeLocalizedData.CpcResourceTypeId IS NULL)
                                AND (CpcResourceFamilyLocalizedData.LanguageCode = 'en'
                                OR CorporateProductCatalog.ResourceFamilyId IS NULL)
                                AND (CpcMaterialLocalizedData.LanguageCode = 'en'
                                OR CorporateProductCatalog.CpcMaterialId IS NULL)
                                AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = 'en'
                                OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)
                                AND CorporateProductCatalog.IsDeleted=0
                                ";
        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var result = dbConnection.Query<CpcExporter>(sql);
            return result;
        }
    }

    public async Task<CpcForBorDto> GetCpcForBorById(CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;
        if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Equals("en-us")) lang = "en";

        var query = @"
                              select CorporateProductCatalog.Id, CorporateProductCatalog.Title AS Title,
                              CorporateProductCatalog.ResourceNumber,CorporateProductCatalog.ResourceTypeId, CpcResourceTypeLocalizedData.Label AS ResourceType,
                              CpcBasicUnitOfMeasureLocalizedData.Id AS BasicUnitOfMeasureId, CpcBasicUnitOfMeasureLocalizedData.Label AS BasicUnitOfMeasure
                              from CorporateProductCatalog
                              LEFT OUTER JOIN CpcResourceTypeLocalizedData on CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                              LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                              WHERE (CpcResourceTypeLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.ResourceTypeId is null)
                              AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang OR CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                              AND CorporateProductCatalog.ResourceNumber = @id
                             ";
        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
        var parameters = new { lang, id = cpcParameters.Id };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var result = dbConnection.Query<CpcForBorDto>(query, parameters)
                .FirstOrDefault();
            return result;
        }
    }

    public async Task<CpcDetails> GetCpcDetailsById(CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;
        if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Equals("en-us")) lang = "en";

        var query = @"
                                SELECT
                                  CorporateProductCatalog.Id AS Id
                                 ,CorporateProductCatalog.Title AS ResourceTitle
                                 ,CpcResourceTypeLocalizedData.Label AS ResourceType
                                 ,CpcBasicUnitOfMeasureLocalizedData.Label AS BasicUnitOfMeasure
                                 ,CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId AS BasicUnitOfMeasureId
                                 ,CorporateProductCatalog.ResourceNumber AS ResourceNumber
                                FROM dbo.CorporateProductCatalog
                                LEFT OUTER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData
                                  ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                                LEFT OUTER JOIN dbo.CpcResourceTypeLocalizedData
                                  ON CorporateProductCatalog.ResourceTypeId = CpcResourceTypeLocalizedData.CpcResourceTypeId
                                WHERE (CpcResourceTypeLocalizedData.LanguageCode = @lang
                                OR CorporateProductCatalog.ResourceTypeId IS NULL)
                                AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang
                                OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)
                                AND (CorporateProductCatalog.ResourceNumber = @id OR CorporateProductCatalog.ID = @id)";
        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
        var parameters = new { lang, id = cpcParameters.Id };
        //cpcParameters.TenantProvider.GetTenant().ConnectionString;
        CpcDetails result;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            result = dbConnection.Query<CpcDetails>(query, parameters).FirstOrDefault();
            if (result != null) return result;
        }

        await using (var dbConnection =
                     new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        {
            result = dbConnection.Query<CpcDetails>(query, parameters).FirstOrDefault();
            if (result != null) return result;
        }

        var catelogConnection = cpcParameters.TenantProvider.GetTenant().CatelogConnectionString;
        var queryCU =
            @"select  ConnectionString from [dbo].[UPrinceCustomerContractingUnit] where ContractingUnitId = @sequenceCode";
        var parameter = new { sequenceCode = cpcParameters.ContractingUnitSequenceId };
        await using (var dbConnection = new SqlConnection(catelogConnection))
        {
            connectionString = dbConnection.Query<string>(queryCU, parameter).FirstOrDefault();
            await using (var dbConnectionX = new SqlConnection(connectionString))
            {
                result = dbConnectionX.Query<CpcDetails>(query, parameters).FirstOrDefault();
                if (result != null) return result;
            }
        }


        return result;
    }

    public async Task<IEnumerable<CpcForProductDto>> ReadVehiclesForQr(CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;
        if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Equals("en-us")) lang = "en";

        var query = @"
                             select c.Id AS [Key], c.Title AS Text from CorporateProductCatalog c
                             where c.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04'
                            AND c.Title like '%" + cpcParameters.Title + "%'";

        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            null, cpcParameters.TenantProvider);
        var parameters = new { lang, id = cpcParameters.Id };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var result =
                dbConnection.Query<CpcForProductDto>(query, parameters);
            

            return result;
        }
    }

    public async Task<IEnumerable<CoperateProductCatalogFilterDto>> MissingCPC(CpcParameters cpcParameters)
    {
        try
        {
            var select =
                @"SELECT * , DB_NAME() AS Project FROM dbo.CorporateProductCatalog WHERE ResourceTypeId IS NULL OR CpcBasicUnitOfMeasureId IS NULL";

            var result = new List<Databases>();
            string env = null;

            if (cpcParameters.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                env = "uprincev4uatdb";
            else if (cpcParameters.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                env = "uprincev4einstein";

            using (var connection = new SqlConnection("Server=tcp:" + env +
                                                      ".database.windows.net,1433;Initial Catalog=master;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                result = connection.Query<Databases>(
                        @"select [name] as DatabaseName from sys.databases WHERE name NOT IN('master', 'MsalTokenCacheDatabase', 'UPrinceV4EinsteinCatelog', 'UPrinceV4UATCatelog', 'COM0001', 'UPrinceV4Einstein', 'UPrinceV4UAT') order by name")
                    .ToList();
            }

            var pbsData = new List<CoperateProductCatalogFilterDto>();

            var cpc = new List<CoperateProductCatalogFilterDto>();

            foreach (var i in result)
            {
                var connectionString = "Server=tcp:" + env + ".database.windows.net,1433;Initial Catalog=" +
                                       i.DatabaseName +
                                       ";Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (var connection = new SqlConnection(connectionString))
                {
                    cpc.AddRange(connection.Query<CoperateProductCatalogFilterDto>(select).ToList());
                }
            }


            return cpc;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    
    private async Task<string> CreateCpc(ApplicationDbContext context, CoperateProductCatalogCreateDto cpcDto,
        CpcParameters cpcParameters, string connectionString)
    {
        var ResourceNumber = "";

        var sb1 =
            new StringBuilder(
                "select * from CorporateProductCatalog where  ResourceNumber  = @ResourceNumber OR Id = @Id");
        await using (var connection = new SqlConnection(connectionString))
        {
            var cpc = connection
                .QuerySingleOrDefaultAsync<CorporateProductCatalog>(sb1.ToString(),
                    new { cpcDto.Id, cpcDto.ResourceNumber }).Result;

            if (cpc != null)
            {
                var sb2 = new StringBuilder(
                    "update CorporateProductCatalog set  ResourceTitle= @ResourceTitle,ResourceTypeId= @ResourceTypeId,ResourceFamilyId=@ResourceFamilyId,CpcBasicUnitOfMeasureId=@CpcBasicUnitOfMeasureId,CpcMaterialId=@CpcMaterialId,CpcPressureClassId=@CpcPressureClassId,InventoryPrice=@InventoryPrice,CpcUnitOfSizeMeasureId=@CpcUnitOfSizeMeasureId,Size=@Size,WallThickness=@WallThickness,MinOrderQuantity=@MinOrderQuantity,MaxOrderQuantity=@MaxOrderQuantity,Weight=@Weight,Status=@Status,IsDeleted=@IsDeleted,CpcBrandId=@CpcBrandId, Title=@Title ");
                sb2.Append(" where Id = @Id");
                var affectedRows = connection.ExecuteAsync(sb2.ToString(), new
                {
                    cpcDto.Id,
                    cpcDto.ResourceTitle,
                    cpcDto.ResourceTypeId,
                    cpcDto.ResourceFamilyId,
                    cpcDto.CpcBasicUnitOfMeasureId,
                    cpcDto.CpcMaterialId,
                    cpcDto.CpcPressureClassId,
                    cpcDto.InventoryPrice,
                    cpcDto.CpcUnitOfSizeMeasureId,
                    cpcDto.Size,
                    cpcDto.WallThickness,
                    cpcDto.MinOrderQuantity,
                    cpcDto.MaxOrderQuantity,
                    cpcDto.Weight,
                    cpcDto.Status,
                    IsDeleted = false,
                    cpcDto.CpcBrandId,
                    Title = cpc.ResourceNumber + " - " + cpcDto.ResourceTitle
                }).Result;

                ResourceNumber = cpcDto.ResourceNumber;
            }
            else
            {
                if (cpcParameters.isCopy == false)
                {
                    var idGenerator = new IdGenerator();
                    ResourceNumber = idGenerator.GenerateId(context, "Ar01-", "CpcResourceNumber");

                    if (cpcParameters.ProjectSequenceId == null && cpcParameters.ContractingUnitSequenceId == null)
                        ResourceNumber = "O" + ResourceNumber;
                    else if (cpcParameters.ProjectSequenceId != null)
                        ResourceNumber = "P" + ResourceNumber;
                    else
                        ResourceNumber = "C" + ResourceNumber;
                }
                else
                {
                    ResourceNumber = cpcDto.ResourceNumber;
                    //cpcDto.Id = Guid.NewGuid().ToString();
                }


                var sb = new StringBuilder(
                    "INSERT INTO CorporateProductCatalog VALUES(@Id, @ResourceTitle,@ResourceTypeId,@ResourceFamilyId,@CpcBasicUnitOfMeasureId,@CpcMaterialId,@CpcPressureClassId,@InventoryPrice,@CpcUnitOfSizeMeasureId,@Size,@WallThickness,@MinOrderQuantity,@MaxOrderQuantity,@Weight,@Status,@ResourceNumber,@IsDeleted,@CpcBrandId, @Title);");


                var affectedRows = connection.ExecuteAsync(sb.ToString(), new
                {
                    cpcDto.Id,
                    cpcDto.ResourceTitle,
                    cpcDto.ResourceTypeId,
                    cpcDto.ResourceFamilyId,
                    cpcDto.CpcBasicUnitOfMeasureId,
                    cpcDto.CpcMaterialId,
                    cpcDto.CpcPressureClassId,
                    cpcDto.InventoryPrice,
                    cpcDto.CpcUnitOfSizeMeasureId,
                    cpcDto.Size,
                    cpcDto.WallThickness,
                    cpcDto.MinOrderQuantity,
                    cpcDto.MaxOrderQuantity,
                    cpcDto.Weight,
                    cpcDto.Status,
                    ResourceNumber,
                    IsDeleted = false,
                    cpcDto.CpcBrandId,
                    Title = ResourceNumber + " - " + cpcDto.ResourceTitle
                }).Result;
            }

            connection.Close();
        }

        return ResourceNumber;
    }

    private void CreateCpcImage(List<CpcImageCreateDto> images, string connectionString, string cpcId)
    {
        foreach (var i in images)
            if (i.Id == null)
            {
                var Id = Guid.NewGuid().ToString();

                var sb =
                    new StringBuilder(
                        "INSERT INTO CpcImage VALUES(@Id,@CoperateProductCatalogId,@Image,@IsDeleted);");

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = connection.ExecuteAsync(sb.ToString(), new
                    {
                        Id,
                        CoperateProductCatalogId = cpcId,
                        i.Image,
                        IsDeleted = false
                    }).Result;
                    connection.Close();
                }
            }
            else
            {
                var sb1 = new StringBuilder("select * from CpcImage where Id = @Id ");
                using (var connection = new SqlConnection(connectionString))
                {
                    var image = connection
                        .QuerySingleOrDefaultAsync<CpcImage>(sb1.ToString(), new { i.Id }).Result;

                    var sb2 = new StringBuilder(
                        "update CpcImage set  CoperateProductCatalogId=@CoperateProductCatalogId,Image=@Image ");
                    sb2.Append(" where Id = @Id");
                    var affectedRows = connection.ExecuteAsync(sb2.ToString(), new
                    {
                        i.Id,
                        CoperateProductCatalogId = cpcId,
                        i.Image
                    }).Result;
                    connection.Close();
                }
            }
    }

    private void CreateNickName(List<CpcResourceNicknameCreateDto> names, string connectionString, string cpcId)
    {
        var nickname = new CpcResourceNickname();
        foreach (var name in names)
            if (name.Id == null)
            {
                var Id = Guid.NewGuid().ToString();

                var sb =
                    new StringBuilder(
                        "insert into CpcResourceNickname values (@Id,@NickName,@CoperateProductCatalogId,@IsDeleted,@Language,@LocaleCode);");

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = connection.ExecuteAsync(sb.ToString(), new
                    {
                        Id,
                        CoperateProductCatalogId = cpcId,
                        name.NickName,
                        IsDeleted = false,
                        name.Language,
                        name.LocaleCode
                    }).Result;
                    connection.Close();
                }
            }
            else
            {
                var sb1 = new StringBuilder("select * from CpcResourceNickname where Id = @Id ");
                using (var connection = new SqlConnection(connectionString))
                {
                    var n = connection
                        .QuerySingleOrDefaultAsync<CpcResourceNickname>(sb1.ToString(), new { name.Id }).Result;

                    var sb2 = new StringBuilder(
                        "update CpcResourceNickname set NickName= @NickName,CoperateProductCatalogId= @CoperateProductCatalogId,Language=@Language,LocaleCode=@LocaleCode ");
                    sb2.Append(" where Id = @Id");
                    var affectedRows = connection.ExecuteAsync(sb2.ToString(), new
                    {
                        name.Id,
                        CoperateProductCatalogId = cpcId,
                        name.NickName,
                        name.Language,
                        name.LocaleCode
                    }).Result;
                    connection.Close();
                }
            }
    }

    private void CreateVendor(List<CpcVendorCreateDto> vendors, string connectionString, string cpcId)
    {
        foreach (var v in vendors)
            if (v.Id == null)
            {
                var Id = Guid.NewGuid().ToString();

                var sb = new StringBuilder("INSERT INTO CpcVendor VALUES (@Id" +
                                           ", @ResourceNumber" +
                                           ",@ResourceTitle" +
                                           ",@PurchasingUnit" +
                                           ",@ResourcePrice" +
                                           ",@ResourceLeadTime" +
                                           ",@MinOrderQuantity,@MaxOrderQuantity,@RoundingValue,@CoperateProductCatalogId,@IsDeleted,@PreferredParty,@CompanyId);");

                using var connection = new SqlConnection(connectionString);
                var affectedRows = connection.ExecuteAsync(sb.ToString(), new
                {
                    Id,
                    v.ResourceNumber,
                    v.ResourceTitle,
                    v.PurchasingUnit,
                    v.ResourcePrice,
                    v.ResourceLeadTime,
                    v.MinOrderQuantity,
                    v.MaxOrderQuantity,
                    v.RoundingValue,
                    CoperateProductCatalogId = cpcId,
                    v.PreferredParty,
                    v.CompanyId,
                    IsDeleted = false
                }).Result;
                connection.Close();
            }
            else
            {
                var sb1 = new StringBuilder("select * from CpcVendor where Id = @Id ");
                using var connection = new SqlConnection(connectionString);
                var n = connection
                    .QuerySingleOrDefaultAsync<CpcResourceNickname>(sb1.ToString(), new { v.Id }).Result;

                var sb2 = new StringBuilder(
                    "update CpcVendor set ResourceNumber=@ResourceNumber,ResourceTitle=@ResourceTitle,PurchasingUnit=@PurchasingUnit,ResourcePrice=@ResourcePrice,ResourceLeadTime=@ResourceLeadTime,MinOrderQuantity=@MinOrderQuantity,MaxOrderQuantity=@MaxOrderQuantity,RoundingValue=@RoundingValue,CoperateProductCatalogId=@CoperateProductCatalogId,PreferredParty=@PreferredParty,CompanyId=@CompanyId ");
                sb2.Append(" where Id = @Id");
                var affectedRows = connection.ExecuteAsync(sb2.ToString(), new
                {
                    v.Id,
                    v.ResourceNumber,
                    v.ResourceTitle,
                    v.PurchasingUnit,
                    v.ResourcePrice,
                    v.ResourceLeadTime,
                    v.MinOrderQuantity,
                    v.MaxOrderQuantity,
                    v.RoundingValue,
                    CoperateProductCatalogId = cpcId,
                    v.PreferredParty,
                    v.CompanyId
                }).Result;
                connection.Close();
            }
    }

    private async Task CreateHistory(CoperateProductCatalogCreateDto cpcDto, IHttpContextAccessor contextAccessor,
        string cpcId, CpcParameters cpcParameters)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        //var options1 = new DbContextOptions<ApplicationDbContext>();
        // var applicationDbContext =
        //     new ApplicationDbContext(options1, cpcParameters.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString, cpcParameters.TenantProvider))
        {
            //var oid = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var userId = applicationDbContext.ApplicationUser.First(u => u.OId == oid).OId;


            var jsonProject = JsonConvert.SerializeObject(cpcDto, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var history = new CpcHistoryLog();

            var isAdded = context.CpcHistoryLog.Any(x => x.CoperateProductCatalogId == cpcDto.Id);
            history.Action = isAdded == false ? HistoryState.ADDED.ToString() : HistoryState.UPDATED.ToString();

            history.ChangedByUserId = cpcParameters.Oid;
            history.Id = Guid.NewGuid().ToString();
            history.HistoryLog = jsonProject;
            history.ChangedTime = DateTime.UtcNow;
            history.CoperateProductCatalogId = cpcDto.Id;

            context.CpcHistoryLog.Add(history);
            await context.SaveChangesAsync();
        }
    }

    private async Task<IEnumerable<CpcBrand>> getCpcBrand(CpcParameters cpcParameters)
    {
        var lang = cpcParameters.Lang;
        //if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Equals("en-us")) lang = "en";

        var query = @"
                              select CpcBrandId as Id, Name from CpcBrand where LanguageCode = @lang order by Name
                             ";

        var parameters = new { lang };
        // await using (var dbConnection =
        //              new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        // {
        //     var result = await dbConnection.QueryAsync<CpcBrand>(query, parameters);
        //     
        //
        //     return result;
        // }
        
        return await cpcParameters.TenantProvider.orgSqlConnection().QueryAsync<CpcBrand>(query, parameters);

    }

    private async Task<IEnumerable<CpcResourceFamily>> getCpcResouceFamily(CpcParameters cpcParameters)
    {
       // var lang = ;
        //if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Equals("en-us")) lang = "en";

        var query = @"select CpcResourceFamilyId as Id, Label as Title, ParentId 
                              from CpcResourceFamilyLocalizedData
                              where LanguageCode = @lang
                              order by Title 
                             ";

        var parameters = new { lang = cpcParameters.Lang };
        // await using (var dbConnection =
        //              new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        // {
        //     var result = await dbConnection.QueryAsync<CpcResourceFamily>(query, parameters);
        //     
        //
        //     return result;
        // }
        return await cpcParameters.TenantProvider.orgSqlConnection().QueryAsync<CpcResourceFamily>(query, parameters);
    }

    private async Task<IEnumerable<CpcUnitOfSizeMeasure>> getCpcUnitOfSizeMeasure(CpcParameters cpcParameters)
    {
        var query =
            @"SELECT CpcBasicUnitOfMeasureLocalizedData.Label AS Name ,CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId AS Id
FROM dbo.CpcBasicUnitOfMeasureLocalizedData WHERE LanguageCode = @lang order by Name";

        // using (var dbConnection =
        //        new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        // {
        //     var result =
        //         await dbConnection.QueryAsync<CpcUnitOfSizeMeasure>(query,
        //             new { lang = cpcParameters.Lang });
        //     
        //
        //     return result;
        // }
        
        return await cpcParameters.TenantProvider.orgSqlConnection().QueryAsync<CpcUnitOfSizeMeasure>(query,  new {lang= cpcParameters.Lang });
    }

    private async Task<IEnumerable<CpcMaterial>> getCpcMaterial(CpcParameters cpcParameters)
    {
       // var lang = cpcParameters.Lang;

        var query = @"
                             select CpcMaterialId as Id, Label as Name  from CpcMaterialLocalizedData
                             where LanguageCode = @lang order by Label
                             ";

        //var parameters = new { lang };
        // await using (var dbConnection =
        //              new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        // {
        //     var result = await dbConnection.QueryAsync<CpcMaterial>(query, parameters);
        //     
        //
        //     return result;
        // }
        return await cpcParameters.TenantProvider.orgSqlConnection().QueryAsync<CpcMaterial>(query, new {lang = cpcParameters.Lang});

    }

    private async Task<IEnumerable<CpcPressureClass>> getCpcPressureClass(CpcParameters cpcParameters)
    {
        // var lang = cpcParameters.Lang;
        // if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Equals("en-us")) lang = "en";

        var query = @"select Id, Name from CpcPressureClass order by Name";

        // var parameters = new { cpcParameters.Lang };
        // await using (var dbConnection =
        //              new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        // {
        //     var result = await dbConnection.QueryAsync<CpcPressureClass>(query, parameters);
        //     
        //
        //     return result;
        // }
        
        return await cpcParameters.TenantProvider.orgSqlConnection().QueryAsync<CpcPressureClass>(query, new { lang = cpcParameters.Lang });
    }

    private async Task<IEnumerable<CpcBasicUnitOfMeasure>> getCpcBasicUnitOfMeasure(CpcParameters cpcParameters)
    {
        // var lang = cpcParameters.Lang;
        // if (string.IsNullOrEmpty(cpcParameters.Lang) || lang.ToLower().Equals("en-us")) lang = "en";

        var query = @"
                             select CpcBasicUnitOfMeasureId as Id, Label as Name from CpcBasicUnitOfMeasureLocalizedData
                             where LanguageCode = @lang
                             order by Label
                             ";

        //var parameters = new { lang };
        // using (var dbConnection =
        //        new SqlConnection(cpcParameters.TenantProvider.GetTenant().ConnectionString))
        // {
        //     var result = await dbConnection.QueryAsync<CpcBasicUnitOfMeasure>(query, parameters);
        //     
        //
        //     return result;
        // }
        
        return await cpcParameters.TenantProvider.orgSqlConnection().QueryAsync<CpcBasicUnitOfMeasure>(query, new {lang = cpcParameters.Lang});

    }

    public void CommonDelete(CpcParameters cpcParameters, string tableName)
    {
        var connectionString = ConnectionString.MapConnectionString(cpcParameters.ContractingUnitSequenceId,
            cpcParameters.ProjectSequenceId, cpcParameters.TenantProvider);
        foreach (var id in cpcParameters.IdList)
        {
            using var dbConnection = new SqlConnection(connectionString);
            dbConnection.Execute("update " + tableName + " set IsDeleted = @val where Id = @id",
                new { val = true, id });

            
        }
    }

    private string GenerateHeaderTitle(CorporateProductCatalogDto dto)
    {
        var headerTitle = dto.ResourceTitle;
        if (dto.Size != null) headerTitle += " - " + dto.Size;

        if (dto.CpcBrand != null) headerTitle += " - " + dto.CpcBrand.Text;

        if (dto.CpcMaterial is { Key: { } }) headerTitle += " - " + dto.CpcMaterial.Text;

        if (dto.CpcBasicUnitOfMeasure != null) headerTitle += " - " + dto.CpcBasicUnitOfMeasure.Text;

        return headerTitle;
    }
    
    public async Task<IEnumerable<CoperateProductCatalogFilterNewDto>> CpcLobourFilterMyEnv(CpcParameters cpcParameters)
    {
        const string sql = @"SELECT
                  CorporateProductCatalog.Id
                 ,CorporateProductCatalog.ResourceTitle
                 ,CorporateProductCatalog.Status AS Status
                 ,CorporateProductCatalog.ResourceNumber AS ResourceNumber
                 ,CpcResourceTypeLocalizedData.Label AS ResourceType
                 ,CorporateProductCatalog.Title AS Title
                 ,CorporateProductCatalog.CpcBasicUnitOfMeasureId AS MouId
                 ,CONCAT(CorporateProductCatalog.ResourceNumber, ' - ', CorporateProductCatalog.ResourceTitle) AS HeaderTitle
                FROM dbo.CpcResourceTypeLocalizedData
                    ,dbo.CorporateProductCatalog
                     INNER JOIN dbo.CpcResourceType
                       ON CorporateProductCatalog.ResourceTypeId = CpcResourceType.Id
                WHERE CorporateProductCatalog.IsDeleted = 0
                AND CpcResourceType.Id = CorporateProductCatalog.ResourceTypeId
                AND CpcResourceTypeLocalizedData.CpcResourceTypeId = CpcResourceType.Id
                AND CpcResourceTypeLocalizedData.LanguageCode = @lang
                AND CorporateProductCatalog.ResourceTypeId = 'c46c3a26-39a5-42cc-b07s-89655304eh6'";
        
        
        var sb = new StringBuilder(sql);

        if (cpcParameters.CpcLobourFilterMyEnvDto != null)
        {
            var words = cpcParameters.CpcLobourFilterMyEnvDto.Title.Split(" ");
            foreach (var element in words)
                sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
        }

        var connectionString = ConnectionString.MapConnectionString(
            cpcParameters.CpcLobourFilterMyEnvDto?.Cu, cpcParameters.CpcLobourFilterMyEnvDto?.ProjectSequenceId,
            cpcParameters.TenantProvider);
        await using var dbConnection = new SqlConnection(connectionString);
        var result = dbConnection.Query<CoperateProductCatalogFilterNewDto>(sb.ToString(), new { cpcParameters.Lang });
        return result;
    }
}