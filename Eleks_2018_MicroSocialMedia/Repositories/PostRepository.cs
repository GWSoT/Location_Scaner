using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class PostRepository
        : BaseRepository<Post, Guid>,
          IPostRepository
    {
        public PostRepository(MSMContext context) 
            : base(context)
        { }

        public void AddLike(AppUser user, Post post)
        {
            DbSet.Where(p => p == post)
                .Include(p => p.Likes)
                    .ThenInclude(p => p.Profile)
                .First()
                .AddOrRemoveLike(user.Profile);
        }

        public void LoadUserPosts(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.Posts)
                    .ThenInclude(p => p.Likes)
                        .ThenInclude(p => p.Profile)
                .Include(p => p.Posts)
                    .ThenInclude(p => p.Profile)
                .Load();
        }
    }
}
