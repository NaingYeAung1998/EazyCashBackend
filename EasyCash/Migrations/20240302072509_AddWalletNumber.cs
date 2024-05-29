using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyCash.Migrations
{
    public partial class AddWalletNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WalletNumber",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletNumber",
                table: "Wallets");
        }
    }
}
