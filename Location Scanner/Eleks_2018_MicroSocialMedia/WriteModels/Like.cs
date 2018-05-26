using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Like
    {
        public int Id { get; set; }
        public Guid? PostId { get; set; }
        public Post Post { get; set; }
        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
