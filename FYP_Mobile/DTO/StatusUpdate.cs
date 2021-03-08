using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FYP_Mobile.Common;

namespace FYP_Mobile.DTO
{
    [DataContract]
    public class StatusUpdate
    {
        [DataMember(Name = "Status_User_ID")]
        public string UserId { get; set; }
        [DataMember(Name = "Status_User_Status")]
        public Status status { get; set; }
    }
}
