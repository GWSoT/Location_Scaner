﻿using AutoMapper;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.UoW.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eleks_2018_MicroSocialMedia.Extensions;

namespace Eleks_2018_MicroSocialMedia.Services
{
    public class ProfileService
        : IProfileService
    {
        private readonly IProfileRepository _profileRepo;
        private readonly IFriendRepository _friendRepo;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IGeoRepository _geoRepo;
        private readonly ILogger<ProfileService> _logger;
        private readonly INotificationRepository _notificationRepo;
        private readonly IMeetingRepository _meetingRepo;
        private readonly ILastGeoRepository _lastGeoRepo;
        private readonly IPostRepository _postRepo;
        private readonly INotificationService _notificationService;
        private readonly IGeolocationHistoryRepository _geolocationHistoryRepo;

        public ProfileService(
            IPostRepository postRepo,
            IProfileRepository profileRepo, 
            IUserRepository userRepo, 
            UserManager<AppUser> userManager, 
            IMapper mapper, 
            IUnitOfWork uow,
            IFriendRepository friendRepo,
            IGeoRepository geoRepo,
            INotificationRepository notificationRepo,
            IMeetingRepository meetingRepo,
            ILastGeoRepository lastGeoRepo,
            IGeolocationHistoryRepository geolocationHistoryRepo,
            INotificationService notificationService,
            ILogger<ProfileService> logger)
        {
            _geolocationHistoryRepo = geolocationHistoryRepo;
            _notificationService = notificationService;
            _postRepo = postRepo;
            _profileRepo = profileRepo;
            _userRepo = userRepo;
            _userManager = userManager;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _friendRepo = friendRepo;
            _geoRepo = geoRepo;
            _meetingRepo = meetingRepo;
            _lastGeoRepo = lastGeoRepo;
            _notificationRepo = notificationRepo;
        }

        public async Task<ICollection<GeolocationHistoryDto>> GetGeolocationsAsync(ClaimsPrincipal claims, DateTime date, DateTime hour)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadUserProfile(user);

            var historyRecords = _geolocationHistoryRepo.GetRecordByGivenDateAndHour(date, hour, user.Profile);
            _logger.LogCritical($"History Records: {historyRecords.Count()}");
            return _mapper.Map<List<GeolocationHistoryDto>>(historyRecords);
        }

        public async Task<ProfileDto> QueryUserOrDefault(string userId, ClaimsPrincipal userClaims)
        {
            _logger.LogCritical($"{userId}");
            var requestUser = await _userManager.GetUserAsync(userClaims);

            if (requestUser == null)
            {
                _logger.LogCritical("Request User is null");
                return null;
            }

            _profileRepo.LoadUserProfileWithFriends(requestUser);

            var userProfile = _profileRepo.GetProfileByExternalId(userId);

            if (userProfile == null)
            {
                _logger.LogCritical("profile is null");
                return null;
            }

            _profileRepo.LoadUserFromProfile(userProfile);

            _profileRepo.LoadProfileWithGeo(userProfile.User);

            var isMyProfile = requestUser.Profile == userProfile;
            Friend friend = null;

            if (!isMyProfile)
            {
                friend = userProfile
                    .Friends
                    .SingleOrDefault(
                    x => 
                        x.RequestedBy == requestUser.Profile || x.RequestedTo == requestUser.Profile);
            }

            

            var profile = _mapper.Map<ProfileDto>(userProfile, opts => {
                opts.Items["IsMyProfile"] = isMyProfile;
                opts.Items["IsFriend"] = isMyProfile
                                            ? true 
                                            : friend == null 
                                                ? false 
                                                : friend.Approved;
            });



            return profile;
        }

        public async Task<bool> UpdateGeodata(ClaimsPrincipal userClaims, GeolocationDto geolocation)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            if (user == null)
            {
                return false;
            }

            _profileRepo.LoadProfileWithGeo(user);


            user.Profile.LastGeolocation.Latitude = user.Profile.Geolocation.Latitude;
            user.Profile.LastGeolocation.Longitude = user.Profile.Geolocation.Longitude;

            user.Profile.Geolocation.Latitude = geolocation.Latitude;
            user.Profile.Geolocation.Longitude = geolocation.Longitude;

            _geoRepo.Update(user.Profile.Geolocation);
            _lastGeoRepo.Update(user.Profile.LastGeolocation);

            _geolocationHistoryRepo.AddGeolocationHistoryRecord(user.Profile, geolocation);

            _logger.LogCritical($"Before commit: {geolocation.Longitude} : {geolocation.Latitude}");
            if (!_uow.Commit())
            { 
                return false;
            }

