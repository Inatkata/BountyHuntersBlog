using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BountyHuntersBlog.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionComments_AspNetUsers_ApplicationUserId",
                table: "MissionComments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MissionComments");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "MissionComments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MissionComments_AspNetUsers_ApplicationUserId",
                table: "MissionComments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionComments_AspNetUsers_ApplicationUserId",
                table: "MissionComments");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
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
                name: "FK_MissionComments_AspNetUsers_ApplicationUserId",
                table: "MissionComments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
