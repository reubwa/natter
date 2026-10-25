using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Natter.Models;

namespace Natter.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);   // keep this, Identity's table setup lives here

        builder.Entity<ApplicationUser>()
            .Property(u => u.ScreenName)
            .UseCollation("NOCASE");

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.ScreenName)
            .IsUnique();
    }
}