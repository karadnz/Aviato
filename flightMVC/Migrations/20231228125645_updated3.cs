using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flightMVC.Migrations
{
    public partial class updated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "Routes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "Routes");
        }
    }
}
