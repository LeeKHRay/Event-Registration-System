using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class SetEventEnrollmentCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventEnrollment",
                table: "EventEnrollment");

            migrationBuilder.DropIndex(
                name: "IX_EventEnrollment_EventId",
                table: "EventEnrollment");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EventEnrollment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventEnrollment",
                table: "EventEnrollment",
                columns: new[] { "EventId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventEnrollment",
                table: "EventEnrollment");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EventEnrollment",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventEnrollment",
                table: "EventEnrollment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrollment_EventId",
                table: "EventEnrollment",
                column: "EventId");
        }
    }
}
