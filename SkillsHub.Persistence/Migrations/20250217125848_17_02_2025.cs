using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class _17022025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_ApplicationUserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserId",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "ExternalConnections");

            migrationBuilder.DropTable(
                name: "Finances");

            migrationBuilder.DropTable(
                name: "RequestLessons");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_ApplicationUserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_ApplicationUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "BaseUserInfo",
                newName: "MiddleName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserStudentId",
                table: "AspNetUsers",
                column: "UserStudentId",
                unique: true,
                filter: "[UserStudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserTeacherId",
                table: "AspNetUsers",
                column: "UserTeacherId",
                unique: true,
                filter: "[UserTeacherId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Students_UserStudentId",
                table: "AspNetUsers",
                column: "UserStudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teachers_UserTeacherId",
                table: "AspNetUsers",
                column: "UserTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Students_UserStudentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teachers_UserTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserStudentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserTeacherId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "BaseUserInfo",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ExternalConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BaseUserInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserProfile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalConnections_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExternalConnections_BaseUserInfo_BaseUserInfoId",
                        column: x => x.BaseUserInfoId,
                        principalTable: "BaseUserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Finances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(15,4)", precision: 15, scale: 4, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finances_Finances_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Finances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Finances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestLessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonBeforeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnswerStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NewEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestMessage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestLessons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestLessons_Lessons_LessonBeforeId",
                        column: x => x.LessonBeforeId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ApplicationUserId",
                table: "Teachers",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ApplicationUserId",
                table: "Students",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalConnections_ApplicationUserId",
                table: "ExternalConnections",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalConnections_BaseUserInfoId",
                table: "ExternalConnections",
                column: "BaseUserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Finances_ParentId",
                table: "Finances",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Finances_StudentId",
                table: "Finances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLessons_LessonBeforeId",
                table: "RequestLessons",
                column: "LessonBeforeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLessons_UserId",
                table: "RequestLessons",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_ApplicationUserId",
                table: "Students",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserId",
                table: "Teachers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
