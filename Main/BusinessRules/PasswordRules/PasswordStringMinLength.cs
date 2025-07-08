namespace Crm.BusinessRules.PasswordRules
{
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation.BaseRules;

	public class PasswordStringMinLength : MinLengthRule<string>
	{
		private readonly IAuthenticationService authenticationService;
		protected override bool IsIgnoredFor(string password)
		{
			return !authenticationService.PasswordChangeSupported || password == null || password.Length == 0;
		}

		public PasswordStringMinLength(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
			Init(p => p, 6, "Password");
		}
	}
}