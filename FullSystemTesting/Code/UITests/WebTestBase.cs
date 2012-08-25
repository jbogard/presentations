using MvcContrib.TestHelper.Ui;
using MvcContrib.TestHelper.WatiN;
using NUnit.Framework;
using WatiN.Core;

namespace CodeCampServerLite.UITests
{
    public abstract class WebTestBase
    {

        [SetUp]
        public void FixtureSetup()
        {
            var baseUrl = "http://localhost:8084";
            Browser = new WatinDriver(new IE(baseUrl), baseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            Browser.Dispose();
            Browser = null;
        }

        protected InputForm<T> Form<T>()
        {
            return new InputForm<T>(Browser, _factoryRegistry);
        }

        private static readonly InputTesterFactoryRegistry _factoryRegistry
            = new InputTesterFactoryRegistry();

        protected IBrowserDriver Browser { get; private set; }
    }
}