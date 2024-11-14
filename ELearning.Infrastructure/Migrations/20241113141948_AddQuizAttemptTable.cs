using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizAttemptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuizAttemptId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuizAttemptId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    QuizAttemptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasPassed = table.Column<bool>(type: "bit", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttemptedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScorePercentage = table.Column<double>(type: "float", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswersCount = table.Column<int>(type: "int", nullable: false),
                    IncorrectAnswersCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.QuizAttemptId);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Quizs_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizs",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizAttemptId",
                table: "Questions",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuizAttemptId",
                table: "Answers",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_CreatedById",
                table: "QuizAttempts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_StudentId",
                table: "QuizAttempts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_UpdatedById",
                table: "QuizAttempts",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_QuizAttempts_QuizAttemptId",
                table: "Answers",
                column: "QuizAttemptId",
                principalTable: "QuizAttempts",
                principalColumn: "QuizAttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuizAttempts_QuizAttemptId",
                table: "Questions",
                column: "QuizAttemptId",
                principalTable: "QuizAttempts",
                principalColumn: "QuizAttemptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_QuizAttempts_QuizAttemptId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuizAttempts_QuizAttemptId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizAttemptId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuizAttemptId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuizAttemptId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuizAttemptId",
                table: "Answers");
        }
    }
}
