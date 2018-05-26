using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto> QueryUserOrDefault(string userId, ClaimsPrincipal userClaims);
        Task<ProfileDto> QueryUserOrDefault(ClaimsPrincipal userClaims);
        Task<bool> AddFriend(string userId, ClaimsPrincipal userClaims);
        Task<ICollection<FriendDto>> GetMyFriends(ClaimsPrincipal userClaims);
        Task<ICollection<FriendDto>> GetMyReceievedFriends(ClaimsPrincipal userClaims);
        Task<ICollection<FriendDto>> GetMySentFriendRequests(ClaimsPrincipal userClaims);
        Task<bool> AcceptFriend(int requestId, string userId, ClaimsPrincipal userClaims);
        Task<bool> UpdateGeodata(ClaimsPrincipal userClaims, GeolocationDto geolocation);
        Task<bool> AddDevice(string deviceId, ClaimsPrincipal userClaims);
        Task DeleteDeviceId(ClaimsPrincipal userClaims, string deviceId);
        Task<IQueryable<GeomarkerDto>> GetFriendGeomarkers(ClaimsPrincipal userClaims);
        Task<PostDto> AddPost(string postContent, ClaimsPrincipal claims);
        Task<ICollection<PostDto>> GetUserPosts(string userId, ClaimsPrincipal claims);
        Task<bool> LikePost(string postId, ClaimsPrincipal claims);
        Task<ICollection<MeetingDto>> GetMeetingsAsync(ClaimsPrincipal claims);
    }
}
