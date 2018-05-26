using Eleks_2018_MicroSocialMedia.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Friend
    {
        public int Id { get; set; }
        [Column(Order = 0)]
        public int? RequestedById { get; set; }
        [Column(Order = 1)]
        public int? RequestedToId { get; set; }
        public virtual Profile RequestedBy { get; set; }
        public virtual Profile RequestedTo { get; set; }

        public DateTime LastNotification { get; set; }

        public FriendFlag FriendFlag { get; set; }

        [NotMapped]
        public double DistanceBetweenGeolocations => RequestedTo.DistanceBetweenGeolocations(RequestedBy);

        [NotMapped]
        public double DistanceBetweenLastGeolocations => RequestedTo.DistanceBetweenLastGeolocation(RequestedBy);

        [NotMapped]
        public bool Approved => FriendFlag == FriendFlag.Approved;

        [NotMapped]
        public bool IsInRange => DistanceBetweenGeolocations <= 0.05;

        [NotMapped]
        public bool UsersOnline => RequestedBy.IsOnline && RequestedTo.IsOnline;

        [NotMapped]
        public bool IsInMeetRange => DistanceBetweenGeolocations <= 0.01;

        [NotMapped]
        public bool CanAddAsMeeting => LastDistance > 0.1 && IsInMeetRange && UsersOnline;

        [NotMapped]
        public double LastDistance => Math.Abs(DistanceBetweenLastGeolocations - DistanceBetweenGeolocations);

        [NotMapped]
        public bool CanSendNotification => DateTime.Now > LastNotification.AddMinutes(15) && LastDistance > 0.05;
    }

    public enum FriendFlag
    {
        None,
        Approved
    }
}
