using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class MessageGroup
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<MessageGroupProfile> Members { get; set; }

        [NotMapped]
        public int MembersCount => Members.Count;

        public void AddMember(Profile profile)
        {
            Members.Add(new MessageGroupProfile
            {
                Profile = profile,
                MessageGroup = this
            });
        }

        public Message AddMessage(string messageBody, Profile messageFrom)
        {
            var profile = Members.SingleOrDefault(p => p.Profile == messageFrom);

            if (profile == null)
            {
                this.AddMember(messageFrom);
            }

            var newMessage = new Message
            {
                MessageBody = messageBody,
                MessageStatus = MessageStatus.Unread,
                MessageDate = DateTime.Now,
                MessageGroup = this,
                MessageFrom = messageFrom
            };

            Messages.Add(newMessage);

            return newMessage;
        }
    }
}
