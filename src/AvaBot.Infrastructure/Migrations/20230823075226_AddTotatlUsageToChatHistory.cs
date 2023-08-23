using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvaBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTotatlUsageToChatHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalUsage",
                table: "ChatHistories",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalUsage",
                table: "ChatHistories");
        }
    }
}
