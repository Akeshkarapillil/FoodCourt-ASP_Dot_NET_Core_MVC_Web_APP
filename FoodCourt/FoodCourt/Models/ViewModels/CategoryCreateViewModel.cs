namespace FoodCourt.Models.ViewModels
{
    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
