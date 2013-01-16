using MvcContrib.TestHelper.Ui;
using MvcContrib.TestHelper.WatiN;
using NUnit.Framework;
using WatiN.Core;

namespace CodeCampServerLite.UITests
{
    [RequiresSTA]
    public class WebTestBase
    {
        private InputTesterFactoryRegistry _factoryRegistry;

        [SetUp]
        public void Setup()
        {
            Browser = new WatinDriver(new IE("http://localhost:8084"), "http://localhost:8084");
            _factoryRegistry = new InputTesterFactoryRegistry();
        }

        protected WatinDriver Browser { get; set; }

        protected InputForm<T> Form<T>()
        {
            return new InputForm<T>(Browser, _factoryRegistry);
        }

        [TearDown]
        public void TearDown()
        {
            Browser.Dispose();
            Browser = null;
        }
    }
}