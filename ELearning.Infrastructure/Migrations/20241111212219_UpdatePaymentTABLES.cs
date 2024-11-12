using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentTABLES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Courses_CourseId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_CourseId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Payment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Payment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CourseId",
                table: "Payment",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Courses_CourseId",
                table: "Payment",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
