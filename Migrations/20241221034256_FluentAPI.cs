using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeDotLog.Migrations
{
    /// <inheritdoc />
    public partial class FluentAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_AttendanceId",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AttendanceId",
                table: "Reports",
                column: "AttendanceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_AttendanceId",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AttendanceId",
                table: "Reports",
                column: "AttendanceId");
        }
    }
}
