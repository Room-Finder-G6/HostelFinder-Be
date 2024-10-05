using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ver22_EditEntityWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wishlists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
