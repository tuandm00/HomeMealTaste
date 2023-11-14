using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HomeMealTaste.Data.Models
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

        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<DishType> DishTypes { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Kitchen> Kitchens { get; set; } = null!;
        public virtual DbSet<Meal> Meals { get; set; } = null!;
        public virtual DbSet<MealDish> MealDishes { get; set; } = null!;
        public virtual DbSet<MealSession> MealSessions { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Wallet> Wallets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.District).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Customer_Area");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Customer_User");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");

                entity.Property(e => e.Image).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.DishType)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.DishTypeId)
                    .HasConstraintName("FK_Dish_DishType");

                entity.HasOne(d => d.Kitchen)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.KitchenId)
                    .HasConstraintName("FK_Dish_Kitchen");
            });

            modelBuilder.Entity<DishType>(entity =>
            {
                entity.ToTable("DishType");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("District");

                entity.Property(e => e.DistrictName).HasMaxLength(50);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Feedback_Customer");

                entity.HasOne(d => d.Kitchen)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.KitchenId)
                    .HasConstraintName("FK_Feedback_Kitchen");
            });

            modelBuilder.Entity<Kitchen>(entity =>
            {
                entity.ToTable("Kitchen");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Kitchens)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Kitchen_Area");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Kitchens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Kitchen_User");
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.ToTable("Meal");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Image).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Kitchen)
                    .WithMany(p => p.Meals)
                    .HasForeignKey(d => d.KitchenId)
                    .HasConstraintName("FK_Meal_Kitchen");
            });

            modelBuilder.Entity<MealDish>(entity =>
            {
                entity.ToTable("Meal_Dish");

                entity.Property(e => e.MealDishId).HasColumnName("Meal_DishId");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.MealDishes)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK_Meal_Dish_Dish");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.MealDishes)
                    .HasForeignKey(d => d.MealId)
                    .HasConstraintName("FK_Meal_Dish_Meal");
            });

            modelBuilder.Entity<MealSession>(entity =>
            {
                entity.ToTable("Meal_Session");

                entity.Property(e => e.MealSessionId).HasColumnName("Meal_SessionId");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.HasOne(d => d.Kitchen)
                    .WithMany(p => p.MealSessions)
                    .HasForeignKey(d => d.KitchenId)
                    .HasConstraintName("FK_Meal_Session_Kitchen");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.MealSessions)
                    .HasForeignKey(d => d.MealId)
                    .HasConstraintName("FK_Meal_Session_Meal");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.MealSessions)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK_Meal_Session_Session");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.MealSessionId).HasColumnName("Meal_SessionId");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Time).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.MealSession)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MealSessionId)
                    .HasConstraintName("FK_Order_Meal_Session");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.PostId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.MealSessionId).HasColumnName("Meal_SessionId");

                entity.HasOne(d => d.MealSession)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.MealSessionId)
                    .HasConstraintName("FK_Post_Meal_Session");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.EndTime).HasColumnType("smalldatetime");

                entity.Property(e => e.SessionType).HasMaxLength(50);

                entity.Property(e => e.StartTime).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Session_Area");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Session_User");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Transaction_Order");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK_Transaction_Wallet");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Wallet_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
