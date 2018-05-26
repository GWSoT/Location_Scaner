using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories.Interfaces
{
    public interface IPostRepository
        : IRepository<Post, Guid>
    {
        void LoadUserPosts(AppUser user);
        void AddLike(AppUser user, Post post);
    }
}
