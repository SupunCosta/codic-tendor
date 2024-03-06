using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class ChangedconsumedQntyMou : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ConsumedQuantityMou",
                table: "PsResource",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ConsumedQuantityMou",
                table: "PsResource",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
