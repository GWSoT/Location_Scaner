using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.ReadModels
{
    public class MessageDto
    {
        public bool IsMyMessage { get; set; }
        public ProfileDto MessageFrom { get; set; }
        public DateTime MessageTime { get; set; }
        public string MessageBody { get; set; }
        public Guid MessageGroupId { get; set; }
    }
}
