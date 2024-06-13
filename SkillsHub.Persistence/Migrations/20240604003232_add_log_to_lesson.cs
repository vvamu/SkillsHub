using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addlogtolesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Lessons",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "Lessons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ParentId",
                table: "Lessons",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Lessons_ParentId",
                table: "Lessons",
                column: "ParentId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Lessons_ParentId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ParentId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Lessons");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Lessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
