using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventRegistrationSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_OrganizationUser_OrganizationId",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Event",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Event_OrganizationId",
                table: "Event",
                newName: "IX_Event_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_OrganizationUser_CreatorId",
                table: "Event",
                column: "CreatorId",
                principalTable: "OrganizationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_OrganizationUser_CreatorId",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Event",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Event_CreatorId",
                table: "Event",
                newName: "IX_Event_OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_OrganizationUser_OrganizationId",
                table: "Event",
                column: "OrganizationId",
                principalTable: "OrganizationUser",
                principalColumn: "Id");
        }
    }
}
