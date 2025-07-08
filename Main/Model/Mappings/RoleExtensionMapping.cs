namespace Crm.Model.Mappings
{
	using Crm.Model.Extension;

	using LMobile.Unicore;
	using LMobile.Unicore.NHibernate;

	using NHibernate.Mapping.ByCode.Conformist;

	public class RoleExtensionMapping : ComponentMapping<PermissionSchemaRoleExtension>, INHibernateMappingExtension<PermissionSchemaRole, PermissionSchemaRoleExtension>
	{
		public RoleExtensionMapping()
		{
			Property(x => x.Description);
		}
	}
}
