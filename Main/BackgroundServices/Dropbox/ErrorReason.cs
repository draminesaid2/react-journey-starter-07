namespace Crm.BackgroundServices.Dropbox
{
	public enum ErrorReason
	{
		None,
		NoTextPlainPart,
		NoTextFound,
		InvalidDropboxAddress,
		InvalidEntityId,
		EntityIdDoesNotExist,
		NoUserCorrespondingToDropboxToken,
		NoContactMailAddressFound,
		NoContactCorrespondingToContactMailAddress,
		NoContactCorrespondingToContactId,
		ForwardedWithoutMailAddressInBody,
		ForwardedWithoutValidDropboxAddress,
		MailWithoutBccOrXEnvelopeOrReceivedHeader,
		InvalidUserMailAddress
	}
}