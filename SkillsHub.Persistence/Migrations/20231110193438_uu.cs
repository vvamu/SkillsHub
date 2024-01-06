using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class uu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Groups_GroupId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_GroupId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Task",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "LessonTimeInMinutes",
                table: "Cources");

            migrationBuilder.AddColumn<int>(
                name: "LessonTimeInMinutes",
                table: "LessonTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinumumLessonsToPay",
                table: "LessonTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumAge",
                table: "CourseNames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumAge",
                table: "CourseNames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_StudentId",
                table: "Groups",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Students_StudentId",
                table: "Groups",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Students_StudentId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_StudentId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LessonTimeInMinutes",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "MinumumLessonsToPay",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "MaximumAge",
                table: "CourseNames");

            migrationBuilder.DropColumn(
                name: "MinimumAge",
                table: "CourseNames");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Students",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Task",
                table: "LessonTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonTimeInMinutes",
                table: "Cources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_GroupId",
                table: "Students",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Groups_GroupId",
                table: "Students",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
