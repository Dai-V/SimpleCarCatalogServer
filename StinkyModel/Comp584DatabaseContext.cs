using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StinkyModel;

public partial class Comp584DatabaseContext : DbContext
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=Comp584Database;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
