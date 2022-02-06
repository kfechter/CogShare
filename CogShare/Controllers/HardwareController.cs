using CogShare.Domain.Entities;
using CogShare.EFCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using X.PagedList;

namespace CogShare.Controllers
{
    public class HardwareController : Controller
    {
        private readonly ILogger<HardwareController> _logger;
        private readonly UserManager<CogShareUser> _userManager;
        private readonly CogShareContext _cogShareContext;

        public HardwareController(ILogger<HardwareController> logger, UserManager<CogShareUser> userManager, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _cogShareContext = cogShareContext;
        }

        public async Task<IActionResult> Hardware(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (user == null)
            {
                return Unauthorized();
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TagSortParm = string.IsNullOrEmpty(sortOrder) ? "tag_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "type" ? "type_desc" : "type";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var hardware = await _cogShareContext.Hardware.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                hardware = hardware.Where(s => s.Name.Contains(searchString) || s.Tag.Contains(searchString) || s.Type.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    hardware = hardware.OrderByDescending(s => s.Name).ToList();
                    break;
                case "name":
                    hardware = hardware.OrderBy(s => s.Name).ToList();
                    break;
                case "type_desc":
                    hardware = hardware.OrderByDescending(s => s.Type).ToList();
                    break;
                case "type":
                    hardware = hardware.OrderBy(s => s.Type).ToList();
                    break;
                case "tag_desc":
                    hardware = hardware.OrderByDescending(s => s.Tag).ToList();
                    break;
                default:
                    hardware = hardware.OrderBy(s => s.Tag).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(hardware.ToPagedList(pageNumber, pageSize));

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Hardware hardware)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _cogShareContext.Hardware.Add(hardware);
                    _cogShareContext.SaveChanges();
                    return RedirectToAction("Hardware");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(hardware);
        }
    }
}
