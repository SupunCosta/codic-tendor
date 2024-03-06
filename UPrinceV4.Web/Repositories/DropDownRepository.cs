using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.DD;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.Stock;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class DropDownRepository : IDropDownRepository
{
    public async Task<DropDownList> DropDownFilter(DDParameter DDParameter)
    {
        var mDropDownList = new DropDownList();
        mDropDownList.CoporateProductCatalog = FilterCPC(DDParameter).Result;
        mDropDownList.Project = FilterProject(DDParameter).Result;
        mDropDownList.ProjectBreakDownStructure = FilterPBS(DDParameter).Result;
        mDropDownList.ProjectMolecule = FilterProjectMolecule(DDParameter).Result;
        return mDropDownList;
    }


    public async Task<CoporateProductCatalog> FilterCPC(DDParameter DDParameter)
    {
        var query =
            @"SELECT Distinct CpcBasicUnitOfMeasureLocalizedData.LanguageCode, Language.Country , Language.Lang As Language FROM dbo.Language INNER JOIN dbo.CpcBasicUnitOfMeasureLocalizedData ON Language.Code = CpcBasicUnitOfMeasureLocalizedData.LanguageCode;
                                 SELECT Distinct CpcResourceFamilyLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.CpcResourceFamilyLocalizedData ON Language.Code = CpcResourceFamilyLocalizedData.LanguageCode;
                                 SELECT Distinct CpcResourceTypeLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.CpcResourceTypeLocalizedData ON Language.Code = CpcResourceTypeLocalizedData.LanguageCode;
                                 SELECT Distinct CpcMaterialLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.CpcMaterialLocalizedData ON Language.Code = CpcMaterialLocalizedData.LanguageCode;
                                 SELECT Distinct CpcBrand.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.CpcBrand ON Language.Code = CpcBrand.LanguageCode";

        var mCoporateProductCatalog = new CoporateProductCatalog();

        var parameters = new { lang = DDParameter.Lang };

        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var muilti = await connection.QueryMultipleAsync(query, parameters);
            mCoporateProductCatalog.CpcBasicUnitOfMeasure = muilti.Read<CpcBasicUnitOfMeasure>();
            mCoporateProductCatalog.CpcResourceFamily = muilti.Read<CpcResourceFamily>();
            mCoporateProductCatalog.CpcResourceType = muilti.Read<CpcResourceType>();
            mCoporateProductCatalog.CpcMaterial = muilti.Read<CpcMaterial>();
            mCoporateProductCatalog.CpcBrand = muilti.Read<CpcBrand>();
        }

        return mCoporateProductCatalog;
    }

    public async Task<ProjectBreakDownStructure> FilterPBS(DDParameter DDParameter)
    {
        var query =
            @"SELECT Distinct PbsToleranceStateLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.PbsToleranceStateLocalizedData ON Language.Code = PbsToleranceStateLocalizedData.LanguageCode;
                             SELECT Distinct PbsProductStatusLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.PbsProductStatusLocalizedData ON Language.Code = PbsProductStatusLocalizedData.LanguageCode;
                             SELECT Distinct PbsProductItemTypeLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.PbsProductItemTypeLocalizedData ON Language.Code = PbsProductItemTypeLocalizedData.LanguageCode;
                             SELECT Distinct PbsExperienceLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.PbsExperienceLocalizedData ON Language.Code = PbsExperienceLocalizedData.LanguageCode;
                             SELECT Distinct PbsSkillLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.PbsSkillLocalizedData ON Language.Code = PbsSkillLocalizedData.LanguageCode";

        var mProjectBreakDownStructure = new ProjectBreakDownStructure();

        var parameters = new { lang = DDParameter.Lang };

        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var muilti = await connection.QueryMultipleAsync(query, parameters);
            mProjectBreakDownStructure.ProjectBreakDownStructureToleranceState =
                muilti.Read<ProjectBreakDownStructureToleranceState>();
            mProjectBreakDownStructure.ProductState = muilti.Read<ProductState>();
            mProjectBreakDownStructure.ItemTypes = muilti.Read<ItemTypes>();
            mProjectBreakDownStructure.Experions = muilti.Read<Experions>();
            mProjectBreakDownStructure.Skills = muilti.Read<Skills>();
        }

        return mProjectBreakDownStructure;
    }

    public async Task<Project> FilterProject(DDParameter DDParameter)
    {
        var query =
            @"SELECT Distinct ProjectManagementLevelLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.ProjectManagementLevelLocalizedData ON Language.Code = ProjectManagementLevelLocalizedData.LanguageCode;
                             SELECT Distinct ProjectToleranceStateLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.ProjectToleranceStateLocalizedData ON Language.Code = ProjectToleranceStateLocalizedData.LanguageCode;
                             SELECT Distinct ProjectTypeLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.ProjectTypeLocalizedData ON Language.Code = ProjectTypeLocalizedData.LanguageCode;
                             SELECT Distinct ProjectTemplateLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.ProjectTemplateLocalizedData ON Language.Code = ProjectTemplateLocalizedData.LanguageCode;
                             SELECT Distinct ProjectStateLocalizedData.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.ProjectStateLocalizedData ON Language.Code = ProjectStateLocalizedData.LanguageCode";

        var mProject = new Project();

        var parameters = new { lang = DDParameter.Lang };

        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var muilti = await connection.QueryMultipleAsync(query, parameters);
            mProject.MagementLevel = muilti.Read<MagementLevel>();
            mProject.ToleranceState = muilti.Read<ToleranceState>();
            mProject.Types = muilti.Read<Types>();
            mProject.Templates = muilti.Read<Templates>();
            mProject.States = muilti.Read<States>();
        }

        return mProject;
    }

    public async Task<ProjectMolecule> FilterProjectMolecule(DDParameter DDParameter)
    {
        var query =
            @"SELECT Distinct PMolStatus.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.PMolStatus ON Language.Code = PMolStatus.LanguageCode;
                             SELECT Distinct PMolType.LanguageCode,Language.Country,Language.Lang FROM dbo.Language INNER JOIN dbo.PMolType ON Language.Code = PMolType.LanguageCode";

        var mProjectMolecule = new ProjectMolecule();

        var parameters = new { lang = DDParameter.Lang };

        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var muilti = await connection.QueryMultipleAsync(query, parameters);
            mProjectMolecule.Staus = muilti.Read<ProjectMoleculeStaus>();
            mProjectMolecule.Type = muilti.Read<ProjectMoleculeType>();
        }

        return mProjectMolecule;
    }

    public async Task<IEnumerable<RoleDto>> GetProjectRoleByCode(DDParameter DDParameter)
    {
        IEnumerable<RoleDto> role = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            role = connection.Query<RoleDto>(
                "SELECT Id, TenantId, LanguageCode,RoleName AS Label ,RoleId FROM Role WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in role)
        {
            string roleName;
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                roleName = connection
                    .Query<string>("SELECT RoleName FROM Role WHERE LanguageCode = 'en' AND RoleId =@RoleId ",
                        new { r.RoleId }).FirstOrDefault();
            }

            r.Name = roleName ?? r.Label;
        }

        return role;
    }

    public async Task<IEnumerable<DatabasesException>> AddProjectRole(DDParameter DDParameter)
    {
        string roleId;
        string newRoleId;
        IEnumerable<DatabasesException> exceptionLst = null;
        if (DDParameter.DdDto.Id == null)
        {
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                roleId = connection.Query<string>("SELECT RoleId FROM Role WHERE RoleName = @RoleName ",
                    new { RoleName = DDParameter.DdDto.Name }).FirstOrDefault();
            }

            newRoleId = roleId ?? Guid.NewGuid().ToString();

            var insertQuery =
                @"INSERT INTO [dbo].[Role] ([Id], [TenantId], [RoleName], [RoleId], [LanguageCode]) VALUES (@Id, @TenantId, @RoleName, @RoleId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                TenantId = 1,
                RoleName = DDParameter.DdDto.Label,
                RoleId = newRoleId,
                DDParameter.DdDto.LanguageCode
            };


            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[Role] SET [RoleName] = @RoleName WHERE Id = @Id";


            var parameters = new { DDParameter.DdDto.Id, RoleName = DDParameter.DdDto.Label };


            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteProjectRole(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[Role] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<StockDropdownAddDto>> GetStockResourceByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,TypeId FROM StockType WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>("SELECT Name FROM StockType WHERE LanguageCode = 'en' AND TypeId =@TypeId ",
                        new { r.TypeId }).FirstOrDefault();
            }

            r.Name = name ?? r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddStockResource(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection.Query<StockDropdownAddDto>("SELECT * FROM StockType WHERE Name = @Name ",
                    new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                await using (var connection =
                             new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder =
                        connection.Query<int>("select max(displayOrder) from StockType").FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[StockType] ([Id], [DisplayOrder], [Name], [TypeId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @TypeId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                TypeId = newTypeId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[StockType] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteStockResource(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[StockType] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<StockDropdownAddDto>> GetStockTypeByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,TypeId FROM StockActiveType WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM StockActiveType WHERE LanguageCode = 'en' AND TypeId =@TypeId ",
                        new { r.TypeId }).FirstOrDefault();
            }

            r.Name = name ?? r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddStockType(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection.Query<StockDropdownAddDto>("SELECT * FROM StockActiveType WHERE Name = @Name ",
                    new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                await using (var connection =
                             new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from StockActiveType")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[StockActiveType] ([Id], [DisplayOrder], [Name], [TypeId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @TypeId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                TypeId = newTypeId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[StockActiveType] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteStockType(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[StockActiveType] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }


    public async Task<IEnumerable<StockDropdownAddDto>> GetStockStatusByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,StatusId FROM StockStatus WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM StockStatus WHERE LanguageCode = 'en' AND StatusId =@StatusId ",
                        new { r.StatusId }).FirstOrDefault();
            }

            r.Name = name ?? r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddStockStatus(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection.Query<StockDropdownAddDto>("SELECT * FROM StockStatus WHERE Name = @Name ",
                    new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.StatusId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from StockStatus")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[StockStatus] ([Id], [DisplayOrder], [Name], [StatusId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @StatusId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                StatusId = newTypeId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[StockStatus] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteStockStatus(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[StockStatus] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }


    public async Task<IEnumerable<StockDropdownAddDto>> GetWFStatusByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,StatusId FROM WFActivityStatus WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM WFActivityStatus WHERE LanguageCode = 'en' AND StatusId =@StatusId ",
                        new { r.StatusId }).FirstOrDefault();
            }

            r.Name = name ?? r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddWFStatus(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection.Query<StockDropdownAddDto>("SELECT * FROM WFActivityStatus WHERE Name = @Name ",
                    new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.StatusId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from WFActivityStatus")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[WFActivityStatus] ([Id], [DisplayOrder], [Name], [StatusId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @StatusId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                StatusId = newTypeId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[WFActivityStatus] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteWFStatus(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[WFActivityStatus] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }


    public async Task<IEnumerable<StockDropdownAddDto>> GetWFTypeByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,TypeId FROM WFType WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>("SELECT Name FROM WFType WHERE LanguageCode = 'en' AND TypeId =@TypeId ",
                        new { r.TypeId }).FirstOrDefault();
            }

            if (name != null)
                r.Name = name;
            else
                r.Name = r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddWFType(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection.Query<StockDropdownAddDto>("SELECT * FROM WFType WHERE Name = @Name ",
                    new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from WFType").FirstOrDefault() +
                                   1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[WFType] ([Id], [DisplayOrder], [Name], [TypeId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @TypeId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                TypeId = newTypeId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[WFType] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteWFType(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[WFType] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<StockDropdownAddDto>> GetWHTypeByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,TypeId FROM WHType WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>("SELECT Name FROM WHType WHERE LanguageCode = 'en' AND TypeId =@TypeId ",
                        new { r.TypeId }).FirstOrDefault();
            }

            r.Name = name ?? r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddWHType(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection.Query<StockDropdownAddDto>("SELECT * FROM WHType WHERE Name = @Name ",
                    new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from WHType").FirstOrDefault() +
                                   1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[WHType] ([Id], [DisplayOrder], [Name], [TypeId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @TypeId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                TypeId = newTypeId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[WHType] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteWHType(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[WHType] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<StockDropdownAddDto>> GetWHStatusByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        await using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,StatusId FROM WHStatus WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>("SELECT Name FROM WHStatus WHERE LanguageCode = 'en' AND StatusId =@StatusId ",
                        new { r.StatusId }).FirstOrDefault();
            }

            r.Name = name ?? r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddWHStatus(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection.Query<StockDropdownAddDto>("SELECT * FROM WHStatus WHERE Name = @Name ",
                    new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.StatusId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder =
                        connection.Query<int>("select max(displayOrder) from WHStatus").FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[WHStatus] ([Id], [DisplayOrder], [Name], [StatusId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @StatusId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                StatusId = newTypeId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[WHStatus] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteWHStatus(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[WHStatus] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<StockDropdownAddDto>> GetWHTaxonomyLevelByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId As TypeId FROM WHTaxonomyLevel WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM WHTaxonomyLevel WHERE LanguageCode = 'en' AND LevelId =@TypeId ",
                        new { r.TypeId }).FirstOrDefault();
            }

            if (name != null)
                r.Name = name;
            else
                r.Name = r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddWHTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection
                    .Query<StockDropdownAddDto>(
                        "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId As TypeId FROM WHTaxonomyLevel WHERE Name = @Name ",
                        new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from WHTaxonomyLevel")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[WHTaxonomyLevel] ([Id], [DisplayOrder], [Name], [LevelId], [LanguageCode], [IsChildren]) VALUES (@Id, @DisplayOrder, @Name, @TypeId, @LanguageCode, @IsChildren);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                TypeId = newTypeId,
                DDParameter.Dto.LanguageCode,
                IsChildren = 1
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[WHTaxonomyLevel] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteWHTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[WHTaxonomyLevel] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }


    public async Task<List<DatabasesException>> Migration(string env, string sql, object param)
    {
        // var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0009\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0020\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"P0055\"},{\"DatabaseName\":\"P0056\"},{\"DatabaseName\":\"P0057\"},{\"DatabaseName\":\"P0058\"},{\"DatabaseName\":\"P0059\"},{\"DatabaseName\":\"P0060\"},{\"DatabaseName\":\"P0061\"},{\"DatabaseName\":\"P0062\"},{\"DatabaseName\":\"P0063\"},{\"DatabaseName\":\"P0064\"},{\"DatabaseName\":\"P0065\"},{\"DatabaseName\":\"P0066\"},{\"DatabaseName\":\"P0067\"},{\"DatabaseName\":\"P0068\"},{\"DatabaseName\":\"P0069\"},{\"DatabaseName\":\"P0070\"},{\"DatabaseName\":\"P0071\"},{\"DatabaseName\":\"P0072\"},{\"DatabaseName\":\"P0073\"},{\"DatabaseName\":\"P0074\"},{\"DatabaseName\":\"P0075\"},{\"DatabaseName\":\"P0076\"},{\"DatabaseName\":\"P0077\"},{\"DatabaseName\":\"P0078\"},{\"DatabaseName\":\"P0079\"},{\"DatabaseName\":\"P0080\"},{\"DatabaseName\":\"P0081\"},{\"DatabaseName\":\"P0082\"},{\"DatabaseName\":\"P0083\"},{\"DatabaseName\":\"P0084\"},{\"DatabaseName\":\"P0085\"},{\"DatabaseName\":\"P0086\"},{\"DatabaseName\":\"P0087\"},{\"DatabaseName\":\"P0088\"},{\"DatabaseName\":\"P0089\"},{\"DatabaseName\":\"P0090\"},{\"DatabaseName\":\"P0091\"},{\"DatabaseName\":\"P0092\"},{\"DatabaseName\":\"P0093\"},{\"DatabaseName\":\"P0094\"},{\"DatabaseName\":\"P0095\"},{\"DatabaseName\":\"P0096\"},{\"DatabaseName\":\"P0097\"},{\"DatabaseName\":\"P0098\"},{\"DatabaseName\":\"P0099\"},{\"DatabaseName\":\"P0100\"},{\"DatabaseName\":\"P0101\"},{\"DatabaseName\":\"P0102\"},{\"DatabaseName\":\"P0103\"},{\"DatabaseName\":\"P0104\"},{\"DatabaseName\":\"P0105\"},{\"DatabaseName\":\"P0106\"},{\"DatabaseName\":\"P0107\"},{\"DatabaseName\":\"UPrinceV4Einstein\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"}]";
        //var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"COM0053\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"},{\"DatabaseName\":\"UPrinceV4UAT\"}]";
        var result = new List<Databases>();
        //uprincev4uatdb
        //uprincev4einstein
        // var env = "uprincev4einstein";
        var exceptionLst = new List<DatabasesException>();
        using (var connection = new SqlConnection("Server=tcp:" + env +
                                                  ".database.windows.net,1433;Initial Catalog=master;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        {
            result = connection
                .Query<Databases>(
                    @"select [name] as DatabaseName from sys.databases WHERE name NOT IN('master', 'MsalTokenCacheDatabase', 'UPrinceV4EinsteinCatelog', 'UPrinceV4UATCatelog') order by name")
                .ToList();
        }


        foreach (var project in result)
        {
            var connectionString = "Server=tcp:" + env + ".database.windows.net,1433;Initial Catalog=" +
                                   project.DatabaseName +
                                   ";Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            {
                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.ExecuteAsync(sql, param);
                    }
                    catch (Exception ex)
                    {
                        var mDatabasesException = new DatabasesException();
                        mDatabasesException.DatabaseName = project.DatabaseName;
                        mDatabasesException.Exception = ex;
                        exceptionLst.Add(mDatabasesException);
                    }
                }
            }
        }

        return exceptionLst;
    }


    public async Task<IEnumerable<VPOrganisationShortcutPaneDto>> GetVPOrganisationShortcutPaneByCode(
        DDParameter DDParameter)
    {
        IEnumerable<VPOrganisationShortcutPaneDto> dropdown = null;
        using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<VPOrganisationShortcutPaneDto>(
                "SELECT Id,Name AS Label,LanguageCode,DisplayOrder,OrganisationId FROM dbo.VPOrganisationShortcutPane WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM VPOrganisationShortcutPane WHERE LanguageCode = 'en' AND OrganisationId = @OrganisationId ",
                        new { r.OrganisationId }).FirstOrDefault();
            }

            if (name != null)
                r.Name = name;
            else
                r.Name = r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddVPOrganisationShortcutPane(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        VPOrganisationShortcutPaneDto item;
        string newOrganisationId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection
                    .Query<VPOrganisationShortcutPaneDto>(
                        "SELECT * FROM VPOrganisationShortcutPane WHERE Name = @Name ",
                        new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newOrganisationId = item.OrganisationId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newOrganisationId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from VPOrganisationShortcutPane")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[VPOrganisationShortcutPane] ([Id], [DisplayOrder], [Name], [OrganisationId], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @OrganisationId, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                OrganisationId = newOrganisationId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[VPOrganisationShortcutPane] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteVPOrganisationShortcutPane(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[VPOrganisationShortcutPane] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<OrganizationTaxonomyLevelDropdown>> GetOrganizationTaxonomyLevelByCode(
        DDParameter DDParameter)
    {
        IEnumerable<OrganizationTaxonomyLevelDropdown> dropdown = null;
        using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<OrganizationTaxonomyLevelDropdown>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId FROM OrganizationTaxonomyLevel WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM OrganizationTaxonomyLevel WHERE LanguageCode = 'en' AND LevelId =@LevelId ",
                        new { r.LevelId }).FirstOrDefault();
            }

            if (name != null)
                r.Name = name;
            else
                r.Name = r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddOrganizationTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.LevelCreateDto.Id == null)
        {
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection
                    .Query<StockDropdownAddDto>(
                        "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId As TypeId FROM OrganizationTaxonomyLevel WHERE Name = @Name ",
                        new { DDParameter.LevelCreateDto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from OrganizationTaxonomyLevel")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[OrganizationTaxonomyLevel] ([Id], [DisplayOrder], [Name], [LevelId], [LanguageCode], [IsChildren]) VALUES (@Id, @DisplayOrder, @Name, @LevelId, @LanguageCode, @IsChildren);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.LevelCreateDto.Label,
                LevelId = newTypeId,
                DDParameter.LevelCreateDto.LanguageCode,
                IsChildren = 1
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[OrganizationTaxonomyLevel] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.LevelCreateDto.Id, Name = DDParameter.LevelCreateDto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteOrganizationTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[OrganizationTaxonomyLevel] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<StockDropdownAddDto>> GetPOShortcutPaneByCode(DDParameter DDParameter)
    {
        IEnumerable<StockDropdownAddDto> dropdown = null;
        using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<StockDropdownAddDto>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,Type AS TypeId  FROM POShortcutpaneData WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM POShortcutpaneData WHERE LanguageCode = 'en' AND Type =@TypeId ",
                        new { r.TypeId }).FirstOrDefault();
            }

            if (name != null)
                r.Name = name;
            else
                r.Name = r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddPOShortcutPane(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        POShortcutpaneDataDto item;
        string newOrganisationId;
        int displayOrder;
        if (DDParameter.Dto.Id == null)
        {
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection
                    .Query<POShortcutpaneDataDto>("SELECT * FROM POShortcutpaneData WHERE Name = @Name ",
                        new { DDParameter.Dto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newOrganisationId = item.Type;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newOrganisationId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from POShortcutpaneData")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[POShortcutpaneData] ([Id], [DisplayOrder], [Name], [Type], [LanguageCode]) VALUES (@Id, @DisplayOrder, @Name, @Type, @LanguageCode);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.Dto.Label,
                Type = newOrganisationId,
                DDParameter.Dto.LanguageCode
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[POShortcutpaneData] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.Dto.Id, Name = DDParameter.Dto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeletePOShortcutPane(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[POShortcutpaneData] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<OrganizationTaxonomyLevelDropdown>> GetCompetenciesTaxonomyLevelByCode(
        DDParameter DDParameter)
    {
        IEnumerable<OrganizationTaxonomyLevelDropdown> dropdown = null;
        using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<OrganizationTaxonomyLevelDropdown>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId FROM CompetenciesTaxonomyLevel WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM CompetenciesTaxonomyLevel WHERE LanguageCode = 'en' AND LevelId =@LevelId ",
                        new { r.LevelId }).FirstOrDefault();
            }

            if (name != null)
                r.Name = name;
            else
                r.Name = r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddCompetenciesTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.LevelCreateDto.Id == null)
        {
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection
                    .Query<StockDropdownAddDto>(
                        "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId As TypeId FROM CompetenciesTaxonomyLevel WHERE Name = @Name ",
                        new { DDParameter.LevelCreateDto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from CompetenciesTaxonomyLevel")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[CompetenciesTaxonomyLevel] ([Id], [DisplayOrder], [Name], [LevelId], [LanguageCode], [IsChildren]) VALUES (@Id, @DisplayOrder, @Name, @LevelId, @LanguageCode, @IsChildren);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.LevelCreateDto.Label,
                LevelId = newTypeId,
                DDParameter.LevelCreateDto.LanguageCode,
                IsChildren = 1
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[CompetenciesTaxonomyLevel] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.LevelCreateDto.Id, Name = DDParameter.LevelCreateDto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteCompetenciesTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[CompetenciesTaxonomyLevel] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<OrganizationTaxonomyLevelDropdown>> GetCertificationTaxonomyLevelByCode(
        DDParameter DDParameter)
    {
        IEnumerable<OrganizationTaxonomyLevelDropdown> dropdown = null;
        using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
        {
            dropdown = connection.Query<OrganizationTaxonomyLevelDropdown>(
                "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId FROM CertificationTaxonomyLevel WHERE LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }

        foreach (var r in dropdown)
        {
            string name;
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                name = connection
                    .Query<string>(
                        "SELECT Name FROM CertificationTaxonomyLevel WHERE LanguageCode = 'en' AND LevelId =@LevelId ",
                        new { r.LevelId }).FirstOrDefault();
            }

            if (name != null)
                r.Name = name;
            else
                r.Name = r.Label;
        }

        return dropdown;
    }

    public async Task<IEnumerable<DatabasesException>> AddCertificationTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        StockDropdownAddDto item;
        string newTypeId;
        int displayOrder;
        if (DDParameter.LevelCreateDto.Id == null)
        {
            using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            {
                item = connection
                    .Query<StockDropdownAddDto>(
                        "SELECT Id, DisplayOrder, LanguageCode,Name AS Label ,LevelId As TypeId FROM CertificationTaxonomyLevel WHERE Name = @Name ",
                        new { DDParameter.LevelCreateDto.Name }).FirstOrDefault();
            }

            if (item != null)
            {
                newTypeId = item.TypeId;
                displayOrder = item.DisplayOrder;
            }
            else
            {
                newTypeId = Guid.NewGuid().ToString();
                using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    displayOrder = connection.Query<int>("select max(displayOrder) from CertificationTaxonomyLevel")
                        .FirstOrDefault() + 1;
                }
            }

            var insertQuery =
                @"INSERT INTO [dbo].[CertificationTaxonomyLevel] ([Id], [DisplayOrder], [Name], [LevelId], [LanguageCode], [IsChildren]) VALUES (@Id, @DisplayOrder, @Name, @LevelId, @LanguageCode, @IsChildren);";
            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DisplayOrder = displayOrder,
                Name = DDParameter.LevelCreateDto.Label,
                LevelId = newTypeId,
                DDParameter.LevelCreateDto.LanguageCode,
                IsChildren = 1
            };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", insertQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", insertQuery, parameters);
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[CertificationTaxonomyLevel] SET [Name] = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.LevelCreateDto.Id, Name = DDParameter.LevelCreateDto.Label };

            if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
                exceptionLst = await Migration("uprincev4uatdb", updateQuery, parameters);
            else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
                exceptionLst = await Migration("uprincev4einstein", updateQuery, parameters);
        }

        return exceptionLst;
    }

    public async Task<IEnumerable<DatabasesException>> DeleteCertificationTaxonomyLevel(DDParameter DDParameter)
    {
        IEnumerable<DatabasesException> exceptionLst = null;
        var deleteQuery = @"DELETE FROM [dbo].[CertificationTaxonomyLevel] WHERE Id =@Id";

        var parameters = new { Id = DDParameter.Code };


        if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4uatdb"))
            exceptionLst = await Migration("uprincev4uatdb", deleteQuery, parameters);
        else if (DDParameter.TenantProvider.GetTenant().ConnectionString.Contains("uprincev4einstein"))
            exceptionLst = await Migration("uprincev4einstein", deleteQuery, parameters);


        return exceptionLst;
    }

    public async Task<IEnumerable<GetPOTypeDto>> GetPoTypeByCode(DDParameter DDParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(DDParameter.ContractingUnitSequenceId,
            DDParameter.ProjectSequenceId, DDParameter.TenantProvider);

        IEnumerable<GetPOTypeDto> dropdown = null;
        using (var connection = new SqlConnection(connectionString))
        {
            dropdown = connection.Query<GetPOTypeDto>(
                "SELECT * FROM [dbo].[POType] where LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }


        return dropdown;
    }

    public async Task<IEnumerable<GetPOStatusDto>> GetPoStatusByCode(DDParameter DDParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(DDParameter.ContractingUnitSequenceId,
            DDParameter.ProjectSequenceId, DDParameter.TenantProvider);

        IEnumerable<GetPOStatusDto> dropdown = null;
        using (var connection = new SqlConnection(connectionString))
        {
            dropdown = connection.Query<GetPOStatusDto>(
                "SELECT * FROM [dbo].[POStatus] where LanguageCode = @lang ",
                new { lang = DDParameter.Code });
        }


        return dropdown;
    }

    public async Task<string> AddPoType(DDParameter DDParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(DDParameter.ContractingUnitSequenceId,
            DDParameter.ProjectSequenceId, DDParameter.TenantProvider);

        GetPOTypeDto item;

        string newTypeId;
        int displayOrder;
        if (DDParameter.CreatePOType.Id == null)
        {
            //using (var connection = new SqlConnection(connectionString))
            //{

            //    item = connection
            //        .Query<GetPOTypeDto>(
            //            "SELECT * FROM [POType] WHERE Name = @Name ",
            //            new { DDParameter.CreatePOType.Name }).FirstOrDefault();
            //}

            //if (item != null)
            //{
            //    newTypeId = item.TypeId;
            //    displayOrder = item.DisplayOrder;
            //}
            //else
            //{
            //    newTypeId = Guid.NewGuid().ToString();
            //    using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            //    {
            //        displayOrder = connection.Query<int>("select max(displayOrder) from CertificationTaxonomyLevel")
            //            .FirstOrDefault() + 1;
            //    }
            //}

            var insertQuery =
                @"INSERT INTO [dbo].[POType]
           ([Id]
           ,[Name]
           ,[LanguageCode]
           ,[TypeId]
           ,[DisplayOrder])
            VALUES
           (@Id
           ,@Name
           ,@LanguageCode
           ,@TypeId
           ,@DisplayOrder)";

            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DDParameter.CreatePOType.DisplayOrder,
                DDParameter.CreatePOType.Name,
                DDParameter.CreatePOType.TypeId,
                DDParameter.CreatePOType.LanguageCode
            };

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(insertQuery, parameters);
            }

            return newId;
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[POType] SET Name = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.CreatePOType.Id, DDParameter.CreatePOType.Name };

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(updateQuery, parameters);
            }

            return DDParameter.CreatePOType.Id;
        }
    }

    public async Task<string> AddPoStatus(DDParameter DDParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(DDParameter.ContractingUnitSequenceId,
            DDParameter.ProjectSequenceId, DDParameter.TenantProvider);

        GetPOStatusDto item;

        string newStatusId;
        int displayOrder;
        if (DDParameter.CreatePOStatus.Id == null)
        {
            //using (var connection = new SqlConnection(connectionString))
            //{

            //    item = connection
            //        .Query<GetPOTypeDto>(
            //            "SELECT * FROM [POType] WHERE Name = @Name ",
            //            new { DDParameter.CreatePOType.Name }).FirstOrDefault();
            //}

            //if (item != null)
            //{
            //    newTypeId = item.TypeId;
            //    displayOrder = item.DisplayOrder;
            //}
            //else
            //{
            //    newTypeId = Guid.NewGuid().ToString();
            //    using (var connection = new SqlConnection(DDParameter.TenantProvider.GetTenant().ConnectionString))
            //    {
            //        displayOrder = connection.Query<int>("select max(displayOrder) from CertificationTaxonomyLevel")
            //            .FirstOrDefault() + 1;
            //    }
            //}

            var insertQuery =
                @"INSERT INTO [dbo].[POStatus]
           ([Id]
           ,[Name]
           ,[LanguageCode]
           ,[StatusId]
           ,[DisplayOrder])
            VALUES
           (@Id
           ,@Name
           ,@LanguageCode
           ,@StatusId
           ,@DisplayOrder)";

            var newId = Guid.NewGuid().ToString();
            var parameters = new
            {
                Id = newId,
                DDParameter.CreatePOStatus.DisplayOrder,
                DDParameter.CreatePOStatus.Name,
                DDParameter.CreatePOStatus.StatusId,
                DDParameter.CreatePOStatus.LanguageCode
            };

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(insertQuery, parameters);
            }

            return newId;
        }
        else
        {
            var updateQuery = @"UPDATE [dbo].[POStatus] SET Name = @Name WHERE Id = @Id";


            var parameters = new { DDParameter.CreatePOStatus.Id, DDParameter.CreatePOStatus.Name };

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(updateQuery, parameters);
            }

            return DDParameter.CreatePOStatus.Id;
        }
    }
}