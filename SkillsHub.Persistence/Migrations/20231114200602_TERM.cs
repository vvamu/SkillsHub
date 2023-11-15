using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TERM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                table: "Cources");

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "Cources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
