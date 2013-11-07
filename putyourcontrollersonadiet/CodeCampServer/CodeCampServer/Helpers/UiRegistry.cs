using System.Web.Security;
using CodeCampServerLite.Areas.Security.Models;
using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Helpers
{
    using Controllers;

    public class UiRegistry : Registry
    {
        public UiRegistry()
        {
			Scan(scanner => 
			{
				scanner.TheCallingAssembly();
				scanner.ConnectImplementationsToTypesClosing(typeof(IFormHandler<>));
			    scanner.AddAllTypesOf(typeof (IQueryHandler<,>));
			});
            For<IMembershipService>().Use<AccountMembershipService>();
            For<IFormsAuthenticationService>().Use<FormsAuthenticationService>();
            For<MembershipProvider>().Use(() => Membership.Provider);
        }
    }
}