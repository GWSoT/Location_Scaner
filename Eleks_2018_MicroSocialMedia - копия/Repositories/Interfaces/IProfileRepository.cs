using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories.Interfaces
{
    public interface IProfileRepository 
        : IRepository<Profile, int>
    {
        void LoadUserProfile(AppUser user);
        void LoadUserProfileWithFriends(AppUser user);
        Profile GetProfileByExternalId(string id);
        void LoadUserFromProfile(Profile profile);
        void LoadProfileWithGeo(AppUser user);
        void LoadProfileWithDevices(AppUser user);
        void LoadUserProfileWithFriendsAndDevices(AppUser user);
        void UpdateUserConnectionId(AppUser user, string connetionId);

        void UpdateUserStatusOnline(AppUser user);
        void UpdateUserStatusLogout(AppUser user);

        void LoadUserProfileWithMessageGroups(AppUser user);
    }
}
