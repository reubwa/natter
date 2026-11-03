using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Natter.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixScreenName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ScreenName",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ScreenName",
                table: "AspNetUsers",
                column: "ScreenName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ScreenName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ScreenName",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true,
                oldCollation: "NOCASE");
        }
    }
}
