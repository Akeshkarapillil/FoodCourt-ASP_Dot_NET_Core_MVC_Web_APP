using Microsoft.AspNetCore.Identity;

namespace FoodCourt.Models.Entities
{
    public class ApplicationUser: IdentityUser
    {
        [StringLength(25)]
        public string FirstName { get; set; }

        [StringLength(25)]
        public string LastName { get; set; }

    }
}
