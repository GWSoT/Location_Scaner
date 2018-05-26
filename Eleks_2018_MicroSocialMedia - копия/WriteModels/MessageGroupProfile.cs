using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class MessageGroupProfile
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public Guid MessageGroupId { get; set; }
        public MessageGroup MessageGroup { get; set; }
    }
}
