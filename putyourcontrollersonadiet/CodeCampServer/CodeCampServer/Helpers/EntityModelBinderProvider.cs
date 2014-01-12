using System;
using System.Web.Mvc;
using CodeCampServerLite.Core.Domain;
using CodeCampServerLite.Core.Domain.Model;

namespace CodeCampServerLite.Helpers
{

	public class EntityModelBinder<TEntity> 
		: IModelBinder
		where TEntity : Entity
	{
		private readonly IRepository<TEntity> _repository;

		public EntityModelBinder(IRepository<TEntity> repository)
		{
			_repository = repository;
		}

		public object BindModel(ControllerContext controllerContext, 
			ModelBindingContext bindingContext)
		{
			ValueProviderResult value = bindingContext
				.ValueProvider.GetValue(bindingContext.ModelName);

			var id = Guid.Parse(value.AttemptedValue);

			var entity = _repository.GetById(id);

			return entity;
		}
	}
}