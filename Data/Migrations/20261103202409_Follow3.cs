using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Natter.Data.Migrations
{
    /// <inheritdoc />
    public partial class Follow3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Follow_NoSelfFollow",
                table: "Follows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Follow_NoSelfFollow",
                table: "Follows",
                sql: "FollowerId <> FolloweeId");
        }
    }
}
