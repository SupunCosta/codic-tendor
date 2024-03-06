using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class wbsDocupdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentFileId",
                table: "WbsDocument",
                newName: "FileUrl");

            migrationBuilder.AddColumn<string>(
                name: "FileDownloadUrl",
                table: "WbsDocument",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "WbsDocument",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDownloadUrl",
                table: "WbsDocument");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "WbsDocument");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "WbsDocument",
                newName: "DocumentFileId");
        }
    }
}
