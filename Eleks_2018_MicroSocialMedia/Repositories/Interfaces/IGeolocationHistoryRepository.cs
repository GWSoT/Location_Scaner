using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories.Interfaces
{
    public interface IGeolocationHistoryRepository
        : IRepository<GeolocationHistory, int>
    {
        void AddGeolocationHistoryRecord(Profile profile, GeolocationDto geolocation);

        IEnumerable<GeolocationHistory> GetHistoryByGivenDate(DateTime date, Profile profile);
        IEnumerable<GeolocationHistory> GetRecordByGivenDateAndHour(DateTime date, DateTime hour, Profile profile);
    }
}
