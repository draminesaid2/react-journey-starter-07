namespace Crm.Services
{
	extern alias SystemConfigurationConfigurationManager;
	using System;
	using System.IO;
	using System.Net;
	using System.Net.Mail;
	using System.Xml;

	using SystemConfigurationConfigurationManager::System.Configuration;

	using Crm.Services.Interfaces;

	public class SmtpClientProvider : ISmtpClientProvider
	{
		private readonly string from;
		private readonly string host;
		private readonly int? port;
		private readonly string userName;
		private readonly string password;
		private readonly bool? enableSsl;
		private readonly string deliveryMethod;
		private readonly string pickupDirectoryLocation;

		public SmtpClientProvider()
		{
			var mailSettings = GetMailSettings();
			from = GetValueFromXml(mailSettings, "smtp", "from");
			host = GetValueFromXml(mailSettings, "network", "host");
			var portString = GetValueFromXml(mailSettings, "network", "port");
			port = portString != null ? int.Parse(portString) : null;
			userName = GetValueFromXml(mailSettings, "network", "userName");
			password = GetValueFromXml(mailSettings, "network", "password");
			var enableSslString = GetValueFromXml(mailSettings, "network", "enableSsl");
			enableSsl = enableSslString != null ? bool.Parse(enableSslString) : null;
			deliveryMethod = GetValueFromXml(mailSettings, "smtp", "deliveryMethod");
			pickupDirectoryLocation = GetValueFromXml(mailSettings, "specifiedPickupDirectory", "pickupDirectoryLocation");
		}

		protected virtual XmlDocument GetMailSettings()
		{
			var configMap = new ExeConfigurationFileMap { ExeConfigFilename = "web.config" };
			var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
			var systemNet = config.GetSection("system.net").SectionInformation.GetRawXml();
			var mailSettings = new XmlDocument();
			using var xmlReader = XmlReader.Create(new StringReader(systemNet));
			mailSettings.Load(xmlReader);
			return mailSettings;
		}

		protected virtual string GetValueFromXml(XmlDocument document, string tagName, string attributeName)
		{
			var elements = document.GetElementsByTagName(tagName);
			if (elements.Count == 0)
			{
				return null;
			}

			return elements[0]?.Attributes?[attributeName]?.Value;
		}

		public virtual MailMessage CreateMailMessage()
		{
			var mailMessage = new MailMessage();

			if (from != null)
			{
				mailMessage.From = new MailAddress(from);
			}

			return mailMessage;
		}

		public virtual SmtpClient CreateSmtpClient()
		{
			var smtpClient = new SmtpClient();

			if (host != null)
			{
				smtpClient.Host = host;
			}

			if (port != null)
			{
				smtpClient.Port = port.Value;
			}

			smtpClient.Credentials = new NetworkCredential(userName, password);

			if (enableSsl != null)
			{
				smtpClient.EnableSsl = enableSsl.Value;
			}

			if (deliveryMethod != null && deliveryMethod.Equals(nameof(SmtpDeliveryMethod.SpecifiedPickupDirectory), StringComparison.InvariantCultureIgnoreCase))
			{
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
				smtpClient.PickupDirectoryLocation = pickupDirectoryLocation;
			}

			smtpClient.DeliveryFormat = SmtpDeliveryFormat.International;

			return smtpClient;
		}
	}
}
