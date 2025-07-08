namespace Crm.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain;
	using Crm.Library.Extensions;
	using Crm.Model.Lookups;

	public class Communication : EntityBase<Guid>, IExportable, ISoftDelete
	{
		// Properties
		public virtual string LegacyId { get; set; }
		[DomainSignature]
		public virtual string TypeKey { get; set; }
		public virtual Type TypeType { get { throw new NotImplementedException(); } }
		[DomainSignature]
		public virtual Guid ContactId { get; set; }
		public virtual Guid? AddressId { get; set; }
		[DomainSignature]
		public virtual string Data { get; set; }
		public virtual string DataOnlyNumbers { get; set; }
		public virtual string Comment { get; set; }
		public virtual bool IsExported { get; set; }
		public virtual string CallingCode { get; set; }
		public virtual string CountryKey { get; set; }
		public virtual string AreaCode { get; set; }

		public virtual Contact Contact { get; set; }
		public virtual Address Address { get; set; }

		public virtual Country Country
		{
			get { return CountryKey != null ? LookupManager.Get<Country>(CountryKey) : null; }
		}
	}

	public class Phone : Communication
	{
		public override Type TypeType
		{
			get { return typeof(PhoneType); }
		}
		public virtual PhoneType Type
		{
			get { return TypeKey != null ? LookupManager.Get<PhoneType>(TypeKey) : null; }
		}
	}

	public class Email : Communication
	{
		public override Type TypeType
		{
			get { return typeof(EmailType); }
		}
		public virtual EmailType Type
		{
			get { return TypeKey != null ? LookupManager.Get<EmailType>(TypeKey) : null; }
		}
	}

	public class Fax : Communication
	{
		public override Type TypeType
		{
			get { return typeof(FaxType); }
		}
		public virtual FaxType Type
		{
			get { return TypeKey != null ? LookupManager.Get<FaxType>(TypeKey) : null; }
		}
	}

	public class Website : Communication
	{
		public override Type TypeType
		{
			get { return typeof(WebsiteType); }
		}
		public virtual WebsiteType Type
		{
			get { return TypeKey != null ? LookupManager.Get<WebsiteType>(TypeKey) : null; }
		}

		public override string ToString()
		{
			var httpPrefixes = new[] { "http://", "https://" };
			return Data.StartsWithAny(httpPrefixes, true)
			       	? Data
			       	: "http://{0}".WithArgs(Data);
		}
	}
}
