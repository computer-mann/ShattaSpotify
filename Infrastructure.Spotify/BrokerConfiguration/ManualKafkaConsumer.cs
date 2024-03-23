using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
