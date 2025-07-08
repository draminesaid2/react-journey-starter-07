namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model;
	using Crm.Model.Lookups;

	public class CompanyActionRoleProvider : RoleCollectorBase
	{
		public CompanyActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Company, PermissionName.View, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, PermissionName.Read, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, MainPlugin.PermissionGroup.Company, PermissionName.Index);
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(Branch1));
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(Branch2));
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(Branch3));
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(Branch4));
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(Company));
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(CompanyBranch));
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(NumberOfEmployees));
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Read, PermissionGroup.WebApi, nameof(Turnover));
			Add(MainPlugin.PermissionGroup.Company, PermissionName.Create, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Create, MainPlugin.PermissionGroup.Company, PermissionName.Read);
			Add(MainPlugin.PermissionGroup.Company, PermissionName.Delete, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Delete, MainPlugin.PermissionGroup.Company, PermissionName.Edit);
			Add(MainPlugin.PermissionGroup.Company, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, PermissionName.Edit, MainPlugin.PermissionGroup.Company, PermissionName.Create);

			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.ToggleActive, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.ToggleActive, MainPlugin.PermissionGroup.Company, PermissionName.Edit);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.Merge, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.Merge, MainPlugin.PermissionGroup.Company, PermissionName.Edit);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.MakeStandardAddress, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.MakeStandardAddress, MainPlugin.PermissionGroup.Company, PermissionName.Edit);

			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.CreateTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.AssociateTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.RenameTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.RenameTag, MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DeleteTag);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.RemoveTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.RemoveTag, MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.AssociateTag);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DeleteTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DeleteTag, MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.CreateTag);
		}
	}
}