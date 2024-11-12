using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInEnrollmentAndPaymentTABLES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Students_StudentId",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Payment",
                newName: "EnrollmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_StudentId",
                table: "Payment",
                newName: "IX_Payment_EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Enrollment_EnrollmentId",
                table: "Payment",
                column: "EnrollmentId",
                principalTable: "Enrollment",
                principalColumn: "EnrollmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Enrollment_EnrollmentId",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "Payment",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_EnrollmentId",
                table: "Payment",
                newName: "IX_Payment_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Students_StudentId",
                table: "Payment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
