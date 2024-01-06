using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class okok2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Cources_CourceId1",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "CourceId1",
                table: "Groups",
                newName: "CourseNameId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceId1",
                table: "Groups",
                newName: "IX_Groups_CourceNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId",
                table: "Groups",
                column: "CourseNameId",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "CourseNameId",
                table: "Groups",
                newName: "CourceId1");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceNameId",
                table: "Groups",
                newName: "IX_Groups_CourceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Cources_CourceId1",
                table: "Groups",
                column: "CourceId1",
                principalTable: "Cources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
