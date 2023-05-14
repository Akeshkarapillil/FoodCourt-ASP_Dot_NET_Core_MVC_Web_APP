﻿namespace FoodCourt.Models.ViewModels
{
    public class ProfileUpdateViewModel
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
    }
}
