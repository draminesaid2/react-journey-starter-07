namespace Crm.Services
{
	using System;
	using System.Linq;

	using Crm.Library.BaseModel;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class CommunicationVisibilityProvider : ICommunicationVisibilityProvider
	{
		private readonly IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository;
		public CommunicationVisibilityProvider(IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository)
		{
			this.usergroupRepository = usergroupRepository;
		}
		public virtual IQueryable<T> FilterByContactVisibility<T>(IQueryable<T> communications, User user)
			where T : Communication
		{
			var usergroupIds = usergroupRepository.GetAll().Where(x => x.Members.Any(y => y.Id == user.Id)).Select(x => x.Id);
			communications = communications.Where(
				x => x.Contact == null
				     || x.Contact.Visibility == Visibility.Everybody
				     || x.Contact.Visibility == Visibility.OnlyMe && x.Contact.CreateUser == user.Id
				     || x.Contact.Visibility == Visibility.UserGroups && x.Contact.VisibleToUsergroupIds.Any(ug => usergroupIds.Contains(ug))
				     || x.Contact.Visibility == Visibility.Users && x.Contact.VisibleToUserIds.Any(u => u == user.Id));
			return communications;
		}
	}
}
