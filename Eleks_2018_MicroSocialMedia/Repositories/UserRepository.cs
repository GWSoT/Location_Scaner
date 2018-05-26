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
    public class UserRepository 
        : BaseRepository<AppUser, string>, IUserRepository
    {
        public UserRepository(MSMContext context)
            : base(context)
        { }

        public AppUser GetUserByID(string id)
        {
            return Find(id);
        }
    }
}
