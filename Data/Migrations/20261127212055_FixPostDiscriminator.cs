using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Natter.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPostDiscriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE Posts SET Discriminator = 'Post' WHERE Discriminator = '';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
