using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIFantasyPremierLeague.API.Migrations
{
    /// <inheritdoc />
    public partial class GameWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameWeek",
                table: "PlayerHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameWeek",
                table: "PlayerHistory");
        }
    }
}
