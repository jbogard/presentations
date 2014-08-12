using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
    using System.Web.Security;
    using Areas.Security.Models;

    public class UiRegistry : Registry
	{
		public UiRegistry()
		{
			Scan(cfg =>
			{
				cfg.Assembly("CodeCampServerLite");
                cfg.LookForRegistries();
                cfg.WithDefaultConventions();
			});
            For<IMembershipService>().Use<AccountMembershipService>();
            For<IFormsAuthenticationService>().Use<FormsAuthenticationService>();
            For<MembershipProvider>().Use(() => Membership.Provider);

		}
	}
}