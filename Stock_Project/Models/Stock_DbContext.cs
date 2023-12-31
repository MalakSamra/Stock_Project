using Microsoft.EntityFrameworkCore;

namespace Stock_Project.Models
{
    public class Stock_DbContext:DbContext
    {
        public virtual DbSet<Store>? Stores { get; set; }
        public virtual DbSet<Item>? Items { get; set; }
        public virtual DbSet<StoreItem>? StoreItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-1B1BE1O;Database=StockDB;Trusted_Connection=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreItem>()
                .HasKey(x => new { x.StoreID, x.ItemID });
            base.OnModelCreating(modelBuilder);
        }
    }
}
