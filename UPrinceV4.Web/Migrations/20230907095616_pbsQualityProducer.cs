using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UPrinceV4
{
    /// <inheritdoc />
    public partial class pbsQualityProducer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectTitle",
                table: "PmolAssignTime",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualityProducerId",
                table: "PbsProduct",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "ContractorTeamList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotSubscribe",
                table: "ContractorTeamList",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUploaded",
                table: "ContractorTeamList",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectTitle",
                table: "PmolAssignTime");

            migrationBuilder.DropColumn(
                name: "QualityProducerId",
                table: "PbsProduct");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ContractorTeamList");

            migrationBuilder.DropColumn(
                name: "IsNotSubscribe",
                table: "ContractorTeamList");

            migrationBuilder.DropColumn(
                name: "IsUploaded",
                table: "ContractorTeamList");
        }
    }
}
