using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    studentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouresId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    enrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollment_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollment_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Enrollment_Courses_CouresId",
                        column: x => x.CouresId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollment_Students_studentId",
                        column: x => x.studentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CouresId",
                table: "Enrollment",
                column: "CouresId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CreatedById",
                table: "Enrollment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_studentId",
                table: "Enrollment",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_UpdatedById",
                table: "Enrollment",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment");
        }
    }
}
