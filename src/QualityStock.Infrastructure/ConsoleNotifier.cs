using QualityStock.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Infrastructure
{
    public sealed class ConsoleNotifier : INotifier
    {
        public Task NotifyAsync(string topic, string message, CancellationToken ct = default)
        {
            Console.WriteLine($"[{DateTimeOffset.UtcNow:u}] {topic}: {message}");
            return Task.CompletedTask;
        }
    }
}
