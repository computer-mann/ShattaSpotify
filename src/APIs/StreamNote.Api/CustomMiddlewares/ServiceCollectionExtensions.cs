using BrokerConfiguration;
using Confluent.Kafka;
using MassTransit;
using StackExchange.Redis;

namespace StreamNote.Api.CustomMiddlewares
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddTypedHttpClients(this IServiceCollection services)
        {
           // services.AddHttpClient<SpotifyHttpService>();

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
            var confOptions=ConfigurationOptions.Parse(configuration.GetConnectionString("Redis")!);
            confOptions.DefaultDatabase =int.Parse(configuration["RedisDefaultDatabase"]!);
            confOptions.User = "default";
            confOptions.Password = configuration["RedisPassword"];

            var multiplexer = ConnectionMultiplexer.Connect(confOptions);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            //services.AddSingleton<IRedisConnectionProvider>(new RedisConnectionProvider(multiplexer));
        }
    }
}
