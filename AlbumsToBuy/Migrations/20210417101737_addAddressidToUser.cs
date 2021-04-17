using Microsoft.EntityFrameworkCore.Migrations;

namespace AlbumsToBuy.Migrations
{
    public partial class addAddressidToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_HomeAddressid",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "HomeAddressid",
                table: "Users",
                newName: "HomeAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HomeAddressid",
                table: "Users",
                newName: "IX_Users_HomeAddressId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Addresses",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "HomeAddressId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_HomeAddressId",
                table: "Users",
                column: "HomeAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_HomeAddressId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "HomeAddressId",
                table: "Users",
                newName: "HomeAddressid");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HomeAddressId",
                table: "Users",
                newName: "IX_Users_HomeAddressid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Addresses",
                newName: "id");

            migrationBuilder.AlterColumn<int>(
                name: "HomeAddressid",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_HomeAddressid",
                table: "Users",
                column: "HomeAddressid",
                principalTable: "Addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
