namespace Crm.Services.Interfaces
{
	using System.Net.Mail;

	using Crm.Library.AutoFac;

	public interface ISmtpClientProvider : ISingletonDependency
	{
		public MailMessage CreateMailMessage();
		public SmtpClient CreateSmtpClient();
	}
}
