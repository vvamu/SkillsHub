using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewtableteachercourses2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameStudent_CourceNames_CourseNameId",
                table: "CourseNameStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameStudent_Students_StudentId",
                table: "CourseNameStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameTeacher_CourceNames_CourseNameId",
                table: "CourseNameTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameTeacher_Teachers_TeacherId",
                table: "CourseNameTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseNameTeacher",
                table: "CourseNameTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseNameStudent",
                table: "CourseNameStudent");

            migrationBuilder.RenameTable(
                name: "CourseNameTeacher",
                newName: "CourseNameTeachers");

            migrationBuilder.RenameTable(
                name: "CourseNameStudent",
                newName: "CourseNameStudents");

            migrationBuilder.RenameIndex(
                name: "IX_CourseNameTeacher_TeacherId",
                table: "CourseNameTeachers",
                newName: "IX_CourseNameTeachers_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseNameStudent_StudentId",
                table: "CourseNameStudents",
                newName: "IX_CourseNameStudents_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseNameTeachers",
                table: "CourseNameTeachers",
                columns: new[] { "CourseNameId", "TeacherId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseNameStudents",
                table: "CourseNameStudents",
                columns: new[] { "CourseNameId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameStudents_CourceNames_CourseNameId",
                table: "CourseNameStudents",
                column: "CourseNameId",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameStudents_Students_StudentId",
                table: "CourseNameStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameTeachers_CourceNames_CourseNameId",
                table: "CourseNameTeachers",
                column: "CourseNameId",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameTeachers_Teachers_TeacherId",
                table: "CourseNameTeachers",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameStudents_CourceNames_CourseNameId",
                table: "CourseNameStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameStudents_Students_StudentId",
                table: "CourseNameStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameTeachers_CourceNames_CourseNameId",
                table: "CourseNameTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseNameTeachers_Teachers_TeacherId",
                table: "CourseNameTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseNameTeachers",
                table: "CourseNameTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseNameStudents",
                table: "CourseNameStudents");

            migrationBuilder.RenameTable(
                name: "CourseNameTeachers",
                newName: "CourseNameTeacher");

            migrationBuilder.RenameTable(
                name: "CourseNameStudents",
                newName: "CourseNameStudent");

            migrationBuilder.RenameIndex(
                name: "IX_CourseNameTeachers_TeacherId",
                table: "CourseNameTeacher",
                newName: "IX_CourseNameTeacher_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseNameStudents_StudentId",
                table: "CourseNameStudent",
                newName: "IX_CourseNameStudent_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseNameTeacher",
                table: "CourseNameTeacher",
                columns: new[] { "CourseNameId", "TeacherId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseNameStudent",
                table: "CourseNameStudent",
                columns: new[] { "CourseNameId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameStudent_CourceNames_CourseNameId",
                table: "CourseNameStudent",
                column: "CourseNameId",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameStudent_Students_StudentId",
                table: "CourseNameStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameTeacher_CourceNames_CourseNameId",
                table: "CourseNameTeacher",
                column: "CourseNameId",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNameTeacher_Teachers_TeacherId",
                table: "CourseNameTeacher",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
