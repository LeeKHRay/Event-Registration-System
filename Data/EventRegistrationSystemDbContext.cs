using EventRegistrationSystem.Areas.Identity.Models;
using EventRegistrationSystem.Models.Events;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationSystem.Data;

public class EventRegistrationSystemDbContext : IdentityDbContext<ApplicationUser>
{
    public EventRegistrationSystemDbContext(DbContextOptions<EventRegistrationSystemDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().UseTptMappingStrategy();

        // avoid causing multiple cascade paths
        modelBuilder.Entity<OrganizationUser>()
            .HasMany(e => e.Events)
            .WithOne(ei => ei.Creator)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Event>()
            .Property(ei => ei.Location)
            .HasMaxLength(100)
            .IsRequired(true);

        modelBuilder.Entity<Event>()
            .Property(ei => ei.Description)
            .HasMaxLength(1000)
            .IsRequired(true);
    }

    public DbSet<GeneralUser> GeneralUser { get; set; }
    public DbSet<OrganizationUser> OrganizationUser { get; set; }

    public DbSet<Event> Event { get; set; } = default!;
    public DbSet<EventCategory> EventCategory { get; set; } = default!;
    public DbSet<EventImage> EventImage { get; set; } = default!;
    public DbSet<EventEnrollment> EventEnrollment { get; set; } = default!;
}
