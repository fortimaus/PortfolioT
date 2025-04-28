using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class analisisRepoFixUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analisys_AnalisisUser_userId",
                table: "Analisys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnalisisUser",
                table: "AnalisisUser");

            migrationBuilder.RenameTable(
                name: "AnalisisUser",
                newName: "AnalisysUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnalisysUsers",
                table: "AnalisysUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Analisys_AnalisysUsers_userId",
                table: "Analisys",
                column: "userId",
                principalTable: "AnalisysUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analisys_AnalisysUsers_userId",
                table: "Analisys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnalisysUsers",
                table: "AnalisysUsers");

            migrationBuilder.RenameTable(
                name: "AnalisysUsers",
                newName: "AnalisisUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnalisisUser",
                table: "AnalisisUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Analisys_AnalisisUser_userId",
                table: "Analisys",
                column: "userId",
                principalTable: "AnalisisUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
