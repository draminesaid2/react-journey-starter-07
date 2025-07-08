namespace Crm.AttributeForms.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Model;

	using Newtonsoft.Json;

	public class AttributeForm : DynamicFormReference
	{
		[JsonIgnore]
		public virtual Contact Contact { get; set; }
	}
}
