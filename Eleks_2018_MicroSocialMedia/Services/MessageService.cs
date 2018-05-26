using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.UoW.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Services
{
    public class MessageService
        : IMessageService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMessageRepository _messageRepo;
        private readonly IProfileRepository _profileRepo;
        private readonly IMessageGroupRepository _messageGroupRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IMessageGroupProfileRepository _messageGroupProfileRepo;
        private readonly ILogger<MessageService> _logger;

        public MessageService(
            UserManager<AppUser> userManager,
            IMessageRepository messageRepo,
            IProfileRepository profileRepo,
            IMessageGroupRepository messageGroupRepo,
            IMapper mapper,
            IUnitOfWork uow,
            IMessageGroupProfileRepository messageGroupProfileRepo,
            ILogger<MessageService> logger)
        {
            _mapper = mapper;
            _messageGroupRepo = messageGroupRepo;
            _userManager = userManager;
            _messageGroupProfileRepo = messageGroupProfileRepo;
            _messageRepo = messageRepo;
            _profileRepo = profileRepo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ICollection<MessageGroupDto>> GetMessageGroups(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadUserProfileWithMessageGroups(user);

            return _mapper.Map<List<MessageGroupDto>>(user.Profile.MessageGroups.Select(p => p.MessageGroup));
        }

        public async Task DeleteConversation(string conversationId, ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return;
            }

            var messageGroup = _messageGroupProfileRepo.GetMessageGroupProfileByMessageGroupId(Guid.Parse(conversationId), user.ProfileId);

            if (messageGroup == null)
            {
                return;
            }

            _profileRepo.LoadUserProfileWithMessageGroups(user);

            user.Profile.MessageGroups.Remove(messageGroup);

            _uow.Commit();
        }

        public async Task CreateConversation(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return;
            }

            _profileRepo.LoadUserProfileWithMessageGroups(user);

            user.Profile.MessageGroups.Add(new MessageGroupProfile
            {
                Profile = user.Profile,
                MessageGroup = new MessageGroup
                {
                    GroupName = "Default Name",
                    Messages = new List<Message>(),
                    Members = new List<MessageGroupProfile>(),
                }
            });

            _logger.LogCritical("Created new conversation");

            _uow.Commit();
        }

        public async Task InviteUserToConversation(string conversationId, ClaimsPrincipal claims, string userId)
        {
            var user = await _userManager.GetUserAsync(claims);
            
            if (user == null)
            {
                return;
            }

            var parsedConversationId = Guid.Parse(conversationId);

            var conversation = _messageGroupProfileRepo.GetMessageGroupProfileByMessageGroupId(parsedConversationId, user.ProfileId);

            if (conversation == null)
            {
                return;
            }

            _messageGroupProfileRepo.LoadMessagesFromMessageGroup(conversation);

            var invitedProfile = _profileRepo.GetProfileByExternalId(userId);

            if (invitedProfile == null)
            {
                return;
            }


            conversation.MessageGroup.AddMember(invitedProfile);

            _uow.Commit();
        }

        public async Task<MessageDto> CreateMessageAsync(string messageBody, string groupId, ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return null;
            }

            _profileRepo.LoadUserProfileWithMessageGroups(user);

            var messageGroup = user.Profile.MessageGroups.SingleOrDefault(p => p.MessageGroupId == Guid.Parse(groupId));

            if (messageGroup == null)
            {
                return null;
            }

            _messageGroupProfileRepo.LoadMessagesFromMessageGroup(messageGroup);

            var message = messageGroup.MessageGroup.AddMessage(messageBody, user.Profile);

            if (!_uow.Commit())
            {
                return null;
            }

            return _mapper.Map<MessageDto>(message, opts => opts.Items["Profile"] = user.Profile);
        }

        public async Task<ICollection<MessageDto>> GetMessages(ClaimsPrincipal claims, string conversationId)
        {
            var user = await _userManager.GetUserAsync(claims);

            if (user == null)
            {
                return null;
            }


            _profileRepo.LoadUserProfileWithMessageGroups(user);

            var messageGroup = user.Profile.MessageGroups
                                .SingleOrDefault(p => p.MessageGroupId == Guid.Parse(conversationId));

            if (messageGroup == null)
            {
                return null;
            }

            _messageGroupProfileRepo.LoadMessagesFromMessageGroup(messageGroup);

            var messages = _mapper.Map<ICollection<MessageDto>>(messageGroup.MessageGroup.Messages, opts => opts.Items["Profile"] = user.Profile);

            return messages;
        }
    }
}
