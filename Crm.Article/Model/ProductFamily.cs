using System.Collections.Generic;

namespace Crm.Article.Model
{
	using AutoMapper;

	using Crm.Model;

	public class ProductFamily : Contact
	{
		private Dictionary<string, string> descriptions;
		public virtual ProductFamily ParentProductFamily { get; set; }
		public virtual string StatusKey { get; set; }
		public virtual ProductFamilyStatus Status
		{
			get { return LookupManager.Get<ProductFamilyStatus>(StatusKey); }
			set { StatusKey = value.Key; }
		}
		public virtual ICollection<ProductFamily> ChildProductFamilies { get; set; }
		[IgnoreMap]
		public virtual Dictionary<string, string> Descriptions
		{
			get { return descriptions; /*?? LookupManager.DictByKey<ProductFamilyDescription>(Id, LookupManager.List<Language>().Where(x => x.IsSystemLanguage).Select(x => x.Key));*/ }
			set { descriptions = value; }
		}
		public ProductFamily()
		{
			ChildProductFamilies = new List<ProductFamily>();
		}
	}
}
