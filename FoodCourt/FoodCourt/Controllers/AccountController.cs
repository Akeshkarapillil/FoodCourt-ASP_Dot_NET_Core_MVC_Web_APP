using FoodCourt.Core.ExtensionMethods;
using FoodCourt.Models.Entities;
using FoodCourt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodCourt.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model, [FromQuery] string ReturnUrl = null)
        {
            ReturnUrl ??= Url.Action("Index", "Home");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "There is no account with this email id.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                TempData.SetMessage(MessageType.Success, "User logged in.");
                if(User.IsInRole("Admin"))
                {
                    return LocalRedirect("/Admin");
                }
                return LocalRedirect(ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                TempData.SetMessage(MessageType.Error, "User account locked out.");
                return RedirectToAction(nameof(Lockout));
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = Guid.NewGuid().ToString(),
            };

            var userCreation = await _userManager.CreateAsync(user, model.Password);
            if (userCreation.Succeeded)
            {
                TempData.SetMessage(MessageType.Success, "Account created successfully.");
                await _userManager.AddToRoleAsync(user, "User");
                return RedirectToAction(nameof(Login));
            }

            // Show error message if login failed.
            foreach (var error in userCreation.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            TempData.SetMessage(MessageType.Error, "Account creation failed.");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var model = new ProfileUpdateViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile([FromForm] ProfileUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);


            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if(emailExists == null)
            {
                ModelState.AddModelError("Email", "Email already taken");
                return View(model);
            }

            user.Email = model.Email; 
            user.FirstName = model.FirstName; 
            user.LastName = model.LastName;

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }
    }
}
