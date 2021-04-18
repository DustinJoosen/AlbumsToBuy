using Microsoft.EntityFrameworkCore.Migrations;

namespace AlbumsToBuy.Migrations
{
    public partial class RemoveUserToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserToken",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
