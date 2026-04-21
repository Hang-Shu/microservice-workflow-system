using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : IExchangeInfo;
        Task PublishAsync<T>(T @event, string exchangeName, string routingKey);
    }
}
