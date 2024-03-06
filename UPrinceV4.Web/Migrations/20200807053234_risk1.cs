using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class risk1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskStatus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Risk",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RiskDetails = table.Column<string>(nullable: true),
                    RiskTypeId = table.Column<string>(nullable: true),
                    CabPersonId = table.Column<string>(nullable: true),
                    RiskStatusId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risk_CabPerson_CabPersonId",
                        column: x => x.CabPersonId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_RiskStatus_RiskStatusId",
                        column: x => x.RiskStatusId,
                        principalTable: "RiskStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Risk_RiskType_RiskTypeId",
                        column: x => x.RiskTypeId,
                        principalTable: "RiskType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Risk_CabPersonId",
                table: "Risk",
                column: "CabPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_RiskStatusId",
                table: "Risk",
                column: "RiskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_RiskTypeId",
                table: "Risk",
                column: "RiskTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Risk");

            migrationBuilder.DropTable(
                name: "RiskStatus");

            migrationBuilder.DropTable(
                name: "RiskType");


        }
    }
}
