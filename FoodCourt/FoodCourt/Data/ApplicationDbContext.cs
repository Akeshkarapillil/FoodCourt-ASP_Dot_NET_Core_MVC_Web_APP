using FoodCourt.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FoodCourt.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var categories = new List<Category>()
            {
                new Category{Id = 1, Name = "Uncategorized", Description = "Uncategorized", IsDefault=true},
                new Category{Id = 2, Name = "Beverages", Description = "Beverages", IsDefault=true},
                new Category{Id = 3, Name = "Bread/Bakery", Description = "Bread/Bakery", IsDefault=true},
                new Category{Id = 4, Name = "Canned/Jarred Goods", Description = "Canned/Jarred Goods", IsDefault=true},
                new Category{Id = 5, Name = "Dairy", Description = "Dairy", IsDefault=true}
            };
            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Category>().Property(m => m.IsDefault).HasDefaultValue(false);

            var roles = new List<IdentityRole>()
            {
                new IdentityRole{Id="0a2ddf40-f92c-4fb5-b66a-b1fd73a619f8", Name="Admin", NormalizedName="ADMIN"},
                new IdentityRole{Id="c9a5b829-95d1-400f-9d4b-333766c80b7c", Name="Staff", NormalizedName="STAFF"},
                new IdentityRole{Id="ede69915-fbd3-4632-8f73-7e27f0b2e2be", Name="User", NormalizedName="USER"}
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }

        public DbSet<Category> Categories { get; set; }
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

    }
}
