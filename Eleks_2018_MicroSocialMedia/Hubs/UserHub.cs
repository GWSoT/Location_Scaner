using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.UoW.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Hubs
{   
    public class UserHub : Hub
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserHub> _logger;
        private readonly IProfileService _profileService;
        public UserHub(
            IProfileRepository profileRepository,
            IUnitOfWork uow,
            UserManager<AppUser> userManager,
            IProfileService profileService,
            ILogger<UserHub> logger)
        {
            _profileRepository = profileRepository;
            _uow = uow;
            _userManager = userManager;
            _logger = logger;
            _profileService = profileService;
        }

        public async override Task OnConnectedAsync()
        {
            var user = await GetUser(Context.User);

            if (user == null)
            {
                _logger.LogCritical($"auth_token: {Context.Connection.GetHttpContext().Request.Query["auth_token"]}");
                _logger.LogCritical("User is null");
                return;
            }

            if (!IsContextIdExists(user, Context.ConnectionId))
            {
                _profileRepository.UpdateUserConnectionId(user, Context.ConnectionId);
                _profileRepository.UpdateUserStatusOnline(user);
                _uow.Commit();
            }


            await base.OnConnectedAsync();
        }

        public async Task UpdateGeodata(GeolocationDto geolocation)
        {
            await _profileService.UpdateGeodata(Context.User, geolocation);
        }

        public async Task OnUserLogout()
        {
            var user = await GetUser(Context.User);

            if (user == null)
            {
                return;
            }

            _profileRepository.LoadUserProfile(user);

            _profileRepository.UpdateUserStatusLogout(user);

            _uow.Commit();
        }

        public async Task OnUserUpdateStatus()
        {
            var user = await GetUser(Context.User);

            if (user == null)
            {
                return;
            }

            _profileRepository.LoadUserProfile(user);

            _profileRepository.UpdateUserStatusOnline(user);

            _uow.Commit();
        }




        private async Task<AppUser> GetUser(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return null;
            }

            _profileRepository.LoadUserProfile(user);

            return user;
        }


        private Func<AppUser, string, bool> IsContextIdExists =
            (AppUser user, string contextId) => user.Profile.SignalRHubContextId == contextId;
    }
}
