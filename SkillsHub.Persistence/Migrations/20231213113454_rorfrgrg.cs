using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class rorfrgrg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Cources_CourceId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "ScheduleDays");

            migrationBuilder.DropTable(
                name: "Cources");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourceId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CourceId",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "CourseNameId",
                table: "Groups",
                newName: "CourceNameId1");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceNameId",
                table: "Groups",
                newName: "IX_Groups_CourceNameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId1",
                table: "Groups",
                column: "CourceNameId1",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CourceNames_CourceNameId1",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "CourceNameId1",
                table: "Groups",
                newName: "CourseNameId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourceNameId1",
                table: "Groups",
                newName: "IX_Groups_CourceNameId");

            migrationBuilder.AddColumn<Guid>(
                name: "CourceId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnglishLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MaxArrivedStudents = table.Column<int>(type: "int", nullable: false),
                    MinimumAge = table.Column<int>(type: "int", nullable: false),
                    SubscriptionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cources_CourceNames_NameId",
                        column: x => x.NameId,
                        principalTable: "CourseNames",
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
                    CourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "IX_Lessons_CourceId",
                table: "Lessons",
                column: "CourceId");

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
                name: "FK_Groups_CourceNames_CourceNameId",
                table: "Groups",
                column: "CourseNameId",
                principalTable: "CourseNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Cources_CourceId",
                table: "Lessons",
                column: "CourceId",
                principalTable: "Cources",
                principalColumn: "Id");
        }
    }
}
