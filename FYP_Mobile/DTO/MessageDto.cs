using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FYP_Mobile.Common;

namespace FYP_Mobile.DTO
{
    [DataContract(Name = "Message")]
    public class MessageDto
    {
        [DataMember(Name = "Message_Type")]
        public MessageType MessageType { get; set; }
        [DataMember(Name = "Message_Sender_ID")]
        public string SenderId { get; set; }
        [DataMember(Name = "Message_Recipient_ID")]
        public string RecipientId { get; set; }
        [DataMember(Name = "Message_Content")]
        public string Content { get; set; }
    }
}
