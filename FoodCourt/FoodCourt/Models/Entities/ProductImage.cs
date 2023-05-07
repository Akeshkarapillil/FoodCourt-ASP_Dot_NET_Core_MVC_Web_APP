namespace FoodCourt.Models.Entities
{
    [Index(nameof(ImageUrl), IsUnique = true)]
    public class ProductImage
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; } = null;
    }
}
