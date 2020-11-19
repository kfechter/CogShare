using Microsoft.EntityFrameworkCore.Migrations;

namespace CogShare.EFCore.Migrations
{
    public partial class NoCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
