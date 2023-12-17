using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flightMVC.Migrations
{
    public partial class updated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AircraftName",
                table: "Aircrafts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AircraftName",
                table: "Aircrafts");
        }
    }
}
