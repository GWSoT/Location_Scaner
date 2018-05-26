using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    // LastGeolocation is basically entity for checking whether user changed his position or not
    // The main reason for doing this, is to make sure, that my app wouldn't spam through PushNotification
    // To other users if they in same location for a long time 
    // or, for example they live nearby.

    public class LastGeolocation
    {
        public int Id { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }
    }
}
