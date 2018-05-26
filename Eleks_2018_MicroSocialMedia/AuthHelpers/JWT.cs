using Eleks_2018_MicroSocialMedia.AuthHelpers;
using Eleks_2018_MicroSocialMedia.AuthHelpers.interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Helpers
{
    public class JWT
    {
        public static async Task<string> GenerateJwt(AppUser user, IJwtFactory jwtFactory, string userName, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                authToken = await jwtFactory.GenerateEncodedToken(userName, user),
                expiresIn = (int)120
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }

    }
}
