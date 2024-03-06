using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UPrinceV4.Web.Migrations
{
    /// <inheritdoc />
    public partial class mcForContractorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "WHRockCpcImage",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasurementCode",
                table: "PublishedContractorsPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteManagerId",
                table: "ProjectDefinition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCiawEnabled",
                table: "ProjectCiawSite",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IsAccept",
                table: "PmolPersonComment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "HRHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingOrganization",
                table: "HRHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasurementCode",
                table: "ContractorPdfData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CIAWReferenceId",
                table: "CiawHeader",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMailSend",
                table: "CiawHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PresenceRegistrationId",
                table: "CiawHeader",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CiawCancelJson",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelFailJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawCancelJson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CiawCancelStatus",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawCancelStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CiawError",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    errorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    errorDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawError", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CiawResponseJson",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CiawReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuccessJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CiawResponseJson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExactOnline",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExactOnlineEndpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExactOnline", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectConfiguration",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectConfiguration", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CiawStatus",
                columns: new[] { "Id", "DisplayOrder", "LanguageCode", "Name", "StatusId" },
                values: new object[,]
                {
                    { "77143c60-ff00-cancl-ciaws-213d2d1c5f5a", "2", "nl", "Cancelled(nl)", "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce" },
                    { "77143c60-ff45-cancl-ciaws-213d2d1c5f5a", "2", "en", "Cancelled", "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce" }
                });

            migrationBuilder.InsertData(
                table: "StockActivityType",
                columns: new[] { "Id", "ActivityName", "ActivityTypeId", "DisplayOrder", "LanguageCode" },
                values: new object[,]
                {
                    { "4e01a893-0001-48af-inst-b7a93ebd1cnl", "Stock Counting", "7bcb4e8d-8e8c-inst-81sc-6b91c89fc3da", 3, "en" },
                    { "4e01a893-0002-48af-outs-b7a93ebd1cnl", "Stock Counting(nl)", "7bcb4e8d-8e8c-inst-81sc-6b91c89fc3da", 3, "nl" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CiawCancelJson");

            migrationBuilder.DropTable(
                name: "CiawCancelStatus");

            migrationBuilder.DropTable(
                name: "CiawError");

            migrationBuilder.DropTable(
                name: "CiawResponseJson");

            migrationBuilder.DropTable(
                name: "ExactOnline");

            migrationBuilder.DropTable(
                name: "ProjectConfiguration");

            migrationBuilder.DeleteData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff00-cancl-ciaws-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "CiawStatus",
                keyColumn: "Id",
                keyValue: "77143c60-ff45-cancl-ciaws-213d2d1c5f5a");

            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0001-48af-inst-b7a93ebd1cnl");

            migrationBuilder.DeleteData(
                table: "StockActivityType",
                keyColumn: "Id",
                keyValue: "4e01a893-0002-48af-outs-b7a93ebd1cnl");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "WHRockCpcImage");

            migrationBuilder.DropColumn(
                name: "MeasurementCode",
                table: "PublishedContractorsPdfData");

            migrationBuilder.DropColumn(
                name: "SiteManagerId",
                table: "ProjectDefinition");

            migrationBuilder.DropColumn(
                name: "IsCiawEnabled",
                table: "ProjectCiawSite");

            migrationBuilder.DropColumn(
                name: "IsAccept",
                table: "PmolPersonComment");

            migrationBuilder.DropColumn(
                name: "Organization",
                table: "HRHeader");

            migrationBuilder.DropColumn(
                name: "WorkingOrganization",
                table: "HRHeader");

            migrationBuilder.DropColumn(
                name: "MeasurementCode",
                table: "ContractorPdfData");

            migrationBuilder.DropColumn(
                name: "CIAWReferenceId",
                table: "CiawHeader");

            migrationBuilder.DropColumn(
                name: "IsMailSend",
                table: "CiawHeader");

            migrationBuilder.DropColumn(
                name: "PresenceRegistrationId",
                table: "CiawHeader");
        }
    }
}
