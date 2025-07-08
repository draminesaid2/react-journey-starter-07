namespace Crm.BusinessRules.PasswordRules
{
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation.BaseRules;

	public class PasswordMinLength : MinLengthRule<User>
	{
		private readonly IAuthenticationService authenticationService;
		protected override bool IsIgnoredFor(User user)
		{
			return !authenticationService.PasswordChangeSupported || user.Password == null || user.Password.Length == 0;
		}

		public PasswordMinLength(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
			Init(u => u.Password, 6);
		}
	}
}