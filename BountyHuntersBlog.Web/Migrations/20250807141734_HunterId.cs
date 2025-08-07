using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BountyHuntersBlog.Migrations
{
    /// <inheritdoc />
    public partial class HunterId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionComments_AspNetUsers_HunterId",
                table: "MissionComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MissionComments");

            migrationBuilder.AlterColumn<string>(
                name: "HunterId",
                table: "MissionComments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MissionComments_AspNetUsers_HunterId",
                table: "MissionComments",
                column: "HunterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionComments_AspNetUsers_HunterId",
                table: "MissionComments");

            migrationBuilder.AlterColumn<string>(
                name: "HunterId",
                table: "MissionComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MissionComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MissionComments_AspNetUsers_HunterId",
                table: "MissionComments",
                column: "HunterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
