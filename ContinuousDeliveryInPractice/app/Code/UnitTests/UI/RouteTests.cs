using System.Web.Routing;
using CodeCampServerLite.UI.Controllers;
using NUnit.Framework;
using MvcContrib.TestHelper;
using UI;

namespace CodeCampServerLite.UnitTests.UI
{
    [TestFixture]
    public class RouteTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);
        }

        [Test]
        public void Should_map_default_route()
        {
            "~/".ShouldMapTo<HomeController>(x => x.Index());
        }

        [Test]
        public void Should_map_to_default_action()
        {
            "~/home".ShouldMapTo<HomeController>(x => x.Index());
        }

        [Test]
        public void Should_map_to_specific_controller_and_action()
        {
            "~/home/about".ShouldMapTo<HomeController>(x => x.About());
        }
    }
}