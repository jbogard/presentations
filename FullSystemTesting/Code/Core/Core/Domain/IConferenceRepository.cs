using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Core.Domain
{
    public interface IConferenceRepository : IRepository<Conference>
    {
        Conference GetByName(string eventName);
    }
}