namespace Crm.Offline.Controllers.ActionRoleProvider
{
	using System.Collections.Generic;

	using Crm.Article;
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Article.Model.Relationships;
	using Crm.AttributeForms;
	using Crm.AttributeForms.Model;
	using Crm.Campaigns;
	using Crm.Campaigns.Model;
	using Crm.Campaigns.Model.Lookups;
	using Crm.Configurator;
	using Crm.Configurator.Model;
	using Crm.Configurator.Model.Lookups;
	using Crm.DynamicForms;
	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.ErpExtension;
	using Crm.ErpExtension.Model;
	using Crm.ErpExtension.Model.Lookups;
	using Crm.Library.Globalization;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Lookup;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;
	using Crm.MarketInsight;
	using Crm.MarketInsight.Model;
	using Crm.MarketInsight.Model.Lookups;
	using Crm.MarketInsight.Model.Relationships;
	using Crm.Model;
	using Crm.Model.Lookups;
	using Crm.Model.Notes;
	using Crm.Model.Relationships;
	using Crm.Offline.Extensions;
	using Crm.Order;
	using Crm.Order.Model;
	using Crm.Order.Model.Lookups;
	using Crm.PerDiem;
	using Crm.PerDiem.Germany;
	using Crm.PerDiem.Germany.Model;
	using Crm.PerDiem.Germany.Model.Lookups;
	using Crm.PerDiem.Model;
	using Crm.PerDiem.Model.Lookups;
	using Crm.Project;
	using Crm.Project.Model;
	using Crm.Project.Model.Lookups;
	using Crm.Project.Model.Relationships;
	using Crm.Service;
	using Crm.Service.Model;
	using Crm.Service.Model.Lookup;
	using Crm.Service.Model.Relationships;
	using Crm.VisitReport;
	using Crm.VisitReport.Lookups;
	using Crm.VisitReport.Model;
	using Crm.VisitReport.Model.Relationships;

	using LMobile.Unicore;

	using Main.Multitenant;
	using Main.Replication.Model;
	using Sms.Checklists;
	using Sms.Checklists.Model;
	using Sms.Einsatzplanung.Connector;
	using Sms.Einsatzplanung.Connector.Model;
	using Sms.Einsatzplanung.Team;
	using Sms.Einsatzplanung.Team.Model;
	using Sms.TimeManagement;
	using Sms.TimeManagement.Model;
	using Sms.TimeManagement.Model.Lookup;

	using User = Crm.Library.Model.User;

	public class SyncActionRoleProvider : RoleCollectorBase
	{
		public SyncActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var fieldSales = MainPlugin.Roles.FieldSales;
			var fieldService = ServicePlugin.Roles.FieldService;
			var activePluginNames = new HashSet<string>(pluginProvider.ActivePluginNames);

