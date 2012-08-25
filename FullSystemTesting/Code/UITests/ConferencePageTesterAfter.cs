using CodeCampServerLite.UI;
using CodeCampServerLite.UI.Controllers;
using CodeCampServerLite.UI.Models;
using MvcContrib.TestHelper.Ui;
using NUnit.Framework;

namespace CodeCampServerLite.UITests
{
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
}