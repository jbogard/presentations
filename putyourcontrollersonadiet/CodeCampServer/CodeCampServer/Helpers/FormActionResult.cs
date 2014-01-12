using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace CodeCampServerLite.Helpers
{
	public class FormActionResult<T> : ActionResult
	{		
		public ViewResult Failure { get; private set; }
		public ActionResult Success { get; private set; }
		public T Form { get; private set; }

		public FormActionResult(T form, ActionResult success, ViewResult failure)
		{
			Form = form;
			Success = success;
			Failure = failure;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (!context.Controller.ViewData.ModelState.IsValid)
			{
				Failure.ExecuteResult(context);

				return;
			}

			var handler = ObjectFactory.GetInstance<IFormHandler<T>>();

			handler.Handle(Form);

			Success.ExecuteResult(context);
		}
	}
}