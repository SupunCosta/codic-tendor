using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class POProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "POHeaderId",
                table: "POResourcesDocument",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "POProduct",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HeaderTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsProductItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsProductItemTypeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsProductStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsProductStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsToleranceState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbsToleranceStateId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PQuantity = table.Column<int>(type: "int", nullable: false),
                    CQuantity = table.Column<int>(type: "int", nullable: false),
                    Mou = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PUnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUnitPrice = table.Column<int>(type: "int", nullable: false),
                    PTotalPrice = table.Column<int>(type: "int", nullable: false),
                    CTotalPrice = table.Column<int>(type: "int", nullable: false),
                    CCrossReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PCrossReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POHeaderId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POProduct", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POProduct");

            migrationBuilder.DropColumn(
                name: "POHeaderId",
                table: "POResourcesDocument");
        }
    }
}
