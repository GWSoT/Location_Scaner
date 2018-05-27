using Eleks_2018_MicroSocialMedia.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class GeolocationHistory
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
