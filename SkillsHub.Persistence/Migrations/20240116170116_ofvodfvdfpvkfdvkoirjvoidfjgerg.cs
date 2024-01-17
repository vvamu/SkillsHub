using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ofvodfvdfpvkfdvkoirjvoidfjgerg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountPayedLessons",
                table: "Students");

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Teachers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentAmount",
                table: "Students",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PaymentAmount",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "CountPayedLessons",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
