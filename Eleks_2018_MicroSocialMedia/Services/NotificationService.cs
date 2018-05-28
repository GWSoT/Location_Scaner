using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Services
{
    public class NotificationService
        : INotificationService
    {
        private readonly string fcmUrl;
        private readonly string senderId;
        private readonly string firebaseKey;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IConfiguration config, ILogger<NotificationService> logger)
        {
            var firebase = config.GetSection("Firebase");
            fcmUrl = firebase.GetValue<string>("FcmUrl");
            senderId = firebase.GetValue<string>("SenderId");
            firebaseKey = firebase.GetValue<string>("ServerKey");
            _logger = logger;
        }

        public void NotifyDevices(ICollection<Device> devices, string fullName)
        {
            foreach(var device in devices)
            {
                this.SendPush(device.DeviceId, fullName);
            }
        }

        public void NotifyUserUsingWebPush(IEnumerable<Friend> friends)
        {
            foreach (var friend in friends)
            {
                this.NotifyUserUsingWebPush(friend);
            }
        }

        public void NotifyUserUsingWebPush(Friend friendRequest)
        {
            this.NotifyDevices(friendRequest.RequestedBy.Devices, friendRequest.RequestedTo.FirstName + " " + friendRequest.RequestedTo.FirstName);
            this.NotifyDevices(friendRequest.RequestedTo.Devices, friendRequest.RequestedBy.FirstName + " " + friendRequest.RequestedBy.LastName);
        }

        public void SendPush(string deviceId, string fullName)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(fcmUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";

                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = $"{fullName} is near you",
                        title = "Friend!",
                    }
                };

                var json = JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                webRequest.Headers.Add($"Authorization: key={firebaseKey}");
                webRequest.Headers.Add($"Sender: id={senderId}");
                webRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse webResponse = webRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = webResponse.GetResponseStream())
                        {

                            using (StreamReader reader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromServer = reader.ReadToEnd();
                                string str = responseFromServer;
                            }
                        }
                    }
                }
                _logger.LogCritical($"Successfully notified device with id: {deviceId}");
            }
            catch
            {
                _logger.LogCritical($"Error in NotificationService for user {fullName}");
            }
        }
    }
}
