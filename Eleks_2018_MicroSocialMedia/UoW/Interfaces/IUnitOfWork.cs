using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.UoW.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProfileRepository ProfileRepository { get; }
        IFriendRepository FriendRepository { get; }
        IGeoRepository GeoRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IMeetingRepository MeetingRepository { get; }
        ILastGeoRepository LastGeoRepository { get; }
        IMessageRepository MessageRepository { get; }
        IMessageGroupRepository MessageGroupRepository { get; }
        IMessageGroupProfileRepository MessageGroupProfileRepository { get; }
        IPostRepository PostRepository { get; }
        bool Commit();
    }
}
