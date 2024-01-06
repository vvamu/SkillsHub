using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewtableteachercourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseNameStudent");

            migrationBuilder.DropTable(
                name: "CourceNameTeacher");

            migrationBuilder.CreateTable(
                name: "CourseNameStudent",
                columns: table => new
                {
                    CourseNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseNameStudent", x => new { x.CourseNameId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_CourseNameStudent_CourceNames_CourseNameId",
                        column: x => x.CourseNameId,
                        principalTable: "CourseNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseNameStudent_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseNameTeacher",
                columns: table => new
                {
                    CourseNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseNameTeacher", x => new { x.CourseNameId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_CourseNameTeacher_CourceNames_CourseNameId",
                        column: x => x.CourseNameId,
                        principalTable: "CourseNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseNameTeacher_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseNameStudent_StudentId",
                table: "CourseNameStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseNameTeacher_TeacherId",
                table: "CourseNameTeacher",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseNameStudent");

            migrationBuilder.DropTable(
                name: "CourseNameTeacher");

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
    }
}
