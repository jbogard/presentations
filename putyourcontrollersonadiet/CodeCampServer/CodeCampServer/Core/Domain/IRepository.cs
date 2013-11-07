using System;
using System.Collections.Generic;
using CodeCampServerLite.Core.Domain.Model;
using NHibernate.Linq;

namespace CodeCampServerLite.Core.Domain
{
    using Model;

    public interface IRepository<T> where T : Entity
    {
        T GetById(Guid id);
        void Save(T entity);
        IEnumerable<T> GetAll();
        INHibernateQueryable<T> Query();
        void Delete(T entity);
    }
}
