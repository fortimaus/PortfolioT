using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class addLanguge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "language",
                table: "Analisys",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "language",
                table: "Analisys");
        }
    }
}
