namespace Crm.BusinessRules.UserRules
{
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation.BaseRules;
	using Crm.Services;

	public class AdNameRequired : RequiredRule<User>
	{
		private readonly IAuthenticationService authenticationService;
		protected override bool IsIgnoredFor(User user)
		{
			return !(authenticationService is ActiveDirectoryAuthenticationService);
		}

		public AdNameRequired(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
			Init(u => u.AdName);
		}
	}
}