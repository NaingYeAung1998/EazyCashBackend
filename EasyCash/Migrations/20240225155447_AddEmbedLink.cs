using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyCash.Migrations
{
    public partial class AddEmbedLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmbedLink",
                table: "Advertisments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbedLink",
                table: "Advertisments");
        }
    }
}
