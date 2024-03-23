using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Spotify.BrokerConfiguration
{
    public record KafkaTestMessage(string Artiste,string SongTitle);
}
