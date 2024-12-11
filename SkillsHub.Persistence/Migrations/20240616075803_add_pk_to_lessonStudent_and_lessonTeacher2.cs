using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addpktolessonStudentandlessonTeacher2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId1",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_LessonTeachers_LessonId",
                table: "LessonTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_LessonId1",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "LessonId1",
                table: "LessonStudents");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTeachers_LessonId",
                table: "LessonTeachers",
                column: "LessonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LessonTeachers_LessonId",
                table: "LessonTeachers");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonId1",
                table: "LessonStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonTeachers_LessonId",
                table: "LessonTeachers",
                column: "LessonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_LessonId1",
                table: "LessonStudents",
                column: "LessonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId1",
                table: "LessonStudents",
                column: "LessonId1",
                principalTable: "Lessons",
                principalColumn: "Id");
        }
    }
}
