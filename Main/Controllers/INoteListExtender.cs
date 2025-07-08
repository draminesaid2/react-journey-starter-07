namespace Crm.Controllers
{
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Model;

	public interface IRelatedContact : ITransientDependency
	{
		IQueryable<Contact> RelatedContact(Contact contact);
	}
	public interface IRelatedContact<T> : IRelatedContact
	{

	}
}