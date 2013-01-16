using System;
using System.Threading;
using CodeCampServerLite.UI;
using CodeCampServerLite.UI.Controllers;
using CodeCampServerLite.UI.Models;
using MvcContrib.TestHelper.Ui;
using NUnit.Framework;
using Should;

namespace CodeCampServerLite.UITests
{
    [TestFixture]
    public class ConferenceEditingTests : WebTestBase
    {
        [Test]
        public void Should_be_able_to_edit_conference_name()
        {
            Browser.ClickLink(SiteNav.Conferences);

            Browser.ClickLink(SiteNav.EditPrefix + " CodeMash");

            Form<ConferenceEditModel>()
                .Input(m => m.Name, "CodeMashFoo")
                .Submit();

            Browser.VerifyPage<ConferenceController>(m => m.Index(0));

            Browser.ClickLink(SiteNav.EditPrefix + " CodeMashFoo");

            Browser.AssertValue<ConferenceShowModel>(m => m.Name, "CodeMashFoo");
        }
    }
}