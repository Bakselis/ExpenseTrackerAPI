using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseTracker.Data.Migrations
{
    public partial class ChangedPostToExpenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Posts",
                newName: "Title");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseId",
                table: "PostTags",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_ExpenseId",
                table: "PostTags",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Posts_ExpenseId",
                table: "PostTags",
                column: "ExpenseId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Posts_ExpenseId",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_ExpenseId",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Posts",
                newName: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Posts_PostId",
                table: "PostTags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
