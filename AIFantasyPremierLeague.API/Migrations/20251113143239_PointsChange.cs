using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIFantasyPremierLeague.API.Migrations
{
    /// <inheritdoc />
    public partial class PointsChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Players",
                newName: "PredictedPoints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PredictedPoints",
                table: "Players",
                newName: "Age");
        }
    }
}
