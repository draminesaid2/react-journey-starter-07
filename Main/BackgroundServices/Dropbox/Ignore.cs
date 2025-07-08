namespace Crm.BackgroundServices.Dropbox
{
	using System;
	using System.Linq;
	using System.Net.Mail;
	using System.Text.RegularExpressions;

	using Crm.Library.AutoFac;
	using Crm.Library.Extensions;

	public abstract class SubjectDropboxMessageIgnoreRule : DropboxMessageIgnoreRule
	{
		public override bool IsSatisfiedBy(MimeKit.MimeMessage message)
		{
			return Regex.Match(message.Subject, Pattern, RegexOptions.IgnoreCase).Success;
		}
		public SubjectDropboxMessageIgnoreRule(string pattern)
			: base("Subject", pattern)
		{
		}
	}

	public abstract class FromDropboxMessageIgnoreRule : DropboxMessageIgnoreRule
	{
		public override bool IsSatisfiedBy(MimeKit.MimeMessage message)
		{
			return message.From.Mailboxes.Any(x => Regex.Match(x.Address, Pattern, RegexOptions.IgnoreCase).Success);
		}
		public FromDropboxMessageIgnoreRule(string pattern)
			: base("From", pattern)
		{
		}
	}

	public abstract class DropboxMessageIgnoreRule : IDropboxMessageIgnoreRule
	{
		public virtual bool IsSatisfiedBy(MimeKit.MimeMessage message)
		{
			return message.Headers
							.Any(x => string.Equals(x.Value, Header, StringComparison.InvariantCultureIgnoreCase) && Regex.Match(x.Value, Pattern, RegexOptions.IgnoreCase).Success);
		}
		public virtual string Header { get; protected set; }
		public virtual string Pattern { get; protected set; }

		public DropboxMessageIgnoreRule(string header, string pattern)
		{
			Header = header;
			Pattern = pattern;
		}
	}

	public interface IDropboxMessageIgnoreRule : ISingletonDependency
	{
		/// <summary>
		/// Will be called to inspect the incoming message
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		bool IsSatisfiedBy(MimeKit.MimeMessage message);
		/// <summary>
		/// The header field of the message to inspect.
		/// </summary>
		string Header { get;  }
		/// <summary>
		/// A regular expression. When any matches of the expression occur, the message will be ignored
		/// </summary>
		string Pattern { get; }
	}

	public class Ignore
	{
		public Ignore(string header, DropboxMessage dropboxMessage)
		{
			Reason = IgnoreReason.CustomRule;
			Header = header;
			DropboxMessage = dropboxMessage;
		}
		public Ignore(IgnoreReason reason, DropboxMessage dropboxMessage)
		{
			Reason = reason;
			DropboxMessage = dropboxMessage;
		}
		public IgnoreReason Reason { get; set; }
		public string Header { get; set; }
		public DropboxMessage DropboxMessage { get; set; }
		public string LogMessage
		{
			get { return GetLogMessage(); }
		}
		public string NotificationMessage
		{
			get { return GetNotificationMessage(); }
		}
		// Methods
		private string GetLogMessage()
		{
			switch (Reason)
			{
				case IgnoreReason.CustomRule:
					return "E-Mail ignored according to custom rule {1}"
						.WithArgs(Header);
				case IgnoreReason.NotSentToDropboxMailAddress:
					return "E-mail received that was not addressed to a dropbox domain address.";
				case IgnoreReason.ForwardedMailDoesNotContainValidDropboxAddressInToHeader:
					return "Forwarded e-mail does not contain a valid dropbox address in To header as first recipient.";
				case IgnoreReason.MailContainsDropboxAddressInToOrCcHeader:
					return "Non-Forwarded e-mail contains a dropbox address in To oder CC header.";
				default:
					return "No log message defined for this ignore reason.";
			}
		}
		private string GetNotificationMessage()
		{
			switch (Reason)
			{
				case IgnoreReason.ForwardedMailDoesNotContainValidDropboxAddressInToHeader:
					return "When forwarding an e-mail to the dropbox agent, the dropbox address must be the first element in the recipients list.";
				case IgnoreReason.MailContainsDropboxAddressInToOrCcHeader:
					return "Non-forwarded e-mails must be sent to the dropbox agent as BCC.";
				default:
					return null;
			}
		}
	}
}
