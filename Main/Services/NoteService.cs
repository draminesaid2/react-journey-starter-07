namespace Crm.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Library.BaseModel;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Services.Interfaces;
	using Crm.Model.Notes;
	using Crm.Services.Interfaces;

	public class NoteService : INoteService
	{
		private readonly IAuthorizationManager authorizationManager;
		private readonly IEnumerable<INoteFilter> noteFilters;
		private readonly IUserService userService;
		public NoteService(IUserService userService, IAuthorizationManager authorizationManager, IEnumerable<INoteFilter> noteFilters)
		{
			this.userService = userService;
			this.authorizationManager = authorizationManager;
			this.noteFilters = noteFilters;
		}
		public virtual IQueryable<T> Filter<T>(IQueryable<T> notes)
			where T : Note
		{
			var user = userService.CurrentUser;

			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Visibility, PermissionName.SkipCheck) || !notes.Any())
			{
				return notes;
			}

			foreach (INoteFilter noteFilter in noteFilters)
			{
				notes = (IQueryable<T>)noteFilter.Filter(notes);
			}

			return FilterByContactVisibility(notes, user);
		}
		protected virtual IQueryable<T> FilterByContactVisibility<T>(IQueryable<T> notes, User user)
			where T : Note
		{
			var userGroupIds = user.Usergroups.Select(ug => ug.Id).ToList();

			// ReSharper disable ConvertClosureToMethodGroup
			// for NHibernate translations and better readability
			// This is a bit of a mouthful and for the moment it only supports nesting within second level.
			notes = from n in notes
				where n.ContactId == null
				      || (n.Contact != null && n.Contact.Visibility == Visibility.Everybody)
				      || (n.Contact != null && n.Contact.Visibility == Visibility.OnlyMe && n.Contact.CreateUser == user.Id)
				      || (n.Contact != null && n.Contact.Visibility == Visibility.UserGroups && n.Contact.VisibleToUsergroupIds.Any(ug => userGroupIds.Contains(ug)))
				      || (n.Contact != null && n.Contact.Visibility == Visibility.Users && n.Contact.VisibleToUserIds.Any(u => u == user.Id))
				      || (n.Contact != null && n.Contact.Parent != null && n.Contact.Parent.Visibility == Visibility.Everybody)
				      || (n.Contact != null && n.Contact.Parent != null && n.Contact.Parent.Visibility == Visibility.OnlyMe && n.Contact.Parent.CreateUser == user.Id)
				      || (n.Contact != null && n.Contact.Parent != null && n.Contact.Parent.Visibility == Visibility.UserGroups && n.Contact.Parent.VisibleToUsergroupIds.Any(ug => userGroupIds.Contains(ug)))
				      || (n.Contact != null && n.Contact.Parent != null && n.Contact.Parent.Visibility == Visibility.Users && n.Contact.Parent.VisibleToUserIds.Any(u => u == user.Id))
				select n;
			// ReSharper restore ConvertClosureToMethodGroup
			return notes;
		}
	}
}
