using Infrastructure.Spotify.BrokerConfiguration;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Spotify.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ITopicProducer<KafkaTestMessage> _topicProducer;
        private readonly ITopic

        public TrackController(ITopicProducer<KafkaTestMessage> topicProducer)
        {
            _topicProducer = topicProducer;
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
           await _topicProducer.Produce(new KafkaTestMessage("Shatta", "Chasing Paper"),ct);
            return Accepted();
        }
    }
}
