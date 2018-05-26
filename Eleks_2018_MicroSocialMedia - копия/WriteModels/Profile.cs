using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Profile
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string SignalRHubContextId { get; set; }
        public UserStatus UserStatus { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }
 
        public int? GeolocationId { get; set; }
        [ForeignKey("GeolocationId")]
        public Geolocation Geolocation { get; set; }

        public int? LastGeolocationId { get; set; }
        [ForeignKey("LastGeolocationId")]
        public LastGeolocation LastGeolocation { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<MessageGroupProfile> MessageGroups { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Friend> SentFriendRequests { get; set; }
        public virtual ICollection<Friend> ReceievedFriendRequests { get; set; }

        public double VisibleRange { get; set; }

        [NotMapped]
        public virtual ICollection<Friend> Friends
        {
            get
            {
                var friends = SentFriendRequests.Where(x => x.Approved).ToList();
                friends.AddRange(ReceievedFriendRequests.Where(x => x.Approved));
                return friends;
            }
        }

        [NotMapped]
        public bool IsOnline => UserStatus == UserStatus.Online && LastActivityDate.AddMinutes(3) > DateTime.Now;

        public AppUser User { get; set; }

        public Profile()
        {
            SentFriendRequests = new List<Friend>();
            ReceievedFriendRequests = new List<Friend>();
        }

    }

    public enum UserStatus
    {
        Offline,
        Online
    }
}