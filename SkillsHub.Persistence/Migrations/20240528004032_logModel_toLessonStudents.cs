using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class logModeltoLessonStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdd",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "DateAdd",
                table: "LessonStudents");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "PaymentCategories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Locations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTypeTeachers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTypeStudents",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLessonId",
                table: "LessonTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentTeacherId",
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

            migrationBuilder.AddColumn<double>(
                name: "Grade",
                table: "LessonStudents",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLessonId",
                table: "LessonStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentStudentId",
                table: "LessonStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "GroupTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
                name: "ParentGroupId",
                table: "GroupTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "GroupTeachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentTeacherId",
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
                name: "ParentGroupId",
                table: "GroupStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "GroupStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentStudentId",
                table: "GroupStudents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Groups",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Courses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "BaseUserInfo",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "AgeTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers",
                columns: new[] { "ParentLessonId", "ParentTeacherId" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents",
                columns: new[] { "ParentLessonId", "ParentStudentId" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers",
                columns: new[] { "ParentGroupId", "ParentTeacherId" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_ParentGroupId_ParentStudentId",
                table: "GroupStudents",
                columns: new[] { "ParentGroupId", "ParentStudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentGroupId_ParentStudentId",
                table: "GroupStudents",
                columns: new[] { "ParentGroupId", "ParentStudentId" },
                principalTable: "GroupStudents",
                principalColumns: new[] { "GroupId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers",
                columns: new[] { "ParentGroupId", "ParentTeacherId" },
                principalTable: "GroupTeachers",
                principalColumns: new[] { "GroupId", "TeacherId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents",
                columns: new[] { "ParentLessonId", "ParentStudentId" },
                principalTable: "LessonStudents",
                principalColumns: new[] { "LessonId", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers",
                columns: new[] { "ParentLessonId", "ParentTeacherId" },
                principalTable: "LessonTeachers",
                principalColumns: new[] { "LessonId", "TeacherId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_GroupStudents_ParentGroupId_ParentStudentId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupTeachers_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonStudents_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTeachers_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonTeachers_ParentLessonId_ParentTeacherId",
                table: "LessonTeachers");

            migrationBuilder.DropIndex(
                name: "IX_LessonStudents_ParentLessonId_ParentStudentId",
                table: "LessonStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupTeachers_ParentGroupId_ParentTeacherId",
                table: "GroupTeachers");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_ParentGroupId_ParentStudentId",
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
                name: "ParentLessonId",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "ParentTeacherId",
                table: "LessonTeachers");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "ParentLessonId",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "ParentStudentId",
                table: "LessonStudents");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "ParentGroupId",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "ParentTeacherId",
                table: "GroupTeachers");

            migrationBuilder.DropColumn(
                name: "DateRegistration",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "ParentGroupId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "ParentStudentId",
                table: "GroupStudents");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "PaymentCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Locations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTypeTeachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTypeStudents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonTeachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdd",
                table: "LessonTeachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "LessonStudents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdd",
                table: "LessonStudents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "GroupTypes",
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Groups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "BaseUserInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "AgeTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
