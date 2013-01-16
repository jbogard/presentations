using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
	public class SessionMap : EntityMap<Session>
	{
		public SessionMap()
		{
			Map(x => x.Title);
			Map(x => x.Abstract).Length(NVarCharMax);

			References(x => x.Speaker).Cascade.SaveUpdate();
			References(x => x.Conference);
		}
	}
}