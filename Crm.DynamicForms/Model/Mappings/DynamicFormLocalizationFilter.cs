namespace Crm.DynamicForms.Model.Mappings
{
	using System;
	using System.Collections.Generic;

	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Data.Domain;

	using NHibernate.Engine;
	using NHibernate.Mapping.ByCode;
	using NHibernate.Type;

	public class DynamicFormLocalizationFilter : ISessionFilter
	{
		public const string Name = "DynamicFormLocalizationFilter";
		
		public static string Condition => $"({nameof(DynamicFormLocalization.DynamicFormElementId)} IS NULL)";
		public static Action<IFilterMapper> FilterMapping
		{
			get { return x => x.Condition(Condition); }
		}
		public virtual FilterDefinition Definition => new FilterDefinition(Name, Condition, new Dictionary<string, IType>(), true);
	}
}
