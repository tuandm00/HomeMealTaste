using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HomeMealTaste.Models
{
    public partial class HomeMealTasteContext : DbContext
    {
        public HomeMealTasteContext()
        {
        }

        public HomeMealTasteContext(DbContextOptions<HomeMealTasteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Chef> Chefs { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Discount> Discounts { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<DishType> DishTypes { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<FoodPackage> FoodPackages { get; set; } = null!;
        public virtual DbSet<FoodPackageDish> FoodPackageDishes { get; set; } = null!;
        public virtual DbSet<FoodPackageSession> FoodPackageSessions { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.AdminId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Chef>(entity =>
            {
                entity.ToTable("Chef");

                entity.Property(e => e.ChefId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.DiscountId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.DiscountCode).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");

                entity.Property(e => e.DishId).ValueGeneratedNever();

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.ChefId)
                    .HasConstraintName("FK_Dish_Chef");

                entity.HasOne(d => d.DishType)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.DishTypeId)
                    .HasConstraintName("FK_Dish_DishType");

                entity.HasOne(d => d.FoodPackage)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.FoodPackageId)
                    .HasConstraintName("FK_Dish_FoodPackage");
            });

            modelBuilder.Entity<DishType>(entity =>
            {
                entity.ToTable("DishType");

                entity.Property(e => e.DishTypeId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.DishTypes)
                    .HasForeignKey(d => d.ChefId)
                    .HasConstraintName("FK_DishType_Chef");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.HasOne(d => d.Chef)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.ChefId)
                    .HasConstraintName("FK_Feedback_Chef");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Feedback_Customer");
            });

            modelBuilder.Entity<FoodPackage>(entity =>
            {
                entity.ToTable("FoodPackage");

                entity.Property(e => e.FoodPackageId).ValueGeneratedNever();

                entity.Property(e => e.DefaultPrice).HasColumnType("money");

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<FoodPackageDish>(entity =>
            {
                entity.ToTable("FoodPackage_Dish");

                entity.Property(e => e.FoodPackageDishId)
                    .ValueGeneratedNever()
                    .HasColumnName("FoodPackage_DishId");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.FoodPackageDishes)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK_FoodPackage_Dish_Dish");

                entity.HasOne(d => d.FoodPackage)
                    .WithMany(p => p.FoodPackageDishes)
                    .HasForeignKey(d => d.FoodPackageId)
                    .HasConstraintName("FK_FoodPackage_Dish_FoodPackage");
            });

            modelBuilder.Entity<FoodPackageSession>(entity =>
            {
                entity.ToTable("FoodPackage_Session");

                entity.Property(e => e.FoodPackageSessionId)
                    .ValueGeneratedNever()
                    .HasColumnName("FoodPackage_SessionId");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.FoodPackage)
                    .WithMany(p => p.FoodPackageSessions)
                    .HasForeignKey(d => d.FoodPackageId)
                    .HasConstraintName("FK_FoodPackage_Session_FoodPackage");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.FoodPackageSessions)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK_FoodPackage_Session_Session");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");

                entity.Property(e => e.OrderItemId).ValueGeneratedNever();

                entity.Property(e => e.FoodPackageSessionId).HasColumnName("FoodPackage_SessionId");

                entity.HasOne(d => d.FoodPackage)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.FoodPackageId)
                    .HasConstraintName("FK_OrderItem_FoodPackage");

                entity.HasOne(d => d.FoodPackageSession)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.FoodPackageSessionId)
                    .HasConstraintName("FK_OrderItem_FoodPackage_Session");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderItem_Order");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");

                entity.Property(e => e.SessionId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("date");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");

                entity.Property(e => e.WalletId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
