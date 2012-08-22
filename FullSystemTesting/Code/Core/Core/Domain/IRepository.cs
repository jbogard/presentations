using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampServerLite.Core.Domain.Model;
using NHibernate.Linq;

namespace CodeCampServerLite.Core.Domain
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(Guid id);
        void Save(T entity);
        IEnumerable<T> GetAll();
        IQueryable<T> Query();
        void Delete(T entity);
    }
}
