using MassTransit;

namespace Infrastructure.Spotify.BrokerConfiguration
{
    internal class ManualKafkaConsumer : IConsumer<KafkaTestMessage>
    {
        public Task Consume(ConsumeContext<KafkaTestMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
