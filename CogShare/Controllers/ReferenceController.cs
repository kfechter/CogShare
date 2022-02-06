using CogShare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using CogShare.EFCore;
using CogShare.Models;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;

namespace CogShare.Controllers
{
    public class ReferenceController : Controller
    {
        private readonly ILogger<ReferenceController> _logger;
        private readonly UserManager<CogShareUser> _userManager;
        private readonly CogShareContext _cogShareContext;

        public ReferenceController(ILogger<ReferenceController> logger, UserManager<CogShareUser> userManager, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _cogShareContext = cogShareContext;
        }

        public async Task<IActionResult> Reference(string sortOrder, string currentFilter, string searchString, int? page)
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

            var docs = await _cogShareContext.Docs.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                docs = docs.Where(s => s.Name.Contains(searchString) || s.Tag.Contains(searchString) || s.Type.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    docs = docs.OrderByDescending(s => s.Name).ToList();
                    break;
                case "name":
                    docs = docs.OrderBy(s => s.Name).ToList();
                    break;
                case "type_desc":
                    docs = docs.OrderByDescending(s => s.Type).ToList();
                    break;
                case "type":
                    docs = docs.OrderBy(s => s.Type).ToList();
                    break;
                case "tag_desc":
                    docs = docs.OrderByDescending(s => s.Tag).ToList();
                    break;
                default:
                    docs = docs.OrderBy(s => s.Tag).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(docs.ToPagedList(pageNumber, pageSize));

        }
    }
}
