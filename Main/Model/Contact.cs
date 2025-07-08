namespace Crm.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.BaseClasses;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization;
	using Crm.Library.Model;
	using Crm.Model.Lookups;
	using Crm.Model.Notes;

	using StackExchange.Profiling;

	using Extensions;

	using NHibernate.Proxy;

	public class Contact : EntityWithVisibility<Guid>, IExportable, ISoftDelete
	{
		[DomainSignature]
		public virtual string LegacyId { get; set; }
		public virtual long? LegacyVersion { get; set; }
		public virtual string LegacyName { get; set; }

		//this property is needed to get overwritten by plugins
		public virtual string ReferenceLink
		{
			get { return Name; }
		}
		public virtual string LanguageKey { get; set; }
		public virtual string BackgroundInfo { get; set; }
		public virtual string ContactType { get; set; }
		public virtual ICollection<DocumentAttribute> DocumentAttributes { get; set; }
		public virtual string ResponsibleUser { get; set; }
		public virtual User ResponsibleUserObject { get; set; }
		public virtual string SourceTypeKey { get; set; }
		public virtual Guid? CampaignSource { get; set; }
		public virtual bool IsExported { get; set; }
		public virtual DateTime? InactiveDate { get; set; }
		public virtual string InactiveUser { get; set; }
		public virtual ICollection<Bravo> Bravos { get; set; }

		// Link entity to some parent contact
		[DomainSignature]
		public virtual Guid? ParentId { get; set; }
		public virtual string ParentName { get { return Parent != null ? Parent.Name : null; } }

		public virtual Contact Parent { get; set; }

		public virtual Address StandardAddress { get; protected set; }
		public virtual void SetStandardAddress(Address address)
		{
			if (Addresses.IsNull())
			{
				Addresses = new List<Address>();
			}
			if (address.IsNotNull())
			{
				if (!Addresses.Contains(address))
					Addresses.Add(address);

				StandardAddress = address;
				Addresses.ForEach(a => a.IsCompanyStandardAddress = Equals(a, address));
			}
		}
		public virtual ICollection<Person> Staff { get; set; }
		public virtual ICollection<Address> Addresses { get; set; }

		// Lookup properties
		public virtual Language Language
		{
			get { return LanguageKey != null ? LookupManager.Get<Language>(LanguageKey) : null; }
		}
		public virtual SourceType SourceType
		{
			get { return SourceTypeKey != null ? LookupManager.Get<SourceType>(SourceTypeKey) : null; }
		}

		public virtual ICollection<Communication> Communications { get; set; }
		/// <summary>
		/// When you add to this list, your changes get lost. Set with a new instance of List(Phone) instead
		/// or push to the "IList(Communication) Communications"
		/// </summary>
		public virtual List<Phone> Phones
		{
			get { return Communications.OfType<Phone>().ToList(); }
			set
			{
				RemovePhones();
				AddCommunications(value);
			}
		}
		/// <summary>
		/// When you add to this list, your changes get lost. Set with a new instance of List(Email) instead
		/// or push to the "IList(Communication) Communications"
		/// </summary>
		public virtual List<Email> Emails
		{
			get { return Communications.OfType<Email>().ToList(); }
			set
			{
				RemoveEmails();
				AddCommunications(value);
			}
		}
		/// <summary>
		/// When you add to this list, your changes get lost. Set with a new instance of List(Fax) instead
		/// or push to the "IList(Communication) Communications"
		/// </summary>
		public virtual List<Fax> Faxes
		{
			get { return Communications.OfType<Fax>().ToList(); }
			set
			{
				RemoveFaxes();
				AddCommunications(value);
			}
		}
		/// <summary>
		/// When you add to this list, your changes get lost. Set with a new instance of List(Website) instead
		/// or push to the "IList(Communication) Communications"
		/// </summary>
		public virtual List<Website> Websites
		{
			get { return Communications.OfType<Website>().ToList(); }
			set
			{
				RemoveWebsites();
				AddCommunications(value);
			}
		}

		public virtual ICollection<Note> Notes { get; set; }
		public virtual ICollection<Tag> Tags { get; set; }

		#region ContactSearchProperties

		public virtual ICollection<RecentPage> UserRecentPages { get; set; }
		public virtual string StandardAddressStreet { get { return StandardAddress != null ? StandardAddress.Street : null; } }
		public virtual string StandardAddressZipCode { get { return StandardAddress != null ? StandardAddress.ZipCode : null; } }
		public virtual string StandardAddressCity { get { return StandardAddress != null ? StandardAddress.City : null; } }
		public virtual string StandardAddressRegionKey { get { return StandardAddress != null ? StandardAddress.RegionKey : null; } }
		public virtual string StandardAddressCountryKey { get { return StandardAddress != null ? StandardAddress.CountryKey : null; } }

		public virtual Email GetPrimaryEmail()
		{
			return Emails.OrderBy(x => x.CreateDate).FirstOrDefault(x => x.IsActive);
		}
		public virtual string PrimaryEmailData
		{
			get
			{
				var primaryEmail = GetPrimaryEmail();
				return primaryEmail != null ? primaryEmail.Data : null;
			}
		}
		public virtual Phone GetPrimaryPhone()
		{
			return Phones.OrderBy(x => x.CreateDate).FirstOrDefault(x => x.IsActive);
		}
		public virtual string PrimaryPhoneData
		{
			get
			{
				var primaryPhone = GetPrimaryPhone();
				return primaryPhone != null ? primaryPhone.DataOrPhoneNumber() : null;
			}
		}
		public virtual Fax GetPrimaryFax()
		{
			return Faxes.OrderBy(x => x.CreateDate).FirstOrDefault(x => x.IsActive);
		}
		public virtual string PrimaryFaxData
		{
			get
			{
				var primaryFax = GetPrimaryFax();
				return primaryFax != null ? primaryFax.DataOrPhoneNumber() : null;
			}
		}
		
		#endregion ContactSearchProperties

		// Methods
		public virtual void RemovePhones()
		{
			foreach (Phone phone in Phones.ToList())
			{
				Communications.Remove(phone);
			}
		}

		public virtual void RemoveEmails()
		{
			foreach (Email email in Emails.ToList())
			{
				Communications.Remove(email);
			}
		}

		public virtual void RemoveFaxes()
		{
			foreach (Fax fax in Faxes.ToList())
			{
				Communications.Remove(fax);
			}
		}

		public virtual void RemoveWebsites()
		{
			foreach (Website website in Websites.ToList())
			{
				Communications.Remove(website);
			}
		}

		public virtual void AddCommunications<TCommunication>(IEnumerable<TCommunication> communications)
			where TCommunication : Communication
		{
			if (communications == null)
			{
				return;
			}

			foreach (TCommunication communication in communications)
			{
				Communications.Add(communication);
			}
		}

		public virtual bool TryRemoveCommunication(Guid communicationId)
		{
			var communication = Communications.FirstOrDefault(c => c.Id == communicationId);
			Communications.Remove(communication);
			return communication != null;
		}

		public virtual List<Phone> GetPhonesFor(Address address)
		{
			return GetPhonesFor(address.Id);
		}

		public virtual List<Phone> GetPhonesFor(Guid addressId)
		{
			return Phones.Where(p => p.AddressId == addressId).ToList();
		}

		public virtual List<Email> GetEmailsFor(Address address)
		{
			return GetEmailsFor(address.Id);
		}

		public virtual List<Email> GetEmailsFor(Guid addressId)
		{
			return Emails.Where(e => e.AddressId == addressId).ToList();
		}

		public virtual List<Fax> GetFaxesFor(Address address)
		{
			return GetFaxesFor(address.Id);
		}

		public virtual List<Fax> GetFaxesFor(Guid addressId)
		{
			return Faxes.Where(e => e.AddressId == addressId).ToList();
		}

		public virtual List<Website> GetWebsitesFor(Address address)
		{
			return GetWebsitesFor(address.Id);
		}

		public virtual List<Website> GetWebsitesFor(Guid addressId)
		{
			return Websites.Where(w => w.AddressId == addressId).ToList();
		}

		public override bool Equals(object obj)
		{
			var other = obj as Contact;
			if (other == null)
			{
				return false;
			}

			if (Name == null && other.Name == null)
			{
				if (ToString() == null && obj.ToString() == null)
				{
					return true;
				}
				if (ToString().Equals(obj.ToString()))
				{
					return true;
				}
			}

			return Equals(other.Id, Id) && String.Equals(other.Name, Name, StringComparison.CurrentCultureIgnoreCase);
		}
		public override int GetHashCode()
		{
			return (String.Format("{0}|{1}", GetType().FullName, Id)).GetHashCode();
		}

		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(LegacyId)
				? Name
				: LegacyName;
		}

		/// <summary>
		/// This is useful when we have a ContactProxy and need to get the real Contact instance.
		/// </summary>
		public virtual Contact Self
		{
			get { return this; }
		}

		// Constructor
		public Contact()
		{
			using (MiniProfiler.Current.Step("Creating an Contact object"))
			{
				IsActive = true;
				Addresses = new List<Address>();
				Communications = new List<Communication>();
				DocumentAttributes = new List<DocumentAttribute>();
				Staff = new List<Person>();
				Notes = new List<Note>();
				Tags = new List<Tag>();
				VisibleToUsergroupIds = new LazyList<Guid>();
				VisibleToUserIds = new LazyList<string>();
				Visibility = Visibility.Everybody;
				ContactType = (this.IsProxy() ? GetType().BaseType : GetType()).Name; //do not use ActualType here, as the entity/proxy is not yet fully initialized
			}
		}
	}
}