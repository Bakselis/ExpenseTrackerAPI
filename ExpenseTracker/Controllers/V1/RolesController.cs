using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers.V1
{
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // // GET
        // public IActionResult Index()
        // {
        //     IdentityUser = 
        //     _userManager.AddToRoleAsync();
        // }

    }
}