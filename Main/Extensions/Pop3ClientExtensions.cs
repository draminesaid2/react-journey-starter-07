namespace Crm.Extensions
{
	using Crm.Library.Extensions;
	using MimeKit;
	using MimeKit.Text;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Mail;

	public static class Pop3ClientExtensions
	{
		public static MailMessage ToMailMessage(this MimeMessage mimeMessage)
		{
			MailMessage mailMessage = new MailMessage();
			mimeMessage.Headers.ForEach(x => mailMessage.Headers.Add(x.Id.ToHeaderName(), x.Value ?? "N/A"));
			mailMessage.From = mimeMessage.From.Mailboxes.Select(x => new MailAddress(x.Address)).First();
			mailMessage.To.AddRange(mimeMessage.To.Mailboxes.Select(x => new MailAddress(x.Address)));
			mailMessage.CC.AddRange(mimeMessage.Cc.Mailboxes.Select(x => new MailAddress(x.Address)));
			mailMessage.Bcc.AddRange(mimeMessage.Bcc.Mailboxes.Select(x => new MailAddress(x.Address)));
			mailMessage.Subject = mimeMessage.Subject;
			mailMessage.Body = mimeMessage.GetTextBody(MimeKit.Text.TextFormat.Plain);
			return mailMessage;
		}
		public static IEnumerable<MailAddress> ToMailAddresses(this InternetAddressList internetAddresses)
		{
			return internetAddresses.Mailboxes.Select(x => new MailAddress(x.Address, x.Name));
		}
		public static IEnumerable<string> GetAddresses(this InternetAddressList internetAddresses)
		{
			return internetAddresses.Mailboxes.Select(x => x.Address);
		}
		public static string ConvertHtmlToPlainText(this TextPart textPart)
		{
			TextConverter converter = new HtmlToHtml
			{
				HtmlTagCallback = (HtmlTagContext ctx, HtmlWriter htmlWriter) => { 
					ctx.DeleteEndTag = true;
					ctx.DeleteTag = true;
					if (ctx.TagId == HtmlTagId.Br)
						htmlWriter.WriteText("\r");
					return; 
				},
			};
			return converter.Convert(textPart.Text);
		}
	}
}