using Microsoft.EntityFrameworkCore.Migrations;

namespace AlbumsToBuy.Migrations
{
    public partial class AddQuantityToAlbumOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "AlbumOlders",
                type: "int",
                nullable: true,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "AlbumOlders");
        }
    }
}
