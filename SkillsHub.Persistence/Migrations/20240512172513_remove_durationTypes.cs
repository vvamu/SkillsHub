using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removedurationTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgeTypes_GroupTypes_ParentId",
                table: "AgeTypes");

            /* migrationBuilder.DropForeignKey(
                 name: "FK_PaymentCategories_DurationTypes_DurationTypeId",
                 table: "PaymentCategories");

             migrationBuilder.DropTable(
                 name: "DurationTypes");
            
            migrationBuilder.DropIndex(
                name: "IX_PaymentCategories_DurationTypeId",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "DurationTypeId",
                table: "PaymentCategories");*/

            migrationBuilder.DropColumn(
                name: "LessonTimeInMinutes",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "StudentPricePerLesson",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "TeacherPricePerLesson",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "MinumumLessonsToPay",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "StudentPrice",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "StudentPricePerLesson",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "TeacherPrice",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "TeacherPricePerLesson",
                table: "LessonTypes");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "BaseUserInfo",
                newName: "DateRegistration");

            migrationBuilder.AlterColumn<decimal>(
                name: "StudentPrice",
                table: "PaymentCategories",
                type: "decimal(15,4)",
                precision: 15,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "PaymentCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "Locations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRegistration",
                table: "LessonTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GroupTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegistration",
                table: "GroupTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BaseUserInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AgeTypes_AgeTypes_ParentId",
                table: "AgeTypes",
                column: "ParentId",
                principalTable: "AgeTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgeTypes_AgeTypes_ParentId",
                table: "AgeTypes");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BaseUserInfo");

            migrationBuilder.RenameColumn(
                name: "DateRegistration",
                table: "BaseUserInfo",
                newName: "RegistrationDate");

            migrationBuilder.AlterColumn<decimal>(
                name: "StudentPrice",
                table: "PaymentCategories",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,4)",
                oldPrecision: 15,
                oldScale: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "DurationTypeId",
                table: "PaymentCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonTimeInMinutes",
                table: "PaymentCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "StudentPricePerLesson",
                table: "PaymentCategories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TeacherPricePerLesson",
                table: "PaymentCategories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MinumumLessonsToPay",
                table: "LessonTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "StudentPrice",
                table: "LessonTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "StudentPricePerLesson",
                table: "LessonTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TeacherPrice",
                table: "LessonTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TeacherPricePerLesson",
                table: "LessonTypes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "GroupTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegistration",
                table: "GroupTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DurationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DurationTypes_DurationTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DurationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCategories_DurationTypeId",
                table: "PaymentCategories",
                column: "DurationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DurationTypes_ParentId",
                table: "DurationTypes",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgeTypes_GroupTypes_ParentId",
                table: "AgeTypes",
                column: "ParentId",
                principalTable: "GroupTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCategories_DurationTypes_DurationTypeId",
                table: "PaymentCategories",
                column: "DurationTypeId",
                principalTable: "DurationTypes",
                principalColumn: "Id");
        }
    }
}
