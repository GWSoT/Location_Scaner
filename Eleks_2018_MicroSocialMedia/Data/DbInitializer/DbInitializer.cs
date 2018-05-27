using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Data.DbInitializer
{
    public class DbInitializer
    {
        private static List<GeolocationDto> Geolocations = new List<GeolocationDto>
        {
            NewGeolocation(49.039906, 24.396324),
            NewGeolocation(49.040061, 24.393406),
            NewGeolocation(49.039463, 24.391786),
            NewGeolocation(49.038992, 24.389769),
            NewGeolocation(49.039190, 24.386921),
            NewGeolocation(49.038648, 24.385832),
            NewGeolocation(49.038377, 24.385322),
            NewGeolocation(49.039463, 24.371786),
            NewGeolocation(49.038992, 24.369769),
            NewGeolocation(49.039190, 24.356921),
            NewGeolocation(49.038648, 24.345832),
            NewGeolocation(49.038377, 24.335322),
            NewGeolocation(49.029906, 24.396324),
            NewGeolocation(49.010061, 24.393406),
            NewGeolocation(49.999463, 24.391786),
            NewGeolocation(49.988992, 24.389769),
            NewGeolocation(49.979190, 24.386921),
            NewGeolocation(49.968648, 24.385832),
            NewGeolocation(49.958377, 24.385322),
            NewGeolocation(49.949463, 24.371786),
            NewGeolocation(49.938992, 24.369769),
            NewGeolocation(49.929190, 24.356921),
            NewGeolocation(49.918648, 24.345832),
            NewGeolocation(49.908377, 24.335322),
        };

        public static void Seed(MSMContext context)
        {
            if (!context.GeolocationHistory.Any())
            {
                var user = context.Users.FirstOrDefault(x => x.UserName == "123@123");
                var profile = context.Profiles.FirstOrDefault(x => x.Id == user.ProfileId);

                var idx = 0;
                foreach(var geolocation in Geolocations)
                {
                    context.GeolocationHistory.Add(new GeolocationHistory
                    {
                        Profile = profile,
                        Latitude = geolocation.Latitude,
                        Longitude = geolocation.Longitude,
                        Time = DateTime.Now.AddMinutes(-idx),
                    });
                }

                context.SaveChanges();
            }
        }

        private static GeolocationDto NewGeolocation(double latitude, double longitude)
        {
            return new GeolocationDto { Longitude = longitude, Latitude = latitude };
        }
    }
}
