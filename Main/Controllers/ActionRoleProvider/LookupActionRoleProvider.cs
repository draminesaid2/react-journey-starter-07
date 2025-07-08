namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model;
	using Crm.Model.Lookups;

	public class LookupActionRoleProvider : RoleCollectorBase
	{
		public LookupActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.WebApi, nameof(AddressType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(BravoCategory), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(BusinessRelationshipType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(BusinessTitle), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag1), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag2), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag3), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag4), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyGroupFlag5), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyPersonRelationshipType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Country), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Currency), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DepartmentType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(EmailType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(FaxType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(InvoicingType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(LengthUnit), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Log), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(NoteType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(NumberOfEmployees), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PhoneType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Region), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Salutation), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(SalutationLetter), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(SourceType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(TaskType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(TimeUnit), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Title), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Turnover), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(WebsiteType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ZipCodeFilterType), Roles.APIUser);
			Add(PermissionGroup.WebApi, MainPlugin.PermissionName.Lookup);
		}
	}
}
