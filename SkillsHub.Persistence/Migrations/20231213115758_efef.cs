using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class efef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId1",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId1",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "LessonTypeId1",
                table: "Groups",
                newName: "LessonTypeId");

            migrationBuilder.RenameColumn(
                name: "CourceNameId1",
                table: "Groups",
                newName: "CourseNameId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_LessonTypeId1",
                table: "Groups",
                newName: "IX_Groups_LessonTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceNameId1",
                table: "Groups",
                newName: "IX_Groups_CourceNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId",
                table: "Groups",
                column: "CourseNameId",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId",
                table: "Groups",
                column: "LessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "LessonTypeId",
                table: "Groups",
                newName: "LessonTypeId1");

            migrationBuilder.RenameColumn(
                name: "CourseNameId",
                table: "Groups",
                newName: "CourceNameId1");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_LessonTypeId",
                table: "Groups",
                newName: "IX_Groups_LessonTypeId1");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceNameId",
                table: "Groups",
                newName: "IX_Groups_CourceNameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId1",
                table: "Groups",
                column: "CourceNameId1",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId1",
                table: "Groups",
                column: "LessonTypeId1",
                principalTable: "LessonTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
