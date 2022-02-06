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
    public class PersonalProjectController : Controller
    {
        private readonly ILogger<PersonalProjectController> _logger;
        private readonly UserManager<CogShareUser> _userManager;
        private readonly CogShareContext _cogShareContext;

        public PersonalProjectController(ILogger<PersonalProjectController> logger, UserManager<CogShareUser> userManager, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _cogShareContext = cogShareContext;
        }

        public async Task<IActionResult> PersonalProject(string sortOrder, string currentFilter, string searchString, int? page)
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
            ViewBag.OwnerSortParm = sortOrder == "owner" ? "owner_desc" : "owner";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var personalProjects = await _cogShareContext.PersonalProjects.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                personalProjects = personalProjects.Where(s => s.ProjectName.Contains(searchString) || s.Tag.Contains(searchString) || s.Type.Contains(searchString) || s.Owner.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    personalProjects = personalProjects.OrderByDescending(s => s.ProjectName).ToList();
                    break;
                case "name":
                    personalProjects = personalProjects.OrderBy(s => s.ProjectName).ToList();
                    break;
                case "type_desc":
                    personalProjects = personalProjects.OrderByDescending(s => s.Type).ToList();
                    break;
                case "type":
                    personalProjects = personalProjects.OrderBy(s => s.Type).ToList();
                    break;
                case "owner_desc":
                    personalProjects = personalProjects.OrderByDescending(s => s.Owner).ToList();
                    break;
                case "owner":
                    personalProjects = personalProjects.OrderBy(s => s.Owner).ToList();
                    break;
                case "tag_desc":
                    personalProjects = personalProjects.OrderByDescending(s => s.Tag).ToList();
                    break;
                default:
                    personalProjects = personalProjects.OrderBy(s => s.Tag).ToList();
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(personalProjects.ToPagedList(pageNumber, pageSize));

        }
    }
}
