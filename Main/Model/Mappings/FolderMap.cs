namespace Crm.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class FolderMap : SubclassMapping<Folder>
	{
		public FolderMap()
		{
			DiscriminatorValue("Folder");
		}
	}
}