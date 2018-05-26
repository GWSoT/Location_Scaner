using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class NotificationRepository
        : BaseRepository<Notification, int>,
          INotificationRepository
    {
        public NotificationRepository(MSMContext context)
            : base(context)
        { }
    }
}
