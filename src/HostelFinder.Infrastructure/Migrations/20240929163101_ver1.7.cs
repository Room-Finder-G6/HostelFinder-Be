using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ver17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCosts_Rooms_RoomId",
                table: "ServiceCosts");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCosts_Rooms_RoomId",
                table: "ServiceCosts",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCosts_Rooms_RoomId",
                table: "ServiceCosts");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAmenities_Rooms_RoomId",
                table: "RoomAmenities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCosts_Rooms_RoomId",
                table: "ServiceCosts",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
