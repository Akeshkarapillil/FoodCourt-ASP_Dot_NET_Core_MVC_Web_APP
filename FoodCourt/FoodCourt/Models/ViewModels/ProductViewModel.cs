using FoodCourt.Core.Types;
using FoodCourt.Core.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;


namespace FoodCourt.Models.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [StringLength(70)]
        public string Name { get; set; }

        [Required]
        [StringLength(70)]
        public string Slug { get; set; }

        [StringLength(500)]
        public string ShortDescription { get; set; }

        [StringLength(20000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [Required]
        public ProductStatus Status { get; set; }

        //[Required(ErrorMessage = "Please select an image file.")]
        [ContentType(FileType.Image)]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only image files with .jpg, .jpeg, or .png extensions are allowed.")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "The maximum file size allowed is 5 MB.")]
        public IEnumerable<IFormFile>? ProductImages { get; set; }

        public int CategoryId { get; set; }

        public SelectList? Categories { get; set; }
    }
}
