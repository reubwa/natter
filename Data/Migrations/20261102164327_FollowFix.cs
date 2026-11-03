using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Natter.Data.Migrations
{
    /// <inheritdoc />
    public partial class FollowFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowerId_FolloweeId",
                table: "Follows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerId_FolloweeId",
                table: "Follows",
                columns: new[] { "FollowerId", "FolloweeId" });
        }
    }
}
