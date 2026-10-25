using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Natter.Models;

namespace Natter.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Post> Posts => Set<Post>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Post>()
            .HasIndex(p => new { p.AuthorId, p.CreatedAt });
        builder.Entity<ApplicationUser>()
            .Property(u => u.ScreenName)
            .UseCollation("NOCASE");

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.ScreenName)
            .IsUnique();
    }
}