using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ver13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "ServiceCosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "ServiceCosts");
        }
    }
}
