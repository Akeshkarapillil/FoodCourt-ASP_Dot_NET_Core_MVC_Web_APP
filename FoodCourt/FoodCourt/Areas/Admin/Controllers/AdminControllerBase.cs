using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodCourt.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminControllerBase : Controller
    {
    }
}
