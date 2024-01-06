using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class workingdays2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Students_StudentId",
                table: "WorkingDay");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Teachers_TeacherId",
                table: "WorkingDay");
            */
            /*
            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Students_StudentId",
                table: "WorkingDay",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);


            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Teachers_TeacherId",
                table: "WorkingDay",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Students_StudentId",
                table: "WorkingDay");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Teachers_TeacherId",
                table: "WorkingDay");


            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Students_StudentId",
                table: "WorkingDay",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Teachers_TeacherId",
                table: "WorkingDay",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
            */
        }
    }
}
