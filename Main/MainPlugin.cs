namespace Crm
{
	using Crm.Library.Configuration;
	using Crm.Library.Modularization;

	[Plugin(Requires = "")]
	public class MainPlugin : Plugin
	{
		public new static string Name = "Main";

		public static class Roles
		{
			public const string SalesBackOffice = "SalesBackOffice";
			public const string HeadOfSales = "HeadOfSales";
			public const string InternalSales = "InternalSales";
			public const string FieldSales = "FieldSales";
		}

		public static class PermissionGroup
		{
			public const string Contact = "Contact";
			public const string Address = "Address";
			public const string Bravo = "Bravo";
			public const string BusinessRelationship = "BusinessRelationship";
			public const string BackgroundService = "BackgroundService";
			public const string Category = "Category";
			public const string Company = "Company";
			public const string Person = "Person";
			public const string CompanyPersonRelationship = "CompanyPersonRelationship";
			public const string Note = "Note";
			public const string Site = "Site";
			public const string Task = "Task";
			public const string UserAccount = "UserAccount";
			public const string DocumentArchive = "DocumentArchive";
			public const string DocumentAttribute = "DocumentAttribute";
			public const string Dispatch = "Dispatch";
			public const string Branch = "Branch";
		}

		public static class PermissionName
		{
			public const string CreateTag = "CreateTag";
			public const string AssociateTag = "AssociateTag";
			public const string RenameTag = "RenameTag";
			public const string RemoveTag = "RemoveTag";
			public const string DeleteTag = "DeleteTag";

			public const string RelationshipsTab = "RelationshipsTab";
			public const string NotesTab = "NotesTab";
			public const string StaffTab = "StaffTab";
			public const string TasksTab = "TasksTab";
			public const string DocumentsTab = "DocumentsTab";

			public const string ImportFromVcf = "ImportFromVcf";
			public const string ExportAsVcf = "ExportAsVcf";
			public const string ExportAsCsv = "ExportAsCsv";
			public const string Rss = "Rss";
			public const string Ics = "Ics";
			public const string DownloadAsPdf = "DownloadAsPdf";

			public const string SetStatus = "SetStatus";
			public const string SetStatusMultiple = "SetStatusMultiple";
			public const string Merge = "Merge";

			public const string Deactivate = "Deactivate";
			public const string Activate = "Activate";
			public const string CreateAddress = "CreateAddress";
			public const string EditAddress = "EditAddress";
			public const string DeleteAddress = "DeleteAddress";
			public const string DeleteCommunication = "DeleteCommunication";
			public const string MakeStandardAddress = "MakeStandardAddress";
			public const string UpgradeProspect = "UpgradeProspect";
			public const string ToggleActive = "ToggleActive";
			public const string AddTask = "AddTask";
			public const string SeeAllUsersTasks = "SeeAllUsersTasks";

			public const string CreateBusinessRelationship = "CreateBusinessRelationship";
			public const string DeleteBusinessRelationship = "DeleteBusinessRelationship";
			public const string EditAddressRelationship = "EditAddressRelationship";
			public const string RemoveAddressRelationship = "RemoveAddressRelationship";

			public const string SidebarStaffList = "SidebarStaffList";
			public const string SidebarClientCompanies = "SidebarClientCompanies";
			public const string SidebarBravo = "SidebarBravo";
			public const string SidebarContactInfo = "SidebarContactInfo";
			public const string SidebarBackgroundInfo = "SidebarBackgroundInfo";
			public const string SidebarDropbox = "SidebarDropbox";
			public const string SidebarTasks = "SidebarTasks";
			public const string SidebarDocumentArchive = "SidebarDocumentArchive";

			public const string PublicHeaderInfo = "PublicHeaderInfo";
			public const string PrivateHeaderInfo = "PrivateHeaderInfo";
			public const string DetailsContactInfo = "DetailsContactInfo";
			public const string DetailsBackgroundInfo = "DetailsBackgroundInfo";
			public const string DetailsTechnicianInfo = "DetailsTechnicianInfo";

			public const string Complete = "Complete";
			public const string Close = "Close";

			public const string ExportDropboxAddressAsVCard = "ExportDropboxAddressAsVCard";
			public const string UpdateStatus = "UpdateStatus";

			public const string CreateRole = "CreateRole";
			public const string DeleteRole = "DeleteRole";
			public const string EditRole = "EditRole";
			public const string EditVisibility = "EditVisibility";

			public const string AddUser = "AddUser";
			public const string CreateUser = "CreateUser";
			public const string EditUser = "EditUser";
			public const string DeleteUser = "DeleteUser";

			public const string ListPermissions = "ListPermissions";
			public const string AddPermission = "AddPermission";
			public const string RemovePermission = "RemovePermission";
			public const string ListRoles = "ListRoles";
			public const string AddRole = "AddRole";
			public const string RemoveRole = "RemoveRole";
			public const string ListUsergroups = "ListUsergroups";
			public const string AddUserGroup = "AddUserGroup";
			public const string RemoveUserGroup = "RemoveUserGroup";

			public const string UserResetDropboxToken = "UserResetDropboxToken";
			public const string UserResetGeneralToken = "UserResetGeneralToken";
			public const string RefreshUserCache = "RefreshUserCache";
			public const string CreateUserGroup = "CreateUserGroup";
			public const string AssignUserGroup = "AssignUserGroup";
			public const string DeleteUserGroup = "DeleteUserGroup";
			public const string EditUserGroup = "EditUserGroup";
			public const string SaveUserGroup = "SaveUserGroup";
			public const string CreateStation = "CreateStation";
			public const string EditStation = "EditStation";
			public const string DeleteStation = "DeleteStation";
			public const string AssignRole = "AssignRole";
			public const string AssignSkill = "AssignSkill";
			public const string SignalR = "SignalR";

			public const string RequestLocalStorage = "RequestLocalStorage";
			public const string RequestLocalDatabase = "RequestLocalDatabase";
			public const string SetLogLevel = "SetLogLevel";
			public const string ViewLocalStorageLog = "ViewLocalStorageLog";
			public const string ViewLocalDatabaseLog = "ViewLocalDatabaseLog";
			public const string SendJavaScriptCommand = "SendJavaScriptCommand";
			public const string UserDetail = "UserDetail";
			public const string ResetUserPassword = "ResetUserPassword";

			public const string Lookup = "Lookup";
		}

		public static class Settings
		{
			public static class Address
			{
				public static SettingDefinition<bool> AllowEditAddressesWithLegacyId => new SettingDefinition<bool>("AllowEditAddressWithLegacyId", Name);
				public static SettingDefinition<bool> DisplayOnlyRegionKey => new SettingDefinition<bool>("Address/DisplayOnlyRegionKey", Name);
			}

			public static class Maintenance
			{
				public static SettingDefinition<int> ReplicatedClientDeprecationDays => new SettingDefinition<int>("Maintenance/ReplicatedClientDeprecationDays", Name);
				public static SettingDefinition<int> PostingDeprecationDays => new SettingDefinition<int>("Maintenance/PostingDeprecationDays", Name);
				public static SettingDefinition<int> LogDeprecationDays => new SettingDefinition<int>("Maintenance/LogDeprecationDays", Name);
				public static SettingDefinition<int> ErrorLogDeprecationDays => new SettingDefinition<int>("Maintenance/ErrorLogDeprecationDays", Name);
				public static SettingDefinition<int> MessageDeprecationDays => new SettingDefinition<int>("Maintenance/MessageDeprecationDays", Name);
				public static SettingDefinition<int> FragmentationLevel1 => new SettingDefinition<int>("Maintenance/FragmentationLevel1", Name);
				public static SettingDefinition<int> FragmentationLevel2 => new SettingDefinition<int>("Maintenance/FragmentationLevel2", Name);
				public static SettingDefinition<int> AmountOfRecentPagesToKeep => new SettingDefinition<int>("Maintenance/AmountOfRecentPagesToKeep", Name);
				public static SettingDefinition<int> CommandTimeout => new SettingDefinition<int>("Maintenance/CommandTimeout", Name);
			}
			public static class Bravo
			{
				public static SettingDefinition<bool> EnabledForCompanies => new SettingDefinition<bool>("Configuration/BravoActiveForCompanies", Name);
				public static SettingDefinition<bool> EnabledForPersons => new SettingDefinition<bool>("Configuration/BravoActiveForPersons", Name);
			}

			public static class Company
			{
				public static SettingDefinition<bool> AllowEditCompanyWithLegacyId => new SettingDefinition<bool>("AllowEditCompanyWithLegacyId", Name);
				public static SettingDefinition<bool> AllowCompanyTypeSelection => new SettingDefinition<bool>("AllowCompanyTypeSelection", Name);
			}
			
			public static class Cordova
			{
				public static SettingDefinition<string> AndroidServiceAppLink => new SettingDefinition<string>("Cordova/AndroidServiceAppLink", Name);
				public static SettingDefinition<string> AppleIosServiceAppLink => new SettingDefinition<string>("Cordova/AppleIosServiceAppLink", Name);
				public static SettingDefinition<string> Windows10ServiceAppLink => new SettingDefinition<string>("Cordova/Windows10ServiceAppLink", Name);
				public static SettingDefinition<string> AndroidSalesAppLink => new SettingDefinition<string>("Cordova/AndroidSalesAppLink", Name);
				public static SettingDefinition<string> AppleIosSalesAppLink => new SettingDefinition<string>("Cordova/AppleIosSalesAppLink", Name);
				public static SettingDefinition<string> Windows10SalesAppLink => new SettingDefinition<string>("Cordova/Windows10SalesAppLink", Name);
			}

			public static class Dropbox
			{
				public static SettingDefinition<string> DropboxDomain => new SettingDefinition<string>("DropboxDomain", Name);
				public static SettingDefinition<bool> DropboxLogMessages => new SettingDefinition<bool>("DropboxLogMessages", Name);
				public static SettingDefinition<string[]> DropboxForwardPrefixes => new SettingDefinition<string[]>("DropboxForwardPrefixes", Name);
				public static SettingDefinition<int> MinFileSizeInBytes => new SettingDefinition<int>("MinFileSizeInBytes", Name);
			}

			public static class Email
			{
				public static SettingDefinition<bool> SenderImpersonation => new SettingDefinition<bool>("Email/SenderImpersonation", Name);
			}
			
			public static class FileResource
			{
				public static SettingDefinition<string[]> ContentTypesOpenedWithoutSandbox => new SettingDefinition<string[]>("FileResource/ContentTypesOpenedWithoutSandbox", Name);
			}

			public static class Geocoder
			{
				public static SettingDefinition<string> GeocoderService => new SettingDefinition<string>("Geocoder/GeocoderService", Name);
				public static SettingDefinition<string> GoogleMapsApiKey => new SettingDefinition<string>("Geocoder/GoogleMapsApiKey", Name);
				public static SettingDefinition<string> MapQuestApiKey => new SettingDefinition<string>("Geocoder/MapQuestApiKey", Name);
				public static SettingDefinition<string> BingMapsApiKey => new SettingDefinition<string>("Geocoder/BingMapsApiKey", Name);
				public static SettingDefinition<string> YahooMapsApiKey => new SettingDefinition<string>("Geocoder/YahooMapsApiKey", Name);
				public static SettingDefinition<string> YahooMapsApiSecret => new SettingDefinition<string>("Geocoder/YahooMapsApiSecret", Name);
				public static SettingDefinition<int> BatchSize => new SettingDefinition<int>("Geocoder/Address/BatchSize", Name);
			}

			public static class Person
			{
				public static SettingDefinition<bool> AllowEditPersonWithLegacyId => new SettingDefinition<bool>("AllowEditPersonWithLegacyId", Name);
				public static SettingDefinition<bool> DepartmentIsLookup => new SettingDefinition<bool>("Person/DepartmentIsLookup", Name);
				public static SettingDefinition<bool> BusinessTitleIsLookup => new SettingDefinition<bool>("Person/BusinessTitleIsLookup", Name);
			}

			public static class Posting
			{
				public static SettingDefinition<int> MaxRetries => new SettingDefinition<int>("Posting/MaxRetries", Name);
				public static SettingDefinition<int> RetryAfter => new SettingDefinition<int>("Posting/RetryAfter", Name);
			}

			public static class PushNotification
			{
				public static SettingDefinition<bool> Enabled => new SettingDefinition<bool>("PushNotification/Enabled", Name);
				public static SettingDefinition<string> Configuration => new SettingDefinition<string>("PushNotification/Configuration", Name);
				public static SettingDefinition<string> FCMServerKey => new SettingDefinition<string>("PushNotification/FCMServerKey", Name);
			}

			public static class Report
			{
				public static SettingDefinition<float> HeaderHeight => new SettingDefinition<float>("Report/HeaderHeight", Name);
				public static SettingDefinition<float> HeaderMargin => new SettingDefinition<float>("Report/HeaderSpacing", Name);
				public static SettingDefinition<float> FooterHeight => new SettingDefinition<float>("Report/FooterHeight", Name);
				public static SettingDefinition<float> FooterMargin => new SettingDefinition<float>("Report/FooterSpacing", Name);
			}

			public static class Search
			{
				public static SettingDefinition<bool> CompanyGroupFlagsAreSearchable => new SettingDefinition<bool>("CompanyGroupFlags/AreSearchable", Name);
				public static SettingDefinition<bool> LegacyNameIsDefault => new SettingDefinition<bool>("Lucene/LegacyNameIsDefault", Name);
			}

			public static class System
			{
				public static SettingDefinition<string> CefToPdfPath => new SettingDefinition<string>("CefToPdfPath", Name);
				public static SettingDefinition<string> RedisConfiguration => new SettingDefinition<string>("RedisConfiguration", Name);
				public static SettingDefinition<bool> SoftDelete => new SettingDefinition<bool>("SoftDelete", Name);
				public static SettingDefinition<bool> UseActiveDirectoryAuthenticationService => new SettingDefinition<bool>("UseActiveDirectoryAuthenticationService", Name);
				public static SettingDefinition<string> ActiveDirectoryEndpoint => new SettingDefinition<string>("ActiveDirectoryEndpoint", Name);

				public static class OpenIdAuthentication
				{
					public static SettingDefinition<bool> UseOpenIdAuthentication => new SettingDefinition<bool>("UseOpenIdAuthentication", Name);
				}

				public static class Maps
				{
					public static SettingDefinition<string> MapTileLayerUrl => new SettingDefinition<string>("MapTileLayerUrl", Name);
				}
				public static SettingDefinition<int> MaxFileLengthInKb => new SettingDefinition<int>("MaxFileLengthInKb", Name);
			}
    }
	}
}
