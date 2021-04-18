using Microsoft.EntityFrameworkCore.Migrations;

namespace AlbumsToBuy.Migrations
{
    public partial class AddQuantityToShoppingListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShoppingListItems",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingListItems");
        }
    }
}
