using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotKnotty.Migrations
{
    public partial class RemoveShipNameFromRepairTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairTickets_AspNetUsers_UserId",
                table: "RepairTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairTickets_ShipConfigurations_ShipConfigurationId",
                table: "RepairTickets");

            migrationBuilder.DropIndex(
                name: "IX_RepairTickets_UserId",
                table: "RepairTickets");

            migrationBuilder.DropColumn(
                name: "ShipName",
                table: "RepairTickets");

            migrationBuilder.AlterColumn<int>(
                name: "ShipConfigurationId",
                table: "RepairTickets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTickets_ShipConfigurations_ShipConfigurationId",
                table: "RepairTickets",
                column: "ShipConfigurationId",
                principalTable: "ShipConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairTickets_ShipConfigurations_ShipConfigurationId",
                table: "RepairTickets");

            migrationBuilder.AlterColumn<int>(
                name: "ShipConfigurationId",
                table: "RepairTickets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "ShipName",
                table: "RepairTickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_UserId",
                table: "RepairTickets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTickets_AspNetUsers_UserId",
                table: "RepairTickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTickets_ShipConfigurations_ShipConfigurationId",
                table: "RepairTickets",
                column: "ShipConfigurationId",
                principalTable: "ShipConfigurations",
                principalColumn: "Id");
        }
    }
}
