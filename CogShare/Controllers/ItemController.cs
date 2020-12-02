using CogShare.Domain.Entities;
using CogShare.EFCore;
using CogShare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CogShare.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CogShareContext _cogShareContext;

        public ItemController(ILogger<ItemController> logger, UserManager<ApplicationUser> userManager, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _cogShareContext = cogShareContext;
        }

        public IActionResult Items()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result; // get the user ID and do the rest with db context.
            var items = _cogShareContext.Items.Include(x => x.Borrower).Where(x => x.Owner == user).ToList();
            var itemViewModel = new ItemViewModel(items);
            return View(itemViewModel);
        }

        public IActionResult Requests()
        {
            return View();
        }
    }
}
