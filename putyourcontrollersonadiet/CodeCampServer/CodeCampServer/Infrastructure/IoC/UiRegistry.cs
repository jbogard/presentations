using StructureMap.Configuration.DSL;

namespace CodeCampServerLite.Infrastructure.IoC
{
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
		}
	}
}