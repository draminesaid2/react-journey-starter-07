namespace Crm.DynamicForms.Rest.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	public class DynamicFormReferenceRest : RestEntityWithExtensionValues
	{
		public Guid DynamicFormKey { get; set; }
		public string LegacyId { get; set; }
		[NotReceived] public string ReferenceType { get; set; }
		public Guid? ReferenceKey { get; set; }
		public bool Completed { get; set; }
		[ExplicitExpand, NotReceived] public DynamicFormRest DynamicForm { get; set; }
		[ExplicitExpand, NotReceived] public IList<DynamicFormResponseRest> Responses { get; set; }
	}
}
