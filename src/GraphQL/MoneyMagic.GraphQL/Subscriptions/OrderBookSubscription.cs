namespace MoneyMagic.GraphQL.Subscriptions
{
    [SubscriptionType]
    public class OrderBookSubscription
    {
        [Subscribe]
        [Topic(nameof(OrderBookSubscription.BinanceStream))]
        public string BinanceStream([EventMessage] string stream,[Service] ILogger<OrderBookSubscription> logger)
        {
            logger.LogInformation("{Stream}", stream);
            return stream;
        }
    }
}
