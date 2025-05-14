using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StinkyModel;

public partial class Comp584DatabaseContext : IdentityDbContext<User>
{
    public Comp584DatabaseContext()
    {
    }

    public Comp584DatabaseContext(DbContextOptions<Comp584DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Make> Makes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Car>(entity =>
        {
            entity.Property(e => e.Model).IsFixedLength();

            entity.HasOne(d => d.Make).WithMany(p => p.Cars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_car_make");
        });

        modelBuilder.Entity<Make>(entity =>
        {
            entity.Property(e => e.Name).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
