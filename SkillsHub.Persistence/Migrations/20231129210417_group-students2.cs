using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class groupstudents2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Students_StudentId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Groups_GroupId",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_GroupId",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_Groups_StudentId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Groups");

            migrationBuilder.CreateTable(
                name: "GroupStudent",
                columns: table => new
                {
                    ArrivedStudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupStudent", x => new { x.ArrivedStudentsId, x.GroupsId });
                    table.ForeignKey(
                        name: "FK_GroupStudent_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GroupStudent_Students_ArrivedStudentsId",
                        column: x => x.ArrivedStudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudent_GroupsId",
                table: "GroupStudent",
                column: "GroupsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupStudent");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "LessonStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_GroupId",
                table: "LessonStudents",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_StudentId",
                table: "Groups",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Students_StudentId",
                table: "Groups",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Groups_GroupId",
                table: "LessonStudents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
