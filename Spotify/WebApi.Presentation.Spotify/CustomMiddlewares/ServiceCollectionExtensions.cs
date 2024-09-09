using Confluent.Kafka;
using Infrastructure.Spotify.BrokerConfiguration;
using MassTransit;
using Spotify.Services;

namespace Api.Presentation.Spotify.CustomMiddlewares
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddTypedHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<SpotifyHttpService>();

        }
        public static void AddKafkaProducer(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingInMemory();
                x.AddRider(ride =>
                {                    
                    ride.AddProducer<KafkaTestMessage>(KakfaTopics.NewMusicFriday, new ProducerConfig()
                    {
                        Acks = Acks.Leader,
                        Partitioner=Partitioner.Consistent,
                        
                    });
                    ride.UsingKafka((context, k) => 
                    {
                        k.Host(["localhost:8099", "localhost:8097", "localhost:8098"]);
                        
                    });
                });
            });
        }
    }
}
