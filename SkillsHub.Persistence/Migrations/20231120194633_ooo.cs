using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ooo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "CourceNames",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourceNames_TeacherId",
                table: "CourceNames",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourceNames_Teachers_TeacherId",
                table: "CourceNames",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "Groups",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourceNames_Teachers_TeacherId",
                table: "CourceNames");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_CourceNames_TeacherId",
                table: "CourceNames");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "CourceNames");
        }
    }
}
