using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OptmizeEnrollmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Courses_CouresId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Students_studentId",
                table: "Enrollment");

            migrationBuilder.RenameColumn(
                name: "studentId",
                table: "Enrollment",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "CouresId",
                table: "Enrollment",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_studentId",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CouresId",
                table: "Enrollment",
                newName: "IX_Enrollment_CourseId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "completedAt",
                table: "Enrollment",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Courses_CourseId",
                table: "Enrollment",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Students_StudentId",
                table: "Enrollment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Courses_CourseId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Students_StudentId",
                table: "Enrollment");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Enrollment",
                newName: "studentId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Enrollment",
                newName: "CouresId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment",
                newName: "IX_Enrollment_studentId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                newName: "IX_Enrollment_CouresId");

            migrationBuilder.AlterColumn<string>(
                name: "completedAt",
                table: "Enrollment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Courses_CouresId",
                table: "Enrollment",
                column: "CouresId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Students_studentId",
                table: "Enrollment",
                column: "studentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
