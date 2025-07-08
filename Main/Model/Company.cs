namespace Crm.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Model.Interfaces;
	using Crm.Model.Lookups;
	using Crm.Model.Relationships;

	public class Company : Contact, IMergable, IEntityWithTags
	{
		// Properties
		public virtual string ShortText { get; set; }
		public virtual bool IsOwnCompany { get; set; }
		public virtual string NumberOfEmployeesKey { get; set; }
		public virtual string TurnoverKey { get; set; }
		public virtual string CompanyGroupFlag1Key { get; set; }
		public virtual string CompanyGroupFlag2Key { get; set; }
		public virtual string CompanyGroupFlag3Key { get; set; }
		public virtual string CompanyGroupFlag4Key { get; set; }
		public virtual string CompanyGroupFlag5Key { get; set; }
		public virtual string CompanyNo { get; set; }
		public virtual string CompanyTypeKey { get; set; }
		public virtual string AreaSalesManager { get; set; }
		public virtual User AreaSalesManagerObject { get; set; }
		public virtual string SalesRepresentative { get; set; }
		public virtual bool ErpDeliveryProhibited { get; set; }
		public virtual bool IsEnabled { get; set; }
		public virtual ICollection<CompanyBranch> CompanyBranches { get; set; }
		public virtual CompanyType CompanyType
		{
			get { return CompanyTypeKey != null ? LookupManager.Get<CompanyType>(CompanyTypeKey) : null; }
		}
		public virtual IList<Company> ClientCompanies { get; set; }

		public virtual IList<BusinessRelationship> BusinessRelationships { get; set; }
		public virtual IList<CompanyPersonRelationship> CompanyPersonRelationships { get; set; }

		public virtual NumberOfEmployees NumberOfEmployees
		{
			get { return NumberOfEmployeesKey != null ? LookupManager.Get<NumberOfEmployees>(NumberOfEmployeesKey) : null; }
		}
		public virtual Turnover Turnover
		{
			get { return TurnoverKey != null ? LookupManager.Get<Turnover>(TurnoverKey) : null; }
		}
		public virtual CompanyGroupFlag1 CompanyGroupFlag1
		{
			get { return CompanyGroupFlag1Key != null ? LookupManager.Get<CompanyGroupFlag1>(CompanyGroupFlag1Key) : null; }
		}
		public virtual CompanyGroupFlag2 CompanyGroupFlag2
		{
			get { return CompanyGroupFlag2Key != null ? LookupManager.Get<CompanyGroupFlag2>(CompanyGroupFlag2Key) : null; }
		}
		public virtual CompanyGroupFlag3 CompanyGroupFlag3
		{
			get { return CompanyGroupFlag3Key != null ? LookupManager.Get<CompanyGroupFlag3>(CompanyGroupFlag3Key) : null; }
		}
		public virtual CompanyGroupFlag4 CompanyGroupFlag4
		{
			get { return CompanyGroupFlag4Key != null ? LookupManager.Get<CompanyGroupFlag4>(CompanyGroupFlag4Key) : null; }
		}
		public virtual CompanyGroupFlag5 CompanyGroupFlag5
		{
			get { return CompanyGroupFlag5Key != null ? LookupManager.Get<CompanyGroupFlag5>(CompanyGroupFlag5Key) : null; }
		}
		public virtual Company ParentCompany { get; set; }
		public virtual Guid? StationKey { get; set; }
		public virtual Station Station { get; set; }
		public override string ToString()
		{
			var result = String.Empty;

			result += String.IsNullOrWhiteSpace(LegacyId)
									? Name
									: LegacyName;

			if (CompanyTypeKey.IsNotNullOrEmpty() && CompanyType.IsNotNull())
			{
				result += " - " + CompanyType.Value;
			}
			return result;
		}

		// Constructor
		public Company()
		{
			Staff = new List<Person>();
			ClientCompanies = new List<Company>();
			BusinessRelationships = new List<BusinessRelationship>();
			CompanyPersonRelationships = new List<CompanyPersonRelationship>();
			Bravos = new List<Bravo>();
			IsEnabled = true;
		}
	}
}
