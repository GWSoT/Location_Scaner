using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Firebase
{
    public static class PushNotifications
    {

        public static string firebaseKey = "<INPUT YOUR FIREBASE KEY>";
        public static string senderId = "<INPUT YOUR SENDER ID>";
        public static string fcmUrl = "https://fcm.googleapis.com/fcm/send";

        public static void NotifyDevices(this ICollection<Device> devices, string fullName, ILogger logger = null)
        {
            foreach(var device in devices)
            {
                SendPush(device.DeviceId, fullName, logger);

                if (logger != null)
                {
                    logger.LogCritical("Notify Devices");
                }
            }
        }

        public static void SendPush(string deviceId, string fullName, ILogger logger = null)
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
                logger.LogCritical("Fine!");
            } catch { logger.LogCritical("Exception in Send Push"); }
        }

        public class Data
        {
            public string To { get; set; }
            public Notification Notification { get; set; }
        }

        public class Notification
        {
            public string Body { get; set; }
            public string Title { get; set; }
        }
    }
}
