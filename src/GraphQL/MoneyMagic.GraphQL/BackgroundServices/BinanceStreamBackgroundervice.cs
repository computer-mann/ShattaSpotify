
using HotChocolate.Subscriptions;
using MoneyMagic.GraphQL.Subscriptions;
using System.Text.Json;

namespace MoneyMagic.GraphQL.BackgroundServices
{
    public class BinanceStreamBackgroundervice : BackgroundService
    {
        
        private readonly IServiceProvider _serviceProvider;

        public BinanceStreamBackgroundervice(IServiceProvider serviceProvider)
        {
            
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var _topicEventSender = scope.ServiceProvider.GetRequiredService<ITopicEventSender>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<BinanceStreamBackgroundervice>>();
            logger.LogInformation("Binance Stream Background Service is starting.");
            while ( !stoppingToken.IsCancellationRequested)
            {
                // Simulate receiving data from Binance stream
                var simulatedData = new { Symbol = "BTCUSDT", Price = Random.Shared.NextDouble(), Timestamp = DateTime.UtcNow };
                // Publish the data to a topic
                await _topicEventSender.SendAsync(nameof(OrderBookSubscription.BinanceStream), JsonSerializer.Serialize(simulatedData), stoppingToken);
                // Wait for a short period before sending the next update
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
