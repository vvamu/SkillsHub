using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ooo222222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Groups");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId1",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TeacherId1",
                table: "Groups",
                column: "TeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Teachers_TeacherId1",
                table: "Groups",
                column: "TeacherId1",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Teachers_TeacherId1",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TeacherId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TeacherId1",
                table: "Groups");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "Groups",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
