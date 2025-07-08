namespace Crm.Rest.Model
{
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Crm.Model.Email))]
	public class Email : Communication
	{
	}
}