			if (activePluginNames.Contains(ArticlePlugin.PluginName))
			{
				this.AddSyncPermission(nameof(Article), fieldService);
				this.AddSyncPermission(nameof(ArticleDescription), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup01), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup02), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup03), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup04), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup05), fieldService);
				this.AddSyncPermission(nameof(ArticleRelationship), fieldService);
				this.AddSyncPermission(nameof(ArticleRelationshipType), fieldService);
				this.AddSyncPermission(nameof(ArticleCompanyRelationship), fieldService);
				this.AddSyncPermission(nameof(ArticleCompanyRelationship), fieldSales);
				this.AddSyncPermission(nameof(ArticleCompanyRelationshipType), fieldService);
				this.AddSyncPermission(nameof(ArticleCompanyRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(ArticleType), fieldService);
				this.AddSyncPermission(nameof(ProductFamilyStatus), fieldSales);
				this.AddSyncPermission(nameof(ProductFamilyDescription), fieldSales);
				this.AddSyncPermission(nameof(ProductFamily), fieldSales, fieldService);
				this.AddSyncPermission(nameof(VATLevel), fieldService);
			}

			if (activePluginNames.Contains(CampaignsPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(Campaign), fieldSales);
				this.AddSyncPermission(nameof(CampaignCompany), fieldSales);
				this.AddSyncPermission(nameof(CampaignPerson), fieldSales);
				this.AddSyncPermission(nameof(CampaignStatus), fieldSales);
				this.AddSyncPermission(nameof(SourceType), fieldSales);
			}

			if (activePluginNames.Contains(ConfiguratorPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(ConfigurationBase), fieldSales);
				this.AddSyncPermission(nameof(ConfigurationRule), fieldSales);
				this.AddSyncPermission(nameof(ConfigurationRuleType), fieldSales);
				this.AddSyncPermission(nameof(Variable), fieldSales);
			}

			if (activePluginNames.Contains(ErpPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(ArticleGroup01), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup02), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup03), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup04), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup05), fieldSales);
				this.AddSyncPermission(nameof(CreditNote), fieldSales);
				this.AddSyncPermission(nameof(CreditNotePosition), fieldSales);
				this.AddSyncPermission(nameof(DeliveryNote), fieldSales);
				this.AddSyncPermission(nameof(DeliveryNotePosition), fieldSales);
				this.AddSyncPermission(nameof(ErpDocumentStatus), fieldSales);
				this.AddSyncPermission(nameof(ErpTurnover), fieldSales);
				this.AddSyncPermission(nameof(Invoice), fieldSales);
				this.AddSyncPermission(nameof(InvoicePosition), fieldSales);
				this.AddSyncPermission(nameof(MasterContract), fieldSales);
				this.AddSyncPermission(nameof(MasterContractPosition), fieldSales);
				this.AddSyncPermission(nameof(Quote), fieldSales);
				this.AddSyncPermission(nameof(QuotePosition), fieldSales);
				this.AddSyncPermission(nameof(SalesOrder), fieldSales);
				this.AddSyncPermission(nameof(SalesOrderPosition), fieldSales);
				this.AddSyncPermission(nameof(ErpPaymentMethod), fieldSales);
				this.AddSyncPermission(nameof(ErpDeliveryMethod), fieldSales);
				this.AddSyncPermission(nameof(ErpPaymentTerms), fieldSales);
				this.AddSyncPermission(nameof(ErpTermsOfDelivery), fieldSales);
				this.AddSyncPermission(nameof(ErpDeliveryProhibitedReason), fieldSales);
				this.AddSyncPermission(nameof(ErpPartialDeliveryProhibitedReason), fieldSales);
			}

			if (activePluginNames.Contains(MarketInsightPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(MarketInsight), fieldSales);
				this.AddSyncPermission(nameof(MarketInsightContactRelationship), fieldSales);
				this.AddSyncPermission(nameof(MarketInsightContactRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(MarketInsightReference), fieldSales);
				this.AddSyncPermission(nameof(MarketInsightStatus), fieldSales);
			}

			if (activePluginNames.Contains(OrderPlugin.Name))
			{
				this.AddSyncPermission(nameof(Article), fieldSales);
				this.AddSyncPermission(nameof(ArticleDescription), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup01), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup02), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup03), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup04), fieldSales);
				this.AddSyncPermission(nameof(ArticleGroup05), fieldSales);
				this.AddSyncPermission(nameof(ArticleRelationship), fieldSales);
				this.AddSyncPermission(nameof(ArticleRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(ArticleCompanyRelationship), fieldSales);
				this.AddSyncPermission(nameof(ArticleCompanyRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(ArticleType), fieldSales);
				this.AddSyncPermission(nameof(CalculationPosition), fieldSales);
				this.AddSyncPermission(nameof(CalculationPositionType), fieldSales);
				this.AddSyncPermission(nameof(Offer), fieldSales);
				this.AddSyncPermission(nameof(Order), fieldSales);
				this.AddSyncPermission(nameof(OrderCategory), fieldSales);
				this.AddSyncPermission(nameof(OrderEntryType), fieldSales);
				this.AddSyncPermission(nameof(OrderCancelReasonCategory), fieldSales);
				this.AddSyncPermission(nameof(OrderItem), fieldSales);
				this.AddSyncPermission(nameof(OrderStatus), fieldSales);
				this.AddSyncPermission(nameof(QuantityUnit), fieldSales);
				this.AddSyncPermission(nameof(VATLevel), fieldSales);
			}

			if (activePluginNames.Contains(PerDiemPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(ExpenseType), fieldSales, fieldService);
				this.AddSyncPermission(nameof(CostCenter), fieldSales, fieldService);
				this.AddSyncPermission(nameof(Currency), fieldSales, fieldService);
				this.AddSyncPermission(nameof(PerDiemReport), fieldSales, fieldService);
				this.AddSyncPermission(nameof(PerDiemReportStatus), fieldSales, fieldService);
				this.AddSyncPermission(nameof(PerDiemReportType), fieldSales, fieldService);
				this.AddSyncPermission(nameof(TimeEntryType), fieldSales, fieldService);
				this.AddSyncPermission(nameof(UserExpense), fieldSales, fieldService);
				this.AddSyncPermission(nameof(UserTimeEntry), fieldSales, fieldService);
			}

			if (activePluginNames.Contains(PerDiemGermanyPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(PerDiemAllowance), fieldSales, fieldService);
				this.AddSyncPermission(nameof(PerDiemAllowanceAdjustment), fieldSales, fieldService);
				this.AddSyncPermission(nameof(PerDiemAllowanceEntry), fieldSales, fieldService);
				this.AddSyncPermission(nameof(PerDiemAllowanceEntryAllowanceAdjustmentReference), fieldSales, fieldService);
			}

			if (activePluginNames.Contains(ProjectPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(Project), fieldSales);
				this.AddSyncPermission(nameof(ProjectCategory), fieldSales);
				this.AddSyncPermission(nameof(ProjectCategoryGroups));
				this.AddSyncPermission(nameof(ProjectContactRelationship), fieldSales);
				this.AddSyncPermission(nameof(ProjectContactRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(ProjectLostReasonCategory), fieldSales);
				this.AddSyncPermission(nameof(ProjectStatus), fieldSales);

				this.AddSyncPermission(nameof(Potential), fieldSales);
				this.AddSyncPermission(nameof(PotentialPriority), fieldSales);
				this.AddSyncPermission(nameof(PotentialContactRelationship), fieldSales);
				this.AddSyncPermission(nameof(PotentialContactRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(PotentialStatus), fieldSales);

				this.AddSyncPermission(nameof(DocumentEntry), fieldSales);
				this.AddSyncPermission(nameof(Article), fieldSales);
			}

			if (activePluginNames.Contains(ServicePlugin.PluginName))
			{
				this.AddSyncPermission(nameof(Address), fieldService);
				this.AddSyncPermission(nameof(AddressType), fieldService);
				this.AddSyncPermission(nameof(Article), fieldService);
				this.AddSyncPermission(nameof(ArticleDescription), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup01), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup02), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup03), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup04), fieldService);
				this.AddSyncPermission(nameof(ArticleGroup05), fieldService);
				this.AddSyncPermission(nameof(ArticleRelationship), fieldService);
				this.AddSyncPermission(nameof(ArticleRelationshipType), fieldService);
				this.AddSyncPermission(nameof(ArticleCompanyRelationship), fieldService);
				this.AddSyncPermission(nameof(ArticleCompanyRelationshipType), fieldService);
				this.AddSyncPermission(nameof(ArticleType), fieldService);
				this.AddSyncPermission(nameof(Branch1), fieldService);
				this.AddSyncPermission(nameof(Branch2), fieldService);
				this.AddSyncPermission(nameof(Branch3), fieldService);
				this.AddSyncPermission(nameof(Branch4), fieldService);
				this.AddSyncPermission(nameof(Bravo), fieldService);
				this.AddSyncPermission(nameof(BravoCategory), fieldService);
				this.AddSyncPermission(nameof(BusinessRelationship), fieldService);
				this.AddSyncPermission(nameof(BusinessRelationshipType), fieldService);
				this.AddSyncPermission(nameof(BusinessTitle), fieldService);
				this.AddSyncPermission(nameof(CauseOfFailure), fieldService);
				this.AddSyncPermission(nameof(CommissioningStatus), fieldService);
				this.AddSyncPermission(nameof(Company), fieldService);
				this.AddSyncPermission(nameof(CompanyBranch), fieldService);
				this.AddSyncPermission(nameof(CompanyGroupFlag1), fieldService);
				this.AddSyncPermission(nameof(CompanyGroupFlag2), fieldService);
				this.AddSyncPermission(nameof(CompanyGroupFlag3), fieldService);
				this.AddSyncPermission(nameof(CompanyGroupFlag4), fieldService);
				this.AddSyncPermission(nameof(CompanyGroupFlag5), fieldService);
				this.AddSyncPermission(nameof(CompanyType), fieldService);
				this.AddSyncPermission(nameof(Component), fieldService);
				this.AddSyncPermission(nameof(CostCenter), fieldService);
				this.AddSyncPermission(nameof(Country), fieldService);
				this.AddSyncPermission(nameof(Currency), fieldService);
				this.AddSyncPermission(nameof(DepartmentType), fieldService);
				this.AddSyncPermission(nameof(Device), fieldService);
				this.AddSyncPermission(nameof(DocumentAttribute), fieldService);
				this.AddSyncPermission(nameof(DocumentCategory), fieldService);
				this.AddSyncPermission(nameof(Email), fieldService);
				this.AddSyncPermission(nameof(EmailType), fieldService);
				this.AddSyncPermission(nameof(EntityType));
				this.AddSyncPermission(nameof(ErrorCode), fieldService);
				this.AddSyncPermission(nameof(ExpenseType), fieldService);
				this.AddSyncPermission(nameof(Fax), fieldService);
				this.AddSyncPermission(nameof(FaxType), fieldService);
				this.AddSyncPermission(nameof(FileResource), fieldService);
				this.AddSyncPermission(nameof(Installation), fieldService);
				this.AddSyncPermission(nameof(InstallationAddressRelationship), fieldService);
				this.AddSyncPermission(nameof(InstallationAddressRelationshipType), fieldService);
				this.AddSyncPermission(nameof(InstallationHeadStatus), fieldService);
				this.AddSyncPermission(nameof(InstallationPos), fieldService);
				this.AddSyncPermission(nameof(InstallationType), fieldService);
				this.AddSyncPermission(nameof(InvoicingType), fieldService);
				this.AddSyncPermission(nameof(LinkResource), fieldService);
				this.AddSyncPermission(nameof(Location), fieldService);
				this.AddSyncPermission(nameof(MaintenancePlan), fieldService);
				this.AddSyncPermission(nameof(Manufacturer), fieldService);
				this.AddSyncPermission(nameof(MonitoringDataType));
				this.AddSyncPermission(nameof(NoCausingItemPreviousSerialNoReason));
				this.AddSyncPermission(nameof(NoCausingItemSerialNoReason));
				this.AddSyncPermission(nameof(NoPreviousSerialNoReason), fieldService);
				this.AddSyncPermission(nameof(Note), fieldService);
				this.AddSyncPermission(nameof(NoteType), fieldService);
				this.AddSyncPermission(nameof(NotificationStandardAction));
				this.AddSyncPermission(nameof(NumberOfEmployees), fieldService);
				this.AddSyncPermission(nameof(PaymentCondition), fieldService);
				this.AddSyncPermission(nameof(PaymentInterval), fieldService);
				this.AddSyncPermission(nameof(PaymentType), fieldService);
				this.AddSyncPermission(nameof(Permission), fieldService);
				this.AddSyncPermission(nameof(PermissionSchemaRole), fieldService);
				this.AddSyncPermission(nameof(Person), fieldService);
				this.AddSyncPermission(nameof(Phone), fieldService);
				this.AddSyncPermission(nameof(PhoneType), fieldService);
				this.AddSyncPermission(nameof(QuantityUnit), fieldService);
				this.AddSyncPermission(nameof(Region), fieldService);
				this.AddSyncPermission(nameof(Salutation), fieldService);
				this.AddSyncPermission(nameof(SalutationLetter));
				this.AddSyncPermission(nameof(ReplenishmentOrder), fieldService);
				this.AddSyncPermission(nameof(ReplenishmentOrderItem), fieldService);
				this.AddSyncPermission(nameof(ServiceCase), fieldService);
				this.AddSyncPermission(nameof(ServiceCaseCategory), fieldService);
				this.AddSyncPermission(nameof(ServiceCaseStatus), fieldService);
				this.AddSyncPermission(nameof(ServiceCaseTemplate), fieldService);
				this.AddSyncPermission(nameof(ServiceContract), fieldService);
				this.AddSyncPermission(nameof(ServiceContractAddressRelationship));
				this.AddSyncPermission(nameof(ServiceContractAddressRelationshipType), fieldService);
				this.AddSyncPermission(nameof(ServiceContractInstallationRelationship), fieldService);
				this.AddSyncPermission(nameof(ServiceContractLimitType));
				this.AddSyncPermission(nameof(ServiceContractStatus), fieldService);
				this.AddSyncPermission(nameof(ServiceContractType), fieldService);
				this.AddSyncPermission(nameof(ServiceObject), fieldService);
				this.AddSyncPermission(nameof(ServiceObjectCategory), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderDispatch), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderDispatchRejectReason), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderDispatchReportRecipient), fieldService);
				AddImport(PermissionGroup.Sync, nameof(ServiceOrderDispatch), PermissionGroup.Sync, nameof(ServiceOrderDispatchReportRecipient));
				this.AddSyncPermission(nameof(ServiceOrderDispatchStatus), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderHead), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderInvoiceReason), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderMaterial), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderMaterialSerial), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderNoInvoiceReason), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderStatus), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderTime), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderTimeCategory));
				this.AddSyncPermission(nameof(ServiceOrderTimeLocation));
				this.AddSyncPermission(nameof(ServiceOrderTimePriority));
				this.AddSyncPermission(nameof(ServiceOrderTimePosting), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderTimeStatus), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderType), fieldService);
				this.AddSyncPermission(nameof(ServicePriority), fieldService);
				this.AddSyncPermission(nameof(Skill), fieldService);
				this.AddSyncPermission(nameof(SourceType), fieldService);
				this.AddSyncPermission(nameof(SparePartsBudgetInvoiceType), fieldService);
				this.AddSyncPermission(nameof(SparePartsBudgetTimeSpanUnit), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyProductType), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyMainAssembly), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeySubAssembly), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyAssemblyGroup), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyFaultImage), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyRemedy), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyCause), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyWeighting), fieldService);
				this.AddSyncPermission(nameof(StatisticsKeyCauser), fieldService);
				this.AddSyncPermission(nameof(Store), fieldService);
				this.AddSyncPermission(nameof(Tag), fieldService);
				this.AddSyncPermission(nameof(Task), fieldService);
				this.AddSyncPermission(nameof(TaskType), fieldService);
				this.AddSyncPermission(nameof(TimeEntryType), fieldService);
				this.AddSyncPermission(nameof(TimeUnit), fieldService);
				this.AddSyncPermission(nameof(Title), fieldService);
				this.AddSyncPermission(nameof(Turnover), fieldService);
				this.AddSyncPermission(nameof(User), fieldService);
				this.AddSyncPermission(nameof(UserExpense), fieldService);
				this.AddSyncPermission(nameof(Usergroup), fieldService);
				this.AddSyncPermission(nameof(UserNote), fieldService);
				this.AddSyncPermission(nameof(UserSubscription), fieldService);
				this.AddSyncPermission(nameof(UserTimeEntry), fieldService);
				this.AddSyncPermission(nameof(VATLevel));
				this.AddSyncPermission(nameof(Website), fieldService);
				this.AddSyncPermission(nameof(WebsiteType), fieldService);
			}

			if (activePluginNames.Contains(DynamicFormsPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(DynamicForm), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormCategory), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormElementRule), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormElementRuleCondition), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormLanguage), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormLocalization), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormStatus), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormResponse), fieldSales);
				this.AddSyncPermission(nameof(DynamicFormFileResponse), fieldSales);
			}

			if (activePluginNames.Contains(VisitReportPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(ContactPersonRelationship), fieldSales, fieldService);
				this.AddSyncPermission(nameof(ContactPersonRelationshipType), fieldSales, fieldService);
				this.AddSyncPermission(nameof(Language), fieldSales);
				this.AddSyncPermission(nameof(Visit), fieldSales);
				this.AddSyncPermission(nameof(VisitAim), fieldSales);
				this.AddSyncPermission(nameof(VisitReport), fieldSales);
				this.AddSyncPermission(nameof(VisitTopic), fieldSales);
				this.AddSyncPermission(nameof(VisitStatus), fieldSales);
			}

			if (activePluginNames.Contains(MainPlugin.Name))
			{
				this.AddSyncPermission(nameof(Address), fieldSales);
				this.AddSyncPermission(nameof(AddressType), fieldSales);
				this.AddSyncPermission(nameof(Bravo), fieldSales);
				this.AddSyncPermission(nameof(BravoCategory), fieldSales);
				this.AddSyncPermission(nameof(BusinessRelationship), fieldSales);
				this.AddSyncPermission(nameof(BusinessRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(BusinessTitle), fieldSales);
				this.AddSyncPermission(nameof(Company), fieldSales);
				this.AddSyncPermission(nameof(CompanyGroupFlag1), fieldSales);
				this.AddSyncPermission(nameof(CompanyGroupFlag2), fieldSales);
				this.AddSyncPermission(nameof(CompanyGroupFlag3), fieldSales);
				this.AddSyncPermission(nameof(CompanyGroupFlag4), fieldSales);
				this.AddSyncPermission(nameof(CompanyGroupFlag5), fieldSales);
				this.AddSyncPermission(nameof(CompanyBranch), fieldSales);
				this.AddSyncPermission(nameof(Branch1), fieldSales);
				this.AddSyncPermission(nameof(Branch2), fieldSales);
				this.AddSyncPermission(nameof(Branch3), fieldSales);
				this.AddSyncPermission(nameof(Branch4), fieldSales);
				this.AddSyncPermission(nameof(CompanyPersonRelationship), fieldSales);
				this.AddSyncPermission(nameof(CompanyPersonRelationshipType), fieldSales);
				this.AddSyncPermission(nameof(CompanyType), fieldSales);
				this.AddSyncPermission(nameof(Country), fieldSales);
				this.AddSyncPermission(nameof(Currency), fieldSales);
				this.AddSyncPermission(nameof(DepartmentType), fieldSales);
				this.AddSyncPermission(nameof(Device), fieldSales);
				this.AddSyncPermission(nameof(DocumentAttribute), fieldSales);
				this.AddSyncPermission(nameof(DocumentCategory), fieldSales);
				this.AddSyncPermission(nameof(Email), fieldSales);
				this.AddSyncPermission(nameof(EmailType), fieldSales);
				this.AddSyncPermission(nameof(Fax), fieldSales);
				this.AddSyncPermission(nameof(FaxType), fieldSales);
				this.AddSyncPermission(nameof(FileResource), fieldSales);
				this.AddSyncPermission(nameof(InvoicingType), fieldSales);
				this.AddSyncPermission(nameof(Language), fieldSales);
				this.AddSyncPermission(nameof(LinkResource), fieldSales);
				this.AddSyncPermission(nameof(Note), fieldSales);
				this.AddSyncPermission(nameof(NoteType), fieldSales);
				this.AddSyncPermission(nameof(NumberOfEmployees), fieldSales);
				this.AddSyncPermission(nameof(Permission), fieldSales);
				this.AddSyncPermission(nameof(PermissionSchemaRole), fieldSales);
				this.AddSyncPermission(nameof(Person), fieldSales);
				this.AddSyncPermission(nameof(Phone), fieldSales);
				this.AddSyncPermission(nameof(PhoneType), fieldSales);
				this.AddSyncPermission(nameof(Region), fieldSales);
				this.AddSyncPermission(nameof(Salutation), fieldSales);
				this.AddSyncPermission(nameof(SalutationLetter));
				this.AddSyncPermission(nameof(Site), fieldSales, fieldService);
				this.AddSyncPermission(nameof(Station), fieldSales, fieldService);
				this.AddSyncPermission(nameof(SourceType), fieldSales);
				this.AddSyncPermission(nameof(Tag), fieldSales);
				this.AddSyncPermission(nameof(Task), fieldSales);
				this.AddSyncPermission(nameof(TaskType), fieldSales);
				this.AddSyncPermission(nameof(TimeUnit), fieldSales);
				this.AddSyncPermission(nameof(Title), fieldSales);
				this.AddSyncPermission(nameof(Turnover), fieldSales);
				this.AddSyncPermission(nameof(User), fieldSales);
				this.AddSyncPermission(nameof(Usergroup), fieldSales);
				this.AddSyncPermission(nameof(UserNote), fieldSales);
				this.AddSyncPermission(nameof(UserSubscription), fieldSales);
				this.AddSyncPermission(nameof(Website), fieldSales);
				this.AddSyncPermission(nameof(WebsiteType), fieldSales);
			}

			if (activePluginNames.Contains(MultitenantPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(Domain), fieldSales, fieldService);
			}

			if (activePluginNames.Contains(ChecklistsPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(DynamicForm), fieldService);
				this.AddSyncPermission(nameof(DynamicFormCategory), fieldService);
				this.AddSyncPermission(nameof(DynamicFormElementRule), fieldService);
				this.AddSyncPermission(nameof(DynamicFormElementRuleCondition), fieldService);
				this.AddSyncPermission(nameof(DynamicFormLanguage), fieldService);
				this.AddSyncPermission(nameof(DynamicFormLocalization), fieldService);
				this.AddSyncPermission(nameof(DynamicFormStatus), fieldService);
				this.AddSyncPermission(nameof(DynamicFormResponse), fieldService);
				this.AddSyncPermission(nameof(DynamicFormFileResponse), fieldService);
				this.AddSyncPermission(nameof(Language), fieldService);
				this.AddSyncPermission(nameof(ServiceCaseChecklist), fieldService);
				this.AddSyncPermission(nameof(ServiceOrderChecklist), fieldService);
			}

			if (activePluginNames.Contains(EinsatzplanungTeamPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(TeamDispatchUser), fieldService);
			}

			if (activePluginNames.Contains(TimeManagementPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(TimeManagementEvent), fieldService, fieldSales);
				this.AddSyncPermission(nameof(TimeManagementEventType), fieldService, fieldSales);
			}

			if (activePluginNames.Contains(AttributeFormsPlugin.Name))
			{
				this.AddSyncPermission(nameof(AttributeForm), fieldSales);
				this.AddSyncPermission(nameof(AttributeForm), fieldService);
			}

			if (activePluginNames.Contains(EinsatzplanungConnectorPlugin.PluginName))
			{
				this.AddSyncPermission(nameof(AbsenceDispatch), fieldService);
			}
			
			this.AddSyncPermission(nameof(LengthUnit));
			this.AddSyncPermission(nameof(NumberOfEmployees));
			this.AddSyncPermission(nameof(RecentPage), fieldSales, fieldService);
			Add(PermissionGroup.WebApi, nameof(ReplicationGroup), fieldSales, fieldService);
			this.AddSyncPermission(nameof(ReplicationGroup), fieldSales, fieldService);
			Add(PermissionGroup.WebApi, nameof(ReplicationGroupSetting), fieldSales, fieldService);
			this.AddSyncPermission(nameof(ReplicationGroupSetting), fieldSales, fieldService);
			this.AddSyncPermission(nameof(SalutationLetter));
			this.AddSyncPermission(nameof(Turnover));
			this.AddSyncPermission(nameof(UserStatus));
			this.AddSyncPermission(nameof(ZipCodeFilterType));
		}
	}
}
