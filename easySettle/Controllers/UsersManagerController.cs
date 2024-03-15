using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace easySettle.Controllers
{
    public class UsersManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersManagerController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            return View(users);
        }
    }
}
