using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UPrinceV4.Web.Migrations
{
    public partial class cabupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    OId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNo = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppConfigurationData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    IsConfigured = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigurationData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarTemplate", x => x.Id);
                });

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
                name: "CpcResourceFamily",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcResourceFamily", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpcResourceFamily_CpcResourceFamily_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CpcResourceFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CpcResourceType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcResourceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorMessage",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandPhone",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LandPhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandPhone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LastSeenProjectDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<string>(nullable: true),
                    ViewedUserId = table.Column<string>(nullable: true),
                    ViewTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastSeenProjectDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locales",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Locale = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalizedData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizedData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Altitude = table.Column<double>(nullable: false),
                    HorizontalAccuracy = table.Column<double>(nullable: false),
                    VerticleAccuracy = table.Column<double>(nullable: false),
                    Speed = table.Column<double>(nullable: false),
                    Heading = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobilePhone",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MobilePhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilePhone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectManagementLevel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    LocaleCode = table.Column<string>(nullable: true),
                    ListingOrder = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectManagementLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectState",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    LocaleCode = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTemplate",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectToleranceState",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    LocaleCode = table.Column<string>(nullable: true),
                    ListingOrder = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectToleranceState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    LocaleCode = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Salutation",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SalutationName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salutation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skype",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SkypeNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skype", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeClockActivityType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    LocaleCode = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeClockActivityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Whatsapp",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    WhatsappNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Whatsapp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(nullable: true),
                    LocaleCode = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkingStatus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDefinitionHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    ProjectDefinitionId = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDefinitionHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDefinitionHistoryLog_AllUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
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
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoperateProductCatalog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ResourceTitle = table.Column<string>(nullable: false),
                    ResourceTypeId = table.Column<string>(nullable: false),
                    ResourceFamilyId = table.Column<string>(nullable: true),
                    BasicUnitOfMeasure = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoperateProductCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoperateProductCatalog_CpcResourceFamily_ResourceFamilyId",
                        column: x => x.ResourceFamilyId,
                        principalTable: "CpcResourceFamily",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoperateProductCatalog_CpcResourceType_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "CpcResourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDefinition",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ProjectTypeId = table.Column<string>(nullable: true),
                    ProjectManagementLevelId = table.Column<string>(nullable: false),
                    ProjectToleranceStateId = table.Column<string>(nullable: false),
                    ProjectTemplateId = table.Column<string>(nullable: true),
                    SequenceCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDefinition_ProjectManagementLevel_ProjectManagementLevelId",
                        column: x => x.ProjectManagementLevelId,
                        principalTable: "ProjectManagementLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDefinition_ProjectTemplate_ProjectTemplateId",
                        column: x => x.ProjectTemplateId,
                        principalTable: "ProjectTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectDefinition_ProjectToleranceState_ProjectToleranceStateId",
                        column: x => x.ProjectToleranceStateId,
                        principalTable: "ProjectToleranceState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDefinition_ProjectType_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_AllUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    WorkflowStateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_AllUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shifts_WorkflowState_WorkflowStateId",
                        column: x => x.WorkflowStateId,
                        principalTable: "WorkflowState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person",
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
                    WhatsappId = table.Column<string>(nullable: true),
                    SkypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_Email_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Email",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_LandPhone_LandPhoneId",
                        column: x => x.LandPhoneId,
                        principalTable: "LandPhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_MobilePhone_MobilePhoneId",
                        column: x => x.MobilePhoneId,
                        principalTable: "MobilePhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_Salutation_SalutationId",
                        column: x => x.SalutationId,
                        principalTable: "Salutation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_Skype_SkypeId",
                        column: x => x.SkypeId,
                        principalTable: "Skype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_AllUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_Whatsapp_WhatsappId",
                        column: x => x.WhatsappId,
                        principalTable: "Whatsapp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CpcHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpcHistoryLog_AllUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CpcHistoryLog_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CpcImage",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CoperateProductCatalogId = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpcImage_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CpcResourceNickname",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(nullable: true),
                    CoperateProductCatalogId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcResourceNickname", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpcResourceNickname_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CpcVendor",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    ResourceNumber = table.Column<string>(nullable: true),
                    ResourceTitle = table.Column<string>(nullable: true),
                    PurchasingUnit = table.Column<string>(nullable: true),
                    ResourcePrice = table.Column<double>(nullable: true),
                    ResourceLeadTime = table.Column<string>(nullable: true),
                    MinOrderQuantity = table.Column<double>(nullable: true),
                    MaxOrderQuantity = table.Column<double>(nullable: true),
                    RoundingValue = table.Column<double>(nullable: true),
                    CoperateProductCatalogId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcVendor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpcVendor_CoperateProductCatalog_CoperateProductCatalogId",
                        column: x => x.CoperateProductCatalogId,
                        principalTable: "CoperateProductCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFinance",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TotalBudget = table.Column<double>(nullable: true),
                    BudgetLabour = table.Column<double>(nullable: true),
                    BudgetMaterial = table.Column<double>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFinance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFinance_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectFinance_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectStateId = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(nullable: false),
                    ModifiedByUserId = table.Column<string>(nullable: true),
                    RevisionNumber = table.Column<int>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectHistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectHistoryLog_AllUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectHistoryLog_AllUsers_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectHistoryLog_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectHistoryLog_ProjectState_ProjectStateId",
                        column: x => x.ProjectStateId,
                        principalTable: "ProjectState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectKPI",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CustomLabelOne = table.Column<string>(nullable: true),
                    CustomPropOne = table.Column<double>(nullable: true),
                    CustomLabelTwo = table.Column<string>(nullable: true),
                    CustomPropTwo = table.Column<double>(nullable: true),
                    CustomLabelThree = table.Column<string>(nullable: true),
                    CustomPropThree = table.Column<double>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectKPI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectKPI_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTeam",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Customer = table.Column<string>(nullable: true),
                    ContractingUnit = table.Column<string>(nullable: false),
                    ProjectOwnerId = table.Column<string>(nullable: true),
                    ProjectManagerId = table.Column<string>(nullable: false),
                    ProjectEngineerId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_AllUsers_ProjectEngineerId",
                        column: x => x.ProjectEngineerId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_AllUsers_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_AllUsers_ProjectOwnerId",
                        column: x => x.ProjectOwnerId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTime",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CalendarTemplateId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTime_CalendarTemplate_CalendarTemplateId",
                        column: x => x.CalendarTemplateId,
                        principalTable: "CalendarTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTime_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRCode",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    VehicleNo = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    PersonalId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    ActivityTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QRCode_TimeClockActivityType_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "TimeClockActivityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QRCode_ProjectDefinition_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ProjectDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company",
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
                    VAT = table.Column<string>(nullable: true),
                    BankAccount = table.Column<string>(nullable: true),
                    AddressId = table.Column<string>(nullable: true),
                    PersonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_AllUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeClock",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: true),
                    FromLocation = table.Column<string>(nullable: true),
                    ToLocation = table.Column<string>(nullable: true),
                    QRCodeId = table.Column<string>(nullable: true),
                    LocationId = table.Column<string>(nullable: true),
                    ShiftId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeClock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeClock_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeClock_QRCode_QRCodeId",
                        column: x => x.QRCodeId,
                        principalTable: "QRCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeClock_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeClock_AllUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CabHistoryLog",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HistoryLog = table.Column<string>(nullable: true),
                    ProjectDefinitionId = table.Column<string>(nullable: true),
                    ChangedByUserId = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ChangedTime = table.Column<DateTime>(nullable: false),
                    RevisionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<string>(nullable: true),
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
                        name: "FK_CabHistoryLog_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CabHistoryLog_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonCompany",
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
                    EmailId = table.Column<string>(nullable: true),
                    MobilePhoneId = table.Column<string>(nullable: true),
                    LandPhoneId = table.Column<string>(nullable: true),
                    WhatsappId = table.Column<string>(nullable: true),
                    SkypeId = table.Column<string>(nullable: true),
                    AddressId = table.Column<string>(nullable: true),
                    JoinedDate = table.Column<DateTime>(nullable: true),
                    ResignedDate = table.Column<DateTime>(nullable: true),
                    TimePeriod = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonCompany_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_Email_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Email",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_LandPhone_LandPhoneId",
                        column: x => x.LandPhoneId,
                        principalTable: "LandPhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_MobilePhone_MobilePhoneId",
                        column: x => x.MobilePhoneId,
                        principalTable: "MobilePhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_Skype_SkypeId",
                        column: x => x.SkypeId,
                        principalTable: "Skype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_AllUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AllUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_Whatsapp_WhatsappId",
                        column: x => x.WhatsappId,
                        principalTable: "Whatsapp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonCompany_WorkingStatus_WorkingStatusId",
                        column: x => x.WorkingStatusId,
                        principalTable: "WorkingStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CountryId",
                table: "Address",
                column: "CountryId");

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
                name: "IX_Company_AddressId",
                table: "Company",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_PersonId",
                table: "Company",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_UsersId",
                table: "Company",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_CoperateProductCatalog_ResourceFamilyId",
                table: "CoperateProductCatalog",
                column: "ResourceFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_CoperateProductCatalog_ResourceTypeId",
                table: "CoperateProductCatalog",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_CountryName",
                table: "Country",
                column: "CountryName",
                unique: true,
                filter: "[CountryName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CpcHistoryLog_ChangedByUserId",
                table: "CpcHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcHistoryLog_CoperateProductCatalogId",
                table: "CpcHistoryLog",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcImage_CoperateProductCatalogId",
                table: "CpcImage",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcResourceFamily_ParentId",
                table: "CpcResourceFamily",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcResourceNickname_CoperateProductCatalogId",
                table: "CpcResourceNickname",
                column: "CoperateProductCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CpcVendor_CoperateProductCatalogId",
                table: "CpcVendor",
                column: "CoperateProductCatalogId");

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
                name: "IX_Person_AddressId",
                table: "Person",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_EmailId",
                table: "Person",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_LandPhoneId",
                table: "Person",
                column: "LandPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_MobilePhoneId",
                table: "Person",
                column: "MobilePhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_SalutationId",
                table: "Person",
                column: "SalutationId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_SkypeId",
                table: "Person",
                column: "SkypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_UsersId",
                table: "Person",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_WhatsappId",
                table: "Person",
                column: "WhatsappId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_AddressId",
                table: "PersonCompany",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_CompanyId",
                table: "PersonCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_EmailId",
                table: "PersonCompany",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_LandPhoneId",
                table: "PersonCompany",
                column: "LandPhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_MobilePhoneId",
                table: "PersonCompany",
                column: "MobilePhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_PersonId",
                table: "PersonCompany",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_SkypeId",
                table: "PersonCompany",
                column: "SkypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_UsersId",
                table: "PersonCompany",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_WhatsappId",
                table: "PersonCompany",
                column: "WhatsappId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonCompany_WorkingStatusId",
                table: "PersonCompany",
                column: "WorkingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectManagementLevelId",
                table: "ProjectDefinition",
                column: "ProjectManagementLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectTemplateId",
                table: "ProjectDefinition",
                column: "ProjectTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectToleranceStateId",
                table: "ProjectDefinition",
                column: "ProjectToleranceStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_ProjectTypeId",
                table: "ProjectDefinition",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinition_SequenceCode",
                table: "ProjectDefinition",
                column: "SequenceCode",
                unique: true,
                filter: "[SequenceCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDefinitionHistoryLog_ChangedByUserId",
                table: "ProjectDefinitionHistoryLog",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFinance_CurrencyId",
                table: "ProjectFinance",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFinance_ProjectId",
                table: "ProjectFinance",
                column: "ProjectId",
                unique: true,
                filter: "[ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHistoryLog_CreatedByUserId",
                table: "ProjectHistoryLog",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHistoryLog_ModifiedByUserId",
                table: "ProjectHistoryLog",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHistoryLog_ProjectId",
                table: "ProjectHistoryLog",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectHistoryLog_ProjectStateId",
                table: "ProjectHistoryLog",
                column: "ProjectStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectKPI_ProjectId",
                table: "ProjectKPI",
                column: "ProjectId",
                unique: true,
                filter: "[ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_ProjectEngineerId",
                table: "ProjectTeam",
                column: "ProjectEngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_ProjectId",
                table: "ProjectTeam",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_ProjectManagerId",
                table: "ProjectTeam",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_ProjectOwnerId",
                table: "ProjectTeam",
                column: "ProjectOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTime_CalendarTemplateId",
                table: "ProjectTime",
                column: "CalendarTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTime_ProjectId",
                table: "ProjectTime",
                column: "ProjectId",
                unique: true,
                filter: "[ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QRCode_ActivityTypeId",
                table: "QRCode",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QRCode_ProjectId",
                table: "QRCode",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_UserId",
                table: "Shifts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_WorkflowStateId",
                table: "Shifts",
                column: "WorkflowStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Skype_SkypeNumber",
                table: "Skype",
                column: "SkypeNumber",
                unique: true,
                filter: "[SkypeNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClock_LocationId",
                table: "TimeClock",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClock_QRCodeId",
                table: "TimeClock",
                column: "QRCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClock_ShiftId",
                table: "TimeClock",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClock_UserId",
                table: "TimeClock",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Whatsapp_WhatsappNumber",
                table: "Whatsapp",
                column: "WhatsappNumber",
                unique: true,
                filter: "[WhatsappNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConfigurationData");

            migrationBuilder.DropTable(
                name: "CabHistoryLog");

            migrationBuilder.DropTable(
                name: "CpcHistoryLog");

            migrationBuilder.DropTable(
                name: "CpcImage");

            migrationBuilder.DropTable(
                name: "CpcResourceNickname");

            migrationBuilder.DropTable(
                name: "CpcVendor");

            migrationBuilder.DropTable(
                name: "ErrorMessage");

            migrationBuilder.DropTable(
                name: "LastSeenProjectDefinition");

            migrationBuilder.DropTable(
                name: "Locales");

            migrationBuilder.DropTable(
                name: "LocalizedData");

            migrationBuilder.DropTable(
                name: "PersonCompany");

            migrationBuilder.DropTable(
                name: "ProjectDefinitionHistoryLog");

            migrationBuilder.DropTable(
                name: "ProjectFinance");

            migrationBuilder.DropTable(
                name: "ProjectHistoryLog");

            migrationBuilder.DropTable(
                name: "ProjectKPI");

            migrationBuilder.DropTable(
                name: "ProjectTeam");

            migrationBuilder.DropTable(
                name: "ProjectTime");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "TimeClock");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "CoperateProductCatalog");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "WorkingStatus");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "ProjectState");

            migrationBuilder.DropTable(
                name: "CalendarTemplate");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "QRCode");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "CpcResourceFamily");

            migrationBuilder.DropTable(
                name: "CpcResourceType");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "TimeClockActivityType");

            migrationBuilder.DropTable(
                name: "ProjectDefinition");

            migrationBuilder.DropTable(
                name: "WorkflowState");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "LandPhone");

            migrationBuilder.DropTable(
                name: "MobilePhone");

            migrationBuilder.DropTable(
                name: "Salutation");

            migrationBuilder.DropTable(
                name: "Skype");

            migrationBuilder.DropTable(
                name: "AllUsers");

            migrationBuilder.DropTable(
                name: "Whatsapp");

            migrationBuilder.DropTable(
                name: "ProjectManagementLevel");

            migrationBuilder.DropTable(
                name: "ProjectTemplate");

            migrationBuilder.DropTable(
                name: "ProjectToleranceState");

            migrationBuilder.DropTable(
                name: "ProjectType");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
