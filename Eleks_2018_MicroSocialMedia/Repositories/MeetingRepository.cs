using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class MeetingRepository
        : BaseRepository<Meeting, int>,
          IMeetingRepository
    {
        public MeetingRepository(MSMContext context)
            : base(context)
        { }

        public void LoadUserProfileWithMeetings(AppUser user)
        {
            _dbContext.Entry(user)
                .Reference(p => p.Profile)
                .Query()
                .Include(p => p.Meetings)
                    .ThenInclude(p => p.Meeting)
                        .ThenInclude(p => p.Friends)
                .Load();
        }
    }
}
