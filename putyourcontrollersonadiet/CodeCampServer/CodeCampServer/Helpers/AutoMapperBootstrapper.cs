using AutoMapper;
using CodeCampServerLite.Core.Domain.Model;
using CodeCampServerLite.Models;

namespace CodeCampServerLite.Helpers
{
    using Models;

    public static class AutoMapperBootstrapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ViewModelProfile>();
                cfg.AddProfile<EditModelProfile>();
            });
        }
	}

    public class ViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Conference, ConferenceListModel>();
			CreateMap<Conference, ConferenceShowModel>();
			CreateMap<Session, ConferenceShowModel.SessionModel>();
			CreateMap<Attendee, ConferenceShowModel.AttendeeModel>();
        }
    }

    public class EditModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Conference, ConferenceEditModel>();
            CreateMap<Attendee, ConferenceEditModel.AttendeeEditModel>();
        }
    }


}