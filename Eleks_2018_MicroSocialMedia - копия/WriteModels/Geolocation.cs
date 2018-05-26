using System.ComponentModel.DataAnnotations.Schema;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Geolocation
    {
        public int Id { get; set; }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }
    }
}