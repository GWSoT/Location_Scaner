using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class ProfileRepository
        : BaseRepository<Profile, int>,
          IProfileRepository
        
    {
        public ProfileRepository(MSMContext dbContext)
            : base(dbContext)
        { }



        public void LoadUserProfileWithFriendsAndDevices(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.ReceievedFriendRequests)
                .Include(p => p.SentFriendRequests)
                .Include(p => p.Devices)
                .Include(p => p.Notifications)
                .Include(p => p.Meetings)
                .Load();
        }

        public void LoadUserProfileWithDevices(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.Devices)
                .Load();
        }

        public void LoadUserProfile(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Load();
        }

        public void LoadUserProfileWithFriends(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.ReceievedFriendRequests)
                .Include(p => p.SentFriendRequests)
                .Load();
        }

        public Profile GetProfileByExternalId(string id)
        {
            return DbSet.SingleOrDefault(x => x.ExternalId == id);
        }

        public void LoadUserFromProfile(Profile profile)
        {
            _dbContext.Entry(profile)
                .Reference(p => p.User)
                .Query()
                .Include(p => p.Profile.Geolocation)
                .Load();
        }

        public void LoadProfileWithGeo(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.Geolocation)
                .Include(p => p.LastGeolocation)
                .Load();
        }

        public void LoadProfileWithDevices(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.Devices)
                .Load();
        }

        public void UpdateUserConnectionId(AppUser user, string connetionId)
        {
            user.Profile.SignalRHubContextId = connetionId;
            user.Profile.LastActivityDate = DateTime.Now;
            user.Profile.UserStatus = UserStatus.Online;
            base.Update(user.Profile);
        }

        public void UpdateUserStatusOnline(AppUser user)
        {
            user.Profile.LastActivityDate = DateTime.Now;
            user.Profile.UserStatus = UserStatus.Online;
            base.Update(user.Profile);
        }

        public void UpdateUserStatusLogout(AppUser user)
        {
            user.Profile.UserStatus = UserStatus.Offline;
            base.Update(user.Profile);
        }

        public void LoadUserProfileWithMessageGroups(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.MessageGroups)
                    .ThenInclude(s => s.MessageGroup)
                        .ThenInclude(p => p.Members)
                .Load();
        }
    }
}
