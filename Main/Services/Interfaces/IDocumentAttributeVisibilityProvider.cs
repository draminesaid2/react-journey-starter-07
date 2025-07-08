namespace Crm.Services.Interfaces
{
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Model;
	using Crm.Model;

	public interface IDocumentAttributeVisibilityProvider : IDependency
	{
		IQueryable<DocumentAttribute> FilterByContactVisibility(IQueryable<DocumentAttribute> documentAttributes, User user);
	}
}