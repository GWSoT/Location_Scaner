using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IMessageService _messageService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            IProfileService profileService,
            ILogger<ProfileController> logger,
            UserManager<AppUser> userManager,
            IMessageService messageService)
        {
            _userManager = userManager;
            _profileService = profileService;
            _logger = logger;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> LikePost([FromQuery] string postId)
        {
            var success = await _profileService.LikePost(postId, User);

            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] string userId)
        {
            var posts = await _profileService.GetUserPosts(userId, User);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] PostViewModel postBody)
        {
            var post = await _profileService.AddPost(postBody.PostBody, User);

            if (post == null)
            {
                return BadRequest();
            }

            return Ok(post);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Profile(string id)
        {
            _logger.LogCritical($"prof_id: {id}");
            if (string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("null_id", "id string can't be empty.");
                return BadRequest(ModelState);
            }

            var user = await _profileService.QueryUserOrDefault(id, User);

            if (user == null)
            {
                ModelState.AddModelError("null_profile", "can't find user profile with specified id.");
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConversation([FromQuery] string conversationId)
        {
            await _messageService.DeleteConversation(conversationId, User);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeviceId([FromBody] DeviceDto model)
        {
            var result = await _profileService.AddDevice(model.DeviceId, User);

            if (result == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromQuery]string conversationName)
        {
            await _messageService.CreateConversation(User);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> InviteUserToConversation([FromQuery]string userId, [FromQuery]string groupId)
        {
            await _messageService.InviteUserToConversation(groupId, User, userId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            var meetings = await _profileService.GetMeetingsAsync(User);
            return Ok(meetings);
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations()
        {
            var messageGroups = await _messageService.GetMessageGroups(User);
            return Ok(messageGroups);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] string conversationId)
        {
            var messages = await _messageService.GetMessages(User, conversationId);
            return Ok(messages);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOldDeviceId([FromQuery]string deviceId)
        {
            await _profileService.DeleteDeviceId(User, deviceId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Friends()
        {
            /// TODO: Get friends list of other user
            var friends = await _profileService.GetMyFriends(User);

            if (friends == null)
            {
                return BadRequest();
            }

            return Ok(friends.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> SentFriendRequest()
        {
            /// TODO: Get friends list of other user
            var friends = await _profileService.GetMySentFriendRequests(User);

            if (friends == null)
            {
                return BadRequest();
            }

            return Ok(friends.Where(p => !(p.FriendFlag == FriendFlag.Approved)).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> FriendGeolocation()
        {
            var friendMarkers = await _profileService.GetFriendGeomarkers(User);

            return Ok(friendMarkers.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> ReceievedFriends()
        {
            /// TODO: Get friends list of other user
            var friends = await _profileService.GetMyReceievedFriends(User);

            if (friends == null)
            {
                return BadRequest();
            }

            return Ok(friends.Where(p => !(p.FriendFlag == FriendFlag.Approved)).ToList());
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> AcceptFriend(string id,[FromQuery] int friendRequestId)
        {
            if (string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("null_id", "id string can't be empty.");
                return BadRequest(ModelState);
            }

            var user = await _profileService.QueryUserOrDefault(id, User);

            if (user == null)
            {
                ModelState.AddModelError("null_profile", "can't find user profile with specified id.");
                return BadRequest(ModelState);
            }

            var accept = await _profileService.AcceptFriend(friendRequestId, id, User);

            if (accept == false)
            {
                ModelState.AddModelError("freind_request_id", "can't accept friend request that was not made by other user");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> AddFriend(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("null_id", "id string can't be empty.");
                return BadRequest(ModelState);
            }

            var user = await _profileService.QueryUserOrDefault(id, User);

            if (user == null)
            {
                ModelState.AddModelError("null_profile", "can't find user profile with specified id.");
                return BadRequest(ModelState);
            }

            var result = await _profileService.AddFriend(id, User);

            if (result == false)
            {
                ModelState.AddModelError("null_profile", "can't find user profile with specified id.");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CurrentUser()
        {
            _logger.LogCritical($"{Request.Headers["Authorization"]}");
            var user = await _profileService.QueryUserOrDefault(User);
            
            if (user == null)
            {
                _logger.LogCritical("Critical user is null");
                ModelState.AddModelError("null_user", "Null user");
                return BadRequest(ModelState);
            }

            _logger.LogCritical("Ok()");

            return Ok(user);
        }
    }
}
