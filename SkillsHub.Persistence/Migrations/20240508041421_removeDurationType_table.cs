using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeDurationTypetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DurationTypeStudentName",
                table: "PaymentCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationTypeStudentValue",
                table: "PaymentCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DurationTypeTeacherName",
                table: "PaymentCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationTypeTeacherValue",
                table: "PaymentCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "StudentPricePerLesson",
                table: "PaymentCategories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TeacherPricePerLesson",
                table: "PaymentCategories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DurationTypeName",
                table: "LessonTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationTypeValue",
                table: "LessonTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "StudentPricePerLesson",
                table: "LessonTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TeacherPricePerLesson",
                table: "LessonTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationTypeStudentName",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "DurationTypeStudentValue",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "DurationTypeTeacherName",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "DurationTypeTeacherValue",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "StudentPricePerLesson",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "TeacherPricePerLesson",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "DurationTypeName",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "DurationTypeValue",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "StudentPricePerLesson",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "TeacherPricePerLesson",
                table: "LessonTypes");
        }
    }
}
