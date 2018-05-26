using AutoMapper;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(
            IMessageService messageService, 
            IMapper mapper,
            ILogger<ChatHub> logger)
        {
            _messageService = messageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SubscribeToGroup(string groupId)
        {
            await Groups.AddAsync(Context.ConnectionId, groupId);
        }

        public async Task UnsubscribeFromGroup(string groupId)
        {
            await Groups.RemoveAsync(Context.ConnectionId, groupId);
        }

        public async Task SendMessage(string messageBody, string groupId)
        {
            if (string.IsNullOrEmpty(messageBody))
            {
                _logger.LogCritical("Empty message");
                return;
            }

            var message = await _messageService.CreateMessageAsync(messageBody, groupId, Context.Connection.User);

            if (message == null)
            {
                _logger.LogCritical("Message == null");
                return;
            }

            _logger.LogCritical("Sending message to other clients");
            await Clients
                .Group(groupId)
                .SendAsync("NewMessage", message);
        }
    }
}
