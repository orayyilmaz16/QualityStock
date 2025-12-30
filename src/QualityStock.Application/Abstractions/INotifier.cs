using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Application.Abstractions
{
    public interface INotifier
    {
        Task NotifyAsync(string topic, string message, CancellationToken ct = default);
    }
}
