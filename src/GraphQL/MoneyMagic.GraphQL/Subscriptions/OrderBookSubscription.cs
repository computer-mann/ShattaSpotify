namespace MoneyMagic.GraphQL.Subscriptions
{
    [SubscriptionType]
    public class OrderBookSubscription
    {
        public async IAsyncEnumerable<int> CountToTen([Service] ILogger<OrderBookSubscription> logger)
        {
            for (int i = 1; i <= 10; i++)
            {
                logger.LogInformation("Emitting count: {Count}", i);
                yield return i;
                await Task.Delay(1000); // Simulate some delay
            }
        }
    }
}
