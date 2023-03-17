using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Write_Erase.Models;

public partial class TradeContext : DbContext
{
    public TradeContext()
    {
    }

    public TradeContext(DbContextOptions<TradeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Measurement> Measurements { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrderUser> OrderUsers { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Сategory> Сategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=trade", ServerVersion.Parse("8.0.25-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Idmanufacturers).HasName("PRIMARY");

            entity.ToTable("manufacturers");

            entity.Property(e => e.Idmanufacturers).HasColumnName("idmanufacturers");
            entity.Property(e => e.Manufacturer1)
                .HasMaxLength(80)
                .HasColumnName("manufacturer");
        });

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.HasKey(e => e.Idmeasurements).HasName("PRIMARY");

            entity.ToTable("measurements");

            entity.Property(e => e.Idmeasurements).HasColumnName("idmeasurements");
            entity.Property(e => e.Measurement1)
                .HasMaxLength(45)
                .HasColumnName("measurement");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.IdorderStatus).HasName("PRIMARY");

            entity.ToTable("order_status");

            entity.Property(e => e.IdorderStatus).HasColumnName("idorder_status");
            entity.Property(e => e.OrderStatus1)
                .HasMaxLength(45)
                .HasColumnName("order_status");
        });

        modelBuilder.Entity<OrderUser>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("order_user");

            entity.HasIndex(e => e.OrderPickupPoint, "fk_OrderPickupPoint_idx");

            entity.HasIndex(e => e.OrderStatus, "fk_OrderStatus_idx");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.FioClient)
                .HasMaxLength(90)
                .HasColumnName("FIO_Client");
            entity.Property(e => e.GetCode).HasColumnName("Get_Code");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDeliveryDate).HasColumnType("datetime");

            entity.HasOne(d => d.OrderPickupPointNavigation).WithMany(p => p.OrderUsers)
                .HasForeignKey(d => d.OrderPickupPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OrderPickupPoint");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.OrderUsers)
                .HasForeignKey(d => d.OrderStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OrderStatus");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("orderproduct");

            entity.HasIndex(e => e.ProductArticleNumber, "ProductArticleNumber");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.Count).HasMaxLength(45);

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_ibfk_1");

            entity.HasOne(d => d.ProductArticleNumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.ProductArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_ibfk_2");
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.IdpickupPoint).HasName("PRIMARY");

            entity.ToTable("pickup_point");

            entity.Property(e => e.IdpickupPoint).HasColumnName("idpickup_point");
            entity.Property(e => e.City).HasMaxLength(45);
            entity.Property(e => e.Street).HasMaxLength(45);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticleNumber).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductCategory, "fk_ProductCategory_idx");

            entity.HasIndex(e => e.ProductManufacturer, "fk_ProductManufacturer_idx");

            entity.HasIndex(e => e.ProductMeasurement, "fk_ProductMeasurement_idx");

            entity.HasIndex(e => e.ProductSupplier, "fk_ProductSupplier_idx");

            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.ProductCost).HasPrecision(19, 4);
            entity.Property(e => e.ProductDescription).HasColumnType("text");
            entity.Property(e => e.ProductName).HasColumnType("text");
            entity.Property(e => e.ProductPhoto).HasColumnType("text");
            entity.Property(e => e.ProductStatus).HasColumnType("text");

            entity.HasOne(d => d.ProductCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ProductCategory");

            entity.HasOne(d => d.ProductManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductManufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ProductManufacturer");

            entity.HasOne(d => d.ProductMeasurementNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductMeasurement)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ProductMeasurement");

            entity.HasOne(d => d.ProductSupplierNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductSupplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ProductSupplier");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Idsuppliers).HasName("PRIMARY");

            entity.ToTable("suppliers");

            entity.Property(e => e.Idsuppliers).HasColumnName("idsuppliers");
            entity.Property(e => e.Supplier1)
                .HasMaxLength(45)
                .HasColumnName("supplier");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserRole, "UserRole");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserLogin).HasColumnType("text");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPassword).HasColumnType("text");
            entity.Property(e => e.UserPatronymic).HasMaxLength(100);
            entity.Property(e => e.UserSurname).HasMaxLength(100);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<Сategory>(entity =>
        {
            entity.HasKey(e => e.Idсategories).HasName("PRIMARY");

            entity.ToTable("сategories");

            entity.Property(e => e.Idсategories).HasColumnName("idсategories");
            entity.Property(e => e.СategoryProduct)
                .HasMaxLength(80)
                .HasColumnName("сategory_product");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
