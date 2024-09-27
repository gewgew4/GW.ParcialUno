using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Documents");

            migrationBuilder.AddColumn<byte>(
                name: "Priority",
                table: "PrintJobs",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "PrintJobs");

            migrationBuilder.AddColumn<byte>(
                name: "Priority",
                table: "Documents",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
