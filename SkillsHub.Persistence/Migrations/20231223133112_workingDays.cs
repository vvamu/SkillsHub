using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class workingDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Students_StudentId",
                table: "WorkingDays");

            migrationBuilder.DropTable(
                name: "DaySchedules");

            migrationBuilder.DropTable(
                name: "EnglishLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingDays",
                table: "WorkingDays");

            migrationBuilder.DropColumn(
                name: "EnglishLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EnglishLevelConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EndWorkingDate",
                table: "WorkingDays");

            migrationBuilder.DropColumn(
                name: "StartWorkingDate",
                table: "WorkingDays");

            migrationBuilder.RenameTable(
                name: "WorkingDays",
                newName: "WorkingDay");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingDays_StudentId",
                table: "WorkingDay",
                newName: "IX_WorkingDay_StudentId");

            migrationBuilder.AlterColumn<int>(
                name: "DayName",
                table: "WorkingDay",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "WorkingDay",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "WorkingDay",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingEndTime",
                table: "WorkingDay",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkingStartTime",
                table: "WorkingDay",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingDay",
                table: "WorkingDay",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "NotificationMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationMessages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationMessages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationMessages_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationMessages_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDay_GroupId",
                table: "WorkingDay",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDay_TeacherId",
                table: "WorkingDay",
                column: "TeacherId");

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

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMessages_UserId",
                table: "NotificationMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            /*
            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Students_StudentId",
                table: "WorkingDay",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Teachers_TeacherId",
                table: "WorkingDay",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay");

            /*
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Students_StudentId",
                table: "WorkingDay");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Teachers_TeacherId",
                table: "WorkingDay");
            */
            migrationBuilder.DropTable(
                name: "NotificationMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingDay",
                table: "WorkingDay");

            migrationBuilder.DropIndex(
                name: "IX_WorkingDay_GroupId",
                table: "WorkingDay");

            migrationBuilder.DropIndex(
                name: "IX_WorkingDay_TeacherId",
                table: "WorkingDay");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "WorkingDay");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "WorkingDay");

            migrationBuilder.DropColumn(
                name: "WorkingEndTime",
                table: "WorkingDay");

            migrationBuilder.DropColumn(
                name: "WorkingStartTime",
                table: "WorkingDay");

            migrationBuilder.RenameTable(
                name: "WorkingDay",
                newName: "WorkingDays");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingDay_StudentId",
                table: "WorkingDays",
                newName: "IX_WorkingDays_StudentId");

            migrationBuilder.AddColumn<string>(
                name: "EnglishLevel",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnglishLevelConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "DayName",
                table: "WorkingDays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndWorkingDate",
                table: "WorkingDays",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartWorkingDate",
                table: "WorkingDays",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingDays",
                table: "WorkingDays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DaySchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayName = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    WorkingEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkingStartTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaySchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaySchedules_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DaySchedules_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnglishLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnglishLevels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaySchedules_ApplicationUserId",
                table: "DaySchedules",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DaySchedules_GroupId",
                table: "DaySchedules",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_Students_StudentId",
                table: "WorkingDays",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
