using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContext_v12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetails_Rooms_PostId",
                table: "RoomDetails");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "RoomDetails",
                newName: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "RoomDetails",
                newName: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetails_Rooms_PostId",
                table: "RoomDetails",
                column: "PostId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
