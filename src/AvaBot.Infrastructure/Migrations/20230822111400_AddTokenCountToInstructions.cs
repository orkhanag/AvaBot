using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvaBot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenCountToInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TokensCount",
                table: "Instructions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokensCount",
                table: "Instructions");
        }
    }
}
