using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class correctuserWorkingDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWorkingDays");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "DaySchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaySchedules_ApplicationUserId",
                table: "DaySchedules",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DaySchedules_AspNetUsers_ApplicationUserId",
                table: "DaySchedules",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaySchedules_AspNetUsers_ApplicationUserId",
                table: "DaySchedules");

            migrationBuilder.DropIndex(
                name: "IX_DaySchedules_ApplicationUserId",
                table: "DaySchedules");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DaySchedules");

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
        }
    }
}
