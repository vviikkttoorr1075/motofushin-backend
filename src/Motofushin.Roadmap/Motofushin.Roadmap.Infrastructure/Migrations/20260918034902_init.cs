using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motofushin.Roadmap.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "scraper_dwh");

            migrationBuilder.CreateTable(
                name: "dim_route",
                schema: "scraper_dwh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LengthM = table.Column<decimal>(type: "numeric", nullable: true),
                    PointCount = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_route", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "route_point",
                schema: "scraper_dwh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Seq = table.Column<int>(type: "integer", nullable: false),
                    Lat = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    Lon = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    ElevationM = table.Column<decimal>(type: "numeric(7,1)", nullable: true),
                    RecordedAt = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_point", x => new { x.Id, x.Seq });
                    table.ForeignKey(
                        name: "FK_route_point_dim_route_Id",
                        column: x => x.Id,
                        principalSchema: "scraper_dwh",
                        principalTable: "dim_route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_dim_route_source_external_id",
                schema: "scraper_dwh",
                table: "dim_route",
                columns: new[] { "Source", "ExternalId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "route_point",
                schema: "scraper_dwh");

            migrationBuilder.DropTable(
                name: "dim_route",
                schema: "scraper_dwh");
        }
    }
}
