using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class wbsTaxonomy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WbsTaxonomy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SequenceId",
                table: "WbsTaxonomy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "WbsTaxonomy",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "WbsTaxonomy");

            migrationBuilder.DropColumn(
                name: "SequenceId",
                table: "WbsTaxonomy");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "WbsTaxonomy");
        }
    }
}
