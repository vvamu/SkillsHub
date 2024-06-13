using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddlogtogroupaddtolessonTypecheckAgeTypecheckGroupType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentTypeId",
                table: "Groups",
                newName: "ParentId");

            migrationBuilder.AddColumn<bool>(
                name: "CheckAgeType",
                table: "LessonTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckGroupType",
                table: "LessonTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "Groups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ParentId",
                table: "Groups",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Groups_ParentId",
                table: "Groups",
                column: "ParentId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Groups_ParentId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ParentId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CheckAgeType",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "CheckGroupType",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Groups",
                newName: "PaymentTypeId");
        }
    }
}
