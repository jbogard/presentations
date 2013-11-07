using System;
using System.Web.Mvc;
using AutoMapper;

namespace CodeCampServerLite.Helpers
{
    public class AutoMapViewResult : ActionResult
    {     
		public Type SourceType { get; private set; }
        public Type DestinationType { get; private set; }
        public ViewResult View { get; private set; }

        public AutoMapViewResult(Type sourceType, Type destinationType, ViewResult view)
        {
            SourceType = sourceType;
            DestinationType = destinationType;
            View = view;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var model = Mapper.Map(View.ViewData.Model, SourceType, DestinationType);

            View.ViewData.Model = model;

            View.ExecuteResult(context);
        }
    }
}