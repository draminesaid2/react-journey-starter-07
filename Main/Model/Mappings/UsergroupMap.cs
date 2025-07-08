namespace Crm.Model.Mappings
{
	using System;

	using Crm.Library.BaseModel.Mappings;
	using Crm.Library.Model;

	using NHibernate.Mapping.ByCode;

	public class UsergroupMap : EntityClassMapping<Usergroup>
	{
		public UsergroupMap()
		{
			Cache(x =>
			{
				x.Include(CacheInclude.All);
				x.Usage(CacheUsage.ReadWrite);
			});
			Schema("CRM");
			Table("Usergroup");

			Id(x => x.Id, map =>
			{
				map.Column("UserGroupId");
				map.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				map.UnsavedValue(Guid.Empty);
			});

			Property(x => x.Name);
			Set(x => x.Members, map =>
			{
				map.Schema("CRM");
				map.Table("UserUserGroup");
				map.Key(km => km.Column("UserGroupKey"));
				map.Fetch(CollectionFetchMode.Select);
				map.Cascade(Cascade.All.Include(Cascade.Persist));
			}, action => action.ManyToMany(map => map.Column("Username")));
		}
	}
}