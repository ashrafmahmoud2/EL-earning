using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCongsToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UpdatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Payment_EnrollmentId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UpdatedById",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Comments",
                newName: "CommentedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatedById",
                table: "Comments",
                newName: "IX_Comments_CommentedByUserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Sections",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Instructors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentPath",
                table: "Documents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Comments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "CommentText",
                table: "Comments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorys",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Title_CourseId",
                table: "Sections",
                columns: new[] { "Title", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizs_QuizId_LessonId",
                table: "Quizs",
                columns: new[] { "QuizId", "LessonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizs_Title",
                table: "Quizs",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizs_Title_QuizId",
                table: "Quizs",
                columns: new[] { "Title", "QuizId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId_StudentId",
                table: "QuizAttempts",
                columns: new[] { "QuizId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionId_Text_QuizId",
                table: "Questions",
                columns: new[] { "QuestionId", "Text", "QuizId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_EnrollmentId_PaymentId",
                table: "Payment",
                columns: new[] { "EnrollmentId", "PaymentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Title_DocumentPath",
                table: "Documents",
                columns: new[] { "Title", "DocumentPath" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorys_Name",
                table: "Categorys",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommentedByUserId",
                table: "Comments",
                column: "CommentedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommentedByUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Sections_Title_CourseId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Quizs_QuizId_LessonId",
                table: "Quizs");

            migrationBuilder.DropIndex(
                name: "IX_Quizs_Title",
                table: "Quizs");

            migrationBuilder.DropIndex(
                name: "IX_Quizs_Title_QuizId",
                table: "Quizs");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_QuizId_StudentId",
                table: "QuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionId_Text_QuizId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Payment_EnrollmentId_PaymentId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Documents_Title_DocumentPath",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Categorys_Name",
                table: "Categorys");

            migrationBuilder.RenameColumn(
                name: "CommentedByUserId",
                table: "Comments",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentedByUserId",
                table: "Comments",
                newName: "IX_Comments_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Sections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Instructors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentPath",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Comments",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommentText",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Comments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_EnrollmentId",
                table: "Payment",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UpdatedById",
                table: "Comments",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UpdatedById",
                table: "Comments",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                table: "Instructors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
