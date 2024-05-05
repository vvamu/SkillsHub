using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ooo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "BaseUserInfo",
                newName: "Phones");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "BaseUserInfo",
                newName: "Emails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phones",
                table: "BaseUserInfo",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Emails",
                table: "BaseUserInfo",
                newName: "Email");
        }
    }
}
