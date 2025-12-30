using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.Abstractions
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
        DateOnly TodayUtcDateOnly { get; }
    }
}
