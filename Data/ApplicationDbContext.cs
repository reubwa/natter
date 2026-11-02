using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Natter.Models;

namespace Natter.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Follow> Follows => Set<Follow>();
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

        builder.Entity<Follow>(b =>
        {

            b.HasOne(f => f.Follower).WithMany(f => f.Following).HasForeignKey(f => f.FollowerId).OnDelete(DeleteBehavior.Cascade);
            
            b.HasOne(f => f.Followee).WithMany(f => f.Followers).HasForeignKey(f => f.FolloweeId).OnDelete(DeleteBehavior.Cascade);
            
            b.HasIndex(f => f.FolloweeId);
            
            b.ToTable(t => t.HasCheckConstraint(
                "CK_Follow_NoSelfFollow",
                "FollowerId <> FolloweeId"));
            
        });
        
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