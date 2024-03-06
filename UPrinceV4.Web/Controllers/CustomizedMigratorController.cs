using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Controllers;

//[Microsoft.AspNetCore.Authorization.Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomizedMigratorController
{
    private readonly ITenantProvider _TenantProvider;

    public CustomizedMigratorController(ITenantProvider tenantProvider)
    {
        _TenantProvider = tenantProvider;
    }

    [HttpGet("MigrateDb")]
    public void MigrateDb()
    {
        try
        {
            //string orgCon = "Server=tcp:upuatprod.database.windows.net,1433;Initial Catalog=UPrinceV4DevTest; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;";
            //string catelogCon = "Server = tcp:upuatprod.database.windows.net,1433; Initial Catalog = UPrinceV4DevCatelog; Persist Security Info = False; User ID = uprincedbuser; Password = UPrince2017!; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; MultipleActiveResultSets = true; ";

            var orgCon =
                "Server=tcp:uprincev4uatdb.database.windows.net,1433;Initial Catalog=UPrinceV4UAT;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var catelogCon =
                "Server=tcp:uprincev4uatdb.database.windows.net,1433;Initial Catalog=UPrinceV4UATCatelog;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var connectionStrings = new List<string>();
            var queryProjects = @"
                                select ProjectConnectionString from ProjectDefinition where ProjectConnectionString is not null
                             ";

            var queryCu = @"
                                select ConnectionString from [dbo].[UPrinceCustomerContractingUnit]
                             ";

            using (var dbConnection = new SqlConnection(orgCon))
            {
                connectionStrings = (List<string>)dbConnection.Query<string>(queryProjects);
            }

            List<string> cu;
            using (var dbConnection = new SqlConnection(catelogCon))
            {
                cu = (List<string>)dbConnection.Query<string>(queryCu);
            }

            foreach (var s in cu) connectionStrings.Append(s);

            connectionStrings.Append(orgCon);
            // connectionStrings.Append("Server=tcp:upuatprod.database.windows.net,1433;Initial Catalog=UPrinceV4ProjectTemplate; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;");
            connectionStrings.Append(
                "Server=tcp:uprincev4uatdb.database.windows.net,1433;Initial Catalog=UPrinceV4ProjectTemplate;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            var sqlListQuery = @"
                                select sql from [dbo].[DbUpdator] where ExecutionStatus = 0 order by Id
                             ";

            var test = @"DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PMol]') AND [c].[name] = N'ProjectMoleculeId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PMol] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [PMol] ALTER COLUMN [ProjectMoleculeId] nvarchar(450) NULL

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PbsProduct]') AND [c].[name] = N'ProductId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PbsProduct] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [PbsProduct] ALTER COLUMN [ProductId] nvarchar(450) NULL

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Bor]') AND [c].[name] = N'ItemId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Bor] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Bor] ALTER COLUMN [ItemId] nvarchar(450) NULL

GO

CREATE UNIQUE INDEX [IX_PMol_ProjectMoleculeId] ON [PMol] ([ProjectMoleculeId]) WHERE [ProjectMoleculeId] IS NOT NULL

GO

CREATE UNIQUE INDEX [IX_PbsProduct_ProductId] ON [PbsProduct] ([ProductId]) WHERE [ProductId] IS NOT NULL

GO

CREATE UNIQUE INDEX [IX_Bor_ItemId] ON [Bor] ([ItemId]) WHERE [ItemId] IS NOT NULL

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201116114607_addeduniquesequencenumbers', N'3.1.9')

GO

ALTER TABLE [PmolTeamRole] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit)

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201117044710_addedIsDeletedToPmolTeam', N'3.1.9')

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Title', N'LocaleCode', N'ParentId', N'DisplayOrder') AND [object_id] = OBJECT_ID(N'[CpcResourceFamily]'))
    SET IDENTITY_INSERT [CpcResourceFamily] ON;
