using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class MessageGroupProfileRepository
        : BaseRepository<MessageGroupProfile, Guid>,
          IMessageGroupProfileRepository
    {
        public MessageGroupProfileRepository(MSMContext context)
            : base(context) { }

        public MessageGroupProfile Find(Guid conversationId, int id)
        {
            return DbSet.Find(id, conversationId);
        }

        public MessageGroupProfile GetMessageGroupProfileByMessageGroupId(Guid conversationId, int id)
        {
            return DbSet.SingleOrDefault(p => p.MessageGroupId == conversationId && p.ProfileId == id);
        }

        public void LoadMessagesFromMessageGroup(MessageGroupProfile messageGroupProfile)
        {
            _dbContext.Entry(messageGroupProfile)
                .Reference(p => p.MessageGroup)
                .Query()
                .Include(p => p.Messages)
                    .ThenInclude(p => p.MessageFrom)
                .Load();
        }
    }
}
