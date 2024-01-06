using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class odofofofof : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay");
            /*
            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
