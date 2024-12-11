using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnewmigra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentId",
                table: "GroupTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentId",
                table: "LessonStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentId",
                table: "LessonTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonTeachers_ParentId",
                table: "LessonTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_ParentId",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupTeachers_ParentId",
                table: "GroupTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_ParentId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "GroupStudents");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTeachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonStudents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "GroupTeachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "GroupStudents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTeachers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "LessonTeachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LessonTeachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "LessonTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonStudents",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "LessonStudents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LessonStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "LessonStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "GroupTeachers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "GroupTeachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GroupTeachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "GroupTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "GroupStudents",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "GroupStudents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GroupStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "GroupStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonTeachers_ParentId",
                table: "LessonTeachers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_ParentId",
                table: "LessonStudents",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTeachers_ParentId",
                table: "GroupTeachers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_ParentId",
                table: "GroupStudents",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentId",
                table: "GroupStudents",
                column: "ParentId",
                principalTable: "GroupStudents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentId",
                table: "GroupTeachers",
                column: "ParentId",
                principalTable: "GroupTeachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentId",
                table: "LessonStudents",
                column: "ParentId",
                principalTable: "LessonStudents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentId",
                table: "LessonTeachers",
                column: "ParentId",
                principalTable: "LessonTeachers",
                principalColumn: "Id");
        }
    }
}