INSERT INTO [CpcResourceFamily] ([Id], [Title], [LocaleCode], [ParentId], [DisplayOrder])
VALUES (N'0c355800-91fd-4d99-8010-921a42f0ba04', N'Vehicle', N'Vehicle', NULL, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Title', N'LocaleCode', N'ParentId', N'DisplayOrder') AND [object_id] = OBJECT_ID(N'[CpcResourceFamily]'))
    SET IDENTITY_INSERT [CpcResourceFamily] OFF

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcResourceFamilyId', N'ParentId') AND [object_id] = OBJECT_ID(N'[CpcResourceFamilyLocalizedData]'))
    SET IDENTITY_INSERT [CpcResourceFamilyLocalizedData] ON;
INSERT INTO [CpcResourceFamilyLocalizedData] ([Id], [Label], [LanguageCode], [CpcResourceFamilyId], [ParentId])
VALUES (N'72d58f8f-5bcf-4415-86aa-b6677712ba47', N'Vehicle', N'en', N'0c355800-91fd-4d99-8010-921a42f0ba04', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcResourceFamilyId', N'ParentId') AND [object_id] = OBJECT_ID(N'[CpcResourceFamilyLocalizedData]'))
    SET IDENTITY_INSERT [CpcResourceFamilyLocalizedData] OFF

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcResourceFamilyId', N'ParentId') AND [object_id] = OBJECT_ID(N'[CpcResourceFamilyLocalizedData]'))
    SET IDENTITY_INSERT [CpcResourceFamilyLocalizedData] ON;
INSERT INTO [CpcResourceFamilyLocalizedData] ([Id], [Label], [LanguageCode], [CpcResourceFamilyId], [ParentId])
VALUES (N'b6677712ba47-5bcf-4415-86aa-72d58f8f', N'Vehicle(nl)', N'nl', N'0c355800-91fd-4d99-8010-921a42f0ba04', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcResourceFamilyId', N'ParentId') AND [object_id] = OBJECT_ID(N'[CpcResourceFamilyLocalizedData]'))
    SET IDENTITY_INSERT [CpcResourceFamilyLocalizedData] OFF

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcResourceFamilyId', N'ParentId') AND [object_id] = OBJECT_ID(N'[CpcResourceFamilyLocalizedData]'))
    SET IDENTITY_INSERT [CpcResourceFamilyLocalizedData] ON;
INSERT INTO [CpcResourceFamilyLocalizedData] ([Id], [Label], [LanguageCode], [CpcResourceFamilyId], [ParentId])
VALUES (N'77712ba47-4415-5bcfb66-4415-86aa-72d58f8f', N'Vehicle(nl-BE)', N'nl-BE', N'0c355800-91fd-4d99-8010-921a42f0ba04', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcResourceFamilyId', N'ParentId') AND [object_id] = OBJECT_ID(N'[CpcResourceFamilyLocalizedData]'))
    SET IDENTITY_INSERT [CpcResourceFamilyLocalizedData] OFF

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'DisplayOrder', N'LocaleCode') AND [object_id] = OBJECT_ID(N'[CpcBasicUnitOfMeasure]'))
    SET IDENTITY_INSERT [CpcBasicUnitOfMeasure] ON;
INSERT INTO [CpcBasicUnitOfMeasure] ([Id], [Name], [DisplayOrder], [LocaleCode])
VALUES (N'0c355800-91fd-4d99-8010-921a42f0ba04', N'km', 0, N'km');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'DisplayOrder', N'LocaleCode') AND [object_id] = OBJECT_ID(N'[CpcBasicUnitOfMeasure]'))
    SET IDENTITY_INSERT [CpcBasicUnitOfMeasure] OFF

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcBasicUnitOfMeasureId') AND [object_id] = OBJECT_ID(N'[CpcBasicUnitOfMeasureLocalizedData]'))
    SET IDENTITY_INSERT [CpcBasicUnitOfMeasureLocalizedData] ON;
INSERT INTO [CpcBasicUnitOfMeasureLocalizedData] ([Id], [Label], [LanguageCode], [CpcBasicUnitOfMeasureId])
VALUES (N'582f2318-287f-45b6-b628-20183f3ccfe4', N'km', N'en', N'0c355800-91fd-4d99-8010-921a42f0ba04');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcBasicUnitOfMeasureId') AND [object_id] = OBJECT_ID(N'[CpcBasicUnitOfMeasureLocalizedData]'))
    SET IDENTITY_INSERT [CpcBasicUnitOfMeasureLocalizedData] OFF

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcBasicUnitOfMeasureId') AND [object_id] = OBJECT_ID(N'[CpcBasicUnitOfMeasureLocalizedData]'))
    SET IDENTITY_INSERT [CpcBasicUnitOfMeasureLocalizedData] ON;
INSERT INTO [CpcBasicUnitOfMeasureLocalizedData] ([Id], [Label], [LanguageCode], [CpcBasicUnitOfMeasureId])
VALUES (N'20183f3ccfe4-287f-45b6-b628-582f2318', N'km', N'nl', N'0c355800-91fd-4d99-8010-921a42f0ba04');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Label', N'LanguageCode', N'CpcBasicUnitOfMeasureId') AND [object_id] = OBJECT_ID(N'[CpcBasicUnitOfMeasureLocalizedData]'))
    SET IDENTITY_INSERT [CpcBasicUnitOfMeasureLocalizedData] OFF

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201119065136_dataseedVehicle', N'3.1.9')

GO

ALTER TABLE [QRCode] ADD [TravellerType] nvarchar(max) NULL

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201119080442_addedTrsavellerTypeToQr', N'3.1.9')

GO

";

            var sqlList = new List<string>();
            sqlList.Add(test);

            foreach (var connection in connectionStrings)
            foreach (var sql in sqlList)
                using (var dbConnection = new SqlConnection(connection))
                {
                    dbConnection.Open();
                    var command = new SqlCommand(sql, dbConnection);
                    command.ExecuteNonQuery();
                }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}