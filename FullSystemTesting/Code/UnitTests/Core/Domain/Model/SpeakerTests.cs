using CodeCampServerLite.Core.Domain.Model;
using NUnit.Framework;
using Should;

namespace CodeCampServerLite.UnitTests.Core.Domain.Model
{
	[TestFixture]
	public class SpeakerTests
	{
		[Test]
		public void Should_construct_speaker_with_first_and_last_name()
		{
			var speaker = new Speaker("Bob", "Smith");

			speaker.FirstName.ShouldEqual("Bob");
			speaker.LastName.ShouldEqual("Smith");
		}
	}
}