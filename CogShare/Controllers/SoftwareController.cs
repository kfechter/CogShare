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
    public class SoftwareController : Controller
    {
        private readonly ILogger<SoftwareController> _logger;
        private readonly UserManager<CogShareUser> _userManager;
        private readonly CogShareContext _cogShareContext;

        public SoftwareController(ILogger<SoftwareController> logger, UserManager<CogShareUser> userManager, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _cogShareContext = cogShareContext;
        }

        public async Task<IActionResult> Software(string sortOrder, string currentFilter, string searchString, int? page)
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

            var software = await _cogShareContext.Software.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                software = software.Where(s => s.Name.Contains(searchString) || s.Tag.Contains(searchString) || s.Type.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    software = software.OrderByDescending(s => s.Name).ToList();
                    break;
                case "name":
                    software = software.OrderBy(s => s.Name).ToList();
                    break;
                case "type_desc":
                    software = software.OrderByDescending(s => s.Type).ToList();
                    break;
                case "type":
                    software = software.OrderBy(s => s.Type).ToList();
                    break;
                case "tag_desc":
                    software = software.OrderByDescending(s => s.Tag).ToList();
                    break;
                default:
                    software = software.OrderBy(s => s.Tag).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(software.ToPagedList(pageNumber, pageSize));

        }
    }
}
