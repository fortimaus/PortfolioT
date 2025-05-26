using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class addUserRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "rating",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                table: "Users");
        }
    }
}
