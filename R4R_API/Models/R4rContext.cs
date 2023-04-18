using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace R4R_API.Models;

public partial class R4rContext : DbContext
{
    public R4rContext()
    {
    }

    public R4rContext(DbContextOptions<R4rContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=containers-us-west-12.railway.app;Database=railway;Username=postgres;Password=nwQ6SIdnBq9a3XcVh7IJ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("room_pkey");

            entity.ToTable("room");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Activeby)
                .HasMaxLength(255)
                .HasColumnName("activeby");
            entity.Property(e => e.Activedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("activedate");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Area)
                .HasMaxLength(255)
                .HasColumnName("area");
            entity.Property(e => e.Capacity)
                .HasMaxLength(255)
                .HasColumnName("capacity");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Createdby)
                .HasMaxLength(255)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Deposit)
                .HasMaxLength(255)
                .HasColumnName("deposit");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Electricprice)
                .HasMaxLength(255)
                .HasColumnName("electricprice");
            entity.Property(e => e.Houseowner)
                .HasMaxLength(255)
                .HasColumnName("houseowner");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Otherprice)
                .HasMaxLength(255)
                .HasColumnName("otherprice");
            entity.Property(e => e.Ownerphone)
                .HasMaxLength(255)
                .HasColumnName("ownerphone");
            entity.Property(e => e.Price)
                .HasMaxLength(255)
                .HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Waterprice)
                .HasMaxLength(255)
                .HasColumnName("waterprice");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .HasColumnName("fullname");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
