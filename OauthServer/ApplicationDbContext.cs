using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OauthServer.Features.ProductRequest;
using OauthServer.Helpers;

namespace OauthServer;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ProductRequest> ProductRequest { get; set; }

    public DbSet<ProductRequestVote> ProductRequestVote { get; set; }

    public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            ((BaseEntity) entry.Entity).DateUpdated = DateTime.Now;
            if (entry.State == EntityState.Added)
            {
                ((BaseEntity) entry.Entity).DateCreated = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}