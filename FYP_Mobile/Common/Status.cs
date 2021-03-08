using System;
using System.Collections.Generic;
using System.Text;

namespace FYP_Mobile.Common
{
    public enum Status
    {
        NotOnShift = 0,
        OnShiftNotMobile = 1,
        TransitToLocation = 2,
        AtLocation = 3,
        Break = 4,
        Unknown = 5,
        AssistanceRequired = 6
    }
}
