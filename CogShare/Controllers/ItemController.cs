using CogShare.Domain.Entities;
using CogShare.EFCore;
using CogShare.Models;
using CogShare.Services;
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
        private readonly IUserCommunicationService _communicationService;


        public ItemController(ILogger<ItemController> logger, UserManager<ApplicationUser> userManager, IUserCommunicationService communicationService, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _cogShareContext = cogShareContext;
            _communicationService = communicationService;
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
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var myRequests = _cogShareContext.Requests.Include(x => x.Requestee).Include(y => y.Requestor).Include(z => z.RequestedItem).Where(request => request.Requestee.Id == user.Id || request.Requestor.Id == user.Id).ToList();
            var myRequestViewModel = new RequestViewModel(user.Id, myRequests);
            return View("Requests", myRequestViewModel);
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
            // Get item, and owner of item, then current user
            var item = _cogShareContext.Items.Include(x => x.Owner).Where(y => y.Id == itemId).SingleOrDefault();
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (item == null)
            {
                var requests = _cogShareContext.Requests.Include(x => x.Requestee).Include(y => y.Requestor).Include(z => z.RequestedItem).Where(request => request.Requestee.Id == user.Id || request.Requestor.Id == user.Id).ToList();
                var requestViewModel = new RequestViewModel(user.Id, requests, true, "The item could not be found");
                return View("Requests", requestViewModel);
            }

            var itemRequest = new Request()
            {
                RequestedItem = item,
                Requestor = user,
                Requestee = item.Owner,
                RequestMessage = $"{user.Email} has requested to borrow {item.DisplayName}"
            };

            _cogShareContext.Requests.Add(itemRequest);
            _cogShareContext.SaveChanges();

            _communicationService.SendItemRequest(itemRequest);

            var myRequests = _cogShareContext.Requests.Include(x => x.Requestee).Include(y => y.Requestor).Include(z => z.RequestedItem).Where(request => request.Requestee.Id == user.Id || request.Requestor.Id == user.Id).ToList();
            var myRequestViewModel = new RequestViewModel(user.Id, myRequests, true, "The item could not be found");
            return View("Requests", myRequestViewModel);
        }
    }
}
