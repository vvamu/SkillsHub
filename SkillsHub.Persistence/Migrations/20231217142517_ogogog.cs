using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ogogog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishLevel",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EnglishLevelConfirmed",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsRecorded",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "EnglishLevel",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnglishLevelConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EnglishLevelConfirmed",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "EnglishLevel",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnglishLevelConfirmed",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IsRecorded",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
