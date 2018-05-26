using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.AuthHelpers.interfaces
{
    public interface IJwtFactory
    {
        Task<object> GenerateEncodedToken(string userName, IdentityUser user);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
