using AutoMapper;
using CodeCampServerLite.UI.Helpers;
using MbUnit.Framework;

namespace CodeCampServerLite.UnitTests.UI
{
    [TestFixture]
    public class AutoMapperTests
    {
        [FixtureSetUp]
        public void Setup()
        {
            AutoMapperBootstrapper.Initialize();
        }

        [Test]
        public void Should_map_all_dtos()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}