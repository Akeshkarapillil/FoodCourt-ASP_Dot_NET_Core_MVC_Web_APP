
namespace FoodCourt.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
