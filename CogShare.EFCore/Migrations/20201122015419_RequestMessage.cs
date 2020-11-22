using Microsoft.EntityFrameworkCore.Migrations;

namespace CogShare.EFCore.Migrations
{
    public partial class RequestMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Friendships",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Friendships");
        }
    }
}
