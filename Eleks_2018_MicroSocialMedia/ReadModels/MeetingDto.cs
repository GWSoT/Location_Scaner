using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.ReadModels
{
    public class MeetingDto
    {
        public DateTime MeetingTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<ProfileDto> Friends { get; set; }
    }
}
