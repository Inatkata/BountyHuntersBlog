using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BountyHuntersBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserIdCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_MissionId",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MissionId_CreatedOn",
                table: "Comments",
                columns: new[] { "MissionId", "CreatedOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_MissionId_CreatedOn",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MissionId",
                table: "Comments",
                column: "MissionId");
        }
    }
}
