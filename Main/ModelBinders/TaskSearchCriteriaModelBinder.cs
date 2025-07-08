namespace Crm.ModelBinders
{
	using System;

	using Crm.Library.ModelBinder;
	using Crm.SearchCriteria;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ModelBinding;

	[ModelBinderFor(typeof(TaskSearchCriteria))]
	public class TaskSearchCriteriaModelBinder : TimeSpanSearchCriteriaModelBinder, ICrmModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var criteria = base.BindModel(controllerContext, bindingContext) as TaskSearchCriteria;

			if (criteria == null)
			{
				return null;
			}


			criteria.ResponsibleUser = GetValue<string>(bindingContext, "filter_responsible");
			if (String.IsNullOrEmpty(criteria.ResponsibleUser))
			{
				criteria.ResponsibleUser = GetValue<string>(bindingContext, "responsibleUser");
			}

			criteria.GroupBy = GetValue<string>(bindingContext, "GroupBy");

			criteria.UserGroup = GetValue<Guid>(bindingContext, "filter_group");
			if (criteria.UserGroup == default(Guid))
			{
				criteria.UserGroup = GetValue<Guid>(bindingContext, "userGroup");
			}

			return criteria;
		}
	}
}