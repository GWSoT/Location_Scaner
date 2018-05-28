using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class MeetingProfile
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
    }
}
