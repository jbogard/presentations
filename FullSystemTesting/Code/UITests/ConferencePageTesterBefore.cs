using System;
using System.Threading;
using CodeCampServerLite.UI;
using CodeCampServerLite.UI.Controllers;
using CodeCampServerLite.UI.Models;
using MvcContrib.TestHelper.Ui;
using MvcContrib.TestHelper.WatiN;
using NUnit.Framework;
using Should;
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

    [TestFixture, RequiresSTA]
    public class ConferencePageTesterAfter : WebTestBase
    {
        [Test]
        public void Should_be_able_to_edit_conference()
        {
            Browser.ClickLink(SiteNav.Conferences);

            Browser.ClickLink("Edit CodeMash");

			Form<ConferenceEditModel>()
				.Input(m => m.Name, "CodeMashFoo")
				.Submit();

            Browser
                .VerifyPage<ConferenceController>(c => c.Index(0));

			Browser.AssertValue<ConferenceListModel[]>(
				c => c[0].Name, "CodeMashFoo");
        }
    }

    [TestFixture, RequiresSTA]
    public class ConferencePageTesterBefore
    {
        [Test]
        public void Should_be_able_to_edit_conference()
        {
            using (var ie = new IE("http://localhost:8084"))
            {
                var conferencesLink = ie.Link(Find.ByText("Conferences"));
                conferencesLink.Click();

                var editCodeMashLink = ie.Link(Find.ByText("Edit"));
                editCodeMashLink.Click();

                var nameBox = ie.TextField(Find.ByName("Name"));
                nameBox.TypeText("CodeMashFoo");

                var submitBtn = ie.Button(Find.ByValue("Save"));
                submitBtn.Click();

                ie.Url.ShouldEqual("http://localhost:8084/Conference");


                ie.ContainsText("CodeMashFoo").ShouldBeTrue();
            }
        }
    }
}