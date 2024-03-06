using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class mecopch1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MecopConversionBrandweerstand");

            migrationBuilder.DropTable(
                name: "MecopConversionDraaiRichting");

            migrationBuilder.DropTable(
                name: "MecopConversionMateriaal");

            migrationBuilder.DropTable(
                name: "MecopConversionPaumelles");

            migrationBuilder.DropTable(
                name: "MecopConversionTegenPlaat");

            migrationBuilder.DropTable(
                name: "MecopConversionType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MecopMetaData",
                table: "MecopMetaData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MecopConversion",
                table: "MecopConversion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mecop",
                table: "Mecop");

            migrationBuilder.RenameTable(
                name: "MecopMetaData",
                newName: "CustomerOrderToPoMetaData");

            migrationBuilder.RenameTable(
                name: "MecopConversion",
                newName: "CustomerOrderToPoConversion");

            migrationBuilder.RenameTable(
                name: "Mecop",
                newName: "CustomerOrderToPo");

            migrationBuilder.AddColumn<string>(
                name: "Customer",
                table: "CustomerOrderToPoConversion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "CustomerOrderToPoConversion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerOrderToPoMetaData",
                table: "CustomerOrderToPoMetaData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerOrderToPoConversion",
                table: "CustomerOrderToPoConversion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerOrderToPo",
                table: "CustomerOrderToPo",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerOrderToPoMetaData",
                table: "CustomerOrderToPoMetaData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerOrderToPoConversion",
                table: "CustomerOrderToPoConversion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerOrderToPo",
                table: "CustomerOrderToPo");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "CustomerOrderToPoConversion");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "CustomerOrderToPoConversion");

            migrationBuilder.RenameTable(
                name: "CustomerOrderToPoMetaData",
                newName: "MecopMetaData");

            migrationBuilder.RenameTable(
                name: "CustomerOrderToPoConversion",
                newName: "MecopConversion");

            migrationBuilder.RenameTable(
                name: "CustomerOrderToPo",
                newName: "Mecop");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MecopMetaData",
                table: "MecopMetaData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MecopConversion",
                table: "MecopConversion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mecop",
                table: "Mecop",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MecopConversionBrandweerstand",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MecopConversionBrandweerstand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MecopConversionDraaiRichting",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MecopConversionDraaiRichting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MecopConversionMateriaal",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MecopConversionMateriaal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MecopConversionPaumelles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MecopConversionPaumelles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MecopConversionTegenPlaat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MecopConversionTegenPlaat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MecopConversionType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MecopConversionType", x => x.Id);
                });
        }
    }
}
