using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.ReadModels
{
    public class FriendDto
    {
        public string Id { get; set; }
        public int RequestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public FriendFlag FriendFlag { get; set; }
    }
}
