using Microsoft.EntityFrameworkCore.Migrations;

namespace CertificationCenter.Migrations
{
    public partial class AnswerString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6276ac93-7b60-4195-8cbc-67063a987031");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a63cc7e7-963b-4997-ba2e-1cfed1c29d80");

            migrationBuilder.AddColumn<string>(
                name: "AnswerString",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6e68a228-f40b-4430-b332-236b19c5fb06", "63f07f81-8bac-49ea-aea2-50d54b9fe0a8", "user", "USER" },
                    { "1aac6c60-2658-4bab-86b8-47e207073727", "85e47075-322a-4cce-9fa3-d118cb27f5ca", "admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1aac6c60-2658-4bab-86b8-47e207073727");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6e68a228-f40b-4430-b332-236b19c5fb06");

            migrationBuilder.DropColumn(
                name: "AnswerString",
                table: "Questions");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6276ac93-7b60-4195-8cbc-67063a987031", "657a76db-8d1c-4268-ab70-7006f0321481", "user", "USER" },
                    { "a63cc7e7-963b-4997-ba2e-1cfed1c29d80", "163fbe6d-b927-4c7c-9ed1-38d990b7043d", "admin", "ADMIN" }
                });
        }
    }
}
