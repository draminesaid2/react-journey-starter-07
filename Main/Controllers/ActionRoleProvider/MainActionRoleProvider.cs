namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Globalization;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Lookup;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model;
	using Crm.Model.Lookups;
	using Crm.Model.Notes;
	using Crm.Model.Relationships;
	using Crm.Rest.Model;

	using LMobile.Unicore;

	using Email = Crm.Model.Email;
	using Fax = Crm.Model.Fax;
	using Phone = Crm.Model.Phone;
	using User = Crm.Library.Model.User;
	using Website = Crm.Model.Website;

	public class MainActionRoleProvider : RoleCollectorBase
	{
		public MainActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Dashboard, PermissionName.View, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.Dashboard, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(nameof(Log), PermissionName.Rss);
			Add(PermissionGroup.MaterialDashboard, PermissionName.View, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.MaterialDashboard, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.MaterialDashboard, PermissionName.Calendar, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(PermissionGroup.Login, PermissionName.Backend);
			Add(PermissionGroup.Login, PermissionName.MaterialClientOnline);
			Add(PermissionGroup.Login, PermissionName.MaterialClientOnline, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Index, PermissionGroup.WebApi, nameof(DocumentCategory));
			Add(MainPlugin.PermissionGroup.Note, PermissionName.Create, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Note, PermissionName.Delete, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Note, PermissionName.Delete, MainPlugin.PermissionGroup.Note, PermissionName.Edit);
			Add(MainPlugin.PermissionGroup.Note, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Note, PermissionName.Edit, MainPlugin.PermissionGroup.Note, PermissionName.Create);
			Add(MainPlugin.PermissionGroup.Note, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Note, PermissionName.View, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(PermissionGroup.File, PermissionName.GetContent, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.File, PermissionName.Delete, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.File, PermissionName.ThumbnailImage, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(PermissionGroup.Visibility, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.Visibility, PermissionName.SkipCheck, Roles.APIUser);

			Add(PermissionGroup.WebApi, nameof(Address), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Branch1), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Branch2), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Branch3), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Branch4), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Bravo), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(BusinessRelationship), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Company), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyPersonRelationship), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(CompanyBranch), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DocumentCategory), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.WebApi, nameof(DocumentAttribute), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DocumentGeneratorEntry));
			Add(PermissionGroup.WebApi, nameof(Domain), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Email), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(EntityType));
			Add(PermissionGroup.WebApi, nameof(Fax), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(FileResource), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(LinkResource), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Language), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Note), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(NumberOfEmployees), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentCondition), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentInterval), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(PaymentType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Permission), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.WebApi, nameof(PermissionSchemaRole), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.WebApi, nameof(Person), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Phone), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Station), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Tag), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Task), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Turnover), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(User), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.WebApi, nameof(UserNote), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Usergroup), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(UserStatus), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(Website), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);

			Add(PermissionGroup.WebApi, nameof(MainPlugin.PermissionGroup.BackgroundService));

		}
	}
}
