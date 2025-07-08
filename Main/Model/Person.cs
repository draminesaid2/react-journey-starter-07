namespace Crm.Model
{
	using System;
	using System.Text;

	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain;
	using Crm.Library.Model;
	using Crm.Model.Interfaces;
	using Crm.Model.Lookups;

	public class Person : Contact, IMergable, IEntityWithTags
	{
		// Properties
		public virtual string PersonNo { get; set; }
		[DomainSignature]
		public virtual string Firstname { get; set; }
		[DomainSignature]
		public virtual string Surname { get; set; }
		public virtual string DepartmentTypeKey { get; set; }
		public virtual string BusinessTitleKey { get; set; }
		public virtual string TitleKey { get; set; }
		public virtual string SalutationKey { get; set; }
		public virtual string SalutationLetterKey { get; set; }

		[Obsolete("Use Person.Address instead", false)]
		public override Address StandardAddress
		{
			get { return Address; }
			protected set { Address = value; }
		}
		public virtual Address Address { get; set; }
		public virtual Guid StandardAddressKey { get; set; }

		public virtual bool Mima { get; set; }
		public virtual bool IsRetired { get; set; }
		public virtual Title Title
		{
			get { return TitleKey != null ? LookupManager.Get<Title>(TitleKey) : null; }
		}
		public virtual Salutation Salutation
		{
			get { return SalutationKey != null ? LookupManager.Get<Salutation>(SalutationKey) : null; }
		}
		public virtual SalutationLetter SalutationLetter
		{
			get { return SalutationLetterKey != null ? LookupManager.Get<SalutationLetter>(SalutationLetterKey) : null; }
		}
		public virtual DepartmentType DepartmentType
		{
			get { return DepartmentTypeKey != null ? LookupManager.Get<DepartmentType>(DepartmentTypeKey) : null; }
		}
		public virtual BusinessTitle BusinessTitle
		{
			get { return BusinessTitleKey != null ? LookupManager.Get<BusinessTitle>(BusinessTitleKey) : null; }
		}
		public virtual Guid? StationKey { get; set; }
		public virtual Station Station { get; set; }

		// Methods
		public override string ToString()
		{
			var fullName = new StringBuilder();

			if (!String.IsNullOrEmpty(Surname))
			{
				fullName.Append(Surname);
				fullName.Append(", ");
			}

			if (!String.IsNullOrEmpty(Firstname))
			{
				fullName.Append(Firstname);
			}
			else if (fullName.Length >= 2)
			{
				fullName.Remove(fullName.Length - 2, 2);
			}

			if (Title != null && TitleKey != Title.None.Key)
			{
				fullName.Insert(0, " ");
				fullName.Insert(0, Title.Value);
			}

			if (Salutation != null && SalutationKey != Salutation.None.Key)
			{
				fullName.Insert(0, " ");
				fullName.Insert(0, Salutation.Value);
			}

			return fullName.Length > 0 ? fullName.ToString() : String.Empty;
		}
	}
}