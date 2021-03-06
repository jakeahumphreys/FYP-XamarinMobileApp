using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FYP_Mobile.DTO
{
    [DataContract(Name = "MobileUser")]
    public class MobileUserDto
    {
        [DataMember(Name = "User_ID")]
        public string Id { get; set; }
        [DataMember(Name = "User_First_Name")]
        public string FirstName { get; set; }
        [DataMember(Name = "User_Surname")]
        public string Surname { get; set; }
    }
}
