using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations
{
    public partial class pbsDynamicAttributesNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key1",
                table: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Key2",
                table: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Key3",
                table: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Key4",
                table: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Key5",
                table: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Value2",
                table: "PbsDynamicAttributes");

            migrationBuilder.DropColumn(
                name: "Value3",
                table: "PbsDynamicAttributes");

            migrationBuilder.RenameColumn(
                name: "Value5",
                table: "PbsDynamicAttributes",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Value4",
                table: "PbsDynamicAttributes",
                newName: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "PbsDynamicAttributes",
                newName: "Value5");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "PbsDynamicAttributes",
                newName: "Value4");

            migrationBuilder.AddColumn<string>(
                name: "Key1",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key2",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key3",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key4",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key5",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value2",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value3",
                table: "PbsDynamicAttributes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
