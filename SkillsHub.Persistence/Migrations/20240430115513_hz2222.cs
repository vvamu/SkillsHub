using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class hz2222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ApplicationUserBaseUserInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationUserBaseUserInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApplicationUserBaseUserInfo");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ApplicationUserBaseUserInfo");
        }
    }
}
