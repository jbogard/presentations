namespace CodeCampServerLite.IntegrationTests.Infrastructure.Maps
{
    using Core.Domain.Model;
    using Should;
    using Xunit;

    public class SpeakerMapTester : IntegrationTestBase
    {
        [Fact]
        public void Should_map_all_speaker_fields()
        {
            var speaker = new Speaker("Joe", "Schmoe")
            {
                Bio = "I come from France"
            };

            SaveEntities(speaker);

            var loaded = SessionSource.CreateSession().Load<Speaker>(speaker.Id);

            loaded.FirstName.ShouldEqual(speaker.FirstName);
            loaded.LastName.ShouldEqual(speaker.LastName);
            loaded.Bio.ShouldEqual(speaker.Bio);
        }
    }
}