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

        public IActionResult RequestItem(string userId)
        {
            var currentRequests = _cogShareContext.Requests.Where(x => x.RequestedItem.Owner.Id == userId).Select(x => x.RequestedItem.Id).ToList();
            var availableItems = _cogShareContext.Items.Where(x => x.CanBorrow && !x.OnLoan && (!x.Consumable || x.QuantityOnHand > 0) && x.Owner.Id == userId && !currentRequests.Contains(x.Id)).ToList();
            var requesteeUserName = _cogShareContext.ApplicationUser.Where(x => x.Id == userId).First().Email;
            var itemRequestModel = new RequestItemViewModel(availableItems, requesteeUserName);
            return View(itemRequestModel);
        }

        [HttpGet]
        public IActionResult CreateItem()
        {
            return View(new CreateItemViewModel());
        }

        [HttpPost]
        public IActionResult CreateItem([FromForm] CreateItemViewModel newItem)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var inventoryItem = new Item()
            {
                DisplayName = newItem.DisplayName,
                CanBorrow = newItem.CanBorrow,
                Owner = user,
                Consumable = newItem.Consumable,
                QuantityOnHand = newItem.QuantityOnHand
            };

            _cogShareContext.Items.Add(inventoryItem);
            _cogShareContext.SaveChanges();

            var items = _cogShareContext.Items.Include(x => x.Borrower).Where(x => x.Owner == user).ToList();
            var itemViewModel = new ItemViewModel(false, "Item Created Successfully", items);

            return View("Items", itemViewModel);
        }

        public IActionResult DeleteItem(int itemId)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            var item = _cogShareContext.Items.Where(x => x.Id == itemId).FirstOrDefault();
            if(item != null)
            {
                _cogShareContext.Items.Remove(item);
                _cogShareContext.SaveChanges();
            }

            var items = _cogShareContext.Items.Include(x => x.Borrower).Where(x => x.Owner == user).ToList();
            var itemViewModel = new ItemViewModel(false, "Item Deleted Successfully", items);

            return View("Items", itemViewModel);
        }

        public IActionResult SendItemRequest(int itemId)
        {
            return View();
        }
    }
}
