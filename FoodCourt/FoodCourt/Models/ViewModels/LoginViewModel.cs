namespace FoodCourt.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(25)]
        public string Password { get; set; }

        [DataType("checkbox")]
        public bool RememberMe { get; set; }
    }
}
