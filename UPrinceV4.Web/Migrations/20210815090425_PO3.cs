          using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class PO3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                table: "StockHistoryLog",
                newName: "Ty");

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryRequest",
                table: "POHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaxonomyId",
                table: "POHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryRequest",
                table: "POHeader");

            migrationBuilder.DropColumn(
                name: "TaxonomyId",
                table: "POHeader");

            migrationBuilder.RenameColumn(
                name: "Ty",
                table: "StockHistoryLog",
                newName: "ActivityTypeId");
        }
    }
}
