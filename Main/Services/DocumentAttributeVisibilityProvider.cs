namespace Crm.Services
{
	using System;
	using System.Linq;

	using Crm.Library.BaseModel;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class DocumentAttributeVisibilityProvider : IDocumentAttributeVisibilityProvider
	{
		private readonly IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository;
		public DocumentAttributeVisibilityProvider(IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository)
		{
			this.usergroupRepository = usergroupRepository;
		}

		public virtual IQueryable<DocumentAttribute> FilterByContactVisibility(IQueryable<DocumentAttribute> documentAttributes, User user)
		{
			var usergroupIds = usergroupRepository.GetAll().Where(x => x.Members.Any(y => y.Id == user.Id)).Select(x => x.Id);
			documentAttributes = documentAttributes.Where(
				x => x.Contact == null
						 || x.Contact.Visibility == Visibility.Everybody
						 || x.Contact.Visibility == Visibility.OnlyMe && x.Contact.CreateUser == user.Id
						 || x.Contact.Visibility == Visibility.UserGroups && x.Contact.VisibleToUsergroupIds.Any(ug => usergroupIds.Contains(ug))
						 || x.Contact.Visibility == Visibility.Users && x.Contact.VisibleToUserIds.Any(u => u == user.Id));
			return documentAttributes;
		}
	}
}
