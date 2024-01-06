using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addrequestLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Students_StudentId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_StudentId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Groups",
                newName: "DateStart");

            migrationBuilder.CreateTable(
                name: "RequestLessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonBeforeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LessonAfterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestLessons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestLessons_Lessons_LessonAfterId",
                        column: x => x.LessonAfterId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestLessons_Lessons_LessonBeforeId",
                        column: x => x.LessonBeforeId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestLessons_LessonAfterId",
                table: "RequestLessons",
                column: "LessonAfterId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLessons_LessonBeforeId",
                table: "RequestLessons",
                column: "LessonBeforeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLessons_UserId",
                table: "RequestLessons",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents");

            migrationBuilder.DropTable(
                name: "RequestLessons");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "Groups",
                newName: "StartDate");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_StudentId",
                table: "Lessons",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Students_StudentId",
                table: "Lessons",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
