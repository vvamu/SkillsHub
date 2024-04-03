using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeLessonType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_LessonTypes_LessonTypeId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "LessonTypeAgeTypes");

            migrationBuilder.DropTable(
                name: "LessonTypeStudents");

            migrationBuilder.DropTable(
                name: "LessonTypeTeachers");

            migrationBuilder.DropTable(
                name: "LessonTypes");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LessonTypeId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonTypeId",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "LessonTypeId",
                table: "Groups",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_LessonTypeId",
                table: "Groups",
                newName: "IX_Groups_CourseId");

            migrationBuilder.AddColumn<int>(
                name: "LessonTimeInMinutes",
                table: "PaymentCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupTypeId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonTimeInMinutes",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationTypeId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentCategoryId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationType",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PossibleCourseTeacher",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleCourseTeacher", x => new { x.CourseId, x.TeacherId, x.LocationTypeId });
                    table.ForeignKey(
                        name: "FK_PossibleCourseTeacher_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PossibleCourseTeacher_GroupTypes_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PossibleCourseTeacher_LocationTypes_LocationTypeId",
                        column: x => x.LocationTypeId,
                        principalTable: "LocationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PossibleCourseTeacher_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreferenceCourseStudent",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferenceCourseStudent", x => new { x.CourseId, x.StudentId, x.LocationTypeId });
                    table.ForeignKey(
                        name: "FK_PreferenceCourseStudent_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferenceCourseStudent_GroupTypes_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreferenceCourseStudent_LocationTypes_LocationTypeId",
                        column: x => x.LocationTypeId,
                        principalTable: "LocationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferenceCourseStudent_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupTypeId",
                table: "Groups",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LocationTypeId",
                table: "Groups",
                column: "LocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_PaymentCategoryId",
                table: "Groups",
                column: "PaymentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PossibleCourseTeacher_GroupTypeId",
                table: "PossibleCourseTeacher",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PossibleCourseTeacher_LocationTypeId",
                table: "PossibleCourseTeacher",
                column: "LocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PossibleCourseTeacher_TeacherId",
                table: "PossibleCourseTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenceCourseStudent_GroupTypeId",
                table: "PreferenceCourseStudent",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenceCourseStudent_LocationTypeId",
                table: "PreferenceCourseStudent",
                column: "LocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenceCourseStudent_StudentId",
                table: "PreferenceCourseStudent",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupTypes_GroupTypeId",
                table: "Groups",
                column: "GroupTypeId",
                principalTable: "GroupTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_LocationTypes_LocationTypeId",
                table: "Groups",
                column: "LocationTypeId",
                principalTable: "LocationTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_PaymentCategories_PaymentCategoryId",
                table: "Groups",
                column: "PaymentCategoryId",
                principalTable: "PaymentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupTypes_GroupTypeId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_LocationTypes_LocationTypeId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_PaymentCategories_PaymentCategoryId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "PossibleCourseTeacher");

            migrationBuilder.DropTable(
                name: "PreferenceCourseStudent");

            migrationBuilder.DropTable(
                name: "LocationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupTypeId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_LocationTypeId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_PaymentCategoryId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LessonTimeInMinutes",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "GroupTypeId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LessonTimeInMinutes",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LocationTypeId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PaymentCategoryId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Groups",
                newName: "LessonTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CourseId",
                table: "Groups",
                newName: "IX_Groups_LessonTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonTypeId",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LessonTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LessonTimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    LocationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinEnglishLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonTypes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessonTypes_GroupTypes_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "GroupTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessonTypes_LessonTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "LessonTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessonTypes_PaymentCategories_PaymentCategoryId",
                        column: x => x.PaymentCategoryId,
                        principalTable: "PaymentCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessonTypes_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessonTypeAgeTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgeTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTypeAgeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonTypeAgeTypes_AgeTypes_AgeTypeId",
                        column: x => x.AgeTypeId,
                        principalTable: "AgeTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LessonTypeAgeTypes_LessonTypes_LessonTypeId",
                        column: x => x.LessonTypeId,
                        principalTable: "LessonTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessonTypeStudents",
                columns: table => new
                {
                    LessonTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTypeStudents", x => new { x.LessonTypeId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_LessonTypeStudents_LessonTypes_LessonTypeId",
                        column: x => x.LessonTypeId,
                        principalTable: "LessonTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonTypeStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonTypeTeachers",
                columns: table => new
                {
                    LessonTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTypeTeachers", x => new { x.LessonTypeId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_LessonTypeTeachers_LessonTypes_LessonTypeId",
                        column: x => x.LessonTypeId,
                        principalTable: "LessonTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonTypeTeachers_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonTypeId",
                table: "Lessons",
                column: "LessonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypeAgeTypes_AgeTypeId",
                table: "LessonTypeAgeTypes",
                column: "AgeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypeAgeTypes_LessonTypeId",
                table: "LessonTypeAgeTypes",
                column: "LessonTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_CourseId",
                table: "LessonTypes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_GroupTypeId",
                table: "LessonTypes",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_ParentId",
                table: "LessonTypes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_PaymentCategoryId",
                table: "LessonTypes",
                column: "PaymentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_SubjectId",
                table: "LessonTypes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypeStudents_StudentId",
                table: "LessonTypeStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypeTeachers_TeacherId",
                table: "LessonTypeTeachers",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_LessonTypes_LessonTypeId",
                table: "Groups",
                column: "LessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_LessonTypes_LessonTypeId",
                table: "Lessons",
                column: "LessonTypeId",
                principalTable: "LessonTypes",
                principalColumn: "Id");
        }
    }
}
