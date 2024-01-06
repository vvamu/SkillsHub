using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class oaoaooaoaoaoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessages_AspNetUsers_UserId",
                table: "NotificationMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessages_Groups_GroupId",
                table: "NotificationMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessages_Students_StudentId",
                table: "NotificationMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessages_Teachers_TeacherId",
                table: "NotificationMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestLessons_Lessons_LessonAfterId",
                table: "RequestLessons");

            migrationBuilder.DropIndex(
                name: "IX_RequestLessons_LessonAfterId",
                table: "RequestLessons");

            migrationBuilder.DropIndex(
                name: "IX_NotificationMessages_GroupId",
                table: "NotificationMessages");

            migrationBuilder.DropIndex(
                name: "IX_NotificationMessages_StudentId",
                table: "NotificationMessages");

            migrationBuilder.DropIndex(
                name: "IX_NotificationMessages_TeacherId",
                table: "NotificationMessages");

            migrationBuilder.DropColumn(
                name: "LessonAfterId",
                table: "RequestLessons");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "NotificationMessages");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "NotificationMessages");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "NotificationMessages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NotificationMessages",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationMessages_UserId",
                table: "NotificationMessages",
                newName: "IX_NotificationMessages_SenderId");

            migrationBuilder.AddColumn<string>(
                name: "RequestMessage",
                table: "RequestLessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationMessageId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NotificationMessageId",
                table: "AspNetUsers",
                column: "NotificationMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_NotificationMessages_NotificationMessageId",
                table: "AspNetUsers",
                column: "NotificationMessageId",
                principalTable: "NotificationMessages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessages_AspNetUsers_SenderId",
                table: "NotificationMessages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_NotificationMessages_NotificationMessageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMessages_AspNetUsers_SenderId",
                table: "NotificationMessages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NotificationMessageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RequestMessage",
                table: "RequestLessons");

            migrationBuilder.DropColumn(
                name: "NotificationMessageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "NotificationMessages",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationMessages_SenderId",
                table: "NotificationMessages",
                newName: "IX_NotificationMessages_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonAfterId",
                table: "RequestLessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "NotificationMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "NotificationMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "NotificationMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestLessons_LessonAfterId",
                table: "RequestLessons",
                column: "LessonAfterId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessages_GroupId",
                table: "NotificationMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessages_StudentId",
                table: "NotificationMessages",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessages_TeacherId",
                table: "NotificationMessages",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessages_AspNetUsers_UserId",
                table: "NotificationMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessages_Groups_GroupId",
                table: "NotificationMessages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessages_Students_StudentId",
                table: "NotificationMessages",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMessages_Teachers_TeacherId",
                table: "NotificationMessages",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLessons_Lessons_LessonAfterId",
                table: "RequestLessons",
                column: "LessonAfterId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }
    }
}
