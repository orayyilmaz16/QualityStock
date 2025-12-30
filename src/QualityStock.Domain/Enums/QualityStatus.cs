using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Domain.Enums
{
    public enum QualityStatus
    {
        Unknown = 0,
        Pending = 1,
        Passed = 2,
        Failed = 3,
        Quarantined = 4
    }
}
