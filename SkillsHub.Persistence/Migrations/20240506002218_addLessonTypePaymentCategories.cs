using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addLessonTypePaymentCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonTypePaymentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LessonTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTypePaymentCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonTypePaymentCategories_LessonTypes_LessonTypeId",
                        column: x => x.LessonTypeId,
                        principalTable: "LessonTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonTypePaymentCategories_PaymentCategories_PaymentCategoryId",
                        column: x => x.PaymentCategoryId,
                        principalTable: "PaymentCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypePaymentCategories_LessonTypeId",
                table: "LessonTypePaymentCategories",
                column: "LessonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypePaymentCategories_PaymentCategoryId",
                table: "LessonTypePaymentCategories",
                column: "PaymentCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonTypePaymentCategories");
        }
    }
}
