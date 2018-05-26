using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.ReadModels
{
    public class ProfileDto
    {
        public bool IsMyProfile { get; set; }
        public bool IsFriend { get; set; }
        //public bool FriendshipRequested { get; set; }
    
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GeolocationDto Geolocation { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsOnline { get; set; }
    }
}
