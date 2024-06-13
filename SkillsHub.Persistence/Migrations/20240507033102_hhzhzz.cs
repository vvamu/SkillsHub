using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class hhzhzz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DropForeignKey(
                name: "FK_DurationTypes_LessonTypes_LessonTypeId",
                table: "DurationTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_DurationTypes_DurationTypeId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_DurationTypeId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_DurationTypes_LessonTypeId",
                table: "DurationTypes");

            migrationBuilder.DropColumn(
                name: "DurationTypeId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LessonTypeId",
                table: "DurationTypes");*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DurationTypeId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LessonTypeId",
                table: "DurationTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DurationTypeId",
                table: "Groups",
                column: "DurationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DurationTypes_LessonTypeId",
                table: "DurationTypes",
                column: "LessonTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DurationTypes_LessonTypes_LessonTypeId",
                table: "DurationTypes",
                column: "LessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_DurationTypes_DurationTypeId",
                table: "Groups",
                column: "DurationTypeId",
                principalTable: "DurationTypes",
                principalColumn: "Id");
        }
    }
}
