namespace Crm.Services.Interfaces
{
	using Crm.BackgroundServices.Dropbox;
	using Crm.Library.AutoFac;
	using MimeKit;

	public interface IDropboxMessageService : ITransientDependency
	{
		bool TryParseMessage(MimeMessage message, out DropboxMessage dropboxMessage, string dropboxMail = null);
		bool Save(DropboxMessage dropboxMessage, MimeMessage message = null);
		void NotifyUser(DropboxMessage dropboxMessage);
	}
}
