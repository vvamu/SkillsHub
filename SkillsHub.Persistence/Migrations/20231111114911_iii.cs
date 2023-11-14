using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class iii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RusName",
                table: "Cources");

            migrationBuilder.AddColumn<string>(
                name: "Term",
                table: "Cources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Term",
                table: "Cources");

            migrationBuilder.AddColumn<string>(
                name: "RusName",
                table: "Cources",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
