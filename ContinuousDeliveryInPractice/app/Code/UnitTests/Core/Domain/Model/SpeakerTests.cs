namespace CodeCampServerLite.UnitTests.Core.Domain.Model
{
    using CodeCampServerLite.Core.Domain.Model;
    using Should;
    using Xunit;

    public class SpeakerTests
    {
        [Fact]
        public void Should_construct_speaker_with_first_and_last_name()
        {
            var speaker = new Speaker("Bob", "Smith");

            speaker.FirstName.ShouldEqual("Bob");
            speaker.LastName.ShouldEqual("Smith");
        }
    }
}