using Confluent.Kafka;
using Infrastructure.Spotify.BrokerConfiguration;
using MassTransit;
using Redis.OM;
using Spotify.Services;
using StackExchange.Redis;

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
                        k.Host(["localhost:9092"]);
                        
                    });
                });
            });
        }
        public static void AddRedisOm(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton<RedisConnectionProvider>(new RedisConnectionProvider(configuration.GetSection("Redis").Value!));
        }
    }
}
