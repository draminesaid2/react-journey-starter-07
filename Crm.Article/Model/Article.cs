namespace Crm.Article.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Article.Model.Lookups;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization;
	using Crm.Model;
	using Crm.Model.Interfaces;
	using Crm.Model.Lookups;

	public class Article : Contact, IEntityWithTags
	{
		private Dictionary<string, string> descriptions;

		public virtual string ItemNo { get; set; }
		public virtual string ManufacturerItemNo { get; set; }
		public virtual string Description { get; set; }
		public virtual string ArticleTypeKey { get; set; }
		public virtual decimal? Price { get; set; }
		public virtual string CurrencyKey { get; set; }
		public virtual string QuantityUnitKey { get; set; }
		public virtual bool IsSerial { get; set; }
		public virtual bool SerialRequired { get; set; }
		public virtual decimal? Weight { get; set; }
		public virtual decimal? WeightNet { get; set; }
		public virtual decimal? Length { get; set; }
		public virtual decimal? Width { get; set; }
		public virtual decimal? Height { get; set; }
		public virtual string MatchCode { get; set; }
		public virtual string ArticleGroup01Key { get; set; }
		public virtual string ArticleGroup02Key { get; set; }
		public virtual string ArticleGroup03Key { get; set; }
		public virtual string ArticleGroup04Key { get; set; }
		public virtual string ArticleGroup05Key { get; set; }
		public virtual string VATLevelKey { get; set; }
		public virtual bool IsBatch { get; set; }
		public virtual string Remark { get; set; }
		public virtual bool DangerousGoodsFlag { get; set; }
		public virtual decimal? MinimumStock { get; set; }
		public virtual string BarCode { get; set; }
		public virtual bool IsSparePart { get; set; }
		public virtual bool IsWarehouseManaged { get; set; }
		public virtual bool IsEnabled { get; set; }
		public virtual DateTime? ValidTo { get; set; }
		public virtual DateTime? ValidFrom { get; set; }
		public virtual int? WarrantyInMonths { get; set; }
		public virtual int? GuaranteeInMonths { get; set; }
		public virtual decimal? LeadTimeInDays { get; set; }
		public virtual int DocsCount { get; set; }
		/// <summary>
		/// In an order, the quantity of the article can be incremented/decremented by this number only.
		/// </summary>
		public virtual decimal QuantityStep { get; set; }
		//public virtual int ThumbnailFileResourceId { get; set; }
		public virtual decimal? PurchasePrice { get; set; }
		public virtual Guid? ProductFamilyKey { get; set; }
		public virtual ProductFamily ProductFamily { get; set; }
		// Lookups
		[IgnoreMap]
		public virtual ArticleType ArticleType
		{
			get { return ArticleTypeKey != null ? LookupManager.Get<ArticleType>(ArticleTypeKey) : null; }
		}
		[IgnoreMap]
		public virtual QuantityUnit QuantityUnit
		{
			get { return QuantityUnitKey != null ? LookupManager.Get<QuantityUnit>(QuantityUnitKey) : null; }
		}
		[IgnoreMap]
		public virtual ArticleGroup01 ArticleGroup01
		{
			get { return ArticleGroup01Key != null ? LookupManager.Get<ArticleGroup01>(ArticleGroup01Key) : null; }
		}
		[IgnoreMap]
		public virtual ArticleGroup02 ArticleGroup02
		{
			get { return ArticleGroup02Key != null ? LookupManager.Get<ArticleGroup02>(ArticleGroup02Key) : null; }
		}
		[IgnoreMap]
		public virtual ArticleGroup03 ArticleGroup03
		{
			get { return ArticleGroup03Key != null ? LookupManager.Get<ArticleGroup03>(ArticleGroup03Key) : null; }
		}
		[IgnoreMap]
		public virtual ArticleGroup04 ArticleGroup04
		{
			get { return ArticleGroup04Key != null ? LookupManager.Get<ArticleGroup04>(ArticleGroup04Key) : null; }
		}
		[IgnoreMap]
		public virtual ArticleGroup05 ArticleGroup05
		{
			get { return ArticleGroup05Key != null ? LookupManager.Get<ArticleGroup05>(ArticleGroup05Key) : null; }
		}
		[IgnoreMap]
		public virtual VATLevel VATLevel
		{
			get { return VATLevelKey != null ? LookupManager.Get<VATLevel>(VATLevelKey) : null; }
		}

		[IgnoreMap]
		public virtual Dictionary<string, string> Descriptions
		{
			get { return descriptions ?? (ItemNo != null ? LookupManager.DictByKey<ArticleDescription>(ItemNo, LookupManager.List<Language>().Where(x => x.IsSystemLanguage).Select(x => x.Key)) : new Dictionary<string, string>()); }
			set { descriptions = value; }
		}

		[IgnoreMap]
		public virtual ICollection<ArticleRelationship> ArticleRelationships { get { return ArticleParentRelationships.Union(ArticleChildRelationships).ToList(); } }
		[IgnoreMap]
		public virtual ICollection<ArticleRelationship> ArticleParentRelationships { get; set; }
		[IgnoreMap]
		public virtual ICollection<ArticleRelationship> ArticleChildRelationships { get; set; }
		[IgnoreMap]
		public virtual Currency Currency => CurrencyKey != null ? LookupManager.Instance.Get<Currency>(CurrencyKey) : null;
		// Methods
		public override string ToString()
		{
			var result = string.Empty;
			result += ItemNo.IsNotNullOrEmpty() ? $"{ItemNo} - " : string.Empty;
			result += Description;
			return result;
		}

		// Constructor
		public Article()
		{
			ArticleParentRelationships = new List<ArticleRelationship>();
			ArticleChildRelationships = new List<ArticleRelationship>();
		}
	}
}
