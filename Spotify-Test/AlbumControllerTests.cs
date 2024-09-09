using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spotify.Controllers;

namespace Spotify_Test
{
    public class AlbumControllerTests
    {
        private readonly IHashids hashidsMock = Substitute.For<IHashids>();
        private readonly ILogger<AlbumController> logger = Substitute.For<ILogger<AlbumController>>();
        private readonly AlbumController sut;

        public AlbumControllerTests()
        {
            sut = new AlbumController(hashidsMock,logger);
        }

        [SetUp]
        public void Setup()
        {
            
            hashidsMock.Encode(23).Returns("######");
        }

        [Test]
        public void IndexAction_Returns_A_Hash_ForANumber()
        {
            var two = hashidsMock.Encode(23);
            two.Should().Be("######");
        }
        [Test]
        public void IndexAction_Returns_OK()
        {
             var result=sut.Index();
            hashidsMock.Received(1).Encode(43);
            result.Should().BeOfType<OkObjectResult>().Should();
   
        }
    }
}