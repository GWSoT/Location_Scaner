using System.ComponentModel.DataAnnotations.Schema;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Device
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }

        public int? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }
    }
}