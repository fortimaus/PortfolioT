using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class analisisRepoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalisisUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalisisUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Analisys",
                columns: table => new
                {
                    title = table.Column<string>(type: "text", nullable: false),
                    serviceId = table.Column<long>(type: "bigint", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    link = table.Column<string>(type: "text", nullable: true),
                    scope_decor = table.Column<float>(type: "real", nullable: false),
                    scope_code = table.Column<float>(type: "real", nullable: false),
                    scope_security = table.Column<float>(type: "real", nullable: false),
                    scope_maintability = table.Column<float>(type: "real", nullable: false),
                    scope_reability = table.Column<float>(type: "real", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analisys", x => new { x.userId, x.serviceId, x.title });
                    table.ForeignKey(
                        name: "FK_Analisys_AnalisisUser_userId",
                        column: x => x.userId,
                        principalTable: "AnalisisUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Analisys_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analisys_serviceId",
                table: "Analisys",
                column: "serviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analisys");

            migrationBuilder.DropTable(
                name: "AnalisisUser");
        }
    }
}
