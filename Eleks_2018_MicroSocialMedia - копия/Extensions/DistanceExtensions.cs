using Eleks_2018_MicroSocialMedia.Firebase;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Extensions
{
    public class Coordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
    public static class CoordinatesDistanceExtensions
    {
        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates)
        {
            return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }

        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
    }

    public class UnitOfLength
    {
        public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
        public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
        public static UnitOfLength Miles = new UnitOfLength(1);

        private readonly double _fromMilesFactor;

        private UnitOfLength(double fromMilesFactor)
        {
            _fromMilesFactor = fromMilesFactor;
        }

        public double ConvertFromMiles(double input)
        {
            return input * _fromMilesFactor;
        }
    }

    public static class DistanceExtensions
    {
        public static double DistanceBetweenGeolocations(this Profile profile1, Profile profile2)
        {
            return new Coordinates(profile1.Geolocation.Latitude, profile1.Geolocation.Longitude)
                        .DistanceTo(
                            new Coordinates(profile2.Geolocation.Latitude, profile2.Geolocation.Longitude), 
                            UnitOfLength.Kilometers
                        );
        }

        public static double DistanceBetweenLastGeolocation(this Profile profile1, Profile profile2)
        {
            return new Coordinates(profile1.LastGeolocation.Latitude, profile1.LastGeolocation.Longitude)
                        .DistanceTo(
                            new Coordinates(profile2.LastGeolocation.Latitude, profile2.LastGeolocation.Longitude),
                            UnitOfLength.Kilometers
                        );
        }

        public static void NotifyUserUsingWebPush(this IEnumerable<Friend> friends, ILogger logger = null)
        {
            foreach (var friend in friends)
            {
                friend.NotifyUserUsingWebPush(logger);
            }
        }

        public static void NotifyUserUsingWebPush(this Friend friendRequest, ILogger logger = null)
        {
            friendRequest.RequestedBy.Devices.NotifyDevices(friendRequest.RequestedTo.FirstName + " " + friendRequest.RequestedTo.FirstName, logger);
            friendRequest.RequestedTo.Devices.NotifyDevices(friendRequest.RequestedBy.FirstName + " " + friendRequest.RequestedBy.LastName, logger);
        }
    }
}
