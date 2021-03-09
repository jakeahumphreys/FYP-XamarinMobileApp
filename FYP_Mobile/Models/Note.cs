using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FYP_Mobile.Models
{
    [DataContract(Name = "Note")]
    public class Note
    {
        [DataMember(Name = "Note_Content")]
        public string Content { get; set; }
        [DataMember(Name = "Note_Sender_ID")]
        public string SenderId { get; set; }
        [DataMember(Name = "Note_Time_Created")]
        public DateTime TimeCreated { get; set; }
    }
}
