using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Core.Domain
{
    using Model;

    public interface IConferenceRepository : IRepository<Conference>
    {
        Conference GetByName(string eventName);
    }
}