using CogShare.Domain.Entities;
using CogShare.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using CogShare.EFCore;
using CogShare.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CogShare.Controllers
{
    public class FriendController : Controller
    {
        private readonly ILogger<FriendController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserCommunicationService _communicationService;
        private readonly CogShareContext _cogShareContext;

        public FriendController(ILogger<FriendController> logger, UserManager<ApplicationUser> userManager, IUserCommunicationService communicationService, CogShareContext cogShareContext)
        {
            _logger = logger;
            _userManager = userManager;
            _communicationService = communicationService;
            _cogShareContext = cogShareContext;
        }

        public IActionResult Friends()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result.Id; // get the user ID and do the rest with db context.
            var friendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => (x.User1Id == user || x.User2Id == user));
            var friendViewModel = new FriendViewModel(friendships.ToList());
            return View(friendViewModel);
        }

        [HttpPost]
        public IActionResult CreateFriendRequest(FriendRequestViewModel model)
        {
            var existingFriendShips = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => (x.User1Id == model.User1Id && x.User2Id == model.User2Id) || (x.User1Id == model.User2Id && x.User2Id == model.User1Id));
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (existingFriendShips.Any())
            {
                return View("Friends", new FriendViewModel(true, "An existing or pending request already exists for this combination of users.", _cogShareContext.Friendships.Where(x => x.User1Id == user.Id || x.User2Id == user.Id).ToList()));
            }

            var friendRequestee = _userManager.FindByIdAsync(model.User2Id).Result;

            var friendShip = new Friendship()
            {
                User1 = user,
                User2 = friendRequestee,
                User1Id = user.Id,
                User2Id = friendRequestee.Id,
                Accepted = false
            };

            var emailSent = _communicationService.SendFriendRequest(friendRequestee.Email, user.UserName).IsCompletedSuccessfully;
            if (emailSent)
            {
                _cogShareContext.Friendships.Add(friendShip);
                _cogShareContext.SaveChanges();
                return View("Friends", new FriendViewModel(false, "Request has been created successfully", _cogShareContext.Friendships.Where(x => x.User1Id == user.Id || x.User2Id == user.Id).ToList()));
            }

            var friendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
            var friendViewModel = new FriendViewModel(true, "An error occurred sending the request", friendships.ToList());
            return View("Friends", friendViewModel);
        }

        [HttpPost("{id}")]
        public IActionResult AcceptRequest(int id, string userId)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            try
            {
                var request = _cogShareContext.Friendships.Where(x => x.Id == id).SingleOrDefault();
                if(request == null)
                {
                    var unmodifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                    var friendViewErrorModel = new FriendViewModel(true, "The request you tried to approve has been deleted", unmodifiedFriendships.ToList());
                    return View("Friends", friendViewErrorModel);
                }

                request.Accepted = true;
                _cogShareContext.Friendships.Update(request);
                _cogShareContext.SaveChanges();

                var modifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewCompletedModel = new FriendViewModel(false, "The friend request has been accepted", modifiedFriendships.ToList());
                _communicationService.AcceptFriendRequest(request);
                return View(friendViewCompletedModel);
            }
            catch (Exception ex)
            {
                var existingFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewErrorModel = new FriendViewModel(true, ex.Message, existingFriendships.ToList());
                return View("Friends", friendViewErrorModel);
            }
        }

        public IActionResult DenyRequest()
        {
            return View("Friends" /*, friendModel*/);
        }
    }
}
