using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeLessonTypeIDcol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypes_LessonTypes_ParentLessonTypeId",
                table: "LessonTypes");

            migrationBuilder.DropIndex(
                name: "IX_LessonTypes_ParentLessonTypeId",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "ParentLessonTypeId",
                table: "LessonTypes");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_ParentId",
                table: "LessonTypes",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypes_LessonTypes_ParentId",
                table: "LessonTypes",
                column: "ParentId",
                principalTable: "LessonTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypes_LessonTypes_ParentId",
                table: "LessonTypes");

            migrationBuilder.DropIndex(
                name: "IX_LessonTypes_ParentId",
                table: "LessonTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLessonTypeId",
                table: "LessonTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_ParentLessonTypeId",
                table: "LessonTypes",
                column: "ParentLessonTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypes_LessonTypes_ParentLessonTypeId",
                table: "LessonTypes",
                column: "ParentLessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id");
        }
    }
}
