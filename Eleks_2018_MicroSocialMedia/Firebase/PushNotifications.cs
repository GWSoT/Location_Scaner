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
    [Obsolete("Warning: Push Notifiaction moved to NotificationService.cs")]
    // The code was moved to NotificationService.cs
    // All configuration from now will be stored in appsetting.json
    public static class PushNotifications
    {
        // It's better to move secret & api keys to some config
        public static string firebaseKey = "<INPUT YOUR FIREBASE KEY>";
        public static string senderId = "<INPUT YOUR SENDER ID>";
        public static string fcmUrl = "https://fcm.googleapis.com/fcm/send";
    }
}