            ApplyGeolocationToAllFriends(user);
            return true;
        }

        public async Task<ICollection<FriendDto>> GetMyFriends(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadUserProfile(user);

            if (user.Profile == null)
            {
                return null;
            }

            _friendRepo.LoadFriends(user);

            return _mapper.Map <List<FriendDto>>(user.Profile.Friends, opts => opts.Items["Profile"] = user.Profile);
        }

        public async Task<ICollection<FriendDto>> GetMySentFriendRequests(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadUserProfile(user);

            if (user.Profile == null)
            {
                return null;
            }

            _friendRepo.LoadFriendRequests(user);

            return _mapper.Map<List<FriendDto>>(user.Profile.SentFriendRequests, opts => opts.Items["Profile"] = user.Profile);
        }

        public async Task<ICollection<FriendDto>> GetMyReceievedFriends(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadUserProfile(user);

            _friendRepo.LoadIncomingRequests(user);

            return _mapper.Map<List<FriendDto>>(user.Profile.ReceievedFriendRequests, opts => opts.Items["Profile"] = user.Profile);
        }

        public async Task<bool> AcceptFriend(int requestId, string userId, ClaimsPrincipal userClaims)
        {
            var requestUser = await _userManager.GetUserAsync(userClaims);

            if (requestUser == null)
            {
                return false;
            }

            _friendRepo.LoadFriends(requestUser);

            var userProfile = _profileRepo.GetProfileByExternalId(userId);

            _profileRepo.LoadUserFromProfile(userProfile);

            if (userProfile.User == null)
            {
                return false;
            }

            _friendRepo.LoadFriends(userProfile.User);

            var friend = _friendRepo.GetByRequestIdOrDefault(requestId, requestUser, userProfile.User);

            if (friend == null)
            {
                return false;
            }

            friend.FriendFlag = FriendFlag.Approved;

            if (!_uow.Commit())
            {
                return false;
            }

            _profileRepo.LoadProfileWithDevices(requestUser);
            _profileRepo.LoadProfileWithDevices(userProfile.User);
            ApplyGeolocationToFriend(requestUser, userProfile.User);

            return true;
        }

        public async Task<PostDto> AddPost(string postContent, ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return null;
            }

            _postRepo.LoadUserPosts(user);

            var post = new Post
            {
                Likes = new List<Like>(),
                PostBody = postContent,
                Profile = user.Profile,
                PostDate = DateTime.Now,
            };

            user.Profile.Posts.Add(post);

            if (!_uow.Commit())
            {
                return null;
            }

            return _mapper.Map<PostDto>(post);
        }

        public async Task<bool> AddDevice(string deviceId, ClaimsPrincipal userClaims)
        {
            var requestUser = await _userManager.GetUserAsync(userClaims);

            if (requestUser == null)
            {
                return false;
            }

            _profileRepo.LoadProfileWithDevices(requestUser);

            var existingDeviceId = requestUser.Profile.Devices.FirstOrDefault(x => x.DeviceId == deviceId);

            if (existingDeviceId != null)
            {
                return true;
            }

            requestUser.Profile.Devices.Add(new Device { DeviceId = deviceId, Profile = requestUser.Profile });

            if (!_uow.Commit())
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddFriend(string userId, ClaimsPrincipal userClaims)
        {
            var requestUser = await _userManager.GetUserAsync(userClaims);

            if (requestUser == null)
            {
                return false;
            }

            _profileRepo.LoadUserProfileWithFriendsAndDevices(requestUser);

            var userProfile = _profileRepo.GetProfileByExternalId(userId);

            if (userProfile == null)
            {
                return false;
            }

            _profileRepo.LoadUserFromProfile(userProfile);

            if (userProfile.User == null)
            {
                return false;
            }

            _profileRepo.LoadUserProfileWithFriendsAndDevices(userProfile.User);

            var existingReqeuest = ExistingRequest(userProfile.User, requestUser);

            if (existingReqeuest != null)
            {
                _friendRepo.AcceptFriendRequest(existingReqeuest);
            }
            else
            {
                _friendRepo.AddFriendRequest(requestUser, userProfile.User);
            }

            if (!_uow.Commit())
            {
                return false;
            }

            if (existingReqeuest != null)
            {
                ApplyGeolocationToFriend(requestUser, userProfile.User);
            }

            return true;
        }

        private Friend ExistingRequest(AppUser user, AppUser requestUser)
        {
            var friendRequest = _friendRepo.GetFriendRequestOrDefault(user, requestUser);

            if (friendRequest == null)
            {
                return null;
            }
            return friendRequest;
        }

        public async Task DeleteDeviceId(ClaimsPrincipal userClaims, string deviceId)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            if (user == null)
            {
                return;
            }

            _profileRepo.LoadProfileWithDevices(user);
            var device = user.Profile.Devices.FirstOrDefault(x => x.DeviceId == deviceId);

            if (device == null)
            {
                return;
            }

            user.Profile.Devices.Remove(device);
        }

        public async Task<ProfileDto> QueryUserOrDefault(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadProfileWithGeo(user);

            return _mapper.Map<ProfileDto>(user.Profile, opts =>
            {
                opts.Items["IsMyProfile"] = true;
                opts.Items["IsFriend"] = true;
            });
        }

        public async Task<IQueryable<GeomarkerDto>> GetFriendGeomarkers(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadUserProfileWithFriends(user);

            _friendRepo.LoadFriends(user);

            return user.Profile.Friends
                        .AsQueryable()
                        .ProjectTo<GeomarkerDto>(new { profile = user.Profile });
        }

        // Todo refactor and reimplement logic of this function
        private void ApplyGeolocationToAllFriends(AppUser user)
        {
            _profileRepo.LoadUserProfileWithFriendsAndDevices(user);

            _friendRepo.LoadFriendsWithGeolocationAndDevices(user);

            var friends = user.Profile.Friends
                .Where(p => p.IsInRange && p.CanSendNotification);

            var friendsInMeetingRange = user.Profile.Friends
                .Where(p => p.CanAddAsMeeting);

            _logger.LogCritical($"{user.Profile.FirstName} {user.Profile.LastName} Friends count: {friends.Count()} Devices: {user.Profile.Devices.Count()}");
            foreach (var friend in friends)
            {
                _logger.LogCritical($"{user.Profile.FirstName} {user.Profile.LastName} Notifying user");
                friend.LastNotification = DateTime.Now;
                AddNotification(friend);
                _notificationService.NotifyUserUsingWebPush(friend);
            }

            if (friendsInMeetingRange.Count() > 0)
            {
                _logger.LogCritical($"Friends in meet range {friendsInMeetingRange.Count()} | {user.Profile.Geolocation.Longitude}");

                var friendProfiles = _friendRepo.GetFriendProfilesBySpecifiedRequestedProfile(user.Profile, user.Profile.Friends);

                var meeting = new Meeting
                {
                    Latitude = user.Profile.Geolocation.Latitude,
                    Longitude = user.Profile.Geolocation.Longitude,
                    MeetingTime = DateTime.Now,
                    Profile = user.Profile,
                    Friends = new List<MeetingProfile>(),
                };

                meeting.AddMeetingFriends(friendProfiles);

                user.Profile.Meetings.Add(new MeetingProfile
                {
                    Profile = user.Profile,
                    Meeting = meeting,
                });
            }

            if (_uow.Commit())
            {
                _logger.LogCritical($"Commit? = {friends.Count()}");
            }
        }

        private void ApplyGeolocationToFriend(AppUser user, AppUser friend)
        {
            var friendRequest = _friendRepo.GetFriendRequestOrDefault(user, friend);

            if (friendRequest == null)
            {
                friendRequest = _friendRepo.GetFriendRequestOrDefault(friend, user);

                if (friendRequest == null)
                {
                    return;
                }
            }



            friendRequest.LastNotification = DateTime.Now;
            AddNotification(friendRequest);
            _uow.Commit();
        }

        public async Task<bool> LikePost(string postId, ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return false;
            }

            _profileRepo.LoadUserProfile(user);

            var post = _postRepo.Find(Guid.Parse(postId));
            
            if (post == null)
            {
                return false;
            }

            _postRepo.AddLike(user, post);

            if (!_uow.Commit())
            {
                return false;
            }

            return true;
        }

        private void AddNotification(Friend friendRequest)
        {
            _friendRepo.LoadFromFriendRequest(friendRequest);
            var firstNotification = new Notification
            {
                NotificationText = $"{friendRequest.RequestedTo.FirstName + " " + friendRequest.RequestedTo.LastName} is near you",
                Profile = friendRequest.RequestedBy,
                DateTime = DateTime.Now,
            };

            var secondNotification = new Notification
            {
                NotificationText = $"{friendRequest.RequestedBy.FirstName + " " + friendRequest.RequestedBy.LastName} is near you",
                Profile = friendRequest.RequestedTo,
                DateTime = DateTime.Now,
            };

            _logger.LogCritical($"Distance between two users: {friendRequest.RequestedBy.DistanceBetweenGeolocations(friendRequest.RequestedTo)}");

            friendRequest.RequestedBy.Notifications.Add(firstNotification);
            friendRequest.RequestedTo.Notifications.Add(secondNotification);



            _friendRepo.Update(friendRequest);
            _profileRepo.Update(friendRequest.RequestedBy);
            _profileRepo.Update(friendRequest.RequestedTo);
        }

        public async Task<ICollection<MeetingDto>> GetMeetingsAsync(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return null;
            }

            await this.GetMyFriends(claims);

            _meetingRepo.LoadUserProfileWithMeetings(user);

            var meetings = user.Profile.Meetings.Select(p => p.Meeting);

            var meetingsDto = _mapper.Map<List<MeetingDto>>(meetings, opts => opts.Items["Profile"] = user.Profile);
            return meetingsDto;
        }

        public async Task<ICollection<PostDto>> GetUserPosts(string userId, ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);
            
            if (user == null)
            {
                return null;
            }

            _postRepo.LoadUserPosts(user);

            var profile = _profileRepo.GetProfileByExternalId(userId);

            if (profile == null)
            {
                return null;
            }

            _profileRepo.LoadUserFromProfile(profile);

            _postRepo.LoadUserPosts(profile.User);

            return _mapper.Map<List<PostDto>>(profile.Posts, opts => opts.Items["Profile"] = user.Profile);
        }
    }
}
