using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyCash.Migrations
{
    public partial class AddWalletIdInTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction");

            migrationBuilder.AlterColumn<Guid>(
                name: "WalletId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Wallets_WalletId",
                table: "Transaction",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }
    }
}
