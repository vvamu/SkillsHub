using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addgroupWorkingDayuserWorkingDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingDay",
                table: "WorkingDay");

            migrationBuilder.RenameTable(
                name: "WorkingDay",
                newName: "DaySchedules");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingDay_GroupId",
                table: "DaySchedules",
                newName: "IX_DaySchedules_GroupId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "DaySchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndDate",
                table: "DaySchedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepeatIntervalName",
                table: "DaySchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepeatIntervalValue",
                table: "DaySchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartDate",
                table: "DaySchedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DaySchedules",
                table: "DaySchedules",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserWorkingDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkingDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWorkingDays_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkingDays_ApplicationUserId",
                table: "UserWorkingDays",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DaySchedules_Groups_GroupId",
                table: "DaySchedules",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaySchedules_Groups_GroupId",
                table: "DaySchedules");

            migrationBuilder.DropTable(
                name: "UserWorkingDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DaySchedules",
                table: "DaySchedules");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DaySchedules");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "DaySchedules");

            migrationBuilder.DropColumn(
                name: "RepeatIntervalName",
                table: "DaySchedules");

            migrationBuilder.DropColumn(
                name: "RepeatIntervalValue",
                table: "DaySchedules");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "DaySchedules");

            migrationBuilder.RenameTable(
                name: "DaySchedules",
                newName: "WorkingDay");

            migrationBuilder.RenameIndex(
                name: "IX_DaySchedules_GroupId",
                table: "WorkingDay",
                newName: "IX_WorkingDay_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingDay",
                table: "WorkingDay",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDay_Groups_GroupId",
                table: "WorkingDay",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
