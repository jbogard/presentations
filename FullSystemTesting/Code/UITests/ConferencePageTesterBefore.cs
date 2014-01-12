using System;
using System.Threading;
using NUnit.Framework;
using Should;
using WatiN.Core;

namespace CodeCampServerLite.UITests
{
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