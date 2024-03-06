using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class milestone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MilestoneHeader",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SequenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Abstract = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtilityTaxonomy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationTaxonomy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MachineTaxonomy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountableId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsibleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedAmmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualAmmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilestoneHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MilestoneDocuments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MilestoneHeaderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilestoneDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilestoneDocuments_MilestoneHeader_MilestoneHeaderId",
                        column: x => x.MilestoneHeaderId,
                        principalTable: "MilestoneHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneDocuments_MilestoneHeaderId",
                table: "MilestoneDocuments",
                column: "MilestoneHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilestoneDocuments");

            migrationBuilder.DropTable(
                name: "MilestoneHeader");
        }
    }
}
