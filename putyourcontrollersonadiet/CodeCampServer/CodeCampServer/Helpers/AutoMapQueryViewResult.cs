using System;
using System.Web.Mvc;
using AutoMapper;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;
using StructureMap;

namespace CodeCampServerLite.Helpers
{
    public class AutoMapQueryViewResult<TSourceType, TQueryResultType, TDestinationType> : ActionResult 
		where TSourceType : Entity
    {
        public ViewResult View { get; private set; }

		public AutoMapQueryViewResult(ViewResult view)
        {
            View = view;
        }

        public override void ExecuteResult(ControllerContext context)
        {
        	var query = (Query<TSourceType, TQueryResultType>) View.ViewData.Model;

			var repository = ObjectFactory.GetInstance<IRepository<TSourceType>>();

        	var result = query.Execute(repository.Query());

			var model = Mapper.Map<TQueryResultType, TDestinationType>(result);

            View.ViewData.Model = model;

            View.ExecuteResult(context);
        }
    }
}