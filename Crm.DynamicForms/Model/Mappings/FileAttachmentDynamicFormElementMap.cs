namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class FileAttachmentDynamicFormElementMap : SubclassMapping<FileAttachmentDynamicFormElement>
	{
		public FileAttachmentDynamicFormElementMap()
		{
			DiscriminatorValue(FileAttachmentDynamicFormElement.DiscriminatorValue);
			Property(x => x.Required);

			Property(x => x.MinUploadCount, m => m.Column("Min"));
			Property(x => x.MaxUploadCount, m => m.Column("Max"));

			Property(x => x.MaxImageWidth);
			Property(x => x.MaxImageHeight);

			Property(x => x.MaxFileSize);
		}
	}
}