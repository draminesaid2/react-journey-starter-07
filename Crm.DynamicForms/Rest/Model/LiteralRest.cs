namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Literal))]
	public class LiteralRest : DynamicFormElementRest
	{
	}
}
