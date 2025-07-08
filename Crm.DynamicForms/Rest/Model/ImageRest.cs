namespace Crm.DynamicForms.Rest.Model
{
	using System;

	using Crm.DynamicForms.Model;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Image))]
	public class ImageRest : DynamicFormElementRest
	{
		public Guid? FileResourceId { get; set; }
	}
}