using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addgroupType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupType",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "MaxCountStudents",
                table: "LessonTypePaymentCategories");

            migrationBuilder.RenameColumn(
                name: "MinCountStudents",
                table: "LessonTypePaymentCategories",
                newName: "MonthDuration");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupTypeId",
                table: "LessonTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinCountStudents = table.Column<int>(type: "int", nullable: false),
                    MaxCountStudents = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_GroupTypeId",
                table: "LessonTypes",
                column: "GroupTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypes_GroupTypes_GroupTypeId",
                table: "LessonTypes",
                column: "GroupTypeId",
                principalTable: "GroupTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypes_GroupTypes_GroupTypeId",
                table: "LessonTypes");

            migrationBuilder.DropTable(
                name: "GroupTypes");

            migrationBuilder.DropIndex(
                name: "IX_LessonTypes_GroupTypeId",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "GroupTypeId",
                table: "LessonTypes");

            migrationBuilder.RenameColumn(
                name: "MonthDuration",
                table: "LessonTypePaymentCategories",
                newName: "MinCountStudents");

            migrationBuilder.AddColumn<string>(
                name: "GroupType",
                table: "LessonTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxCountStudents",
                table: "LessonTypePaymentCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
