using FluentNHibernate.Mapping;
using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
    using Core.Domain.Model;

    public abstract class EntityMap<TEntity> : ClassMap<TEntity>
        where TEntity : Entity
    {
        protected const int NVarCharMax = 4001;

        protected EntityMap()
        {
            Id(x => x.Id);
        }
    }
}
