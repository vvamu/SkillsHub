using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addpktolessonStudentandlessonTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentGroupId_ParentStudentId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonTeachers",
                table: "LessonTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonStudents",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupStudents",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_ParentGroupId_ParentStudentId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "ParentLessonId",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "ParentTeacherId",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "ParentLessonId",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ParentGroupId",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "ParentTeacherId",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "ParentGroupId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "ParentStudentId",
                table: "GroupStudents");

            migrationBuilder.RenameColumn(
                name: "ParentStudentId",
                table: "LessonStudents",
                newName: "LessonId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonTeachers",
                table: "LessonTeachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonStudents",
                table: "LessonStudents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupStudents",
                table: "GroupStudents",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTeachers_ParentId",
                table: "LessonTeachers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_LessonId",
                table: "LessonStudents",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_LessonId1",
                table: "LessonStudents",
                column: "LessonId1");

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_ParentId",
                table: "LessonStudents",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTeachers_GroupId",
                table: "GroupTeachers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTeachers_ParentId",
                table: "GroupTeachers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_GroupId",
                table: "GroupStudents",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_ParentId",
                table: "GroupStudents",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentId",
                table: "GroupStudents",
                column: "ParentId",
                principalTable: "GroupStudents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentId",
                table: "GroupTeachers",
                column: "ParentId",
                principalTable: "GroupTeachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentId",
                table: "LessonStudents",
                column: "ParentId",
                principalTable: "LessonStudents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId1",
                table: "LessonStudents",
                column: "LessonId1",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentId",
                table: "LessonTeachers",
                column: "ParentId",
                principalTable: "LessonTeachers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentId",
                table: "GroupTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentId",
                table: "LessonStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId1",
                table: "LessonStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentId",
                table: "LessonTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonTeachers",
                table: "LessonTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonTeachers_ParentId",
                table: "LessonTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonStudents",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_LessonId",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_LessonId1",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_ParentId",
                table: "LessonStudents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GroupTeachers_GroupId",
                table: "GroupTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GroupTeachers_ParentId",
                table: "GroupTeachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupStudents",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_GroupId",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_ParentId",
                table: "GroupStudents");

            migrationBuilder.RenameColumn(
                name: "LessonId1",
                table: "LessonStudents",
                newName: "ParentStudentId");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLessonId",
                table: "LessonTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentTeacherId",
                table: "LessonTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLessonId",
                table: "LessonStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentGroupId",
                table: "GroupTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentTeacherId",
                table: "GroupTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentGroupId",
                table: "GroupStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentStudentId",
                table: "GroupStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonTeachers",
                table: "LessonTeachers",
                columns: new[] { "LessonId", "TeacherId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonStudents",
                table: "LessonStudents",
                columns: new[] { "LessonId", "StudentId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupTeachers",
                table: "GroupTeachers",
                columns: new[] { "GroupId", "TeacherId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupStudents",
                table: "GroupStudents",
                columns: new[] { "GroupId", "StudentId" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers",
                columns: new[] { "ParentLessonId", "ParentTeacherId" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents",
                columns: new[] { "ParentLessonId", "ParentStudentId" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers",
                columns: new[] { "ParentGroupId", "ParentTeacherId" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_ParentGroupId_ParentStudentId",
                table: "GroupStudents",
                columns: new[] { "ParentGroupId", "ParentStudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentGroupId_ParentStudentId",
                table: "GroupStudents",
                columns: new[] { "ParentGroupId", "ParentStudentId" },
                principalTable: "GroupStudents",
                principalColumns: new[] { "GroupId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers",
                columns: new[] { "ParentGroupId", "ParentTeacherId" },
                principalTable: "GroupTeachers",
                principalColumns: new[] { "GroupId", "TeacherId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents",
                columns: new[] { "ParentLessonId", "ParentStudentId" },
                principalTable: "LessonStudents",
                principalColumns: new[] { "LessonId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Lessons_LessonId",
                table: "LessonStudents",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers",
                columns: new[] { "ParentLessonId", "ParentTeacherId" },
                principalTable: "LessonTeachers",
                principalColumns: new[] { "LessonId", "TeacherId" });
        }
    }
}
