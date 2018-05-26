using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Post
    {
        public Guid Id { get; set; }
        public string PostBody { get; set; }
        public DateTime PostDate { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public int? ProfileId { get; set; }
        public Profile Profile { get; set; }


        [NotMapped]
        public int LikesCount => Likes.Count;

        public bool IsLikedByMe(Profile profile) => (Likes.SingleOrDefault(p => p.Profile == profile) != null);

        public void AddOrRemoveLike(Profile profile)
        {
            var existingLike = Likes.SingleOrDefault(x => x.Profile == profile);

            if (existingLike == null)
            {
                Likes.Add(new Like
                {
                    Post = this,
                    Profile = profile,
                });
            }
            else
            {
                Likes.Remove(existingLike);
            }
        }
    }
}
