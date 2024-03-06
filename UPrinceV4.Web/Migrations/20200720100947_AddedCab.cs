using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedCab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CountryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabAddress",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<string>(nullable: true),
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
                name: "CabPrimarySubjectMatterDomain",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SubjectMatter = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabPrimarySubjectMatterDomain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabSalutation",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SalutationName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabSalutation", x => x.Id);
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
                name: "CabSpecialization",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Speciality = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabSpecialization", x => x.Id);
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
                name: "CabWorkingStatus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabWorkingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabPerson",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsSaved = table.Column<bool>(nullable: false),
                    UsersId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    PreferredName = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    AddressId = table.Column<string>(nullable: true),
                    EmailId = table.Column<string>(nullable: true),
                    LandPhoneId = table.Column<string>(nullable: true),
                    MobilePhoneId = table.Column<string>(nullable: true),
                    SalutationId = table.Column<string>(nullable: true),
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
                        name: "FK_CabPerson_CabSalutation_SalutationId",
                        column: x => x.SalutationId,
                        principalTable: "CabSalutation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_CabSkype_SkypeId",
                        column: x => x.SkypeId,
                        principalTable: "CabSkype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPerson_AllUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AllUsers",
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
                    UsersId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        name: "FK_CabCompany_AllUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AllUsers",
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
                    UsersId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    WorkingStatusId = table.Column<string>(nullable: true),
                    JobRole = table.Column<string>(nullable: true),
                    ProjectManagerNumber = table.Column<string>(nullable: true),
                    Craftsman = table.Column<string>(nullable: true),
                    Team = table.Column<string>(nullable: true),
                    SpecializationId = table.Column<string>(nullable: true),
                    PrimarySubjectMatterDomainId = table.Column<string>(nullable: true),
                    EmailId = table.Column<string>(nullable: true),
                    MobilePhoneId = table.Column<string>(nullable: true),
                    LandPhoneId = table.Column<string>(nullable: true),
                    WhatsAppId = table.Column<string>(nullable: true),
                    SkypeId = table.Column<string>(nullable: true),
                    AddressId = table.Column<string>(nullable: true),
                    JoinedDate = table.Column<DateTime>(nullable: true),
                    ResignedDate = table.Column<DateTime>(nullable: true),
                    TimePeriod = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabPersonCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabAddress_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CabAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_CabPersonCompany_CabPrimarySubjectMatterDomain_PrimarySubjectMatterDomainId",
                        column: x => x.PrimarySubjectMatterDomainId,
                        principalTable: "CabPrimarySubjectMatterDomain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabSkype_SkypeId",
                        column: x => x.SkypeId,
                        principalTable: "CabSkype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabSpecialization_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "CabSpecialization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_AllUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabWhatsApp_WhatsAppId",
                        column: x => x.WhatsAppId,
                        principalTable: "CabWhatsApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabPersonCompany_CabWorkingStatus_WorkingStatusId",
                        column: x => x.WorkingStatusId,
                        principalTable: "CabWorkingStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CabAddress_CountryId",
                table: "CabAddress",
                column: "CountryId");

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
                name: "IX_CabCompany_UsersId",
                table: "CabCompany",
                column: "UsersId");

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
                name: "IX_CabPerson_SalutationId",
                table: "CabPerson",
                column: "SalutationId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_SkypeId",
                table: "CabPerson",
                column: "SkypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_UsersId",
                table: "CabPerson",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPerson_WhatsAppId",
                table: "CabPerson",
                column: "WhatsAppId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_AddressId",
                table: "CabPersonCompany",
                column: "AddressId");

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
                name: "IX_CabPersonCompany_PrimarySubjectMatterDomainId",
                table: "CabPersonCompany",
                column: "PrimarySubjectMatterDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_SkypeId",
                table: "CabPersonCompany",
                column: "SkypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_SpecializationId",
                table: "CabPersonCompany",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_UsersId",
                table: "CabPersonCompany",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_WhatsAppId",
                table: "CabPersonCompany",
                column: "WhatsAppId");

            migrationBuilder.CreateIndex(
                name: "IX_CabPersonCompany_WorkingStatusId",
                table: "CabPersonCompany",
                column: "WorkingStatusId");

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
            migrationBuilder.DropTable(
                name: "CabHistoryLog");

            migrationBuilder.DropTable(
                name: "CabPersonCompany");

            migrationBuilder.DropTable(
                name: "CabCompany");

            migrationBuilder.DropTable(
                name: "CabPrimarySubjectMatterDomain");

            migrationBuilder.DropTable(
                name: "CabSpecialization");

            migrationBuilder.DropTable(
                name: "CabWorkingStatus");

            migrationBuilder.DropTable(
                name: "CabBankAccount");

            migrationBuilder.DropTable(
                name: "CabPerson");

            migrationBuilder.DropTable(
                name: "CabVat");

            migrationBuilder.DropTable(
                name: "CabAddress");

            migrationBuilder.DropTable(
                name: "CabEmail");

            migrationBuilder.DropTable(
                name: "CabLandPhone");

            migrationBuilder.DropTable(
                name: "CabMobilePhone");

            migrationBuilder.DropTable(
                name: "CabSalutation");

            migrationBuilder.DropTable(
                name: "CabSkype");

            migrationBuilder.DropTable(
                name: "CabWhatsApp");

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandPhone",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LandPhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPhone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobilePhone",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MobilePhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilePhone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Whatsapp",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Whatsapp", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Email_EmailAddress",
                table: "Email",
                column: "EmailAddress",
                unique: true,
                filter: "[EmailAddress] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LandPhone_LandPhoneNumber",
                table: "LandPhone",
                column: "LandPhoneNumber",
                unique: true,
                filter: "[LandPhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MobilePhone_MobilePhoneNumber",
                table: "MobilePhone",
                column: "MobilePhoneNumber",
                unique: true,
                filter: "[MobilePhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Whatsapp_WhatsappNumber",
                table: "Whatsapp",
                column: "WhatsappNumber",
                unique: true,
                filter: "[WhatsappNumber] IS NOT NULL");
        }
    }
}
