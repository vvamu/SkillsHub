using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changesomethinkwithgroupTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypes_Subject_SubjectId",
                table: "LessonTypes");

            migrationBuilder.DropIndex(
                name: "IX_LessonTypes_SubjectId",
                table: "LessonTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GroupTeachers_GroupId",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "LessonTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers",
                columns: new[] { "GroupId", "TeacherId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "LessonTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_SubjectId",
                table: "LessonTypes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTeachers_GroupId",
                table: "GroupTeachers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypes_Subject_SubjectId",
                table: "LessonTypes",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }
    }
}
