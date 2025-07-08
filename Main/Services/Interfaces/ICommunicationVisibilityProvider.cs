namespace Crm.Services.Interfaces
{
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Model;
	using Crm.Model;

	public interface ICommunicationVisibilityProvider : IDependency
	{
		IQueryable<T> FilterByContactVisibility<T>(IQueryable<T> communications, User user)
			where T : Communication;
	}
}