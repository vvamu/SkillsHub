using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class okok : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Cources_CourceId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "LessonTypeId",
                table: "Groups",
                newName: "LessonTypeId1");

            migrationBuilder.RenameColumn(
                name: "CourceId",
                table: "Groups",
                newName: "CourceId1");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_LessonTypeId",
                table: "Groups",
                newName: "IX_Groups_LessonTypeId1");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceId",
                table: "Groups",
                newName: "IX_Groups_CourceId1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Cources_CourceId1",
                table: "Groups",
                column: "CourceId1",
                principalTable: "Cources",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Cources_CourceId1",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "LessonTypeId1",
                table: "Groups",
                newName: "LessonTypeId");

            migrationBuilder.RenameColumn(
                name: "CourceId1",
                table: "Groups",
                newName: "CourceId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_LessonTypeId1",
                table: "Groups",
                newName: "IX_Groups_LessonTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceId1",
                table: "Groups",
                newName: "IX_Groups_CourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Cources_CourceId",
                table: "Groups",
                column: "CourceId",
                principalTable: "Cources",
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
    }
}
