using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories.Interfaces
{
    public interface IMeetingRepository
        : IRepository<Meeting, int>
    {
        void LoadUserProfileWithMeetings(AppUser user);
    }
}
