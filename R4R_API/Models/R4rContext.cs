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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<imgRoom> ImgRooms { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<PayRoom> PayRooms { get; set; }

    public virtual DbSet<hisRecharge> HisRecharges { get; set; }

    private string host = Environment.GetEnvironmentVariable("PGHOST");


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql("Host=containers-us-west-12.railway.app;Port=7353;Database=railway;Username=postgres;Password=nwQ6SIdnBq9a3XcVh7IJ");

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
            entity.Property(e => e.noSex)
                .HasMaxLength(255)
                .HasColumnName("nosex");
            entity.Property(e => e.utilities)
                .HasMaxLength(255)
                .HasColumnName("utilities");
            entity.Property(e => e.imgRoom).HasColumnName("imgroom");
            entity.Property(e => e.view).HasColumnName("view");
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
            entity.Property(e => e.bankBal).HasColumnName("bank_balance"); 
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
        });

        modelBuilder.Entity<imgRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("imgRoom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.idroom)
                .HasColumnName("idroom");
            entity.Property(e => e.imgbase64)
                .HasColumnName("imgbase64");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tenant_pkey");

            entity.ToTable("Tenant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.idRoom)
                .HasColumnName("id_room");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.adress)
                .HasMaxLength(255)
                .HasColumnName("adress");
            entity.Property(e => e.phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.cartId)
                .HasMaxLength(255)
                .HasColumnName("cart_id");
            entity.Property(e => e.dateJoin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_join");
            entity.Property(e => e.dateOut)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_out");
            entity.Property(e => e.status).HasColumnName("status");
        });

        modelBuilder.Entity<hisRecharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("his_recharge_pkey");

            entity.ToTable("his_recharge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.userEmail)
                .HasColumnName("user_email");
            entity.Property(e => e.moneyRecharge)
                .HasColumnName("money_recharge");
            entity.Property(e => e.note)
                .HasColumnName("note");
            entity.Property(e => e.createDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_create");
        });

        modelBuilder.Entity<PayRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pay_room_pkey");
            entity.ToTable("pay_room");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CartId)
                .HasColumnName("cart_id");
            entity.Property(e => e.IdRoom)
                .HasColumnName("id_room");
            entity.Property(e => e.Month)
                .HasColumnName("month");
            entity.Property(e => e.NoWater)
                .HasColumnName("no_water");
            entity.Property(e => e.RoomPrice)
                .HasColumnName("room_price");
            entity.Property(e => e.note)
                .HasColumnName("note");
            entity.Property(e => e.NoElectic)
                .HasColumnName("no_electic");
            entity.Property(e => e.Created)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created");
            entity.Property(e => e.otherPrice).HasColumnName("otherprice");
            entity.Property(e => e.status).HasColumnName("status");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
