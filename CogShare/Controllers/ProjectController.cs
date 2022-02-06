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
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly UserManager<CogShareUser> _userManager;
        private readonly CogShareContext _cogShareContext;

        public ProjectController(ILogger<ProjectController> logger, UserManager<CogShareUser> userManager, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _cogShareContext = cogShareContext;
        }

        public async Task<IActionResult> Project(string sortOrder, string currentFilter, string searchString, int? page)
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

            var externalProjects = await _cogShareContext.ExternalProjects.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                externalProjects = externalProjects.Where(s => s.Name.Contains(searchString) || s.Tag.Contains(searchString) || s.Type.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    externalProjects = externalProjects.OrderByDescending(s => s.Name).ToList();
                    break;
                case "name":
                    externalProjects = externalProjects.OrderBy(s => s.Name).ToList();
                    break;
                case "type_desc":
                    externalProjects = externalProjects.OrderByDescending(s => s.Type).ToList();
                    break;
                case "type":
                    externalProjects = externalProjects.OrderBy(s => s.Type).ToList();
                    break;
                case "tag_desc":
                    externalProjects = externalProjects.OrderByDescending(s => s.Tag).ToList();
                    break;
                default:
                    externalProjects = externalProjects.OrderBy(s => s.Tag).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(externalProjects.ToPagedList(pageNumber, pageSize));

        }
    }
}
