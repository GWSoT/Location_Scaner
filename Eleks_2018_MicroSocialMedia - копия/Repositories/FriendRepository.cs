using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Extensions;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class FriendRepository
        : BaseRepository<Friend, int>,
          IFriendRepository
    {
        public FriendRepository(MSMContext context) 
            : base(context)
        { }
        
        public void LoadFromFriendRequest(Friend friendsRequest)
        {
            _dbContext.Entry(friendsRequest)
                .Reference(p => p.RequestedBy)
                .Load();
            _dbContext.Entry(friendsRequest)
                .Reference(p => p.RequestedTo)
                .Load();
        }

        public void LoadIncomingRequests(AppUser requestUser)
        {
            DbSet.Where(p => p.RequestedTo == requestUser.Profile)
                .Include(p => p.RequestedBy)
                    .ThenInclude(p => p.Notifications)
                .Include(p => p.RequestedBy)
                    .ThenInclude(p => p.Geolocation)
                .Include(p => p.RequestedBy)
                    .ThenInclude(p => p.Devices)
                .Include(p => p.RequestedBy)
                    .ThenInclude(p => p.LastGeolocation)
                .Load();
        }

        public void LoadFriendRequests(AppUser requestUser)
        {
            DbSet.Where(p => p.RequestedBy == requestUser.Profile)
                .Include(p => p.RequestedTo)
                    .ThenInclude(p => p.Notifications)
                .Include(p => p.RequestedTo)
                    .ThenInclude(p => p.Geolocation)
                .Include(p => p.RequestedTo)
                    .ThenInclude(p => p.Devices)
                .Include(p => p.RequestedTo)
                    .ThenInclude(p => p.LastGeolocation)
                .Load();
        }

        public void LoadFriends(AppUser requestUser)
        {
            this.LoadFriendRequests(requestUser);
            this.LoadIncomingRequests(requestUser);
        }

        public void LoadFriendsWithGeolocationAndDevices(AppUser requestUser)
        {
            this.LoadFriends(requestUser);
        }

        public Friend GetFriendRequestOrDefault(AppUser requestUser, AppUser receieverUser)
        {
            if (requestUser.Profile == null || receieverUser.Profile == null)
            {
                return null;
            }

            return DbSet
                     .FirstOrDefault(x => x.RequestedBy == requestUser.Profile || x.RequestedTo == receieverUser.Profile);
        }

        public Friend GetByRequestIdOrDefault(int id, AppUser requestBy, AppUser requestTo)
        {
            var friendRequest =  base.Find(id);

            if (friendRequest == null)
            {
                return null;
            }

            if (IsNullProfile(requestBy, requestTo))
            {
                return null;
            }

            if (IsLegitRequest(requestBy, requestTo, friendRequest))
            {
                return null;
            }

            return friendRequest;
        }

        public void AddFriendRequest(AppUser requestUser, AppUser receieveUser)
        {
            var friendRequest = new Friend()
            {
                RequestedBy = requestUser.Profile,
                RequestedTo = receieveUser.Profile,
                FriendFlag = FriendFlag.None
            };

            requestUser.Profile.SentFriendRequests.Add(friendRequest);
        }

        public void AcceptFriendRequest(Friend friendRequest)
        {
            friendRequest.FriendFlag = FriendFlag.Approved;
        }

        public override void Update(Friend entity)
        {
            base.Update(entity);
        }

        public void NotifyUserWithWebPush(IEnumerable<Friend> friends)
        {
            foreach(var friend in friends)
            {
                friend.NotifyUserUsingWebPush();   
            }
        }

        private Func<AppUser, AppUser, Friend, bool> IsLegitRequest = 
            (AppUser requestBy, AppUser requestTo, Friend friendRequest) => 
                friendRequest.RequestedBy == requestBy.Profile && 
                friendRequest.RequestedTo == requestTo.Profile;

        private Func<AppUser, AppUser, bool> IsNullProfile =
            (AppUser requestBy, AppUser requestTo) =>
                requestBy.Profile == null || requestTo.Profile == null;
    }
}
