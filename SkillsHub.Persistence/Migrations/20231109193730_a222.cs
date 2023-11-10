using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class a222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonTypes_LessonTypeId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "DefaultWorkingTime",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "MaxArrivedStudents",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "MaxArrivedStudents",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "LessonTypes",
                newName: "Task");

            migrationBuilder.RenameColumn(
                name: "LessonTypeId",
                table: "Lessons",
                newName: "LessonActivityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_LessonTypeId",
                table: "Lessons",
                newName: "IX_Lessons_LessonActivityTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LessonTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "LessonStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourceId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LessonType",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkToWebinar",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CourceId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LessonTypeId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CourceNames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourceNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnglishLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnglishLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonActivityTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TeacherPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonActivityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnglishLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxArrivedStudents = table.Column<int>(type: "int", nullable: false),
                    MinimumAge = table.Column<int>(type: "int", nullable: false),
                    LessonTimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cources_CourceNames_NameId",
                        column: x => x.NameId,
                        principalTable: "CourceNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cources_EnglishLevels_EnglishLevelId",
                        column: x => x.EnglishLevelId,
                        principalTable: "EnglishLevels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleDays_Cources_CourceId",
                        column: x => x.CourceId,
                        principalTable: "Cources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_GroupId",
                table: "LessonStudents",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourceId",
                table: "Lessons",
                column: "CourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CourceId",
                table: "Groups",
                column: "CourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LessonTypeId",
                table: "Groups",
                column: "LessonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cources_EnglishLevelId",
                table: "Cources",
                column: "EnglishLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cources_NameId",
                table: "Cources",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDays_CourceId",
                table: "ScheduleDays",
                column: "CourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Cources_CourceId",
                table: "Groups",
                column: "CourceId",
                principalTable: "Cources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId",
                table: "Groups",
                column: "LessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Cources_CourceId",
                table: "Lessons",
                column: "CourceId",
                principalTable: "Cources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonActivityTypes_LessonActivityTypeId",
                table: "Lessons",
                column: "LessonActivityTypeId",
                principalTable: "LessonActivityTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_Groups_GroupId",
                table: "LessonStudents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Cources_CourceId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Cources_CourceId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonActivityTypes_LessonActivityTypeId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_Groups_GroupId",
                table: "LessonStudents");

            migrationBuilder.DropTable(
                name: "LessonActivityTypes");

            migrationBuilder.DropTable(
                name: "ScheduleDays");

            migrationBuilder.DropTable(
                name: "Cources");

            migrationBuilder.DropTable(
                name: "CourceNames");

            migrationBuilder.DropTable(
                name: "EnglishLevels");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_GroupId",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourceId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CourceId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_LessonTypeId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LessonTypes");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "CourceId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonType",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LinkToWebinar",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CourceId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LessonTypeId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "Task",
                table: "LessonTypes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "LessonActivityTypeId",
                table: "Lessons",
                newName: "LessonTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_LessonActivityTypeId",
                table: "Lessons",
                newName: "IX_Lessons_LessonTypeId");

            migrationBuilder.AddColumn<int>(
                name: "DefaultWorkingTime",
                table: "LessonTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxArrivedStudents",
                table: "LessonTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxArrivedStudents",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupStudents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupStudents_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GroupStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_GroupId",
                table: "GroupStudents",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_StudentId",
                table: "GroupStudents",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonTypes_LessonTypeId",
                table: "Lessons",
                column: "LessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id");
        }
    }
}
