using FoodCourt.Core.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCourt.Models.Entities
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }

        [StringLength(70)]
        public string Name { get; set; }

        [StringLength(70)]
        public string Slug { get; set; }

        [StringLength(500)]
        public string ShortDescription { get; set; }

        [StringLength(20000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime? DateUpdated { get; set; }

        public ProductStatus Status { get; set; } = 0;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null;

        public IEnumerable<ProductImage> ProductImages { get; set; }
    }


}
