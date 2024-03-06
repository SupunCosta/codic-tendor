using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class PbsAssignedLabour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CiawFeatchStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawFeatchStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CiawRemark",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawRemark", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractorFileType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorFileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractorsTotalPriceErrors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorPdfId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<float>(type: "real", nullable: true),
                    TotalPrice = table.Column<float>(type: "real", nullable: true),
                    CorrectTotalPrice = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorsTotalPriceErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CpcSerialNumber",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CPCId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcSerialNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRContractorList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HRId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRContractorList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRContractTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRContractTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsAssignedLabour",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PbsLabourId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabPersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsAssignedLabour", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbsDisplayOrder",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbsDisplayOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThColors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Font = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThTruckAvailability",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StockId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    ResourceFamilyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ETime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortingOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThTruckAvailability", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTask",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstructionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTaskCc",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaskCc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTaskDocuments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaskDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTaskTags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaskTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTaxonomy",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WbsTaxonomyLevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    TemplateId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaxonomy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTaxonomyLevels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTaxonomyLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WbsTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WbsTemplate", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CiawFeatchStatus",
                columns: new[] { "Id", "Status" },
                values: new object[] { "1", false });

            migrationBuilder.InsertData(
                table: "ContractorFileType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "wer9e479-msms-40c6-Lot5-e40dbe6a5wer", 5, "en", "ZIP", "2210e768-msms-po02-Lot5-ee367a82ad22" },
                    { "wer9e479-msms-4ZIP-Lot5-e40dbe6a5wer", 5, "nl", "ZIP(nl)", "2210e768-msms-po02-Lot5-ee367a82ad22" }
                });

            migrationBuilder.InsertData(
                table: "HRContractTypes",
                columns: new[] { "Id", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "005b42ba-574d-4afd-a034-347858e53c9d", "en", "Tempory", "12a22319-8ca7-temp-b588-6fab99474130" },
                    { "1377a17d-3f18-46c1-bc7c-c11edcf65b5c", "nl", "Tempory(nl)", "12a22319-8ca7-temp-b588-6fab99474130" },
                    { "222e3dab-576d-4f53-b976-a9b5c97ee165", "en", "Permenant", "41ce52c0-058b-perm-afbd-1d2d24105ebc" },
                    { "3263aa4e-12a8-4c59-bc99-d561a603748e", "nl", "Permenant(nl)", "41ce52c0-058b-perm-afbd-1d2d24105ebc" }
                });

            migrationBuilder.InsertData(
                table: "ThColors",
                columns: new[] { "Id", "Code", "Font" },
                values: new object[,]
                {
                    { "1", "#c56834", "#fffffff" },
                    { "10", "#f69b68", "#000000" },
                    { "11", "#b3da90", "#000000" },
                    { "12", "#b65ab3", "#fffffff" },
                    { "13", "#08272B", "#fffffff" },
                    { "14", "#19D65B", "#000000" },
                    { "15", "#1C6675", "#fffffff" },
                    { "16", "#AE9675", "#fffffff" },
                    { "17", "#8C2581", "#000000" },
                    { "18", "#7EF6CE", "#000000" },
                    { "19", "#1C85CF", "#000000" },
                    { "2", "#c4d8e5", "#000000" },
                    { "20", "#0F264F", "#fffffff" },
                    { "3", "#97c8ea", "#fffffff" },
                    { "4", "#3b9b36", "#fffffff" },
                    { "5", "#a5982c", "#fffffff" },
                    { "6", "#97ac0f", "#fffffff" },
                    { "7", "#b13748", "#fffffff" },
                    { "8", "#ea716d", "#fffffff" },
                    { "9", "#166fdb", "#fffffff" }
                });

            migrationBuilder.InsertData(
                table: "WbsTaxonomyLevels",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "LevelId", "Name" },
                values: new object[,]
                {
                    { "3263aa4e-12a8-wbs-bc99-d561a603748e", "1", "en", "41ce52c0-058b-wbs-afbd-1d2d24105ebc", "WBS" },
                    { "d263aa4e-12a8-issu-bc99-d561a603748e", "3", "en", "i1ce52c0-058b-issu-afbd-1d2d24105ebc", "Task" },
                    { "p263aa4e-12a8-prod-bc99-d561a603748e", "2", "en", "e1ce52c0-058b-prod-afbd-1d2d24105ebc", "Product" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CiawFeatchStatus");

            migrationBuilder.DropTable(
                name: "CiawRemark");

            migrationBuilder.DropTable(
                name: "ContractorFileType");

            migrationBuilder.DropTable(
                name: "ContractorsTotalPriceErrors");

            migrationBuilder.DropTable(
                name: "CpcSerialNumber");

            migrationBuilder.DropTable(
                name: "HRContractorList");

            migrationBuilder.DropTable(
                name: "HRContractTypes");

            migrationBuilder.DropTable(
                name: "PbsAssignedLabour");

            migrationBuilder.DropTable(
                name: "PbsDisplayOrder");

            migrationBuilder.DropTable(
                name: "ThColors");

            migrationBuilder.DropTable(
                name: "ThTruckAvailability");

            migrationBuilder.DropTable(
                name: "WbsTask");

            migrationBuilder.DropTable(
                name: "WbsTaskCc");

            migrationBuilder.DropTable(
                name: "WbsTaskDocuments");

            migrationBuilder.DropTable(
                name: "WbsTaskTags");

            migrationBuilder.DropTable(
                name: "WbsTaxonomy");

            migrationBuilder.DropTable(
                name: "WbsTaxonomyLevels");

            migrationBuilder.DropTable(
                name: "WbsTemplate");
        }
    }
}
