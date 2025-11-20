using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIFantasyPremierLeague.API.Migrations
{
    /// <inheritdoc />
    public partial class Fixtures3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "TeamFixtures",
                newName: "TeamHome");

            migrationBuilder.RenameColumn(
                name: "OpponentTeam",
                table: "TeamFixtures",
                newName: "TeamAway");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamHome",
                table: "TeamFixtures",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "TeamAway",
                table: "TeamFixtures",
                newName: "OpponentTeam");
        }
    }
}
