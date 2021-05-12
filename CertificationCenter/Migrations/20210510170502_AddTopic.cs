using Microsoft.EntityFrameworkCore.Migrations;

namespace CertificationCenter.Migrations
{
    public partial class AddTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Certifications_CertificationId",
                table: "Questions");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1aac6c60-2658-4bab-86b8-47e207073727");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6e68a228-f40b-4430-b332-236b19c5fb06");

            migrationBuilder.AddColumn<string>(
                name: "TopicId",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TopicId",
                table: "Certifications",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "092e79ce-9495-4442-b566-f3fa8a9b8b5b", "0dfbbc99-e377-4793-84a0-3c192265a556", "user", "USER" },
                    { "922a4d15-bc15-4b62-8c16-386ce6dfd01d", "9e19294b-4461-44ca-8649-73a63837dd65", "admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_TopicId",
                table: "Certifications",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_Topics_TopicId",
                table: "Certifications",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Topics_CertificationId",
                table: "Questions",
                column: "CertificationId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_Topics_TopicId",
                table: "Certifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Topics_CertificationId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Certifications_TopicId",
                table: "Certifications");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "092e79ce-9495-4442-b566-f3fa8a9b8b5b");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "922a4d15-bc15-4b62-8c16-386ce6dfd01d");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Certifications");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6e68a228-f40b-4430-b332-236b19c5fb06", "63f07f81-8bac-49ea-aea2-50d54b9fe0a8", "user", "USER" },
                    { "1aac6c60-2658-4bab-86b8-47e207073727", "85e47075-322a-4cce-9fa3-d118cb27f5ca", "admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Certifications_CertificationId",
                table: "Questions",
                column: "CertificationId",
                principalTable: "Certifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
