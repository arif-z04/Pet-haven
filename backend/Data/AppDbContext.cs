using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Models;

namespace PetHaven.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<PetType> PetTypes => Set<PetType>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ProductQuestion> ProductQuestions => Set<ProductQuestion>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
    public DbSet<PetProfile> PetProfiles => Set<PetProfile>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Constraints
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Phone).IsUnique();
        });

        // Category Self-Referencing Parent
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(c => c.Slug).IsUnique();
            entity.HasOne(c => c.Parent)
                  .WithMany(c => c.SubCategories)
                  .HasForeignKey(c => c.ParentId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(c => c.PetType)
                  .WithMany(pt => pt.Categories)
                  .HasForeignKey(c => c.PetTypeId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // PetType
        modelBuilder.Entity<PetType>(entity =>
        {
            entity.HasIndex(pt => pt.Name).IsUnique();
            entity.HasIndex(pt => pt.Slug).IsUnique();
        });

        // Brand
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasIndex(b => b.Name).IsUnique();
            entity.HasIndex(b => b.Slug).IsUnique();
        });

        // Product Constraints & Relationships
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(p => p.Slug).IsUnique();
            entity.HasIndex(p => p.Sku).IsUnique();
            entity.HasIndex(p => p.Barcode).IsUnique();

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Brand)
                  .WithMany(b => b.Products)
                  .HasForeignKey(p => p.BrandId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(p => p.PetType)
                  .WithMany(pt => pt.Products)
                  .HasForeignKey(p => p.PetTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Product Images & Variants Cascade
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasOne(pi => pi.Product)
                  .WithMany(p => p.Images)
                  .HasForeignKey(pi => pi.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasIndex(pv => pv.Sku).IsUnique();
            entity.HasOne(pv => pv.Product)
                  .WithMany(p => p.Variants)
                  .HasForeignKey(pv => pv.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Cart & Items
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasIndex(c => c.UserId).IsUnique();
            entity.HasIndex(c => c.SessionToken).IsUnique();
            entity.HasOne(c => c.User)
                  .WithOne(u => u.Cart)
                  .HasForeignKey<Cart>(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasIndex(ci => new { ci.CartId, ci.ProductId, ci.VariantId }).IsUnique();
            entity.HasOne(ci => ci.Cart)
                  .WithMany(c => c.Items)
                  .HasForeignKey(ci => ci.CartId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ci => ci.Product)
                  .WithMany()
                  .HasForeignKey(ci => ci.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ci => ci.Variant)
                  .WithMany()
                  .HasForeignKey(ci => ci.VariantId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Wishlist
        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasIndex(w => w.UserId).IsUnique();
            entity.HasOne(w => w.User)
                  .WithOne(u => u.Wishlist)
                  .HasForeignKey<Wishlist>(w => w.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasIndex(wi => new { wi.WishlistId, wi.ProductId }).IsUnique();
            entity.HasOne(wi => wi.Wishlist)
                  .WithMany(w => w.Items)
                  .HasForeignKey(wi => wi.WishlistId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(wi => wi.Product)
                  .WithMany()
                  .HasForeignKey(wi => wi.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Coupon
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasIndex(c => c.Code).IsUnique();
        });

        // Orders & Order Items
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(o => o.OrderNumber).IsUnique();
            entity.HasOne(o => o.User)
                  .WithMany(u => u.Orders)
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.Address)
                  .WithMany()
                  .HasForeignKey(o => o.AddressId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(oi => oi.Variant)
                  .WithMany(v => v.OrderItems)
                  .HasForeignKey(oi => oi.VariantId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Reviews
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasIndex(r => new { r.UserId, r.ProductId }).IsUnique();
            entity.HasOne(r => r.User)
                  .WithMany(u => u.Reviews)
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(r => r.Product)
                  .WithMany(p => p.Reviews)
                  .HasForeignKey(r => r.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(r => r.Order)
                  .WithMany()
                  .HasForeignKey(r => r.OrderId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Pet Profile
        modelBuilder.Entity<PetProfile>(entity =>
        {
            entity.HasOne(p => p.User)
                  .WithMany(u => u.PetProfiles)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.PetType)
                  .WithMany(pt => pt.PetProfiles)
                  .HasForeignKey(p => p.PetTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Blog
        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasIndex(bc => bc.Slug).IsUnique();
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasIndex(bp => bp.Slug).IsUnique();
            entity.HasOne(bp => bp.Category)
                  .WithMany(bc => bc.Posts)
                  .HasForeignKey(bp => bp.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(bp => bp.Author)
                  .WithMany()
                  .HasForeignKey(bp => bp.AuthorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(bp => bp.PetType)
                  .WithMany()
                  .HasForeignKey(bp => bp.PetTypeId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Inventory Transactions
        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasOne(it => it.Product)
                  .WithMany(p => p.InventoryTransactions)
                  .HasForeignKey(it => it.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(it => it.Variant)
                  .WithMany(pv => pv.InventoryTransactions)
                  .HasForeignKey(it => it.VariantId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(it => it.ReferenceOrder)
                  .WithMany(o => o.InventoryTransactions)
                  .HasForeignKey(it => it.ReferenceOrderId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
