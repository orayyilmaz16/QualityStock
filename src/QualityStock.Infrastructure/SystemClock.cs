using QualityStock.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Infrastructure
{

    public sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
        public DateOnly TodayUtcDateOnly => DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
