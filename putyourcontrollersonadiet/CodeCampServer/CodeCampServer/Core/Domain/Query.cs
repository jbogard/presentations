using System;
using NHibernate.Linq;

namespace CodeCampServerLite.Core.Domain
{
	public interface IQuery
	{
		object Execute(INHibernateQueryable queryProvider);
	}

	public abstract class Query<TEntity, TResult> : IQuery
	{
		public abstract TResult Execute(INHibernateQueryable<TEntity> queryProvider);
		
		object IQuery.Execute(INHibernateQueryable queryProvider)
		{
			return Execute((INHibernateQueryable<TEntity>) queryProvider);
		}
	}
}