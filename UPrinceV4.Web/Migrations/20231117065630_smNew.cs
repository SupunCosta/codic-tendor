using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UPrinceV4
{
    /// <inheritdoc />
    public partial class smNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptTender",
                table: "StandardMailHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadTender",
                table: "StandardMailHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscribeTender",
                table: "StandardMailHeader",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptTender",
                table: "StandardMailHeader");

            migrationBuilder.DropColumn(
                name: "DownloadTender",
                table: "StandardMailHeader");

            migrationBuilder.DropColumn(
                name: "SubscribeTender",
                table: "StandardMailHeader");
        }
    }
}
