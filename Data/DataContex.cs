using server.Entities;
using Microsoft.EntityFrameworkCore;

namespace server.Data
{
    public class DataContex:DbContext
    {
        private readonly IConfiguration _config;
        public DataContex(DbContextOptions<DataContex> options,IConfiguration config): base(options)
        {
            this._config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = _config["ConnectionStrings:Auth"]?? throw new Exception("Connection string missing");
            string dbServer = _config["ConnectionStrings:DbServer"] ?? throw new Exception("DbServer Missing");
            string dbName = _config["ConnectionStrings:DbName"]??throw new Exception("DbName Missing");
            string dbUser = _config["ConnectionStrings:DbUserId"]??throw new Exception("Db UserName Missing");
            string dbUserPassword = _config["ConnectionStrings:DbUserPassword"]??throw new Exception("Db User Password Missing");

            string ConnectionStrings = String.Format(conn, dbServer, dbName, dbUser, dbUserPassword);

            optionsBuilder.UseSqlServer(ConnectionStrings);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.OriginalPrice).HasPrecision(18, 2);
                entity.Property(p => p.DiscountPercentage).HasColumnType("decimal(5,2)").IsRequired(false);
                entity.Property(p => p.DiscountAmount).HasColumnType("decimal(18,2)").IsRequired(false);
            });


            modelBuilder.Entity<Product>()
                .HasOne(p => p.Thumbnail)
                .WithOne(i => i.Product)
                .HasForeignKey<Product>(p=>p.ThumbnailId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>()
                .HasOne(p=>p.Image)
                .WithOne(i=>i.Category)
                .HasForeignKey<Category>(c=>c.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Brand>()
                .HasOne(p => p.Image)
                .WithOne(i => i.Brand)
                .HasForeignKey<Brand>(b=>b.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is AuditBaseEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDate = now;
                            break;

                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                            entity.ModifiedDate = now;
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

        public DbSet<User> users { get; set; }
        public DbSet<Brand> brands { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Image> images { get; set; }

        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Wishlist> Wishlists {  get; set; }

        public DbSet<ShoppingCartItem> ShopcartItems { get; set; }
        public DbSet<ShoppingCart> Shopcarts { get;  set; }

         public DbSet<ProductReview> ProductReviews { get;  set; }

         public DbSet<Address> Address { get; set; }

         public DbSet<OrderItem> OrderItems { get; set; }
         public DbSet<Order> Orders { get; set; }

         public DbSet<PaymentDetails> PaymentDetails{ get; set; }
         public DbSet<ShippingAddress> shippingAddresses{ get; set; }
    }
}
