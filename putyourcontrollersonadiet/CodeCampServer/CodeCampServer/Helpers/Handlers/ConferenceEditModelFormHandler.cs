namespace CodeCampServerLite.Helpers.Handlers
{
    using Core.Domain;
    using Core.Domain.Model;
    using Models;

    public class ConferenceEditModelFormHandler
        : IFormHandler<ConferenceEditModel>
    {
        private readonly IConferenceRepository _repository;

        public ConferenceEditModelFormHandler(
            IConferenceRepository repository)
        {
            _repository = repository;
        }

        public void Handle(ConferenceEditModel form)
        {
            Conference conf = _repository.GetById(form.Id);

            conf.ChangeName(form.Name);

            foreach (var attendeeEditModel in GetAttendeeForms(form))
            {
                Attendee attendee = conf.GetAttendee(attendeeEditModel.Id);

                attendee.ChangeName(attendeeEditModel.FirstName,
                    attendeeEditModel.LastName);
                attendee.Email = attendeeEditModel.Email;
            }
        }

        private ConferenceEditModel.AttendeeEditModel[] GetAttendeeForms(ConferenceEditModel form)
        {
            return form.Attendees ??
                   new ConferenceEditModel.AttendeeEditModel[0];
        }
    }
}