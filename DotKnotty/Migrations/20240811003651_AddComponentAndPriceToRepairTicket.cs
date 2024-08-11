using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotKnotty.Migrations
{
    public partial class AddComponentAndPriceToRepairTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Component",
                table: "RepairTickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "RepairTickets",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Component",
                table: "RepairTickets");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "RepairTickets");
        }
    }
}
