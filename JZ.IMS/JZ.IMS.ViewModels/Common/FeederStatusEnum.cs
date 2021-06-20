using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    public enum FeederStatusEnum
    {
        FEEDER_STATUS_USABLE = 1,
        FEEDER_STATUS_IN_USING = 2,
        FEEDER_STATUS_WAITMAINTAIN = 3,
        FEEDER_STATUS_WAITEMEND = 4,
        FEEDER_STATUS_WAITREPAIR = 5,
        FEEDER_STATUS_SCRAPED = 6,
        FEEDER_STATUS_REJECT = 7
    }
}
