using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Services.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> CreateMessageAsync(string messageBody, string groupId, ClaimsPrincipal claims);
        Task<ICollection<MessageGroupDto>> GetMessageGroups(ClaimsPrincipal claims);
        Task CreateConversation(ClaimsPrincipal claims);
        Task InviteUserToConversation(string conversationId, ClaimsPrincipal claims, string userId);
        Task<ICollection<MessageDto>> GetMessages(ClaimsPrincipal user, string conversationId);
        Task DeleteConversation(string conversationId, ClaimsPrincipal claims);
    }
}
