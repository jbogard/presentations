using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;
using NHibernate;
using NHibernate.Linq;

namespace CodeCampServerLite.Infrastructure.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : Entity
    {
        protected Repository(ISession session)
        {
            Session = session;
        }

        protected ISession Session { get; private set; }

        public T GetById(Guid id)
        {
            return Session.Get<T>(id);
        }

        public void Save(T entity)
        {
            Session.Save(entity);
        }

        public IEnumerable<T> GetAll()
        {
            var criteria = Session.CreateCriteria<T>();

            return criteria.List<T>();
        }

        public IQueryable<T> Query()
        {
            return Session.Query<T>();
        }

        public void Delete(T entity)
        {
            Session.Delete(entity);
        }
    }
}