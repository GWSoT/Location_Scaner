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
    public class MessageRepository
        : BaseRepository<Message, int>,
          IMessageRepository
    {
        public MessageRepository(MSMContext context)
            : base(context) { }

        public Message GetLastMessageForGivenGroup(MessageGroup messageGroup)
        {
            _dbContext.Entry(messageGroup)
                .Reference(p => p.Messages)
                .Load();

            return messageGroup.Messages.OrderBy(x => x.MessageDate).Last();
        }
    }
}
