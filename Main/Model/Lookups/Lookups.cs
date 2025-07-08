// ---------------------------------------------------
//									- Important	-
// ---------------------------------------------------
// All classes inside this file are managed by the 
// LookupManager class, make sure to add all necessary
// entries before running the program. See also
// DbServiceRegistry class
// ---------------------------------------------------

namespace Crm.Model.Lookups
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;

	#region ContactContactRelationship

	[Lookup("[LU].[BusinessRelationshipType]")]
	public class BusinessRelationshipType : EntityLookup<string>, ILookupWithInverseRelationship<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string InverseRelationshipTypeKey { get; set; }
		[LookupProperty(Shared = true)]
		public virtual bool IsIndelible { get; set; }
		[RestrictedField]
		public virtual DefaultInverseRelationship DefaultInverseRelationship { get; set; }

		// Constructor
		public BusinessRelationshipType()
		{
			DefaultInverseRelationship = DefaultInverseRelationship.Self;
		}
	}
	[Lookup("[LU].[CompanyPersonRelationshipType]")]
	public class CompanyPersonRelationshipType : EntityLookup<string>
	{
	}

	#endregion ContactContactRelationship

	#region TimeUnit

	[Lookup("[LU].[TimeUnit]", "TimeUnitId")]
	public class TimeUnit : EntityLookup<string>, ILookupWithTimeUnitsPerYear
	{
		[LookupProperty(Shared = true)]
		public virtual int? TimeUnitsPerYear { get; set; }

		public const string YearKey = "Year";
		public const string MonthKey = "Month";
		public const string QuarterKey = "Quarter";
		public const string WeekKey = "Week";
		public const string DayKey = "Day";
		public const string HourKey = "Hour";
		public const string MinuteKey = "Minute";
		public const string SecondKey = "Second";
		public const string MillisecondKey = "Millisecond";
	}

	[Lookup("[LU].[LengthUnit]")]
	public class LengthUnit : EntityLookup<string>
	{
	}

	#endregion TimeUnit

	#region TaskType

	[Lookup("[LU].[TaskType]")]
	public class TaskType : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }

		public TaskType()
		{
			Color = "#AAAAAA";
		}

		// Members
		public static readonly TaskType None = new TaskType { Key = String.Empty, Value = "None" };
		public static readonly TaskType Default = new TaskType { Key = "100" };
	}

	#endregion

	#region SalutationLetter

	[Lookup("[LU].[SalutationLetter]", "LetterSalutationTypeId")]
	public class SalutationLetter : EntityLookup<string>
	{
		// Members
		public static readonly SalutationLetter None = new SalutationLetter { Key = string.Empty, Value = "None" };
	}

	#endregion

	#region Salutation

	[Lookup("[LU].[Salutation]", "SalutationTypeId")]
	public class Salutation : EntityLookup<string>
	{
		// Members
		public static readonly Salutation None = new Salutation { Key = "100", Value = "" };
	}

	#endregion

	#region Title

	[Lookup("[LU].[Title]", "TitleTypeId")]
	public class Title : EntityLookup<string>
	{
		// Members
		public static readonly Title None = new Title { Key = "100", Language = "en", Value = "" };
		public static readonly Title Default = None;
	}

	#endregion

	#region PhoneType

	[Lookup("[LU].[PhoneType]")]
	public class PhoneType : EntityLookup<string>
	{
		// Members
		public static string WorkKey = "PhoneWork";
		public static string MobileKey = "PhoneMobile";
	}

	#endregion

	#region EmailType

	[Lookup("[LU].[EmailType]")]
	public class EmailType : EntityLookup<string>
	{
		// Members
		public static string WorkKey = "EmailWork";
	}

	#endregion

	#region FaxType

	[Lookup("[LU].[FaxType]")]
	public class FaxType : EntityLookup<string>
	{
		// Members
		public static string WorkKey = "FaxWork";
	}

	#endregion

	#region WebsiteType

	[Lookup("[LU].[WebsiteType]")]
	public class WebsiteType : EntityLookup<string>
	{
		// Members
		public static string WorkKey = "WebsiteWork";
	}

	#endregion

	#region AddressType

	[Lookup("[LU].[AddressType]")]
	public class AddressType : EntityLookup<string>
	{
		// Members
		public static readonly AddressType None = new AddressType { Key = string.Empty, Value = "None" };
		public static readonly AddressType Delivery = new AddressType { Key = "102" };
		public static readonly AddressType Invoice = new AddressType { Key = "103" };
	}

	#endregion

	#region CompanyType

	[Lookup("[LU].[CompanyType]")]
	public class CompanyType : EntityLookup<string>, ILookupWithColor
	{
		// Members
		public static readonly CompanyType None = new CompanyType { Key = String.Empty, Value = "None" };
		public static readonly CompanyType Prospect = new CompanyType { Key = "100", Value = "Prospect" };
		public static readonly CompanyType Customer = new CompanyType { Key = "101", Value = "Customer" };
		public static readonly CompanyType Partner = new CompanyType { Key = "102", Value = "Partner" };
		public static readonly CompanyType Competitor = new CompanyType { Key = "103", Value = "Competitor" };
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#9E9E9E";
	}

	#endregion

	#region Region

	[Lookup("[LU].[Region]")]
	public class Region : EntityLookup<string>
	{
		public static readonly Region None = new Region { Key = string.Empty, Value = "None" };
	}

	#endregion

	#region Country

	[Lookup("[LU].[Country]")]
	public class Country : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string Iso2Code { get; set; }

		[LookupProperty(Shared = true)]
		public virtual string Iso3Code { get; set; }

		[LookupProperty(Shared = true)]
		public virtual string CallingCode { get; set; }

		public virtual string NameWithCallingCode
		{
			get { return String.Format("{0} (+{1})", Value, CallingCode); }
		}

		public static readonly Country None = new Country { Key = String.Empty, Value = "None" };
	}

	#endregion

	# region NumberOfEmployees

	[Lookup("[LU].[NumberOfEmployees]")]
	public class NumberOfEmployees : EntityLookup<string>
	{
		// Members
		public static readonly NumberOfEmployees None = new NumberOfEmployees { Key = string.Empty, Value = "None" };
	}

	#endregion

	# region Turnover

	[Lookup("[LU].[Turnover]")]
	public class Turnover : EntityLookup<string>
	{
		// Members
		public static readonly Turnover None = new Turnover { Key = string.Empty, Value = "None" };
	}

	#endregion

	# region Currency

	[Lookup("[LU].[Currency]", "ProjectCategoriyId")]
	public class Currency : EntityLookup<string>
	{
		public static readonly string EuroKey = "EUR";
		public static readonly string DollarKey = "USD";
		public static readonly string PoundKey = "GBP";
		public static Func<ILookup, string> DefaultValueTemplate = c => "{0} - {1}".WithArgs(c.Value, c.Key);
	}

	#endregion

	#region CompanyGroupFlag1

	[Lookup("[LU].[CompanyGroupFlag1]", "CompanyGroupFlag1Id")]
	public class CompanyGroupFlag1 : EntityLookup<string>
	{
		// Members
		public static readonly CompanyGroupFlag1 None = new CompanyGroupFlag1 { Key = string.Empty, Value = "None" };
	}

	#endregion

	#region CompanyGroupFlag2

	[Lookup("[LU].[CompanyGroupFlag2]", "CompanyGroupFlag2Id")]
	public class CompanyGroupFlag2 : EntityLookup<string>
	{
		// Members
		public static readonly CompanyGroupFlag2 None = new CompanyGroupFlag2 { Key = string.Empty, Value = "None" };
	}

	#endregion

	#region CompanyGroupFlag3

	[Lookup("[LU].[CompanyGroupFlag3]", "CompanyGroupFlag3Id")]
	public class CompanyGroupFlag3 : EntityLookup<string>
	{
		// Members
		public static readonly CompanyGroupFlag3 None = new CompanyGroupFlag3 { Key = string.Empty, Value = "None" };
	}

	#endregion

	#region CompanyGroupFlag4

	[Lookup("[LU].[CompanyGroupFlag4]", "CompanyGroupFlag4Id")]
	public class CompanyGroupFlag4 : EntityLookup<string>
	{
		public static readonly CompanyGroupFlag4 None = new CompanyGroupFlag4 { Key = string.Empty, Value = "None" };
	}

	#endregion

	#region CompanyGroupFlag5

	[Lookup("[LU].[CompanyGroupFlag5]", "CompanyGroupFlag5Id")]
	public class CompanyGroupFlag5 : EntityLookup<string>
	{
		public static readonly CompanyGroupFlag5 None = new CompanyGroupFlag5 { Key = string.Empty, Value = "None" };
	}

	#endregion

	#region SourceType

	[Lookup("[LU].[SourceType]", "Id")]
	public class SourceType : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual bool CampaignType { get; set; }
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }

		public SourceType()
		{
			CampaignType = true;
			Color = "#9E9E9E";
		}

		public static readonly SourceType Default = new SourceType { Key = String.Empty };
	}

	#endregion

	#region BravoCategory

	[Lookup("[LU].[BravoCategory]", "BravoCategoryTypeId")]
	public class BravoCategory : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string ContactType { get; set; }
		// Members
		public static readonly BravoCategory None = new BravoCategory { Key = String.Empty, Value = "None" };
	}

	#endregion

	#region PaymentType

	[Lookup("[LU].[PaymentType]")]
	public class PaymentType : EntityLookup<string>
	{
	}

	#endregion

	#region PaymentInterval

	[Lookup("[LU].[PaymentInterval]")]
	public class PaymentInterval : EntityLookup<string>
	{
	}

	#endregion

	#region PaymentConditions

	[Lookup("[LU].[PaymentCondition]")]
	public class PaymentCondition : EntityLookup<string>
	{
	}

	#endregion

	#region InvoicingType

	[Lookup("[LU].[InvoicingType]")]
	public class InvoicingType : EntityLookup<string>
	{
	}

	#endregion

	#region DepartmentType

	[Lookup("[LU].[DepartmentType]", "DepartmentTypeId")]
	public class DepartmentType : EntityLookup<string>
	{
		// Members
		public static readonly DepartmentType None = new DepartmentType { Key = String.Empty, Value = "None" };
		public static readonly DepartmentType Default = new DepartmentType { Key = "00" };
	}

	#endregion

	#region BusinessTitle

	[Lookup("[LU].[BusinessTitle]", "BusinessTitleId")]
	public class BusinessTitle : EntityLookup<string>
	{
		// Members
		public static readonly BusinessTitle None = new BusinessTitle { Key = String.Empty, Value = "None" };
		public static readonly BusinessTitle Default = new BusinessTitle { Key = "00" };
	}

	#endregion

	#region ZipCodeFilter

	[Lookup("[LU].[ZipCodeFilter]", "Id")]
	public class ZipCodeFilterType : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string CountryKey { get; set; }

		[LookupProperty(Shared = true)]
		public virtual string ZipCodeFrom { get; set; }

		[LookupProperty(Shared = true)]
		public virtual string ZipCodeTo { get; set; }
	}

	#endregion

	#region Branch1

	[Lookup("[LU].[Branch1]", "Branch1Id")]
	public class Branch1 : EntityLookup<string>
	{
	}

	#endregion

	#region Branch2

	[Lookup("[LU].[Branch2]", "Branch2Id")]
	public class Branch2 : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string ParentName { get; set; }
	}

	#endregion

	#region Branch3

	[Lookup("[LU].[Branch3]", "Branch3Id")]
	public class Branch3 : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string ParentName { get; set; }
	}

	#endregion

	#region Branch4

	[Lookup("[LU].[Branch4]", "Branch4Id")]
	public class Branch4 : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string ParentName { get; set; }
	}

	#endregion

}
