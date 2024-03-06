using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPrinceV4.Web.Migrations.UprinceV5
{
    /// <inheritdoc />
    public partial class pbsassignedLabour2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Enddate",
                table: "PbsAssignedLabour",
                newName: "EndDate");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "PbsAssignedLabour",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectManager",
                table: "PbsAssignedLabour",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Week",
                table: "PbsAssignedLabour",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "PbsAssignedLabour");

            migrationBuilder.DropColumn(
                name: "ProjectManager",
                table: "PbsAssignedLabour");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "PbsAssignedLabour");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "PbsAssignedLabour",
                newName: "Enddate");
        }
    }
}
