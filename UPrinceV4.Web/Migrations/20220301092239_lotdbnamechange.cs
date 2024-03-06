using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class lotdbnamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BMLotContractorsList");

            migrationBuilder.DropTable(
                name: "BMLotContractorTeamList");

            migrationBuilder.DropTable(
                name: "BMLotFileType");

            migrationBuilder.DropTable(
                name: "BMLotFinalDeliveryDocs");

            migrationBuilder.DropTable(
                name: "BMLotHeader");

            migrationBuilder.DropTable(
                name: "BMLotProvisionalAcceptenceDocs");

            migrationBuilder.DropTable(
                name: "BMLotStatus");

            migrationBuilder.DropTable(
                name: "BMLotTechInstructionsDocs");

            migrationBuilder.DropTable(
                name: "BMLotTenderAward");

            migrationBuilder.DropTable(
                name: "BMLotTenderDocs");

            migrationBuilder.DropTable(
                name: "BMTechDocs");

            migrationBuilder.CreateTable(
                name: "LotContractorsList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorLot = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotContractorsList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotContractorTeamList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationMail = table.Column<bool>(type: "bit", nullable: true),
                    CabPersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotContractorId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotContractorTeamList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotFileType",
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
                    table.PrimaryKey("PK_LotFileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotFinalDeliveryDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotFinalDeliveryDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductItemTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorsTaxonomy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenderBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerBudget = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotProvisionalAcceptenceDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotProvisionalAcceptenceDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotTechDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotTechDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotTechInstructionsDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotTechInstructionsDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotTenderAward",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWinner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotTenderAward", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotTenderDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotTenderDocs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LotFileType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bnb9e479-msms-40c6-Lot-e40dbe6a5bnb", 3, "en", "URL", "2210e768-msms-po02-Lot-ee367a82ad22" },
                    { "qqqab9fe-msms-4088-Lot-d27008688qqq", 1, "en", "pdf", "oo10e768-msms-po02-Lot-ee367a82adooo" },
                    { "wer9e479-msms-40c6-Lot-e40dbe6a5wer", 4, "en", "Word Document", "2210e768-msms-po02-Lot-ee367a82ad22" },
                    { "zzzab9fe-msms-4088-Lot-d27008688zzz", 2, "en", "Image", "oo10e768-msms-po02-Lot-ee367a82adooo" }
                });

            migrationBuilder.InsertData(
                table: "LotStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-Lot897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-Lot0d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-Lotf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-Lot344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-Lota2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-ad25-Lot0d49477" },
                    { "4e01a893-0267-48af-b65a-Lotebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-Lot89fc3da" },
                    { "5015743d-a2e6-4531-808d-Lot00e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-Lotecdb84c" },
                    { "77143c60-ff45-4ca2-8723-Lotd1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-Lot89fc3da" },
                    { "813a0c70-b58f-433d-8945-Lot6ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-Lotecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-Lot1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-Lota82addb" },
                    { "a35ab9fe-df57-4088-82a9-Lot8688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-Lot344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-Lote6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-Lota82addb" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LotContractorsList");

            migrationBuilder.DropTable(
                name: "LotContractorTeamList");

            migrationBuilder.DropTable(
                name: "LotFileType");

            migrationBuilder.DropTable(
                name: "LotFinalDeliveryDocs");

            migrationBuilder.DropTable(
                name: "LotHeader");

            migrationBuilder.DropTable(
                name: "LotProvisionalAcceptenceDocs");

            migrationBuilder.DropTable(
                name: "LotStatus");

            migrationBuilder.DropTable(
                name: "LotTechDocs");

            migrationBuilder.DropTable(
                name: "LotTechInstructionsDocs");

            migrationBuilder.DropTable(
                name: "LotTenderAward");

            migrationBuilder.DropTable(
                name: "LotTenderDocs");

            migrationBuilder.CreateTable(
                name: "BMLotContractorsList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractorLot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotContractorsList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotContractorTeamList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CabPersonId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationMail = table.Column<bool>(type: "bit", nullable: true),
                    LotContractorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotContractorTeamList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotFileType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotFileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotFinalDeliveryDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotFinalDeliveryDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractorsTaxonomy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductItemTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenderBudget = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotProvisionalAcceptenceDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotProvisionalAcceptenceDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotTechInstructionsDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotTechInstructionsDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotTenderAward",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContractorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWinner = table.Column<bool>(type: "bit", nullable: false),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotTenderAward", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMLotTenderDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMLotTenderDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BMTechDocs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LotId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BMTechDocs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BMLotFileType",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "TypeId" },
                values: new object[,]
                {
                    { "bnb9e479-msms-40c6-BMLot-e40dbe6a5bnb", 3, "en", "URL", "2210e768-msms-po02-BMLot-ee367a82ad22" },
                    { "qqqab9fe-msms-4088-BMLot-d27008688qqq", 1, "en", "pdf", "oo10e768-msms-po02-BMLot-ee367a82adooo" },
                    { "wer9e479-msms-40c6-BMLot-e40dbe6a5wer", 4, "en", "Word Document", "2210e768-msms-po02-BMLot-ee367a82ad22" },
                    { "zzzab9fe-msms-4088-BMLot-d27008688zzz", 2, "en", "Image", "oo10e768-msms-po02-BMLot-ee367a82adooo" }
                });

            migrationBuilder.InsertData(
                table: "BMLotStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "098cf409-7cb8-4076-8ddf-BMLot897f5bb", 1, "nl", "in voorbereiding", "d60aad0b-2e84-482b-ad25-BMLot0d49477" },
                    { "12e2d6c5-1ada-4e74-88ba-BMLotf10e27c", 2, "en", "In Development", "94282458-0b40-40a3-b0f9-BMLot344c8f1" },
                    { "2732cd5a-0941-4c56-9c13-BMLota2ab276", 1, "en", "Pending Development", "d60aad0b-2e84-482b-ad25-BMLot0d49477" },
                    { "4e01a893-0267-48af-b65a-BMLotebd1ccf", 4, "nl", "goedgekeurd", "7bcb4e8d-8e8c-487d-8170-BMLot89fc3da" },
                    { "5015743d-a2e6-4531-808d-BMLot00e1eed", 3, "nl", "ter goedkeuring", "7143ff01-d173-4a20-8c17-BMLotecdb84c" },
                    { "77143c60-ff45-4ca2-8723-BMLotd1c5f5a", 4, "en", "Approved", "7bcb4e8d-8e8c-487d-8170-BMLot89fc3da" },
                    { "813a0c70-b58f-433d-8945-BMLot6ae42af", 3, "en", "In Review", "7143ff01-d173-4a20-8c17-BMLotecdb84c" },
                    { "8d109134-ee8d-4c7b-84c5-BMLot1e2391a", 5, "en", "Handed Over", "4010e768-3e06-4702-b337-BMLota82addb" },
                    { "a35ab9fe-df57-4088-82a9-BMLot8688bae", 2, "nl", "in uitvoering", "94282458-0b40-40a3-b0f9-BMLot344c8f1" },
                    { "bdd9e479-75b3-40c6-ad61-BMLote6a51ac", 5, "nl", "afgewerkt en doorgegeven", "4010e768-3e06-4702-b337-BMLota82addb" }
                });
        }
    }
}
