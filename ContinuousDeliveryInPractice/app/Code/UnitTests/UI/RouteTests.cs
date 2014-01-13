namespace CodeCampServerLite.UnitTests.UI
{
    using System.Web.Routing;
    using CodeCampServerLite.UI.Controllers;
    using global::UI;
    using MvcContrib.TestHelper;
    using Xunit;

    public class RouteTests
    {
        static RouteTests()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);
        }

        [Fact]
        public void Should_map_default_route()
        {
            "~/".ShouldMapTo<HomeController>(x => x.Index());
        }

        [Fact]
        public void Should_map_to_default_action()
        {
            "~/home".ShouldMapTo<HomeController>(x => x.Index());
        }

        [Fact]
        public void Should_map_to_specific_controller_and_action()
        {
            "~/home/about".ShouldMapTo<HomeController>(x => x.About());
        }
    }
}