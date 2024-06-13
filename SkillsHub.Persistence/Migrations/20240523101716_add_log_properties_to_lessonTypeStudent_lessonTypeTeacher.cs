using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addlogpropertiestolessonTypeStudentlessonTypeTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "LessonTypeTeachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LessonTypeTeachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "LessonTypeTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "LessonTypeStudents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LessonTypeStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "LessonTypeStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypeTeachers_ParentId",
                table: "LessonTypeTeachers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypeStudents_ParentId",
                table: "LessonTypeStudents",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypeStudents_LessonTypeStudents_ParentId",
                table: "LessonTypeStudents",
                column: "ParentId",
                principalTable: "LessonTypeStudents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypeTeachers_LessonTypeTeachers_ParentId",
                table: "LessonTypeTeachers",
                column: "ParentId",
                principalTable: "LessonTypeTeachers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypeStudents_LessonTypeStudents_ParentId",
                table: "LessonTypeStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypeTeachers_LessonTypeTeachers_ParentId",
                table: "LessonTypeTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonTypeTeachers_ParentId",
                table: "LessonTypeTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonTypeStudents_ParentId",
                table: "LessonTypeStudents");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "LessonTypeTeachers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LessonTypeTeachers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "LessonTypeTeachers");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "LessonTypeStudents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LessonTypeStudents");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "LessonTypeStudents");
        }
    }
}
