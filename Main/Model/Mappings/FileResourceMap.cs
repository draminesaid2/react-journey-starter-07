namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	using GuidCombGeneratorDef = LMobile.Unicore.NHibernate.GuidCombGeneratorDef;

	public class FileResourceMap : EntityClassMapping<FileResource>
	{
		public FileResourceMap()
		{
			Schema("CRM");
			Table("FileResource");

			Id(
				x => x.Id,
				map =>
				{
					map.Column("Id");
					map.Generator(GuidCombGeneratorDef.Instance);
					map.UnsavedValue(Guid.Empty);
				});

			Property(
				x => x.ParentId,
				map =>
				{
					map.Column("ElementKey");
					map.Insert(true);
					map.Update(true);
				});

			Property(x => x.Filename);
			Property(x => x.Path);
			Property(x => x.Length);
			Property(x => x.OfflineRelevant);
			Property(x => x.ContentType);
			Property(x => x.IsActive);
			Property(
				x => x.Content,
				map =>
				{
					map.Length(4194304);
					map.Lazy(true);
					map.Update(false);
				});
			this.EntitySet(
				x => x.DocumentAttributes,
				map =>
				{
					map.Key(km => km.Column("FileResourceKey"));
					map.Fetch(CollectionFetchMode.Select);
					map.Lazy(CollectionLazy.Lazy);
					map.Cascade(Cascade.Persist);
					map.Inverse(true);
				},
				action => action.OneToMany());
		}
	}
}