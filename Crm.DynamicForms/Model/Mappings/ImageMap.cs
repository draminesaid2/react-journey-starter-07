namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class ImageMap : SubclassMapping<Image>
	{
		public ImageMap()
		{
			DiscriminatorValue(Image.DiscriminatorValue);

			Property(x => x.FileResourceId);
		}
	}
}