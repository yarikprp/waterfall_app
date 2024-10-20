using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace waterfall_app.Classes;

public partial class WaterfallDbContext : DbContext
{
    public WaterfallDbContext()
    {
    }

    public WaterfallDbContext(DbContextOptions<WaterfallDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Shedule> Shedules { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TypeTicket> TypeTickets { get; set; }

    public virtual DbSet<Visiter> Visiters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=waterfall_db;Username=postgres;Password=1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shedule>(entity =>
        {
            entity.HasKey(e => e.IdShedule).HasName("shedule_pkey");

            entity.ToTable("shedule");

            entity.Property(e => e.IdShedule).HasColumnName("id_shedule");
            entity.Property(e => e.EntranceTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("entrance_time");
            entity.Property(e => e.NumberOfPeople)
                .HasDefaultValue(0)
                .HasColumnName("number_of_people");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("ticket_pkey");

            entity.ToTable("ticket");

            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.IdShedule).HasColumnName("id_shedule");
            entity.Property(e => e.IdVisiter).HasColumnName("id_visiter");
            entity.Property(e => e.NumberTicket)
                .HasMaxLength(50)
                .HasColumnName("number_ticket");

            entity.HasOne(d => d.IdSheduleNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdShedule)
                .HasConstraintName("ticket_id_shedule_fkey");

            entity.HasOne(d => d.IdVisiterNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdVisiter)
                .HasConstraintName("ticket_id_visiter_fkey");
        });

        modelBuilder.Entity<TypeTicket>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("type_ticket_pkey");

            entity.ToTable("type_ticket");

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.TypeName)
                .HasMaxLength(20)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Visiter>(entity =>
        {
            entity.HasKey(e => e.IdVisiter).HasName("visiter_pkey");

            entity.ToTable("visiter");

            entity.Property(e => e.IdVisiter).HasColumnName("id_visiter");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Fathersname)
                .HasMaxLength(100)
                .HasColumnName("fathersname");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Visiters)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("visiter_id_type_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
