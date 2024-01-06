using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class grtgergregregrgef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourceNames_Students_StudentId",
                table: "CourseNames");

            migrationBuilder.DropForeignKey(
                name: "FK_CourceNames_Teachers_TeacherId",
                table: "CourseNames");

            migrationBuilder.DropIndex(
                name: "IX_CourceNames_StudentId",
                table: "CourseNames");

            migrationBuilder.DropIndex(
                name: "IX_CourceNames_TeacherId",
                table: "CourseNames");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "CourseNames");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "CourseNames");

            migrationBuilder.CreateTable(
                name: "CourseNameStudent",
                columns: table => new
                {
                    PossibleCourcesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourceNameStudent", x => new { x.PossibleCourcesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourceNameStudent_CourceNames_PossibleCourcesId",
                        column: x => x.PossibleCourcesId,
                        principalTable: "CourseNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourceNameStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourceNameTeacher",
                columns: table => new
                {
                    PossibleCourcesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeachersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourceNameTeacher", x => new { x.PossibleCourcesId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_CourceNameTeacher_CourceNames_PossibleCourcesId",
                        column: x => x.PossibleCourcesId,
                        principalTable: "CourseNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourceNameTeacher_Teachers_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourceNameStudent_StudentsId",
                table: "CourseNameStudent",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourceNameTeacher_TeachersId",
                table: "CourceNameTeacher",
                column: "TeachersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseNameStudent");

            migrationBuilder.DropTable(
                name: "CourceNameTeacher");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "CourseNames",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "CourseNames",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourceNames_StudentId",
                table: "CourseNames",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourceNames_TeacherId",
                table: "CourseNames",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourceNames_Students_StudentId",
                table: "CourseNames",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourceNames_Teachers_TeacherId",
                table: "CourseNames",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
