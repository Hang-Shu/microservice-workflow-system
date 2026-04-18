namespace Shared.Common
{
    public interface IExchangeInfo
    {
        string ExchangeName { get; }

        string RoutingKey { get; }
    }
}
