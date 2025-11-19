using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIFantasyPremierLeague.API.Migrations
{
    /// <inheritdoc />
    public partial class HomeAway : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsHome",
                table: "PlayerHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "PlayerHistory");
        }
    }
}
