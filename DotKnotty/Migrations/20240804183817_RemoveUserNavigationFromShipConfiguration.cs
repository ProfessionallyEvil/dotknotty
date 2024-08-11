using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotKnotty.Migrations
{
    public partial class RemoveUserNavigationFromShipConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipConfigurations_AspNetUsers_UserId",
                table: "ShipConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ShipConfigurations_UserId",
                table: "ShipConfigurations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShipConfigurations_UserId",
                table: "ShipConfigurations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipConfigurations_AspNetUsers_UserId",
                table: "ShipConfigurations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
