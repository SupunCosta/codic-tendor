using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class RemovedPersonCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactCompany");

            migrationBuilder.DropTable(
                name: "WorkingStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactCompany",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChangedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContactId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSaved = table.Column<bool>(type: "bit", nullable: false),
                    JobRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LandPhoneId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MobilePhoneId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PersonId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TimePeriod = table.Column<TimeSpan>(type: "time", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WhatsappId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WorkingStatusId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactCompany_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_Email_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Email",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_LandPhone_LandPhoneId",
                        column: x => x.LandPhoneId,
                        principalTable: "LandPhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_MobilePhone_MobilePhoneId",
                        column: x => x.MobilePhoneId,
                        principalTable: "MobilePhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_Skype_SkypeId",
                        column: x => x.SkypeId,
                        principalTable: "Skype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_AllUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_Whatsapp_WhatsappId",
                        column: x => x.WhatsappId,
                        principalTable: "Whatsapp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCompany_WorkingStatus_WorkingStatusId",
                        column: x => x.WorkingStatusId,
                        principalTable: "WorkingStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_AddressId",
                table: "ContactCompany",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_CompanyId",
                table: "ContactCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_EmailId",
                table: "ContactCompany",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_LandPhoneId",
                table: "ContactCompany",
                column: "LandPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_MobilePhoneId",
                table: "ContactCompany",
                column: "MobilePhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_PersonId",
                table: "ContactCompany",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_SkypeId",
                table: "ContactCompany",
                column: "SkypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_UsersId",
                table: "ContactCompany",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_WhatsappId",
                table: "ContactCompany",
                column: "WhatsappId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCompany_WorkingStatusId",
                table: "ContactCompany",
                column: "WorkingStatusId");
        }
    }
}
