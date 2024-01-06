using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addrequestLesson2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLessons_AspNetUsers_UserId",
                table: "RequestLessons");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "RequestLessons",
                newName: "AnswerStatus");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RequestLessons",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLessons_AspNetUsers_UserId",
                table: "RequestLessons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLessons_AspNetUsers_UserId",
                table: "RequestLessons");

            migrationBuilder.RenameColumn(
                name: "AnswerStatus",
                table: "RequestLessons",
                newName: "Status");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RequestLessons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLessons_AspNetUsers_UserId",
                table: "RequestLessons",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
