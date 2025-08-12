using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BountyHuntersBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesAndConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Missions_MissionId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_MissionTags_Missions_MissionId",
                table: "MissionTags");

            migrationBuilder.DropIndex(
                name: "IX_Missions_CategoryId",
                table: "Missions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Likes_Target",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MissionId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Tags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MissionTags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Missions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Missions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Missions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Missions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MissionTags_MissionId",
                table: "MissionTags",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Missions_CategoryId_CreatedOn",
                table: "Missions",
                columns: new[] { "CategoryId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Missions_Title",
                table: "Missions",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_CreatedOn",
                table: "Likes",
                column: "CreatedOn");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Like_Target_XOR",
                table: "Likes",
                sql: "((MissionId IS NULL AND CommentId IS NOT NULL) OR (MissionId IS NOT NULL AND CommentId IS NULL))");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MissionId_CreatedOn",
                table: "Comments",
                columns: new[] { "MissionId", "CreatedOn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Missions_MissionId",
                table: "Comments",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MissionTags_Missions_MissionId",
                table: "MissionTags",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Missions_MissionId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_MissionTags_Missions_MissionId",
                table: "MissionTags");

            migrationBuilder.DropIndex(
                name: "IX_MissionTags_MissionId",
                table: "MissionTags");

            migrationBuilder.DropIndex(
                name: "IX_Missions_CategoryId_CreatedOn",
                table: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_Missions_Title",
                table: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_Likes_CreatedOn",
                table: "Likes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Like_Target_XOR",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MissionId_CreatedOn",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MissionTags");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Missions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Missions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.CreateIndex(
                name: "IX_Missions_CategoryId",
                table: "Missions",
                column: "CategoryId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Likes_Target",
                table: "Likes",
                sql: "([MissionId] IS NOT NULL AND [CommentId] IS NULL) OR ([MissionId] IS NULL AND [CommentId] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MissionId",
                table: "Comments",
                column: "MissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Missions_MissionId",
                table: "Comments",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MissionTags_Missions_MissionId",
                table: "MissionTags",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
