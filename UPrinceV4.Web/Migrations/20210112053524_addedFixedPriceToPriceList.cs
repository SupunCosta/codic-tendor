using Microsoft.EntityFrameworkCore.Migrations;

namespace UPrinceV4.Web.Migrations
{
    public partial class addedFixedPriceToPriceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FixedPrice",
                table: "ResourceItemPriceList",
                type: "float",
                nullable: true);

            migrationBuilder.Sql("delete from PbsExperienceLocalizedData where PbsExperienceId in ('2c945d24-3937-47c6-a793-d82f6b53d0c7','b08b0641-e260-4750-8141-3cd8c25f6344', '1a56fb81-8308-4f72-97c7-32bb0692d297', 'df186961-6453-4c42-af53-c8866684a075','ea27ee00-8b38-48b6-8cc7-6872dc3cf09c', '49c32125-f8c2-498b-83a7-48c4a8d112f1')");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Middelmatig' where Id = '0e1ce56e-6d8a-46d3-b46e-3a3d04107ba6'");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Nieuweling' where Id = '29917bc6-37e3-4252-bf91-cabd7fd52da2'");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Ervaren' where Id = '2a173657-cced-4d51-87a9-54513f1d860a'");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Senior' where Id = '2e768fe6-3bb3-4d9c-945f-1296e49044fd'");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Beginner' where Id = '4ceb97b5-aa3b-41dc-9ede-2081bab55860'");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Expert' where Id = 'c8c06008-bb3a-4c28-8516-e98644a8f4fb'");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Getalenteerd' where Id = 'd690a006-6918-467a-b673-f5bef8aefcb9'");
            migrationBuilder.Sql("update PbsExperienceLocalizedData set Label = 'Gevorderd' where Id = 'dc7d7343-44c5-4b7f-8de1-0f79554b4b13'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedPrice",
                table: "ResourceItemPriceList");
        }
    }
}
