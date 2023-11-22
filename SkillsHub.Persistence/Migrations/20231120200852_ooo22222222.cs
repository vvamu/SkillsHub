using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ooo22222222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "GroupId",
                table: "Teachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_GroupId",
                table: "Teachers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Groups_GroupId",
                table: "Teachers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Groups_GroupId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_GroupId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Teachers");

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
                principalColumn: "Id");
        }
    }
}
