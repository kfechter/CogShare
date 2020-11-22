using CogShare.Domain.Entities;
using CogShare.Services;
using CogShare.Utilities;
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
            var friendViewModel = new FriendViewModel(friendships.ToList(), user);
            return View(friendViewModel);
        }

        public IActionResult FriendRequests()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result.Id; // get the user ID and do the rest with db context.
            var friendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => (x.User1Id == user || x.User2Id == user));
            var friendRequestViewModel = new FriendRequestViewModel(friendships.ToList(), user);
            return View(friendRequestViewModel);
        }

        public IActionResult UserSearch()
        {
            return View();
        }

        public IActionResult SearchUser(string searchQuery)
        {
            if(searchQuery.IsNullWhiteSpaceOrEmpty())
            {
                var userSearchModel = new UserSearchViewModel(true, "No search query was provided");
                return PartialView("_UserSearchPartial", userSearchModel);
            }

            var user = _userManager.GetUserAsync(HttpContext.User).Result.Id; // get the user ID and do the rest with db context.
            var users = _cogShareContext.Users.Where(x => (x.Email.Contains(searchQuery) || x.UserName.Contains(searchQuery)) && x.Id != user).ToList();
            var usersFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user || x.User2Id == user);
            var newUsers = users.Where(x => !usersFriendships.Any(y => y.User1Id == x.Id || y.User2Id == x.Id)).ToList();

            if (newUsers.Count == 0)
            {
                var model = new UserSearchViewModel(false, $"No users found matching search query: {searchQuery}");
                return PartialView("_UserSearchPartial", model);
            }

            var userModel = new UserSearchViewModel(newUsers);
            return PartialView("_UserSearchPartial", userModel);
        }

        public IActionResult AddFriend(string id)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            var existingFriendShips = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => (x.User1Id == user.Id || x.User2Id == user.Id) && (x.User1.Id == id || x.User2.Id == id));

            if (existingFriendShips.Any())
            {
                return View("Friends", new FriendViewModel(true, "An existing or pending request already exists for this combination of users.", _cogShareContext.Friendships.Where(x => x.User1Id == user.Id || x.User2Id == user.Id).ToList(), user.Id));
            }

            // search by username or email
            var friend = _userManager.FindByIdAsync(id).Result;

            if(friend == null)
            {
                return View("Friends", new FriendViewModel(true, "The user could not be found.", _cogShareContext.Friendships.Where(x => x.User1Id == user.Id || x.User2Id == user.Id).ToList(), user.Id));
            }

            var friendShip = new Friendship()
            {
                User1 = user,
                User2 = friend,
                User1Id = user.Id,
                User2Id = friend.Id,
                Accepted = false
            };

            _communicationService.SendFriendRequest(friend.Email, user.UserName);
            _cogShareContext.Friendships.Add(friendShip);
            _cogShareContext.SaveChanges();
            return View("Friends", new FriendViewModel(false, "Request has been created successfully", _cogShareContext.Friendships.Where(x => x.User1Id == user.Id || x.User2Id == user.Id).ToList(), user.Id));
        }

        public IActionResult AcceptRequest(int id)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            try
            {
                var request = _cogShareContext.Friendships.Where(x => x.Id == id).SingleOrDefault();
                if(request == null)
                {
                    var unmodifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                    var friendViewErrorModel = new FriendRequestViewModel(true, "The request you tried to approve has been deleted", unmodifiedFriendships.ToList(), user.Id);
                    return View("FriendRequests", friendViewErrorModel);
                }

                request.Accepted = true;
                _cogShareContext.Friendships.Update(request);
                _cogShareContext.SaveChanges();

                var modifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewCompletedModel = new FriendRequestViewModel(false, "The friend request has been accepted", modifiedFriendships.ToList(), user.Id);
                _communicationService.AcceptFriendRequest(request);
                return View("FriendRequests", friendViewCompletedModel);
            }
            catch (Exception ex)
            {
                var existingFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewErrorModel = new FriendRequestViewModel(true, ex.Message, existingFriendships.ToList(), user.Id);
                return View("FriendRequests", friendViewErrorModel);
            }
        }

        public IActionResult DeclineRequest(int id, string action)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            try
            {
                var request = _cogShareContext.Friendships.Where(x => x.Id == id).SingleOrDefault();
                if (request == null)
                {
                    var unmodifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                    var friendViewErrorModel = new FriendRequestViewModel(true, $"The request you tried to {action} could not be found", unmodifiedFriendships.ToList(), user.Id);
                    return View("FriendRequests", friendViewErrorModel);
                }

                _cogShareContext.Friendships.Remove(request);
                _cogShareContext.SaveChanges();

                var modifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewCompletedModel = new FriendRequestViewModel(false, $"The friend request has been {action}d", modifiedFriendships.ToList(), user.Id);
                return View("FriendRequests", friendViewCompletedModel);
            }
            catch (Exception ex)
            {
                var existingFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewErrorModel = new FriendRequestViewModel(true, ex.Message, existingFriendships.ToList(), user.Id);
                return View("FriendRequests", friendViewErrorModel);
            }
        }

        public IActionResult DeleteFriend(int id)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            try
            {
                var request = _cogShareContext.Friendships.Where(x => x.Id == id).SingleOrDefault();
                if (request == null)
                {
                    var unmodifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                    var friendViewErrorModel = new FriendViewModel(true, "The friend you tried to delete could not be found", unmodifiedFriendships.ToList(), user.Id);
                    return View("Friends", friendViewErrorModel);
                }

                _cogShareContext.Friendships.Remove(request);
                _cogShareContext.SaveChanges();

                var modifiedFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewCompletedModel = new FriendViewModel(false, "The friend has been deleted", modifiedFriendships.ToList(), user.Id);
                return View("Friends", friendViewCompletedModel);
            }
            catch (Exception ex)
            {
                var existingFriendships = _cogShareContext.Friendships.Include(x => x.User1).Include(x => x.User2).Where(x => x.User1Id == user.Id || x.User2Id == user.Id);
                var friendViewErrorModel = new FriendViewModel(true, ex.Message, existingFriendships.ToList(), user.Id);
                return View("Friends", friendViewErrorModel);
            }
        }

    }
}
