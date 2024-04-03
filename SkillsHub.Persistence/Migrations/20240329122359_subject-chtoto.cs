using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class subjectchtoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypePaymentCategories_LessonTypePaymentCategories_ParentId",
                table: "LessonTypePaymentCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypes_LessonTypePaymentCategories_PaymentCategoryId",
                table: "LessonTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonTypePaymentCategories",
                table: "LessonTypePaymentCategories");

            migrationBuilder.RenameTable(
                name: "LessonTypePaymentCategories",
                newName: "PaymentCategories");

            migrationBuilder.RenameIndex(
                name: "IX_LessonTypePaymentCategories_ParentId",
                table: "PaymentCategories",
                newName: "IX_PaymentCategories_ParentId");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "LessonTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentCategories",
                table: "PaymentCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTypes_SubjectId",
                table: "LessonTypes",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypes_PaymentCategories_PaymentCategoryId",
                table: "LessonTypes",
                column: "PaymentCategoryId",
                principalTable: "PaymentCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypes_Subject_SubjectId",
                table: "LessonTypes",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCategories_PaymentCategories_ParentId",
                table: "PaymentCategories",
                column: "ParentId",
                principalTable: "PaymentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypes_PaymentCategories_PaymentCategoryId",
                table: "LessonTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTypes_Subject_SubjectId",
                table: "LessonTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCategories_PaymentCategories_ParentId",
                table: "PaymentCategories");

            migrationBuilder.DropIndex(
                name: "IX_LessonTypes_SubjectId",
                table: "LessonTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentCategories",
                table: "PaymentCategories");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "LessonTypes");

            migrationBuilder.RenameTable(
                name: "PaymentCategories",
                newName: "LessonTypePaymentCategories");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentCategories_ParentId",
                table: "LessonTypePaymentCategories",
                newName: "IX_LessonTypePaymentCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonTypePaymentCategories",
                table: "LessonTypePaymentCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypePaymentCategories_LessonTypePaymentCategories_ParentId",
                table: "LessonTypePaymentCategories",
                column: "ParentId",
                principalTable: "LessonTypePaymentCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTypes_LessonTypePaymentCategories_PaymentCategoryId",
                table: "LessonTypes",
                column: "PaymentCategoryId",
                principalTable: "LessonTypePaymentCategories",
                principalColumn: "Id");
        }
    }
}
