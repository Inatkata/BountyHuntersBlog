using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BountyHuntersBlog.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionStatusToMissionPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MissionPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MissionPosts");
        }
    }
}
