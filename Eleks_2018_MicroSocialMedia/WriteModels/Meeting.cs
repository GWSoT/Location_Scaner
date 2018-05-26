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

        public int? MeetingLocationId { get; set; }
        public Geolocation MeetingLocation { get; set; }

        public virtual ICollection<Friend> Friends { get; set; }

        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
