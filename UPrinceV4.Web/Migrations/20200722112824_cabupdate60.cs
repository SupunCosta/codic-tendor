using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class cabupdate60 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CabBankAccount",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BankAccount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabBankAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabEmail",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabEmail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabLandPhone",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LandPhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabLandPhone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabMobilePhone",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MobilePhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabMobilePhone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabSkype",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SkypeNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabSkype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabVat",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Vat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabVat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabWhatsApp",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    WhatsAppNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabWhatsApp", x => x.Id);
                });


            migrationBuilder.CreateTable(
                name: "CabAddress",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<string>(nullable: true),
                    MailBox = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    CountryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabAddress_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateTable(
                name: "CabPerson",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsSaved = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    CallName = table.Column<string>(nullable: true),
                    BusinessEmailId = table.Column<string>(nullable: true),
                    BusinessPhoneId = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    AddressId = table.Column<string>(nullable: true),
                    EmailId = table.Column<string>(nullable: true),
                    LandPhoneId = table.Column<string>(nullable: true),
                    MobilePhoneId = table.Column<string>(nullable: true),
                    WhatsAppId = table.Column<string>(nullable: true),
                    SkypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabPerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabAddress_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CabAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabEmail_BusinessEmailId",
                        column: x => x.BusinessEmailId,
                        principalTable: "CabEmail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabMobilePhone_BusinessPhoneId",
                        column: x => x.BusinessPhoneId,
                        principalTable: "CabMobilePhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabEmail_EmailId",
                        column: x => x.EmailId,
                        principalTable: "CabEmail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabLandPhone_LandPhoneId",
                        column: x => x.LandPhoneId,
                        principalTable: "CabLandPhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabMobilePhone_MobilePhoneId",
                        column: x => x.MobilePhoneId,
                        principalTable: "CabMobilePhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabSkype_SkypeId",
                        column: x => x.SkypeId,
                        principalTable: "CabSkype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabWhatsApp_WhatsAppId",
                        column: x => x.WhatsAppId,
                        principalTable: "CabWhatsApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateTable(
                name: "CabCompany",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsSaved = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    VatId = table.Column<string>(nullable: true),
                    BankAccountId = table.Column<string>(nullable: true),
                    AddressId = table.Column<string>(nullable: true),
                    CabPersonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabCompany_CabAddress_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CabAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabCompany_CabBankAccount_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "CabBankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabCompany_CabPerson_CabPersonId",
                        column: x => x.CabPersonId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabCompany_CabVat_VatId",
                        column: x => x.VatId,
                        principalTable: "CabVat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateTable(
                name: "CabHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabHistoryLog_AllUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabHistoryLog_CabCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CabCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabHistoryLog_CabPerson_PersonId",
                        column: x => x.PersonId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CabPersonCompany",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsSaved = table.Column<bool>(nullable: false),
                    PersonId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    JobRole = table.Column<string>(nullable: true),
                    EmailId = table.Column<string>(nullable: true),
                    MobilePhoneId = table.Column<string>(nullable: true),
                    LandPhoneId = table.Column<string>(nullable: true),
                    WhatsAppId = table.Column<string>(nullable: true),
                    SkypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabPersonCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabCompany_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CabCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabEmail_EmailId",
                        column: x => x.EmailId,
                        principalTable: "CabEmail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabLandPhone_LandPhoneId",
                        column: x => x.LandPhoneId,
                        principalTable: "CabLandPhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabMobilePhone_MobilePhoneId",
                        column: x => x.MobilePhoneId,
                        principalTable: "CabMobilePhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabPerson_PersonId",
                        column: x => x.PersonId,
                        principalTable: "CabPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabSkype_SkypeId",
                        column: x => x.SkypeId,
                        principalTable: "CabSkype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabWhatsApp_WhatsAppId",
                        column: x => x.WhatsAppId,
                        principalTable: "CabWhatsApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });


            migrationBuilder.CreateIndex(
                name: "IX_CabCompany_AddressId",
                table: "CabCompany",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CabCompany_BankAccountId",
                table: "CabCompany",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CabCompany_CabPersonId",
                table: "CabCompany",
                column: "CabPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CabCompany_VatId",
                table: "CabCompany",
                column: "VatId");

            migrationBuilder.CreateIndex(
                name: "IX_CabEmail_EmailAddress",
                table: "CabEmail",
                column: "EmailAddress",
                unique: true,
                filter: "[EmailAddress] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CabHistoryLog_ChangedByUserId",
                table: "CabHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CabHistoryLog_CompanyId",
                table: "CabHistoryLog",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CabHistoryLog_PersonId",
                table: "CabHistoryLog",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CabLandPhone_LandPhoneNumber",
                table: "CabLandPhone",
                column: "LandPhoneNumber",
                unique: true,
                filter: "[LandPhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CabMobilePhone_MobilePhoneNumber",
                table: "CabMobilePhone",
                column: "MobilePhoneNumber",
                unique: true,
                filter: "[MobilePhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_AddressId",
                table: "CabPerson",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_BusinessEmailId",
                table: "CabPerson",
                column: "BusinessEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_BusinessPhoneId",
                table: "CabPerson",
                column: "BusinessPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_EmailId",
                table: "CabPerson",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_LandPhoneId",
                table: "CabPerson",
                column: "LandPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_MobilePhoneId",
                table: "CabPerson",
                column: "MobilePhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_SkypeId",
                table: "CabPerson",
                column: "SkypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_WhatsAppId",
                table: "CabPerson",
                column: "WhatsAppId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_CompanyId",
                table: "CabPersonCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_EmailId",
                table: "CabPersonCompany",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_LandPhoneId",
                table: "CabPersonCompany",
                column: "LandPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_MobilePhoneId",
                table: "CabPersonCompany",
                column: "MobilePhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_PersonId",
                table: "CabPersonCompany",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_SkypeId",
                table: "CabPersonCompany",
                column: "SkypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_WhatsAppId",
                table: "CabPersonCompany",
                column: "WhatsAppId");

            migrationBuilder.CreateIndex(
                name: "IX_CabSkype_SkypeNumber",
                table: "CabSkype",
                column: "SkypeNumber",
                unique: true,
                filter: "[SkypeNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CabWhatsApp_WhatsAppNumber",
                table: "CabWhatsApp",
                column: "WhatsAppNumber",
                unique: true,
                filter: "[WhatsAppNumber] IS NOT NULL");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
