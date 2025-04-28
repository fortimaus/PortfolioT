using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PortfolioT.Migrations
{
    /// <inheritdoc />
    public partial class fixAnanlisysEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analisys_Services_serviceId",
                table: "Analisys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Analisys",
                table: "Analisys");

            migrationBuilder.DropIndex(
                name: "IX_Analisys_serviceId",
                table: "Analisys");

            migrationBuilder.DropColumn(
                name: "serviceId",
                table: "Analisys");

            migrationBuilder.AddColumn<string>(
                name: "link",
                table: "AnalisysUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "serviceId",
                table: "AnalisysUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Analisys",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AnalisysUsers_name_serviceId",
                table: "AnalisysUsers",
                columns: new[] { "name", "serviceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Analisys",
                table: "Analisys",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AnalisysUsers_serviceId",
                table: "AnalisysUsers",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Analisys_userId",
                table: "Analisys",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalisysUsers_Services_serviceId",
                table: "AnalisysUsers",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalisysUsers_Services_serviceId",
                table: "AnalisysUsers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AnalisysUsers_name_serviceId",
                table: "AnalisysUsers");

            migrationBuilder.DropIndex(
                name: "IX_AnalisysUsers_serviceId",
                table: "AnalisysUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Analisys",
                table: "Analisys");

            migrationBuilder.DropIndex(
                name: "IX_Analisys_userId",
                table: "Analisys");

            migrationBuilder.DropColumn(
                name: "link",
                table: "AnalisysUsers");

            migrationBuilder.DropColumn(
                name: "serviceId",
                table: "AnalisysUsers");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Analisys",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "serviceId",
                table: "Analisys",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Analisys",
                table: "Analisys",
                columns: new[] { "userId", "serviceId", "title" });

            migrationBuilder.CreateIndex(
                name: "IX_Analisys_serviceId",
                table: "Analisys",
                column: "serviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Analisys_Services_serviceId",
                table: "Analisys",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
