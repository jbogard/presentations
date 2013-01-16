using CodeCampServerLite.Core.Domain.Model;
using FluentNHibernate.Mapping;

namespace CodeCampServerLite.Infrastructure.DataAccess.Maps
{
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
