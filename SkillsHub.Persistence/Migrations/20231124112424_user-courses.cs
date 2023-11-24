using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class usercourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "CourceNames",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourceNames_StudentId",
                table: "CourceNames",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourceNames_Students_StudentId",
                table: "CourceNames",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourceNames_Students_StudentId",
                table: "CourceNames");

            migrationBuilder.DropIndex(
                name: "IX_CourceNames_StudentId",
                table: "CourceNames");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "CourceNames");
        }
    }
}
