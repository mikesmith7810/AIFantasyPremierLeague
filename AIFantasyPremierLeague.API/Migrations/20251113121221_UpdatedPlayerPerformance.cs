using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIFantasyPremierLeague.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPlayerPerformance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team",
                table: "PlayerHistory");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "PlayerHistory",
                newName: "Stats_Points");

            migrationBuilder.RenameColumn(
                name: "MinsPlayed",
                table: "PlayerHistory",
                newName: "Stats_MinsPlayed");

            migrationBuilder.RenameColumn(
                name: "Goals",
                table: "PlayerHistory",
                newName: "Stats_Goals");

            migrationBuilder.RenameColumn(
                name: "Assists",
                table: "PlayerHistory",
                newName: "Stats_Assists");

            migrationBuilder.RenameColumn(
                name: "Week",
                table: "PlayerHistory",
                newName: "Stats_YellowCards");

            migrationBuilder.RenameColumn(
                name: "Season",
                table: "PlayerHistory",
                newName: "Stats_Saves");

            migrationBuilder.AddColumn<int>(
                name: "Stats_Bonus",
                table: "PlayerHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stats_CleanSheets",
                table: "PlayerHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stats_GoalsConceded",
                table: "PlayerHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stats_RedCards",
                table: "PlayerHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stats_Bonus",
                table: "PlayerHistory");

            migrationBuilder.DropColumn(
                name: "Stats_CleanSheets",
                table: "PlayerHistory");

            migrationBuilder.DropColumn(
                name: "Stats_GoalsConceded",
                table: "PlayerHistory");

            migrationBuilder.DropColumn(
                name: "Stats_RedCards",
                table: "PlayerHistory");

            migrationBuilder.RenameColumn(
                name: "Stats_Points",
                table: "PlayerHistory",
                newName: "Points");

            migrationBuilder.RenameColumn(
                name: "Stats_MinsPlayed",
                table: "PlayerHistory",
                newName: "MinsPlayed");

            migrationBuilder.RenameColumn(
                name: "Stats_Goals",
                table: "PlayerHistory",
                newName: "Goals");

            migrationBuilder.RenameColumn(
                name: "Stats_Assists",
                table: "PlayerHistory",
                newName: "Assists");

            migrationBuilder.RenameColumn(
                name: "Stats_YellowCards",
                table: "PlayerHistory",
                newName: "Week");

            migrationBuilder.RenameColumn(
                name: "Stats_Saves",
                table: "PlayerHistory",
                newName: "Season");

            migrationBuilder.AddColumn<string>(
                name: "Team",
                table: "PlayerHistory",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
