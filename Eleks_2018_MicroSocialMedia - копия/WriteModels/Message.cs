using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.WriteModels
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime MessageDate { get; set; }
        public string MessageBody { get; set; }
        public MessageStatus MessageStatus { get; set; }
        

        public Guid? MessageGroupId { get; set; }
        public MessageGroup MessageGroup { get; set; }

        public int? MessageFromId { get; set; }
        public Profile MessageFrom { get; set; }

        [NotMapped]
        public bool Read => MessageStatus == MessageStatus.Read;
    }
    
    public enum MessageStatus
    {
        Unread, 
        Read
    }
}
