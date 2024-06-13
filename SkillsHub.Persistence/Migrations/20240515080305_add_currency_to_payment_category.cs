using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addcurrencytopaymentcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyStudent",
                table: "PaymentCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyTeacher",
                table: "PaymentCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyStudent",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "CurrencyTeacher",
                table: "PaymentCategories");
        }
    }
}
