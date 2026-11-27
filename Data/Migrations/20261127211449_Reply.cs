using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Natter.Data;

#nullable disable

namespace Natter.Data.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20261127211449_Reply")]
    public partial class Reply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;", suppressTransaction: true);

            migrationBuilder.Sql(
                """
                CREATE TABLE "Posts_new" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_Posts" PRIMARY KEY AUTOINCREMENT,
                    "Content" TEXT NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    "AuthorId" TEXT NOT NULL,
                    "Discriminator" TEXT NOT NULL DEFAULT '',
                    "ParentPostId" INTEGER NULL,
                    CONSTRAINT "FK_Posts_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
                    CONSTRAINT "FK_Posts_Posts_ParentPostId" FOREIGN KEY ("ParentPostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
                );
                INSERT INTO "Posts_new" ("Id", "Content", "CreatedAt", "AuthorId", "Discriminator", "ParentPostId")
                    SELECT "Id", "Content", "CreatedAt", "AuthorId", '', NULL FROM "Posts";
                DROP TABLE "Posts";
                ALTER TABLE "Posts_new" RENAME TO "Posts";
                CREATE INDEX "IX_Posts_AuthorId_CreatedAt" ON "Posts" ("AuthorId", "CreatedAt");
                CREATE INDEX "IX_Posts_ParentPostId" ON "Posts" ("ParentPostId");
                """);

            migrationBuilder.Sql("PRAGMA foreign_keys = ON;", suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;", suppressTransaction: true);

            migrationBuilder.Sql(
                """
                CREATE TABLE "Posts_old" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_Posts" PRIMARY KEY AUTOINCREMENT,
                    "Content" TEXT NOT NULL,
                    "CreatedAt" TEXT NOT NULL,
                    "AuthorId" TEXT NOT NULL,
                    CONSTRAINT "FK_Posts_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
                );
                INSERT INTO "Posts_old" ("Id", "Content", "CreatedAt", "AuthorId")
                    SELECT "Id", "Content", "CreatedAt", "AuthorId" FROM "Posts";
                DROP TABLE "Posts";
                ALTER TABLE "Posts_old" RENAME TO "Posts";
                CREATE INDEX "IX_Posts_AuthorId_CreatedAt" ON "Posts" ("AuthorId", "CreatedAt");
                """);

            migrationBuilder.Sql("PRAGMA foreign_keys = ON;", suppressTransaction: true);
        }
    }
}
