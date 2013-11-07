using System.Web.Mvc;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;
using StructureMap;

namespace CodeCampServerLite.Helpers
{
    public class ConferenceModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {          
			ValueProviderResult value =
                bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var conferenceRepository =
                ObjectFactory.GetInstance<IConferenceRepository>();

            Conference conference =
                conferenceRepository.GetByName(value.AttemptedValue);

            return conference;
        }
    }
}