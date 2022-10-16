namespace Spotify_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AdditionTest_Equals()
        {
            var two = 2;

            Assert.That(two, Is.EqualTo(1 + 1));
        }
        [Test]
        public void AdditionTest_NotEqual()
        {
            var three = 3;

            Assert.That(three, Is.Not.EqualTo(1 + 1));
        }
    }
}