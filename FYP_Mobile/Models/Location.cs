using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FYP_Mobile.Models
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "Location_ID")]
        public int Id { get; set; }
        [DataMember(Name = "Location_Label")]
        public string Label { get; set; }
        [DataMember(Name = "Location_Latitude")]
        public decimal Latitude { get; set; }
        [DataMember(Name = "Location_Longitude")]
        public decimal Longitude { get; set; }
        [DataMember(Name = "Location_Notes")]
        public List<Note> Notes { get; set; }
    }
}
