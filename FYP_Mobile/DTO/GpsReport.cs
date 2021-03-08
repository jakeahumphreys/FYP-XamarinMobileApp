using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FYP_Mobile.DTO
{
    [DataContract]
    class GpsReport
    {
        [DataMember(Name = "Gps_Report_Time")]
        public DateTime Time { get; set; }
        [DataMember(Name = "Gps_Report_Lat")]
        public decimal Latitude { get; set; }
        [DataMember(Name = "Gps_Report_Long")]
        public decimal Longitude { get; set; }
        [DataMember(Name = "Gps_Report_User_ID")]
        public string UserId { get; set; }
    }
}
