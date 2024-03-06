using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class AddedBasicUnitOfMeasure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_Address_AddressId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_Company_CompanyId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_Email_EmailId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_LandPhone_LandPhoneId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_MobilePhone_MobilePhoneId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_Person_PersonId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_Skype_SkypeId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_AllUsers_UsersId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_Whatsapp_WhatsappId",
                table: "PersonCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonCompany_WorkingStatus_WorkingStatusId",
                table: "PersonCompany");

            migrationBuilder.DropIndex(
                name: "IX_Skype_SkypeNumber",
                table: "Skype");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonCompany",
                table: "PersonCompany");

            migrationBuilder.DropColumn(
                name: "SkypeNumber",
                table: "Skype");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "PersonCompany",
                newName: "ContactCompany");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_WorkingStatusId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_WorkingStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_WhatsappId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_WhatsappId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_UsersId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_SkypeId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_SkypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_PersonId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_MobilePhoneId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_MobilePhoneId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_LandPhoneId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_LandPhoneId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_EmailId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_EmailId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_CompanyId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonCompany_AddressId",
                table: "ContactCompany",
                newName: "IX_ContactCompany_AddressId");

            migrationBuilder.AddColumn<string>(
                name: "WhatsappNumber",
                table: "Skype",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobRoleId",
                table: "Person",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResourceTypeId",
                table: "CoperateProductCatalog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceTitle",
                table: "CoperateProductCatalog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<double>(
                name: "InventoryPrice",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxOrderQuantity",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinOrderQuantity",
                table: "CoperateProductCatalog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CoperateProductCatalog",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandPhoneId",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactId",
                table: "ContactCompany",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactCompany",
                table: "ContactCompany",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CpcBasicUnitOfMeasure",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: false),
                    LocaleCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpcBasicUnitOfMeasure", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_EmailId",
                table: "Company",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_LandPhoneId",
                table: "Company",
                column: "LandPhoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Email_EmailId",
                table: "Company",
                column: "EmailId",
                principalTable: "Email",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Company_LandPhone_LandPhoneId",
                table: "Company",
                column: "LandPhoneId",
                principalTable: "LandPhone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_Address_AddressId",
                table: "ContactCompany",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_Company_CompanyId",
                table: "ContactCompany",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_Email_EmailId",
                table: "ContactCompany",
                column: "EmailId",
                principalTable: "Email",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_LandPhone_LandPhoneId",
                table: "ContactCompany",
                column: "LandPhoneId",
                principalTable: "LandPhone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_MobilePhone_MobilePhoneId",
                table: "ContactCompany",
                column: "MobilePhoneId",
                principalTable: "MobilePhone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_Person_PersonId",
                table: "ContactCompany",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_Skype_SkypeId",
                table: "ContactCompany",
                column: "SkypeId",
                principalTable: "Skype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_AllUsers_UsersId",
                table: "ContactCompany",
                column: "UsersId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_Whatsapp_WhatsappId",
                table: "ContactCompany",
                column: "WhatsappId",
                principalTable: "Whatsapp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactCompany_WorkingStatus_WorkingStatusId",
                table: "ContactCompany",
                column: "WorkingStatusId",
                principalTable: "WorkingStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CoperateProductCatalog",
                column: "ResourceTypeId",
                principalTable: "CpcResourceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Email_EmailId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Company_LandPhone_LandPhoneId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_Address_AddressId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_Company_CompanyId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_Email_EmailId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_LandPhone_LandPhoneId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_MobilePhone_MobilePhoneId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_Person_PersonId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_Skype_SkypeId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_AllUsers_UsersId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_Whatsapp_WhatsappId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactCompany_WorkingStatus_WorkingStatusId",
                table: "ContactCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CoperateProductCatalog");

            migrationBuilder.DropTable(
                name: "CpcBasicUnitOfMeasure");

            migrationBuilder.DropIndex(
                name: "IX_Company_EmailId",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_LandPhoneId",
                table: "Company");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactCompany",
                table: "ContactCompany");

            migrationBuilder.DropColumn(
                name: "WhatsappNumber",
                table: "Skype");

            migrationBuilder.DropColumn(
                name: "JobRoleId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "InventoryPrice",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "MaxOrderQuantity",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "MinOrderQuantity",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CoperateProductCatalog");

            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LandPhoneId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "ContactCompany");

            migrationBuilder.RenameTable(
                name: "ContactCompany",
                newName: "PersonCompany");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_WorkingStatusId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_WorkingStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_WhatsappId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_WhatsappId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_UsersId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_SkypeId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_SkypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_PersonId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_MobilePhoneId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_MobilePhoneId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_LandPhoneId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_LandPhoneId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_EmailId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_EmailId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_CompanyId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_ContactCompany_AddressId",
                table: "PersonCompany",
                newName: "IX_PersonCompany_AddressId");

            migrationBuilder.AddColumn<string>(
                name: "SkypeNumber",
                table: "Skype",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResourceTypeId",
                table: "CoperateProductCatalog",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResourceTitle",
                table: "CoperateProductCatalog",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonCompany",
                table: "PersonCompany",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Skype_SkypeNumber",
                table: "Skype",
                column: "SkypeNumber",
                unique: true,
                filter: "[SkypeNumber] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CoperateProductCatalog_CpcResourceType_ResourceTypeId",
                table: "CoperateProductCatalog",
                column: "ResourceTypeId",
                principalTable: "CpcResourceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_Address_AddressId",
                table: "PersonCompany",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_Company_CompanyId",
                table: "PersonCompany",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_Email_EmailId",
                table: "PersonCompany",
                column: "EmailId",
                principalTable: "Email",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_LandPhone_LandPhoneId",
                table: "PersonCompany",
                column: "LandPhoneId",
                principalTable: "LandPhone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_MobilePhone_MobilePhoneId",
                table: "PersonCompany",
                column: "MobilePhoneId",
                principalTable: "MobilePhone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_Person_PersonId",
                table: "PersonCompany",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_Skype_SkypeId",
                table: "PersonCompany",
                column: "SkypeId",
                principalTable: "Skype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_AllUsers_UsersId",
                table: "PersonCompany",
                column: "UsersId",
                principalTable: "AllUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_Whatsapp_WhatsappId",
                table: "PersonCompany",
                column: "WhatsappId",
                principalTable: "Whatsapp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonCompany_WorkingStatus_WorkingStatusId",
                table: "PersonCompany",
                column: "WorkingStatusId",
                principalTable: "WorkingStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
