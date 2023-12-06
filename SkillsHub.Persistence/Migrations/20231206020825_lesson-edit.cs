using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class lessonedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTasks_Lessons_LessonId",
                table: "LessonTasks");

            migrationBuilder.DropIndex(
                name: "IX_LessonTasks_LessonId",
                table: "LessonTasks");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "LessonTasks");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonTaskId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonTaskId",
                table: "Lessons",
                column: "LessonTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonTasks_LessonTaskId",
                table: "Lessons",
                column: "LessonTaskId",
                principalTable: "LessonTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonTasks_LessonTaskId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LessonTaskId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonTaskId",
                table: "Lessons");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonId",
                table: "LessonTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LessonTasks_LessonId",
                table: "LessonTasks",
                column: "LessonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTasks_Lessons_LessonId",
                table: "LessonTasks",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
