namespace CodeCampServerLite.UITests
{
    using MvcContrib.TestHelper.Ui;
    using UI;
    using UI.Controllers;
    using UI.Models;
    using Xunit;

    public class ConferenceEditingTests : WebTestBase
    {
        [Fact]
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