using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Natter.Models;

namespace Natter.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Like> Likes => Set<Like>();
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
        
        builder.Entity<Like>(b =>
        {

            b.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(l => l.UserId);
        });
    }
}