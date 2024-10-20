using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelFinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMembershipRelationshipsv11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipServices_Memberships_MembershipId",
                table: "MembershipServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_MembershipServices_MembershipServiceId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemberships_Memberships_MembershipId",
                table: "UserMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemberships_Users_UserId",
                table: "UserMemberships");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipServices_Memberships_MembershipId",
                table: "MembershipServices",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_MembershipServices_MembershipServiceId",
                table: "Posts",
                column: "MembershipServiceId",
                principalTable: "MembershipServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemberships_Memberships_MembershipId",
                table: "UserMemberships",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemberships_Users_UserId",
                table: "UserMemberships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipServices_Memberships_MembershipId",
                table: "MembershipServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_MembershipServices_MembershipServiceId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemberships_Memberships_MembershipId",
                table: "UserMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMemberships_Users_UserId",
                table: "UserMemberships");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipServices_Memberships_MembershipId",
                table: "MembershipServices",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_MembershipServices_MembershipServiceId",
                table: "Posts",
                column: "MembershipServiceId",
                principalTable: "MembershipServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemberships_Memberships_MembershipId",
                table: "UserMemberships",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMemberships_Users_UserId",
                table: "UserMemberships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
