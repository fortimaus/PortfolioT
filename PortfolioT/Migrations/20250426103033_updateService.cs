using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class updateService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "Services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "serviceId",
                table: "Achievements",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1L,
                column: "type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2L,
                column: "type",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "title", "type" },
                values: new object[] { "ElibUlstu", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_serviceId",
                table: "Achievements",
                column: "serviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Services_serviceId",
                table: "Achievements",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Services_serviceId",
                table: "Achievements");

            migrationBuilder.DropIndex(
                name: "IX_Achievements_serviceId",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "type",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "serviceId",
                table: "Achievements");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3L,
                column: "title",
                value: "ElibUlsu");
        }
    }
}
