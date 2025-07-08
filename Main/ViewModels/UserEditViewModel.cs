namespace Crm.ViewModels
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Model;

	using LMobile.Unicore;

	using User = Crm.Library.Model.User;

	public class UserEditViewModel : CrmModelItem<User>
	{
		public string DropboxDomain { get; set; }
		public List<Permission> ActivePermissions { get; set; }
		public List<PermissionSchemaRole> ActiveRoles { get; set; }
		public List<Station> ActiveStations { get; set; }
		public int MinPasswordLength { get; set; }
		public bool UseActiveDirectoryAuthenticationService { get; set; }
		public bool PasswordResetSupported { get; set; }
		public List<PermissionSchemaRole> UserRoles { get; set; }
		public bool BlockHasAnyError(string displayRegion) => displayRegion != null && RuleViolations.Where(x => x.DisplayRegion != null).Any(x => x.DisplayRegion == displayRegion);
	}
}
