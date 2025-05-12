using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class addBoolForAch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "basic",
                table: "Achievements",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "basic",
                table: "Achievements");
        }
    }
}
