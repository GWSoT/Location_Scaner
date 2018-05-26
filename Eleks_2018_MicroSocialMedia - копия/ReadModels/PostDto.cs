using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.ReadModels
{
    public class PostDto
    {
        public Guid PostId { get; set; }
        public string PostBody { get; set; }
        public DateTime PostDate { get; set; }
        public int LikesCount { get; set; }
        public ProfileDto PostAuthor { get; set; }
        public bool IsLikedByMe { get; set; }
    }
}
