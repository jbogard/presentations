using System.Web.Security;
using CodeCampServerLite.UI.Areas.Security.Models;
using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.UI.Helpers
{
    public class UiRegistry : Registry
    {
        public UiRegistry()
        {
            For<IMembershipService>().Use<AccountMembershipService>();
            For<IFormsAuthenticationService>().Use<FormsAuthenticationService>();
            For<MembershipProvider>().Use(() => Membership.Provider);
        }
    }
}