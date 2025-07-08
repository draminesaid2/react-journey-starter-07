namespace Crm.Article.Rest.Model
{
	using System;

	using Crm.Article.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;

	using DocumentAttributeRest = Crm.Rest.Model.DocumentAttributeRest;

	[RestTypeFor(DomainType = typeof(Article))]
	public class ArticleRest : BaseArticleRest
	{
	}
	public abstract class BaseArticleRest : ContactRest
	{
		public string ItemNo { get; set; }
		public string ManufacturerItemNo { get; set; }
		public string BarCode { get; set; }
		public string ArticleTypeKey { get; set; }
		public string QuantityUnitKey { get; set; }
		public string CurrencyKey { get; set; }
		public string VATLevelKey { get; set; }
		public bool IsBatch { get; set; }
		public bool DangerousGoodsFlag { get; set; }
		public bool IsSparePart { get; set; }
		public decimal? Price { get; set; }
		public decimal? PurchasePrice { get; set; }
		public bool IsSerial { get; set; }
		public bool SerialRequired { get; set; }
		public decimal? Weight { get; set; }
		public decimal? WeightNet { get; set; }
		public decimal? Length { get; set; }
		public decimal? Width { get; set; }
		public decimal? Height { get; set; }
		public decimal QuantityStep { get; set; }
		public string ArticleGroup01Key { get; set; }
		public string ArticleGroup02Key { get; set; }
		public string ArticleGroup03Key { get; set; }
		public string ArticleGroup04Key { get; set; }
		public string ArticleGroup05Key { get; set; }
		public string Description { get; set; }
		[NotReceived] [RestrictedField] public bool HasAccessory { get; set; }
		public bool IsWarehouseManaged { get; set; }
		public bool IsEnabled { get; set; }
		public DateTime? ValidTo { get; set; }
		public DateTime? ValidFrom { get; set; }
		public int? WarrantyInMonths { get; set; }
		public int? GuaranteeInMonths { get; set; }
		public decimal? LeadTimeInDays { get; set; }
		[ExplicitExpand, NotReceived] public DocumentAttributeRest[] DocumentAttributes { get; set; }
		[NotReceived, ExplicitExpand] public TagRest[] Tags { get; set; }
		[NotReceived, ExplicitExpand] public ProductFamilyRest ProductFamily { get; set; }
		public Guid? ProductFamilyKey { get; set; }
	}
}
