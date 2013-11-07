using NHibernate;
using StructureMap;

namespace CodeCampServerLite.Infrastructure.DataAccess
{
	public class Sessions
	{
		public static ISession Current
		{
			get
			{
			    return null;
			}
		}
	}
}