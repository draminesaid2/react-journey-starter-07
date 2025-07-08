namespace Crm.BackgroundServices.Dropbox
{
	public enum IgnoreReason
	{
		CustomRule,
		NotSentToDropboxMailAddress,
		ForwardedMailDoesNotContainValidDropboxAddressInToHeader,
		ForwardedMailDoesNotContainValidDropboxAddressInBccHeader,
		MailContainsDropboxAddressInToOrCcHeader
	}
}