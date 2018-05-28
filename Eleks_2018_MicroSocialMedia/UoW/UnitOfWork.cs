using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Repositories;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.UoW.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.UoW
{
    public class UnitOfWork
        : IUnitOfWork
    {
        private IUserRepository _userRepository;
        private IProfileRepository _profileRepository;
        private IFriendRepository _friendRepository;
        private IGeoRepository _geoRepository;
        private INotificationRepository _notificationRepository;
        private IMeetingRepository _meetingRepository;
        private ILastGeoRepository _lastGeoRepository;
        private IMessageRepository _messageRepository;
        private IMessageGroupRepository _messageGroupRepository;
        private IMessageGroupProfileRepository _messageGroupProfileRepository;
        private IGeolocationHistoryRepository _geolocationHistoryRepository;
        private IPostRepository _postRepository;
        private IMeetingProfileRepository _meetingProfileRepository;
        private readonly MSMContext _dbContext;

        public UnitOfWork(MSMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository = _userRepository ?? new UserRepository(_dbContext);
            }
        }

        public IProfileRepository ProfileRepository
        {
            get
            {
                return _profileRepository = _profileRepository ?? new ProfileRepository(_dbContext);
            }
        }

        public IFriendRepository FriendRepository
        {
            get
            {
                return _friendRepository = _friendRepository ?? new FriendRepository(_dbContext);
            }
        }

        public IGeoRepository GeoRepository
        {
            get
            {
                return _geoRepository = _geoRepository ?? new GeoRepository(_dbContext);
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                return _notificationRepository = _notificationRepository ?? new NotificationRepository(_dbContext);
            }
        }

        public IMeetingRepository MeetingRepository
        {
            get
            {
                return _meetingRepository = _meetingRepository ?? new MeetingRepository(_dbContext);
            }
        }

        public ILastGeoRepository LastGeoRepository
        {
            get
            {
                return _lastGeoRepository = _lastGeoRepository ?? new LastGeoRepository(_dbContext);
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                return _messageRepository = _messageRepository ?? new MessageRepository(_dbContext);
            }
        }

        public IMessageGroupRepository MessageGroupRepository
        {
            get
            {
                return _messageGroupRepository = _messageGroupRepository ?? new MessageGroupRepository(_dbContext);
            }
        }

        public IMessageGroupProfileRepository MessageGroupProfileRepository
        {
            get
            {
                return _messageGroupProfileRepository = _messageGroupProfileRepository ?? new MessageGroupProfileRepository(_dbContext);
            }
        }

        public IPostRepository PostRepository
        {
            get
            {
                return _postRepository = _postRepository ?? new PostRepository(_dbContext);
            }
        }

        public IGeolocationHistoryRepository GeolocationHistoryRepository
        {
            get
            {
                return _geolocationHistoryRepository = _geolocationHistoryRepository ?? new GeolocationHistoryRepository(_dbContext);
            }
        }

        public IMeetingProfileRepository MeetingProfileRepository
        {
            get
            {
                return _meetingProfileRepository = _meetingProfileRepository ?? new MeetingProfileRepository(_dbContext);
            }
        }

        public bool Commit()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
