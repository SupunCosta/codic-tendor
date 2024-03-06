using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class publishVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MeasuringStatus",
                table: "ContractorHeader",
                type: "float",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<double>(
                name: "Version",
                table: "ConstructorWorkFlow",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "ConstructorWorkFlow");

            migrationBuilder.AlterColumn<bool>(
                name: "MeasuringStatus",
                table: "ContractorHeader",
                type: "bit",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
