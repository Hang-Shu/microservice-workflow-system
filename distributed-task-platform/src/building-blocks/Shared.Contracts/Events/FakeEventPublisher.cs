using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public class FakeEventPublisher : IEventPublisher
    {
        public Task PublishAsync<T>(T @event)
        {
            System.Diagnostics.Debug.WriteLine($"[FakePublisher] Published event: {typeof(T).Name}");
            return Task.CompletedTask;
        }
    }
}
