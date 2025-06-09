using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TreasureHuntApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    N = table.Column<int>(type: "INTEGER", nullable: false),
                    M = table.Column<int>(type: "INTEGER", nullable: false),
                    P = table.Column<int>(type: "INTEGER", nullable: false),
                    Matrix = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inputs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InputId = table.Column<int>(type: "INTEGER", nullable: false),
                    MinFuel = table.Column<double>(type: "REAL", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inputs");

            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
