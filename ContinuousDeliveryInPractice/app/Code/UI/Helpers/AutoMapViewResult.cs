using System;
using System.Web.Mvc;
using AutoMapper;

namespace CodeCampServerLite.UI.Helpers
{
    public class AutoMapViewResult : ActionResult
    {
        public Type SourceType { get; private set; }
        public Type DestinationType { get; private set; }
        public object DomainModel { get; private set; }
        public ViewResultBase View { get; private set; }

		public AutoMapViewResult(Type sourceType, Type destinationType, object domainModel, ViewResultBase view)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
		    DomainModel = domainModel;
            View = view;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var model = Mapper.Map(DomainModel, SourceType, DestinationType);

            View.ViewData.Model = model;

            View.ExecuteResult(context);
        }
    }
}