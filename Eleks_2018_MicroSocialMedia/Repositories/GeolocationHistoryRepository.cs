using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class GeolocationHistoryRepository
        : BaseRepository<GeolocationHistory, int>,
          IGeolocationHistoryRepository
    {
        public GeolocationHistoryRepository(MSMContext context)
            : base(context) { }

        public void AddGeolocationHistoryRecord(Profile profile, GeolocationDto geolocation)
        {
            DbSet.Add(this.CreateNewHistoryRecord(profile, geolocation));
        }

        public IEnumerable<GeolocationHistory> GetHistoryByGivenDate(DateTime date, Profile profile)
        {
            return DbSet.Where(p => p.ProfileId == profile.Id &&
                                p.Time.Day == date.Day &&
                                p.Time.Year == date.Year &&
                                p.Time.Month == date.Month)
                        .OrderBy(p => p.Time);
        }

        public IEnumerable<GeolocationHistory> GetRecordByGivenDateAndHour(DateTime date, DateTime hour, Profile profile)
        {
            return this.GetHistoryByGivenDate(date, profile).Where(p => p.Time.Hour == hour.Hour)
                        .ToList()
                        .TakeLast(20);
        }

        private GeolocationHistory CreateNewHistoryRecord(Profile profile, GeolocationDto geolocation)
        {
            var geolocationHistoryRecord = new GeolocationHistory
            {
                Time = DateTime.Now,
                Profile = profile,
                ProfileId = profile.Id,
                Latitude = geolocation.Latitude,
                Longitude = geolocation.Longitude,
            };

            return geolocationHistoryRecord;
        }
    }
}
