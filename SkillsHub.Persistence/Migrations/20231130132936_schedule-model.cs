using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class schedulemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaySchedules_AspNetUsers_UserId",
                table: "DaySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_Students_ArrivedStudentsId",
                table: "GroupStudent");

            migrationBuilder.RenameColumn(
                name: "ArrivedStudentsId",
                table: "GroupStudent",
                newName: "GroupStudentsId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DaySchedules",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_DaySchedules_UserId",
                table: "DaySchedules",
                newName: "IX_DaySchedules_GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "DayName",
                table: "DaySchedules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "DaySchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaySchedules_ApplicationUserId",
                table: "DaySchedules",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DaySchedules_AspNetUsers_ApplicationUserId",
                table: "DaySchedules",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DaySchedules_Groups_GroupId",
                table: "DaySchedules",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_Students_GroupStudentsId",
                table: "GroupStudent",
                column: "GroupStudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaySchedules_AspNetUsers_ApplicationUserId",
                table: "DaySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_DaySchedules_Groups_GroupId",
                table: "DaySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudent_Students_GroupStudentsId",
                table: "GroupStudent");

            migrationBuilder.DropIndex(
                name: "IX_DaySchedules_ApplicationUserId",
                table: "DaySchedules");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DaySchedules");

            migrationBuilder.RenameColumn(
                name: "GroupStudentsId",
                table: "GroupStudent",
                newName: "ArrivedStudentsId");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "DaySchedules",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DaySchedules_GroupId",
                table: "DaySchedules",
                newName: "IX_DaySchedules_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "DayName",
                table: "DaySchedules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DaySchedules_AspNetUsers_UserId",
                table: "DaySchedules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudent_Students_ArrivedStudentsId",
                table: "GroupStudent",
                column: "ArrivedStudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
