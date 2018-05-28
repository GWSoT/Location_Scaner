using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Services.Interfaces
{
    public interface INotificationService
    {
        void NotifyDevices(ICollection<Device> devices, string fullName);
        void SendPush(string deviceId, string fullName);
        void NotifyUserUsingWebPush(IEnumerable<Friend> friends);
        void NotifyUserUsingWebPush(Friend friendRequest);
    }
}
