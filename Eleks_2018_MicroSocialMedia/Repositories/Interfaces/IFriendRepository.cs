using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories.Interfaces
{
    public interface IFriendRepository
    {
        Friend GetByRequestIdOrDefault(int id, AppUser requestBy, AppUser requestTo);
        Friend GetFriendRequestOrDefault(AppUser requestUser, AppUser receieverUser);
        void LoadIncomingRequests(AppUser requestUser);
        void LoadFriendRequests(AppUser requestUser);
        void LoadFriends(AppUser requestUser);
        void LoadFriendsWithGeolocationAndDevices(AppUser requestUser);
        void AcceptFriendRequest(Friend friendRequest);
        void AddFriendRequest(AppUser requestUser, AppUser receieveUser);
        void LoadFromFriendRequest(Friend friendsRequest);
        void Update(Friend friendRequest);
        IEnumerable<Profile> GetFriendProfilesBySpecifiedRequestedProfile(Profile profile, IEnumerable<Friend> friends);
    }
}
