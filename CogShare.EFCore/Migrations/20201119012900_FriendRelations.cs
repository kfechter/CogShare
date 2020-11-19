using Microsoft.EntityFrameworkCore.Migrations;

namespace CogShare.EFCore.Migrations
{
    public partial class FriendRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Friendships",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_ApplicationUserId",
                table: "Friendships",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_ApplicationUserId",
                table: "Friendships",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_ApplicationUserId",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_ApplicationUserId",
                table: "Friendships");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Friendships");
        }
    }
}
