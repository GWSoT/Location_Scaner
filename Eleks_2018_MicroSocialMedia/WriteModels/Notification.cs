using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Notification
    {
        public int Id { get; set; }
        public string NotificationText { get; set; }
        public DateTime DateTime { get; set; }
        public bool Read { get; set; }

        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
