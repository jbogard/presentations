using MvcContrib.TestHelper.Ui;
using MvcContrib.TestHelper.WatiN;
using WatiN.Core;

namespace CodeCampServerLite.UITests
{
    using System;

    public class WebTestBase : IDisposable
    {
        private readonly InputTesterFactoryRegistry _factoryRegistry;

        public WebTestBase()
        {
            Browser = new WatinDriver(new IE("http://localhost:8084"), "http://localhost:8084");
            _factoryRegistry = new InputTesterFactoryRegistry();
        }

        protected WatinDriver Browser { get; set; }

        protected InputForm<T> Form<T>()
        {
            return new InputForm<T>(Browser, _factoryRegistry);
        }

        public void Dispose()
        {
            Browser.Dispose();
            Browser = null;
        }
    }
}