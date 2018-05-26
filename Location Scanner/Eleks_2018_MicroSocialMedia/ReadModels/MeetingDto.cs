using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.ReadModels
{
    public class MeetingDto
    {
        public DateTime MeetingTime { get; set; }
        public GeolocationDto MeetingLocation { get; set; }
        public ICollection<FriendDto> Friends { get; set; }
    }
}
