using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class addCodeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Analisys_userId",
                table: "Analisys");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Analisys_userId_title",
                table: "Analisys",
                columns: new[] { "userId", "title" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Analisys_userId_title",
                table: "Analisys");

            migrationBuilder.DropColumn(
                name: "code",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Analisys_userId",
                table: "Analisys",
                column: "userId");
        }
    }
}
