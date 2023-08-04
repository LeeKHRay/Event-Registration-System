using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEventEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_AspNetUsers_OrganizationId",
                table: "Event");

            migrationBuilder.CreateTable(
                name: "EventEnrollment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrollTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEnrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventEnrollment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEnrollment_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrollment_EventId",
                table: "EventEnrollment",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrollment_UserId",
                table: "EventEnrollment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_AspNetUsers_OrganizationId",
                table: "Event",
                column: "OrganizationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_AspNetUsers_OrganizationId",
                table: "Event");

            migrationBuilder.DropTable(
                name: "EventEnrollment");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_AspNetUsers_OrganizationId",
                table: "Event",
                column: "OrganizationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
