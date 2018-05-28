using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Meeting
    {
        public int Id { get; set; }

        public DateTime MeetingTime { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public virtual ICollection<MeetingProfile> Friends { get; set; }

        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }

        public void AddMeetingFriend(Profile profile)
        {
            Friends.Add(new MeetingProfile
            {
                Meeting = this,
                Profile = profile,
            });
        }

        public void AddMeetingFriends(IEnumerable<Profile> profiles)
        {
            foreach(var profile in profiles)
            {
                this.AddMeetingFriend(profile);
            }
        }
    }
}
