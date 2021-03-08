using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FYP_Mobile.Models
{
    [DataContract]
    public class Note
    {
        [DataMember(Name = "Note_Content")]
        public string Content { get; set; }
    }
}
