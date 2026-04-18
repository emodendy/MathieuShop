using System;
using Microsoft.EntityFrameworkCore;

namespace MathieuShop.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Role>      Roles      { get; set; }
        public virtual DbSet<User>      Users      { get; set; }
        public virtual DbSet<Category>  Categories { get; set; }
        public virtual DbSet<Supplier>  Suppliers  { get; set; }
        public virtual DbSet<Product>   Products   { get; set; }
        public virtual DbSet<Order>     Orders     { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    "Host=localhost;Port=5432;Database=MathieuShopDB;Username=postgres;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.ToTable("Roles");
                entity.Property(e => e.RoleId).HasColumnName("RoleId").UseIdentityAlwaysColumn();
                entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("Name");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToTable("Users");
                entity.Property(e => e.UserId).HasColumnName("UserId").UseIdentityAlwaysColumn();
                entity.Property(e => e.Login).HasMaxLength(100).HasColumnName("Login");
                entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("Password");
                entity.Property(e => e.FullName).HasMaxLength(150).HasColumnName("FullName");
                entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("Email");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
                entity.HasIndex(e => e.Login).IsUnique();

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.ToTable("Categories");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId").UseIdentityAlwaysColumn();
                entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("Name");
                entity.Property(e => e.Description).HasColumnName("Description");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId);
                entity.ToTable("Suppliers");
                entity.Property(e => e.SupplierId).HasColumnName("SupplierId").UseIdentityAlwaysColumn();
                entity.Property(e => e.CompanyName).HasMaxLength(150).HasColumnName("CompanyName");
                entity.Property(e => e.ContactName).HasMaxLength(100).HasColumnName("ContactName");
                entity.Property(e => e.Phone).HasMaxLength(30).HasColumnName("Phone");
                entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("Email");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.ToTable("Products");
                entity.Property(e => e.ProductId).HasColumnName("ProductId").UseIdentityAlwaysColumn();
                entity.Property(e => e.Name).HasMaxLength(150).HasColumnName("Name");
                entity.Property(e => e.Price).HasPrecision(10, 2).HasColumnName("Price");
                entity.Property(e => e.Stock).HasColumnName("Stock");
                entity.Property(e => e.ImagePath).HasMaxLength(300).HasColumnName("ImagePath");
                entity.Property(e => e.Collection).HasMaxLength(100).HasColumnName("Collection");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
                entity.Property(e => e.SupplierId).HasColumnName("SupplierId");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.ToTable("Orders");
                entity.Property(e => e.OrderId).HasColumnName("OrderId").UseIdentityAlwaysColumn();
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.OrderDate).HasColumnName("OrderDate");
                entity.Property(e => e.Status).HasMaxLength(50).HasColumnName("Status");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);
                entity.ToTable("OrderItems");
                entity.Property(e => e.OrderItemId).HasColumnName("OrderItemId").UseIdentityAlwaysColumn();
                entity.Property(e => e.OrderId).HasColumnName("OrderId");
                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.Quantity).HasColumnName("Quantity");
                entity.Property(e => e.UnitPrice).HasPrecision(10, 2).HasColumnName("UnitPrice");
                entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");

                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
