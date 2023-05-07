namespace FoodCourt.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(25)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
