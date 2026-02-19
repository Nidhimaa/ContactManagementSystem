using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactManagementSystem.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddAppNameToAuditEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppName",
                table: "AuditEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppName",
                table: "AuditEvents");
        }
    }
}
